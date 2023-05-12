using UnityEngine;

public class Btn_to_Main_from_CC : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();

        Guardian.UI.WindowManager.SetCursorStates(shown: true, locked: false);

        IN_GAME_MAIN_CAMERA.Gametype = GameType.Stop;
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
        GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
        Object.Destroy(GameObject.Find("MultiplayerManager"));
        Application.LoadLevel("menu");

        Guardian.GuardianClient.GuiController.OpenScreen(null);
    }
}
