using System;

namespace Guardian.Features.Commands.Impl
{
    class CommandHelp : Command
    {
        public CommandHelp() : base("help", new string[] { "?", "commands" }, "[page]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            int page = 1;

            if (args.Length > 0 && int.TryParse(args[0], out page))
            {
                if (page < 1)
                {
                    page = 1;
                }
                if (page > Mod.Commands.Pages)
                {
                    page = Mod.Commands.Pages;
                }
            }
            int endIndex = Math.Min(page * Mod.Commands.PerPage, Mod.Commands.Elements.Count);

            irc.AddLine($"Commands (Page {page}/{Mod.Commands.Pages})".WithColor("aaff00").AsBold());
            irc.AddLine($"<arg> = Required, [arg] = Optional".WithColor("aaaaaa").AsBold());
            for (int i = (page - 1) * Mod.Commands.PerPage; i < endIndex; i++)
            {
                Command command = Mod.Commands.Elements[i];
                string msg = "> ".WithColor("00ff00").AsBold() + $"/{command.Name} {command.usage}";
                if (command.masterClient)
                {
                    msg += " [MC]".WithColor("ff0000").AsBold();
                }
                if (command.GetType().Namespace.EndsWith("Debug"))
                {
                    msg += " [Debug]".WithColor("aaaaa").AsBold();
                }
                irc.AddLine(msg);
            }
        }
    }
}
