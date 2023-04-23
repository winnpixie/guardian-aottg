using UnityEngine;

public class PopListCamera : MonoBehaviour
{
    private UIPopupList cameraList;

    private void Awake()
    {
        cameraList = GetComponent<UIPopupList>();

        if (PlayerPrefs.HasKey("cameraType"))
        {
            var cameraTypeStr = PlayerPrefs.GetString("cameraType");
            if (GExtensions.TryParseEnum(cameraTypeStr, out CameraType cameraType))
            {
                cameraList.selection = cameraTypeStr;
            }
        }
    }

    private void OnSelectionChange()
    {
        if (GExtensions.TryParseEnum(cameraList.selection, out CameraType cameraType))
        {
            IN_GAME_MAIN_CAMERA.CameraMode = cameraType;
        }

        PlayerPrefs.SetString("cameraType", cameraList.selection);
    }
}
