namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandRestart : Command
    {
        public CommandRestart() : base("restart", new string[0], string.Empty, true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            FengGameManagerMKII.Instance.RestartRC();

            Utilities.GameHelper.Broadcast("The round has been restarted!");
        }
    }
}
