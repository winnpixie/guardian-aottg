namespace Guardian.Features.Commands.Impl.RC
{
    class CommandIgnoreList : Command
    {
        public CommandIgnoreList() : base("ignorelist", new string[] { "ignored" }, string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            irc.AddLine("List of ignored players:".WithColor("FFCC00"));

            foreach (var id in FengGameManagerMKII.IgnoreList)
            {
                irc.AddLine(id.ToString().WithColor("FFCC00"));
            }
        }
    }
}
