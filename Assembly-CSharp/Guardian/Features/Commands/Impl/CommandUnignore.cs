namespace Guardian.Features.Commands.Impl
{
    class CommandUnignore : Command
    {
        public CommandUnignore() : base("unignore", new string[] { "unig" }, "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                if (FengGameManagerMKII.IgnoreList.Contains(id))
                {
                    FengGameManagerMKII.IgnoreList.Remove(id);
                    irc.AddLine($"No longer ignoring data from #{id}.".WithColor("ffcc00"));
                }
            }
        }
    }
}
