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

                if (translator.Get())
                {
                    irc.AddMessage("Translation ".WithColor("ffcc00") + $"({translator.LanguageFrom} -> {translator.LanguageTo})", translator.TranslatedText);
                }
                else
                {
                    irc.AddLine("An error occured trying to retrieve the translation.".WithColor("ff0000"));
                }
            }
        }
    }
}