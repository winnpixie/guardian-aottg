using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class InRoomChat : Photon.MonoBehaviour
{
    public static InRoomChat Instance;
    public static Rect MessagesRect = new Rect(1f, 0f, 329f, 225f);
    public static Rect ChatBoxRect = new Rect(30f, 575f, 300f, 25f);
    public static List<Message> Messages = new List<Message>();
    public static List<PhotonPlayer> Ignored = new List<PhotonPlayer>();
    private static readonly Regex Detagger = new Regex("<\\/?(color|size|b|i|material|quad)[^>]*>", RegexOptions.IgnoreCase);

    public bool IsVisible = true;
    private bool AlignBottom = true;
    public string inputLine = string.Empty;
    private Vector2 ScrollPosition = Guardian.Utilities.GameHelper.ScrollBottom;
    private string TextFieldName = "ChatInput";
    private GUIStyle labelStyle;

    void Awake()
    {
        Instance = this;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (AlignBottom)
        {
            ScrollPosition = Guardian.Utilities.GameHelper.ScrollBottom;
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
        sender = Guardian.Mod.BlacklistedTags.Replace(sender, string.Empty);
        text = Guardian.Mod.BlacklistedTags.Replace(text, string.Empty);

        if (sender.Length != 0 || text.Length != 0)
        {
            Messages.Add(new Message(sender, text));

            if (Messages.Count > Guardian.Mod.Properties.MaxChatLines.Value)
            {
                Messages.RemoveAt(0);
            }

            ScrollPosition = Guardian.Utilities.GameHelper.ScrollBottom;
        }
    }

    private void DrawMessageHistory()
    {

        if (labelStyle == null)
        {
            labelStyle = new GUIStyle(GUI.skin.label)
            {
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
            };
        }

        if (Guardian.Mod.Properties.ChatBackground.Value)
        {
            GUILayout.BeginArea(MessagesRect, Guardian.UI.GSkins.Box);
        }
        else
        {
            GUILayout.BeginArea(MessagesRect);
        }
        GUILayout.FlexibleSpace();
        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);

        foreach (Message message in Messages)
        {
            try
            {
                string messageText = message.ToString();

                if (Guardian.Mod.Properties.ChatTimestamps.Value)
                {
                    DateTime date = Guardian.Utilities.GameHelper.Epoch.AddMilliseconds(message.Timestamp).ToLocalTime();
                    messageText = "[" + date.ToString("HH:mm:ss") + "] " + messageText;
                }

                GUILayout.Label(messageText, labelStyle);
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)
                    && Event.current.type != EventType.Repaint
                    && GUI.GetNameOfFocusedControl().Equals(TextFieldName))
                {
                    if (Input.GetMouseButtonDown(0)) // Left-click
                    {
                        string text = Detagger.Replace(message.Content, string.Empty);

                        Guardian.Mod.Commands.Find("translate").Execute(this, new string[] {
                            "auto",
                            Guardian.Mod.SystemLanguage,
                            text
                        });
                    }
                    else if (Input.GetMouseButtonDown(1)) // Right-click
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
    }

    private void HandleInput()
    {
        KeyCode rcChatKey = FengGameManagerMKII.InputRC.humanKeys[InputCodeRC.Chat];
        if (rcChatKey != KeyCode.None && rcChatKey.WasKeyDownInGUI() && !GUI.GetNameOfFocusedControl().Equals(TextFieldName))
        {
            GUI.FocusControl(TextFieldName);
            inputLine = "\t";
        }
        else if (KeyCode.Slash.IsKeyDownInGUI() && !GUI.GetNameOfFocusedControl().Equals(TextFieldName))
        {
            GUI.FocusControl(TextFieldName);
            inputLine = "/";
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
                            string name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]).ColorParsed();
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
    }

    private void DrawMessageTextField()
    {
        GUILayout.BeginArea(ChatBoxRect);
        GUILayout.BeginHorizontal();
        GUI.SetNextControlName(TextFieldName);
        inputLine = GUILayout.TextField(inputLine, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public void OnGUI()
    {
        if (!IsVisible || !PhotonNetwork.connected)
        {
            return;
        }

        DrawMessageHistory();

        HandleInput();

        DrawMessageTextField();
    }

    public static object[] FormatMessage(string input, string name)
    {
        // Auto-translate
        if (Guardian.Mod.Properties.TranslateOutgoing.Value)
        {
            string[] result = Guardian.Utilities.Translator.Translate(input, Guardian.Mod.Properties.IncomingLanguage.Value, Guardian.Mod.Properties.OutgoingLanguage.Value);

            if (result.Length > 1 && !result[0].Equals(Guardian.Mod.Properties.OutgoingLanguage.Value))
            {
                input = result[1];
            }
        }

        // Emotes
        input = input.Replace("<3", "\u2665");
        input = input.Replace(":lenny:", "( ͡° ͜ʖ ͡°)");

        // Color and fading
        string chatColor = Guardian.Mod.Properties.TextColor.Value;
        if (chatColor.Length > 0)
        {
            input = input.AsColor(chatColor);
        }

        // Bold chat
        if (Guardian.Mod.Properties.BoldText.Value)
        {
            input = input.AsBold();
        }
        // Italic chat
        if (Guardian.Mod.Properties.ItalicText.Value)
        {
            input = input.AsItalic();
        }

        // Custom name
        string customName = Guardian.Mod.Properties.ChatName.Value;
        if (customName.Length != 0)
        {
            name = customName.ColorParsed();
        }
        // Bold name
        if (Guardian.Mod.Properties.BoldName.Value)
        {
            name = name.AsBold();
        }
        // Italic name
        if (Guardian.Mod.Properties.ItalicName.Value)
        {
            name = name.AsItalic();
        }

        return new object[] { $"{Guardian.Mod.Properties.TextPrefix.Value}{input}{Guardian.Mod.Properties.TextSuffix.Value}", name };
    }

    public class Message
    {
        public string Sender;
        public string Content;
        public long Timestamp;

        public Message(string sender, string content)
        {
            this.Sender = sender;
            this.Content = content;

            this.Timestamp = Guardian.Utilities.GameHelper.CurrentTimeMillis();
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
