using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class Light
    {
        public Vector3 direction;
        public Vector3 color;

        public Light(Vector3 direction, Vector3 color, float luminance)
        {
            this.direction = direction.Normalized();
            this.color = color * luminance;
        }
    }
}