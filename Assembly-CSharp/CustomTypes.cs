using ExitGames.Client.Photon;
using UnityEngine;

internal static class CustomTypes
{
    internal static void Register()
    {
        PhotonPeer.RegisterType(typeof(Vector2), 87, SerializeVector2, DeserializeVector2);
        PhotonPeer.RegisterType(typeof(Vector3), 86, SerializeVector3, DeserializeVector3);
        PhotonPeer.RegisterType(typeof(Quaternion), 81, SerializeQuaternion, DeserializeQuaternion);
        PhotonPeer.RegisterType(typeof(PhotonPlayer), 80, SerializePhotonPlayer, DeserializePhotonPlayer);
    }

    private static byte[] SerializeVector3(object obj)
    {
        Vector3 vector = (Vector3)obj;
        int targetOffset = 0;
        byte[] array = new byte[12];
        Protocol.Serialize(vector.x, array, ref targetOffset);
        Protocol.Serialize(vector.y, array, ref targetOffset);
        Protocol.Serialize(vector.z, array, ref targetOffset);
        return array;
    }

    private static object DeserializeVector3(byte[] bytes)
    {
        Vector3 vector = default;
        int offset = 0;
        try
        {
            Protocol.Deserialize(out vector.x, bytes, ref offset);
            Protocol.Deserialize(out vector.y, bytes, ref offset);
            Protocol.Deserialize(out vector.z, bytes, ref offset);
            return vector;
        }
        catch
        {
            Guardian.Mod.Logger.Error("Could not deserialize Vector3.");
            return null;
        }
    }

    private static byte[] SerializeVector2(object obj)
    {
        Vector2 vector = (Vector2)obj;
        byte[] array = new byte[8];
        int targetOffset = 0;
        Protocol.Serialize(vector.x, array, ref targetOffset);
        Protocol.Serialize(vector.y, array, ref targetOffset);
        return array;
    }

    private static object DeserializeVector2(byte[] bytes)
    {
        Vector2 vector = default;
        int offset = 0;
        try
        {
            Protocol.Deserialize(out vector.x, bytes, ref offset);
            Protocol.Deserialize(out vector.y, bytes, ref offset);
            return vector;
        }
        catch
        {
            Guardian.Mod.Logger.Error("Could not deserialize Vector2.");
            return null;
        }
    }

    private static byte[] SerializeQuaternion(object obj)
    {
        Quaternion quaternion = (Quaternion)obj;
        byte[] array = new byte[16];
        int targetOffset = 0;
        Protocol.Serialize(quaternion.w, array, ref targetOffset);
        Protocol.Serialize(quaternion.x, array, ref targetOffset);
        Protocol.Serialize(quaternion.y, array, ref targetOffset);
        Protocol.Serialize(quaternion.z, array, ref targetOffset);
        return array;
    }

    private static object DeserializeQuaternion(byte[] bytes)
    {
        Quaternion quaternion = default;
        int offset = 0;
        try
        {
            Protocol.Deserialize(out quaternion.w, bytes, ref offset);
            Protocol.Deserialize(out quaternion.x, bytes, ref offset);
            Protocol.Deserialize(out quaternion.y, bytes, ref offset);
            Protocol.Deserialize(out quaternion.z, bytes, ref offset);
            return quaternion;
        }
        catch
        {
            Guardian.Mod.Logger.Error("Could not deserialize Quaternion.");
            return null;
        }
    }

    private static byte[] SerializePhotonPlayer(object obj)
    {
        int id = ((PhotonPlayer)obj).id;
        byte[] array = new byte[4];
        int targetOffset = 0;
        Protocol.Serialize(id, array, ref targetOffset);
        return array;
    }

    private static object DeserializePhotonPlayer(byte[] bytes)
    {
        int offset = 0;
        try
        {
            Protocol.Deserialize(out int id, bytes, ref offset);
            if (PhotonNetwork.networkingPeer.mActors.ContainsKey(id))
            {
                return PhotonNetwork.networkingPeer.mActors[id];
            }
        }
        catch
        {
            Guardian.Mod.Logger.Error("Could not deserialize PhotonPlayer.");
        }
        return null;
    }
}
