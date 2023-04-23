using SimpleJson;
using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

namespace Guardian.Utilities
{
    class Translator
    {
        public static string SystemLanguage => CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        //private static readonly string ApiUrl = "https://clients5.google.com/translate_a/t?client=dict-chrome-ex&sl={0}&tl={1}&q={2}"; // Alt-URL
        private static readonly string ApiUrl = "https://translate.googleapis.com/translate_a/single?client=dict-chrome-ex&sl={0}&tl={1}&dt=t&q={2}";

        public static IEnumerator TranslateRoutine(string text, string langFrom, string langTo, Action<string[]> callback)
        {
            string query = WWW.EscapeURL(text);
            string url = string.Format(ApiUrl, langFrom, langTo, query);

            using WWW www = new WWW(url);
            yield return www;

            if (www.error != null)
            {
                callback.Invoke(new string[] { www.error });
            }
            else
            {
                JSONArray json = JSON.Parse(www.text).AsArray;

                callback.Invoke(new string[]
                {
                        json[2].Value, // Language
                        json[0].AsArray[0].AsArray[0].Value // Text
                });
            }
        }

        public static string[] Translate(string text, string langFrom, string langTo)
        {
            string query = WWW.EscapeURL(text);
            string url = string.Format(ApiUrl, langFrom, langTo, query);

            using WWW www = new WWW(url);
            while (!www.isDone) { }

            if (www.error != null)
            {
                return new string[] { www.error };
            }
            else
            {
                JSONArray json = JSON.Parse(www.text).AsArray;
                return new string[]
                {
                    json[2].Value, // Language
                    json[0].AsArray[0].AsArray[0].Value // Text
                };
            }
        }
    }
}
