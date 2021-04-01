using ExitGames.Client.Photon;
using System.Collections;
using UnityEngine;

public static class Extensions
{
    public static PhotonView[] GetPhotonViewsInChildren(this GameObject go)
    {
        return go.GetComponentsInChildren<PhotonView>(includeInactive: true);
    }

    public static PhotonView GetPhotonView(this GameObject go)
    {
        return go.GetComponent<PhotonView>();
    }

    public static bool AlmostEquals(this Vector3 target, Vector3 second, float sqrMagnitudePrecision)
    {
        return (target - second).sqrMagnitude < sqrMagnitudePrecision;
    }

    public static bool AlmostEquals(this Vector2 target, Vector2 second, float sqrMagnitudePrecision)
    {
        return (target - second).sqrMagnitude < sqrMagnitudePrecision;
    }

    public static bool AlmostEquals(this Quaternion target, Quaternion second, float maxAngle)
    {
        return Quaternion.Angle(target, second) < maxAngle;
    }

    public static bool AlmostEquals(this float target, float second, float floatDiff)
    {
        return Mathf.Abs(target - second) < floatDiff;
    }

    public static void Merge(this IDictionary target, IDictionary addHash)
    {
        if (addHash != null && !target.Equals(addHash))
        {
            foreach (object key in addHash.Keys)
            {
                target[key] = addHash[key];
            }
        }
    }

    public static void MergeStringKeys(this IDictionary target, IDictionary addHash)
    {
        if (addHash != null && !target.Equals(addHash))
        {
            foreach (object key in addHash.Keys)
            {
                if (key is string)
                {
                    target[key] = addHash[key];
                }
            }
        }
    }

    public static string ToStringFull(this IDictionary origin)
    {
        return SupportClass.DictionaryToString(origin, includeTypes: false);
    }

    public static ExitGames.Client.Photon.Hashtable StripToStringKeys(this IDictionary original)
    {
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        foreach (DictionaryEntry item in original)
        {
            if (item.Key is string)
            {
                hashtable[item.Key] = item.Value;
            }
        }
        return hashtable;
    }

    public static void StripKeysWithNullValues(this IDictionary original)
    {
        object[] array = new object[original.Count];
        original.Keys.CopyTo(array, 0);
        foreach (object key in array)
        {
            if (original[key] == null)
            {
                original.Remove(key);
            }
        }
    }

    public static bool Contains(this int[] target, int nr)
    {
        if (target == null)
        {
            return false;
        }
        for (int i = 0; i < target.Length; i++)
        {
            if (target[i] == nr)
            {
                return true;
            }
        }
        return false;
    }
}
