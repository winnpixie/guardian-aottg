using UnityEngine;
using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.RC
{
    class CommandSpectate : Command
    {
        public CommandSpectate() : base("spectate", new string[] { "spec" }, "[id]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                if (int.TryParse(args[0], out int id))
                {
                    PhotonPlayer player = PhotonPlayer.Find(id);
                    if (player != null)
                    {
                        if (!GameHelper.IsDead(player))
                        {
                            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(GameHelper.IsPT(player) ? GameHelper.GetPT(player).gameObject : GameHelper.GetHero(player).gameObject, true, false);
                            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(false);
                            irc.AddLine($"Now spectating #{id}.".WithColor("ffcc00"));
                        }
                    }
                }
            }
            else
            {
                if (((int)FengGameManagerMKII.Settings[0xf5]) == 0)
                {
                    FengGameManagerMKII.Settings[0xf5] = 1;
                    FengGameManagerMKII.Instance.EnterSpecMode(true);
                    irc.AddLine("You entered spectator mode.".WithColor("ffcc00"));
                }
                else
                {
                    FengGameManagerMKII.Settings[0xf5] = 0;
                    FengGameManagerMKII.Instance.EnterSpecMode(false);
                    irc.AddLine("You left spectator mode.".WithColor("ffcc00"));
                }
            }
        }
    }
}
