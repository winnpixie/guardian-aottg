using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Guardian.AntiAbuse
{
    class NetworkPatches
    {
        public static List<string> PropertyWhitelist = new List<string>();
        private static List<object> RoomPropertyWhitelist = new List<object>(new object[] {
            (byte)255, (byte)254, (byte)253, (byte)250, (byte)249, (byte)248, "sender", "Map", "Lighting"
        });

        // NetworkingPeer.OnEvent (Code 202)
        public static bool IsInstantiatePacketValid(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer sender)
        {
            bool malformed = evData == null;

            if (!malformed)
            {
                if (evData.ContainsKey((byte)0) && !(evData[(byte)0] is string))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)1) && !(evData[(byte)1] is Vector3))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)2) && !(evData[(byte)2] is Quaternion))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)3) && !(evData[(byte)3] is int))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)4) && !(evData[(byte)4] is int[]))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)5) && !(evData[(byte)5] is object[]))
                {
                    malformed = true;
                }
                if (evData.ContainsKey((byte)8) && !(evData[(byte)8] is short))
                {
                    malformed = true;
                }
                malformed = malformed || !evData.ContainsKey((byte)0) || !evData.ContainsKey((byte)6) || !evData.ContainsKey((byte)7);
            }

            if (malformed)
            {
                Mod.Logger.Error($"Malformed instantiate from #{(sender == null ? "?" : sender.Id.ToString())}.");
                if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(sender.Id);
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
                if (data.ContainsKey((byte)0) && !(data[(byte)0] is int))
                {
                    malformed = true;
                }
                if (data.ContainsKey((byte)1) && !(data[(byte)1] is object[]))
                {
                    malformed = true;
                }
                malformed = malformed || !data.ContainsKey((byte)0) || !data.ContainsKey((byte)1);
            }

            if (malformed)
            {
                Mod.Logger.Error($"Malformed serialization from #{(sender == null ? "?" : sender.Id.ToString())}.");
                if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(sender.Id);
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
                if (rpcData.ContainsKey((byte)0) && !(rpcData[(byte)0] is int))
                {
                    malformed = true;
                }
                if (rpcData.ContainsKey((byte)1) && !(rpcData[(byte)1] is short))
                {
                    malformed = true;
                }
                if (rpcData.ContainsKey((byte)2) && !(rpcData[(byte)2] is int))
                {
                    malformed = true;
                }
                if (rpcData.ContainsKey((byte)3) && !(rpcData[(byte)3] is string))
                {
                    malformed = true;
                }
                if (rpcData.ContainsKey((byte)4) && !(rpcData[(byte)4] is object[]))
                {
                    malformed = true;
                }
                if (rpcData.ContainsKey((byte)5) && !(rpcData[(byte)5] is byte))
                {
                    malformed = true;
                }
                malformed = malformed || !(rpcData.ContainsKey((byte)0) && (rpcData.ContainsKey((byte)5) || rpcData.ContainsKey((byte)3)));
            }

            if (malformed)
            {
                Mod.Logger.Error($"Malformed RPC from #{(sender == null ? "?" : sender.Id.ToString())}.");
                if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(sender.Id);
                }

                return false;
            }

            return true;
        }

        public static bool IsStateChangeValid(PhotonPlayer sender)
        {
            if (sender != null)
            {
                Mod.Logger.Error($"State Change from #{sender.Id}.");
                if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(sender.Id);
                }

                return false;
            }

            return true;
        }

        public static void OnPlayerPropertyModification(object[] playerAndUpdatedProps)
        {
            PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
            ExitGames.Client.Photon.Hashtable properties = playerAndUpdatedProps[1] as ExitGames.Client.Photon.Hashtable;

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
                        Mod.Logger.Error($"#{sender.Id} applied foreign properties to you.");
                        string propertiesModified = string.Join(", ", keys.Select(k => $"{{{k}={properties[k]}}}").ToArray());
                        Mod.Logger.Error($"Properties: {propertiesModified}");

                        ExitGames.Client.Photon.Hashtable nullified = new ExitGames.Client.Photon.Hashtable();
                        keys.ForEach(v => nullified.Add(v, null));
                        PhotonNetwork.player.SetCustomProperties(nullified);

                        if (!FengGameManagerMKII.IgnoreList.Contains(sender.Id))
                        {
                            FengGameManagerMKII.IgnoreList.Add(sender.Id);
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
                            Mod.Logger.Error($"#{sender.Id} applied foreign properties to the room.");
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
