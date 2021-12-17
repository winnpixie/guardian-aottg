using System;

namespace Guardian.Utilities
{
    class MathHelper
    {
        private static readonly Random random = new Random();

        public static int Abs(int val)
        {
            return val < 0 ? -val : val;
        }

        public static int Floor(float val)
        {
            int n = (int)val;
            return n <= val ? n : n - 1;
        }

        public static int Ceil(float val)
        {
            int n = (int)val;
            return n >= val ? n : n + 1;
        }

        public static int Clamp(int val, int min, int max)
        {
            return val < min ? min : val > max ? max : val;
        }

        // Min-inclusive, max-exclusive
        public static int RandomInt(int min, int max)
        {
            // Swap min and max values if min > max
            if (min > max)
            {
                min += max;
                max = min - max;
                min -= max;
            }

            return random.Next(min, max);
        }

        // Thank you, https://stackoverflow.com/a/45859570
        public static int NextPowerOf2(int val)
        {
            int n = val;

            n--;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            n++;

            return n;
        }

        // Yeah, I took this from RCextensions
        public static bool IsPowerOf2(int val)
        {
            return (val & (val - 1)) == 0;
        }
    }
}
