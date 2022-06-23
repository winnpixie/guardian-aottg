using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GExtensions
{
    private static readonly Regex HexColorPattern = new Regex("\\[([a-f0-9]{6}|-)\\]", RegexOptions.IgnoreCase);
    private static readonly Regex ColorTagPattern = new Regex("<\\/?color(=#?\\w+)?>", RegexOptions.IgnoreCase);

    public static T[] CopyOfRange<T>(this T[] arrIn, int startIndex, int endIndex)
    {
        // Decrement endIndex until it is arrIn.Length - 1
        while (endIndex >= arrIn.Length)
        {
            endIndex--;
        }

        int len = endIndex - startIndex + 1;
        T[] arrOut = new T[len];

        Array.Copy(arrIn, startIndex, arrOut, 0, len);

        return arrOut;
    }

    public static T[] Sorted<T>(this T[] arrIn, Comparison<T> comparator)
    {
        T[] sorted = new T[arrIn.Length];
        arrIn.CopyTo(sorted, 0);
        Array.Sort(sorted, comparator);

        return sorted;
    }

    // Converts a NGUI formatted string to Unity Rich Text
    public static string NGUIToUnity(this string str)
    {
        string output = string.Empty;
        Stack<string> colorStack = new Stack<string>(); // Kudos to Kevin, using a Stack makes this a helluva lot simpler
        bool needsClosingTag = false;

        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];

            if (c.Equals('[') && i + 2 < str.Length)
            {
                if (str[i + 1].Equals('-') && str[i + 2].Equals(']')) // [-], aka return to previous color in the stack
                {
                    if (colorStack.Count > 0)
                    {
                        colorStack.Pop();
                    }
                    if (colorStack.Count < 1)
                    {
                        colorStack.Push("FFFFFF"); // No color history, add FFFFFF as the default
                    }

                    output += needsClosingTag ? $"</color><color=#{colorStack.Peek()}>" : $"<color=#{colorStack.Peek()}>";
                    needsClosingTag = true;
                    i += 2;
                    continue;
                }
                else if (i + 7 < str.Length && str[i + 7].Equals(']') && str.Substring(i + 1, 6).IsHex()) // [RRGGBB], aka use the color supplied by RRGGBB
                {
                    string color = str.Substring(i + 1, 6).ToUpper();
                    colorStack.Push(color);
                    output += needsClosingTag ? $"</color><color=#{color}>" : $"<color=#{color}>";
                    needsClosingTag = true;
                    i += 7;
                    continue;
                }
            }

            output += c;
        }

        return output + (needsClosingTag ? "</color>" : string.Empty);
    }

    public static string StripNGUI(this string str)
    {
        return HexColorPattern.Replace(str, string.Empty);
    }

    public static string StripUnityColors(this string str)
    {
        return ColorTagPattern.Replace(str, string.Empty);
    }

    public static bool IsHex(this string str)
    {
        return (str.Length == 6 || str.Length == 8) && int.TryParse(str, System.Globalization.NumberStyles.AllowHexSpecifier, null, out int v);
    }

    public static string ToHex(this Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255f);
        int g = Mathf.RoundToInt(color.g * 255f);
        int b = Mathf.RoundToInt(color.b * 255f);
        int a = Mathf.RoundToInt(color.a * 255f);

        return r.ToString("X2") + g.ToString("X2") + b.ToString("X2") + a.ToString("X2");
    }

    public static Color ToColor(this string str)
    {
        float red = 0;
        float green = 0;
        float blue = 0;
        float alpha = 1f;

        // Red
        if (int.TryParse(str.Substr(0, 1), System.Globalization.NumberStyles.AllowHexSpecifier, null, out int r))
        {
            red = r / 255F;
        }

        // Green
        if (int.TryParse(str.Substr(2, 3), System.Globalization.NumberStyles.AllowHexSpecifier, null, out int g))
        {
            green = g / 255F;
        }

        // Blue
        if (int.TryParse(str.Substr(4, 5), System.Globalization.NumberStyles.AllowHexSpecifier, null, out int b))
        {
            blue = b / 255F;
        }

        // Alpha
        if (str.Length == 8 && int.TryParse(str.Substr(6, 7), System.Globalization.NumberStyles.AllowHexSpecifier, null, out int a))
        {
            alpha = a / 255F;
        }

        return new Color(red, green, blue, alpha);
    }

    public static string AsBold(this string str)
    {
        return $"<b>{str}</b>";
    }

    public static string AsItalic(this string str)
    {
        return $"<i>{str}</i>";
    }

    public static string AsColor(this string str, string hex)
    {
        if (hex.IsHex())
        {
            return $"<color=#{hex}>{str}</color>";
        }

        return $"<color={hex}>{str}</color>";
    }

    public static string AsString(object obj)
    {
        if (obj != null && obj is string)
        {
            return (string)obj;
        }

        return string.Empty;
    }

    public static int AsInt(object obj)
    {
        if (obj != null && obj is int)
        {
            return (int)obj;
        }
        return 0;
    }

    public static float AsFloat(object obj)
    {
        if (obj != null && obj is float)
        {
            return (float)obj;
        }

        return 0f;
    }

    public static bool AsBool(object obj)
    {
        if (obj != null && obj is bool)
        {
            return (bool)obj;
        }

        return false;
    }

    public static bool TryParseEnum<T>(string input, out T value) where T : Enum
    {
        value = default(T);
        try
        {
            Type enumType = typeof(T);
            value = (T)Enum.Parse(enumType, input, true);

            if (Enum.IsDefined(enumType, value))
            {
                return true;
            }
        }
        catch { }

        return false;
    }

    public static string Substr(this string str, int startIndex, int endIndex)
    {
        // Decrement endIndex until it is str.Length - 1
        while (endIndex >= str.Length)
        {
            endIndex--;
        }

        int len = endIndex - startIndex + 1;

        return str.Substring(startIndex, len);
    }

    public static bool IsKeyDown(this KeyCode keyCode)
    {
        return Event.current != null
            && Event.current.type == EventType.KeyDown
            && Event.current.keyCode == keyCode;
    }

    public static bool IsKeyUp(this KeyCode keyCode)
    {
        return Event.current != null
            && Event.current.type == EventType.KeyUp
            && Event.current.keyCode == keyCode;
    }

    public static Vector2 GetScaleVector(this Texture image, int originalWidth, int originalHeight)
    {
        return new Vector2(image.width / (float)originalWidth, image.height / (float)originalHeight);
    }
}