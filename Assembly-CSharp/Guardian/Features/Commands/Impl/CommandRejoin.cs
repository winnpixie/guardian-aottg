using System.Threading;

namespace Guardian.Features.Commands.Impl
{
    class CommandRejoin : Command
    {
        public CommandRejoin() : base("rejoin", new string[] { "relog", "reconnect" }, string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            var room = PhotonNetwork.room.name;

            var addr = PhotonNetwork.networkingPeer.MasterServerAddress.Split(':');
            var host = addr[0];
            var port = Networking.NetworkHelper.Connection.Port;
            if (addr.Length > 1 && int.TryParse(args[1], out port)) { }

            PhotonNetwork.Disconnect();
            PhotonNetwork.ConnectToMaster(host, port, Networking.NetworkHelper.App.Id, UIMainReferences.Version);

            new Thread(() =>
            {
                while (PhotonNetwork.networkingPeer.State != PeerState.JoinedLobby) { }
                PhotonNetwork.JoinRoom(room);
            }).Start();

        }
    }
}