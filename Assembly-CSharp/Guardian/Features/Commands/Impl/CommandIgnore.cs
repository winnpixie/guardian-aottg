namespace Guardian.Features.Commands.Impl
{
    class CommandIgnore : Command
    {
        public CommandIgnore() : base("ignore", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                if (!FengGameManagerMKII.IgnoreList.Contains(id))
                {
                    FengGameManagerMKII.IgnoreList.Add(id);
                    irc.AddLine($"Now ignoring data from #{id}.".WithColor("ffcc00"));
                }
            }
        }
    }
}
