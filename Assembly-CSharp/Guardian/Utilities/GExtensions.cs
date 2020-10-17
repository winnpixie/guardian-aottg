using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GExtensions
{
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

    // My implementation of NGUI color parsing
    public static string Colored(this string str)
    {
        string output = "";
        Stack<string> colors = new Stack<string>(); // Thank you to Kevin for telling me to use a Stack
        bool coloring = false;

        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];

            if (c == '[' && i + 2 < str.Length)
            {
                if (str[i + 1] == '-' && str[i + 2] == ']') // [-], aka return to previous color in the stack
                {
                    string previous = "ffffff"; // Default to white

                    if (colors.Count > 0)
                    {
                        colors.Pop();

                        // Is there any color left to use?
                        if (colors.Count > 0)
                        {
                            previous = colors.Peek();
                        }
                    }

                    output += coloring ? $"</color><color=#{previous}>" : $"<color=#{previous}>";
                    coloring = true;
                    i += 2;
                    continue;
                }
                else if (i + 7 < str.Length && str[i + 7] == ']' && str.Substring(i + 1, 6).IsHex()) // [RRGGBB], aka use the color supplied by RRGGBB
                {
                    string color = str.Substring(i + 1, 6).ToUpper();
                    colors.Push(color);
                    output += coloring ? $"</color><color=#{color}>" : $"<color=#{color}>";
                    coloring = true;
                    i += 7;
                    continue;
                }
            }

            output += c;
        }

        return output + (coloring ? "</color>" : "");
    }

    public static string Uncolored(this string str)
    {
        string output = "";

        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];

            if (c == '[' && i + 2 < str.Length)
            {
                if (str[i + 1] == '-' && str[i + 2] == ']')
                {
                    i += 2;
                    continue;
                }
                else if (i + 7 < str.Length && str[i + 7] == ']' && str.Substring(i + 1, 6).IsHex())
                {
                    i += 7;
                    continue;
                }
            }

            output += c;
        }

        return output;
    }

    public static bool IsHex(this string str)
    {
        return (str.Length == 6 || str.Length == 8) && int.TryParse(str, System.Globalization.NumberStyles.AllowHexSpecifier, null, out int result);
    }

    public static string ToHex(this Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);
        int a = Mathf.RoundToInt(color.a * 255);

        return r.ToString("X2") + g.ToString("X2") + b.ToString("X2") + a.ToString("X2");
    }

    public static string AsBold(this string str)
    {
        return $"<b>{str}</b>";
    }

    public static string AsItalic(this string str)
    {
        return $"<i>{str}</i>";
    }

    public static string WithColor(this string str, string hex)
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
        value = default;
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
        return str.Substring(startIndex, endIndex - startIndex + 1);
    }
}