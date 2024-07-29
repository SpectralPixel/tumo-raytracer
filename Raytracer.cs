namespace raytracer
{
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
                    surface.SetPixel(x, y, 0.6f, 1.0f, 0.2f);
                }
            }
        }

    }
}