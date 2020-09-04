using UnityEngine;

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
                mod += "[ffffff][[da00ff]N[9115ff]e[492bff]k[0040ff]o";
                if (player.isNekoUser)
                {
                    mod += "[ffffff](User)";
                }
                if (player.isNekoOwner)
                {
                    mod += "[ffffff](Owner)";
                }
                mod += "[ffffff]]";
            }

            // Fox
            if (player.isFoxMod)
            {
                mod += "[ff6600][Fox]";
            }

            // Cyrus Essentials
            if (player.isCyrus)
            {
                mod += "[ffff00][CE]";
            }

            // Photon
            if (player.isPhoton)
            {
                mod += "[ffffff][Photon]";
            }

            // Anarchy
            if (player.isAnarchy)
            {
                mod += "[ffffff][Anarchy]";
            }

            // KnK
            if (player.isKnK)
            {
                mod += "[ff0000][KnK]";
            }

            // NRC
            if (player.isNRC)
            {
                mod += "[ffffff][NRC]";
            }

            // TRAP
            if (player.isTrap)
            {
                mod += "[ee66ff][TRAP]";
            }

            // Guardian (mine!)
            if ((properties.ContainsKey("GuardianMod") && properties["GuardianMod"] is int)
                || (properties.ContainsKey("Stats") && properties["Stats"] is int))
            {
                mod += string.Concat("[ffff00][Guardian");
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
                mod += "]";
            }

            if ((properties.ContainsKey("A.S Guard") && properties["A.S Guard"] is int)
                || (properties.ContainsKey("Allstar Mod") && properties["Allstar Mod"] is int))
            {
                mod += "[ffffff][[ff0000]A[-]llStar]";
            }

            // DogS
            if (properties.ContainsKey("dogshitmod") && GExtensions.AsString(properties["dogshitmod"]).Equals("dogshitmod"))
            {
                mod += "[ffffff][DogS]";
            }

            // LNON
            if (properties.ContainsKey("LNON"))
            {
                mod += "[ffffff][LNON]";
            }

            // Ignis
            if (properties.ContainsKey("Ignis"))
            {
                mod += "[ffffff][Ignis]";
            }

            // PedoBear
            if (player.isPedoBear
                || properties.ContainsKey("PBModRC"))
            {
                mod += "[ffffff][[ff6600]P[553300]B[-][-]]";
            }

            // Disciple
            if (properties.ContainsKey("DiscipleMod"))
            {
                mod += "[ffffff][[555555][000000]D[1F1F1F]i[3F3F3F]s[5F5F5F]c[7F7F7F]i[9F9F9F]p[BFBFBF]l[DFDFDF]e[ffffff]]";
            }

            // TLW
            if (properties.ContainsKey("TLW"))
            {
                mod += "[ffffff][TLW]";
            }

            // ARC
            if (properties.ContainsKey("ARC-CREADOR"))
            {
                mod += "[ffffff][ARC (Creator)]";
            }
            if (properties.ContainsKey("ARC"))
            {
                mod += "[ffffff][ARC]";
            }

            // SRC
            if (properties.ContainsKey("SRC"))
            {
                mod += "[ffffff][SRC]";
            }

            // Cyan Mod
            if (player.isCyan
                || properties.ContainsKey("CyanMod")
                || properties.ContainsKey("CyanModNew"))
            {
                mod += "[ffffff][[00ffff]Cyan Mod[-]]";
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
                mod += $"[009900][Expedition[ffffff](v{version})[-]]";
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
                mod += "[aa00aa][Universe[ffffff]";
                if (properties.ContainsKey("UYoutube"))
                {
                    mod += "(You[ff0000]tube[-])";
                }
                if (properties.ContainsKey("UVip"))
                {
                    mod += "([ffcc00]VIP[-])";
                }
                if (properties.ContainsKey("UAdmin"))
                {
                    mod += "([ff0000]Admin[-])";
                }
                mod += "[-]]";
            }

            // Teiko
            if (properties.ContainsKey("Teiko"))
            {
                mod += "[aed6f1][Teiko]";
            }

            // SLB
            if (properties.ContainsKey("Wings")
                || properties.ContainsKey("EarCat")
                || properties.ContainsKey("Horns"))
            {
                mod += "[ffffff][SLB]";
            }

            // Ranked RC
            if (player.isRankedRC
                || properties.ContainsKey("bronze")
                || properties.ContainsKey("diamond")
                || (properties.ContainsKey("") && properties[""] is int))
            {
                mod += "[ffffff][Ranked RC]";
            }

            // DeadInside
            if (properties.ContainsKey("DeadInside"))
            {
                mod += "[000000][DeadInside]";
            }

            // DFSAO
            if (properties.ContainsKey("DFSAO"))
            {
                mod += "[ffffff][DFSAO]";
            }

            // AoE
            if (properties.ContainsKey("AOE") && GExtensions.AsString(properties["AOE"]).Equals("Made By Exile"))
            {
                mod += "[0000ff][AoE]";
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
                mod += "[9999ff][RC]";
            }

            string name = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]);
            string guild = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Guild]);

            // Parrot
            if (guild.StartsWith("[00FF00]PARROT'S MOD"))
            {
                mod += "[00ff00][PARROT]";
            }

            // Unknown
            if (player.isUnknown
                || properties.ContainsKey("Taquila")
                || properties.ContainsKey("Pain")
                || properties.ContainsKey("uishot"))
            {
                mod += "[ffffff][???]";
            }

            // Vanilla
            if (mod.Length == 0)
            {
                mod += "[ffddaa][Vanilla]";
            }

            // >48/>40 chars
            if (name.Length > 48 || guild.Length > 40)
            {
                mod += "[ffffff][";
                if (name.Length > 48)
                {
                    mod += ">48";
                }
                if (guild.Length > 40)
                {
                    mod += name.Length > 48 ? "|>40" : ">40";
                }
                mod += "]";
            }

            return mod;
        }
    }
}
