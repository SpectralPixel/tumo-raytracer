using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
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
            //cam.MoveBy(new Vector3(0.1f, 0f, 0f));

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
}