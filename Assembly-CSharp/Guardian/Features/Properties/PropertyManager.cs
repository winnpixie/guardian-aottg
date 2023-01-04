using Guardian.Utilities;
using System.IO;
using System.Text;
using UnityEngine;

namespace Guardian.Features.Properties
{
    class PropertyManager : FeatureManager<Property>
    {
        private readonly string _dataPath = GuardianClient.RootDir + "\\GameSettings.txt";

        // Gamemodes
        public Property<bool> BombsKillTitans = new Property<bool>("Gamemodes_Bomb:BombsKillTitans", new string[0], true);
        public Property<bool> UseSkyBarrier = new Property<bool>("Gamemodes_Bomb:UseSkyBarrier", new string[0], true);

        // Master Client
        public Property<bool> AnnounceRoundTime = new Property<bool>("MC_AnnounceRoundTime", new string[0], true);
        public Property<bool> AnnounceWaveTime = new Property<bool>("MC_AnnounceWaveTime", new string[0], true);
        public Property<bool> EndlessTitans = new Property<bool>("MC_EndlessTitans", new string[0], false);
        public Property<bool> InfiniteRoom = new Property<bool>("MC_InfiniteRoom", new string[0], true);
        public Property<bool> OGPunkHair = new Property<bool>("MC_OGPunkHair", new string[0], true);
        public Property<bool> DeadlyHooks = new Property<bool>("MC_DeadlyHooks", new string[0], false);
        public Property<bool> FatalCollisions = new Property<bool>("MC_FatalCollisions", new string[0], false);
        public Property<int> FatalSpeedDelta = new Property<int>("MC_FatalSpeedDelta", new string[0], 75);
        public Property<string> CollisionsDeathMessage = new Property<string>("MC_CollisionsDeathMessage", new string[0], "[FF4444]Blunt force trauma");
        public Property<bool> HideNames = new Property<bool>("MC_DisableNames", new string[0], false);

        // Assets
        public Property<string> ThunderSpearSkin = new Property<string>("Assets_ThunderSpearSkin", new string[0], string.Empty);
        public Property<string> LeftRopeSkin = new Property<string>("Assets_LeftRopeSkin", new string[0], string.Empty);
        public Property<float> LeftRopeTileScale = new Property<float>("Assets_LeftRopeTileScale", new string[0], 1f);
        public Property<string> RightRopeSkin = new Property<string>("Assets_RightRopeSkin", new string[0], string.Empty);
        public Property<float> RightRopeTileScale = new Property<float>("Assets_RightRopeTileScale", new string[0], 1f);

        // Player
        public Property<bool> UseRawInput = new Property<bool>("Player_RawTPS-WOWInput", new string[0], true);
        public Property<bool> DoubleTapBurst = new Property<bool>("Player_DoubleTapBurst", new string[0], true);
        public Property<bool> AlternateIdle = new Property<bool>("Player_AHSSIdle", new string[0], false);
        public Property<bool> AlternateBurst = new Property<bool>("Player_CrossBurst", new string[0], false);
        public Property<bool> HideHookArrows = new Property<bool>("Player_HideHookArrows", new string[0], false);
        public Property<bool> HoldForBladeTrails = new Property<bool>("Player_HoldForBladeTrails", new string[0], true);
        public Property<float> ReelOutScrollSmoothing = new Property<float>("Player_ReelOutScrollSmoothing", new string[0], 0.2f);
        public Property<float> OpacityOfOwnName = new Property<float>("Player_OpacityOfOwnName", new string[0], 1.0f);
        public Property<float> OpacityOfOtherNames = new Property<float>("Player_OpacityOfOtherNames", new string[0], 1.0f);
        public Property<bool> DirectionalFlares = new Property<bool>("Player_DirectionalFlares", new string[0], true);
        public Property<string> SuicideMessage = new Property<string>("Player_SuicideMessage", new string[0], "[FFFFFF]Suicide[-]");
        public Property<string> LavaDeathMessage = new Property<string>("Player_LavaDeathMessage", new string[0], "[FF4444]Lava[-]");
        public Property<int> LocalMinDamage = new Property<int>("Player_LocalMinDamage", new string[0], 10);

        // Chat
        public Property<int> MaxChatLines = new Property<int>("Chat_MaxMessages", new string[0], 100);
        public Property<bool> ChatTimestamps = new Property<bool>("Chat_Timestamps", new string[0], false);
        public Property<bool> DrawChatBackground = new Property<bool>("Chat_DrawBackground", new string[0], true);

        public Property<bool> TranslateIncoming = new Property<bool>("Chat_TranslateIncoming", new string[0], false);
        public Property<string> IncomingLanguage = new Property<string>("Chat_IncomingLanguage", new string[0], "auto");
        public Property<bool> TranslateOutgoing = new Property<bool>("Chat_TranslateOutgoing", new string[0], false);
        public Property<string> OutgoingLanguage = new Property<string>("Chat_OutgoingLanguage", new string[0], GuardianClient.SystemLanguage);

        public Property<string> JoinMessage = new Property<string>("Chat_JoinMessage", new string[0], string.Empty);
        public Property<string> ChatName = new Property<string>("Chat_UserName", new string[0], string.Empty);
        public Property<bool> BoldName = new Property<bool>("Chat_BoldName", new string[0], false);
        public Property<bool> ItalicName = new Property<bool>("Chat_ItalicName", new string[0], false);
        public Property<string> TextColor = new Property<string>("Chat_TextColor", new string[0], string.Empty);
        public Property<string> TextPrefix = new Property<string>("Chat_TextPrefix", new string[0], string.Empty);
        public Property<string> TextSuffix = new Property<string>("Chat_TextSuffix", new string[0], string.Empty);
        public Property<bool> BoldText = new Property<bool>("Chat_BoldText", new string[0], false);
        public Property<bool> ItalicText = new Property<bool>("Chat_ItalicText", new string[0], false);

        // Visual [Render]
        public Property<bool> Interpolation = new Property<bool>("Visual_Lerp", new string[0], true);
        public Property<int> DrawDistance = new Property<int>("Visual_DrawDistance", new string[0], 1500);
        public Property<int> FieldOfView = new Property<int>("Visual_FieldOfView", new string[0], 50); // TODO: Re-work
        public Property<bool> Blur = new Property<bool>("Visual_Blur", new string[0], false);
        public Property<bool> UseMainLightColor = new Property<bool>("Visual_CustomMainLightColor", new string[0], true);
        public Property<string> MainLightColor = new Property<string>("Visual_MainLightColor", new string[0], "FFFFFFFF");
        public Property<bool> Fog = new Property<bool>("Visual_Fog", new string[0], true);
        public Property<string> FogColor = new Property<string>("Visual_FogColor", new string[0], "18181865");
        public Property<float> FogDensity = new Property<float>("Visual_FogDensity", new string[0], 0.01f);
        public Property<bool> SoftShadows = new Property<bool>("Visual_SoftShadows", new string[0], false);
        // Visual [Misc]
        public Property<float> CameraTiltStrength = new Property<float>("Visual_CameraTiltStrength", new string[0], 1);
        public Property<string> Flare1Color = new Property<string>("Visual_Flare1Color", new string[0], "00FF007B");
        public Property<string> Flare2Color = new Property<string>("Visual_Flare2Color", new string[0], "FF00007B");
        public Property<string> Flare3Color = new Property<string>("Visual_Flare3Color", new string[0], "00000087");
        public Property<bool> EmissiveFlares = new Property<bool>("Visual_EmissiveFlares", new string[0], true);
        public Property<bool> ShowPlayerMods = new Property<bool>("Visual_ShowPlayerMods", new string[0], true);
        public Property<bool> ShowPlayerPings = new Property<bool>("Visual_ShowPlayerPings", new string[0], true);
        public Property<bool> FPSCamera = new Property<bool>("Visual_FPSCamera", new string[0], false);
        public Property<bool> MultiplayerNapeMeat = new Property<bool>("Visual_MultiplayerNapeMeat", new string[0], false);

        // Misc
        public Property<bool> UseRichPresence = new Property<bool>("Misc_DiscordPresence", new string[0], true);
        public Property<string> PhotonAppId = new Property<string>("Misc_PhotonAppId", new string[0], string.Empty);
        public Property<string> PhotonUserId = new Property<string>("Misc_PhotonUserId", new string[0], string.Empty);

        // Debug
        public Property<bool> ShowFramerate = new Property<bool>("Debug_ShowFramerate", new string[0], true);
        public Property<bool> ShowCoordinates = new Property<bool>("Debug_ShowCoordinates", new string[0], true);
        public Property<int> MaxLogLines = new Property<int>("Debug_MaxLogEntries", new string[0], 100);
        public Property<bool> ShowLog = new Property<bool>("Debug_ShowDebug", new string[0], true);
        public Property<bool> DrawDebugBackground = new Property<bool>("Debug_DrawBackground", new string[0], true);

        public override void Load()
        {
            // Gamemodes
            base.Add(BombsKillTitans);
            base.Add(UseSkyBarrier);

            // Master-Client
            base.Add(AnnounceRoundTime);
            base.Add(AnnounceWaveTime);
            base.Add(EndlessTitans);
            base.Add(InfiniteRoom);
            base.Add(OGPunkHair);
            base.Add(DeadlyHooks);
            base.Add(FatalCollisions);
            base.Add(FatalSpeedDelta);
            base.Add(CollisionsDeathMessage);
            base.Add(HideNames);

            // Assets
            base.Add(ThunderSpearSkin);
            base.Add(LeftRopeSkin);
            base.Add(LeftRopeTileScale);
            base.Add(RightRopeSkin);
            base.Add(RightRopeTileScale);

            // Player
            base.Add(UseRawInput);
            base.Add(DoubleTapBurst);
            base.Add(AlternateIdle);
            base.Add(AlternateBurst);
            base.Add(HideHookArrows);
            base.Add(HoldForBladeTrails);
            base.Add(ReelOutScrollSmoothing);

            OpacityOfOwnName.OnValueChanged = () =>
            {
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
                {
                    foreach (HERO hero in FengGameManagerMKII.Instance.Heroes)
                    {
                        if (hero.photonView.isMine && hero.myNetWorkName != null)
                        {
                            hero.myNetWorkName.GetComponent<UILabel>().alpha = GuardianClient.Properties.OpacityOfOwnName.Value;
                        }
                    }
                }
            };
            base.Add(OpacityOfOwnName);

            OpacityOfOtherNames.OnValueChanged = () =>
            {
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
                {
                    foreach (HERO hero in FengGameManagerMKII.Instance.Heroes)
                    {
                        if (!hero.photonView.isMine && hero.myNetWorkName != null)
                        {
                            hero.myNetWorkName.GetComponent<UILabel>().alpha = GuardianClient.Properties.OpacityOfOtherNames.Value;
                        }
                    }
                }
            };
            base.Add(OpacityOfOtherNames);

            base.Add(DirectionalFlares);
            base.Add(SuicideMessage);
            base.Add(LavaDeathMessage);
            base.Add(LocalMinDamage);

            // Chat
            base.Add(MaxChatLines);
            base.Add(ChatTimestamps);
            base.Add(DrawChatBackground);

            base.Add(TranslateIncoming);
            base.Add(IncomingLanguage);
            base.Add(TranslateOutgoing);
            base.Add(OutgoingLanguage);

            base.Add(JoinMessage);
            base.Add(ChatName);
            base.Add(TextColor);
            base.Add(BoldName);
            base.Add(ItalicName);
            base.Add(TextPrefix);
            base.Add(TextSuffix);
            base.Add(BoldText);
            base.Add(ItalicText);

            // Visual [Render]
            Interpolation.OnValueChanged = () =>
            {
                HERO myHero = IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer ?
                    FengGameManagerMKII.Instance.Heroes[0] : PhotonNetwork.player.GetHero();

                if (myHero != null)
                {
                    myHero.rigidbody.interpolation = Interpolation.Value ? RigidbodyInterpolation.Interpolate
                        : RigidbodyInterpolation.None;
                }
            };
            base.Add(Interpolation);

            DrawDistance.OnValueChanged = () =>
            {
                if (Camera.main != null)
                {
                    Camera.main.farClipPlane = DrawDistance.Value;
                }
            };
            base.Add(DrawDistance);

            Blur.OnValueChanged = () =>
            {
                if (Camera.main != null)
                {
                    TiltShift tiltShift = Camera.main.GetComponent<TiltShift>();
                    if (tiltShift != null)
                    {
                        tiltShift.enabled = Blur.Value && (FengGameManagerMKII.Level != null && !FengGameManagerMKII.Level.Name.StartsWith("Custom"));
                    }
                }
            };
            base.Add(Blur);

            UseMainLightColor.OnValueChanged = () =>
            {
                MainLightColor.OnValueChanged();
            };
            base.Add(UseMainLightColor);

            MainLightColor.OnValueChanged = () =>
            {
                if (UseMainLightColor.Value)
                {
                    GameObject mainLight = GameObject.Find("mainLight");
                    if (mainLight != null)
                    {
                        Light light = mainLight.GetComponent<Light>();
                        if (mainLight != null)
                        {
                            light.color = MainLightColor.Value.ToColor();
                        }
                    }
                }
            };
            base.Add(MainLightColor);

            Fog.OnValueChanged = () =>
            {
                RenderSettings.fog = Fog.Value;
                RenderSettings.fogMode = FogMode.Linear;
                RenderSettings.fogEndDistance = 650f;
            };
            base.Add(Fog);

            FogColor.OnValueChanged = () =>
            {
                RenderSettings.fogColor = FogColor.Value.ToColor();
            };
            base.Add(FogColor);

            FogDensity.OnValueChanged = () =>
            {
                RenderSettings.fogDensity = FogDensity.Value;
            };
            base.Add(FogDensity);

            SoftShadows.OnValueChanged = () =>
            {
                GameObject mainLight = GameObject.Find("mainLight");
                if (mainLight != null)
                {
                    Light light = mainLight.GetComponent<Light>();
                    if (light != null)
                    {
                        if (SoftShadows.Value)
                        {
                            QualitySettings.shadowCascades = 5;
                            light.shadowBias = 0.04f;
                            light.shadowSoftness = 32f;
                        }
                        else
                        {
                            QualitySettings.shadowCascades = 4;
                            light.shadowBias = 0.15f;
                            light.shadowSoftness = 4f;
                        }
                    }
                }
            };
            base.Add(SoftShadows);

            // Visual [Misc]
            base.Add(CameraTiltStrength);
            base.Add(Flare1Color);
            base.Add(Flare2Color);
            base.Add(Flare3Color);
            base.Add(EmissiveFlares);
            base.Add(ShowPlayerMods);
            base.Add(ShowPlayerPings);
            base.Add(FPSCamera);
            base.Add(MultiplayerNapeMeat);

            // Misc
            base.Add(UseRichPresence);

            PhotonAppId.OnValueChanged = () =>
            {
                Networking.PhotonApplication.Custom.Id = PhotonAppId.Value;
            };
            base.Add(PhotonAppId);

            base.Add(PhotonUserId);

            // Debug
            base.Add(ShowFramerate);
            base.Add(ShowCoordinates);
            base.Add(MaxLogLines);
            base.Add(ShowLog);
            base.Add(DrawDebugBackground);

            GuardianClient.Logger.Debug($"Registered {Elements.Count} properties.");

            LoadFromFile();
            Save();
        }

        public void LoadFromFile()
        {
            GameHelper.TryCreateFile(_dataPath, false);

            foreach (string line in File.ReadAllLines(_dataPath))
            {
                string[] data = line.Split(new char[] { '=' }, 2);
                Property property = Find(data[0]);

                if (property != null)
                {
                    if (property.Value is bool)
                    {
                        if (bool.TryParse(data[1], out bool result))
                        {
                            ((Property<bool>)property).Value = result;
                        }
                    }
                    else if (property.Value is int)
                    {
                        if (int.TryParse(data[1], out int result))
                        {
                            ((Property<int>)property).Value = result;
                        }
                    }
                    else if (property.Value is float)
                    {
                        if (float.TryParse(data[1], out float result))
                        {
                            ((Property<float>)property).Value = result;
                        }
                    }
                    else if (property.Value is string)
                    {
                        ((Property<string>)property).Value = data[1];
                    }
                }
            }
        }

        public override void Save()
        {
            GameHelper.TryCreateFile(_dataPath, false);

            StringBuilder builder = new StringBuilder();
            base.Elements.ForEach(property => builder.AppendLine($"{property.Name}={property.Value}"));

            File.WriteAllText(_dataPath, builder.ToString());
        }
    }
}
