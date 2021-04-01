using System;

namespace Guardian.Features.Commands.Impl
{
    class CommandHelp : Command
    {
        public CommandHelp() : base("help", new string[] { "?", "commands" }, "[page/command]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            int maxPages = (Mod.Commands.Elements.Count - 1) / 7;
            int page = 0;

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
                    page = thePage;
                    if (page < 1)
                    {
                        page = 1;
                    }
                    if (page > maxPages + 1)
                    {
                        page = maxPages + 1;
                    }
                    page -= 1;
                }
            }

            int endIndex = Math.Min((page + 1) * 7, Mod.Commands.Elements.Count);
            irc.AddLine($"Commands (Page {page + 1}/{maxPages + 1})".WithColor("AAFF00").AsBold());
            irc.AddLine("<arg> = Required, [arg] = Optional".WithColor("AAAAAA").AsBold());
            for (int i = page * 7; i < endIndex; ++i)
            {
                Command command = Mod.Commands.Elements[i];
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
