namespace raytracer
{
    using System;
    using System.Collections.Generic;
    using OpenTK.Mathematics;

    class RayTracer
    {
        Surface surface;
        Game window;
        public Camera cam;

        public RayTracer(Surface surface, Game window)
        {
            this.surface = surface;
            this.window = window;

            cam = new Camera(
                new Vector3(0f, 3f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector2i(surface.width, surface.height)
            );
        }

        public void Render()
        {      
            cam.MoveBy(new Vector3(0.1f, 0f, 0f));

            for (int x = 0; x < surface.width; x++)
            {
                for (int y = 0; y < surface.height; y++)
                {
                    Vector3 color = Vector3.Zero;

                    Ray ray = cam.GetCameraRay(x, y);

                    if (ray.direction.Y < 0)
                    {
                        float distanceToFloor = ray.position.Y / -ray.direction.Y;
                        Vector3 intersectionPoint = ray.position + distanceToFloor * ray.direction;

                        color.Y += (MathF.Sin(intersectionPoint.X) + 1f) / 2f;
                        color.Z += (MathF.Sin(intersectionPoint.Z) + 1f) / 2f;
                    }
                    else color.X = 1;

                    surface.SetPixel(x, y, color);
                }
            }
        }
    }

    class Camera {
        static Vector3 UP_AXIS = Vector3.UnitY;

        const float FOV_DEGREES = 90f;
        const float FOV_RADIANS = (float)(FOV_DEGREES * Math.PI / 180f);

        Vector2i targetResolution;
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

        public Camera(Vector3 pos, Vector3 rotation, Vector2i targetResolution)
        {
            position = pos;
            forward = rotation;

            RecalculateScreenDimensions(targetResolution);
        }

        public void SetCameraTransform(Vector3 pos, Vector3 rotation)
        {
            position = pos;
            forward = rotation;

            CalculateVectors();
        }

        public void RecalculateScreenDimensions(Vector2i targetRes)
        {
            targetResolution = ConvertScreenDims(targetRes);
            aspectRatio = (float)(targetResolution.X / targetResolution.Y);

            vpHalfHeight = (float)Math.Tan(FOV_RADIANS / 2);
            vpHeight = vpHalfHeight * 2;
            vpWidth = vpHeight * aspectRatio;
            vpHalfWidth = vpWidth / 2;
            
            CalculateVectors();
        }

        public void CalculateVectors()
        {
            right = CrossAndNormalize(UP_AXIS, forward);
            up = CrossAndNormalize(forward, right);

            tlCorner = position + forward +  up * vpHalfHeight + -right * vpHalfWidth;
            trCorner = position + forward +  up * vpHalfHeight +  right * vpHalfWidth;
            blCorner = position + forward + -up * vpHalfHeight + -right * vpHalfWidth;
            brCorner = position + forward + -up * vpHalfHeight +  right * vpHalfWidth;
        }

        public Ray GetCameraRay(int x, int y)
        {
            return GetCameraRay(new Vector2(
                (x + 0.5f) / targetResolution.X,
                (y + 0.5f) / targetResolution.Y
            ));
        }

        public Ray GetCameraRay(Vector2 pos)
        {
            if (pos.X < 0 || pos.X >= 1 || pos.Y < 0 || pos.Y >= 1)
            {
                Console.WriteLine($"{targetResolution} | {pos}");
                throw new ArgumentException("Parameter must be between 0 and 1 (inclusive, exclusive)", nameof(pos));
            }

            Vector3 rayVector = tlCorner + right * pos.X * vpWidth - up * pos.Y * vpHeight;

            return new Ray(position, rayVector);
        }

        private Vector2i ConvertScreenDims(Vector2i dims)
        {
            return new Vector2i(dims.X, dims.Y);
        }

        private Vector3 CrossAndNormalize(Vector3 vectorA, Vector3 vectorB)
        {
            // Several conditions cause the crossing of vectors to result in NaN!
            // Therefore we slightly change the vectors if they would break the simulation.
            float nudge = 0.1f;
            Vector3 nudgeVector = new Vector3(nudge, nudge, nudge);

            // Either vector cannot be equal to zero
            if (vectorA == Vector3.Zero) vectorA += nudgeVector;
            if (vectorB == Vector3.Zero) vectorB += nudgeVector;

            vectorA = vectorA.Normalized();
            vectorB = vectorB.Normalized();

            // The vectors cannot point in the same direction
            float dot = Vector3.Dot(vectorA, vectorB);
            if (dot >= 0.95f) vectorB += nudgeVector;

            Vector3 crossed = Vector3.Cross(vectorA, vectorB).Normalized();

            if (float.IsNaN(crossed.X)) throw new InvalidOperationException($"Crossed vector resulted in NaN! | VectorA: {vectorA} | VectorB: {vectorB}");

            return crossed;
        }

        public void MoveBy(Vector3 translation)
        {
            SetCameraTransform(
                position + translation,
                forward
            );
        }

        public void TurnBy(Vector3 rotation)
        {
            SetCameraTransform(
                position,
                forward + rotation
            );
        }
    }

    class Ray {
        public Vector3 position;
        public Vector3 direction;

        public Ray(Vector3 pos, Vector3 targetPoint)
        {
            this.position = pos;
            this.direction = Vector3.Normalize(targetPoint - pos); // WARNING WARNING!!!!!!! ALREADY SUBTRACTING HERE!!!!!!!
        }
    }
}