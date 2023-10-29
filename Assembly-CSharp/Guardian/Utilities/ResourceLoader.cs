using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

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
            System.Type assetType = typeof(T);

            using WWW www = new WWW(path);
            while (!www.isDone) { }

            if (www.error != null) return false;

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

        // FIXME: Performs an uncached lookup, create a synchronized dictionary?
        public static IEnumerator GetAssetRoutine<T>(string path, Action<T> callback)
        {
            path = "file:///" + Application.streamingAssetsPath + $"/{path}";

            yield return GetRawRoutine(path, callback);
        }

        // FIXME: Performs an uncached lookup, create a synchronized dictionary?
        public static IEnumerator GetRawRoutine<T>(string path, Action<T> callback)
        {
            using WWW www = new WWW(path);
            yield return www;
            if (www.error != null)
            {
                throw new Exception(www.error);
            }

            Type assetType = typeof(T);
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

            callback.Invoke((T)val);
        }
    }
}
