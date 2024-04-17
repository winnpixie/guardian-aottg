namespace Guardian.Features.Commands.Impl
{
    class CommandUnignore : Command
    {
        public CommandUnignore() : base("unignore", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int id)
                || !FengGameManagerMKII.IgnoreList.Contains(id)) return;

            FengGameManagerMKII.IgnoreList.Remove(id);
            irc.AddLine($"No longer ignoring events from #{id}.".AsColor("FFCC00"));
        }
    }
}
