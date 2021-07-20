using UnityEngine;

public class BTN_Server_JPN : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        FengGameManagerMKII.OnPrivateServer = false;
        Guardian.Networking.NetworkHelper.IsCloud = true;

        string appId = "b92ae2ae-b815-4f37-806a-58b4f58573ff";

        if (Guardian.Networking.NetworkHelper.App == Guardian.Networking.PhotonApplication.Custom)
        {
            appId = FengGameManagerMKII.ApplicationId;
        }

        PhotonNetwork.ConnectToMaster("app-jp.exitgamescloud.com", Guardian.Networking.NetworkHelper.Connection.Port, appId, UIMainReferences.Version);
    }
}
