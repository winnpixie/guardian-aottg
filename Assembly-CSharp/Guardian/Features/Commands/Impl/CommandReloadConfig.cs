namespace Guardian.Features.Commands.Impl
{
    class CommandReloadConfig : Command
    {
        public CommandReloadConfig() : base("reloadconfig", new string[] { "rlcfg" }, string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            irc.AddLine("Reloading configuration files...");
            Mod.Properties.LoadFromFile();
            irc.AddLine("Configuration reloaded.");

            irc.AddLine("Reloading skin host whitelist...");
            Mod.LoadSkinHostWhitelist();
            irc.AddLine("Skin host whitelist reloaded.");
        }
    }
}