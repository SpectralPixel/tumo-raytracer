using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class Ray
    {
        public Vector3 position;
        public Vector3 direction;

        public Ray(Vector3 pos, Vector3 targetPoint)
        {
            this.position = pos;
            this.direction = Vector3.Normalize(targetPoint - pos);
        }
    }
}