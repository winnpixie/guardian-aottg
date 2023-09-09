using UnityEngine;

public class BTN_Server_JPN : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();

        Guardian.Networking.NetworkHelper.ConnectToRegion(CloudRegionCode.jp);
    }
}
