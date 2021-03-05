namespace Guardian.Features.Commands.Impl
{
    class CommandMute : Command
    {
        public CommandMute() : base("mute", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                var player = PhotonPlayer.Find(id);
                if (player != null)
                {
                    if (!Mod.Instance.Muted.Contains(id))
                    {
                        Mod.Instance.Muted.Add(id);
                        irc.AddLine($"Ignoring chat messages from #{id}.");
                    }
                }
            }
        }
    }
}
