using UnityEngine;

public class BTN_Server_SA : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        FengGameManagerMKII.OnPrivateServer = false;

        if (Guardian.Networking.NetworkHelper.App == Guardian.Networking.PhotonApplication.AoTTG2)
        {
            PhotonNetwork.ConnectToMaster("172.107.193.233", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            Guardian.Networking.NetworkHelper.IsCloud = false;
        }
        else
        {
            PhotonNetwork.networkingPeer.SetApp(FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            PhotonNetwork.networkingPeer.ConnectToRegionMaster(CloudRegionCode.sa);
            Guardian.Networking.NetworkHelper.IsCloud = true;
        }
    }
}
