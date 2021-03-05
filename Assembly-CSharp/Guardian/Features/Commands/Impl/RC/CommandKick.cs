namespace Guardian.Features.Commands.Impl.RC
{
    class CommandKick : Command
    {
        public CommandKick() : base("kick", new string[0], "<id> [reason]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                if (id != PhotonNetwork.player.Id)
                {
                    if (!(FengGameManagerMKII.OnPrivateServer || PhotonNetwork.isMasterClient))
                    {
                        var name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]).Colored();
                        if (name == string.Empty)
                        {
                            name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]);
                        }
                        FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, "/kick #" + id, name);
                    }
                    else
                    {
                        var player = PhotonPlayer.Find(id);
                        if (player != null)
                        {
                            var reason = args.Length > 1 ? string.Join(" ", args.CopyOfRange(1, args.Length)) : "Kicked.";
                            if (FengGameManagerMKII.OnPrivateServer)
                            {
                                FengGameManagerMKII.Instance.KickPlayer(player, false, reason);
                            }
                            else if (PhotonNetwork.isMasterClient)
                            {
                                FengGameManagerMKII.Instance.KickPlayer(player, false, reason);
                                FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).Colored()
                                    + " has been kicked from the server!".WithColor("FFCC00"), string.Empty);
                                FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, $"Reason: {reason}".WithColor("FFCC00"), string.Empty);
                            }
                        }
                    }
                }
            }
        }
    }
}
