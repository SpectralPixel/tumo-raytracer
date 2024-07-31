using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class RayTracer
    {
        public Camera cam;

        Surface surface;
        Game window;
        Scene scene;
        Light light;

        public RayTracer(Surface surface, Game window)
        {
            this.surface = surface;
            this.window = window;

            this.scene = new Scene();
            this.light = new Light(
                new Vector3(-1f, 1f, 0f),
                new Vector3(1f, 1f, 1f),
                1.4f
            );

            cam = new Camera(
                new Vector3(0f, 1f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector2i(surface.width, surface.height)
            );
        }

        public void Render()
        {      
            cam.MoveBy(new Vector3(-0.1f, 0f, 0f));

            for (int x = 0; x < surface.width; x++)
            {
                for (int y = 0; y < surface.height; y++)
                {
                    Vector3 color = Vector3.Zero;

                    Ray ray = cam.GetCameraRay(x, y);

                    Intersection intersection = scene.FindClosestIntersection(ray);
                    if (intersection != null)
                    {
                        IIntersectable obj = intersection.obj;
                        Vector3 intersectionPoint = ray.position + ray.direction * intersection.t;
                        Vector3 normal = obj.Normal(intersectionPoint);
                        float lambert = Math.Clamp(Vector3.Dot(light.direction, normal), 0f, 1f);
                        color = obj.color * lambert * light.color;
                    }

                    surface.SetPixel(x, y, color);
                }
            }
        }
    }
}