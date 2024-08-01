using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class RayTracer
    {
        const int RAYS_PER_PIXEL = 16;

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

            Vector3[,] accumulationBuffer = new Vector3[surface.width, surface.height];

            scene.CullHidden(cam);

            for (int x = 0; x < surface.width; x++)
            {
                for (int y = 0; y < surface.height; y++)
                {
                    for (int pixelRayIndex = 0; pixelRayIndex < RAYS_PER_PIXEL; pixelRayIndex++)
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
                                Vector3 lightPosition = light.GetPointInside();

                                Ray bounceRay = new Ray(intersectionPoint, lightPosition);
                                bounceRay.position += (bounceRay.direction * 0.05f);
                                Intersection bounceIntersection = scene.FindClosestIntersection(bounceRay);

                                if (bounceIntersection == null)
                                {
                                    Vector3 lightColor = light.GetColor(Vector3.Distance(intersectionPoint, lightPosition));
                                    Vector3 directionToLight = (lightPosition - intersectionPoint).Normalized();

                                    float lambert = Math.Clamp(Vector3.Dot(directionToLight, normal), 0f, 1f);
                                    color += obj.color * lambert * lightColor;
                                }
                            }

                            accumulationBuffer[x, y] += color;
                        }
                    }

                    Vector3 finalColor = accumulationBuffer[x, y] / RAYS_PER_PIXEL;
                    surface.SetPixel(x, y, finalColor);
                }
            }
        }
    }
}