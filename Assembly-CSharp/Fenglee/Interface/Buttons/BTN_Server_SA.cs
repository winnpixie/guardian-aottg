using UnityEngine;

public class BTN_Server_SA : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();

        Guardian.Networking.NetworkHelper.ConnectToRegion(CloudRegionCode.sa);
    }
}
