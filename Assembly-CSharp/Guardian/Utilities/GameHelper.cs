using System;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Guardian.Utilities
{
    class GameHelper
    {
        public static readonly Regex Detagger = new Regex("<\\/?(color(=[^>]*)?|size(=\\d*)?|b|i|material(=[^>]*)?|quad([^>]*)?)>", RegexOptions.IgnoreCase);
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static void Broadcast(string message)
        {
            FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, message, "[Server]".WithColor("aaff00").AsBold());
        }

        public static WWW CreateWWW(string url)
        {
            if (Mod.HostWhitelist.Count < 1)
            {
                return new WWW(url);
            }
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                string textureHost = uri.Authority;
                textureHost = textureHost.StartsWith("www.", StringComparison.OrdinalIgnoreCase) ? textureHost.Substring(4) : textureHost;

                foreach (string whitelistHost in Mod.HostWhitelist)
                {
                    string host = whitelistHost.StartsWith("www.", StringComparison.OrdinalIgnoreCase) ? whitelistHost.Substring(4) : whitelistHost;
                    if (textureHost.Equals(host, StringComparison.OrdinalIgnoreCase))
                    {
                        return new WWW(url);
                    }
                }
                Mod.Logger.Warn($"Unwhitelisted skin host: {textureHost}");
            }
            return null;
        }

        public static HERO GetHero(PhotonPlayer player)
        {
            foreach (HERO hero in FengGameManagerMKII.Instance.heroes)
            {
                if (hero.photonView.ownerId == player.id)
                {
                    return hero;
                }
            }
            return null;
        }

        public static TITAN GetPT(PhotonPlayer player)
        {
            foreach (TITAN titan in FengGameManagerMKII.Instance.titans)
            {
                if (titan.photonView.ownerId == player.id)
                {
                    return titan;
                }
            }
            return null;
        }

        public static bool IsDead(PhotonPlayer player)
        {
            return GExtensions.AsBool(player.customProperties[PhotonPlayerProperty.Dead]);
        }

        public static bool IsAHSS(PhotonPlayer player)
        {
            return GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Team]) == 2;
        }

        public static bool IsPT(PhotonPlayer player)
        {
            return GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.IsTitan]) == 2;
        }

        public static object[] GetRandomTitanRespawnPoint()
        {
            Vector3 position = new Vector3(MathHelper.RandomInt(-400, 401), 0f, MathHelper.RandomInt(-400, 401));
            Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
            if (FengGameManagerMKII.Instance.titanSpawns.Count > 0)
            {

                position = FengGameManagerMKII.Instance.titanSpawns[MathHelper.RandomInt(0, FengGameManagerMKII.Instance.titanSpawns.Count)];
            }
            else
            {
                GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("titanRespawn");
                if (spawnPoints.Length > 0)
                {
                    int index = MathHelper.RandomInt(0, spawnPoints.Length);
                    GameObject spawnPoint = spawnPoints[index];
                    while (spawnPoints[index] == null)
                    {
                        index = MathHelper.RandomInt(0, spawnPoints.Length);
                        spawnPoint = spawnPoints[index];
                    }
                    spawnPoints[index] = null;
                    position = spawnPoint.transform.position;
                    rotation = spawnPoint.transform.rotation;
                }
            }
            return new object[] { position, rotation };
        }

        public static bool TryCreateFile(string path, bool directory)
        {
            try
            {
                if (!directory)
                {
                    if (!File.Exists(path))
                    {
                        File.Create(path).Close();
                    }
                }
                else if (!Directory.Exists(path))
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

        // C# equivalent of java.lang.System#currentTimeMillis()
        public static long CurrentTimeMillis()
        {
            return (long)DateTime.UtcNow.Subtract(epoch).TotalMilliseconds;
        }
    }
}