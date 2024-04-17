using UnityEngine;

public class CameraForLeftEye : MonoBehaviour
{
    public GameObject rightEye;
    private new Camera camera;
    private Camera cameraRightEye;

    private void Start()
    {
        camera = GetComponent<Camera>();
        cameraRightEye = rightEye.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        camera.aspect = cameraRightEye.aspect;
        camera.fieldOfView = cameraRightEye.fieldOfView;
    }
}
