using UnityEngine;

public struct STransform
{
    public Vector3 position;
    public Quaternion rotation;

    public void Reset()
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }

    public void LookAt(Vector3 target, Vector3 up)
    {
        Vector3 forward = target - position;
        rotation = Quaternion.LookRotation(forward, up);
    }
}
