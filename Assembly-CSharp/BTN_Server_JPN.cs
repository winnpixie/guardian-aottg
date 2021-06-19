using UnityEngine;

public class BTN_Server_JPN : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();

        // TODO: Convert Japan button to South America
        if (Guardian.Networking.NetworkHelper.App == Guardian.Networking.PhotonApplication.AoTTG2)
        {
            PhotonNetwork.ConnectToMaster("sa.aottg.tk", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            FengGameManagerMKII.OnPrivateServer = true;
        }
        else
        {
            PhotonNetwork.ConnectToMaster("app-sa.exitgames.com", Guardian.Networking.NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
            FengGameManagerMKII.OnPrivateServer = false;
        }
    }
}
