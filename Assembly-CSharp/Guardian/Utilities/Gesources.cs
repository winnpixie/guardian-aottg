using UnityEngine;
using System.Collections.Generic;

namespace Guardian.Utilities
{
    class Gesources
    {
        public static Dictionary<string, object> Cache = new Dictionary<string, object>();

        public static bool TryGetAsset<T>(string path, out T value)
        {
            path = "file:///" + Application.streamingAssetsPath + $"/{path}";

            return TryGet(path, out value);
        }

        public static bool TryGet<T>(string path, out T value)
        {
            if (Cache.TryGetValue(path, out object cachedValue))
            {
                value = (T)cachedValue;
                return true;
            }

            if (TryGetRaw(path, out value))
            {
                Cache.Add(path, value);
                return true;
            }

            value = default(T);
            return false;
        }

        public static bool TryGetRaw<T>(string path, out T value)
        {
            using WWW www = new WWW(path);
            while (!www.isDone) { }

            if (www.error != null)
            {
                value = default(T);
                return false;
            }

            object val;
            System.Type type = typeof(T);
            if (typeof(MovieTexture).IsAssignableFrom(type))
            {
                val = www.movie;
            }
            else if (typeof(Texture).IsAssignableFrom(type))
            {
                val = www.texture;
            }
            else if (typeof(AudioClip).IsAssignableFrom(type))
            {
                val = www.audioClip;
            }
            else if (typeof(string).IsAssignableFrom(type))
            {
                val = www.text;
            }
            else
            {
                // When in doubt, return raw bytes
                val = www.bytes;
            }

            value = (T)val;
            return true;
        }

        public static Vector2 Scale(Texture image, int originalWidth, int originalHeight)
        {
            return new Vector2(image.width / (float)originalWidth, image.height / (float)originalHeight);
        }
    }
}
