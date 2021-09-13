using UnityEngine;
using System.Collections.Generic;

namespace Guardian.Utilities
{
    class Gesources
    {
        public static Dictionary<string, object> Cache = new Dictionary<string, object>();

        public static T Find<T>(string path)
        {
            if (!Cache.TryGetValue(path, out object res))
            {
                using WWW www = new WWW("file:///" + Application.streamingAssetsPath + $"/{path}");
                while (!www.isDone) { }

                if (www.error != null)
                {
                    return default(T);
                }

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

                Cache.Add(path, res);
            }

            return (T)res;
        }

        public static Vector2 Scale(Texture image, int originalWidth, int originalHeight)
        {
            return new Vector2(image.width / (float)originalWidth, image.height / (float)originalHeight);
        }
    }
}
