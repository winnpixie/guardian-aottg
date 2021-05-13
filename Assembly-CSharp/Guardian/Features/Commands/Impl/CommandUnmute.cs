namespace Guardian.Features.Commands.Impl
{
    class CommandUnmute : Command
    {
        public CommandUnmute() : base("unmute", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                PhotonPlayer player = PhotonPlayer.Find(id);
                if (player != null)
                {
                    if (InRoomChat.Ignored.Contains(player))
                    {
                        InRoomChat.Ignored.Remove(player);
                        irc.AddLine($"No longer ignoring chat messages from #{id}.");
                    }
                }
            }
        }
    }
}
