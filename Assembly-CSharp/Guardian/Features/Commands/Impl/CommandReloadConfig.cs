using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl
{
    class CommandReloadConfig : Command
    {
        public CommandReloadConfig() : base("rlcfg", new string[0], "", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            Mod.Properties.LoadFromFile();
            irc.AddLine("Configuration reloaded.");
            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { "Stats", ModifiedStats.ToInt() }
            });
            Mod.LoadSkinHostWhitelist();
            irc.AddLine("Skin host whitelist reloaded.");
        }
    }
}
