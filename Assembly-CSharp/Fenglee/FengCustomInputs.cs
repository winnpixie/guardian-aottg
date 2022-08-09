using System;
using UnityEngine;

public class FengCustomInputs : MonoBehaviour
{
    public bool menuOn = true;
    public string[] DescriptionString;
    public KeyCode[] inputKey;
    public KeyCode[] default_inputKeys;
    public bool mouseAxisOn;
    public bool mouseButtonsOn = true;
    public bool allowDuplicates;
    private bool[] inputBool;
    public string[] inputString;
    [HideInInspector]
    public bool[] joystickActive;
    [HideInInspector]
    public string[] joystickString;
    private bool[] tempjoy1;
    [HideInInspector]
    public bool[] isInput;
    [HideInInspector]
    public bool[] isInputDown;
    [HideInInspector]
    public bool[] isInputUp;
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

    public void justUPDATEME()
    {
        Update();
    }

    private void Update()
    {
        if (!menuOn)
        {
            inputSetBools();
        }
    }

    private void OnGUI()
    {
        if (menuOn)
        {
            drawButtons1();
        }
    }

    private void inputSetBools()
    {
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            if (Input.GetKey(inputKey[i]) || (joystickActive[i] && Input.GetAxis(joystickString[i]) > 0.95f))
            {
                isInput[i] = true;
            }
            else
            {
                isInput[i] = false;
            }
            if (Input.GetKeyDown(inputKey[i]))
            {
                isInputDown[i] = true;
            }
            else
            {
                isInputDown[i] = false;
            }
            if (joystickActive[i] && Input.GetAxis(joystickString[i]) > 0.95f)
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
            if (!tempjoy1[i] && joystickActive[i] && Input.GetAxis(joystickString[i]) < 0.1f)
            {
                isInputDown[i] = false;
                tempjoy1[i] = true;
            }
            if (Input.GetKeyUp(inputKey[i]))
            {
                isInputUp[i] = true;
            }
            else
            {
                isInputUp[i] = false;
            }
        }
    }

    private void saveInputs()
    {
        string text = string.Empty;
        string text2 = string.Empty;
        string text3 = string.Empty;
        for (int num = DescriptionString.Length - 1; num > -1; num--)
        {
            text = (int)inputKey[num] + "*" + text;
            text2 = joystickString[num] + "*" + text2;
            text3 = inputString[num] + "*" + text3;
        }
        PlayerPrefs.SetString("KeyCodes", text);
        PlayerPrefs.SetString("Joystick_input", text2);
        PlayerPrefs.SetString("Names_input", text3);
        PlayerPrefs.SetInt("KeyLength", DescriptionString.Length);
    }

    private void reset2defaults()
    {
        if (default_inputKeys.Length != DescriptionString.Length)
        {
            default_inputKeys = new KeyCode[DescriptionString.Length];
        }
        string text = string.Empty;
        string text2 = string.Empty;
        string text3 = string.Empty;
        for (int num = DescriptionString.Length - 1; num > -1; num--)
        {
            text = (int)default_inputKeys[num] + "*" + text;
            text2 += "#*";
            text3 = default_inputKeys[num].ToString() + "*" + text3;
            PlayerPrefs.SetString("KeyCodes", text);
            PlayerPrefs.SetString("Joystick_input", text2);
            PlayerPrefs.SetString("Names_input", text3);
            PlayerPrefs.SetInt("KeyLength", DescriptionString.Length);
        }
    }

    private void loadConfig()
    {
        string keyCodes = PlayerPrefs.GetString("KeyCodes");
        string joystickInput = PlayerPrefs.GetString("Joystick_input");
        string namesInput = PlayerPrefs.GetString("Names_input");
        string[] keyCodeArray = keyCodes.Split('*');
        joystickString = joystickInput.Split('*');
        inputString = namesInput.Split('*');
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            int.TryParse(keyCodeArray[i], out int result);
            inputKey[i] = (KeyCode)result;
            if (joystickString[i] == "#")
            {
                joystickActive[i] = false;
            }
            else
            {
                joystickActive[i] = true;
            }
        }
    }

    public void startListening(int n)
    {
        inputBool[n] = true;
        lastInterval = Time.realtimeSinceStartup;
    }

    public void setToDefault()
    {
        reset2defaults();
        loadConfig();
        saveInputs();
        PlayerPrefs.SetFloat("MouseSensitivity", 0.5f);
        PlayerPrefs.SetString("version", UIMainReferences.Version);
        PlayerPrefs.SetInt("invertMouseY", 1);
        PlayerPrefs.SetInt("cameraTilt", 1);
        PlayerPrefs.SetFloat("GameQuality", 0.9f);
    }

    public void showKeyMap()
    {
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            if ((bool)GameObject.Find("CInput" + i))
            {
                GameObject.Find("CInput" + i).transform.Find("Label").gameObject.GetComponent<UILabel>().text = inputString[i];
            }
        }
        if ((bool)GameObject.Find("ChangeQuality"))
        {
            GameObject.Find("ChangeQuality").GetComponent<UISlider>().sliderValue = PlayerPrefs.GetFloat("GameQuality");
        }
        if ((bool)GameObject.Find("MouseSensitivity"))
        {
            GameObject.Find("MouseSensitivity").GetComponent<UISlider>().sliderValue = PlayerPrefs.GetFloat("MouseSensitivity");
        }
        if ((bool)GameObject.Find("CheckboxSettings"))
        {
            GameObject.Find("CheckboxSettings").GetComponent<UICheckbox>().isChecked = ((PlayerPrefs.GetInt("invertMouseY") != 1) ? true : false);
        }
        if ((bool)GameObject.Find("CheckboxCameraTilt"))
        {
            GameObject.Find("CheckboxCameraTilt").GetComponent<UICheckbox>().isChecked = ((PlayerPrefs.GetInt("cameraTilt") == 1) ? true : false);
        }
    }

    private void drawButtons1()
    {
        bool flag = false;
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            if (!joystickActive[i] && inputKey[i] == KeyCode.None)
            {
                joystickString[i] = "#";
            }
            bool flag2 = inputBool[i];
            if (Event.current.type == EventType.KeyDown && inputBool[i])
            {
                inputKey[i] = Event.current.keyCode;
                inputBool[i] = false;
                inputString[i] = inputKey[i].ToString();
                joystickActive[i] = false;
                joystickString[i] = "#";
                saveInputs();
                checDoubles(inputKey[i], i, 1);
            }
            if (mouseButtonsOn)
            {
                int num = 323;
                for (int j = 0; j < 6; j++)
                {
                    if (Input.GetMouseButton(j) && inputBool[i])
                    {
                        num += j;
                        inputKey[i] = (KeyCode)num;
                        inputBool[i] = false;
                        inputString[i] = inputKey[i].ToString();
                        joystickActive[i] = false;
                        joystickString[i] = "#";
                        saveInputs();
                        checDoubles(inputKey[i], i, 1);
                    }
                }
            }
            for (int k = 350; k < 409; k++)
            {
                if (Input.GetKey((KeyCode)k) && inputBool[i])
                {
                    inputKey[i] = (KeyCode)k;
                    inputBool[i] = false;
                    inputString[i] = inputKey[i].ToString();
                    joystickActive[i] = false;
                    joystickString[i] = "#";
                    saveInputs();
                    checDoubles(inputKey[i], i, 1);
                }
            }
            if (mouseAxisOn)
            {
                if (Input.GetAxis("MouseUp") == 1f && inputBool[i])
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseUp";
                    inputString[i] = "Mouse Up";
                    saveInputs();
                    checDoubleAxis(joystickString[i], i, 1);
                }
                if (Input.GetAxis("MouseDown") == 1f && inputBool[i])
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseDown";
                    inputString[i] = "Mouse Down";
                    saveInputs();
                    checDoubleAxis(joystickString[i], i, 1);
                }
                if (Input.GetAxis("MouseLeft") == 1f && inputBool[i])
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseLeft";
                    inputBool[i] = false;
                    inputString[i] = "Mouse Left";
                    saveInputs();
                    checDoubleAxis(joystickString[i], i, 1);
                }
                if (Input.GetAxis("MouseRight") == 1f && inputBool[i])
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseRight";
                    inputString[i] = "Mouse Right";
                    saveInputs();
                    checDoubleAxis(joystickString[i], i, 1);
                }
            }
            if (mouseButtonsOn)
            {
                if (Input.GetAxis("MouseScrollUp") > 0f && inputBool[i])
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseScrollUp";
                    inputBool[i] = false;
                    inputString[i] = "Mouse scroll Up";
                    saveInputs();
                    checDoubleAxis(joystickString[i], i, 1);
                }
                if (Input.GetAxis("MouseScrollDown") > 0f && inputBool[i])
                {
                    inputKey[i] = KeyCode.None;
                    inputBool[i] = false;
                    joystickActive[i] = true;
                    joystickString[i] = "MouseScrollDown";
                    inputBool[i] = false;
                    inputString[i] = "Mouse scroll Down";
                    saveInputs();
                    checDoubleAxis(joystickString[i], i, 1);
                }
            }
            if (Input.GetAxis("JoystickUp") > 0.5f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "JoystickUp";
                inputString[i] = "Joystick Up";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("JoystickDown") > 0.5f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "JoystickDown";
                inputString[i] = "Joystick Down";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("JoystickLeft") > 0.5f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "JoystickLeft";
                inputString[i] = "Joystick Left";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("JoystickRight") > 0.5f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "JoystickRight";
                inputString[i] = "Joystick Right";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_3a") > 0.8f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_3a";
                inputString[i] = "Joystick Axis 3 +";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_3b") > 0.8f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_3b";
                inputString[i] = "Joystick Axis 3 -";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_4a") > 0.8f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_4a";
                inputString[i] = "Joystick Axis 4 +";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_4b") > 0.8f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_4b";
                inputString[i] = "Joystick Axis 4 -";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_5b") > 0.8f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_5b";
                inputString[i] = "Joystick Axis 5 -";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_6b") > 0.8f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_6b";
                inputString[i] = "Joystick Axis 6 -";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_7a") > 0.8f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_7a";
                inputString[i] = "Joystick Axis 7 +";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_7b") > 0.8f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_7b";
                inputString[i] = "Joystick Axis 7 -";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_8a") > 0.8f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_8a";
                inputString[i] = "Joystick Axis 8 +";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (Input.GetAxis("Joystick_8b") > 0.8f && inputBool[i])
            {
                inputKey[i] = KeyCode.None;
                inputBool[i] = false;
                joystickActive[i] = true;
                joystickString[i] = "Joystick_8b";
                inputString[i] = "Joystick Axis 8 -";
                saveInputs();
                checDoubleAxis(joystickString[i], i, 1);
            }
            if (flag2 != inputBool[i])
            {
                flag = true;
            }
        }
        if (flag)
        {
            showKeyMap();
        }
    }

    private void checDoubles(KeyCode testkey, int o, int p)
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
                saveInputs();
            }
        }
    }

    private void checDoubleAxis(string testAxisString, int o, int p)
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
                saveInputs();
            }
        }
    }

    public void setKeyRC(int i, string setting)
    {
        if (setting == "Scroll Up" || setting == "Scroll Down")
        {
            if (setting == "Scroll Up")
            {
                joystickString[i] = "MouseScrollUp";
                inputString[i] = "Mouse scroll Up";
            }
            else if (setting == "Scroll Down")
            {
                joystickString[i] = "MouseScrollDown";
                inputString[i] = "Mouse scroll Down";
            }
            inputKey[i] = KeyCode.None;
            inputBool[i] = false;
            joystickActive[i] = true;
            inputBool[i] = false;
            saveInputs();
            checDoubleAxis(joystickString[i], i, 1);
        }
        else
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
            inputKey[i] = keyCode;
            inputBool[i] = false;
            inputString[i] = inputKey[i].ToString();
            joystickActive[i] = false;
            joystickString[i] = "#";
            saveInputs();
            checDoubles(inputKey[i], i, 1);
        }
    }

    public void setNameRC(int i, string str)
    {
        inputString[i] = str;
    }

    public string getKeyRC(int i)
    {
        return inputString[i];
    }

    private void Start()
    {
        inputBool = new bool[DescriptionString.Length];
        inputString = new string[DescriptionString.Length];
        inputKey = new KeyCode[DescriptionString.Length];
        joystickActive = new bool[DescriptionString.Length];
        joystickString = new string[DescriptionString.Length];
        isInput = new bool[DescriptionString.Length];
        isInputDown = new bool[DescriptionString.Length];
        isInputUp = new bool[DescriptionString.Length];
        tempLength = PlayerPrefs.GetInt("KeyLength");
        tempjoy1 = new bool[DescriptionString.Length];
        if (!PlayerPrefs.HasKey("version"))
        {
            setToDefault();
        }
        tempLength = PlayerPrefs.GetInt("KeyLength");
        if (PlayerPrefs.HasKey("KeyCodes") && tempLength == DescriptionString.Length)
        {
            loadConfig();
        }
        else
        {
            setToDefault();
        }
        for (int i = 0; i < DescriptionString.Length; i++)
        {
            isInput[i] = false;
            isInputDown[i] = false;
            isInputUp[i] = false;
            tempjoy1[i] = true;
        }
    }
}
