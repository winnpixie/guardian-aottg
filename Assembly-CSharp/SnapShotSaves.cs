using UnityEngine;
using System.Collections.Generic;

public class SnapShotSaves
{
    private static List<Texture2D> Images;
    private static List<int> Damages;
    private static bool Initialized;
    private static int Index;
    private static int CurrentIndex;

    public static void Init()
    {
        if (!Initialized)
        {
            Initialized = true;
            Index = 0;
            CurrentIndex = 0;
            Images = new List<Texture2D>();
            Damages = new List<int>();
        }
    }

    public static void AddImage(Texture2D tex, int damage)
    {
        Init();

        Images.Add(tex);
        Damages.Add(damage);

        CurrentIndex = Index;
        Index = (Index + 1) % Images.Count;
    }

    public static int GetCurrentIndex()
    {
        Init();

        return CurrentIndex;
    }

    public static int GetLength()
    {
        Init();

        return Images.Count;
    }

    public static Texture2D GetCurrentImage()
    {
        Init();

        return Images.Count > 0 ? Images[CurrentIndex] : null;
    }

    public static int GetCurrentDamage()
    {
        Init();

        return Damages.Count > 0 ? Damages[CurrentIndex] : 0;
    }

    public static Texture2D GetNextImage()
    {
        Init();

        if (Images.Count == 0)
        {
            return GetCurrentImage();
        }
        CurrentIndex = (CurrentIndex + 1) % Images.Count;

        return GetCurrentImage();
    }

    public static Texture2D GetPreviousImage()
    {
        Init();

        if (Images.Count == 0)
        {
            return GetCurrentImage();
        }
        CurrentIndex = (CurrentIndex + Images.Count - 1) % Images.Count;

        return GetCurrentImage();
    }
}
