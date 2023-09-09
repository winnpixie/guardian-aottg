using UnityEngine;

public class BTN_Server_ASIA : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();

        Guardian.Networking.NetworkHelper.ConnectToRegion(CloudRegionCode.asia);
    }
}
