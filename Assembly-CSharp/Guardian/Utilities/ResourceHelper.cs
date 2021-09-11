using UnityEngine;
using System.Collections.Generic;

namespace Guardian.Utilities
{
    class ResourceHelper
    {
        private static Dictionary<string, object> s_resourceCache = new Dictionary<string, object>();

        public static T Find<T>(string path)
        {
            if (!s_resourceCache.TryGetValue(path, out object res))
            {
                using WWW www = new WWW("file:///" + Application.streamingAssetsPath + $"/{path}");
                while (!www.isDone) { }

                System.Type type = typeof(T);
                if (typeof(MovieTexture).IsAssignableFrom(type))
                {
                    res = www.movie;
                }
                else if (typeof(Texture).IsAssignableFrom(type))
                {
                    res = www.texture;
                }
                else if (typeof(AudioClip).IsAssignableFrom(type))
                {
                    res = www.audioClip;
                }
                else if (typeof(string).IsAssignableFrom(type))
                {
                    res = www.text;
                }
                else
                {
                    // When in doubt, return raw bytes
                    res = www.bytes;
                }

                s_resourceCache.Add(path, res);
            }

            return (T)res;
        }
    }
}
