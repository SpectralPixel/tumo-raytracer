using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class PointLight
    {
        public Vector3 position;
        public float radius;

        public Vector3 color;
        float luminance;

        Random rng = new System.Random();

        public PointLight(Vector3 position, Vector3 color, float radius, float luminance)
        {
            this.position = position;
            this.color = color;
            this.radius = radius;
            this.luminance = luminance;
        }

        public Vector3 GetColor(float distance)
        {
            float intensity = luminance / (distance * distance);
            return color * intensity;
        }

        public Vector3 GetPointInside()
        {
            Vector3 nudge;
            do
            {
                nudge = new Vector3(
                    (float)rng.NextDouble() * radius,
                    (float)rng.NextDouble() * radius,
                    (float)rng.NextDouble() * radius
                );
            }
            while (
                nudge.Normalized().X > nudge.X / radius &&
                nudge.Normalized().Y > nudge.Y / radius &&
                nudge.Normalized().Z > nudge.Z / radius
            );

            return position + nudge;
        }
    }
}