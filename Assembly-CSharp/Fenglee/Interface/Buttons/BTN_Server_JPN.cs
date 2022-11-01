using UnityEngine;

public class BTN_Server_JPN : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        FengGameManagerMKII.OnPrivateServer = false;
        Guardian.Networking.NetworkHelper.IsCloud = true;

        string appId = Guardian.Networking.PhotonApplication.Guardian.Id;

        if (Guardian.Networking.NetworkHelper.App == Guardian.Networking.PhotonApplication.Custom)
        {
            appId = FengGameManagerMKII.ApplicationId;
        }

        PhotonNetwork.networkingPeer.SetApp(appId, UIMainReferences.Version);
        PhotonNetwork.networkingPeer.ConnectToRegionMaster(CloudRegionCode.jp);
    }
}
