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
        public static string Build = "08262020";
        public static string RootDir = Application.dataPath + "\\..";
        public static string HostWhitelistPath = RootDir + "\\Hosts.txt";
        public static string MapData = "";
        public static CommandManager Commands = new CommandManager();
        public static PropertyManager Properties = new PropertyManager();
        public static List<string> HostWhitelist = new List<string>();
        public List<int> Muted = new List<int>();
        public static Regex BlacklistedTags = new Regex("<\\/?(size(=\\d*)?|quad([^>]*)?|material([^>]*)?)>", RegexOptions.IgnoreCase);
        public static Logger Logger = new Logger();
        private static bool Initialized = false;

        void Start()
        {
            Instance = this;

            if (!Initialized)
            {
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
                    FengGameManagerMKII.NameField = LoginFengKAI.Player.name;
                }
                LoginFengKAI.Player.guildname = PlayerPrefs.GetString("guildname", string.Empty);

                // Load various features
                Commands.Load();
                Properties.Load();

                // Property whitelist
                NetworkPatches.PropertyWhitelist.Add("sender");
                NetworkPatches.PropertyWhitelist.Add("GuardianMod");
                NetworkPatches.PropertyWhitelist.Add("Stats");
                foreach (FieldInfo field in typeof(PhotonPlayerProperty).GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    NetworkPatches.PropertyWhitelist.Add((string)field.GetValue(null));
                }

                Initialized = true;

                Discord.Discord.StartTimestamp = GameHelper.CurrentTimeMillis();
            }

            base.gameObject.AddComponent<ModUI>();
            base.gameObject.AddComponent<MicEF>();

            Discord.Discord.SetPresence(new DiscordRpc.RichPresence
            {
                details = "Staring at the main menu..."
            });
        }

        public static IEnumerator CheckForUpdate()
        {
            using (WWW www = new WWW("https://raw.githubusercontent.com/alerithe/guardian/master/BUILD.TXT?t=" + GameHelper.CurrentTimeMillis()))
            {
                yield return www;

                if (!www.text.Equals(Build))
                {
                    Logger.Error("You are running an outdated build, please update!");
                    UIMainReferences.Version = "outdated";
                    GameObject.Find("VERSION").GetComponent<UILabel>().text = "[ff0000]Mod is outdated![-] Please download the latest build from [0099ff]https://tiny.cc/GuardianMod[-]!";
                }
            }
        }

        public static void LoadSkinHostWhitelist()
        {
            HostWhitelist = new List<string>(File.ReadAllLines(HostWhitelistPath));
        }

        public static object[] HandleChat(string input, string name)
        {
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
                        input = GameHelper.Detagger.Replace(input, "");

                        Color startColor = NGUIMath.IntToColor(int.Parse(colors[0] + "FF", System.Globalization.NumberStyles.AllowHexSpecifier, null));
                        Color endColor = NGUIMath.IntToColor(int.Parse(colors[1] + "FF", System.Globalization.NumberStyles.AllowHexSpecifier, null));

                        string faded = "";
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

            // Emotes
            input = input.Replace("<3", "<color=#ff0000>\u2665</color>");
            input = input.Replace(":lenny:", "( ͡° ͜ʖ ͡°)");

            return new object[] { $"{Properties.TextPrefix.Value}{input}{Properties.TextSuffix.Value}", name };
        }

        public void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            Logger.Info($"[{player.id}] ".WithColor("ffcc00") + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).Colored() + " connected.".WithColor("00ff00"));
        }

        public void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            Logger.Info($"[{player.id}] ".WithColor("ffcc00") + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).Colored() + " disconnected.".WithColor("ff0000"));
        }

        public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            NetworkPatches.OnPlayerPropertyModification(playerAndUpdatedProps);
        }

        public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            NetworkPatches.OnRoomPropertyModification(propertiesThatChanged);
        }

        public void OnJoinedLobby()
        {
            Logger.Info("OnJoinedLobby");
            if (PhotonNetwork.room != null)
            {
                PhotonNetwork.networkingPeer.State = PeerState.Joined;
                return;
            }

            Discord.Discord.SetPresence(new DiscordRpc.RichPresence
            {
                details = "Searching for a room...",
                state = $"Region: {NetworkHelper.GetRegionCode()}"
            });
        }

        public void OnLeftLobby()
        {
            Logger.Info("OnLeftLobby");
        }

        public void OnJoinedRoom()
        {
            Logger.Info("OnJoinedRoom");

            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { "GuardianMod", 1 },
                { "Stats", ModifiedStats.ToInt() }
            });

            string[] roomInfo = PhotonNetwork.room.name.Split('`');
            if (roomInfo.Length > 6)
            {
                Discord.Discord.SetPresence(new DiscordRpc.RichPresence
                {
                    details = $"Playing in {(roomInfo[5].Length == 0 ? "" : "[PWD]")} {roomInfo[0].Uncolored()}",
                    state = $"({NetworkHelper.GetRegionCode()}) {roomInfo[1]} / {roomInfo[2]}"
                });
            }
        }

        public void OnLeftRoom()
        {
            Logger.Info("OnLeftRoom");

            MapData = "";
            Discord.Discord.SetPresence(new DiscordRpc.RichPresence
            {
                details = "Idle..."
            });
        }

        public void OnConnectionFail(DisconnectCause cause)
        {
            Logger.Warn($"OnConnectionFail ({cause})");
        }

        void OnApplicationQuit()
        {
            Properties.Save();
            DiscordRpc.Shutdown();
        }

        void Update()
        {
            DiscordRpc.RunCallbacks();
        }
    }
}