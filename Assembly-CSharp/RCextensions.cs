using System;
using UnityEngine;
using Guardian.Utilities;

public static class RCextensions
{
    public static Texture2D LoadImage(WWW link, bool mipmap, int maxFileSize)
    {
        Texture2D output = new Texture2D(4, 4, TextureFormat.DXT1, mipmap);

        if (link.size < maxFileSize)
        {
            Texture2D texture = link.texture;
            int width = texture.width;
            int height = texture.height;
            int imgSize = 0;

            if (width < 4 || !MathHelper.IsPowerOf2(width))
            {
                imgSize = 4;
                width = Math.Min(width, 2047);

                if (imgSize < width)
                {
                    imgSize = MathHelper.NextPowerOf2(width);
                }
            }
            else if (height < 4 || !MathHelper.IsPowerOf2(height))
            {
                imgSize = 4;
                height = Math.Min(height, 2047);

                if (imgSize < height)
                {
                    imgSize = MathHelper.NextPowerOf2(height);
                }
            }

            if (imgSize == 0)
            {
                try
                {
                    link.LoadImageIntoTexture(output);
                }
                catch
                {
                    output = new Texture2D(4, 4, TextureFormat.DXT1, mipmap: false);
                    link.LoadImageIntoTexture(output);
                }
            }
            else if (imgSize >= 4)
            {
                Texture2D resized = new Texture2D(4, 4, TextureFormat.DXT1, mipmap);
                link.LoadImageIntoTexture(resized);

                try
                {
                    resized.Resize(imgSize, imgSize, TextureFormat.DXT1, mipmap);
                }
                catch
                {
                    resized.Resize(imgSize, imgSize, TextureFormat.DXT1, hasMipMap: false);
                }
                resized.Apply();
                output = resized;
            }
        }

        return output;
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
