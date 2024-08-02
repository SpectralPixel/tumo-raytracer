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
        Texture backgroundTexture;

        int frames;
        Vector3[,] accumulationBuffer;

        public RayTracer(Surface surface, Game window)
        {
            this.surface = surface;
            this.window = window;

            this.scene = new Scene();
            this.lights = new SceneLights();

            this.backgroundTexture = new Texture("sky.jpg");

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
            //cam.MoveBy(new Vector3(0.01f, 0f, 0f));

            scene.CullHidden(cam);

            // if it's the first frame, generate a low quality image first
            int resolutionDropoff = frames == 1 ? 4 : 1;
            Raytrace(resolutionDropoff);

            frames++;
        }

        void Raytrace(int resolutionDropoff = 1)
        {
            for (int x = 0; x < surface.width; x += resolutionDropoff)
            {
                for (int y = 0; y < surface.height; y += resolutionDropoff)
                {
                    Vector3 color = GetColorAtPixel(x, y, resolutionDropoff != 1);
                    
                    for (int nudgeX = 0; nudgeX < resolutionDropoff; nudgeX++)
                    {
                        for (int nudgeY = 0; nudgeY < resolutionDropoff; nudgeY++)
                        {
                            int newX = x + nudgeX;
                            int newY = y + nudgeY;
                            accumulationBuffer[newX, newY] += color;
                            surface.SetPixel(newX, newY, accumulationBuffer[x, y] / frames);
                        }
                    }
                }
            }
        }

        Vector3 GetColorAtPixel(int x, int y, bool pixelPerfect)
        {
            Vector3 color = Vector3.Zero;

            Ray ray;
            if (pixelPerfect) ray = cam.GetCameraRay(x, y);
            else ray = cam.GetRandomCameraRay(x, y);

            Intersection intersection = scene.FindClosestIntersection(ray);
            if (intersection == null) return GetBackgroundColor(ray.position + ray.direction * 1000f);

            IIntersectable obj = intersection.obj;
            Vector3 intersectionPoint = ray.position + ray.direction * intersection.t;

            Vector3 normal = obj.Normal(intersectionPoint);

            foreach (PointLight light in lights.sceneLights)
            {
                Vector3 lightPosition;
                if (pixelPerfect) lightPosition = light.position;
                else lightPosition = light.GetPointInside();

                Ray bounceRay = new Ray(intersectionPoint, lightPosition);
                bounceRay.position += (bounceRay.direction * 0.05f);
                Intersection bounceIntersection = scene.FindClosestIntersection(bounceRay);

                if (bounceIntersection != null)
                {
                    accumulationBuffer[x, y] += Vector3.Zero;
                    continue;
                }

                Vector3 objColor = obj.GetColorAtPixel(intersectionPoint);

                Vector3 lightColor = light.GetColor(Vector3.Distance(intersectionPoint, lightPosition));
                Vector3 directionToLight = (lightPosition - intersectionPoint).Normalized();

                float lambert = Math.Clamp(Vector3.Dot(directionToLight, normal), 0f, 1f);
                color += objColor * lambert * lightColor;
            }

            return color;
        }

        Vector3 GetBackgroundColor(Vector3 hitPosition)
        {
            Vector2 direction = Sphere.GetDirectionFromPositions(hitPosition, cam.position);

            return backgroundTexture.GetPixelColorAt(direction);
        }
    }
}