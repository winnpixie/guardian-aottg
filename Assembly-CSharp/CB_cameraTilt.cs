using UnityEngine;

public class CB_cameraTilt : MonoBehaviour
{
    private bool init;

    private void OnActivate(bool result)
    {
        if (!init)
        {
            init = true;
            if (PlayerPrefs.HasKey("cameraTilt"))
            {
                base.gameObject.GetComponent<UICheckbox>().isChecked = ((PlayerPrefs.GetInt("cameraTilt") == 1) ? true : false);
            }
            else
            {
                PlayerPrefs.SetInt("cameraTilt", 1);
            }
        }
        else
        {
            PlayerPrefs.SetInt("cameraTilt", result ? 1 : 0);
        }
        IN_GAME_MAIN_CAMERA.CameraTilt = PlayerPrefs.GetInt("cameraTilt");
    }
}
