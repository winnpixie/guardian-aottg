using UnityEngine;

public class BTN_Server_EU : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();

        Guardian.Networking.NetworkHelper.ConnectToRegion(CloudRegionCode.eu);
    }
}
