// Created By: Elite Future, Discord: Elite Future#1043 for questions, suggestions, or optimizations

using System;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon.Lite;

public class MicGUI : MonoBehaviour
{
    private Rect micRect;
    private Rect overlayRect;
    private Vector2 vSliderValue;
    private Vector2 controlSlider;
    private float appHeight;
    private int selection;
    private bool guiOn;
    private KeyCode guiKey = KeyCode.Backslash;
    private Rect micAreaRect;
    private float labelLength;
    private Rect micOptionsRect;
    private int changingKeys;
    private GUIStyle overlayStyle;
    private GUIStyle micStyle;
    private GUIStyle areaStyle;
    private GUIStyle buttonStyle;
    private Color buttonGUIColor = new Color(0f, 0.2314f, 0.4588f);
    private bool dropDown;
    private Vector2 clickPos;
    private Rect deviceRect;

    public void Start()
    {
        dropDown = false;
        if (PlayerPrefs.HasKey("voiceKey"))
        {
            guiKey = (KeyCode)PlayerPrefs.GetInt("voiceKey");
        }

        changingKeys = -1;
        selection = 0;
        guiOn = false;
        appHeight = Screen.height;
        AdjustRect();
        overlayStyle = new GUIStyle();

        var defaultBackground = new Texture2D(1, 1);
        defaultBackground.SetPixel(0, 0, new Color(0.1569f, 0.1569f, 0.1569f));
        defaultBackground.Apply();
        micStyle = new GUIStyle();
        micStyle.normal.background = defaultBackground; // mic GUI color

        var areaBack = new Texture2D(1, 1);
        areaBack.SetPixel(0, 0, new Color(0.1961f, 0.1961f, 0.1961f));
        areaBack.Apply();
        areaStyle = new GUIStyle();
        areaStyle.normal.background = areaBack; // inner area GUI color

        var buttonBack = new Texture2D(1, 1);
        buttonBack.SetPixel(0, 0, buttonGUIColor);
        buttonBack.Apply();
        buttonStyle = new GUIStyle();
        buttonStyle.normal.background = buttonBack; // Normal button color
        var buttonAct = new Texture2D(1, 1);
        buttonAct.SetPixel(0, 0, adjustColor(buttonGUIColor, 0.75f)); // 25% darker
        buttonAct.Apply();
        buttonStyle.active.background = buttonAct; // Press button Color
        buttonStyle.active.textColor = new Color(0.149f, 0.149f, 0.149f); // active text color
        var buttonHov = new Texture2D(1, 1);
        buttonHov.SetPixel(0, 0, adjustColor(buttonGUIColor, 1.25f)); // 25% brighter
        buttonHov.Apply();
        buttonStyle.hover.background = buttonHov; // Hover button color
        buttonStyle.hover.textColor = new Color(0.149f, 0.149f, 0.149f); // hover text color
        buttonStyle.normal.textColor = Color.white; // normal text Color
        buttonStyle.alignment = TextAnchor.MiddleCenter; // Aligns text to center
        buttonStyle.wordWrap = true;
    }

    public static Color adjustColor(Color col, float adjustment)
    {
        var red = col.r * adjustment;
        var green = col.g * adjustment;
        var blue = col.b * adjustment;

        return new Color(red, green, blue);
    }

    // Transparent overlay GUI to show who is talking
    public void DrawOverlayGUI(int ID)
    {
        try
        {
            GUILayout.BeginVertical();
            if (MicEF.ThreadId != -1) // This sees if your mic is on
            {
                GUILayout.Label("<b>(" + PhotonNetwork.player.Id + ") </b>" + PhotonNetwork.player.customProperties["name"].ToString().Colored());
            }
            foreach (var entry in MicEF.Users)
            {
                var player = entry.Value;
                if (player.clipProcess)
                {
                    GUILayout.Label("<b>(" + entry.Key + ") </b>" + entry.Value.name);
                }
            }
            GUILayout.EndVertical();
        }
        catch (Exception e)
        {
            print(e);
        }
    }

    public void DrawMainGUI(int ID)
    {
        GUILayout.BeginVertical();

        GUILayout.BeginArea(micOptionsRect);
        GUILayout.BeginHorizontal();

        // Button Options
        if (GUILayout.Button("User List", buttonStyle))
        {
            selection = 0;
            dropDown = false;
        }
        else if (GUILayout.Button("Options", buttonStyle))
        {
            selection = 1;
            dropDown = false;
        }
        else if (GUILayout.Button("Credits", buttonStyle))
        {
            selection = 2;
            dropDown = false;
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.BeginArea(micAreaRect, areaStyle);

        switch (selection)
        {
            case 0:
                { // User list
                    vSliderValue = GUILayout.BeginScrollView(vSliderValue);
                    foreach (var entry in MicEF.Users)
                    {
                        var player = entry.Value;

                        GUILayout.BeginHorizontal();

                        GUILayout.Label(player.name, GUILayout.Width(labelLength));
                        var oldCol = buttonStyle.normal.textColor;

                        if (player.mutedYou)
                        {
                            buttonStyle.normal.textColor = Color.yellow;
                        }
                        else if (!player.isMuted)
                        {
                            buttonStyle.normal.textColor = Color.green;
                        }
                        else
                        {
                            buttonStyle.normal.textColor = Color.red;
                        }

                        if (GUILayout.Button("M", buttonStyle)) // Speaker Icon
                        {
                            player.Mute(!player.isMuted);
                        }

                        buttonStyle.normal.textColor = oldCol;

                        if (GUILayout.Button("V", buttonStyle)) // Volume
                        {
                            player.changingVolume = !player.changingVolume;
                        }

                        GUILayout.EndHorizontal();
                        if (player.changingVolume)
                        {
                            player.volume = GUILayout.HorizontalSlider(player.volume, 0f, 4f, new GUILayoutOption[0]);
                            if (!player.isMuted && player.volume == 0f)
                            {
                                player.Mute(true);
                            }
                            else if (player.isMuted)
                            {
                                player.Mute(false);
                            }
                        }
                    }
                    GUILayout.EndScrollView();
                    break;
                }
            case 1:
                { // Options
                    controlSlider = GUILayout.BeginScrollView(controlSlider);
                    GUILayout.BeginVertical();

                    // Voice Assignment
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Push To talk:");
                    var buttonText = MicEF.PushToTalk.ToString();
                    if (changingKeys == 0)
                    {
                        buttonText = "Waiting...";
                        for (int i = 1; i <= 429; i++)
                        {
                            KeyCode code = (KeyCode)(i);
                            if (Input.GetKeyDown(code))
                            {
                                MicEF.PushToTalk = code;
                                changingKeys = -1;
                                PlayerPrefs.SetInt("pushToTalk", (int)code);
                            }
                        }
                    }
                    if (GUILayout.Button(buttonText, buttonStyle))
                    {
                        if (changingKeys == -1)
                        {
                            changingKeys = 0;
                        }
                    }
                    GUILayout.EndHorizontal();


                    // GUI Assignment
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Voice GUI Key:");
                    buttonText = guiKey.ToString();
                    if (changingKeys == 1)
                    {
                        buttonText = "Waiting...";
                        for (var i = 1; i <= 429; i++)
                        {
                            KeyCode code = (KeyCode)i;
                            if (Input.GetKeyDown(code))
                            {
                                guiKey = code;
                                changingKeys = -1;
                                PlayerPrefs.SetInt("voiceKey", (int)code);
                            }
                        }
                    }
                    if (GUILayout.Button(buttonText, buttonStyle))
                    {
                        if (changingKeys == -1)
                        {
                            changingKeys = 1;
                        }
                    }
                    GUILayout.EndHorizontal();


                    // Volume
                    GUILayout.Label("Volume Multiplier: " + MicEF.VolumeMultiplier);
                    var oldVol = MicEF.VolumeMultiplier;
                    MicEF.VolumeMultiplier = GUILayout.HorizontalSlider(MicEF.VolumeMultiplier, 0f, 3f, new GUILayoutOption[0]);
                    if (oldVol != MicEF.VolumeMultiplier)
                    {
                        PlayerPrefs.SetFloat("volumeMultiplier", MicEF.VolumeMultiplier);
                    }


                    // Device Name
                    GUILayout.BeginHorizontal();

                    GUILayout.Label("Microphone: ");

                    var micButtonText = "Default";
                    if (MicEF.DeviceName.Length > 0)
                    {
                        micButtonText = MicEF.DeviceName;
                        if (micButtonText.StartsWith("Microphone ("))
                        {
                            micButtonText = micButtonText.Remove(0, 12);
                            micButtonText = micButtonText.Substring(0, micButtonText.Length - 1);
                        }
                    }

                    if (GUILayout.Button(micButtonText, buttonStyle))
                    {
                        clickPos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                        deviceRect = new Rect(clickPos.x - micAreaRect.width / 5f, clickPos.y + 5, micAreaRect.width / 2.5f, micAreaRect.height);
                        dropDown = !dropDown;
                    }

                    GUILayout.EndHorizontal();


                    //Auto Mute People On Join
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Auto Mute People On Join:");
                    var autoMute = MicEF.AutoMute;
                    MicEF.AutoMute = GUILayout.Toggle(autoMute, "On");
                    if (autoMute != MicEF.AutoMute)
                    {
                        PlayerPrefs.SetString("voiceAutoMute", MicEF.AutoMute + string.Empty);
                    }
                    GUILayout.EndHorizontal();


                    // Auto Connect
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Auto Connect:");
                    var autoConnect = MicEF.AutoConnect;
                    MicEF.AutoConnect = GUILayout.Toggle(autoConnect, "On");
                    if (autoConnect != MicEF.AutoConnect)
                    {
                        PlayerPrefs.SetString("voiceAutoConnect", MicEF.AutoConnect + string.Empty);
                    }
                    GUILayout.EndHorizontal();


                    // Toggle Mic
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Toggle Mic:");
                    var toggleMic = MicEF.ToggleMic;
                    MicEF.ToggleMic = GUILayout.Toggle(toggleMic, "On");
                    if (toggleMic != MicEF.ToggleMic)
                    {
                        PlayerPrefs.SetString("voiceToggleMic", MicEF.ToggleMic + string.Empty);
                    }
                    GUILayout.EndHorizontal();


                    // Disconnect Button
                    buttonText = "Disconnect From Voice";
                    if (MicEF.Disconnected)
                    {
                        buttonText = "Connect To Voice";
                    }

                    var oldCol = buttonStyle.normal.textColor;

                    if (MicEF.Disconnected)
                    {
                        buttonStyle.normal.textColor = Color.green;
                    }
                    else
                    {
                        buttonStyle.normal.textColor = Color.red;
                    }

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(buttonText, buttonStyle))
                    {
                        if (!MicEF.Disconnected)
                        {
                            PhotonNetwork.networkingPeer.OpRaiseEvent((byte)173, new byte[] { (byte)253 }, true, new RaiseEventOptions
                            {
                                Receivers = ReceiverGroup.Others
                            });
                            MicEF.Disconnected = true;
                        }
                        else
                        {
                            PhotonNetwork.networkingPeer.OpRaiseEvent((byte)173, new byte[0], true, new RaiseEventOptions
                            {
                                Receivers = ReceiverGroup.Others
                            });
                            MicEF.Disconnected = false;
                        }
                    }

                    // Reset settings button
                    buttonStyle.normal.textColor = Color.white;
                    if (GUILayout.Button("Reset Settings", buttonStyle))
                    {
                        MicEF.PushToTalk = KeyCode.V;
                        PlayerPrefs.SetInt("pushToTalk", (int)KeyCode.V);
                        guiKey = KeyCode.Backslash;
                        PlayerPrefs.SetInt("voiceKey", (int)KeyCode.Backslash);
                        MicEF.DeviceName = string.Empty;
                        PlayerPrefs.SetString("micDevice", string.Empty);
                        MicEF.VolumeMultiplier = 1f;
                        PlayerPrefs.SetFloat("volumeMultiplier", 1f);
                        MicEF.AutoMute = false;
                        PlayerPrefs.SetString("voiceAutoMute", "false");
                        MicEF.AutoConnect = true;
                        PlayerPrefs.SetString("voiceAutoConnect", "true");
                        MicEF.ToggleMic = false;
                        PlayerPrefs.SetString("voiceToggleMic", "false");
                        foreach (MicPlayer player in MicEF.Users.Values)
                        {
                            player.volume = 1f;
                            player.Mute(false);
                        }
                    }
                    GUILayout.EndHorizontal();

                    buttonStyle.normal.textColor = oldCol;

                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();
                    break;
                }
            case 3: // Credits to Kevin and Sadico
                GUILayout.Label("Main Developer: Elite Future(Kevin) - Discord:Elite Future#1043");
                GUILayout.Label("Data Compression: Sadico");
                break;
        }

        GUILayout.EndArea();
        GUILayout.EndVertical();
        if ((!Input.GetKey(KeyCode.Mouse0) || !Input.GetKey(KeyCode.Mouse1)) && !Input.GetKey(KeyCode.C) && (IN_GAME_MAIN_CAMERA.CameraMode == CAMERA_TYPE.WOW || IN_GAME_MAIN_CAMERA.CameraMode == CAMERA_TYPE.ORIGINAL))
        {
            GUI.DragWindow();
        }
    }

    // GUI to allow the user to change microphones
    public void DrawDeviceList(int ID)
    { // Maybe add a scroll view later
        GUILayout.BeginVertical();

        foreach (var str in Microphone.devices)
        {
            var butText = str.Remove(0, 12);
            butText = butText.Substring(0, butText.Length - 1);
            if (GUILayout.Button(butText, buttonStyle))
            {
                MicEF.DeviceName = str;
                dropDown = false;
                PlayerPrefs.SetString("micDevice", str);
            }
        }
        GUILayout.EndVertical();
    }

    // Just to turn on the GUI, didn't work properly in OnGUI
    public void Update()
    {
        if (Input.GetKeyDown(guiKey) && PhotonNetwork.room != null)
        {
            guiOn = !guiOn;
            dropDown = false;
        }
    }

    // Calls all GUIs
    public void OnGUI()
    {
        try
        {
            if (PhotonNetwork.room != null)
            {
                if (Screen.height != appHeight)
                {
                    appHeight = Screen.height;
                    AdjustRect();
                }
                if (MicEF.Users.Count > 0 || MicEF.ThreadId != -1)
                {
                    overlayRect = GUI.Window(1731, overlayRect, this.DrawOverlayGUI, string.Empty, overlayStyle);
                }
                if (guiOn)
                {
                    if (dropDown)
                    {
                        deviceRect = GUI.Window(1733, deviceRect, this.DrawDeviceList, string.Empty, overlayStyle);
                    }
                    micRect = GUI.Window(1732, micRect, this.DrawMainGUI, string.Empty, micStyle);
                }
            }
        }
        catch (Exception e)
        {
            print(e);
        }
    }

    // Fixes the GUI sizes
    private void AdjustRect()
    {
        var desiredWidth = 1920f; // Theoretically makes 4k screens still look okay(may need to change some other things though)
        var desiredHeight = 1080f;

        if (Screen.width > 1920)
        {
            desiredWidth = Screen.width;
        }
        if (Screen.height > 1080)
        {
            desiredHeight = Screen.height;
        }
        var width = desiredWidth / 4.2f;
        var height = desiredHeight / 4.2f;

        overlayRect = new Rect(0, Screen.height / 2 - 100, 200, 200);
        micRect = new Rect(Screen.width - width, Screen.height - height, width, height);
        micAreaRect = new Rect(10, height / 8, width - 20, height / 8 * 7 - 10);
        micOptionsRect = new Rect(10, 10, width - 20, height / 8);
        labelLength = micAreaRect.width / 8 * 6;
    }
}