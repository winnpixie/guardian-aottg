using System.Collections.Generic;

namespace Guardian.Utilities
{
    class ModDetector
    {
        public static List<string> GetMods(PhotonPlayer player)
        {
            ExitGames.Client.Photon.Hashtable properties = player.customProperties;
            List<string> mods = new List<string>();

            // Neko
            if (player.isNeko)
            {
                string userType = "[ffffff]";
                if (player.isNekoUser)
                {
                    userType += "(User)";
                }
                if (player.isNekoOwner)
                {
                    userType += "(Owner)";
                }
                mods.Add($"[ee00ee][Neko{userType}]");
            }

            // Fox
            if (player.isFoxMod)
            {
                mods.Add("[ff6600][Fox]");
            }

            // Cyrus Essentials
            if (player.isCyrus)
            {
                mods.Add("[ffff00][CE]");
            }

            // Photon
            if (player.isPhoton)
            {
                mods.Add("[ffffff][Photon]");
            }

            // Anarchy
            if (player.isAnarchy)
            {
                mods.Add("[ffffff][Anarchy]");
            }

            // KnK
            if (player.isKnK)
            {
                mods.Add("[ff0000][KnK]");
            }

            // NRC
            if (player.isNRC)
            {
                mods.Add("[ffffff][NRC]");
            }

            // TRAP
            if (player.isTrap)
            {
                mods.Add("[ee66ff][TRAP]");
            }

            // RC83
            if (player.isRC83)
            {
                mods.Add("[ffffff][RC83]");
            }

            // Guardian (mine!)
            if ((properties.ContainsKey("GuardianMod") && properties["GuardianMod"] is int)
                || (properties.ContainsKey("Stats") && properties["Stats"] is int))
            {
                string boostedStats = "[ffffff]";
                if (properties.ContainsKey("Stats"))
                {
                    string modifications = ModifiedStats.FromInt(GExtensions.AsInt(properties["Stats"]));
                    if (modifications.Length > 0)
                    {
                        boostedStats += $"(Inf:{modifications})";
                    }
                }

                mods.Add($"[aaff00][Guardian{boostedStats}[-]]");
            }

            if ((properties.ContainsKey("A.S Guard") && properties["A.S Guard"] is int)
                || (properties.ContainsKey("Allstar Mod") && properties["Allstar Mod"] is int))
            {
                mods.Add("[ffffff][[ff0000]A[-]llStar]");
            }

            // DogS
            if (properties.ContainsKey("dogshitmod") && GExtensions.AsString(properties["dogshitmod"]).Equals("dogshitmod"))
            {
                mods.Add("[ffffff][DogS]");
            }

            // LNON
            if (properties.ContainsKey("LNON"))
            {
                mods.Add("[ffffff][LNON]");
            }

            // Ignis
            if (properties.ContainsKey("Ignis"))
            {
                mods.Add("[ffffff][Ignis]");
            }

            // PedoBear
            if (player.isPedoBear
                || properties.ContainsKey("PBModRC"))
            {
                mods.Add("[ffffff][[ff6600]P[553300]B[-][-]]");
            }

            // Disciple
            if (properties.ContainsKey("DiscipleMod"))
            {
                mods.Add("[777777][Disciple]");
            }

            // TLW
            if (properties.ContainsKey("TLW"))
            {
                mods.Add("[ffffff][TLW]");
            }

            // ARC
            if (properties.ContainsKey("ARC-CREADOR"))
            {
                mods.Add("[ffffff][ARC (Creator)]");
            }
            if (properties.ContainsKey("ARC"))
            {
                mods.Add("[ffffff][ARC]");
            }

            // SRC
            if (properties.ContainsKey("SRC"))
            {
                mods.Add("[ffffff][SRC]");
            }

            // Cyan Mod
            if (player.isCyan
                || properties.ContainsKey("CyanMod")
                || properties.ContainsKey("CyanModNew"))
            {
                mods.Add("[ffffff][[00ffff]Cyan Mod[-]]");
            }

            // Expedition
            if (player.isExpedition
                || properties.ContainsKey("ExpMod")
                || properties.ContainsKey("EMID")
                || properties.ContainsKey("Version")
                || properties.ContainsKey("Pref"))
            {
                string version = "[ffffff]v";
                if (properties.ContainsKey("Version"))
                {
                    version += GExtensions.AsFloat(properties["Version"]);
                }
                if (properties.ContainsKey("Pref"))
                {
                    version += GExtensions.AsString(properties["Pref"]);
                }
                mods.Add($"[009900][Exp {version}[-]]");
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
                string edition = "[ffffff]";
                if (properties.ContainsKey("UYoutube"))
                {
                    edition += "(You[ff0000]tube[-])";
                }
                if (properties.ContainsKey("UVip"))
                {
                    edition += "([ffcc00]VIP[-])";
                }
                if (properties.ContainsKey("UAdmin"))
                {
                    edition += "([ff0000]Admin[-])";
                }
                mods.Add($"[aa00aa][Universe{edition}[-]]");
            }

            // Teiko
            if (properties.ContainsKey("Teiko"))
            {
                mods.Add("[aed6f1][Teiko]");
            }

            // SLB
            if (properties.ContainsKey("Wings")
                || properties.ContainsKey("EarCat")
                || properties.ContainsKey("Horns"))
            {
                mods.Add("[ffffff][SLB]");
            }

            // Ranked RC
            if (player.isRankedRC
                || properties.ContainsKey("bronze")
                || properties.ContainsKey("diamond")
                || (properties.ContainsKey("") && properties[""] is int))
            {
                mods.Add("[ffffff][Ranked RC]");
            }

            // DeadInside
            if (properties.ContainsKey("DeadInside"))
            {
                mods.Add("[000000][DeadInside]");
            }

            // DFSAO
            if (properties.ContainsKey("DFSAO"))
            {
                mods.Add("[ffffff][DFSAO]");
            }

            // AoE
            if (properties.ContainsKey("AOE") && GExtensions.AsString(properties["AOE"]).Equals("Made By Exile"))
            {
                mods.Add("[0000ff][AoE]");
            }

            string name = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]);
            string guild = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Guild]);

            // Parrot
            if (guild.StartsWith("[00FF00]PARROT'S MOD"))
            {
                mods.Add("[00ff00][PARROT]");
            }

            // Unknown
            if (player.isUnknown
                || properties.ContainsKey("Taquila")
                || properties.ContainsKey("Pain")
                || properties.ContainsKey("uishot"))
            {
                mods.Add("[ffffff][???]");
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
                mods.Add("[9999ff][RC]");
            }

            // >48/>40 chars
            if (name.Length > 48 || guild.Length > 40)
            {
                string lengthFlags = "";

                if (name.Length > 48)
                {
                    lengthFlags += ">48";
                }
                if (guild.Length > 40)
                {
                    lengthFlags += name.Length > 48 ? "|>40" : ">40";
                }

                mods.Add($"[ffffff][{lengthFlags}]");
            }

            // Vanilla
            if (mods.Count == 0)
            {
                mods.Add("[ffddaa][Vanilla]");
            }

            return mods;
        }
    }
}
