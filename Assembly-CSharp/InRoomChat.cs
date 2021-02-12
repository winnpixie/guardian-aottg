using Guardian;
using Guardian.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class InRoomChat : Photon.MonoBehaviour
{
    public static InRoomChat Instance;
    public static Rect MessagesRect = new Rect(1f, 0f, 329f, 225f);
    public static Rect ChatBoxRect = new Rect(30f, 575f, 300f, 25f);
    public bool IsVisible = true;
    private bool AlignBottom = true;
    public static List<Message> Messages = new List<Message>();
    public string inputLine = string.Empty;
    private Vector2 ScrollPosition = GameHelper.ScrollBottom;
    private string TextFieldName = "ChatInput";
    private GUIStyle boxStyle;
    private GUIStyle labelStyle;
    private GUIStyle textboxStyle;

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (AlignBottom)
        {
            ScrollPosition = GameHelper.ScrollBottom;
            MessagesRect = new Rect(1f, Screen.height - 255f, 329f, 225f);
            ChatBoxRect = new Rect(30f, Screen.height - 25f, 300f, 25f);
        }
    }

    public void AddLine(string message)
    {
        AddMessage(string.Empty, message);
    }

    public void AddMessage(string sender, string text)
    {
        sender = Mod.BlacklistedTags.Replace(sender, string.Empty);
        text = Mod.BlacklistedTags.Replace(text, string.Empty);

        if (sender.Length != 0 || text.Length != 0)
        {
            if (Messages.Count > 49)
            {
                Messages.RemoveAt(0);
            }
            Messages.Add(new Message(sender, text));
            ScrollPosition = GameHelper.ScrollBottom;
        }
    }

    public void OnGUI()
    {
        if (!IsVisible || !PhotonNetwork.connected)
        {
            return;
        }

        // Chat messages
        if (boxStyle == null)
        {
            boxStyle = new GUIStyle(GUI.skin.box);
            Texture2D flat = new Texture2D(1, 1);
            flat.SetPixel(0, 0, new Color(0.125f, 0.125f, 0.125f, 0.6f));
            flat.Apply();
            boxStyle.normal.background = flat;
        }

        GUI.SetNextControlName(string.Empty);
        if (Mod.Properties.ChatBackground.Value)
        {
            GUILayout.BeginArea(MessagesRect, boxStyle);
        }
        else
        {
            GUILayout.BeginArea(MessagesRect);
        }
        GUILayout.FlexibleSpace();

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);

        if (labelStyle == null)
        {
            labelStyle = new GUIStyle(GUI.skin.label)
            {
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                border = new RectOffset(0, 0, 0, 0)
            };
        }

        foreach (Message message in Messages)
        {
            try
            {
                GUILayout.Label(message.ToString(), labelStyle);
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)
                    && Event.current.type != EventType.Repaint
                    && GUI.GetNameOfFocusedControl().Equals(TextFieldName))
                {
                    if (Input.GetMouseButtonDown(0)) // Mouse1/Left Click
                    {
                        string text = message.Content;

                        text = GameHelper.Detagger.Replace(text, string.Empty);
                        text = GameHelper.SpecialChars.Replace(text, string.Empty);

                        Mod.Commands.Find("translate").Execute(this, new string[] {
                            "auto",
                            System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName,
                            text
                        });
                    }
                    else if (Input.GetMouseButtonDown(1)) // Mouse2/Right Click
                    {
                        TextEditor te = new TextEditor();
                        te.content = new GUIContent(message.Content);
                        te.SelectAll();
                        te.Copy();
                    }
                }
            }
            catch { }
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();

        // Sends chat messages
        KeyCode rcChatKey = FengGameManagerMKII.InputRC.humanKeys[InputCodeRC.Chat];
        if (Event.current.type == EventType.KeyUp && Event.current.keyCode == rcChatKey && rcChatKey != KeyCode.None && !GUI.GetNameOfFocusedControl().Equals(TextFieldName))
        {
            GUI.FocusControl(TextFieldName);
            inputLine = "\t";
        }
        else if (Event.current.type == EventType.KeyDown)
        {
            if ((Event.current.keyCode == KeyCode.Tab || Event.current.character == '\t') && rcChatKey != KeyCode.Tab && !IN_GAME_MAIN_CAMERA.IsPausing)
            {
                Event.current.Use();
            }
            else if (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return)
            {
                if (GUI.GetNameOfFocusedControl().Equals(TextFieldName))
                {
                    if (!string.IsNullOrEmpty(inputLine) && inputLine != "\t")
                    {
                        if (FengGameManagerMKII.RCEvents.ContainsKey("OnChatInput"))
                        {
                            string key = (string)FengGameManagerMKII.RCVariableNames["OnChatInput"];
                            if (FengGameManagerMKII.StringVariables.ContainsKey(key))
                            {
                                FengGameManagerMKII.StringVariables[key] = inputLine;
                            }
                            else
                            {
                                FengGameManagerMKII.StringVariables.Add(key, inputLine);
                            }
                            RCEvent rcEvent = (RCEvent)FengGameManagerMKII.RCEvents["OnChatInput"];
                            rcEvent.CheckEvent();
                        }

                        if (!inputLine.StartsWith("/"))
                        {
                            string name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]).Colored();
                            if (name.Uncolored().Length <= 0)
                            {
                                name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]);
                            }
                            FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, FormatMessage(inputLine, name));
                        }
                        else
                        {
                            Guardian.Mod.Commands.HandleCommand(this);
                        }
                    }

                    GUI.FocusControl(string.Empty);
                    inputLine = string.Empty;
                }
                else
                {
                    GUI.FocusControl(TextFieldName);
                    inputLine = "\t";
                }
            }
        }

        // Chat text-field
        if (textboxStyle == null)
        {
            textboxStyle = new GUIStyle(GUI.skin.textField);
            Texture2D flat = new Texture2D(1, 1);
            flat.SetPixel(0, 0, new Color(0.125f, 0.125f, 0.125f, 0.2f));
            flat.Apply();
            textboxStyle.normal.background = flat;

            Texture2D flatFocused = new Texture2D(1, 1);
            flatFocused.SetPixel(0, 0, new Color(0.125f, 0.125f, 0.125f, 0.6f));
            flatFocused.Apply();
            textboxStyle.focused.background = flatFocused;
        }

        GUILayout.BeginArea(ChatBoxRect);
        GUILayout.BeginHorizontal();
        GUI.SetNextControlName(TextFieldName);
        inputLine = GUILayout.TextField(inputLine, textboxStyle, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public static object[] FormatMessage(string input, string name)
    {
        // Emotes
        input = input.Replace("<3", "\u2665");
        input = input.Replace(":lenny:", "( ͡° ͜ʖ ͡°)");

        // Color and fading
        string chatColor = Mod.Properties.TextColor.Value;
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
        if (Mod.Properties.BoldText.Value)
        {
            input = input.AsBold();
        }
        // Italic chat
        if (Mod.Properties.ItalicText.Value)
        {
            input = input.AsItalic();
        }

        // Custom name
        string customName = Mod.Properties.ChatName.Value;
        if (customName.Length != 0)
        {
            name = customName.Colored();
        }
        // Bold name
        if (Mod.Properties.BoldName.Value)
        {
            name = name.AsBold();
        }
        // Italic name
        if (Mod.Properties.ItalicName.Value)
        {
            name = name.AsItalic();
        }

        return new object[] { $"{Mod.Properties.TextPrefix.Value}{input}{Mod.Properties.TextSuffix.Value}", name };
    }

    public class Message
    {
        public string Sender;
        public string Content;

        public Message(string sender, string content)
        {
            this.Sender = sender;
            this.Content = content;
        }

        public override string ToString()
        {
            if (Sender.Length == 0)
            {
                return Content;
            }
            return Sender + ": " + Content;
        }
    }
}
