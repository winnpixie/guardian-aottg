using Guardian;
using System.Collections.Generic;
using UnityEngine;

public class InRoomChat : Photon.MonoBehaviour
{
    public static InRoomChat Instance => FengGameManagerMKII.Instance.chatRoom;
    public static Rect MessagesRect = new Rect(1f, 0f, 329f, 225f);
    public static Rect ChatBoxRect = new Rect(30f, 575f, 300f, 25f);
    public bool IsVisible = true;
    private bool AlignBottom = true;
    public static List<Message> Messages = new List<Message>();
    public string inputLine = string.Empty;
    private static readonly Vector2 bottom = new Vector2(0, float.MaxValue);
    private Vector2 ScrollPosition = bottom;

    public void Start()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (AlignBottom)
        {
            ScrollPosition = bottom;
            MessagesRect = new Rect(1f, Screen.height - 255f, 329f, 225f);
            ChatBoxRect = new Rect(30f, Screen.height - 25f, 300f, 25f);
        }
    }

    public void AddLine(string message)
    {
        AddMessage("", message);
    }

    public void AddMessage(string sender, string text)
    {
        sender = Mod.BlacklistedTags.Replace(sender, "");
        text = Mod.BlacklistedTags.Replace(text, "");

        if (sender.Length != 0 || text.Length != 0)
        {
            if (Messages.Count > 49)
            {
                Messages.RemoveAt(0);
            }
            Messages.Add(new Message(sender, text));
            ScrollPosition = bottom;
        }
    }

    public void OnGUI()
    {
        if (!IsVisible)
        {
            return;
        }
        if (Event.current.type == EventType.KeyDown)
        {
            if ((Event.current.keyCode != KeyCode.Tab && Event.current.character != '\t') || IN_GAME_MAIN_CAMERA.IsPausing || FengGameManagerMKII.InputRC.humanKeys[InputCodeRC.chat] == KeyCode.Tab)
            {
                goto IL_0127;
            }
            Event.current.Use();
        }
        else
        {
            if (Event.current.type != EventType.KeyUp || Event.current.keyCode == KeyCode.None || Event.current.keyCode != FengGameManagerMKII.InputRC.humanKeys[InputCodeRC.chat] || !(GUI.GetNameOfFocusedControl() != "ChatInput"))
            {
                goto IL_0127;
            }
            inputLine = string.Empty;
            GUI.FocusControl("ChatInput");
        }
        goto IL_219c;
    IL_0127:
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(inputLine))
            {
                GUI.FocusControl(string.Empty);
                if (inputLine == "\t")
                {
                    inputLine = string.Empty;
                    return;
                }

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
                    if (name == string.Empty)
                    {
                        name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]);
                    }
                    FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, Guardian.Mod.HandleChat(inputLine, name));
                }
                else
                {
                    Guardian.Mod.Commands.HandleCommand(this);
                }

                inputLine = string.Empty;
                return;
            }
            inputLine = "\t";
            GUI.FocusControl("ChatInput");
        }
        goto IL_219c;

    IL_219c:
        GUI.SetNextControlName(string.Empty);
        GUILayout.BeginArea(MessagesRect, GUI.skin.box);
        GUILayout.FlexibleSpace();
        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.label)
        {
            margin = new RectOffset(0, 0, 0, 0),
            padding = new RectOffset(0, 0, 0, 0),
            border = new RectOffset(0, 0, 0, 0)
        };

        foreach (Message message in Messages)
        {
            try
            {
                GUILayout.Label(message.ToString(), buttonStyle);
            }
            catch { }
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
        GUILayout.BeginArea(ChatBoxRect);
        GUILayout.BeginHorizontal();
        GUI.SetNextControlName("ChatInput");
        inputLine = GUILayout.TextField(inputLine, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public class Message
    {
        public string sender;
        public string text;

        public Message(string sender, string text)
        {
            this.sender = sender;
            this.text = text;
        }

        public override string ToString()
        {
            if (sender.Length == 0)
            {
                return text;
            }
            return sender + ": " + text;
        }
    }
}
