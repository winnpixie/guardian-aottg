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
		IN_GAME_MAIN_CAMERA.Gametype = GameType.Stop;
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;

        Guardian.UI.WindowManager.SetCursorStates(shown: true, locked: false);

		GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
		Object.Destroy(GameObject.Find("MultiplayerManager"));
		Application.LoadLevel("menu");
	}
}
