using System;

namespace Guardian.Utilities
{
    class MathHelper
    {
        private static readonly Random random = new Random();

        public static int Ceil(float vIn)
        {
            int vOut = (int)vIn;
            return vOut <= vIn ? vOut + 1 : vOut;
        }

        public static int Floor(float vIn)
        {
            int vOut = (int)vIn;
            return vOut <= vIn ? vOut : vOut - 1;
        }

        public static int Clamp(int val, int min, int max)
        {
            return val < min ? min : val > max ? max : val;
        }

        public static float Clamp(float val, float min, float max)
        {
            return val < min ? min : val > max ? max : val;
        }

        public static float ClampAngle180(float angle)
        {
            angle = (angle + 180f) % 360f;

            if (angle < 0)
            {
                angle += 360f;
            }

            return angle - 180f;
        }

        public static float ClampAngle360(float angle)
        {
            angle %= 360f;

            if (angle < 0)
            {
                angle += 360f;
            }

            return angle;
        }

        // Min-inclusive, max-exclusive
        public static int RandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        // Min-inclusive, max-inclusive-ish
        public static float RandomFloat(float min, float max)
        {
            return min + ((float)random.NextDouble() * (max - min));
        }
    }
}
