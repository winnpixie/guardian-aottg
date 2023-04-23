using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Guardian.AntiAbuse.Validators
{
    class NetworkValidator
    {
        public static readonly List<object> PropertyWhitelist = new List<object>(new object[] {
            (byte)255, "sender"
        });
        private static readonly List<object> RoomPropertyWhitelist = new List<object>(new object[] {
            (byte)255, (byte)254, (byte)253, (byte)250, (byte)249, (byte)248, "sender"
        });

        public static void Init()
        {
            // Property whitelist
            foreach (FieldInfo field in typeof(PhotonPlayerProperty).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                PropertyWhitelist.Add((string)field.GetValue(null));
            }
        }

        // NetworkingPeer.OnEvent (Code 202)
        public static bool IsInstantiatePacketValid(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer sender)
        {
            if (evData == null
                || (!evData.ContainsKey((byte)0) || !(evData[(byte)0] is string))
                || (evData.ContainsKey((byte)1) && !(evData[(byte)1] is Vector3))
                || (evData.ContainsKey((byte)2) && !(evData[(byte)2] is Quaternion))
                || (evData.ContainsKey((byte)3) && !(evData[(byte)3] is int))
                || (evData.ContainsKey((byte)4) && !(evData[(byte)4] is int[]))
                || (evData.ContainsKey((byte)5) && !(evData[(byte)5] is object[]))
                || !evData.ContainsKey((byte)6)
                || !evData.ContainsKey((byte)7)
                || (evData.ContainsKey((byte)8) && !(evData[(byte)8] is short)))
            {
                GuardianClient.Logger.Error($"E(202) Malformed instantiate from #{(sender == null ? "?" : sender.Id.ToString())}.");
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
            if (data == null
                || (!data.ContainsKey((byte)0) || !(data[(byte)0] is int))
                || (!data.ContainsKey((byte)1) || !(data[(byte)1] is object[])))
            {
                GuardianClient.Logger.Error($"E(201/206) Malformed serialization from #{(sender == null ? "?" : sender.Id.ToString())}.");
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
            if (rpcData == null
                || (!rpcData.ContainsKey((byte)0) || !(rpcData[(byte)0] is int)) // Missing or Invalid View Id
                || (rpcData.ContainsKey((byte)1) && !(rpcData[(byte)1] is short)) // Invalid View Prefix
                || (!rpcData.ContainsKey((byte)2) || !(rpcData[(byte)2] is int)) // Missing or Invalid Timestamp
                || (rpcData.ContainsKey((byte)3) && (!(rpcData[(byte)3] is string) || rpcData.ContainsKey((byte)5))) // Invalid RPC Name or Index is also present
                || (rpcData.ContainsKey((byte)4) && !(rpcData[(byte)4] is object[])) // Invalid RPC Data
                || (rpcData.ContainsKey((byte)5) && (!(rpcData[(byte)5] is byte) || rpcData.ContainsKey((byte)3)))) // Invalid RPC Index or Name is also present
            {
                GuardianClient.Logger.Error($"E(200) Malformed RPC from #{(sender == null ? "?" : sender.Id.ToString())}.");
                if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(sender.Id);
                }

                return false;
            }

            return true;
        }

        // NetworkingPeer.OnEvent (Code 204)
        public static bool IsPVDestroyValid(PhotonView[] views, PhotonPlayer sender)
        {
            if (views != null && views.Length > 0 && views[0].ownerId != sender.Id && !sender.isMasterClient)
            {
                GuardianClient.Logger.Error($"E(204) Object.Destroy from #{sender.Id}.");
                if (!FengGameManagerMKII.IgnoreList.Contains(sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(sender.Id);
                }

                return false;
            }

            return true;
        }

        // NetworkingPeer.OnEvent (Code 228)
        public static bool IsStateChangeValid(PhotonPlayer sender)
        {
            if (sender == null) return true;

            GuardianClient.Logger.Error($"E(228) State Change from #{sender.Id}.");
            if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(sender.Id);
            }

            return false;
        }

        public static void OnPlayerPropertyModified(object[] playerAndUpdatedProps)
        {
            PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
            ExitGames.Client.Photon.Hashtable properties = playerAndUpdatedProps[1] as ExitGames.Client.Photon.Hashtable;

            if (!player.isLocal
                || !properties.ContainsKey("sender")
                || !(properties["sender"] is PhotonPlayer sender)
                || sender.isLocal) return;

            // Remove invalid properties
            properties.StripKeysWithNullValues();
            List<object> keys = properties.Keys.ToList();
            PropertyWhitelist.ForEach(k => keys.Remove(k));

            if (keys.Count < 1) return;

            GuardianClient.Logger.Error($"#{(sender == null ? "?" : sender.Id.ToString())} applied foreign properties to you.");
            string propertiesModified = string.Join(", ", keys.Select(k => $"{{{k}={properties[k]}}}").ToArray());
            GuardianClient.Logger.Error($"Properties: {propertiesModified}");

            ExitGames.Client.Photon.Hashtable nullified = new ExitGames.Client.Photon.Hashtable();
            keys.ForEach(v => nullified.Add(v, null));
            PhotonNetwork.player.SetCustomProperties(nullified);

            if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(sender.Id);
            }
        }

        public static void OnRoomPropertyModified(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            // Remove invalid properties
            if (!PhotonNetwork.isMasterClient
                || !propertiesThatChanged.ContainsKey("sender")
                || !(propertiesThatChanged["sender"] is PhotonPlayer sender)
                || sender.isLocal
                || sender.isMasterClient) return;

            propertiesThatChanged.StripKeysWithNullValues();
            List<object> keys = propertiesThatChanged.Keys.ToList();
            RoomPropertyWhitelist.ForEach(k => keys.Remove(k));

            if (keys.Count > 0)
            {
                GuardianClient.Logger.Error($"#{sender.Id} applied foreign properties to the room.");
                string propertiesModified = string.Join(", ", keys.Select(k => $"{{{k}={propertiesThatChanged[k]}}}").ToArray());
                GuardianClient.Logger.Error($"Properties: {propertiesModified}");

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
