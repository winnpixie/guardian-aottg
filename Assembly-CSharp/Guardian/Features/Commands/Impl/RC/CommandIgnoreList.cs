namespace Guardian.Features.Commands.Impl.RC
{
    class CommandIgnoreList : Command
    {
        public CommandIgnoreList() : base("ignorelist", new string[] { "ignored" }, string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            irc.AddLine("List of ignored players:".AsColor("FFCC00"));

            foreach (int id in FengGameManagerMKII.IgnoreList)
            {
                irc.AddLine(id.ToString());
            }
        }
    }
}
