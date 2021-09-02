using UnityEngine;

public class CheckBoxCamera : MonoBehaviour
{
    public new CameraType camera;

    private void Start()
    {
        if (PlayerPrefs.HasKey("cameraType"))
        {
            if (camera.ToString().ToUpper() == PlayerPrefs.GetString("cameraType").ToUpper())
            {
                GetComponent<UICheckbox>().isChecked = true;
            }
            else
            {
                GetComponent<UICheckbox>().isChecked = false;
            }
        }
    }

    private void OnSelectionChange(bool yes)
    {
        if (yes)
        {
            IN_GAME_MAIN_CAMERA.CameraMode = camera;
            PlayerPrefs.SetString("cameraType", camera.ToString().ToUpper());
        }
    }
}
