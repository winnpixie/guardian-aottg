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
                string userType = "[FFFFFF]";
                if (player.isNekoUser)
                {
                    userType += "(User)";
                }
                if (player.isNekoOwner)
                {
                    userType += "(Owner)";
                }
                mods.Add($"[EE00EE][Neko{userType}]");
            }

            // Fox
            if (player.isFoxMod)
            {
                mods.Add("[FF6600][Fox]");
            }

            // Cyrus Essentials
            if (player.isCyrus)
            {
                mods.Add("[FFFF00][CE]");
            }

            // Anarchy
            if (player.isAnarchy)
            {
                mods.Add("[FFFFFF][Anarchy]");
            }

            // KnK
            if (player.isKnK)
            {
                mods.Add("[FF0000][KnK]");
            }

            // NRC
            if (player.isNRC)
            {
                mods.Add("[FFFFFF][NRC]");
            }

            // TRAP
            if (player.isTrap)
            {
                mods.Add("[EE66FF][TRAP]");
            }

            // RC83
            if (player.isRC83)
            {
                mods.Add("[FFFFFF][RC83]");
            }

            // Guardian (mine!!)
            if (properties.ContainsKey("GuardianMod") && properties["GuardianMod"] is int)
            {
                mods.Add($"[0099FF][Guardian]");
            }

            // ZMOD
            if ((properties.ContainsKey("ZMOD") && properties["ZMOD"] is string)
                || (properties.ContainsKey("idleGas") && properties["idleGas"] is bool)
                || (properties.ContainsKey("idleEffect") && properties["idleEffect"] is string)
                || (properties.ContainsKey("infGas") && properties["infGas"] is bool)
                || (properties.ContainsKey("infBlades") && properties["infBlades"] is bool))
            {
                mods.Add("[550055][ZMOD]");
            }

            if ((properties.ContainsKey("A.S Guard") && properties["A.S Guard"] is int)
                || (properties.ContainsKey("Allstar Mod") && properties["Allstar Mod"] is int))
            {
                mods.Add("[FFFFFF][[FF0000]A[-]llStar]");
            }

            // DogS
            if (properties.ContainsKey("dogshitmod") && GExtensions.AsString(properties["dogshitmod"]).Equals("dogshitmod"))
            {
                mods.Add("[FFFFFF][DogS]");
            }

            // LNON
            if (properties.ContainsKey("LNON"))
            {
                mods.Add("[FFFFFF][LNON]");
            }

            // Ignis
            if (properties.ContainsKey("Ignis"))
            {
                mods.Add("[FFFFFF][Ignis]");
            }

            // PedoBear
            if (player.isPedoBear
                || properties.ContainsKey("PBModRC"))
            {
                mods.Add("[FFFFFF][[FF6600]P[553300]B[-][-]]");
            }

            // Disciple
            if (properties.ContainsKey("DiscipleMod"))
            {
                mods.Add("[777777][Disciple]");
            }

            // TLW
            if (properties.ContainsKey("TLW"))
            {
                mods.Add("[FFFFFF][TLW]");
            }

            // ARC
            if (properties.ContainsKey("ARC-CREADOR"))
            {
                mods.Add("[FFFFFF][ARC (Creator)]");
            }
            if (properties.ContainsKey("ARC"))
            {
                mods.Add("[FFFFFF][ARC]");
            }

            // SRC
            if (properties.ContainsKey("SRC"))
            {
                mods.Add("[FFFFFF][SRC]");
            }

            // Cyan Mod
            if (player.isCyan
                || properties.ContainsKey("CyanMod")
                || properties.ContainsKey("CyanModNew"))
            {
                mods.Add("[00FFFF][Cyan]");
            }

            // Expedition
            if (player.isExpedition
                || properties.ContainsKey("ExpMod")
                || properties.ContainsKey("EMID")
                || properties.ContainsKey("Version")
                || properties.ContainsKey("Pref"))
            {
                string version = "[FFFFFF]v";
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
                || (properties.ContainsKey(string.Empty) && properties[string.Empty] is string))
            {
                string edition = "[FFFFFF]";
                if (properties.ContainsKey("UYoutube"))
                {
                    edition += "(You[FF0000]Tube[-])";
                }
                if (properties.ContainsKey("UVip"))
                {
                    edition += "([FFCC00]VIP[-])";
                }
                if (properties.ContainsKey("UAdmin"))
                {
                    edition += "([FF0000]Admin[-])";
                }
                mods.Add($"[AA00AA][Universe{edition}[-]]");
            }

            // Teiko
            if (properties.ContainsKey("Teiko"))
            {
                mods.Add("[AED6F1][Teiko]");
            }

            // SLB
            if (properties.ContainsKey("Wings")
                || properties.ContainsKey("EarCat")
                || properties.ContainsKey("Horns"))
            {
                mods.Add("[FFFFFF][SLB]");
            }

            // Ranked RC
            if (player.isRankedRC
                || properties.ContainsKey("bronze")
                || properties.ContainsKey("diamond")
                || (properties.ContainsKey(string.Empty) && properties[string.Empty] is int))
            {
                mods.Add("[FFFFFF][RRC]");
            }

            // DeadInside
            if (properties.ContainsKey("DeadInside"))
            {
                mods.Add("[000000][DeadInside]");
            }

            // DFSAO
            if (properties.ContainsKey("DFSAO"))
            {
                mods.Add("[FFFFFF][DFSAO]");
            }

            // AoE
            if (properties.ContainsKey("AOE") && GExtensions.AsString(properties["AOE"]).Equals("Made By Exile"))
            {
                mods.Add("[0000FF][AoE]");
            }

            string name = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]);
            string guild = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Guild]);

            // Parrot
            if (guild.StartsWith("[00FF00]PARROT'S MOD"))
            {
                mods.Add("[00FF00][PARROT]");
            }

            // Unknown
            if (player.isUnknown
                || properties.ContainsKey("Taquila")
                || properties.ContainsKey("Pain")
                || properties.ContainsKey("uishot"))
            {
                mods.Add("[FFFFFF][???]");
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
                mods.Add("[9999FF][RC]");
            }

            // >48/>40 chars
            if (name.Length > 48 || guild.Length > 40)
            {
                string lengthFlags = string.Empty;

                if (name.Length > 48)
                {
                    lengthFlags += ">48";
                }
                if (guild.Length > 40)
                {
                    lengthFlags += name.Length > 48 ? "|>40" : ">40";
                }

                mods.Add($"[FFFFFF][{lengthFlags}]");
            }

            // Vanilla
            if (mods.Count == 0)
            {
                mods.Add("[FFDDAA][Vanilla]");
            }

            return mods;
        }
    }
}
