namespace Guardian.Features.Commands.Impl.RC.MasterClient
{
    class CommandBanlist : Command
    {
        public CommandBanlist() : base("banlist", new string[0], string.Empty, true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            irc.AddLine("List of banned players:".AsColor("FFCC00"));

            foreach (int id in FengGameManagerMKII.BanHash.Keys)
            {
                irc.AddLine($"#{id} ({GExtensions.AsString(FengGameManagerMKII.BanHash[id]).ColorParsed()})");
            }
        }
    }
}
