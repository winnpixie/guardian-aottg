using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;

namespace Guardian.Utilities
{
    class Translator
    {
        // private static readonly string _apiUrl = "https://clients5.google.com/translate_a/t?client=dict-chrome-ex&sl={0}&tl={1}&q={2}";

        public static IEnumerator Translate(string text, string langCodeFrom, string langCodeTo, Action<string[]> callback)
        {
            string query = WWW.EscapeURL(text);
            string url = $"https://translate.googleapis.com/translate_a/single?client=dict-chrome-ex&sl={langCodeFrom}&tl={langCodeTo}&dt=t&q={query}";

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

        public static string[] Translate(string text, string langCodeFrom, string langCodeTo)
        {
            string query = WWW.EscapeURL(text);
            string url = $"https://translate.googleapis.com/translate_a/single?client=dict-chrome-ex&sl={langCodeFrom}&tl={langCodeTo}&dt=t&q={query}";

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
