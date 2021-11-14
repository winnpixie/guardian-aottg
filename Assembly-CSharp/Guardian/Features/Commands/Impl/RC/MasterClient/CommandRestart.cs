namespace Guardian.Features.Commands.Impl.RC.MasterClient
{
    class CommandRestart : Command
    {
        public CommandRestart() : base("restart", new string[] { "r" }, string.Empty, true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            FengGameManagerMKII.Instance.RestartRC();

            Utilities.GameHelper.Broadcast("The round has been restarted!");
        }
    }
}
