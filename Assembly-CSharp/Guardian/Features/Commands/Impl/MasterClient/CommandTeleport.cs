using System;
using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandTeleport : Command
    {
        public CommandTeleport() : base("teleport", new string[] { "tp" }, "<id/all/x y z> [id/x y z]", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (FengGameManagerMKII.Level.Mode == GameMode.Racing)
            {
                irc.AddLine("Teleport can NOT be used while in Racing.".AsColor("FF0000"));
                return;
            }

            if (args.Length > 3) // Player(s) -> Coordinate
            {
                if (!float.TryParse(args[1], out float x) || !float.TryParse(args[2], out float y) || !float.TryParse(args[3], out float z)) return;

                if (args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        Photon.MonoBehaviour mb = player.IsTitan ? (Photon.MonoBehaviour)player.GetTitan() : (Photon.MonoBehaviour)player.GetHero();
                        if (mb == null) continue;

                        mb.photonView.RPC("moveToRPC", player, x, y, z);
                    }

                    GameHelper.Broadcast($"Teleported everyone to {x:F3} / {y:F3} / {z:F3}");
                }
                else if (int.TryParse(args[0], out int id))
                {
                    PhotonPlayer player = PhotonPlayer.Find(id);
                    if (player == null) return;

                    Photon.MonoBehaviour mb = player.IsTitan ? (Photon.MonoBehaviour)player.GetTitan() : (Photon.MonoBehaviour)player.GetHero();
                    if (mb == null) return;

                    mb.photonView.RPC("moveToRPC", player, x, y, z);
                    FengGameManagerMKII.Instance.photonView.RPC("Chat", player, $"Teleported you to {x:F3} {y:F3} {z:F3}", string.Empty);
                }
            }
            else if (args.Length > 2) // You -> Coordinate
            {
                if (!float.TryParse(args[0], out float x) || !float.TryParse(args[1], out float y) || !float.TryParse(args[2], out float z)) return;

                Photon.MonoBehaviour mb = PhotonNetwork.player.IsTitan ? (Photon.MonoBehaviour)PhotonNetwork.player.GetTitan() : (Photon.MonoBehaviour)PhotonNetwork.player.GetHero();
                if (mb == null) return;

                mb.transform.position = new UnityEngine.Vector3(x, y, z);
                irc.AddLine($"Teleported you to {x:F3} {y:F3} {z:F3}");
            }
            else if (args.Length > 1) // Player(s) -> Target
            {
                if (!int.TryParse(args[1], out int targetId)) return;

                PhotonPlayer target = PhotonPlayer.Find(targetId);
                if (target == null) return;

                Photon.MonoBehaviour targetMb = target.IsTitan ? (Photon.MonoBehaviour)target.GetTitan() : (Photon.MonoBehaviour)target.GetHero();
                if (targetMb == null) return;

                if (args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        Photon.MonoBehaviour mb = player.IsTitan ? (Photon.MonoBehaviour)player.GetTitan() : (Photon.MonoBehaviour)player.GetHero();
                        if (mb == null) continue;

                        mb.photonView.RPC("moveToRPC", player, targetMb.transform.position.x, targetMb.transform.position.y, targetMb.transform.position.z);
                    }

                    GameHelper.Broadcast($"Teleported everyone to #{targetId}");
                }
                else if (int.TryParse(args[0], out int id))
                {
                    PhotonPlayer player = PhotonPlayer.Find(id);
                    if (player == null) return;

                    Photon.MonoBehaviour mb = player.IsTitan ? (Photon.MonoBehaviour)player.GetTitan() : (Photon.MonoBehaviour)player.GetHero();
                    if (mb == null) return;

                    mb.photonView.RPC("moveToRPC", player, targetMb.transform.position.x, targetMb.transform.position.y, targetMb.transform.position.z);
                    FengGameManagerMKII.Instance.photonView.RPC("Chat", player, $"Teleported you to #{targetId}", string.Empty);
                }
            }
            else if (args.Length > 0)// All -> You or You -> Target
            {
                Photon.MonoBehaviour myMb = PhotonNetwork.player.IsTitan ? (Photon.MonoBehaviour)PhotonNetwork.player.GetTitan() : (Photon.MonoBehaviour)PhotonNetwork.player.GetHero();
                if (myMb == null) return;

                if (args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        Photon.MonoBehaviour mb = player.IsTitan ? (Photon.MonoBehaviour)player.GetTitan() : (Photon.MonoBehaviour)player.GetHero();
                        if (mb == null) continue;

                        mb.photonView.RPC("moveToRPC", player, myMb.transform.position.x, myMb.transform.position.y, myMb.transform.position.z);
                    }

                    GameHelper.Broadcast("Teleported everyone to MasterClient!");
                }
                else if (int.TryParse(args[0], out int id))
                {
                    PhotonPlayer player = PhotonPlayer.Find(id);
                    if (player == null) return;

                    Photon.MonoBehaviour mb = player.IsTitan ? (Photon.MonoBehaviour)player.GetTitan() : (Photon.MonoBehaviour)player.GetHero();
                    if (mb == null) return;

                    myMb.transform.position = mb.transform.position;
                    irc.AddLine($"Teleported you to #{id}");
                }
            }
        }
    }
}
