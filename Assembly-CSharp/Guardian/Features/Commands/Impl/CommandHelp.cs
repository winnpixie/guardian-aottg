namespace Guardian.Features.Commands.Impl
{
    class CommandHelp : Command
    {
        private int CommandsPerPage = 7;

        public CommandHelp() : base("help", new string[] { "?", "commands" }, "[page/command]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            int page = 0;
            int pages = Utilities.MathHelper.Ceil((float)GuardianClient.Commands.Elements.Count / CommandsPerPage);

            if (args.Length > 0)
            {
                Command command = GuardianClient.Commands.Find(args[0]);

                if (command != null)
                {
                    irc.AddLine($"Help for command '{command.Name}':".AsColor("AAFF00").AsBold());
                    irc.AddLine($"Usage: /{command.Name} {command.Usage}");
                    irc.AddLine($"Aliases: [{string.Join(", ", command.Aliases)}]");
                    return;
                }
                else if (int.TryParse(args[0], out int thePage))
                {
                    page = Utilities.MathHelper.Clamp(thePage, 1, pages) - 1;
                }
            }

            irc.AddLine("For general help regarding Guardian, visit".AsColor("FFFF00"));
            irc.AddLine("\thttps://winnpixie.github.io/guardian/".AsColor("0099FF") + "!".AsColor("FFFF00"));

            irc.AddLine($"Commands (Page {page + 1}/{pages})".AsColor("AAFF00").AsBold());
            irc.AddLine("<arg> = Required, [arg] = Optional".AsColor("AAAAAA").AsBold());

            for (int i = 0; i < CommandsPerPage; i++)
            {
                int index = i + (page * CommandsPerPage);

                if (index >= GuardianClient.Commands.Elements.Count)
                {
                    break;
                }

                Command command = GuardianClient.Commands.Elements[index];
                string msg = "> ".AsColor("00FF00").AsBold() + $"/{command.Name} {command.Usage}";

                if (command.MasterClient)
                {
                    msg += " [MC]".AsColor("FF0000").AsBold();
                }
                if (command.GetType().Namespace.EndsWith("Debug"))
                {
                    msg += " [DBG]".AsColor("AAAAAA").AsBold();
                }

                irc.AddLine(msg);
            }
        }
    }
}
