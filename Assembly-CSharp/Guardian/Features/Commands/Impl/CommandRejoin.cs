using System.Threading;

namespace Guardian.Features.Commands.Impl
{
    class CommandRejoin : Command
    {
        public CommandRejoin() : base("rejoin", new string[] { "relog", "reconnect" }, string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            string room = PhotonNetwork.room.name;
            string[] addr = PhotonNetwork.networkingPeer.MasterServerAddress.Split(':');
            string host = addr[0];
            int port = Networking.NetworkHelper.Connection.Port;
            if (addr.Length > 1 && int.TryParse(addr[1], out port)) { }

            PhotonNetwork.Disconnect();

            if(PhotonNetwork.ConnectToMaster(host, port, Networking.NetworkHelper.App.Id, UIMainReferences.Version))
            {
                new Thread(() =>
                {
                    while (PhotonNetwork.networkingPeer.State != PeerState.JoinedLobby
                        && IN_GAME_MAIN_CAMERA.Gametype == GameType.Stop
                        && !Mod.IsProgramQuitting) { }
                    PhotonNetwork.JoinRoom(room);
                }).Start();
            }
        }
    }
}