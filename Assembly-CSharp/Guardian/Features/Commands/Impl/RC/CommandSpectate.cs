using UnityEngine;

namespace Guardian.Features.Commands.Impl.RC
{
    class CommandSpectate : Command
    {
        public CommandSpectate() : base("spectate", new string[] { "spec", "specmode" }, "[id]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                if (!int.TryParse(args[0], out int id)) return;

                PhotonPlayer player = PhotonPlayer.Find(id);
                if (player == null || player.IsDead) return;

                if (player.IsTitan)
                {
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObjectTitan(player.GetTitan().gameObject);
                }
                else
                {
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(player.GetHero().gameObject);
                }

                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(false);
                irc.AddLine($"Now spectating #{id}.".AsColor("FFCC00"));
            }
            else
            {
                if (((int)FengGameManagerMKII.Settings[0xf5]) == 0)
                {
                    FengGameManagerMKII.Settings[0xf5] = 1;
                    FengGameManagerMKII.Instance.EnterSpecMode(true);
                    irc.AddLine("You entered spectator mode.".AsColor("FFCC00"));
                }
                else
                {
                    FengGameManagerMKII.Settings[0xf5] = 0;
                    FengGameManagerMKII.Instance.EnterSpecMode(false);
                    irc.AddLine("You left spectator mode.".AsColor("FFCC00"));
                }
            }
        }
    }
}
