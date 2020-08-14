using System.Threading;

namespace Guardian.Features.Commands.Impl
{
    class CommandRejoin : Command
    {
        public CommandRejoin() : base("rejoin", new string[] { "relog", "reconnect" }, "", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            string room = PhotonNetwork.room.name;
            if (PhotonNetwork.LeaveRoom())
            {
                new Thread(() =>
                {
                    while (PhotonNetwork.networkingPeer.State != PeerState.JoinedLobby) { }
                    PhotonNetwork.JoinRoom(room);
                }).Start();
            }
        }
    }
}