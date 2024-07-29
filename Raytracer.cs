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
                    Vector3 color = new Vector3(0f, 0f, 0f);

                    int centerX = surface.width / 2;
                    int centerY = surface.height / 2;

                    float avgScreenSize = (surface.width + surface.height) / 2f;
                    float circleSize = avgScreenSize / 5f;

                    Circle circle1 = new Circle(
                        centerX,
                        centerY,
                        200,
                        new Vector3(1f, 0f, 0f),
                        false
                    );
                    Circle circle2 = new Circle(
                        250,
                        400,
                        150,
                        new Vector3(0f, 1f, 0f),
                        true
                    );

                    float gradientX = x / (float)surface.width;
                    float gradientY = y / (float)surface.height;

                    color += circle1.GetCircleColorAt(x, y);
                    color += circle2.GetCircleColorAt(x, y);

                    surface.SetPixel(x, y, color.X, color.Y, color.Z);
                }
            }
        }
    }

    struct Circle {
        int centerX;
        int centerY;
        float radius;
        Vector3 color;
        bool gradual;

        public Circle(int x, int y, float radius, Vector3 color, bool gradual)
        {
            this.centerX = x;
            this.centerY = y;
            this.radius = radius;
            this.color = color;
            this.gradual = gradual;
        }

        public Vector3 GetCircleColorAt(int x, int y)
        {
            return color * GetCircleLuminanceAt(x, y);
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
}