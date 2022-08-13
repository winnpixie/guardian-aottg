using System.Threading;

namespace Guardian.Features.Commands.Impl
{
    class CommandRejoin : Command
    {
        public CommandRejoin() : base("rejoin", new string[] { "relog", "reconnect" }, string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            string lastRoomName = PhotonNetwork.room.name;

            string[] lastMasterAddr = PhotonNetwork.networkingPeer.MasterServerAddress.Split(':');
            string lastHost = lastMasterAddr[0];

            int lastPort = Networking.NetworkHelper.Connection.Port;
            if (lastMasterAddr.Length > 1 && !int.TryParse(lastMasterAddr[1], out lastPort)) return;

            PhotonNetwork.Disconnect();

            if (!PhotonNetwork.ConnectToMaster(lastHost, lastPort, Networking.NetworkHelper.App.Id, UIMainReferences.Version)) return;

            new Thread(() =>
            {
                while (PhotonNetwork.networkingPeer.State != PeerState.JoinedLobby
                    && IN_GAME_MAIN_CAMERA.Gametype == GameType.Stop
                    && !GuardianClient.WasQuitRequested) { }
                PhotonNetwork.JoinRoom(lastRoomName);
            })
            {
                Name = "guardian_rejoin_thread"
            }.Start();
        }
    }
}