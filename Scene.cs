using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class Scene
    {
        List<IIntersectable> sceneObjects = new List<IIntersectable>();
        List<IIntersectable> inViewObjects = new List<IIntersectable>();
        public Scene()
        {
            sceneObjects.Add(new Plane(0f, new Vector3(0.8f, 0.8f, 0.8f)));
            sceneObjects.Add(new Sphere(new Vector3(5, 1, 0), new Vector3(0, 1, 0), 0.3f));
            sceneObjects.Add(new Sphere(new Vector3(3, 1, 2), new Vector3(0, 1, 1), 0.3f));
            sceneObjects.Add(new Sphere(new Vector3(4, 1, -2), new Vector3(1, 1, 0), 0.2f));
            sceneObjects.Add(new Sphere(new Vector3(4, 0.2f, 1), new Vector3(1, 0.5f, 0), 0.5f));
        }

        public void CullHidden(Camera cam)
        {
            inViewObjects.Clear();
            Console.WriteLine("Cleared!");

            Vector3 camPos = cam.position;
            Vector3 camViewDir = cam.forward;
            float fov = Camera.FOV_RADIANS;
            foreach (IIntersectable obj in sceneObjects)
            {
                bool cull = obj.Cull(camPos, camViewDir, fov);
                if (!cull)
                {
                    inViewObjects.Add(obj);
                    Console.WriteLine($"{obj} not culled!");
                }
            }
        }

        public Intersection FindClosestIntersection(Ray ray)
        {
            Intersection closestIntersection = null;
            foreach (IIntersectable obj in inViewObjects)
            {
                float t = obj.Intersects(ray);
                if (t < 0) continue;

                if (closestIntersection == null)
                {
                    closestIntersection = new Intersection(t, obj);
                }
                else if (t < closestIntersection.t)
                {
                    closestIntersection.t = t;
                    closestIntersection.obj = obj;
                }
            }
            return closestIntersection;

        }
    }
}