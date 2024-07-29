namespace raytracer
{
    using System;

    class RayTracer
    {
        Surface surface;
        Game window;

        float whatToRun = 1;

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
                    Color color = new Color(0f, 0f, 0f);

                    float gradientX = x / (float)surface.width;
                    float gradientY = y / (float)surface.height;

                    int centerX = surface.width / 2;
                    int centerY = surface.height / 2;

                    int distX = Math.Abs(x - centerX);
                    int distY = Math.Abs(y - centerY);
                    float distFromCenter = MathF.Sqrt(distX * distX + distY * distY); // pythagoras

                    float avgScreenSize = (surface.width + surface.height) / 2f;
                    float circleSize = avgScreenSize / 5f;

                    switch (whatToRun) {
                        default: // 0
                            if (distFromCenter < circleSize)
                            {
                                color.r = gradientX;
                                color.g = gradientY;
                            }
                            else
                            {
                                color.b = gradientX;
                                color.r = gradientY;
                            }
                            break;
                        case 1:
                            float gradientCircle = 1 - distFromCenter / circleSize;
                            color.r = gradientX * gradientCircle;
                            color.g = gradientY * gradientCircle;
                            break;
                    }

                    surface.SetPixel(x, y, color.r, color.g, color.b);
                }
            }
        }

    }

    struct Color {
        public float r { get; set; }
        public float g { get; set; }
        public float b { get; set; }
        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}