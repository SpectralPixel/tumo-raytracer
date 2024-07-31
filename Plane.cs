using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class Plane : IIntersectable
    {
        public Vector3 color { get; set; }

        float height;

        public Plane(float height, Vector3 color)
        {
            this.height = height;
            this.color = color;
        }

        public Vector3 Normal(Vector3 pointOnSurface)
        {
            return Vector3.UnitY;
        }

        public float Intersects(Ray ray)
        {
            float t = (ray.position.Y - height) / -ray.direction.Y;
            return t;
        }
    }
}