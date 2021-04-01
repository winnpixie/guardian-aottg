using UnityEngine;

public class BTN_PAUSE_MENU_CONTINUE : MonoBehaviour
{
    private FengCustomInputs inputs;
    private GameObject currentCamera;

    public void Start()
    {
        inputs = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
        currentCamera = GameObject.Find("MainCamera");
    }

    private void OnClick()
    {
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
        {
            Time.timeScale = 1f;
        }
        GameObject gameObject = GameObject.Find("UI_IN_GAME");
        NGUITools.SetActive(gameObject.GetComponent<UIReferArray>().panels[0], state: true);
        NGUITools.SetActive(gameObject.GetComponent<UIReferArray>().panels[1], state: false);
        NGUITools.SetActive(gameObject.GetComponent<UIReferArray>().panels[2], state: false);
        NGUITools.SetActive(gameObject.GetComponent<UIReferArray>().panels[3], state: false);
        if (!currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().enabled)
        {
            Screen.showCursor = true;
            Screen.lockCursor = true;
            inputs.menuOn = false;
            currentCamera.GetComponent<SpectatorMovement>().disable = false;
            currentCamera.GetComponent<MouseLook>().disable = false;
            return;
        }
        IN_GAME_MAIN_CAMERA.IsPausing = false;
        if (IN_GAME_MAIN_CAMERA.CameraMode == CAMERA_TYPE.TPS)
        {
            Screen.showCursor = false;
            Screen.lockCursor = true;
        }
        else
        {
            Screen.showCursor = false;
            Screen.lockCursor = false;
        }
        inputs.menuOn = false;
        inputs.justUPDATEME();
    }
}
