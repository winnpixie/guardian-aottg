using System;
using UnityEngine;
using Guardian.Utilities;

public static class RCextensions
{
    public static Texture2D LoadImage(WWW link, bool mipmapEnabled, int maxFileSize)
    {
        if (link.size < maxFileSize)
        {
            Texture2D texture = link.texture;
            int width = texture.width;
            int height = texture.height;

            int resizedSize = 0;
            if (width < 4 || !MathHelper.IsPowerOf2(width))
            {
                resizedSize = 4;
                width = Math.Min(width, 2047);

                if (resizedSize < width)
                {
                    resizedSize = MathHelper.NextPowerOf2(width);
                }
            }
            else if (height < 4 || !MathHelper.IsPowerOf2(height))
            {
                resizedSize = 4;
                height = Math.Min(height, 2047);

                if (resizedSize < height)
                {
                    resizedSize = MathHelper.NextPowerOf2(height);
                }
            }

            if (resizedSize == 0)
            {
                Texture2D output = new Texture2D(4, 4, texture.format, mipmapEnabled);

                try
                {
                    link.LoadImageIntoTexture(output);
                }
                catch
                {
                    output = new Texture2D(4, 4, texture.format, false);
                    link.LoadImageIntoTexture(output);
                }

                output.Compress(true);
                return output;
            }
            else
            {
                TextureFormat compressionFormat = texture.format == TextureFormat.RGB24
                    ? TextureFormat.DXT1
                    : TextureFormat.DXT5;

                Texture2D resized = new Texture2D(4, 4, compressionFormat, false);
                link.LoadImageIntoTexture(resized);

                try
                {
                    resized.Resize(resizedSize, resizedSize, compressionFormat, mipmapEnabled);
                }
                catch
                {
                    resized.Resize(resizedSize, resizedSize, compressionFormat, false);
                }

                resized.Apply();
                return resized;
            }
        }

        Guardian.GuardianClient.Logger.Debug($"Image too large ({link.url}, {link.size} bytes, {maxFileSize} bytes max");
        return new Texture2D(2, 2, TextureFormat.DXT1, false);
    }

    public static void Add<T>(ref T[] source, T value)
    {
        T[] array = new T[source.Length + 1];
        Array.Copy(source, array, source.Length);
        array[array.Length - 1] = value;

        source = array;
    }

    public static void RemoveAt<T>(ref T[] source, int position)
    {
        if (source.Length == 1)
        {
            source = new T[0];
        }
        else if (source.Length > 1)
        {
            T[] array = new T[source.Length - 1];
            int index = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (i != position)
                {
                    array[index] = source[i];
                    index++;
                }
            }
            source = array;
        }
    }
}
