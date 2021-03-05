
using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;

namespace Guardian.Utilities
{
    class Translator
    {
        public static IEnumerator Translate(string text, string langCodeFrom, string langCodeTo, Action<string[]> callback)
        {
            var query = WWW.EscapeURL(text);
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={langCodeFrom}&tl={langCodeTo}&dt=t&q={query}";

            using (var www = new WWW(url))
            {
                yield return www;

                if (www.error != null)
                {
                    callback.Invoke(new string[] { www.error });
                }
                else
                {
                    var array = JSONNode.Parse(www.text).AsArray;
                    callback.Invoke(new string[]
                    {
                        array[2].Value,
                        array[0].AsArray[0].AsArray[0].Value
                    });
                }
            }
        }
    }
}
