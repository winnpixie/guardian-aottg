using UnityEngine;

public class FengPlayer
{
    public string Name = "GUEST";
    public string Guild = string.Empty;

    public void InitAsGuest()
    {
        Name = "GUEST" + Random.Range(0, short.MaxValue * 2);
        Guild = "No Guild";
    }
}
