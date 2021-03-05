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
                    var player = PhotonPlayer.Find(id);
                    if (player != null)
                    {
                        if (!GameHelper.IsDead(player))
                        {
                            if (GameHelper.IsPT(player))
                            {
                                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObjectTitan(GameHelper.GetPT(player).gameObject);
                            } else
                            {
                                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(GameHelper.GetHero(player).gameObject);
                            }
                            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(false);
                            irc.AddLine($"Now spectating #{id}.".WithColor("FFCC00"));
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
                    irc.AddLine("You entered spectator mode.".WithColor("FFCC00"));
                }
                else
                {
                    FengGameManagerMKII.Settings[0xf5] = 0;
                    FengGameManagerMKII.Instance.EnterSpecMode(false);
                    irc.AddLine("You left spectator mode.".WithColor("FFCC00"));
                }
            }
        }
    }
}
