using System;
using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandRevive : Command
    {
        public CommandRevive() : base("revive", new string[] { "heal", "respawn", "rev", "res" }, "[all/id]", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var player in PhotonNetwork.playerList)
                    {
                        if (GameHelper.IsDead(player) && !GameHelper.IsPT(player))
                        {
                            FengGameManagerMKII.Instance.photonView.RPC("respawnHeroInNewRound", player);
                        }
                    }

                    GameHelper.Broadcast("All players have been revived.");
                }
                else if (int.TryParse(args[0], out int id))
                {
                    var player = PhotonPlayer.Find(id);
                    if (player != null)
                    {
                        if (GameHelper.IsDead(player) && !GameHelper.IsPT(player))
                        {
                            FengGameManagerMKII.Instance.photonView.RPC("respawnHeroInNewRound", player);
                            irc.AddLine($"Revived #{id}.");
                        }
                    }
                }
            }
            else if (GameHelper.IsDead(PhotonNetwork.player) && !GameHelper.IsPT(PhotonNetwork.player))
            {
                FengGameManagerMKII.Instance.photonView.RPC("respawnHeroInNewRound", PhotonNetwork.player);
                irc.AddLine("Revived self.");
            }
        }
    }
}
