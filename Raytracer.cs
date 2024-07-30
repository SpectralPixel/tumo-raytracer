namespace raytracer
{
    using System;
    using System.Collections.Generic;
    using OpenTK.Mathematics;

    class RayTracer
    {
        Surface surface;
        Game window;

        public RayTracer(Surface surface, Game window)
        {
            this.surface = surface;
            this.window = window;
        }

        public void Render()
        {
            for (int x = 0; x < surface.width; x++)
            {
                for (int y = 0; y < surface.height; y++)
                {
                    surface.SetPixel(x, y, 1f, 1f, 1f);
                }
            }
        }
    }
}