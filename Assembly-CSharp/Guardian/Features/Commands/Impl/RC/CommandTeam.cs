using System;

namespace Guardian.Features.Commands.Impl.RC
{
    class CommandTeam : Command
    {
        public CommandTeam() : base("team", new string[0], "<0,individual/1,cyan/2,magenta>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (RCSettings.TeamMode == 1)
            {
                if (args.Length < 1) return;

                if (args[0].Equals("1") || args[0].Equals("cyan", StringComparison.OrdinalIgnoreCase))
                {
                    FengGameManagerMKII.Instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 1);
                    irc.AddLine("You have joined team Cyan.".AsColor("00FFFF"));

                    HERO hero = PhotonNetwork.player.GetHero();
                    if (hero == null || hero.HasDied()) return;

                    hero.MarkDead();
                    hero.photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                }
                else if (args[0].Equals("2") || args[0].Equals("magenta", StringComparison.OrdinalIgnoreCase))
                {
                    FengGameManagerMKII.Instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 2);
                    irc.AddLine("You have joined team Magenta.".AsColor("FF00FF"));

                    HERO hero = PhotonNetwork.player.GetHero();
                    if (hero == null || hero.HasDied()) return;

                    hero.MarkDead();
                    hero.photonView.RPC("netDie2", PhotonTargets.All, -1, "Team Switch");
                }
                else if (args[0].Equals("0") || args[0].Equals("individual", StringComparison.OrdinalIgnoreCase))
                {
                    FengGameManagerMKII.Instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 0);
                    irc.AddLine("You have joined team Individual.".AsColor("FFFFFF"));

                    HERO hero = PhotonNetwork.player.GetHero();
                    if (hero == null || hero.HasDied()) return;

                    hero.MarkDead();
                    hero.photonView.RPC("netDie2", PhotonTargets.All, -1, "Team Switch");
                }
                else
                {
                    irc.AddLine("Invalid team. Accepted values are 0, 1, and 2.".AsColor("FFCC00"));
                }
            }
            else
            {
                irc.AddLine("Teams are either locked or disabled.".AsColor("FFCC00"));
            }
        }
    }
}
