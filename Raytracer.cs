namespace raytracer
{
    using System;
    using System.Collections.Generic;
    using OpenTK.Mathematics;

    class RayTracer
    {
        Surface surface;
        Game window;

        //Console.WriteLine("god i love how easy this function is to use");

        Camera cam = new Camera(
            new Vector2(160f, 90f),
            70f,
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 0f)
        );

        public RayTracer(Surface surface, Game window)
        {
            this.surface = surface;
            this.window = window;
        }

        public void Render()
        {
            for (int x = 0; x < surface.width; x++)
            {
                for (int y = 0; y < surface.height; y++)
                {
                    surface.SetPixel(x, y, 1f, 1f, 1f);
                }
            }
        }
    }

    class Camera {
        static Vector3 UP_AXIS = Vector3.UnitY;

        float aspectRatio;
        float fovDegrees;
        float fovRadians;
        Vector3 position;
        Vector3 forward; // forward vector

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

        public Camera(Vector2d targetResolution, float fov, Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.forward = rotation;
            
            this.aspectRatio = (float)(targetResolution.X / targetResolution.Y);

            this.fovDegrees = fov;
            fovRadians = (float)(fov * Math.PI / 180);

            vpHalfHeight = (float)Math.Tan(fovRadians / 2);
            vpHeight = vpHalfHeight * 2;
            vpWidth = vpHeight * aspectRatio;
            vpHalfWidth = vpWidth / 2;

            right = Vector3.Cross(UP_AXIS, forward).Normalized();
            up = Vector3.Cross(forward, forward).Normalized();

            tlCorner = position + forward +  up * vpHalfHeight * -right * vpHalfHeight;
            trCorner = position + forward +  up * vpHalfHeight *  right * vpHalfHeight;
            blCorner = position + forward + -up * vpHalfHeight * -right * vpHalfHeight;
            brCorner = position + forward + -up * vpHalfHeight *  right * vpHalfHeight;
        }
    }
}