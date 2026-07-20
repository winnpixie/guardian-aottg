using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Guardian.Utilities
{
    public static class GameHelper
    {
        public static readonly Regex DangerousTagsPattern =
            new Regex("<\\/?(size|material|quad)[^>]*>", RegexOptions.IgnoreCase);

        public static readonly Vector2 ScrollBottom = new Vector2(0, float.MaxValue);

        public static void Broadcast(string message)
        {
            FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, message,
                "<b><color=#AAFF00>[MC]</color></b>");
        }

        public static object[] GetRandomTitanRespawnPoint()
        {
            Vector3 position = new Vector3(MathHelper.RandInt(-400, 401), 0f, MathHelper.RandInt(-400, 401));
            Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);

            if (FengGameManagerMKII.Instance.titanSpawns.Count > 0)
            {
                position = FengGameManagerMKII.Instance.titanSpawns[
                    MathHelper.RandInt(0, FengGameManagerMKII.Instance.titanSpawns.Count)];
            }
            else
            {
                GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("titanRespawn");

                if (spawnPoints.Length > 0)
                {
                    GameObject spawnPoint;

                    do
                    {
                        spawnPoint = spawnPoints[MathHelper.RandInt(0, spawnPoints.Length)];
                    } while (spawnPoint == null);

                    position = spawnPoint.transform.position;
                    rotation = spawnPoint.transform.rotation;
                }
            }

            return new object[] { position, rotation };
        }

        public static bool TryCreateFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool TryCreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        // C# "good enough" equivalent of java.lang.System#currentTimeMillis()
        public static long CurrentTimeMillis()
        {
            return DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}