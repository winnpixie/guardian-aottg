using System;

namespace Guardian.Utilities
{
    class MathHelper
    {
        private static readonly Random Rng = new Random();

        public static int Abs(int val)
        {
            return val < 0 ? -val : val;
        }

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

        // Min-inclusive, max-exclusive
        public static int RandomInt(int min, int max)
        {
            return Rng.Next(min, max);
        }
    }
}
