using UnityEngine;

public class BTN_Server_JPN : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectToMaster("app-jp.exitgamescloud.com", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.version);
        FengGameManagerMKII.OnPrivateServer = false;
    }
}
