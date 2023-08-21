using UnityEngine;

public class BTN_ServerUS : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        FengGameManagerMKII.OnPrivateServer = false;

        if (Guardian.Networking.NetworkHelper.App == Guardian.Networking.PhotonApplication.AoTTG2)
        {
            PhotonNetwork.ConnectToMaster(Guardian.Networking.PhotonServerAddress.NorthAmerica, Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            Guardian.Networking.NetworkHelper.IsCloud = false;
        }
        else
        {
            PhotonNetwork.networkingPeer.SetApp(FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            PhotonNetwork.networkingPeer.ConnectToRegionMaster(CloudRegionCode.us);
            Guardian.Networking.NetworkHelper.IsCloud = true;
        }
    }
}
