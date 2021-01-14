namespace Guardian.Features.Commands.Impl
{
    class CommandReloadConfig : Command
    {
        public CommandReloadConfig() : base("rlcfg", new string[0], string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            Mod.Properties.LoadFromFile();
            irc.AddLine("Configuration reloaded.");
            Mod.LoadSkinHostWhitelist();
            irc.AddLine("Skin host whitelist reloaded.");
        }
    }
}
