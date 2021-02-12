using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl
{
    class CommandTranslate : Command
    {
        private Translator translator = new Translator();

        public CommandTranslate() : base("translate", new string[0], "<langfrom> <langto> <message>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 2)
            {
                translator.LanguageFrom = args[0];
                translator.LanguageTo = args[1];
                translator.OriginalText = string.Join(" ", args.CopyOfRange(2, args.Length));

                new System.Threading.Thread(() =>
                {
                    if (translator.Get())
                    {
                        irc.AddMessage("Translation ".WithColor("FFCC00") + $"({translator.LanguageFrom} -> {translator.LanguageTo})", translator.TranslatedText);
                    }
                    else
                    {
                        irc.AddLine("An error occured while trying to retrieve the translation!".WithColor("FF0000"));
                    }
                }).Start();
            }
        }
    }
}