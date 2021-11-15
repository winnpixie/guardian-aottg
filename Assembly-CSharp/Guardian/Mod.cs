using Guardian.Features.Commands;
using Guardian.Features.Properties;
using Guardian.Features.Gamemodes;
using Guardian.Networking;
using Guardian.Utilities;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Guardian
{
    class Mod : MonoBehaviour
    {
        public static string Build = "11152021";
        public static string RootDir = Application.dataPath + "\\..";

        public static CommandManager Commands = new CommandManager();
        public static GamemodeManager Gamemodes = new GamemodeManager();
        public static PropertyManager Properties = new PropertyManager();
        public static UI.UIManager Menus;
        public static Regex BlacklistedTags = new Regex("<(\\/?)(size|material|quad)(.*)>", RegexOptions.IgnoreCase);
        public static Logger Logger = new Logger();
        public static bool IsProgramQuitting = false;
        public static FrameCounter FpsCounter = new FrameCounter();

        public static string SystemLanguage => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        private static bool s_initialized = false;
        private static bool s_firstJoin = true;

        void Start()
        {
            // Load custom textures and audio clips
            {
                if (Gesources.TryGetAsset("Custom/Textures/hud.png", out Texture2D hudTextures))
                {
                    GameObject backgroundGo = GameObject.Find("Background");
                    if (backgroundGo != null)
                    {
                        Material uiMat = backgroundGo.GetComponent<UISprite>().material;
                        uiMat.mainTextureScale = Gesources.Scale(hudTextures, 2048, 2048);
                        uiMat.mainTexture = hudTextures;
                    }
                }

                StartCoroutine(CoWaitAndSetParticleTexture());
            }

            if (!s_initialized)
            {
                // Load skin validation service
                AntiAbuse.Validators.Skins.Init();

                // Load name and guild (if possible)
                FengGameManagerMKII.NameField = PlayerPrefs.GetString("name", string.Empty);
                if (FengGameManagerMKII.NameField.Uncolored().Length == 0)
                {
                    FengGameManagerMKII.NameField = LoginFengKAI.Player.Name;
                }
                LoginFengKAI.Player.Guild = PlayerPrefs.GetString("guildname", string.Empty);

                // Load various features
                Commands.Load();
                Gamemodes.Load();
                Properties.Load();

                // Load network validation service
                AntiAbuse.Validators.Network.Init();

                s_initialized = true;

                DiscordHelper.StartTime = GameHelper.CurrentTimeMillis();

                // Check for an update
                StartCoroutine(CoCheckForUpdate());

            }

            Menus = base.gameObject.AddComponent<UI.UIManager>();
            base.gameObject.AddComponent<MicEF>();
        }

        private IEnumerator CoCheckForUpdate()
        {
            Logger.Info("Checking for update...");
            Logger.Info($"Installed: {Build}");

            using WWW www = new WWW("https://aottg.tk/mods/guardian/version.txt?t=" + GameHelper.CurrentTimeMillis()); // Random long to try and avoid cache issues
            yield return www;

            if (www.error != null)
            {
                Logger.Error(www.error);

                Logger.Error($"\nIf errors persist, contact me on Discord!");
                Logger.Info("Discord:");
                Logger.Info($"\t- {"https://cb.run/FFT".AsColor("0099FF")}");

                try
                {
                    GameObject.Find("VERSION").GetComponent<UILabel>().text = "[FF0000]Could not verify version.[-] If errors persists, contact me @ [0099FF]https://cb.run/FFT[-]!";
                }
                catch { }
            }
            else
            {
                string latestVersion = www.text.Split('\n')[0];
                Logger.Info("Latest: " + latestVersion);

                if (!latestVersion.Equals(Build))
                {
                    Logger.Info($"You are {"OUTDATED".AsBold().AsItalic().AsColor("FF0000")}, please update using the launcher!");
                    Logger.Info("Launcher Download:");
                    Logger.Info($"\t- {"https://cb.run/GuardianAoT".AsColor("0099FF")}");

                    try
                    {
                        GameObject.Find("VERSION").GetComponent<UILabel>().text = "[FF0000]Outdated![-] Please update using the launcher @ [0099FF]https://cb.run/GuardianAoT[-]!";
                    }
                    catch { }
                }
                else
                {
                    Logger.Info($"You are {"UP TO DATE".AsBold().AsItalic().AsColor("AAFF00")}, yay!");
                }
            }
        }

        private IEnumerator CoWaitAndSetParticleTexture()
        {
            yield return new WaitForSeconds(0.1f);

            // TODO: Load custom textures and audio clips
            {
                Gesources.TryGetAsset("Custom/Textures/dust.png", out Texture2D dustTexture);
                Gesources.TryGetAsset("Custom/Textures/blood.png", out Texture2D bloodTexture);
                Gesources.TryGetAsset("Custom/Textures/gun_smoke.png", out Texture2D gunSmokeTexture);

                foreach (ParticleSystem ps in UnityEngine.Object.FindObjectsOfType<ParticleSystem>())
                {
                    if (dustTexture != null)
                    {
                        if ((ps.name.Contains("smoke") || ps.name.StartsWith("boom") || ps.name.StartsWith("bite")
                            || ps.name.StartsWith("Particle System 2") || ps.name.StartsWith("Particle System 3")
                            || ps.name.StartsWith("Particle System 4") || ps.name.Contains("colossal_steam")
                            || ps.name.Contains("FXtitan") || ps.name.StartsWith("dust")) && !ps.name.StartsWith("3dmg"))
                        {
                            ps.renderer.material.mainTexture = dustTexture;
                        }
                    }

                    if (bloodTexture != null)
                    {
                        if (ps.name.Contains("blood"))
                        {
                            ps.renderer.material.mainTexture = bloodTexture;
                        }
                    }

                    if (gunSmokeTexture != null)
                    {
                        if (ps.name.Contains("shotGun"))
                        {
                            ps.renderer.material.mainTexture = gunSmokeTexture;
                        }
                    }
                }
            }

            StartCoroutine(CoWaitAndSetParticleTexture());
        }

        public void ApplyCustomRenderSettings()
        {
            Properties.DrawDistance.OnValueChanged();
            Properties.Fog.OnValueChanged();
            Properties.SoftShadows.OnValueChanged();
        }

        void Update()
        {
            if (PhotonNetwork.isMasterClient)
            {
                Gamemodes.CurrentMode.OnUpdate();
            }

            DiscordHelper.RunCallbacks();

            FpsCounter.UpdateCounter();
        }

        void OnLevelWasLoaded(int level)
        {
            ApplyCustomRenderSettings();

            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || PhotonNetwork.offlineMode)
            {
                string difficulty = "Training";
                switch (IN_GAME_MAIN_CAMERA.Difficulty)
                {
                    case 0:
                        difficulty = "Normal";
                        break;
                    case 1:
                        difficulty = "Hard";
                        break;
                    case 2:
                        difficulty = "Abnormal";
                        break;
                }

                DiscordHelper.SetPresence(new Discord.Activity
                {
                    Details = $"Playing offline.",
                    State = $"{FengGameManagerMKII.Level.Name} / {difficulty}"
                });
            }

            if (PhotonNetwork.isMasterClient)
            {
                Gamemodes.CurrentMode.OnReset();
            }

            if (s_firstJoin)
            {
                s_firstJoin = false;
                string joinMessage = Properties.JoinMessage.Value.ColorParsed();
                if (joinMessage.Uncolored().Length <= 0)
                {
                    joinMessage = Properties.JoinMessage.Value;
                }
                if (joinMessage.Length > 0)
                {
                    Commands.Find("say").Execute(InRoomChat.Instance, joinMessage.Split(' '));
                }
            }
        }

        void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            if (PhotonNetwork.isMasterClient)
            {
                Gamemodes.CurrentMode.OnPlayerJoin(player);
            }

            Logger.Info($"({player.Id}) " + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).ColorParsed() + " connected.".AsColor("00FF00"));
        }

        void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            if (PhotonNetwork.isMasterClient)
            {
                Gamemodes.CurrentMode.OnPlayerLeave(player);
            }

            Logger.Info($"({player.Id}) " + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).ColorParsed() + " disconnected.".AsColor("FF0000"));
        }

        void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            AntiAbuse.Validators.Network.OnPlayerPropertyModification(playerAndUpdatedProps);

            AntiAbuse.ModDetector.OnPlayerPropertyModification(playerAndUpdatedProps);
        }

        void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            AntiAbuse.Validators.Network.OnRoomPropertyModification(propertiesThatChanged);

            if (!s_firstJoin)
            {
                PhotonPlayer sender = null;
                if (propertiesThatChanged.ContainsKey("sender") && propertiesThatChanged["sender"] is PhotonPlayer player)
                {
                    sender = player;
                }

                if (sender == null || sender.isMasterClient)
                {
                    if (propertiesThatChanged.ContainsKey("Map") && propertiesThatChanged["Map"] is string mapName)
                    {
                        LevelInfo levelInfo = LevelInfo.GetInfo(mapName);
                        if (levelInfo != null)
                        {
                            FengGameManagerMKII.Level = levelInfo;
                        }
                    }

                    if (propertiesThatChanged.ContainsKey("Lighting") && propertiesThatChanged["Lighting"] is string lightLevel)
                    {
                        if (GExtensions.TryParseEnum(lightLevel, out DayLight time))
                        {
                            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetLighting(time);
                        }
                    }
                }
            }
        }

        void OnJoinedLobby()
        {
            // TODO: Begin testing with Photon Friends API
            PhotonNetwork.playerName = SystemInfo.deviceUniqueIdentifier;

            // TODO: Potentially use custom event/group combo to sync game-settings whilst not triggering other mods
            int[] groups = new int[byte.MaxValue];
            for (int i = 0; i < byte.MaxValue; i++)
            {
                groups[i] = i + 1;
            }
            PhotonNetwork.SetReceivingEnabled(groups, new int[0]);
            PhotonNetwork.SetSendingEnabled(groups, new int[0]);
        }

        void OnJoinedRoom()
        {
            s_firstJoin = true;

            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                 { PhotonPlayerProperty.GuardianMod, Build }
            });

            string[] roomInfo = PhotonNetwork.room.name.Split('`');
            if (roomInfo.Length > 6)
            {
                DiscordHelper.SetPresence(new Discord.Activity
                {
                    Details = $"Playing in {(roomInfo[5].Length == 0 ? string.Empty : "[PWD]")} {roomInfo[0].Uncolored()}",
                    State = $"({NetworkHelper.GetRegionCode().ToUpper()}) {roomInfo[1]} / {roomInfo[2].ToUpper()}"
                });
            }
        }

        void OnLeftRoom()
        {
            Gamemodes.CurrentMode.CleanUp();

            PhotonNetwork.SetPlayerCustomProperties(null);

            DiscordHelper.SetPresence(new Discord.Activity
            {
                Details = "Idle..."
            });
        }

        void OnConnectionFail(DisconnectCause cause)
        {
            Logger.Warn($"OnConnectionFail ({cause})");
        }

        void OnPhotonRoomJoinFailed(object[] codeAndMsg)
        {
            Logger.Error($"OnPhotonRoomJoinFailed");

            foreach (object obj in codeAndMsg)
            {
                Logger.Error($" - {obj}");
            }
        }

        void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            Logger.Error($"OnPhotonCreateRoomFailed");

            foreach (object obj in codeAndMsg)
            {
                Logger.Error($" - {obj}");
            }
        }

        // Attempts to fix some dumb bugs that occur when you alt-tab
        void OnApplicationFocus(bool hasFocus)
        {
            UI.WindowManager.HandleWindowFocusEvent(hasFocus);

            if (hasFocus)
            {
                if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Stop)
                {
                    // Minimap turning white
                    if (Minimap.Instance != null)
                    {
                        Minimap.WaitAndTryRecaptureInstance(0.1f);
                    }

                    // TPS crosshair ending up where it shouldn't
                    if (IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS)
                    {
                        Screen.lockCursor = false;
                        Screen.lockCursor = true;
                    }
                }
            }
        }

        void OnApplicationQuit()
        {
            IsProgramQuitting = true;

            Properties.Save();

            DiscordHelper.Dispose();
        }
    }
}