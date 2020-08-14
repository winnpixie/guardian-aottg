namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandRestart : Command
    {
        public CommandRestart() : base("restart", new string[0], "", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            FengGameManagerMKII.Instance.RestartRC();
            FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, "MasterClient restarted the game!".WithColor("ffcc00"), string.Empty);
        }
    }
}
