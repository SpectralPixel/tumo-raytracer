using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class RayTracer
    {
        const int RAYS_PER_PIXEL = 1;

        public Camera cam;

        Surface surface;
        Game window;

        Scene scene;
        SceneLights lights;

        int frames;
        Vector3[,] accumulationBuffer;

        public RayTracer(Surface surface, Game window)
        {
            this.surface = surface;
            this.window = window;

            this.scene = new Scene();
            this.lights = new SceneLights();

            cam = new Camera(
                this,
                new Vector3(0f, 1f, 0f),
                new Vector2(0f, 0f),
                new Vector2i(surface.width, surface.height)
            );

            ResetAccumulationBuffer();
        }

        public void ResetAccumulationBuffer()
        {
            frames = 1;
            accumulationBuffer = new Vector3[surface.width, surface.height];
        }

        public void Render()
        {
            // cam.MoveBy(new Vector3(0.01f, 0f, 0f));

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
                        if (intersection == null)
                        {
                            accumulationBuffer[x, y] += Vector3.Zero;
                            continue;
                        }

                        IIntersectable obj = intersection.obj;
                        Vector3 intersectionPoint = ray.position + ray.direction * intersection.t;

                        Vector3 normal = obj.Normal(intersectionPoint);

                        foreach (PointLight light in lights.sceneLights)
                        {
                            Vector3 lightPosition = light.GetPointInside();

                            Ray bounceRay = new Ray(intersectionPoint, lightPosition);
                            bounceRay.position += (bounceRay.direction * 0.05f);
                            Intersection bounceIntersection = scene.FindClosestIntersection(bounceRay);

                            if (bounceIntersection != null)
                            {
                                accumulationBuffer[x, y] += Vector3.Zero;
                                continue;
                            }

                            Vector3 lightColor = light.GetColor(Vector3.Distance(intersectionPoint, lightPosition));
                            Vector3 directionToLight = (lightPosition - intersectionPoint).Normalized();

                            float lambert = Math.Clamp(Vector3.Dot(directionToLight, normal), 0f, 1f);
                            color += obj.color * lambert * lightColor;
                        }

                        accumulationBuffer[x, y] += color;
                    }

                    Vector3 finalColor = accumulationBuffer[x, y] / (frames * RAYS_PER_PIXEL);
                    surface.SetPixel(x, y, finalColor);
                }
            }

            frames++;
        }
    }
}