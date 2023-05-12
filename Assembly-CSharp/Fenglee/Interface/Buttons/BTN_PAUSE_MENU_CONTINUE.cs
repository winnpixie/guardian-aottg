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
            Guardian.UI.WindowManager.SetCursorStates(shown: true, locked: true);

            inputs.menuOn = false;
            currentCamera.GetComponent<SpectatorMovement>().disable = false;
            currentCamera.GetComponent<MouseLook>().disable = false;
            return;
        }
        IN_GAME_MAIN_CAMERA.IsPausing = false;
        if (IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS)
        {
            Guardian.UI.WindowManager.SetCursorStates(shown: false, locked: true);
        }
        else
        {
            Guardian.UI.WindowManager.SetCursorStates(shown: false, locked: false);
        }
        inputs.menuOn = false;
        inputs.JustUpdate();
    }
}
