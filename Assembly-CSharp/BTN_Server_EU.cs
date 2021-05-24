using UnityEngine;

public class BTN_Server_EU : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        
        if (Guardian.Networking.NetworkHelper.App == Guardian.Networking.PhotonApplication.AoTTG2)
        {
            PhotonNetwork.ConnectToMaster("eu.aot2.cf", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            FengGameManagerMKII.OnPrivateServer = true;
        }
        else
        {
            PhotonNetwork.ConnectToMaster("app-us.exitgamescloud.com", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            FengGameManagerMKII.OnPrivateServer = false;
        }
    }
}
