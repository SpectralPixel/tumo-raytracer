using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    interface IIntersectable
    {
        Vector3 color { get; set; }

        Vector3 Normal(Vector3 pointOnSurface);
        bool Cull(Vector3 cameraPosition, Vector3 cameraViewDir, float fovRadians);
        float Intersects(Ray ray);
    }

    class Intersection 
    {
        public float t; // remember the formula?
        public IIntersectable obj;
        
        public Intersection(float t, IIntersectable obj)
        {
            this.t = t;
            this.obj = obj;
        }
    }
}