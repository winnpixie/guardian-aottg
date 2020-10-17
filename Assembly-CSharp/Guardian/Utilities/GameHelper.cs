using System;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Guardian.Utilities
{
    class GameHelper
    {
        public static readonly Regex Detagger = new Regex("<\\/?(color(=[^>]*)?|size(=\\d*)?|b|i|material(=[^>]*)?|quad([^>]*)?)>", RegexOptions.IgnoreCase);
        public static readonly Vector2 ScrollBottom = new Vector2(0, float.MaxValue);
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static void Broadcast(string message)
        {
            FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, message, "[MC]".WithColor("aaff00").AsBold());
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
                if (hero.photonView.ownerId == player.Id)
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
                if (titan.photonView.ownerId == player.Id)
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

        public static string FormatTime(float time, bool precise = false, bool isSeconds = true)
        {
            if (Mod.Properties.LegacyTimeFormat.Value)
            {
                float secs = isSeconds ? time : (time / 1000f);

                if (!precise)
                {
                    secs = MathHelper.Floor(secs);
                }

                return secs > 1 ? secs + " secs" : secs + " sec";
            }

            string output = "";

            if (isSeconds)
            {
                time *= 1000;
            }

            int ms = (int)time % 1000;
            int sec = MathHelper.Floor(time / 1000f);
            int min = MathHelper.Floor(sec / 60f);
            sec -= min * 60;
            int hrs = MathHelper.Floor(min / 60f);
            min -= hrs * 60;
            int days = MathHelper.Floor(hrs / 24f);
            hrs -= days * 24;
            int years = MathHelper.Floor(days / 365f);
            days -= years * 365;


            // Milliseconds
            if (ms > 0 && precise)
            {
                output = $"{ms}ms";
            }

            // Seconds
            if (sec > 0)
            {
                if (output.Length > 0)
                {
                    output = $", {output}";
                }
                output = sec > 1 ? $"{sec} secs{output}" : $"{sec} sec{output}";
            }

            // Minutes
            if (min > 0)
            {
                if (output.Length > 0)
                {
                    output = $", {output}";
                }
                output = min > 1 ? $"{min } mins{output}" : $"{min} min{output}";
            }

            // Hours
            if (hrs > 0)
            {
                if (output.Length > 0)
                {
                    output = $", {output}";
                }
                output = hrs > 1 ? $"{hrs} hrs{output}" : $"{hrs} hr{output}";
            }

            // Days
            if (days > 0)
            {
                if (output.Length > 0)
                {
                    output = $", {output}";
                }
                output = days > 1 ? $"{days} days{output}" : $"{days} day{output}";
            }

            // Years
            if (years > 0)
            {
                if (output.Length > 0)
                {
                    output = $", {output}";
                }
                output = years > 1 ? $"{years} yrs{output}" : $"{years} yr{output}";

            }

            return output;
        }
    }
}