namespace raytracer
{
    using System;
    using System.Collections.Generic;
    using OpenTK.Mathematics;

    class RayTracer
    {
        Surface oldSurface;
        Surface surface;
        Game window;
        Camera cam;

        float fov = 90f;

        public RayTracer(Surface surface, Game window)
        {
            this.surface = surface;
            this.window = window;

            cam = new Camera(
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 0f),
                Camera.ConvertScreenDims(surface.width, surface.height),
                fov
            );
        }

        public void Render()
        {          
            if (surface != oldSurface) cam.RecalculateScreenDimensions(Camera.ConvertScreenDims(surface.width, surface.height), fov);

            for (int x = 0; x < surface.width; x++)
            {
                for (int y = 0; y < surface.height; y++)
                {
                    Vector3 color = Vector3.Zero;

                    Ray ray = cam.GetCameraRay(x, y);

                    if (ray.direction.Y < 0) color.X = 1;

                    surface.SetPixel(x, y, color.X, color.Y, color.Z);
                }
            }

            oldSurface = surface;
        }
    }

    class Camera {
        static Vector3 UP_AXIS = Vector3.UnitY;

        Vector2 targetResolution;
        float aspectRatio;

        float fovDegrees;
        float fovRadians;

        Vector3 position;
        Vector3 forward;

        // VP = view plane
        float vpHalfWidth;
        float vpHalfHeight;
        float vpWidth;
        float vpHeight;

        Vector3 tlCorner; // Top-Left
        Vector3 trCorner; // Top-Right
        Vector3 blCorner; // Bottom-Left
        Vector3 brCorner; // Bottom-Right

        // view plane's vectors
        Vector3 up;
        Vector3 right;

        public Camera(Vector3 position, Vector3 rotation, Vector2 targetResolution, float fov)
        {
            this.position = position;
            this.forward = rotation;

            RecalculateScreenDimensions(targetResolution, fov);
        }

        public void RecalculateScreenDimensions(Vector2 targetResolution, float fov)
        {
            this.targetResolution = targetResolution;
            this.aspectRatio = (float)(targetResolution.X / targetResolution.Y);

            this.fovDegrees = fov;
            fovRadians = (float)(fov * Math.PI / 180);

            vpHalfHeight = (float)Math.Tan(fovRadians / 2);
            vpHeight = vpHalfHeight * 2;
            vpWidth = vpHeight * aspectRatio;
            vpHalfWidth = vpWidth / 2;
            
            right = CrossAndNormalize(UP_AXIS, forward);
            up = CrossAndNormalize(forward, right);

            tlCorner = position + forward +  up * vpHalfHeight * -right * vpHalfHeight;
            trCorner = position + forward +  up * vpHalfHeight *  right * vpHalfHeight;
            blCorner = position + forward + -up * vpHalfHeight * -right * vpHalfHeight;
            brCorner = position + forward + -up * vpHalfHeight *  right * vpHalfHeight;

            Console.WriteLine($"targetResolution: {this.targetResolution}");
            Console.WriteLine($"aspectRatio: {this.aspectRatio}");
            Console.WriteLine($"fovDegrees: {this.fovDegrees}");
            Console.WriteLine($"fovRadians: {fovRadians}");
            Console.WriteLine($"vpHalfHeight: {vpHalfHeight}");
            Console.WriteLine($"vpHalfWidth: {vpHalfWidth}");
            Console.WriteLine($"vpHeight: {vpHeight}");
            Console.WriteLine($"vpWidth: {vpWidth}");
            Console.WriteLine($"right: {right}");
            Console.WriteLine($"up: {up}");
            Console.WriteLine($"tlCorner: {tlCorner}");
            Console.WriteLine($"trCorner: {trCorner}");
            Console.WriteLine($"blCorner: {blCorner}");
            Console.WriteLine($"brCorner: {brCorner}");
            Console.WriteLine($"--------");
        }

        public Ray GetCameraRay(int x, int y)
        {
            return GetCameraRay(new Vector2(
                x / targetResolution.X,
                y / targetResolution.Y
            ));
        }

        public Ray GetCameraRay(Vector2 pos)
        {
            if (
                pos.X < 0 ||
                pos.X >= 1 ||
                pos.Y < 0 ||
                pos.Y >= 1
            ) throw new ArgumentException("Parameter must be between 0 and 1 (inclusive, exclusive)", nameof(pos));

            Vector3 rayVector = tlCorner + right * pos.X * vpWidth - up * pos.Y * vpHeight;

            return new Ray(position, rayVector);
        }

        public static Vector2 ConvertScreenDims(int width, int height)
        {
            return new Vector2(width, height);
        }

        private Vector3 CrossAndNormalize(Vector3 vectorA, Vector3 vectorB)
        {
            // Vectors being either equal or set to zero will result in NaN!
            // Therefore we slightly change the vectors if they would break the simulation.
            float nudge = 0.1f;
            Vector3 nudgeVector = new Vector3(nudge, nudge, nudge);
            if (vectorA == Vector3.Zero) vectorA += nudgeVector;
            if (vectorB == Vector3.Zero) vectorB += nudgeVector;
            if (vectorA == vectorB)      vectorB += nudgeVector;
            return Vector3.Cross(vectorA, vectorB).Normalized();
        }
    }

    class Ray {
        public Vector3 position;
        public Vector3 direction;

        public Ray(Vector3 pos, Vector3 dir)
        {
            this.position = pos;
            //this.direction = dir - pos; // WARNING WARNING!!!!!!! ALREADY SUBTRACTING HERE!!!!!!!
            this.direction = dir;
        }
    }
}