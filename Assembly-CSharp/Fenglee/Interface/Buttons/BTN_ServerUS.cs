using UnityEngine;

public class BTN_ServerUS : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();

        Guardian.Networking.NetworkHelper.ConnectToRegion(CloudRegionCode.us);
    }
}
