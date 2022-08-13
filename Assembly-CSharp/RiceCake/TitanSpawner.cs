using UnityEngine;

public class TitanSpawner
{
    public float Delay
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public Vector3 Location
    {
        get;
        set;
    }

    public float Time
    {
        get;
        set;
    }

    public bool Endless
    {
        get;
        set;
    }

    public TitanSpawner()
    {
        Name = string.Empty;
        Location = new Vector3(0f, 0f, 0f);
        Time = 30f;
        Endless = false;
        Delay = 30f;
    }

    public void ResetTime()
    {
        Time = Delay;
    }
}
