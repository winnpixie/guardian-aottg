using UnityEngine;

// TODO: Japan region gone forever?
public class BTN_Server_JPN : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        FengGameManagerMKII.OnPrivateServer = false;
        Guardian.Networking.NetworkHelper.IsCloud = true;

        if (Guardian.Networking.NetworkHelper.App == Guardian.Networking.PhotonApplication.Custom)
        {
            PhotonNetwork.ConnectToMaster("app-jp.exitgamescloud.com", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
        }
        else
        {
            PhotonNetwork.ConnectToMaster("app-jp.exitgamescloud.com", Guardian.Networking.NetworkHelper.Connection.Port, "b92ae2ae-b815-4f37-806a-58b4f58573ff", UIMainReferences.Version);
        }
    }
}
