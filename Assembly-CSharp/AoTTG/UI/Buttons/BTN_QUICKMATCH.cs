using UnityEngine;

public class BTN_QUICKMATCH : MonoBehaviour
{
    public void OnClick()
    {
        PhotonNetwork.Disconnect();

        PhotonNetwork.offlineMode = true;

        FengGameManagerMKII.Instance.OnJoinedLobby();
    }
}
