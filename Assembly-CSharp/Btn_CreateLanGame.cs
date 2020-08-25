using UnityEngine;

public class Btn_CreateLanGame : MonoBehaviour
{
	private void OnClick()
	{
		PhotonNetwork.Disconnect();
		MonoBehaviour.print("IP:" + Network.player.ipAddress + Network.player.externalIP);
		PhotonNetwork.ConnectToMaster(Network.player.ipAddress, 5055, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
	}

	private void Start()
	{
		Object.Destroy(base.gameObject);
	}
}
