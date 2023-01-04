using UnityEngine;

public class RCRegion
{
    public readonly Vector3 location;
    private readonly float dimX;
    private readonly float dimY;
    private readonly float dimZ;
    public GameObject myBox;

    public RCRegion(Vector3 loc, float x, float y, float z)
    {
        location = loc;
        dimX = x;
        dimY = y;
        dimZ = z;
    }

    public float GetRandomX()
    {
        return location.x + Random.Range((0f - dimX) / 2f, dimX / 2f);
    }

    public float GetRandomY()
    {
        return location.y + Random.Range((0f - dimY) / 2f, dimY / 2f);
    }

    public float GetRandomZ()
    {
        return location.z + Random.Range((0f - dimZ) / 2f, dimZ / 2f);
    }
}
