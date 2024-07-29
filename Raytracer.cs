namespace raytracer
{
    using System;
    using System.Collections.Generic;
    using OpenTK.Mathematics;

    class RayTracer
    {
        Surface surface;
        Game window;

        public static Vector2i MAX_SCREEN_DIMS = new Vector2i(1920, 1080);

        Circles circles = new Circles();

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
                        
                    float gradientX = x / (float)surface.width;
                    float gradientY = y / (float)surface.height;

                    color += circles.GetColorAtPos(x, y);

                    surface.SetPixel(x, y, color.X, color.Y, color.Z);
                }
            }
        }
    }

    struct Circles {
        List<Circle> circles;

        public Circles()
        {
            this.circles = new List<Circle>();

            Random r = new Random();
            for (int i = 0; i < r.Next(30, 70); i++)
            {
                int x = r.Next(RayTracer.MAX_SCREEN_DIMS.X);
                int y = r.Next(RayTracer.MAX_SCREEN_DIMS.Y);
                float size = (float)r.Next(50, 300);
                bool gradual = r.Next(0, 2) == 0 ? false : true;

                float maximumSolidity;
                if (gradual) maximumSolidity = 1f;
                else maximumSolidity = 0.6f;
                Vector3 color = new Vector3(
                    r.Next(1000) * maximumSolidity / 1000,
                    r.Next(1000) * maximumSolidity / 1000,
                    r.Next(1000) * maximumSolidity / 1000
                );

                circles.Add(new Circle(x, y, size, color, gradual));
            }
        }

        public Vector3 GetColorAtPos(int x, int y)
        {
            Vector3 color = new Vector3();
            foreach (Circle c in circles) {
                color += c.GetCircleColorAt(x, y);
            }
            return color;
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