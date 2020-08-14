using UnityEngine;

public class BTN_RESULT_TO_MAIN : MonoBehaviour
{
	private void OnClick()
	{
		Time.timeScale = 1f;
		if (PhotonNetwork.connected)
		{
			PhotonNetwork.Disconnect();
		}
		IN_GAME_MAIN_CAMERA.Gametype = GAMETYPE.STOP;
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
		Screen.lockCursor = false;
		Screen.showCursor = true;
		GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
		Object.Destroy(GameObject.Find("MultiplayerManager"));
		Application.LoadLevel("menu");
	}
}
