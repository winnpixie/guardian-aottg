using Guardian.Utilities;
using SimpleJSON;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Guardian.Features.Commands.Impl
{
    class CommandTranslate : Command
    {
        private Regex NonLetters = new Regex("[~!@#$%^&*()_+`\\-=\\[\\]{}\\|;:'\",<.>\\/?]+", RegexOptions.IgnoreCase);

        public CommandTranslate() : base("translate", new string[0], "<message>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                string culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = GameHelper.Detagger.Replace(args[i], "");
                    args[i] = NonLetters.Replace(args[i], "");
                }
                string query = Uri.EscapeDataString(string.Join(" ", args));
                using (WWW www = new WWW($"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={culture}&dt=t&q={query}"))
                {
                    while (!www.isDone) { }
                    JSONArray array = JSONNode.Parse(www.text).AsArray;
                    JSONArray dataArray = array[0].AsArray;
                    string detectedLanguage = array[2].Value;
                    string message = dataArray[0].AsArray[0].Value;
                    irc.AddLine($"<color=#ffcc00>Translation</color> ({detectedLanguage.ToUpper()} -> {culture.ToUpper()}): {message}");
                }
            }
        }
    }
}