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

        new Thread(() =>
        {
            while (PhotonNetwork.networkingPeer.State != PeerState.JoinedLobby
                && IN_GAME_MAIN_CAMERA.Gametype == GameType.Stop
                && !Guardian.Mod.IsProgramQuitting) { }
            PhotonNetwork.JoinRandomRoom();
        }).Start();
    }
}
