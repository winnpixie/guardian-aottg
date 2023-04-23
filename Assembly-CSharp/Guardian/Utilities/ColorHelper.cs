using UnityEngine;

namespace Guardian.Utilities
{
    class ColorHelper
    {
        public static readonly Color Orange = new Color(1f, 0.5f, 0f);

        public static bool IsHex(string str)
        {
            return (str.Length == 6 || str.Length == 8) && int.TryParse(str, System.Globalization.NumberStyles.AllowHexSpecifier, null, out _);
        }

        public static string ToHex(Color color)
        {
            int r = Mathf.RoundToInt(color.r * 255f);
            int g = Mathf.RoundToInt(color.g * 255f);
            int b = Mathf.RoundToInt(color.b * 255f);
            int a = Mathf.RoundToInt(color.a * 255f);

            return r.ToString("X2") + g.ToString("X2") + b.ToString("X2") + a.ToString("X2");
        }

        public static Color FromHex(string str)
        {
            float red = 0;
            float green = 0;
            float blue = 0;
            float alpha = 1f;

            // Red
            if (int.TryParse(str.Substr(0, 1), System.Globalization.NumberStyles.AllowHexSpecifier, null, out int r))
            {
                red = r / 255f;
            }

            // Green
            if (int.TryParse(str.Substr(2, 3), System.Globalization.NumberStyles.AllowHexSpecifier, null, out int g))
            {
                green = g / 255f;
            }

            // Blue
            if (int.TryParse(str.Substr(4, 5), System.Globalization.NumberStyles.AllowHexSpecifier, null, out int b))
            {
                blue = b / 255f;
            }

            // Alpha
            if (str.Length == 8 && int.TryParse(str.Substr(6, 7), System.Globalization.NumberStyles.AllowHexSpecifier, null, out int a))
            {
                alpha = a / 255f;
            }

            return new Color(red, green, blue, alpha);
        }
    }
}
