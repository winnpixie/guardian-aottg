using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.RC.MasterClient
{
    class CommandPause : Command
    {
        public CommandPause() : base("pause", new string[0], string.Empty, true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            FengGameManagerMKII.Instance.photonView.RPC("pauseRPC", PhotonTargets.All, true);

            GameHelper.Broadcast("MasterClient has paused the game!");
        }
    }
}
