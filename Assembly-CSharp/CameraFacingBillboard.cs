using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    public enum Axis
    {
        up,
        down,
        left,
        right,
        forward,
        back
    }

    private Camera referenceCamera;
    public bool reverseFace;
    public Axis axis;

    public Vector3 GetAxis(Axis refAxis)
    {
        switch (refAxis)
        {
            case Axis.down:
                return Vector3.down;
            case Axis.forward:
                return Vector3.forward;
            case Axis.back:
                return Vector3.back;
            case Axis.left:
                return Vector3.left;
            case Axis.right:
                return Vector3.right;
            default:
                return Vector3.up;
        }
    }

    private void Awake()
    {
        if (!referenceCamera)
        {
            referenceCamera = Camera.main;
        }
    }

    private void Update()
    {
        Vector3 worldPosition = base.transform.position + referenceCamera.transform.rotation * ((!reverseFace) ? Vector3.back : Vector3.forward);
        Vector3 worldUp = referenceCamera.transform.rotation * GetAxis(axis);
        base.transform.LookAt(worldPosition, worldUp);
    }
}
