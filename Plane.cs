using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class Plane : IIntersectable
    {
        public bool reflective { get; }

        Vector3 color;
        Texture texture;

        float height;

        public Plane(float height, String texImgPath, bool reflective)
        {
            this.height = height;
            this.color = new Vector3(-1f, 0f, 0f);
            this.texture = new Texture(texImgPath);
            this.reflective = reflective;
        }

        public Plane(float height, Vector3 color, bool reflective)
        {
            this.height = height;
            this.color = color;
            this.reflective = reflective;
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

        public Vector3 GetColorAtPixel(Vector3 worldPosition)
        {
            if (color.X >= 0f) return color;
            return texture.GetPixelColorAt(new Vector2(worldPosition.X, worldPosition.Z));
        }
    }
}