namespace Guardian.Features.Commands.Impl.Debug
{
    class CommandRPC : Command
    {
        public CommandRPC() : base("rpc", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 1 && int.TryParse(args[0], out int id))
            {
                PhotonPlayer player = PhotonPlayer.Find(id);
                if (player != null)
                {
                    string rpcName = string.Join("_", args.GetRange(1, args.Length));
                    FengGameManagerMKII.Instance.photonView.RPC(rpcName, player);
                    irc.AddLine($"Sent RPC '{rpcName}' to #{id}.");
                }
            }
        }
    }
}
