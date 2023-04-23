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
        Stack<string> colorHistory = new Stack<string>(); // Kudos to Kevin, using a Stack makes this a helluva lot simpler
        bool isTagOpen = false;

        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];

            if (c.Equals('[') && i + 2 < str.Length)
            {
                if (str[i + 1].Equals('-') && str[i + 2].Equals(']')) // [-], aka return to previous color in the stack
                {
                    if (colorHistory.Count > 0)
                    {
                        colorHistory.Pop();
                    }
                    if (colorHistory.Count < 1)
                    {
                        colorHistory.Push("FFFFFF"); // No color history, add FFFFFF as the default
                    }

                    output += isTagOpen ? $"</color><color=#{colorHistory.Peek()}>" : $"<color=#{colorHistory.Peek()}>";
                    isTagOpen = true;
                    i += 2;
                    continue;
                }
                else if (i + 7 < str.Length && str[i + 7].Equals(']') && Guardian.Utilities.ColorHelper.IsHex(str.Substring(i + 1, 6))) // [RRGGBB], aka use the color supplied by RRGGBB
                {
                    string color = str.Substring(i + 1, 6).ToUpper();
                    colorHistory.Push(color);
                    output += isTagOpen ? $"</color><color=#{color}>" : $"<color=#{color}>";
                    isTagOpen = true;
                    i += 7;
                    continue;
                }
            }

            output += c;
        }

        return output + (isTagOpen ? "</color>" : string.Empty);
    }

    public static string StripNGUI(this string str)
    {
        return HexColorPattern.Replace(str, string.Empty);
    }

    public static string StripUnityColors(this string str)
    {
        return ColorTagPattern.Replace(str, string.Empty);
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
        if (Guardian.Utilities.ColorHelper.IsHex(hex))
        {
            return $"<color=#{hex}>{str}</color>";
        }

        return $"<color={hex}>{str}</color>";
    }

    public static string AsString(object obj)
    {
        return obj != null && obj is string str ? str : string.Empty;
    }

    public static int AsInt(object obj)
    {
        return obj != null && obj is int i ? i : 0;
    }

    public static float AsFloat(object obj)
    {
        return obj != null && obj is float f ? f : 0;
    }

    public static bool AsBool(object obj)
    {
        return obj != null && obj is bool b && b;
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