using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Guardian.AntiAbuse.Validators
{
    class SkinValidator
    {
        public static readonly string AllowedHostsPath = GuardianClient.RootDir + "\\Hosts.txt";
        public static List<string> AllowedHosts = new List<string>();

        public static void Init()
        {
            if (!File.Exists(AllowedHostsPath))
            {
                AllowedHosts.Add("i.imgur.com");
                AllowedHosts.Add("imgur.com");
                AllowedHosts.Add("cdn.discordapp.com");
                AllowedHosts.Add("cdn.discord.com");
                AllowedHosts.Add("media.discordapp.net");
                AllowedHosts.Add("i.gyazo.com");

                File.WriteAllLines(AllowedHostsPath, AllowedHosts.ToArray());
            }
            AllowedHosts = new List<string>(File.ReadAllLines(AllowedHostsPath));

            if (AllowedHosts.Count < 1)
            {
                GuardianClient.Logger.Warn("Allowing ALL hosts for skins.");
                GuardianClient.Logger.Warn("\tThis leaves you at risk of being IP-logged!".AsColor("FF0000"));
            }
            else
            {
                GuardianClient.Logger.Debug($"Allowing {AllowedHosts.Count} host(s) for skins.");
            }
        }

        public static WWW CreateWWW(string url)
        {
            if (url.ToLower().StartsWith("file://")) return new WWW(url);
            if (AllowedHosts.Count < 1) return new WWW(url);
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri)) return null;

            string textureHost = uri.Authority;
            if (textureHost.StartsWith("www.", StringComparison.OrdinalIgnoreCase)) textureHost = textureHost.Substring(4);

            foreach (string hostEntry in AllowedHosts)
            {
                string host = hostEntry;
                if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase)) host = host.Substring(4);

                if (!textureHost.Equals(host, StringComparison.OrdinalIgnoreCase)) continue;

                return new WWW(url);
            }

            if (textureHost.Length > 0)
            {
                GuardianClient.Logger.Warn($"Denied skin host: {textureHost}");
            }

            return null;
        }
    }
}
