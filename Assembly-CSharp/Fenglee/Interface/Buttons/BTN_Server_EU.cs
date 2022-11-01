using UnityEngine;

public class BTN_Server_EU : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        FengGameManagerMKII.OnPrivateServer = false;

        if (Guardian.Networking.NetworkHelper.App == Guardian.Networking.PhotonApplication.AoTTG2)
        {
            PhotonNetwork.ConnectToMaster("135.125.239.180", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            Guardian.Networking.NetworkHelper.IsCloud = false;
        }
        else
        {
            PhotonNetwork.networkingPeer.SetApp(FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            PhotonNetwork.networkingPeer.ConnectToRegionMaster(CloudRegionCode.eu);
            Guardian.Networking.NetworkHelper.IsCloud = true;
        }
    }
}
