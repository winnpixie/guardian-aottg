using System;
using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.RC
{
    class CommandTeam : Command
    {
        public CommandTeam() : base("team", new string[0], "<0,individual/1,cyan/2,magenta>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (RCSettings.TeamMode == 1)
            {
                if (args.Length > 0)
                {
                    if (args[0].Equals("1") || args[0].Equals("cyan", StringComparison.OrdinalIgnoreCase))
                    {
                        FengGameManagerMKII.Instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 1);
                        irc.AddLine("You have joined team Cyan.".WithColor("00FFFF"));

                        var hero = GameHelper.GetHero(PhotonNetwork.player);
                        if (hero != null)
                        {
                            hero.MarkDead();
                            hero.photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                        }
                    }
                    else if (args[0].Equals("2") || args[0].Equals("magenta", StringComparison.OrdinalIgnoreCase))
                    {
                        FengGameManagerMKII.Instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 2);
                        irc.AddLine("You have joined team Magenta.".WithColor("FF00FF"));

                        var hero = GameHelper.GetHero(PhotonNetwork.player);
                        if (hero != null)
                        {
                            hero.MarkDead();
                            hero.photonView.RPC("netDie2", PhotonTargets.All, -1, "Team Switch");
                        }
                    }
                    else if (args[0].Equals("0") || args[0].Equals("individual", StringComparison.OrdinalIgnoreCase))
                    {
                        FengGameManagerMKII.Instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 0);
                        irc.AddLine("You have joined team Individual.".WithColor("FFFFFF"));

                        var hero = GameHelper.GetHero(PhotonNetwork.player);
                        if (hero != null)
                        {
                            hero.MarkDead();
                            hero.photonView.RPC("netDie2", PhotonTargets.All, -1, "Team Switch");
                        }
                    }
                    else
                    {
                        irc.AddLine("Invalid team. Accepted values are 0, 1, and 2.".WithColor("FFCC00"));
                    }
                }
            }
            else
            {
                irc.AddLine("Teams are either locked or disabled.".WithColor("FFCC00"));
            }
        }
    }
}
