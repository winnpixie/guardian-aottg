using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Guardian.AntiAbuse.Validators
{
    class SkinChecker
    {
        public static string HostWhitelistPath = GuardianClient.RootDir + "\\Hosts.txt";
        public static List<string> HostWhitelist = new List<string>();

        public static void Init()
        {
            if (!File.Exists(HostWhitelistPath))
            {
                HostWhitelist.Add("i.imgur.com");
                HostWhitelist.Add("imgur.com");
                HostWhitelist.Add("cdn.discordapp.com");
                HostWhitelist.Add("cdn.discord.com");
                HostWhitelist.Add("media.discordapp.net");
                HostWhitelist.Add("i.gyazo.com");

                File.WriteAllLines(HostWhitelistPath, HostWhitelist.ToArray());
            }
            HostWhitelist = new List<string>(File.ReadAllLines(HostWhitelistPath));

            if (HostWhitelist.Count < 1)
            {
                GuardianClient.Logger.Warn("Accepting ALL hosts for skins.");
                GuardianClient.Logger.Warn("\tThis leaves you at risk of being IP-logged!".AsColor("FF4444"));
            }
            else
            {
                GuardianClient.Logger.Debug($"Accepting {HostWhitelist.Count} host(s) for the skin whitelist.");
            }
        }

        public static WWW CreateWWW(string url)
        {
            if (url.ToLower().StartsWith("file://") || HostWhitelist.Count < 1)
            {
                return new WWW(url);
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri)) return null;

            string textureHost = uri.Authority;
            textureHost = textureHost.StartsWith("www.", StringComparison.OrdinalIgnoreCase) ? textureHost.Substring(4) : textureHost;

            foreach (string hostEntry in HostWhitelist)
            {
                string host = hostEntry.StartsWith("www.", StringComparison.OrdinalIgnoreCase) ? hostEntry.Substring(4) : hostEntry;
                if (!textureHost.Equals(host, StringComparison.OrdinalIgnoreCase)) continue;

                return new WWW(url);
            }

            if (textureHost.Length < 1) return null;

            GuardianClient.Logger.Warn($"Unwhitelisted host: {textureHost}");
            return null;
        }
    }
}
