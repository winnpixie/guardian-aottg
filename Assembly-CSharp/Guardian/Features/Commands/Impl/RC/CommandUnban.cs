namespace Guardian.Features.Commands.Impl.RC
{
    class CommandUnban : Command
    {
        public CommandUnban() : base("unban", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                if (FengGameManagerMKII.OnPrivateServer)
                {
                    FengGameManagerMKII.ServerRequestUnban(id.ToString());
                }
                else if (PhotonNetwork.isMasterClient)
                {
                    if (FengGameManagerMKII.BanHash.ContainsKey(id))
                    {
                        FengGameManagerMKII.BanHash.Remove(id);
                        FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, GExtensions.AsString(FengGameManagerMKII.BanHash[id]) + " has been unbanned from the server.".WithColor("ffcc00"), string.Empty);
                    }
                    else
                    {
                        irc.AddLine($"Player #{id} is not banned.".WithColor("ffcc00"));
                    }
                }
                else
                {
                    irc.AddLine("Command requires master client.".WithColor("ff4444"));
                }
            }
        }
    }
}
