using UnityEngine;

public class BTN_rotate_character : MonoBehaviour
{
    public GameObject hero;
    public new GameObject camera;
    private float distance = 3f;
    private bool isRotate;

    private void OnPress(bool press)
    {
        isRotate = press;
    }

    private void Update()
    {
        distance -= Input.GetAxis("Mouse ScrollWheel") * 0.05f;
        distance = Mathf.Clamp(distance, 0.8f, 3.5f);
        camera.transform.position = hero.transform.position;
        camera.transform.position += Vector3.up * 1.1f;
        if (isRotate)
        {
            float angle = Input.GetAxis("Mouse X") * 2.5f;
            float angle2 = (0f - Input.GetAxis("Mouse Y")) * 2.5f;
            camera.transform.RotateAround(camera.transform.position, Vector3.up, angle);
            camera.transform.RotateAround(camera.transform.position, camera.transform.right, angle2);
        }
        camera.transform.position -= camera.transform.forward * distance;
    }
}
