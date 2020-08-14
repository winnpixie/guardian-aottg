using Guardian.Networking;
using System.Threading;
using UnityEngine;

public class BTN_QUICKMATCH : MonoBehaviour
{
    public void OnClick()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectToMaster(NetworkHelper.GetBestRegion(), NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.version);
        FengGameManagerMKII.OnPrivateServer = false;

        // TODO: Change functionality since this proves itself to be useless for true randomness
        new Thread(() =>
        {
            while (PhotonNetwork.networkingPeer.State != PeerState.JoinedLobby) { }
            PhotonNetwork.JoinRandomRoom();
        }).Start();
    }
}
