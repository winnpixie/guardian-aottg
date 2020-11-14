using UnityEngine;

public class BTN_PAUSE_MENU_QUIT : MonoBehaviour
{
    private void OnClick()
    {
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
        {
            Time.timeScale = 1f;
        }
        else
        {
            PhotonNetwork.Disconnect();
        }
        Screen.lockCursor = false;
        Screen.showCursor = true;
        IN_GAME_MAIN_CAMERA.Gametype = GameType.Stop;
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
        GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
        Object.Destroy(GameObject.Find("MultiplayerManager"));
        Application.LoadLevel("menu");
    }
}
