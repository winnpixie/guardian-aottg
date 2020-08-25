namespace Guardian.Features.Commands.Impl.RC
{
    class CommandBan : Command
    {
        public CommandBan() : base("ban", new string[0], "<id> [reason]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                if (id != PhotonNetwork.player.id)
                {
                    if (!(FengGameManagerMKII.OnPrivateServer || PhotonNetwork.isMasterClient))
                    {
                        string name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]).Colored();
                        if (name == string.Empty)
                        {
                            name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]);
                        }
                        FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, "/kick #" + id, name);
                    }
                    else
                    {
                        PhotonPlayer player = PhotonPlayer.Find(id);
                        if (player != null)
                        {
                            string reason = args.Length > 1 ? string.Join(" ", args.CopyOfRange(1, args.Length)) : "Banned.";
                            if (FengGameManagerMKII.OnPrivateServer)
                            {
                                FengGameManagerMKII.Instance.KickPlayer(player, true, reason);
                            }
                            else if (PhotonNetwork.isMasterClient)
                            {
                                FengGameManagerMKII.Instance.KickPlayer(player, true, reason);
                                FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).Colored() + " has been banned from the server!".WithColor("ffcc00"), string.Empty);
                                FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, $"Reason: {reason}".WithColor("ffcc00"), string.Empty);
                            }
                        }
                    }
                }
            }
        }
    }
}
