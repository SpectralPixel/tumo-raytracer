using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class Intersection
    {
        public float t; // remember the formula?
        public Sphere sphere;
        
        public Intersection(float t, Sphere sphere)
        {
            this.t = t;
            this.sphere = sphere;
        }
    }
}