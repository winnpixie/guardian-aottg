namespace Guardian.Features.Commands.Impl
{
    class CommandRageQuit : Command
    {
        public CommandRageQuit() : base("ragequit", new string[] { "rq", "quit", "leave" }, string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            PhotonNetwork.Disconnect();
        }
    }
}
