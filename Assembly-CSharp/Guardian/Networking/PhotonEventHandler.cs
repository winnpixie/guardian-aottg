using System.Collections;
using Discord;
using Guardian.AntiAbuse;
using Guardian.AntiAbuse.Validators;
using Guardian.Utilities;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Guardian.Networking
{
    public class PhotonEventHandler : MonoBehaviour
    {
        private static bool _loadedLevelOnce;

        private void OnLevelWasLoaded(int level)
        {
            if (_loadedLevelOnce)
            {
                return;
            }

            _loadedLevelOnce = true;

            string joinMessage = GuardianClient.Properties.JoinMessage.Value;
            if (joinMessage.Length < 1)
            {
                return;
            }

            if (joinMessage.StripNGUI().Length > 0)
            {
                joinMessage = joinMessage.NGUIToUnity();
            }

            GuardianClient.Commands.Find("say").Execute(InRoomChat.Instance, joinMessage.Split(' '));
        }

        private void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            GuardianClient.Logger.Info($"({player.Id}) " + player.Username.NGUIToUnity() +
                                       " connected.".AsColor("00FF00"));
        }

        private void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            GuardianClient.Logger.Info($"({player.Id}) " + player.Username.NGUIToUnity() +
                                       " disconnected.".AsColor("FF0000"));
        }

        private void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            NetworkValidator.OnPlayerPropertyModified(playerAndUpdatedProps);

            ModDetector.OnPlayerPropertyModified(playerAndUpdatedProps);
        }

        private void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
        {
            NetworkValidator.OnRoomPropertyModified(propertiesThatChanged);

            PhotonPlayer sender = null;
            if (propertiesThatChanged.ContainsKey("sender") && propertiesThatChanged["sender"] is PhotonPlayer player)
            {
                sender = player;
            }

            if (sender != null && !sender.isMasterClient)
            {
                return;
            }

            if (propertiesThatChanged.ContainsKey("Map") && propertiesThatChanged["Map"] is string mapName)
            {
                LevelInfo levelInfo = LevelInfo.GetInfo(mapName);
                if (levelInfo != null) FengGameManagerMKII.Level = levelInfo;
            }

            if (propertiesThatChanged.ContainsKey("Lighting") && propertiesThatChanged["Lighting"] is string lightLevel
                                                              && GExtensions.TryParseEnum(lightLevel,
                                                                  out DayLight time))
            {
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetLighting(time);
            }
        }

        private void OnJoinedLobby()
        {
            // TODO: Begin working on Friend system with Photon Friend API
            PhotonNetwork.playerName = GuardianClient.Properties.PhotonUserId.Value;
        }

        private void OnJoinedRoom()
        {
            _loadedLevelOnce = false;

            // TODO: Potentially use custom event/group combo to sync game-settings whilst not triggering other mods
            int[] groups = new int[byte.MaxValue];
            for (int i = 0; i < byte.MaxValue; i++)
            {
                groups[i] = i + 1;
            }

            PhotonNetwork.SetReceivingEnabled(groups, null);
            PhotonNetwork.SetSendingEnabled(groups, null);

            PhotonNetwork.player.SetCustomProperties(new Hashtable
            {
                { GuardianPlayerProperty.GuardianMod, GuardianClient.Build }
            });

            StartCoroutine(UpdateMyPing());

            string[] roomInfo = PhotonNetwork.room.name.Split('`');
            if (roomInfo.Length < 7) return;

            DiscordHelper.SetPresence(new Activity
            {
                Details = $"Playing in {(roomInfo[5].Length < 1 ? string.Empty : "[PWD]")} {roomInfo[0].StripNGUI()}",
                State = $"({NetworkHelper.GetRegionCode().ToUpper()}) {roomInfo[1]} / {roomInfo[2].ToUpper()}"
            });
        }

        private IEnumerator UpdateMyPing()
        {
            while (PhotonNetwork.inRoom)
            {
                int currentPing = PhotonNetwork.player.Ping;
                int newPing = PhotonNetwork.GetPing();

                if (newPing != currentPing)
                {
                    PhotonNetwork.player.SetCustomProperties(new Hashtable
                    {
                        { GuardianPlayerProperty.Ping, newPing }
                    });
                }

                yield return new WaitForSeconds(3f);
            }
        }

        private void OnLeftRoom()
        {
            PhotonNetwork.SetPlayerCustomProperties(null);

            // FIXME: Why don't these properly reset?
            RCSettings.BombCeiling = false;
            RCSettings.HideNames = false;

            SyncedSettings.InfiniteGas = false;
            SyncedSettings.InfiniteAmmo = false;

            DiscordHelper.SetPresence(new Activity
            {
                Details = "Idle..."
            });
        }

        private void OnConnectionFail(DisconnectCause cause)
        {
            GuardianClient.Logger.Warn($"OnConnectionFail ({cause})");
        }

        private void OnPhotonRoomJoinFailed(object[] codeAndMsg)
        {
            GuardianClient.Logger.Error("OnPhotonRoomJoinFailed");

            foreach (object obj in codeAndMsg)
            {
                GuardianClient.Logger.Error($" - {obj}");
            }
        }

        private void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            GuardianClient.Logger.Error("OnPhotonCreateRoomFailed");

            foreach (object obj in codeAndMsg)
            {
                GuardianClient.Logger.Error($" - {obj}");
            }
        }
    }
}