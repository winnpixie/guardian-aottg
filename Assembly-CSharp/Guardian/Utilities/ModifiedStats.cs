using System.Collections.Generic;

namespace Guardian.Utilities
{
    class ModifiedStats
    {
        public static readonly byte InfiniteGas = 1;
        public static readonly byte InfiniteBlades = 2;
        public static readonly byte InfiniteAhssAmmo = 4;

        public static List<char> FromInt(int val)
        {
            List<char> stats = new List<char>();

            if ((val & InfiniteGas) == InfiniteGas)
            {
                stats.Add('g');
            }

            if ((val & InfiniteBlades) == InfiniteBlades)
            {
                stats.Add('b');
            }

            if ((val & InfiniteAhssAmmo) == InfiniteAhssAmmo)
            {
                stats.Add('a');
            }

            return stats;
        }

        public static int ToInt()
        {
            int stats = 0;

            /* Legacy code, kept in the event that I do ever bring this back.
            if (Mod.Properties.InfiniteGas.Value)
            {
                stats |= INFINITE_GAS;
            }
            if (Mod.Properties.InfiniteBlades.Value)
            {
                stats |= INFINITE_BLADES;
            }
            if (Mod.Properties.InfiniteAhss.Value)
            {
                stats |= INFINITE_AHSS_AMMO;
            }
            */

            return stats;
        }
    }
}