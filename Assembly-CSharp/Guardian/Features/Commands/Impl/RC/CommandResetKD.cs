using Guardian.Utilities;
using System;

namespace Guardian.Features.Commands.Impl.RC
{
    class CommandResetKD : Command
    {
        public CommandResetKD() : base("resetkd", new string[0], "[all|id]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable()
            {
                { PhotonPlayerProperty.Kills, 0 },
                { PhotonPlayerProperty.Deaths, 0 },
                { PhotonPlayerProperty.MaxDamage, 0 },
                { PhotonPlayerProperty.TotalDamage, 0 }
            };

            if (args.Length > 0)
            {
                if (!PhotonNetwork.isMasterClient)
                {
                    irc.AddLine("Command requires master client!".AsColor("FF0000"));
                    return;
                }

                if (args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        player.SetCustomProperties(properties);
                    }

                    GameHelper.Broadcast("All stats have been reset.");
                }
                else if (int.TryParse(args[0], out int id))
                {
                    PhotonPlayer player = PhotonPlayer.Find(id);
                    if (player == null) return;

                    player.SetCustomProperties(properties);
                    irc.AddLine($"You reset #{id}'s stats.".AsColor("FFCC00"));
                }
            }
            else
            {
                PhotonNetwork.player.SetCustomProperties(properties);
                irc.AddLine("Your stats have been reset.".AsColor("FFCC00"));
            }
        }
    }
}
