namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandSetMaster : Command
    {
        public CommandSetMaster() : base("setmc", new string[] { "setmaster", "passmc", "passmaster" }, "<id>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                PhotonPlayer player = PhotonPlayer.Find(id);

                if (player != null && !player.isLocal)
                {
                    PhotonNetwork.SetMasterClient(player);
                }
            }
        }
    }
}
