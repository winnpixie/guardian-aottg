using UnityEngine;

public class BTN_ServerUS : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectToMaster("app-us.exitgamescloud.com", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.version);
        FengGameManagerMKII.OnPrivateServer = false;
    }
}
