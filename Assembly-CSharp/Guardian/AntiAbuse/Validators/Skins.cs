using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Guardian.AntiAbuse.Validators
{
    class Skins
    {
        public static string HostWhitelistPath = Mod.RootDir + "\\Hosts.txt";
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
                Mod.Logger.Warn("Accepting ALL hosts for the skin whitelist.");
                Mod.Logger.Warn("\tThis leaves you at risk of being IP-logged!".AsColor("FF4444"));
            } else
            {
                Mod.Logger.Debug($"Accepting {HostWhitelist.Count} host(s) for the skin whitelist.");
            }
        }

        public static WWW CreateWWW(string url)
        {
            if (url.ToLower().StartsWith("file://") || HostWhitelist.Count < 1)
            {
                return new WWW(url);
            }

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                string textureHost = uri.Authority;
                textureHost = textureHost.StartsWith("www.", StringComparison.OrdinalIgnoreCase) ? textureHost.Substring(4) : textureHost;

                foreach (string whitelistHost in HostWhitelist)
                {
                    string host = whitelistHost.StartsWith("www.", StringComparison.OrdinalIgnoreCase) ? whitelistHost.Substring(4) : whitelistHost;
                    if (textureHost.Equals(host, StringComparison.OrdinalIgnoreCase))
                    {
                        return new WWW(url);
                    }
                }

                if (textureHost.Length > 0)
                {
                    Mod.Logger.Warn($"Unwhitelisted host: {textureHost}");
                }
            }

            return null;
        }
    }
}
