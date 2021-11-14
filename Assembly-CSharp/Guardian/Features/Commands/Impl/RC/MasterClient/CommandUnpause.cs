using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.RC.MasterClient
{
    class CommandUnpause : Command
    {
        public CommandUnpause() : base("unpause", new string[0], "", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            FengGameManagerMKII.Instance.photonView.RPC("pauseRPC", PhotonTargets.All, false);

            GameHelper.Broadcast("MasterClient has unpaused the game!");
        }
    }
}
