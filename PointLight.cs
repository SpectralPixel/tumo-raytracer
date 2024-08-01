using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class PointLight
    {
        public Vector3 position;

        public Vector3 color;
        float luminance;

        public PointLight(Vector3 position, Vector3 color, float luminance)
        {
            this.position = position;
            this.color = color;
            this.luminance = luminance;
        }

        public Vector3 GetColor(float distance)
        {
            float intensity = luminance / (distance * distance);
            return color * intensity;
        }
    }
}