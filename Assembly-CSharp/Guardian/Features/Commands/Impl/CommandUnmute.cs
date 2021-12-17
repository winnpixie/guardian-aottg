namespace Guardian.Features.Commands.Impl
{
    class CommandUnmute : Command
    {
        public CommandUnmute() : base("unmute", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int id)) return;

            PhotonPlayer player = PhotonPlayer.Find(id);
            if (player == null || !player.Muted) return;

            player.Muted = false;
            irc.AddLine($"No longer ignoring chat messages from #{id}.");
        }
    }
}
