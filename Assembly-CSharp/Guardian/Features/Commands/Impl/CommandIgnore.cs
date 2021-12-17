namespace Guardian.Features.Commands.Impl
{
    class CommandIgnore : Command
    {
        public CommandIgnore() : base("ignore", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int id)
                || FengGameManagerMKII.IgnoreList.Contains(id)) return;

            FengGameManagerMKII.IgnoreList.Add(id);
            irc.AddLine($"Ignoring events from #{id}.".AsColor("FFCC00"));
        }
    }
}
