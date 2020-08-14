namespace Guardian.Utilities
{
    class ModDetector
    {
        public static string GetMod(PhotonPlayer player)
        {
            ExitGames.Client.Photon.Hashtable properties = player.customProperties;
            string mod = "";

            // Neko
            if (player.isNeko)
            {
                mod += "[ffcc00][[da00ff]N[9115ff]e[492bff]k[0040ff]o";
                if (player.isNekoUser)
                {
                    mod += "[ffffff](User)";
                }
                if (player.isNekoOwner)
                {
                    mod += "[ffffff](Owner)";
                }
                mod += "[ffcc00]]";
            }

            // Photon mod
            if (player.isPhoton)
            {
                mod += "[ffcc00][[ffffff]Photon[-]]";
            }

            // Anarchy mod
            if (player.isAnarchy)
            {
                mod += "[ffcc00][[ffffff]Anarchy[-]]";
            }

            // KnK
            if (player.isKnK)
            {
                mod += "[ffcc00][[ffffff]KnK[-]]";
            }

            // NRC
            if (player.isNRC)
            {
                mod += "[ffcc00][[ffffff]NRC[-]]";
            }

            // TRAP
            if (player.isTrap)
            {
                mod += "[ffcc00][[ee66ff]TRAP[-]]";
            }

            // Mine!
            if ((properties.ContainsKey("GuardianMod") && properties["GuardianMod"] is int)
                || (properties.ContainsKey("Stats") && properties["Stats"] is int))
            {
                mod += string.Concat("[ffcc00][[aabb66]Guardian");
                if (properties.ContainsKey("Stats"))
                {
                    int stats = GExtensions.AsInt(properties["Stats"]);
                    string modifications = ModifiedStats.FromInt(stats);
                    if (modifications.Length == 0)
                    {
                        mod += "[ffffff](Inf:None)[-]";
                    }
                    else
                    {
                        mod += $"[ffffff](Inf:{modifications})[-]";
                    }
                }
                mod += "[-]]";
            }

            // DogS
            if (properties.ContainsKey("dogshitmod") && GExtensions.AsString(properties["dogshitmod"]).Equals("dogshitmod"))
            {
                mod += "[ffcc00][[ffffff]DogS[-]]";
            }

            // LNON
            if (properties.ContainsKey("LNON"))
            {
                mod += "[ffcc00][[ffffff]LNON[-]]";
            }

            // Ignis
            if (properties.ContainsKey("Ignis"))
            {
                mod += "[ffcc00][[ffffff]Ignis[-]]";
            }

            // PedoBear
            if (player.isPedoBear
                || properties.ContainsKey("PBModRC"))
            {
                mod += "[ffcc00][[ffffff]PedoBear[-]]";
            }

            // Disciple
            if (properties.ContainsKey("DiscipleMod"))
            {
                mod += "[ffcc00][[555555][000000]D[1F1F1F]i[3F3F3F]s[5F5F5F]c[7F7F7F]i[9F9F9F]p[BFBFBF]l[DFDFDF]e[ffcc00]]";
            }

            // TLW
            if (properties.ContainsKey("TLW"))
            {
                mod += "[ffcc00][[ffffff]TLW[-]]";
            }

            // ARC
            if (properties.ContainsKey("ARC-CREADOR"))
            {
                mod += "[ffcc00][[ffffff]ARC (Creator)[-]]";
            }
            if (properties.ContainsKey("ARC"))
            {
                mod += "[ffcc00][[ffffff]ARC[-]]";
            }

            // SRC
            if (properties.ContainsKey("SRC"))
            {
                mod += "[ffcc00][[ffffff]SRC[-]]";
            }

            // Cyan Mod
            if (player.isCyan
                || properties.ContainsKey("CyanMod")
                || properties.ContainsKey("CyanModNew"))
            {
                mod += "[ffcc00][[00ffff]Cyan Mod[-]]";
            }

            // Expedition
            if (player.isExpedition
                || properties.ContainsKey("ExpMod")
                || properties.ContainsKey("EMID")
                || properties.ContainsKey("Version")
                || properties.ContainsKey("Pref"))
            {
                string version = "";
                if (properties.ContainsKey("Version"))
                {
                    version += GExtensions.AsFloat(properties["Version"]);
                }
                if (properties.ContainsKey("Pref"))
                {
                    version += GExtensions.AsString(properties["Pref"]);
                }
                mod += $"[ffcc00][[009900]Expedition[ffffff](v{version})[ffcc00]]";
            }

            // Universe
            if (player.isUniverse
                || properties.ContainsKey("UPublica")
                || properties.ContainsKey("UPublica2")
                || properties.ContainsKey("UGrup")
                || properties.ContainsKey("Hats")
                || properties.ContainsKey("UYoutube")
                || properties.ContainsKey("UVip")
                || properties.ContainsKey("SUniverse")
                || properties.ContainsKey("UAdmin")
                || properties.ContainsKey("coins")
                || (properties.ContainsKey("") && properties[""] is string))
            {
                mod += "[ffcc00][[aa00aa]Universe";
                if (properties.ContainsKey("UYoutube"))
                {
                    mod += "[ffffff](You[ff0000]tube[ffffff])";
                }
                if (properties.ContainsKey("UAdmin"))
                {
                    mod += "[ff0000](Admin)";
                }
                mod += "[ffcc00]]";
            }

            // Teiko
            if (properties.ContainsKey("Teiko"))
            {
                mod += "[ffcc00][[aed6f1]Teiko[-]]";
            }

            // SLB
            if (properties.ContainsKey("Wings")
                || properties.ContainsKey("EarCat")
                || properties.ContainsKey("Horns"))
            {
                mod += "[ffcc00][[ffffff]SLB[-]]";
            }

            // Ranked RC
            if (player.isRankedRC
                || properties.ContainsKey("bronze")
                || properties.ContainsKey("diamond")
                || (properties.ContainsKey("") && properties[""] is int))
            {
                mod += "[ffcc00][[ffffff]Ranked RC[-]]";
            }

            // DeadInside?
            if (properties.ContainsKey("DeadInside"))
            {
                mod += "[ffcc00][[ffffff]DeadInside[-]]";
            }

            // DFSAO
            if (properties.ContainsKey("DFSAO"))
            {
                mod += "[ffcc00][[ffffff]DFSAO[-]]";
            }

            // AoE
            if (properties.ContainsKey("AOE") && GExtensions.AsString(properties["AOE"]).Equals("Made By Exile"))
            {
                mod += "[ffcc00][[0000ff]AoE[-]]";
            }

            // RC
            if (properties.ContainsKey(PhotonPlayerProperty.RCTeam)
                || properties.ContainsKey(PhotonPlayerProperty.RCBombR)
                || properties.ContainsKey(PhotonPlayerProperty.RCBombG)
                || properties.ContainsKey(PhotonPlayerProperty.RCBombB)
                || properties.ContainsKey(PhotonPlayerProperty.RCBombA)
                || properties.ContainsKey(PhotonPlayerProperty.RCBombRadius)
                || properties.ContainsKey(PhotonPlayerProperty.CustomBool)
                || properties.ContainsKey(PhotonPlayerProperty.CustomFloat)
                || properties.ContainsKey(PhotonPlayerProperty.CustomInt)
                || properties.ContainsKey(PhotonPlayerProperty.CustomString))
            {
                mod += "[ffcc00][[9999ff]RC[-]]";
            }

            string name = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]);
            string guild = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Guild]);

            // Parrot
            if (guild.StartsWith("[00FF00]PARROT'S MOD"))
            {
                mod += "[ffcc00][[00ff00]PARROT'S[-]]";
            }

            // Unknown
            if (player.isUnknown
                || properties.ContainsKey("Taquila")
                || properties.ContainsKey("Pain")
                || properties.ContainsKey("uishot"))
            {
                mod += "[ffcc00][[ffffff]?[-]]";
            }

            // Vanilla
            if (mod.Length == 0)
            {
                mod += "[ffcc00][[ffddaa]Vanilla[-]]";
            }

            // >48/>40 chars
            if (name.Length > 48 || guild.Length > 40)
            {
                mod += "[ffcc00][[ffffff]";
                if (name.Length > 48)
                {
                    mod += ">48";
                }
                if (guild.Length > 40)
                {
                    mod += name.Length > 48 ? "|>40" : ">40";
                }
                mod += "[-]]";
            }

            return mod;
        }
    }
}
