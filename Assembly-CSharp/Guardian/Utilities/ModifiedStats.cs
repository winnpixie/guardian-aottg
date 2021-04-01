using System.Collections.Generic;

namespace Guardian.Utilities
{
    class ModifiedStats
    {
        public static readonly byte INFINITE_GAS = 1;
        public static readonly byte INFINITE_BLADES = 2;
        public static readonly byte INFINITE_AHSS_AMMO = 4;

        public static List<char> FromInt(int val)
        {
            List<char> stats = new List<char>();

            if ((val & INFINITE_GAS) == INFINITE_GAS)
            {
                stats.Add('g');
            }
            if ((val & INFINITE_BLADES) == INFINITE_BLADES)
            {
                stats.Add('b');
            }
            if ((val & INFINITE_AHSS_AMMO) == INFINITE_AHSS_AMMO)
            {
                stats.Add('a');
            }

            return stats;
        }

        public static int ToInt()
        {
            int stats = 0;

            /* Legacy Code.
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