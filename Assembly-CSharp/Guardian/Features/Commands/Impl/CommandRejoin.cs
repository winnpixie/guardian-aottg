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
            PhotonNetwork.ConnectToMaster(host, port, Networking.NetworkHelper.App.Id, UIMainReferences.Version);

            Thread relogThread = new Thread(() =>
            {
                while (PhotonNetwork.networkingPeer.State != PeerState.JoinedLobby && !GThreadPool.ShutdownRequested) { }
                PhotonNetwork.JoinRoom(room);
            });
            relogThread.Name = "GThread#Relog";
            GThreadPool.Enqueue(relogThread);
        }
    }
}