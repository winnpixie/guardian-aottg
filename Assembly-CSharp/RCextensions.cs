using System;
using UnityEngine;

public static class RCextensions
{
    public static Texture2D LoadImage(WWW link, bool mipmap, int size)
    {
        Texture2D output = new Texture2D(4, 4, TextureFormat.DXT1, mipmap);
        if (link.size < size)
        {
            Texture2D texture = link.texture;
            int width = texture.width;
            int height = texture.height;
            int num = 0;
            if (width < 4 || (width & (width - 1)) != 0)
            {
                num = 4;
                width = Math.Min(width, 1023);
                while (num < width)
                {
                    num *= 2;
                }
            }
            else if (height < 4 || (height & (height - 1)) != 0)
            {
                num = 4;
                height = Math.Min(height, 1023);
                while (num < height)
                {
                    num *= 2;
                }
            }
            if (num == 0)
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
            else if (num >= 4)
            {
                Texture2D resized = new Texture2D(4, 4, TextureFormat.DXT1, mipmap: false);
                link.LoadImageIntoTexture(resized);
                try
                {
                    resized.Resize(num, num, TextureFormat.DXT1, mipmap);
                }
                catch
                {
                    resized.Resize(num, num, TextureFormat.DXT1, false);
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
        for (int i = 0; i < source.Length; i++)
        {
            array[i] = source[i];
        }
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
