using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class Scene
    {
        List<Sphere> sceneObjects = new List<Sphere>();
        public Scene()
        {
            sceneObjects.Add(new Sphere(new Vector3(5, 1, 0), new Vector3(0, 1, 0), 0.3f));
            sceneObjects.Add(new Sphere(new Vector3(3, 1, 2), new Vector3(0, 1, 1), 0.3f));
            sceneObjects.Add(new Sphere(new Vector3(4, 1, -2), new Vector3(1, 1, 0), 0.2f));
            sceneObjects.Add(new Sphere(new Vector3(4, 0.2f, 1), new Vector3(1, 0.5f, 0), 0.5f));
        }

        public Intersection FindClosestIntersection(Ray ray)
        {
            Intersection closestIntersection = null;
            foreach (Sphere sphere in sceneObjects)
            {
                float t = sphere.Intersects(ray);
                if (t < 0) continue;

                if (closestIntersection == null)
                {
                    closestIntersection = new Intersection(t, sphere);
                }
                else if (t < closestIntersection.t)
                {
                    closestIntersection.t = t;
                    closestIntersection.sphere = sphere;
                }
            }
            return closestIntersection;

        }
    }
}