using UnityEngine;

public class PopListCamera : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.HasKey("cameraType"))
        {
            GetComponent<UIPopupList>().selection = PlayerPrefs.GetString("cameraType");
        }
    }

    private void OnSelectionChange()
    {
        if (GExtensions.TryParseEnum(GetComponent<UIPopupList>().selection, out CameraType cameraType))
        {
            IN_GAME_MAIN_CAMERA.CameraMode = cameraType;
        }
        PlayerPrefs.SetString("cameraType", GetComponent<UIPopupList>().selection);
    }
}
