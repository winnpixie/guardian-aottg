namespace Guardian.Features.Commands.Impl
{
    class CommandHelp : Command
    {
        private int CommandsPerPage = 7;

        public CommandHelp() : base("help", new string[] { "?", "commands" }, "[page/command]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            int page = 0;
            int pages = Utilities.MathHelper.Ceil((float)Mod.Commands.Elements.Count / CommandsPerPage);

            if (args.Length > 0)
            {
                Command command = Mod.Commands.Find(args[0]);

                if (command != null)
                {
                    irc.AddLine($"Help for command '{command.Name}':".WithColor("AAFF00").AsBold());
                    irc.AddLine($"Usage: /{command.Name} {command.Usage}");
                    irc.AddLine($"Aliases: [{string.Join(", ", command.Aliases)}]");
                    return;
                }
                else if (int.TryParse(args[0], out int thePage))
                {
                    page = Utilities.MathHelper.Clamp(thePage, 1, pages) - 1;
                }
            }

            irc.AddLine($"Commands (Page {page + 1}/{pages})".WithColor("AAFF00").AsBold());
            irc.AddLine("<arg> = Required, [arg] = Optional".WithColor("AAAAAA").AsBold());

            for (int i = 0; i < CommandsPerPage; i++)
            {
                int index = i + (page * CommandsPerPage);

                if (index >= Mod.Commands.Elements.Count)
                {
                    break;
                }

                Command command = Mod.Commands.Elements[index];
                string msg = "> ".WithColor("00FF00").AsBold() + $"/{command.Name} {command.Usage}";

                if (command.MasterClient)
                {
                    msg += " [MC]".WithColor("FF0000").AsBold();
                }
                if (command.GetType().Namespace.EndsWith("Debug"))
                {
                    msg += " [DBG]".WithColor("AAAAAA").AsBold();
                }

                irc.AddLine(msg);
            }
        }
    }
}
