using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace raytracer
{
    class Texture
    {
        Image<Rgba32> image;

        public Texture(String filePath)
        {
            image = Image.Load<Rgba32>(filePath);
        }

        public Vector3 GetPixelColorAt(Vector2 pos)
        {
            // Wrap the direction to be between 0 and 1
            while (pos.X < 0f) pos.X += 1f;
            while (pos.Y < 0f) pos.Y += 1f;

            pos.X %= 1f;
            pos.Y %= 1f;

            int pixelX = (int)(pos.X * image.Width);
            int pixelY = (int)(pos.Y * image.Height);

            Rgba32 color = image[pixelX, pixelY];

            return Rgba32ToVector3(color);
        }

        Vector3 Rgba32ToVector3(Rgba32 color)
        {
            return new Vector3(
                color.R / 255f,
                color.G / 255f,
                color.B / 255f
            );
        }
    }
}