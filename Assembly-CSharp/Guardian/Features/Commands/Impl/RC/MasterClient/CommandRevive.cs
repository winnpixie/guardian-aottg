using System;
using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.RC.MasterClient
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
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        if (!player.IsDead || player.IsTitan) continue;

                        FengGameManagerMKII.Instance.photonView.RPC("respawnHeroInNewRound", player);
                    }

                    GameHelper.Broadcast("All players have been revived.");
                }
                else if (int.TryParse(args[0], out int id))
                {
                    PhotonPlayer player = PhotonPlayer.Find(id);
                    if (player == null || !player.IsDead || player.IsTitan) return;

                    FengGameManagerMKII.Instance.photonView.RPC("respawnHeroInNewRound", player);
                    irc.AddLine($"Revived #{id}.");
                }
            }
            else if (PhotonNetwork.player.IsDead && !PhotonNetwork.player.IsTitan)
            {
                FengGameManagerMKII.Instance.photonView.RPC("respawnHeroInNewRound", PhotonNetwork.player);
                irc.AddLine("Revived self.");
            }
        }
    }
}
