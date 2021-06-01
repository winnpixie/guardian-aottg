using Guardian.Networking;
using System.Threading;
using UnityEngine;

public class BTN_QUICKMATCH : MonoBehaviour
{
    public void OnClick()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectToMaster(NetworkHelper.GetBestRegion(), NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
        FengGameManagerMKII.OnPrivateServer = false;

        Thread quickmatchThread = new Thread(() =>
        {
            while (PhotonNetwork.networkingPeer.State != PeerState.JoinedLobby && !GThreadPool.ShutdownRequested) { }
            PhotonNetwork.JoinRandomRoom();
        });
        quickmatchThread.Name = "GThread#Quickmatch";
        GThreadPool.Enqueue(quickmatchThread);
    }
}
