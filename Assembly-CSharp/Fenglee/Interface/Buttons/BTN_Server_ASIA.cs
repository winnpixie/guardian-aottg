using UnityEngine;

public class BTN_Server_ASIA : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        FengGameManagerMKII.OnPrivateServer = false;

        if (Guardian.Networking.NetworkHelper.App == Guardian.Networking.PhotonApplication.AoTTG2)
        {
            PhotonNetwork.ConnectToMaster("51.79.164.137", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            Guardian.Networking.NetworkHelper.IsCloud = false;
        }
        else
        {
            PhotonNetwork.ConnectToMaster("app-asia.exitgamescloud.com", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            Guardian.Networking.NetworkHelper.IsCloud = true;
        }
    }
}
