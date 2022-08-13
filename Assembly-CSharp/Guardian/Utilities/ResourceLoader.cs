using UnityEngine;
using System.Collections.Generic;

namespace Guardian.Utilities
{
    class ResourceLoader
    {
        public static Dictionary<string, object> AssetCache = new Dictionary<string, object>();

        public static bool TryGetAsset<T>(string path, out T value)
        {
            path = "file:///" + Application.streamingAssetsPath + $"/{path}";

            return TryGet(path, out value);
        }

        public static bool TryGet<T>(string path, out T value)
        {
            if (AssetCache.TryGetValue(path, out object cachedValue))
            {
                value = (T)cachedValue;
                return true;
            }

            if (TryGetRaw(path, out value))
            {
                AssetCache.Add(path, value);
                return true;
            }

            value = default;
            return false;
        }

        public static bool TryGetRaw<T>(string path, out T value)
        {
            value = default;
            using WWW www = new WWW(path);
            while (!www.isDone) { }

            if (www.error != null) return false;

            System.Type assetType = typeof(T);
            object val = www.bytes;
            if (typeof(MovieTexture).IsAssignableFrom(assetType))
            {
                val = www.movie;
            }
            else if (typeof(Texture).IsAssignableFrom(assetType))
            {
                val = www.texture;
            }
            else if (typeof(AudioClip).IsAssignableFrom(assetType))
            {
                val = www.audioClip;
            }
            else if (typeof(AssetBundle).IsAssignableFrom(assetType))
            {
                val = www.assetBundle;
            }
            else if (typeof(string).IsAssignableFrom(assetType))
            {
                val = www.text;
            }

            value = (T)val;
            return true;
        }
    }
}
