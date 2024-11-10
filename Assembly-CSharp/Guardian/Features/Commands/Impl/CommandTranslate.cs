using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl
{
    class CommandTranslate : Command
    {
        public CommandTranslate() : base("translate", new string[0], "<langfrom> <langto> <message>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 3) return;

            irc.StartCoroutine(Translator.TranslateRoutine(string.Join(" ", args.CopyOfRange(2, args.Length)), args[0], args[1],
                result => irc.AddMessage($"[gt::{result[0].ToLower()}->{args[1].ToLower()}]".AsColor("0099FF"), result[1]),
                error =>
                {
                    irc.AddLine("An error occured while trying to retrieve the translation!".AsColor("FF0000"));
                    irc.AddLine(error.AsColor("FF0000"));
                }));
        }
    }
}