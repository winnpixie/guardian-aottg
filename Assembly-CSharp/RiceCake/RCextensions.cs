using System;
using UnityEngine;
using Guardian.Utilities;

public static class RCextensions
{
    public static Texture2D LoadImage(WWW link, bool enableMipMapping, int maxFileSize)
    {
        if (link.error != null)
        {
            Guardian.GuardianClient.Logger.Warn($"Can not load image, url: '{link.url}', reason: '{link.error}'");
            return new Texture2D(1, 1, TextureFormat.DXT1, false);
        }

        if (link.size > maxFileSize && Guardian.GuardianClient.Properties.LimitSkinSizes.Value)
        {
            Guardian.GuardianClient.Logger.Warn($"Image too large, url: '{link.url}', size: {link.size} bytes, max: {maxFileSize} bytes");
            return new Texture2D(1, 1, TextureFormat.DXT1, false);
        }

        Texture2D texture = link.texture;
        int width = texture.width;
        int height = texture.height;

        int newSize = 0;
        if (width < 4 || !IsPowerOf2(width))
        {
            newSize = 4;
            width = Math.Min(width, 2047);

            if (newSize < width)
            {
                newSize = MathHelper.NextPowerOf2(width);
            }
        }
        else if (height < 4 || !IsPowerOf2(height))
        {
            newSize = 4;
            height = Math.Min(height, 2047);

            if (newSize < height)
            {
                newSize = MathHelper.NextPowerOf2(height);
            }
        }

        Texture2D output = new Texture2D(1, 1, texture.format, enableMipMapping);
        link.LoadImageIntoTexture(output);

        if (newSize > 0)
        {
            TextureScale.Bilinear(output, newSize, newSize);
        }

        output.Compress(true);
        output.Apply(true);

        UnityEngine.Object.Destroy(texture);
        return output;
    }

    public static bool IsPowerOf2(int val)
    {
        return (val & (val - 1)) == 0;
    }

    public static void Add<T>(ref T[] source, T value)
    {
        T[] array = new T[source.Length + 1];
        Array.Copy(source, array, source.Length);
        array[array.Length - 1] = value;

        source = array;
    }

    public static void RemoveAt<T>(ref T[] source, int index)
    {
        if (source.Length == 1)
        {
            source = new T[0];
            return;
        }

        if (source.Length > 1)
        {
            T[] newArr = new T[source.Length - 1];
            int idx = 0;

            for (int i = 0; i < source.Length; i++)
            {
                if (i == index) continue;

                newArr[idx] = source[i];
                idx++;
            }

            source = newArr;
        }
    }
}
