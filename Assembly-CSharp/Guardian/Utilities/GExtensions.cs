using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Guardian.Utilities;
using UnityEngine;

public static class GExtensions
{
    private static readonly Regex HexColorPattern = new Regex("\\[([a-f0-9]{6}|-)\\]", RegexOptions.IgnoreCase);
    private static readonly Regex ColorTagPattern = new Regex("<\\/?color.*?>", RegexOptions.IgnoreCase);

    public static T[] CopyOfRange<T>(this T[] src, int start, int end)
    {
        int max = src.Length - 1;
        if (end > max)
        {
            end = max;
        }

        int len = end - start + 1;
        T[] dst = new T[len];

        Array.Copy(src, start, dst, 0, len);

        return dst;
    }

    public static T[] Sorted<T>(this T[] src, Comparison<T> comparator)
    {
        int len = src.Length;
        T[] dst = new T[len];
        Array.Copy(src, 0, dst, 0, len);

        Array.Sort(dst, comparator);

        return dst;
    }

    // Converts a NGUI formatted string to Unity Rich Text
    public static string NGUIToUnity(this string str)
    {
        StringBuilder output = new StringBuilder(str.Length);
        Stack<string> colors = new Stack<string>(); // Kudos to Kevin, using a Stack makes this a helluva lot simpler

        bool open = false;
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];

            if (c.Equals('[') && i + 2 < str.Length)
            {
                if (str[i + 1].Equals('-') && str[i + 2].Equals(']')) // [-], aka return to previous color in the stack
                {
                    if (colors.Count > 0)
                    {
                        colors.Pop();
                    }

                    if (colors.Count == 0)
                    {
                        colors.Push("FFFFFF"); // No color history, add FFFFFF as the default
                    }

                    output.Append(open ? $"</color><color=#{colors.Peek()}>" : $"<color=#{colors.Peek()}>");
                    open = true;

                    i += 2;
                    continue;
                }
                else if (i + 7 < str.Length && str[i + 7].Equals(']') && ColorHelper.IsHex(str.Substring(i + 1, 6))) // [RRGGBB], aka use the color supplied by RRGGBB
                {
                    string color = str.Substring(i + 1, 6).ToUpper();
                    colors.Push(color);
                    output.Append(open ? $"</color><color=#{color}>" : $"<color=#{color}>");
                    open = true;

                    i += 7;
                    continue;
                }
            }

            output.Append(c);
        }

        if (open)
        {
            output.Append("</color>");
        }

        return output.ToString();
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
        if (ColorHelper.IsHex(hex))
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