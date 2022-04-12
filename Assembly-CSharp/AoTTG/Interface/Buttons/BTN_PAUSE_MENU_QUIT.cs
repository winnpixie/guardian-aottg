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

        FengGameManagerMKII.Instance.gameStart = false;

        GameObject imc = GameObject.Find("InputManagerController");
        if(imc != null)
        {
            imc.GetComponent<FengCustomInputs>().menuOn = false;
        }

        Object.Destroy(GameObject.Find("MultiplayerManager"));
        Application.LoadLevel("menu");
    }
}
