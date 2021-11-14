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
                if (float.TryParse(args[1], out float x) && float.TryParse(args[2], out float y) && float.TryParse(args[3], out float z))
                {
                    if (args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (PhotonPlayer player in PhotonNetwork.playerList)
                        {
                            Photon.MonoBehaviour mb = GameHelper.IsPT(player) ? (Photon.MonoBehaviour)GameHelper.GetPT(player)
                                : (Photon.MonoBehaviour)GameHelper.GetHero(player);

                            if (mb != null)
                            {
                                mb.photonView.RPC("moveToRPC", player, x, y, z);
                            }
                        }

                        GameHelper.Broadcast($"Teleported everyone to {x:F3} / {y:F3} / {z:F3}");
                    }
                    else if (int.TryParse(args[0], out int id))
                    {
                        PhotonPlayer player = PhotonPlayer.Find(id);
                        if (player != null)
                        {
                            Photon.MonoBehaviour mb = GameHelper.IsPT(player) ? (Photon.MonoBehaviour)GameHelper.GetPT(player)
                                : (Photon.MonoBehaviour)GameHelper.GetHero(player);

                            if (mb != null)
                            {
                                mb.photonView.RPC("moveToRPC", player, x, y, z);

                                FengGameManagerMKII.Instance.photonView.RPC("Chat", player, $"Teleported you to {x:F3} {y:F3} {z:F3}", string.Empty);
                            }
                        }
                    }
                }
            }
            else if (args.Length > 2) // You -> Coordinate
            {
                if (float.TryParse(args[0], out float x) && float.TryParse(args[1], out float y) && float.TryParse(args[2], out float z))
                {
                    Photon.MonoBehaviour mb = GameHelper.IsPT(PhotonNetwork.player) ? (Photon.MonoBehaviour)GameHelper.GetPT(PhotonNetwork.player)
                        : (Photon.MonoBehaviour)GameHelper.GetHero(PhotonNetwork.player);

                    if (mb != null)
                    {
                        mb.transform.position = new UnityEngine.Vector3(x, y, z);

                        irc.AddLine($"Teleported you to {x:F3} {y:F3} {z:F3}");
                    }
                }
            }
            else if (args.Length > 1) // Player(s) -> Target
            {
                if (int.TryParse(args[1], out int targetId))
                {
                    PhotonPlayer target = PhotonPlayer.Find(targetId);

                    if (target != null)
                    {
                        Photon.MonoBehaviour targetMb = GameHelper.IsPT(target) ? (Photon.MonoBehaviour)GameHelper.GetPT(target)
                            : (Photon.MonoBehaviour)GameHelper.GetHero(target);
                        if (targetMb != null)
                        {
                            if (args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
                            {
                                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                                {
                                    Photon.MonoBehaviour mb = GameHelper.IsPT(player) ? (Photon.MonoBehaviour)GameHelper.GetPT(player)
                                        : (Photon.MonoBehaviour)GameHelper.GetHero(player);

                                    if (mb != null)
                                    {
                                        mb.photonView.RPC("moveToRPC", player, targetMb.transform.position.x, targetMb.transform.position.y, targetMb.transform.position.z);
                                    }
                                }

                                GameHelper.Broadcast($"Teleported everyone to #{targetId}");
                            }
                            else if (int.TryParse(args[0], out int id))
                            {
                                PhotonPlayer player = PhotonPlayer.Find(id);
                                if (player != null)
                                {
                                    Photon.MonoBehaviour mb = GameHelper.IsPT(player) ? (Photon.MonoBehaviour)GameHelper.GetPT(player)
                                        : (Photon.MonoBehaviour)GameHelper.GetHero(player);

                                    if (mb != null)
                                    {
                                        mb.photonView.RPC("moveToRPC", player, targetMb.transform.position.x, targetMb.transform.position.y, targetMb.transform.position.z);
                                    }

                                    FengGameManagerMKII.Instance.photonView.RPC("Chat", player, $"Teleported you to #{targetId}", string.Empty);
                                }
                            }
                        }
                    }
                }
            }
            else if (args.Length > 0) // All -> You or You -> Target
            {
                Photon.MonoBehaviour myMb = GameHelper.IsPT(PhotonNetwork.player) ? (Photon.MonoBehaviour)GameHelper.GetPT(PhotonNetwork.player)
                     : (Photon.MonoBehaviour)GameHelper.GetHero(PhotonNetwork.player);

                if (myMb != null)
                {
                    if (args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (PhotonPlayer player in PhotonNetwork.playerList)
                        {
                            Photon.MonoBehaviour mb = GameHelper.IsPT(player) ? (Photon.MonoBehaviour)GameHelper.GetPT(player)
                                : (Photon.MonoBehaviour)GameHelper.GetHero(player);

                            if (mb != null)
                            {
                                mb.photonView.RPC("moveToRPC", player, myMb.transform.position.x, myMb.transform.position.y, myMb.transform.position.z);
                            }
                        }

                        GameHelper.Broadcast("Teleported everyone to MasterClient!");
                    }
                    else if (int.TryParse(args[0], out int id))
                    {
                        PhotonPlayer player = PhotonPlayer.Find(id);
                        if (player != null)
                        {
                            Photon.MonoBehaviour mb = GameHelper.IsPT(player) ? (Photon.MonoBehaviour)GameHelper.GetPT(player)
                                : (Photon.MonoBehaviour)GameHelper.GetHero(player);

                            if (mb != null)
                            {
                                myMb.transform.position = mb.transform.position;
                            }

                            irc.AddLine($"Teleported to #{id}");
                        }
                    }
                }
            }
        }
    }
}
