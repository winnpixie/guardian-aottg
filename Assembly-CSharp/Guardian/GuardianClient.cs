using Guardian.AntiAbuse;
using Guardian.AntiAbuse.Validators;
using Guardian.Features.Commands;
using Guardian.Features.Properties;
using Guardian.Features.Gamemodes;
using Guardian.Networking;
using Guardian.UI.Toasts;
using Guardian.Utilities;
using System.Collections;
using UnityEngine;
using Guardian.UI.Impl.Debug;
using Guardian.UI;

namespace Guardian
{
    class GuardianClient : MonoBehaviour
    {
        public static readonly string Build = "1.6.0.1";
        public static readonly string RootDir = Application.dataPath + "\\..";

        public static readonly CommandManager Commands = new CommandManager();
        public static readonly GamemodeManager Gamemodes = new GamemodeManager();
        public static readonly PropertyManager Properties = new PropertyManager();
        public static readonly FrameCounter FpsCounter = new FrameCounter();
        public static readonly ToastManager Toasts = new ToastManager();

        public static readonly Logger Logger = new Logger();
        public static GuiController GuiController;
        public static bool ProgramExiting;
        
        private static bool _firstInit = true;
        private static bool _loadedLevelOnce;

        private void Start()
        {
            // Load custom textures and audio clips
            {
                if (ResourceLoader.TryGetAsset("Custom/Textures/hud.png", out Texture2D hudTextures))
                {
                    GameObject backgroundGo = GameObject.Find("Background");
                    if (backgroundGo != null)
                    {
                        Material uiMat = backgroundGo.GetComponent<UISprite>().material;
                        uiMat.mainTextureScale = hudTextures.GetScaleVector(2048, 2048);
                        uiMat.mainTexture = hudTextures;
                    }
                }

                StartCoroutine(WaitAndSetParticleTextures());
            }

            GuiController = base.gameObject.AddComponent<GuiController>();
            base.gameObject.AddComponent<MicEF>();

            if (!_firstInit) return;
            _firstInit = false;

            string cli = System.Environment.CommandLine;
            if (cli.IndexOf(' ') > 0) Logger.Debug($"CLI:{cli.Substring(cli.IndexOf(' '))}");

            // Load Window Manager service
            WindowManager.Init();

            // Load network validation service
            NetworkValidator.Init();

            // Load skin validation service
            SkinValidator.Init();

            // Load name and guild (if possible)
            string playerName = PlayerPrefs.GetString("name", string.Empty);
            string playerGuild = PlayerPrefs.GetString("guildname", string.Empty);
            if (playerName.StripNGUI().Length > 0) LoginFengKAI.Player.Name = playerName;
            LoginFengKAI.Player.Guild = playerGuild;

            // Load various features
            Commands.Load();
            Gamemodes.Load();
            Properties.Load();

            // Load emotes
            EmoteHelper.Load();

            // Check for an update
            StartCoroutine(UpdateChecker.CheckForUpdate());

            DiscordHelper.StartTime = GameHelper.CurrentTimeMillis();
            DiscordHelper.Initialize();
        }

        private IEnumerator WaitAndSetParticleTextures()
        {
            // Load custom textures and audio clips
            ResourceLoader.TryGetAsset("Custom/Textures/dust.png", out Texture2D dustTexture);
            ResourceLoader.TryGetAsset("Custom/Textures/blood.png", out Texture2D bloodTexture);
            ResourceLoader.TryGetAsset("Custom/Textures/gun_smoke.png", out Texture2D gunSmokeTexture);

            while (true)
            {
                foreach (ParticleSystem ps in UnityEngine.Object.FindObjectsOfType<ParticleSystem>())
                {
                    if (dustTexture != null
                        && (ps.name.Contains("smoke")
                            || ps.name.StartsWith("boom")
                            || ps.name.StartsWith("bite")
                            || ps.name.StartsWith("Particle System 2")
                            || ps.name.StartsWith("Particle System 3")
                            || ps.name.StartsWith("Particle System 4")
                            || ps.name.Contains("colossal_steam")
                            || ps.name.Contains("FXtitan")
                            || ps.name.StartsWith("dust"))
                        && !ps.name.StartsWith("3dmg"))
                    {
                        ps.renderer.material.mainTexture = dustTexture;
                    }

                    if (bloodTexture != null && ps.name.Contains("blood"))
                    {
                        ps.renderer.material.mainTexture = bloodTexture;
                    }

                    if (gunSmokeTexture != null && ps.name.Contains("shotGun"))
                    {
                        ps.renderer.material.mainTexture = gunSmokeTexture;
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        public void ApplyCustomRenderSettings()
        {
            Properties.DrawDistance.OnValueChanged();
            Properties.Blur.OnValueChanged();
            // Custom MainLight Color is handled in IN_GAME_MAIN_CAMERA
            Properties.Fog.OnValueChanged();
            Properties.FogColor.OnValueChanged();
            Properties.FogDensity.OnValueChanged();
            Properties.SoftShadows.OnValueChanged();
        }

        private void Update()
        {
            if (PhotonNetwork.isMasterClient) Gamemodes.CurrentMode.OnUpdate();

            DiscordHelper.RunCallbacks();

            FpsCounter.UpdateCounter();
        }

        private void OnLevelWasLoaded(int level)
        {
            ApplyCustomRenderSettings();

            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || PhotonNetwork.offlineMode)
            {
                string difficulty = IN_GAME_MAIN_CAMERA.Difficulty switch
                {
                    -1 => "Training",
                    0 => "Normal",
                    1 => "Hard",
                    2 => "Abnormal",
                    _ => "Unknown"
                };

                DiscordHelper.SetPresence(new Discord.Activity
                {
                    Details = "Playing Offline.",
                    State = $"{FengGameManagerMKII.Level.DisplayName} / {difficulty}"
                });
            }

            if (PhotonNetwork.isMasterClient) Gamemodes.CurrentMode.OnReset();

            if (_loadedLevelOnce) { return; }
            _loadedLevelOnce = true;

            string joinMessage = Properties.JoinMessage.Value;
            if (joinMessage.Length < 1) return;
            if (joinMessage.StripNGUI().Length > 0) joinMessage = joinMessage.NGUIToUnity();
            Commands.Find("say").Execute(InRoomChat.Instance, joinMessage.Split(' '));
        }

        private void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            if (PhotonNetwork.isMasterClient) Gamemodes.CurrentMode.OnPlayerJoin(player);

            Logger.Info($"({player.Id}) " + player.Username.NGUIToUnity() + " connected.".AsColor("00FF00"));
        }

        private void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            if (PhotonNetwork.isMasterClient) Gamemodes.CurrentMode.OnPlayerLeave(player);

            Logger.Info($"({player.Id}) " + player.Username.NGUIToUnity() + " disconnected.".AsColor("FF0000"));
        }

        private void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            NetworkValidator.OnPlayerPropertyModified(playerAndUpdatedProps);

            ModDetector.OnPlayerPropertyModified(playerAndUpdatedProps);
        }

        private void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            NetworkValidator.OnRoomPropertyModified(propertiesThatChanged);

            PhotonPlayer sender = null;
            if (propertiesThatChanged.ContainsKey("sender") && propertiesThatChanged["sender"] is PhotonPlayer player)
            {
                sender = player;
            }

            if (sender != null && !sender.isMasterClient) return;

            if (propertiesThatChanged.ContainsKey("Map") && propertiesThatChanged["Map"] is string mapName)
            {
                LevelInfo levelInfo = LevelInfo.GetInfo(mapName);
                if (levelInfo != null) FengGameManagerMKII.Level = levelInfo;
            }

            if (propertiesThatChanged.ContainsKey("Lighting") && propertiesThatChanged["Lighting"] is string lightLevel
                && GExtensions.TryParseEnum(lightLevel, out DayLight time))
            {
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetLighting(time);
            }
        }

        private void OnJoinedLobby()
        {
            // TODO: Begin working on Friend system with Photon Friend API
            PhotonNetwork.playerName = Properties.PhotonUserId.Value;
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

            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                 { GuardianPlayerProperty.GuardianMod, Build }
            });

            StartCoroutine(UpdateMyPing());

            string[] roomInfo = PhotonNetwork.room.name.Split('`');
            if (roomInfo.Length < 7) return;

            DiscordHelper.SetPresence(new Discord.Activity
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
                    PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                    {
                        { GuardianPlayerProperty.Ping, newPing }
                    });
                }

                yield return new WaitForSeconds(3f);
            }
        }

        private void OnLeftRoom()
        {
            Gamemodes.CurrentMode.CleanUp();

            PhotonNetwork.SetPlayerCustomProperties(null);

            // FIXME: Why don't these properly reset?
            RCSettings.BombCeiling = false;
            RCSettings.HideNames = false;

            SyncedSettings.InfiniteGas = false;
            SyncedSettings.InfiniteAmmo = false;

            DiscordHelper.SetPresence(new Discord.Activity
            {
                Details = "Idle..."
            });
        }

        private void OnConnectionFail(DisconnectCause cause)
        {
            Logger.Warn($"OnConnectionFail ({cause})");
        }

        private void OnPhotonRoomJoinFailed(object[] codeAndMsg)
        {
            Logger.Error("OnPhotonRoomJoinFailed");

            foreach (object obj in codeAndMsg)
            {
                Logger.Error($" - {obj}");
            }
        }

        private void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            Logger.Error("OnPhotonCreateRoomFailed");

            foreach (object obj in codeAndMsg)
            {
                Logger.Error($" - {obj}");
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            WindowManager.HandleWindowFocusEvent(hasFocus);

            if (!hasFocus || IN_GAME_MAIN_CAMERA.Gametype == GameType.Stop) return;

            // Minimap turns white
            if (Minimap.Instance != null) Minimap.WaitAndTryRecaptureInstance(0.5f);

            // TPS crosshair ending up where it shouldn't
            if (IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS)
            {
                WindowManager.SetCursorStates(shown: Screen.showCursor, locked: false);
                WindowManager.SetCursorStates(shown: Screen.showCursor, locked: true);
            }
        }

        private void OnApplicationQuit()
        {
            ProgramExiting = true;

            Properties.Save();

            DiscordHelper.Dispose();
        }
    }
}