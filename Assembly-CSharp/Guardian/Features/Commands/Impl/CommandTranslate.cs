using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl
{
    class CommandTranslate : Command
    {
        public CommandTranslate() : base("translate", new string[0], "<langfrom> <langto> <message>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 3) return;

            irc.StartCoroutine(Translator.TranslateRoutine(string.Join(" ", args.CopyOfRange(2, args.Length)), args[0], args[1], result =>
            {
                if (result.Length > 1)
                {
                    irc.AddMessage("Translation ".AsColor("FFCC00") + $"({result[0].ToUpper()}->{args[1].ToUpper()})", result[1]);
                }
                else
                {
                    irc.AddLine("An error occured while trying to retrieve the translation!".AsColor("FF0000"));
                    irc.AddLine(result[0].AsColor("FF0000"));
                }
            }));
        }
    }
}