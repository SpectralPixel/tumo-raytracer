using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class Camera {
        static Vector3 UP_AXIS = Vector3.UnitY;

        const float FOV_DEGREES = 70f;
        public const float FOV_RADIANS = (float)(FOV_DEGREES * Math.PI / 180f);

        public Vector3 position;
        public Vector3 forward;
        
        // X = yaw, left/right
        // Y = pitch, up/down
        Vector2 rotation;

        Vector2i targetResolution;
        float aspectRatio;

        // VP = view plane
        float vpHalfWidth;
        float vpHalfHeight;
        float vpWidth;
        float vpHeight;

        Vector3 tlCorner; // Top-Left
        Vector3 trCorner; // Top-Right
        Vector3 blCorner; // Bottom-Left
        Vector3 brCorner; // Bottom-Right

        // view plane's vectors
        Vector3 up;
        Vector3 right;

        public Camera(Vector3 pos, Vector2 rotation, Vector2i targetResolution)
        {
            SetCameraNewTransform(pos, rotation);
            RecalculateScreenDimensions(targetResolution);
        }

        void ValidateCameraTransform(Vector3 pos, Vector2 rotation)
        {
            SetCameraNewTransform(pos, rotation);
            CalculateVectors();
        }

        void SetCameraNewTransform(Vector3 pos, Vector2 rotation)
        {
            position = pos;
            forward = CalculateForwardVector(rotation);
        }

        Vector3 CalculateForwardVector(Vector2 rotation)
        {
            this.rotation = rotation;

            float xzLen = (float)Math.Cos(rotation.Y);

            return new Vector3(
                (float)Math.Cos(rotation.X) * xzLen,
                (float)Math.Sin(rotation.Y),
                (float)Math.Sin(-rotation.X) * xzLen
            ).Normalized();
        }

        public void RecalculateScreenDimensions(Vector2i targetRes)
        {
            targetResolution = ConvertScreenDims(targetRes);
            aspectRatio = targetResolution.X / (float)targetResolution.Y;

            vpHalfHeight = (float)Math.Tan(FOV_RADIANS / 2);
            vpHeight = vpHalfHeight * 2;
            vpWidth = vpHeight * aspectRatio;
            vpHalfWidth = vpWidth / 2;
            
            CalculateVectors();
        }

        void CalculateVectors()
        {
            // if the camera is upside-down, multiply the universal up vector by -1 to allow for a flipped view
            Vector3 upAxis = UP_AXIS;
            upAxis.Y = (float)Math.Sign(Math.Cos(rotation.Y));

            Console.WriteLine($"Up Axis: {upAxis}");

            right = CrossAndNormalize(upAxis, forward);
            up = CrossAndNormalize(forward, right);

            tlCorner = position + forward +  up * vpHalfHeight + -right * vpHalfWidth;
            trCorner = position + forward +  up * vpHalfHeight +  right * vpHalfWidth;
            blCorner = position + forward + -up * vpHalfHeight + -right * vpHalfWidth;
            brCorner = position + forward + -up * vpHalfHeight +  right * vpHalfWidth;
        }

        public Ray GetCameraRay(int x, int y)
        {
            return GetCameraRay(new Vector2(
                (x + 0.5f) / targetResolution.X,
                (y + 0.5f) / targetResolution.Y
            ));
        }

        public Ray GetCameraRay(Vector2 pos)
        {
            if (pos.X < 0 || pos.X >= 1 || pos.Y < 0 || pos.Y >= 1)
            {
                Console.WriteLine($"{targetResolution} | {pos}");
                throw new ArgumentException("Parameter must be between 0 and 1 (inclusive, exclusive)", nameof(pos));
            }

            Vector3 rayVector = tlCorner + right * pos.X * vpWidth - up * pos.Y * vpHeight;

            return new Ray(position, rayVector);
        }

        private Vector2i ConvertScreenDims(Vector2i dims)
        {
            return new Vector2i(dims.X, dims.Y);
        }

        private Vector3 CrossAndNormalize(Vector3 vectorA, Vector3 vectorB)
        {
            // Several conditions cause the crossing of vectors to result in NaN!
            // Therefore we slightly change the vectors if they would break the simulation.
            float nudge = 0.1f;
            Vector3 nudgeVector = new Vector3(nudge, nudge, nudge);

            // Either vector cannot be equal to zero
            if (vectorA == Vector3.Zero) vectorA += nudgeVector;
            if (vectorB == Vector3.Zero) vectorB += nudgeVector;

            // You can't normalize a zero vector
            vectorA = vectorA.Normalized();
            vectorB = vectorB.Normalized();

            // The vectors cannot point in the same direction
            float dot = Vector3.Dot(vectorA, vectorB);
            if (dot >= 0.95f) vectorB += nudgeVector;

            Vector3 crossed = Vector3.Cross(vectorA, vectorB).Normalized();

            if (float.IsNaN(crossed.X)) throw new InvalidOperationException($"Crossed vector resulted in NaN! | VectorA: {vectorA} | VectorB: {vectorB}");

            return crossed;
        }

        public void MoveBy(Vector3 translation)
        {
            ValidateCameraTransform(
                position + translation,
                rotation
            );
        }

        public void TurnBy(Vector2 rotAmt)
        {
            rotation += rotAmt;

            float tau = (float)Math.Tau;

            // Wrap the direction to be between 0 and 360
            while (rotation.X < 0f) rotation.X += tau;
            while (rotation.Y < 0f) rotation.Y += tau;

            rotation.X %= tau;
            rotation.Y %= tau;

            Console.WriteLine($"View Direction: {rotation}");

            ValidateCameraTransform(
                position,
                rotation
            );
        }
    }
}