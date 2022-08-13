using System;
using UnityEngine;

public class InputManagerRC
{
    public int[] humanWheel = new int[8];
    public KeyCode[] humanKeys = new KeyCode[8];
    public int[] horseWheel = new int[7];
    public KeyCode[] horseKeys = new KeyCode[7];
    public int[] titanWheel = new int[15];
    public KeyCode[] titanKeys = new KeyCode[15];
    public int[] levelWheel = new int[17];
    public KeyCode[] levelKeys = new KeyCode[17];
    public int[] cannonWheel = new int[7];
    public KeyCode[] cannonKeys = new KeyCode[7];

    public InputManagerRC()
    {
        for (int i = 0; i < humanWheel.Length; i++)
        {
            humanWheel[i] = 0;
            humanKeys[i] = KeyCode.None;
        }
        for (int i = 0; i < horseWheel.Length; i++)
        {
            horseWheel[i] = 0;
            horseKeys[i] = KeyCode.None;
        }
        for (int i = 0; i < titanWheel.Length; i++)
        {
            titanWheel[i] = 0;
            titanKeys[i] = KeyCode.None;
        }
        for (int i = 0; i < levelWheel.Length; i++)
        {
            levelWheel[i] = 0;
            levelKeys[i] = KeyCode.None;
        }
    }

    public bool IsInputHuman(int code)
    {
        if (humanWheel[code] != 0)
        {
            return Input.GetAxis("Mouse ScrollWheel") * (float)humanWheel[code] > 0f;
        }
        return Input.GetKey(humanKeys[code]);
    }

    public bool IsInputHumanDown(int code)
    {
        if (humanWheel[code] != 0)
        {
            return Input.GetAxis("Mouse ScrollWheel") * (float)humanWheel[code] > 0f;
        }
        return Input.GetKeyDown(humanKeys[code]);
    }

    public bool IsInputHorse(int code)
    {
        if (horseWheel[code] != 0)
        {
            return Input.GetAxis("Mouse ScrollWheel") * (float)horseWheel[code] > 0f;
        }
        return Input.GetKey(horseKeys[code]);
    }

    public bool IsInputHorseDown(int code)
    {
        if (horseWheel[code] != 0)
        {
            return Input.GetAxis("Mouse ScrollWheel") * (float)horseWheel[code] > 0f;
        }
        return Input.GetKeyDown(horseKeys[code]);
    }

    public bool IsInputTitan(int code)
    {
        if (titanWheel[code] != 0)
        {
            return Input.GetAxis("Mouse ScrollWheel") * (float)titanWheel[code] > 0f;
        }
        return Input.GetKey(titanKeys[code]);
    }

    public bool IsInputLevel(int code)
    {
        if (levelWheel[code] != 0)
        {
            return Input.GetAxis("Mouse ScrollWheel") * (float)levelWheel[code] > 0f;
        }
        return Input.GetKey(levelKeys[code]);
    }

    public bool IsInputLevelDown(int code)
    {
        if (levelWheel[code] != 0)
        {
            return Input.GetAxis("Mouse ScrollWheel") * (float)levelWheel[code] > 0f;
        }
        return Input.GetKeyDown(levelKeys[code]);
    }

    public bool IsInputCannon(int code)
    {
        if (cannonWheel[code] != 0)
        {
            return Input.GetAxis("Mouse ScrollWheel") * (float)cannonWheel[code] > 0f;
        }
        return Input.GetKey(cannonKeys[code]);
    }

    public bool IsInputCannonDown(int code)
    {
        if (cannonWheel[code] != 0)
        {
            return Input.GetAxis("Mouse ScrollWheel") * (float)cannonWheel[code] > 0f;
        }
        return Input.GetKeyDown(cannonKeys[code]);
    }

    public void SetInputHuman(int code, string setting)
    {
        humanKeys[code] = KeyCode.None;
        humanWheel[code] = 0;
        if (setting == "Scroll Up")
        {
            humanWheel[code] = 1;
        }
        else if (setting == "Scroll Down")
        {
            humanWheel[code] = -1;
        }
        else if (Enum.IsDefined(typeof(KeyCode), setting))
        {
            humanKeys[code] = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
        }
    }

    public void SetInputHorse(int code, string setting)
    {
        horseKeys[code] = KeyCode.None;
        horseWheel[code] = 0;
        if (setting == "Scroll Up")
        {
            horseWheel[code] = 1;
        }
        else if (setting == "Scroll Down")
        {
            horseWheel[code] = -1;
        }
        else if (Enum.IsDefined(typeof(KeyCode), setting))
        {
            horseKeys[code] = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
        }
    }

    public void SetInputCannon(int code, string setting)
    {
        cannonKeys[code] = KeyCode.None;
        cannonWheel[code] = 0;
        if (setting == "Scroll Up")
        {
            cannonWheel[code] = 1;
        }
        else if (setting == "Scroll Down")
        {
            cannonWheel[code] = -1;
        }
        else if (Enum.IsDefined(typeof(KeyCode), setting))
        {
            cannonKeys[code] = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
        }
    }

    public void SetInputTitan(int code, string setting)
    {
        titanKeys[code] = KeyCode.None;
        titanWheel[code] = 0;
        if (setting == "Scroll Up")
        {
            titanWheel[code] = 1;
        }
        else if (setting == "Scroll Down")
        {
            titanWheel[code] = -1;
        }
        else if (Enum.IsDefined(typeof(KeyCode), setting))
        {
            titanKeys[code] = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
        }
    }

    public void SetInputLevel(int code, string setting)
    {
        levelKeys[code] = KeyCode.None;
        levelWheel[code] = 0;
        if (setting == "Scroll Up")
        {
            levelWheel[code] = 1;
        }
        else if (setting == "Scroll Down")
        {
            levelWheel[code] = -1;
        }
        else if (Enum.IsDefined(typeof(KeyCode), setting))
        {
            levelKeys[code] = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
        }
    }
}
