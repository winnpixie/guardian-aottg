using UnityEngine;

public class custom_inputs : MonoBehaviour
{
    private float buttonHeight = 20f;
    public bool menuOn;
    public string[] DescriptionString;
    private KeyCode[] inputKey;
    private KeyCode[] inputKey2;
    public KeyCode[] default_inputKeys;
    public KeyCode[] alt_default_inputKeys;
    private bool altInputson;
    private float DescBox_X = -320f;
    private float InputBox_X = -100f;
    private float AltInputBox_X = 120f;
    public float Boxes_Y = 300f;
    public float BoxesMargin_Y = 30f;
    public int DescriptionSize = 200;
    public int buttonSize = 200;
    private float resetbuttonLocX = -100f;
    public float resetbuttonLocY = 600f;
    public string resetbuttonText = "Reset to defaults";
    public bool mouseAxisOn;
    public bool mouseButtonsOn = true;
    public bool allowDuplicates;
    private bool[] inputBool;
    private bool[] inputBool2;
    private string[] inputString;
    private string[] inputString2;
    [HideInInspector]
    public bool[] joystickActive;
    [HideInInspector]
    public bool[] joystickActive2;
    [HideInInspector]
    public string[] joystickString;
    [HideInInspector]
    public string[] joystickString2;
    private bool[] tempjoy1;
    private bool[] tempjoy2;
    [HideInInspector]
    public bool[] isInput;
    [HideInInspector]
    public bool[] isInputDown;
    [HideInInspector]
    public bool[] isInputUp;
    private bool tempbool;
    public GUISkin OurSkin;
    private float lastInterval;
    [HideInInspector]
    public float analogFeel_up;
    [HideInInspector]
    public float analogFeel_down;
    [HideInInspector]
    public float analogFeel_left;
    [HideInInspector]
    public float analogFeel_right;
    [HideInInspector]
    public float analogFeel_jump;
    public float analogFeel_gravity = 0.2f;
    public float analogFeel_sensitivity = 0.8f;
    private int tempLength;
    private float DescriptionBox_X;
    private float InputBox1_X;
    private float InputBox2_X;
    private float resetbuttonX;

    private void Start()
    {
        if (alt_default_inputKeys.Length == default_inputKeys.Length)
        {
            altInputson = true;
        }
        inputBool = new bool[DescriptionString.Length];
        inputString = new string[DescriptionString.Length];
        inputKey = new KeyCode[DescriptionString.Length];
        joystickActive = new bool[DescriptionString.Length];
        joystickString = new string[DescriptionString.Length];
        inputBool2 = new bool[DescriptionString.Length];
        inputString2 = new string[DescriptionString.Length];
        inputKey2 = new KeyCode[DescriptionString.Length];
        joystickActive2 = new bool[DescriptionString.Length];
        joystickString2 = new string[DescriptionString.Length];
        isInput = new bool[DescriptionString.Length];
        isInputDown = new bool[DescriptionString.Length];
        isInputUp = new bool[DescriptionString.Length];
        tempLength = PlayerPrefs.GetInt("KeyLength");
        tempjoy1 = new bool[DescriptionString.Length];
        tempjoy2 = new bool[DescriptionString.Length];
        if (!PlayerPrefs.HasKey("KeyCodes") || !PlayerPrefs.HasKey("KeyCodes2"))
        {
            ResetAll();
        }
        tempLength = PlayerPrefs.GetInt("KeyLength");
        if (PlayerPrefs.HasKey("KeyCodes") && tempLength == DescriptionString.Length)
        {
            LoadConfig();
        }
        else
        {
            PlayerPrefs.DeleteAll();
            ResetAll();
            LoadConfig();
            SaveInputs();
        }
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            isInput[i] = false;
            isInputDown[i] = false;
            isInputUp[i] = false;
            tempjoy1[i] = true;
            tempjoy2[i] = false;
        }
    }

    private void Update()
    {
        DescriptionBox_X = (float)(Screen.width / 2) + DescBox_X;
        InputBox1_X = (float)(Screen.width / 2) + InputBox_X;
        InputBox2_X = (float)(Screen.width / 2) + AltInputBox_X;
        resetbuttonX = (float)(Screen.width / 2) + resetbuttonLocX;
        if (!menuOn)
        {
            SetInputBools();
        }
        if (Input.GetKeyDown("escape"))
        {
            if (menuOn)
            {
                Time.timeScale = 1f;
                tempbool = false;
                menuOn = false;
                SaveInputs();
            }
            else
            {
                Time.timeScale = 0f;
                menuOn = true;
                Screen.showCursor = true;
                Screen.lockCursor = false;
            }
        }
    }

    private void OnGUI()
    {
        if (Time.realtimeSinceStartup > lastInterval + 3f)
        {
            tempbool = false;
        }
        if (menuOn)
        {
            DrawButtons1();
            if (altInputson)
            {
                DrawButtons2();
            }
        }
    }

    private void SetInputBools()
    {
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            if (Input.GetKey(inputKey[i]) || (joystickActive[i] && Input.GetAxis(joystickString[i]) > 0.95f) || Input.GetKey(inputKey2[i]) || (joystickActive2[i] && Input.GetAxis(joystickString2[i]) > 0.95f))
            {
                isInput[i] = true;
            }
            else
            {
                isInput[i] = false;
            }
            if (Input.GetKeyDown(inputKey[i]) || Input.GetKeyDown(inputKey2[i]))
            {
                isInputDown[i] = true;
            }
            else
            {
                isInputDown[i] = false;
            }
            if ((joystickActive[i] && Input.GetAxis(joystickString[i]) > 0.95f) || (joystickActive2[i] && Input.GetAxis(joystickString2[i]) > 0.95f))
            {
                if (!tempjoy1[i])
                {
                    isInputDown[i] = false;
                }
                if (tempjoy1[i])
                {
                    isInputDown[i] = true;
                    tempjoy1[i] = false;
                }
            }
            if (!tempjoy1[i] && joystickActive[i] && Input.GetAxis(joystickString[i]) < 0.1f && joystickActive2[i] && Input.GetAxis(joystickString2[i]) < 0.1f)
            {
                isInputDown[i] = false;
                tempjoy1[i] = true;
            }
            if (!tempjoy1[i] && !joystickActive[i] && joystickActive2[i] && Input.GetAxis(joystickString2[i]) < 0.1f)
            {
                isInputDown[i] = false;
                tempjoy1[i] = true;
            }
            if (!tempjoy1[i] && !joystickActive2[i] && joystickActive[i] && Input.GetAxis(joystickString[i]) < 0.1f)
            {
                isInputDown[i] = false;
                tempjoy1[i] = true;
            }
            if (Input.GetKeyUp(inputKey[i]) || Input.GetKeyUp(inputKey2[i]))
            {
                isInputUp[i] = true;
            }
            else
            {
                isInputUp[i] = false;
            }
            if ((joystickActive[i] && Input.GetAxis(joystickString[i]) > 0.95f) || (joystickActive2[i] && Input.GetAxis(joystickString2[i]) > 0.95f))
            {
                if (tempjoy2[i])
                {
                    isInputUp[i] = false;
                }
                if (!tempjoy2[i])
                {
                    isInputUp[i] = false;
                    tempjoy2[i] = true;
                }
            }
            if (tempjoy2[i] && joystickActive[i] && Input.GetAxis(joystickString[i]) < 0.1f && joystickActive2[i] && Input.GetAxis(joystickString2[i]) < 0.1f)
            {
                isInputUp[i] = true;
                tempjoy2[i] = false;
            }
            if (tempjoy2[i] && !joystickActive[i] && joystickActive2[i] && Input.GetAxis(joystickString2[i]) < 0.1f)
            {
                isInputUp[i] = true;
                tempjoy2[i] = false;
            }
            if (tempjoy2[i] && !joystickActive2[i] && joystickActive[i] && Input.GetAxis(joystickString[i]) < 0.1f)
            {
                isInputUp[i] = true;
                tempjoy2[i] = false;
            }
        }
    }

    private void SaveInputs()
    {
        string text = string.Empty;
        string text2 = string.Empty;
        string text3 = string.Empty;
        string text4 = string.Empty;
        string text5 = string.Empty;
        string text6 = string.Empty;
        for (int num = DescriptionString.Length - 1; num > -1; num--)
        {
            text = (int)inputKey[num] + "*" + text;
            text2 = joystickString[num] + "*" + text2;
            text3 = inputString[num] + "*" + text3;
            text4 = (int)inputKey2[num] + "*" + text4;
            text5 = joystickString2[num] + "*" + text5;
            text6 = inputString2[num] + "*" + text6;
        }
        PlayerPrefs.SetString("KeyCodes", text);
        PlayerPrefs.SetString("Joystick_input", text2);
        PlayerPrefs.SetString("Names_input", text3);
        PlayerPrefs.SetString("KeyCodes2", text4);
        PlayerPrefs.SetString("Joystick_input2", text5);
        PlayerPrefs.SetString("Names_input2", text6);
        PlayerPrefs.SetInt("KeyLength", DescriptionString.Length);
    }

    private void ResetAll()
    {
        if (default_inputKeys.Length != DescriptionString.Length)
        {
            default_inputKeys = new KeyCode[DescriptionString.Length];
        }
        if (alt_default_inputKeys.Length != default_inputKeys.Length)
        {
            alt_default_inputKeys = new KeyCode[default_inputKeys.Length];
        }
        string text = string.Empty;
        string text2 = string.Empty;
        string text3 = string.Empty;
        string text4 = string.Empty;
        string text5 = string.Empty;
        string text6 = string.Empty;
        for (int num = DescriptionString.Length - 1; num > -1; num--)
        {
            text = (int)default_inputKeys[num] + "*" + text;
            text2 += "#*";
            text3 = default_inputKeys[num].ToString() + "*" + text3;
            PlayerPrefs.SetString("KeyCodes", text);
            PlayerPrefs.SetString("Joystick_input", text2);
            PlayerPrefs.SetString("Names_input", text3);
            text4 = (int)alt_default_inputKeys[num] + "*" + text4;
            text5 += "#*";
            text6 = alt_default_inputKeys[num].ToString() + "*" + text6;
            PlayerPrefs.SetString("KeyCodes2", text4);
            PlayerPrefs.SetString("Joystick_input2", text5);
            PlayerPrefs.SetString("Names_input2", text6);
            PlayerPrefs.SetInt("KeyLength", DescriptionString.Length);
        }
    }

    private void LoadConfig()
    {
        string @string = PlayerPrefs.GetString("KeyCodes");
        string string2 = PlayerPrefs.GetString("Joystick_input");
        string string3 = PlayerPrefs.GetString("Names_input");
        string string4 = PlayerPrefs.GetString("KeyCodes2");
        string string5 = PlayerPrefs.GetString("Joystick_input2");
        string string6 = PlayerPrefs.GetString("Names_input2");
        string[] array = @string.Split('*');
        joystickString = string2.Split('*');
        inputString = string3.Split('*');
        string[] array2 = string4.Split('*');
        joystickString2 = string5.Split('*');
        inputString2 = string6.Split('*');
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            int.TryParse(array[i], out int result);
            inputKey[i] = (KeyCode)result;
            int.TryParse(array2[i], out int result2);
            inputKey2[i] = (KeyCode)result2;
            if (joystickString[i] == "#")
            {
                joystickActive[i] = false;
            }
            else
            {
                joystickActive[i] = true;
            }
            if (joystickString2[i] == "#")
            {
                joystickActive2[i] = false;
            }
            else
            {
                joystickActive2[i] = true;
            }
        }
    }

    private void DrawButtons1()
    {
        float num = Boxes_Y;
        Vector3 mousePosition = Input.mousePosition;
        float x = mousePosition.x;
        Vector3 mousePosition2 = Input.mousePosition;
        float y = mousePosition2.y;
        Vector3 point = GUI.matrix.inverse.MultiplyPoint3x4(new Vector3(x, (float)Screen.height - y, 1f));
        GUI.skin = OurSkin;
        GUI.Box(new Rect(0f, 0f, Screen.width, Screen.height), string.Empty);
        GUI.Box(new Rect(60f, 60f, Screen.width - 120, Screen.height - 120), string.Empty, "window");
        GUI.Label(new Rect(DescriptionBox_X, num - 10f, DescriptionSize, buttonHeight), "name", "textfield");
        GUI.Label(new Rect(InputBox1_X, num - 10f, DescriptionSize, buttonHeight), "input", "textfield");
        GUI.Label(new Rect(InputBox2_X, num - 10f, DescriptionSize, buttonHeight), "alt input", "textfield");
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            num += BoxesMargin_Y;
            GUI.Label(new Rect(DescriptionBox_X, num, DescriptionSize, buttonHeight), DescriptionString[i], "box");
            Rect position = new Rect(InputBox1_X, num, buttonSize, buttonHeight);
            GUI.Button(position, inputString[i]);
            if (!joystickActive[i] && inputKey[i] == KeyCode.None)
            {
                joystickString[i] = "#";
            }
            if (inputBool[i])
            {
                GUI.Toggle(position, value: true, string.Empty, OurSkin.button);
            }
            if (position.Contains(point) && Input.GetMouseButtonUp(0) && !tempbool)
            {
                tempbool = true;
                inputBool[i] = true;
                lastInterval = Time.realtimeSinceStartup;
            }
            if (GUI.Button(new Rect(resetbuttonX, resetbuttonLocY, buttonSize, buttonHeight), resetbuttonText) && Input.GetMouseButtonUp(0))
            {
                PlayerPrefs.DeleteAll();
                ResetAll();
                LoadConfig();
                SaveInputs();
            }
            if (Event.current.type == EventType.KeyDown && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = Event.current.keyCode;
                inputBool[i] = false;
                inputString[i] = inputKey[i].ToString();
                tempbool = false;
                joystickActive[i] = false;
                joystickString[i] = "#";
                SaveInputs();
                CheckDoubles(inputKey[i], i, 1);
            }
            if (mouseButtonsOn)
            {
                int num2 = 323;
                for (int j = 0; j < 6; j++)
                {
                    if (Input.GetMouseButton(j) && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
                    {
                        num2 += j;
                        inputKey[i] = (KeyCode)num2;
                        inputBool[i] = false;
                        inputString[i] = inputKey[i].ToString();
                        joystickActive[i] = false;
                        joystickString[i] = "#";
                        SaveInputs();
                        CheckDoubles(inputKey[i], i, 1);
                    }
                }
            }
            for (int k = 350; k < 409; k++)
            {
                if (Input.GetKey((KeyCode)k) && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey[i] = (KeyCode)k;
                    inputBool[i] = false;
                    inputString[i] = inputKey[i].ToString();
                    tempbool = false;
                    joystickActive[i] = false;
                    joystickString[i] = "#";
                    SaveInputs();
                    CheckDoubles(inputKey[i], i, 1);
                }
            }
            if (mouseAxisOn)
            {
                if (Input.GetAxis("MouseUp") == 1f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseUp";
                    inputString[i] = "Mouse Up";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString[i], i, 1);
                }
                if (Input.GetAxis("MouseDown") == 1f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseDown";
                    inputString[i] = "Mouse Down";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString[i], i, 1);
                }
                if (Input.GetAxis("MouseLeft") == 1f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseLeft";
                    inputBool[i] = false;
                    inputString[i] = "Mouse Left";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString[i], i, 1);
                }
                if (Input.GetAxis("MouseRight") == 1f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseRight";
                    inputString[i] = "Mouse Right";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString[i], i, 1);
                }
            }
            if (mouseButtonsOn)
            {
                if (Input.GetAxis("MouseScrollUp") > 0f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseScrollUp";
                    inputBool[i] = false;
                    inputString[i] = "Mouse scroll Up";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString[i], i, 1);
                }
                if (Input.GetAxis("MouseScrollDown") > 0f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseScrollDown";
                    inputBool[i] = false;
                    inputString[i] = "Mouse scroll Down";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString[i], i, 1);
                }
            }
            if (Input.GetAxis("JoystickUp") > 0.5f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "JoystickUp";
                inputString[i] = "Joystick Up";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("JoystickDown") > 0.5f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "JoystickDown";
                inputString[i] = "Joystick Down";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("JoystickLeft") > 0.5f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "JoystickLeft";
                inputString[i] = "Joystick Left";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("JoystickRight") > 0.5f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "JoystickRight";
                inputString[i] = "Joystick Right";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_3a") > 0.8f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_3a";
                inputString[i] = "Joystick Axis 3 +";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_3b") > 0.8f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_3b";
                inputString[i] = "Joystick Axis 3 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_4a") > 0.8f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_4a";
                inputString[i] = "Joystick Axis 4 +";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_4b") > 0.8f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_4b";
                inputString[i] = "Joystick Axis 4 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_5b") > 0.8f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_5b";
                inputString[i] = "Joystick Axis 5 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_6b") > 0.8f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_6b";
                inputString[i] = "Joystick Axis 6 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_7a") > 0.8f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_7a";
                inputString[i] = "Joystick Axis 7 +";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_7b") > 0.8f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_7b";
                inputString[i] = "Joystick Axis 7 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_8a") > 0.8f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_8a";
                inputString[i] = "Joystick Axis 8 +";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_8b") > 0.8f && inputBool[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_8b";
                inputString[i] = "Joystick Axis 8 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString[i], i, 1);
            }
        }
    }

    private void DrawButtons2()
    {
        float num = Boxes_Y;
        Vector3 mousePosition = Input.mousePosition;
        float x = mousePosition.x;
        Vector3 mousePosition2 = Input.mousePosition;
        float y = mousePosition2.y;
        Vector3 point = GUI.matrix.inverse.MultiplyPoint3x4(new Vector3(x, (float)Screen.height - y, 1f));
        GUI.skin = OurSkin;
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            num += BoxesMargin_Y;
            Rect position = new Rect(InputBox2_X, num, buttonSize, buttonHeight);
            GUI.Button(position, inputString2[i]);
            if (!joystickActive2[i] && inputKey2[i] == KeyCode.None)
            {
                joystickString2[i] = "#";
            }
            if (inputBool2[i])
            {
                GUI.Toggle(position, value: true, string.Empty, OurSkin.button);
            }
            if (position.Contains(point) && Input.GetMouseButtonUp(0) && !tempbool)
            {
                tempbool = true;
                inputBool2[i] = true;
                lastInterval = Time.realtimeSinceStartup;
            }
            if (Event.current.type == EventType.KeyDown && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = Event.current.keyCode;
                inputBool2[i] = false;
                inputString2[i] = inputKey2[i].ToString();
                tempbool = false;
                joystickActive2[i] = false;
                joystickString2[i] = "#";
                SaveInputs();
                CheckDoubles(inputKey2[i], i, 2);
            }
            if (mouseButtonsOn)
            {
                int num2 = 323;
                for (int j = 0; j < 6; j++)
                {
                    if (Input.GetMouseButton(j) && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
                    {
                        num2 += j;
                        inputKey2[i] = (KeyCode)num2;
                        inputBool2[i] = false;
                        inputString2[i] = inputKey2[i].ToString();
                        joystickActive2[i] = false;
                        joystickString2[i] = "#";
                        SaveInputs();
                        CheckDoubles(inputKey2[i], i, 2);
                    }
                }
            }
            for (int k = 350; k < 409; k++)
            {
                if (Input.GetKey((KeyCode)k) && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey2[i] = (KeyCode)k;
                    inputBool2[i] = false;
                    inputString2[i] = inputKey2[i].ToString();
                    tempbool = false;
                    joystickActive2[i] = false;
                    joystickString2[i] = "#";
                    SaveInputs();
                    CheckDoubles(inputKey2[i], i, 2);
                }
            }
            if (mouseAxisOn)
            {
                if (Input.GetAxis("MouseUp") == 1f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey2[i] = KeyCode.None;
                    inputBool2[i] = false;
                    joystickActive2[i] = true;
                    joystickString2[i] = "MouseUp";
                    inputString2[i] = "Mouse Up";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString2[i], i, 2);
                }
                if (Input.GetAxis("MouseDown") == 1f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey2[i] = KeyCode.None;
                    inputBool2[i] = false;
                    joystickActive2[i] = true;
                    joystickString2[i] = "MouseDown";
                    inputString2[i] = "Mouse Down";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString2[i], i, 2);
                }
                if (Input.GetAxis("MouseLeft") == 1f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey2[i] = KeyCode.None;
                    inputBool2[i] = false;
                    joystickActive2[i] = true;
                    joystickString2[i] = "MouseLeft";
                    inputBool2[i] = false;
                    inputString2[i] = "Mouse Left";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString2[i], i, 2);
                }
                if (Input.GetAxis("MouseRight") == 1f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey2[i] = KeyCode.None;
                    inputBool2[i] = false;
                    joystickActive2[i] = true;
                    joystickString2[i] = "MouseRight";
                    inputString2[i] = "Mouse Right";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString2[i], i, 2);
                }
            }
            if (mouseButtonsOn)
            {
                if (Input.GetAxis("MouseScrollUp") > 0f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey2[i] = KeyCode.None;
                    inputBool2[i] = false;
                    joystickActive2[i] = true;
                    joystickString2[i] = "MouseScrollUp";
                    inputBool2[i] = false;
                    inputString2[i] = "Mouse scroll Up";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString2[i], i, 2);
                }
                if (Input.GetAxis("MouseScrollDown") > 0f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
                {
                    inputKey2[i] = KeyCode.None;
                    inputBool2[i] = false;
                    joystickActive2[i] = true;
                    joystickString2[i] = "MouseScrollDown";
                    inputBool2[i] = false;
                    inputString2[i] = "Mouse scroll Down";
                    tempbool = false;
                    SaveInputs();
                    CheckDoubleAxis(joystickString2[i], i, 2);
                }
            }
            if (Input.GetAxis("JoystickUp") > 0.5f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "JoystickUp";
                inputString2[i] = "Joystick Up";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("JoystickDown") > 0.5f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "JoystickDown";
                inputString2[i] = "Joystick Down";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("JoystickLeft") > 0.5f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "JoystickLeft";
                inputBool2[i] = false;
                inputString2[i] = "Joystick Left";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("JoystickRight") > 0.5f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "JoystickRight";
                inputString2[i] = "Joystick Right";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("Joystick_3a") > 0.8f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "Joystick_3a";
                inputString2[i] = "Joystick Axis 3 +";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("Joystick_3b") > 0.8f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "Joystick_3b";
                inputString2[i] = "Joystick Axis 3 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("Joystick_4a") > 0.8f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "Joystick_4a";
                inputString2[i] = "Joystick Axis 4 +";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("Joystick_4b") > 0.8f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "Joystick_4b";
                inputString2[i] = "Joystick Axis 4 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("Joystick_5b") > 0.8f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "Joystick_5b";
                inputString2[i] = "Joystick Axis 5 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("Joystick_6b") > 0.8f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "Joystick_6b";
                inputString2[i] = "Joystick Axis 6 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("Joystick_7a") > 0.8f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "Joystick_7a";
                inputString2[i] = "Joystick Axis 7 +";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("Joystick_7b") > 0.8f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "Joystick_7b";
                inputString2[i] = "Joystick Axis 7 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("Joystick_8a") > 0.8f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "Joystick_8a";
                inputString2[i] = "Joystick Axis 8 +";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
            if (Input.GetAxis("Joystick_8b") > 0.8f && inputBool2[i] && Event.current.keyCode != KeyCode.Escape)
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                joystickActive2[i] = true;
                joystickString2[i] = "Joystick_8b";
                inputString2[i] = "Joystick Axis 8 -";
                tempbool = false;
                SaveInputs();
                CheckDoubleAxis(joystickString2[i], i, 2);
            }
        }
    }

    private void CheckDoubles(KeyCode testkey, int o, int p)
    {
        if (allowDuplicates)
        {
            return;
        }
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            if (testkey == inputKey[i] && (i != o || p == 2))
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                inputString[i] = inputKey[i].ToString();
                joystickActive[i] = false;
                joystickString[i] = "#";
                SaveInputs();
            }
            if (testkey == inputKey2[i] && (i != o || p == 1))
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                inputString2[i] = inputKey2[i].ToString();
                joystickActive2[i] = false;
                joystickString2[i] = "#";
                SaveInputs();
            }
        }
    }

    private void CheckDoubleAxis(string testAxisString, int o, int p)
    {
        if (allowDuplicates)
        {
            return;
        }
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            if (testAxisString == joystickString[i] && (i != o || p == 2))
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                inputString[i] = inputKey[i].ToString();
                joystickActive[i] = false;
                joystickString[i] = "#";
                SaveInputs();
            }
            if (testAxisString == joystickString2[i] && (i != o || p == 1))
            {
                inputKey2[i] = KeyCode.None;
                inputBool2[i] = false;
                inputString2[i] = inputKey2[i].ToString();
                joystickActive2[i] = false;
                joystickString2[i] = "#";
                SaveInputs();
            }
        }
    }
}
