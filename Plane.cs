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

        public bool Cull(Vector3 cameraPosition, Vector3 cameraViewDir, float fovRadians)
        {
            Vector3 floorVector = cameraViewDir;
            floorVector.Y = 0f;
            floorVector = floorVector.Normalized();

            float angle = (float)Math.Acos(Vector3.Dot(cameraViewDir, floorVector));

            if (cameraPosition.Y > height && cameraViewDir.Y < 0) return false;
            if (cameraPosition.Y < height && cameraViewDir.Y > 0) return false;

            return angle > fovRadians;
        }

        public float Intersects(Ray ray)
        {
            float t = (ray.position.Y - height) / -ray.direction.Y;
            return t;
        }
    }
}