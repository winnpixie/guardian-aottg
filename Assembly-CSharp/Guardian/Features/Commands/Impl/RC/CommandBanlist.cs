namespace Guardian.Features.Commands.Impl.RC
{
    class CommandBanlist : Command
    {
        public CommandBanlist() : base("banlist", new string[0], string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            irc.AddLine("List of banned players:".WithColor("FFCC00"));

            foreach (var id in FengGameManagerMKII.BanHash.Keys)
            {
                irc.AddLine($"{id}: {GExtensions.AsString(FengGameManagerMKII.BanHash[id]).Colored()}");
            }
        }
    }
}
