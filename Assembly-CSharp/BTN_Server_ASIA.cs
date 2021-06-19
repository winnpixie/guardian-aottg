using UnityEngine;

public class BTN_Server_ASIA : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();

        if (Guardian.Networking.NetworkHelper.App == Guardian.Networking.PhotonApplication.AoTTG2)
        {
            PhotonNetwork.ConnectToMaster("sg.aottg.tk", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            FengGameManagerMKII.OnPrivateServer = true;
        }
        else
        {
            PhotonNetwork.ConnectToMaster("app-us.exitgamescloud.com", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            FengGameManagerMKII.OnPrivateServer = false;
        }
    }
}
