using Guardian.Networking;
using System.Threading;
using UnityEngine;

public class BTN_QUICKMATCH : MonoBehaviour
{
    public void OnClick()
    {
        PhotonNetwork.Disconnect();

        string bestRegion = NetworkHelper.GetBestRegion();
        if (bestRegion.Equals("asia") || bestRegion.Equals("jp"))
        {
            bestRegion = "sg";
        }

        PhotonNetwork.ConnectToMaster(bestRegion + ".aottg.tk", NetworkHelper.Connection.Port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version);
        FengGameManagerMKII.OnPrivateServer = false;
        NetworkHelper.IsCloud = FengGameManagerMKII.ApplicationId.Length > 0;

        new Thread(() =>
        {
            while (PhotonNetwork.networkingPeer.State != PeerState.JoinedLobby
                && IN_GAME_MAIN_CAMERA.Gametype == GameType.Stop
                && !Guardian.Mod.IsProgramQuitting) { }
            PhotonNetwork.JoinRandomRoom(MatchmakingMode.RandomMatching);
        }).Start();
    }
}
 