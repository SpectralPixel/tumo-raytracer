using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class Plane : IIntersectable
    {
        float height;

        public Vector3 color { get; set; }

        public Plane(float height, Vector3 color)
        {
            this.height = height;
            this.color = color;
        }

        public float Intersects(Ray ray)
        {
            float t = (ray.position.Y - height) / -ray.direction.Y;
            return t;
        }
    }
}