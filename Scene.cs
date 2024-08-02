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
            sceneObjects.Add(new Plane(0f, "earth.jpg"));
            sceneObjects.Add(new Sphere(new Vector3(5, 1, 0), new Vector3(0, 1, 0), 0.3f));
            sceneObjects.Add(new Sphere(new Vector3(3, 1, 2), new Vector3(0, 1, 1), 0.3f));
            sceneObjects.Add(new Sphere(new Vector3(4, 1, -2), new Vector3(1, 1, 0), 0.2f));
            sceneObjects.Add(new Sphere(new Vector3(4, 0.2f, 1), "fabric.jpg", 0.5f));
        }

        public void CullHidden(Camera cam)
        {
            inViewObjects.Clear();

            Vector3 camPos = cam.position;
            Vector3 camViewDir = cam.forward;
            float fov = Camera.FOV_RADIANS;
            foreach (IIntersectable obj in sceneObjects)
            {
                bool cull = obj.Cull(camPos, camViewDir, fov);
                if (!cull)
                {
                    inViewObjects.Add(obj);
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