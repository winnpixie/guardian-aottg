using Guardian.Features.Commands;
using Guardian.Utilities;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using Guardian.Features.Properties;
using Guardian.AntiAbuse;
using Guardian.Networking;
using System.Collections;

namespace Guardian
{
    class Mod : MonoBehaviour
    {
        public static Mod Instance;
        public static string Build = "02032021-1";
        public static string RootDir = Application.dataPath + "\\..";
        public static string HostWhitelistPath = RootDir + "\\Hosts.txt";
        public static string MapData = string.Empty;
        public static CommandManager Commands = new CommandManager();
        public static PropertyManager Properties = new PropertyManager();
        public static List<string> HostWhitelist = new List<string>();
        public static Regex BlacklistedTags = new Regex("<\\/?(size(=\\d*)?|quad([^>]*)?|material([^>]*)?)>", RegexOptions.IgnoreCase);
        public static Logger Logger = new Logger();
        private static bool Initialized = false;
        private static bool FirstJoin = true;

        public List<int> Muted = new List<int>();
        public bool IsMultiMap;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            if (!Initialized)
            {
                // Check for an update before doing anything
                StartCoroutine(CheckForUpdate());

                // Host whitelist (for skins)
                if (!File.Exists(HostWhitelistPath))
                {
                    HostWhitelist.Add("i.imgur.com");
                    HostWhitelist.Add("imgur.com");
                    HostWhitelist.Add("cdn.discordapp.com");
                    HostWhitelist.Add("cdn.discord.com");
                    HostWhitelist.Add("media.discordapp.net");
                    HostWhitelist.Add("i.gyazo.com");
                    File.WriteAllLines(HostWhitelistPath, HostWhitelist.ToArray());
                }
                LoadSkinHostWhitelist();

                // Auto-load RC name and guild (if possible)
                FengGameManagerMKII.NameField = PlayerPrefs.GetString("name", string.Empty);
                if (FengGameManagerMKII.NameField.Uncolored().Length == 0)
                {
                    FengGameManagerMKII.NameField = LoginFengKAI.Player.Name;
                }
                LoginFengKAI.Player.Guild = PlayerPrefs.GetString("guildname", string.Empty);

                // Load various features
                Commands.Load();
                Properties.Load();

                // Print out debug information
                Logger.Info($"Installed Version: {Build}");
                Logger.Info($"Unity Version: {Application.unityVersion}");
                Logger.Info($"OS: {SystemInfo.operatingSystem}");
                Logger.Info($"Platform: {Application.platform}");

                // Property whitelist
                NetworkPatches.PropertyWhitelist.Add("sender");
                NetworkPatches.PropertyWhitelist.Add("GuardianMod");
                foreach (FieldInfo field in typeof(PhotonPlayerProperty).GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    NetworkPatches.PropertyWhitelist.Add((string)field.GetValue(null));
                }

                Initialized = true;

                DiscordHelper.StartTime = GameHelper.CurrentTimeMillis();
            }

            base.gameObject.AddComponent<UI.ModUI>();
            base.gameObject.AddComponent<MicEF>();

            DiscordHelper.SetPresence(new Discord.Activity
            {
                Details = $"Staring at the main menu...",
            });
        }

        private IEnumerator CheckForUpdate()
        {
            using (WWW www = new WWW("http://lewd.cf/GUARDIAN_BUILD.TXT?t=" + GameHelper.CurrentTimeMillis()))
            {
                yield return www;

                Logger.Info("Latest Version: " + www.text);

                if (!www.text.Split('\n')[0].Equals(Build))
                {
                    Logger.Error("You are running an outdated build, please update!");
                    Logger.Error("https://tiny.cc/GuardianMod".WithColor("0099ff"));

                    try
                    {
                        GameObject.Find("VERSION").GetComponent<UILabel>().text = "[ff0000]Mod is outdated![-] Please download the latest build from [0099ff]https://tiny.cc/GuardianMod[-]!";
                    }
                    catch { }
                }
            }
        }

        public static void LoadSkinHostWhitelist()
        {
            HostWhitelist = new List<string>(File.ReadAllLines(HostWhitelistPath));
        }

        public static object[] HandleChat(string input, string name)
        {
            // Emotes
            input = input.Replace("<3", "\u2665");
            input = input.Replace(":lenny:", "( ͡° ͜ʖ ͡°)");

            // Color and fading
            string chatColor = Properties.TextColor.Value;
            if (chatColor.Length != 0)
            {
                if (!chatColor.Contains(","))
                {
                    input = input.WithColor(chatColor);
                }
                else
                {
                    string[] colors = chatColor.Split(new char[] { ',' }, 2);

                    if (colors.Length > 1 && colors[0].IsHex() && colors[1].IsHex())
                    {
                        input = GameHelper.Detagger.Replace(input, string.Empty);

                        Color startColor = NGUIMath.IntToColor(int.Parse(colors[0] + "FF", System.Globalization.NumberStyles.AllowHexSpecifier, null));
                        Color endColor = NGUIMath.IntToColor(int.Parse(colors[1] + "FF", System.Globalization.NumberStyles.AllowHexSpecifier, null));

                        string faded = string.Empty;
                        for (int i = 0; i < input.Length; i++)
                        {
                            Color color = Color.Lerp(startColor, endColor, (float)i / (float)input.Length);
                            faded += $"<color=#{color.ToHex()}>{input[i]}</color>";
                        }
                        input = faded;
                    }
                }
            }

            // Bold chat
            if (Properties.BoldText.Value)
            {
                input = input.AsBold();
            }
            // Italic chat
            if (Properties.ItalicText.Value)
            {
                input = input.AsItalic();
            }

            // Custom name
            string customName = Properties.ChatName.Value;
            if (customName.Length != 0)
            {
                name = customName.Colored();
            }
            // Bold name
            if (Properties.BoldName.Value)
            {
                name = name.AsBold();
            }
            // Italic name
            if (Properties.ItalicName.Value)
            {
                name = name.AsItalic();
            }

            return new object[] { $"{Properties.TextPrefix.Value}{input}{Properties.TextSuffix.Value}", name };
        }

        void OnLevelWasLoaded(int level)
        {
            RenderSettings.haloStrength = 100;
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
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
                    Details = $"Playing in singleplayer.",
                    State = $"{FengGameManagerMKII.Level.Name} / {difficulty}"
                });
            }

            if (FirstJoin)
            {
                FirstJoin = false;
                string joinMessage = Properties.JoinMessage.Value.Colored();
                if (joinMessage.Uncolored().Length <= 0)
                {
                    joinMessage = Properties.JoinMessage.Value;
                }
                if (joinMessage.Length > 0)
                {
                    string name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]).Colored();
                    if (name.Uncolored().Length <= 0)
                    {
                        name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]);
                    }
                    FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, HandleChat(joinMessage, name));
                }
            }
        }

        void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            Logger.Info($"[{player.Id}] ".WithColor("ffcc00") + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).Colored() + " connected.".WithColor("00ff00"));
        }

        void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            Logger.Info($"[{player.Id}] ".WithColor("ffcc00") + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).Colored() + " disconnected.".WithColor("ff0000"));
        }

        void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            NetworkPatches.OnPlayerPropertyModification(playerAndUpdatedProps);

            PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
            ExitGames.Client.Photon.Hashtable properties = playerAndUpdatedProps[1] as ExitGames.Client.Photon.Hashtable;

            // Neko Mod detection
            if (properties.ContainsValue("N_user") || properties.ContainsValue("N_owner"))
            {
                player.isNeko = true;
                player.isNekoUser = properties.ContainsValue("N_user");
                player.isNekoOwner = properties.ContainsValue("N_owner");
            }

            if (properties.ContainsKey("FoxMod"))
            {
                player.isFoxMod = true;
            }
        }

        void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            NetworkPatches.OnRoomPropertyModification(propertiesThatChanged);

            PhotonPlayer sender = null;
            if (propertiesThatChanged.ContainsKey("sender") && propertiesThatChanged["sender"] is PhotonPlayer)
            {
                sender = (PhotonPlayer)propertiesThatChanged["sender"];
            }

            if (sender == null || sender.isMasterClient)
            {
                if (propertiesThatChanged.ContainsKey("Map") && propertiesThatChanged["Map"] is string && IsMultiMap)
                {
                    LevelInfo levelInfo = LevelInfo.GetInfo((string)propertiesThatChanged["Map"]);
                    if (levelInfo != null)
                    {
                        FengGameManagerMKII.Level = levelInfo;
                    }
                }

                if (propertiesThatChanged.ContainsKey("Lighting") && propertiesThatChanged["Lighting"] is string)
                {
                    if (GExtensions.TryParseEnum((string)propertiesThatChanged["Lighting"], out DayLight time))
                    {
                        Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setDayLight(time);
                    }
                }
            }
        }

        void OnJoinedLobby()
        {
            PhotonNetwork.playerName = LoginFengKAI.Player.Name;

            DiscordHelper.SetPresence(new Discord.Activity
            {
                Details = "Searching for a room...",
                State = $"Region: {NetworkHelper.GetRegionCode()}"
            });
        }

        void OnJoinedRoom()
        {
            IsMultiMap = PhotonNetwork.room.name.Split('`')[1].StartsWith("Multi-Map");
            Muted = new List<int>();
            FirstJoin = true;

            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { "GuardianMod", 1 },
            });

            string[] roomInfo = PhotonNetwork.room.name.Split('`');
            if (roomInfo.Length > 6)
            {

                DiscordHelper.SetPresence(new Discord.Activity
                {
                    Details = $"Playing in {(roomInfo[5].Length == 0 ? string.Empty : "[PWD]")} {roomInfo[0].Uncolored()}",
                    State = $"({NetworkHelper.GetRegionCode()}) {roomInfo[1]} / {roomInfo[2].ToUpper()}"
                });
            }
        }

        void OnLeftRoom()
        {
            MapData = string.Empty;

            DiscordHelper.SetPresence(new Discord.Activity
            {
                Details = "Idle..."
            });
        }

        void OnConnectionFail(DisconnectCause cause)
        {
            Logger.Warn($"OnConnectionFail ({cause})");
        }

        void OnApplicationQuit()
        {
            Properties.Save();

            DiscordHelper.DiscordInstance.Dispose();
        }

        void Update()
        {
            DiscordHelper.DiscordInstance.RunCallbacks();
        }
    }
}