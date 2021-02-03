using System;
using UnityEngine;
using SimpleJSON;

namespace Guardian.Utilities
{
    class Translator
    {
        public string OriginalText = string.Empty;
        public string TranslatedText { get; private set; }

        public string LanguageFrom = "auto";
        public string LanguageTo = string.Empty;

        public bool Get()
        {
            string query = Uri.EscapeDataString(OriginalText);

            using (WWW www = new WWW($"https://translate.googleapis.com/translate_a/single?client=gtx&sl={LanguageFrom}&tl={LanguageTo}&dt=t&q={query}"))
            {
                while (!www.isDone) { }

                if (www.error != null)
                {
                    return false;
                }

                JSONArray array = JSONNode.Parse(www.text).AsArray;
                LanguageFrom = array[2].Value;
                TranslatedText = array[0].AsArray[0].AsArray[0].Value;
            }

            return true;
        }
    }
}
