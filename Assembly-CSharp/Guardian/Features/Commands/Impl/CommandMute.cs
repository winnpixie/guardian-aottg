namespace Guardian.Features.Commands.Impl
{
    class CommandMute : Command
    {
        public CommandMute() : base("mute", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int id)) return;

            PhotonPlayer player = PhotonPlayer.Find(id);
            if (player == null || player.Muted) return;

            player.Muted = true;
            irc.AddLine($"Ignoring chat messages from #{id}.");
        }
    }
}
