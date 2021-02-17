using UnityEngine;

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

            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
            {
                foreach (HERO hero in Object.FindObjectsOfType<HERO>())
                {
                    if (hero.myNetWorkName != null)
                    {
                        if (hero.photonView.isMine)
                        {
                            hero.myNetWorkName.GetComponent<UILabel>().alpha = (float)Mod.Properties.OpacityOfOwnName.Value;
                        }
                        else
                        {
                            hero.myNetWorkName.GetComponent<UILabel>().alpha = (float)Mod.Properties.OpacityOfOtherNames.Value;
                        }
                    }
                }
            }
        }
    }
}