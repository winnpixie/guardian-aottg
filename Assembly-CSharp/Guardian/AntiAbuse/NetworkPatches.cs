using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Guardian.AntiAbuse
{
    class NetworkPatches
    {
        public static List<string> PropertyWhitelist = new List<string>();
        private static List<object> RoomPropertyWhitelist = new List<object>(new object[] {
            (byte)255, (byte)254, (byte)253, (byte)250, (byte)249, (byte)248, "sender"
        });

        // NetworkingPeer.OnEvent (Code 202)
        public static bool IsInstantiatePacketValid(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer sender)
        {
            bool malformed = evData == null;

            if (!malformed)
            {
                if (evData.ContainsKey((byte)0x0) && !(evData[(byte)0x0] is string))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)0x1) && !(evData[(byte)0x1] is Vector3))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)0x2) && !(evData[(byte)0x2] is Quaternion))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)0x3) && !(evData[(byte)0x3] is int))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)0x4) && !(evData[(byte)0x4] is int[]))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)0x5) && !(evData[(byte)0x5] is object[]))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)0x8) && !(evData[(byte)0x8] is short))
                {
                    malformed = true;
                }
                malformed = malformed || !evData.ContainsKey((byte)0x0) || !evData.ContainsKey((byte)0x6) || !evData.ContainsKey((byte)0x7);
            }

            if (malformed)
            {
                Mod.Logger.Error($"Malformed instantiate from #{(sender == null ? "?" : sender.id.ToString())}.");
                if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.id))
                {
                    FengGameManagerMKII.IgnoreList.Add(sender.id);
                }
                return false;
            }

            return true;
        }

        // NetworkingPeer.OnEvent (Codes 201 and 206)
        public static bool IsSerializeReadValid(ExitGames.Client.Photon.Hashtable data, PhotonPlayer sender)
        {
            bool malformed = data == null;

            if (!malformed)
            {
                if (data.ContainsKey((byte)0x0) && !(data[(byte)0x0] is int))
                {
                    malformed = true;
                }
                if (data.ContainsKey((byte)0x1) && !(data[(byte)0x1] is object[]))
                {
                    malformed = true;
                }
                malformed = malformed || !data.ContainsKey((byte)0x0) || !data.ContainsKey((byte)0x1);
            }

            if (malformed)
            {
                Mod.Logger.Error($"Malformed serialization from #{(sender == null ? "?" : sender.id.ToString())}.");
                if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.id))
                {
                    FengGameManagerMKII.IgnoreList.Add(sender.id);
                }
                return false;
            }

            return true;
        }

        // NetworkingPeer.OnEvent (Code 200)
        public static bool IsRPCValid(ExitGames.Client.Photon.Hashtable rpcData, PhotonPlayer sender)
        {
            bool malformed = rpcData == null;

            if (!malformed)
            {
                if (rpcData.ContainsKey((byte)0x0) && !(rpcData[(byte)0x0] is int))
                {
                    malformed = true;
                }
                if (rpcData.ContainsKey((byte)0x1) && !(rpcData[(byte)0x1] is short))
                {
                    malformed = true;
                }
                if (rpcData.ContainsKey((byte)0x2) && !(rpcData[(byte)0x2] is int))
                {
                    malformed = true;
                }
                if (rpcData.ContainsKey((byte)0x3) && !(rpcData[(byte)0x3] is string))
                {
                    malformed = true;
                }
                if (rpcData.ContainsKey((byte)0x4) && !(rpcData[(byte)0x4] is object[]))
                {
                    malformed = true;
                }
                if (rpcData.ContainsKey((byte)0x5) && !(rpcData[(byte)0x5] is byte))
                {
                    malformed = true;
                }
                malformed = malformed || !(rpcData.ContainsKey((byte)0x0) && (rpcData.ContainsKey((byte)0x5) || rpcData.ContainsKey((byte)0x3)));
            }

            if (malformed)
            {
                Mod.Logger.Error($"Malformed RPC from #{(sender == null ? "?" : sender.id.ToString())}.");
                if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.id))
                {
                    FengGameManagerMKII.IgnoreList.Add(sender.id);
                }
                return false;
            }

            return true;
        }

        public static void OnPlayerPropertyModification(object[] playerAndUpdatedProps)
        {
            PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
            ExitGames.Client.Photon.Hashtable properties = playerAndUpdatedProps[1] as ExitGames.Client.Photon.Hashtable;

            // Photon Mod detection (probably not the right way to detect but it'll work
            if (properties.ContainsKey("guildName")
                && properties["guildName"] is string && ((string)properties["guildName"]).Equals("photonMod"))
            {
                player.isPhoton = true;
            }

            // Neko Mod detection
            if (properties.ContainsValue("N_user") || properties.ContainsValue("N_owner"))
            {
                player.isNeko = true;
                player.isNekoUser = properties.ContainsValue("N_user");
                player.isNekoOwner = properties.ContainsValue("N_owner");
            }

            // Remove invalid properties
            if (player.isLocal && properties.ContainsKey("sender") && properties["sender"] is PhotonPlayer)
            {
                PhotonPlayer sender = (PhotonPlayer)properties["sender"];
                if (!sender.isLocal)
                {
                    properties.StripKeysWithNullValues();
                    List<object> keys = properties.Keys.ToList();
                    PropertyWhitelist.ForEach(k => keys.Remove(k));

                    if (keys.Count > 0)
                    {
                        Mod.Logger.Error($"#{sender.id} applied foreign properties to you.");
                        string propertiesModified = string.Join(", ", keys.Select(k => $"{{{k}={properties[k]}}}").ToArray());
                        Mod.Logger.Error($"Properties: {propertiesModified}");

                        ExitGames.Client.Photon.Hashtable nullified = new ExitGames.Client.Photon.Hashtable();
                        keys.ForEach(v => nullified.Add(v, null));
                        PhotonNetwork.player.SetCustomProperties(nullified);

                        if (!FengGameManagerMKII.IgnoreList.Contains(sender.id))
                        {
                            FengGameManagerMKII.IgnoreList.Add(sender.id);
                        }
                    }
                }
            }
        }

        public static void OnRoomPropertyModification(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            // Remove invalid properties
            if (PhotonNetwork.isMasterClient)
            {
                if (propertiesThatChanged.ContainsKey("sender") && propertiesThatChanged["sender"] is PhotonPlayer)
                {
                    PhotonPlayer sender = (PhotonPlayer)propertiesThatChanged["sender"];
                    if (!sender.isLocal && !sender.isMasterClient)
                    {
                        propertiesThatChanged.StripKeysWithNullValues();
                        List<object> keys = propertiesThatChanged.Keys.ToList();
                        RoomPropertyWhitelist.ForEach(k => keys.Remove(k));

                        if (keys.Count > 0)
                        {
                            Mod.Logger.Error($"#{sender.id} applied foreign properties to the room.");
                            string propertiesModified = string.Join(", ", keys.Select(k => $"{{{k}={propertiesThatChanged[k]}}}").ToArray());
                            Mod.Logger.Error($"Properties: {propertiesModified}");

                            ExitGames.Client.Photon.Hashtable nullified = new ExitGames.Client.Photon.Hashtable();
                            keys.ForEach(v => nullified.Add(v, null));
                            PhotonNetwork.room.SetCustomProperties(nullified);
                        }

                        // Restore Joinability
                        if (PhotonNetwork.room.expectedJoinability != PhotonNetwork.room.open)
                        {
                            PhotonNetwork.room.open = PhotonNetwork.room.expectedJoinability;
                        }

                        // Restore Visibility
                        if (PhotonNetwork.room.expectedVisibility != PhotonNetwork.room.visible)
                        {
                            PhotonNetwork.room.visible = PhotonNetwork.room.expectedVisibility;
                        }

                        // Restore MaxPlayers
                        if (PhotonNetwork.room.expectedMaxPlayers != PhotonNetwork.room.maxPlayers)
                        {
                            PhotonNetwork.room.maxPlayers = PhotonNetwork.room.expectedMaxPlayers;
                        }
                    }
                }
            }
        }
    }
}
