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
        SceneLights lights;

        public RayTracer(Surface surface, Game window)
        {
            this.surface = surface;
            this.window = window;

            this.scene = new Scene();
            this.lights = new SceneLights();

            cam = new Camera(
                new Vector3(0f, 1f, 0f),
                new Vector2(0f, 0f),
                new Vector2i(surface.width, surface.height)
            );
        }

        public void Render()
        {      
            //cam.TurnBy(new Vector2(0.0f, 0.03f));

            scene.CullHidden(cam);

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

                        foreach (PointLight light in lights.sceneLights)
                        {
                            Ray bounceRay = new Ray(intersectionPoint, light.position);
                            bounceRay.position += (bounceRay.direction * 0.05f);
                            Intersection bounceIntersection = scene.FindClosestIntersection(bounceRay);

                            if (bounceIntersection == null)
                            {
                                Vector3 lightColor = light.GetColor(Vector3.Distance(intersectionPoint, light.position));
                                Vector3 directionToLight = (light.position - intersectionPoint).Normalized();

                                float lambert = Math.Clamp(Vector3.Dot(directionToLight, normal), 0f, 1f);
                                color += obj.color * lambert * lightColor;
                            }
                        }
                    }

                    surface.SetPixel(x, y, color);
                }
            }
        }
    }
}