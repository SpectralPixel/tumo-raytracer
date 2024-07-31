using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class Sphere : IIntersectable
    {
        public Vector3 center { get; set; }
        public Vector3 color { get; set; }

        public float radius;

        public Sphere(Vector3 center, Vector3 color, float radius)
        {
            this.center = center;
            this.color = color;
            this.radius = radius;
        }

        public Vector3 Normal(Vector3 pointOnSurface)
        {
            return (pointOnSurface - center).Normalized();
        }

        public float Intersects(Ray ray)
        {
            Vector3 d = ray.direction;
            Vector3 q = ray.position - center;

            float a = Vector3.Dot(ray.direction, ray.direction); // dot product of a vector with itself => squared length
            float b = 2 * q.X * d.X + 2 * q.Y * d.Y + 2 * q.Z * d.Z;
            float c = Vector3.Dot(q, q) - sq(radius);

            float discriminant = CalculateDiscriminant(a, b, c);

            if (discriminant < 0) { 
                return -1;
            }

            float t1, t2;
            (t1, t2) = Bazooka(a, b, c);

            // Return the smaller number if it is larger than zero
            float t = Math.Min(t1, t2);
            if (t < 0) t = Math.Max(t1, t2);
            
            return t;
        }

        private float CalculateDiscriminant(float a, float b, float c)
        {
            return sq(b) - 4 * a * c;
        }

        // When doing it by hand, there are faster ways to solve quadratics
        // than to use the formula. This is especially important during a test,
        // since you have a limited amount of time to solve each question. If
        // all else fails though, feel free to pull out the bazooka and blow
        // the problem to bits. Works every time.
        private (float, float) Bazooka(float a, float b, float c)
        {
            return Bazooka(a, b, c, CalculateDiscriminant(a, b, c));
        }

        private (float, float) Bazooka(float a, float b, float c, float discriminant)
        {
            return (
                (-b + MathF.Sqrt(discriminant)) / (2 * a),
                (-b - MathF.Sqrt(discriminant)) / (2 * a)
            );
        }

        private int sq(int n)
        {
            return n * n;
        }

        private float sq(float n)
        {
            return n * n;
        }
    }
}