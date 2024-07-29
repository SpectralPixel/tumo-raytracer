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

                    int centerX = surface.width / 2;
                    int centerY = surface.height / 2;

                    float avgScreenSize = (surface.width + surface.height) / 2f;
                    float circleSize = avgScreenSize / 5f;

                    Circle circle1 = new Circle(centerX, centerY, 200);
                    Circle circle2 = new Circle(500, 700, 200);

                    float gradientX = x / (float)surface.width;
                    float gradientY = y / (float)surface.height;

                    color.r = circle1.GetCircleLuminanceAt(x, y) + circle2.GetCircleLuminanceAt(x, y);

                    // switch (whatToRun) {
                    //     default: // 0
                    //         if (distFromCenter < circleSize)
                    //         {
                    //             color.r = gradientX;
                    //             color.g = gradientY;
                    //         }
                    //         else
                    //         {
                    //             color.b = gradientX;
                    //             color.r = gradientY;
                    //         }
                    //         break;
                    //     case 1:
                    //         float gradientCircle = 1 - distFromCenter / circleSize;
                    //         color.r = gradientX * gradientCircle;
                    //         color.g = gradientY * gradientCircle;
                    //         break;
                    // }

                    surface.SetPixel(x, y, color.r, color.g, color.b);
                }
            }
        }
    }

    struct Circle {
        int centerX;
        int centerY;
        float radius;

        public Circle(int x, int y, float radius)
        {
            this.centerX = x;
            this.centerY = y;
            this.radius = radius;
        }

        public float GetCircleLuminanceAt(int x, int y)
        {
            int distanceX = x - centerX;
            int distanceY = y - centerY;
            float distanceFromCenter = MathF.Sqrt(
                distanceX * distanceX +
                distanceY * distanceY
            ); // pythagoras

            // scale circle up by diving it by the circle radius
            // "change from position space to color space"
            // set an extreme to a gradient by normalizing it
            float circleGradient = distanceFromCenter / radius;

            // now invert the colors for a black background
            // remember, the maximum luminance will always be 1
            float circleLuminance = 1 - circleGradient;
            return circleLuminance;
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