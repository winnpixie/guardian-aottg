using System;
using Discord;
using Guardian.AntiAbuse.Validators;
using Guardian.Features.Commands;
using Guardian.Features.Gamemodes;
using Guardian.Features.Properties;
using Guardian.Networking;
using Guardian.UI;
using Guardian.UI.Impl.Debug;
using Guardian.UI.Toasts;
using Guardian.Utilities;
using Guardian.Utilities.Resources;
using UnityEngine;

namespace Guardian
{
    class GuardianClient : MonoBehaviour
    {
        public static readonly string Build = "1.6.0.2";
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

        private void Start()
        {
            GuiController = base.gameObject.AddComponent<GuiController>();
            base.gameObject.AddComponent<MicEF>();
            
            base.gameObject.AddComponent<CustomResourceProcessor>();
            base.gameObject.AddComponent<PhotonEventHandler>();
            base.gameObject.AddComponent<GamemodeNetworkHandler>();

            if (!_firstInit)
            {
                return;
            }
            
            _firstInit = false;

            string cli = Environment.CommandLine;
            if (cli.IndexOf(' ') > 0) Logger.Debug($"CLI:{cli.Substring(cli.IndexOf(' '))}");

            // Load Window Manager service
            WindowManager.Init();

            // Load network validation service
            NetworkValidator.Init();

            // Load skin validation service
            SkinValidator.Init();

            // Load name and guild (if possible)
            string playerName = PlayerPrefs.GetString("name", string.Empty);
            if (playerName.StripNGUI().Length > 0)
            {
                LoginFengKAI.Player.Name = playerName;
            }
            
            string playerGuild = PlayerPrefs.GetString("guildname", string.Empty);
            LoginFengKAI.Player.Guild = playerGuild;

            // Load various features
            Commands.Load();
            Gamemodes.Load();
            Properties.Load();

            // Load emotes
            EmoteHelper.Load();

            // Check for an update
            StartCoroutine(UpdateChecker.CheckForUpdate());
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
            if (PhotonNetwork.isMasterClient)
            {
                Gamemodes.CurrentMode.OnUpdate();
            }

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

                DiscordHelper.SetPresence(new Activity
                {
                    Details = "Playing Offline.",
                    State = $"{FengGameManagerMKII.Level.DisplayName} / {difficulty}"
                });
            }
            
            if (PhotonNetwork.isMasterClient) Gamemodes.CurrentMode.OnReset();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            WindowManager.HandleWindowFocusEvent(hasFocus);

            if (!hasFocus || IN_GAME_MAIN_CAMERA.Gametype == GameType.Stop)
            {
                return;
            }

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