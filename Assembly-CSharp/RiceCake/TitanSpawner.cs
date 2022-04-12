using UnityEngine;

public class TitanSpawner
{
    public float delay
    {
        get;
        set;
    }

    public string name
    {
        get;
        set;
    }

    public Vector3 location
    {
        get;
        set;
    }

    public float time
    {
        get;
        set;
    }

    public bool endless
    {
        get;
        set;
    }

    public TitanSpawner()
    {
        name = string.Empty;
        location = new Vector3(0f, 0f, 0f);
        time = 30f;
        endless = false;
        delay = 30f;
    }

    public void resetTime()
    {
        time = delay;
    }
}
