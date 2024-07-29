namespace raytracer
{
    using System;
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
                    Color color = new Color(0f, 0f, 0f);

                    int centerX = surface.width / 2;
                    int centerY = surface.height / 2;

                    float avgScreenSize = (surface.width + surface.height) / 2f;
                    float circleSize = avgScreenSize / 5f;

                    Circle circle1 = new Circle(centerX, centerY, 200, false);
                    Circle circle2 = new Circle(500, 700, 200, true);

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
        bool gradual;

        public Circle(int x, int y, float radius, bool gradual)
        {
            this.centerX = x;
            this.centerY = y;
            this.radius = radius;
            this.gradual = gradual;
        }

        public float GetCircleLuminanceAt(int x, int y)
        {
            float distance = calculateDistanceFromCenter(x, y);
            float circleLuminance;

            switch (gradual) {
                case false:
                    // scale circle up by diving it by the circle radius
                    // "change from position space to color space"
                    // set an extreme to a gradient by normalizing it
                    float circleGradient = distance / radius;

                    // now invert the colors for a black background
                    // remember, the maximum luminance will always be 1
                    // this also has to be clamped to prevent one circle from interfering with other pixels by subtracting values that should be zero
                    circleLuminance = Math.Clamp(1 - circleGradient, 0, 1);
                    break;
                case true:
                    circleLuminance = distance < radius ? 1 : 0;
                    break;
            }
    

            return circleLuminance;
        }

        private float calculateDistanceFromCenter(int otherX, int otherY)
        {
            int distanceX = otherX - centerX;
            int distanceY = otherY - centerY;
            // pythagoras
            float distanceFromCenter = MathF.Sqrt(
                distanceX * distanceX +
                distanceY * distanceY
            );
            return distanceFromCenter;
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