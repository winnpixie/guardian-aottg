using System;
using System.Collections.Generic;
using UnityEngine;

namespace RC.UI.Impl
{
    class RCGuiKeyBindSettings : RCGui
    {
        public override void Draw(FengGameManagerMKII fgm, float halfMenuWidth, float halfMenuHeight)
        {
            if (GUI.Button(new Rect(halfMenuWidth + 233f, halfMenuHeight + 51f, 55f, 25f), "Human"))
            {
                FengGameManagerMKII.Settings[190] = 0;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 293f, halfMenuHeight + 51f, 52f, 25f), "Titan"))
            {
                FengGameManagerMKII.Settings[190] = 1;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 350f, halfMenuHeight + 51f, 53f, 25f), "Horse"))
            {
                FengGameManagerMKII.Settings[190] = 2;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 408f, halfMenuHeight + 51f, 59f, 25f), "Cannon"))
            {
                FengGameManagerMKII.Settings[190] = 3;
            }
            if ((int)FengGameManagerMKII.Settings[190] == 0)
            {
                List<string> list6 = new List<string>
                                {
                                    "Forward:",
                                    "Backward:",
                                    "Left:",
                                    "Right:",
                                    "Jump:",
                                    "Dodge:",
                                    "Left Hook:",
                                    "Right Hook:",
                                    "Both Hooks:",
                                    "Lock:",
                                    "Attack:",
                                    "Special:",
                                    "Salute:",
                                    "Change Camera:",
                                    "Reset:",
                                    "Pause:",
                                    "Show/Hide Cursor:",
                                    "Fullscreen:",
                                    "Change Blade:",
                                    "Flare Green:",
                                    "Flare Red:",
                                    "Flare Black:",
                                    "Reel in:",
                                    "Reel out:",
                                    "Gas Burst:",
                                    "Minimap Max:",
                                    "Minimap Toggle:",
                                    "Minimap Reset:",
                                    "Open Chat:",
                                    "Live Spectate"
                                };

                for (int j = 0; j < list6.Count; j++)
                {
                    int k = j;
                    float num31 = 80f;
                    if (k > 14)
                    {
                        num31 = 390f;
                        k -= 15;
                    }
                    GUI.Label(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 145f, 22f), list6[j], "Label");
                }
                bool flag36 = false;
                if ((int)FengGameManagerMKII.Settings[97] == 1)
                {
                    flag36 = true;
                }
                bool flag37 = false;
                if ((int)FengGameManagerMKII.Settings[116] == 1)
                {
                    flag37 = true;
                }
                bool flag38 = false;
                if ((int)FengGameManagerMKII.Settings[181] == 1)
                {
                    flag38 = true;
                }
                bool flag39 = GUI.Toggle(new Rect(halfMenuWidth + 457f, halfMenuHeight + 261f, 40f, 20f), flag36, "On");
                if (flag36 != flag39)
                {
                    if (flag39)
                    {
                        FengGameManagerMKII.Settings[97] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[97] = 0;
                    }
                }
                bool flag40 = GUI.Toggle(new Rect(halfMenuWidth + 457f, halfMenuHeight + 286f, 40f, 20f), flag37, "On");
                if (flag37 != flag40)
                {
                    if (flag40)
                    {
                        FengGameManagerMKII.Settings[116] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[116] = 0;
                    }
                }
                bool flag41 = GUI.Toggle(new Rect(halfMenuWidth + 457f, halfMenuHeight + 311f, 40f, 20f), flag38, "On");
                if (flag38 != flag41)
                {
                    if (flag41)
                    {
                        FengGameManagerMKII.Settings[181] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[181] = 0;
                    }
                }
                for (int j = 0; j < 22; j++)
                {
                    int k = j;
                    float num31 = 190f;
                    if (k > 14)
                    {
                        num31 = 500f;
                        k -= 15;
                    }
                    if (GUI.Button(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 120f, 20f), fgm.inputManager.GetKeyRC(j)))
                    {
                        FengGameManagerMKII.Settings[100] = j + 1;
                        fgm.inputManager.SetNameRC(j, "waiting...");
                    }
                }
                if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 261f, 120f, 20f), (string)FengGameManagerMKII.Settings[98]))
                {
                    FengGameManagerMKII.Settings[98] = "waiting...";
                    FengGameManagerMKII.Settings[100] = 98;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 286f, 120f, 20f), (string)FengGameManagerMKII.Settings[99]))
                {
                    FengGameManagerMKII.Settings[99] = "waiting...";
                    FengGameManagerMKII.Settings[100] = 99;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 311f, 120f, 20f), (string)FengGameManagerMKII.Settings[182]))
                {
                    FengGameManagerMKII.Settings[182] = "waiting...";
                    FengGameManagerMKII.Settings[100] = 182;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 336f, 120f, 20f), (string)FengGameManagerMKII.Settings[232]))
                {
                    FengGameManagerMKII.Settings[232] = "waiting...";
                    FengGameManagerMKII.Settings[100] = 232;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 361f, 120f, 20f), (string)FengGameManagerMKII.Settings[233]))
                {
                    FengGameManagerMKII.Settings[233] = "waiting...";
                    FengGameManagerMKII.Settings[100] = 233;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 386f, 120f, 20f), (string)FengGameManagerMKII.Settings[234]))
                {
                    FengGameManagerMKII.Settings[234] = "waiting...";
                    FengGameManagerMKII.Settings[100] = 234;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 411f, 120f, 20f), (string)FengGameManagerMKII.Settings[236]))
                {
                    FengGameManagerMKII.Settings[236] = "waiting...";
                    FengGameManagerMKII.Settings[100] = 236;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 436f, 120f, 20f), (string)FengGameManagerMKII.Settings[262]))
                {
                    FengGameManagerMKII.Settings[262] = "waiting...";
                    FengGameManagerMKII.Settings[100] = 262;
                }
                if ((int)FengGameManagerMKII.Settings[100] != 0)
                {
                    Event current = Event.current;
                    bool flag3 = false;
                    string text3 = "waiting...";
                    if (current.type == EventType.KeyDown && current.keyCode != 0)
                    {
                        flag3 = true;
                        text3 = current.keyCode.ToString();
                    }
                    else if (Input.GetKey(KeyCode.LeftShift))
                    {
                        flag3 = true;
                        text3 = KeyCode.LeftShift.ToString();
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        flag3 = true;
                        text3 = KeyCode.RightShift.ToString();
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                    {
                        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                        {
                            flag3 = true;
                            text3 = "Scroll Up";
                        }
                        else
                        {
                            flag3 = true;
                            text3 = "Scroll Down";
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (Input.GetKeyDown((KeyCode)(323 + j)))
                            {
                                flag3 = true;
                                text3 = "Mouse" + Convert.ToString(j);
                            }
                        }
                    }
                    if (flag3)
                    {
                        if ((int)FengGameManagerMKII.Settings[100] == 98)
                        {
                            FengGameManagerMKII.Settings[98] = text3;
                            FengGameManagerMKII.Settings[100] = 0;
                            FengGameManagerMKII.InputRC.SetInputHuman(InputCodeRC.ReelIn, text3);
                        }
                        else if ((int)FengGameManagerMKII.Settings[100] == 99)
                        {
                            FengGameManagerMKII.Settings[99] = text3;
                            FengGameManagerMKII.Settings[100] = 0;
                            FengGameManagerMKII.InputRC.SetInputHuman(InputCodeRC.ReelOut, text3);
                        }
                        else if ((int)FengGameManagerMKII.Settings[100] == 182)
                        {
                            FengGameManagerMKII.Settings[182] = text3;
                            FengGameManagerMKII.Settings[100] = 0;
                            FengGameManagerMKII.InputRC.SetInputHuman(InputCodeRC.Dash, text3);
                        }
                        else if ((int)FengGameManagerMKII.Settings[100] == 232)
                        {
                            FengGameManagerMKII.Settings[232] = text3;
                            FengGameManagerMKII.Settings[100] = 0;
                            FengGameManagerMKII.InputRC.SetInputHuman(InputCodeRC.MapMaximize, text3);
                        }
                        else if ((int)FengGameManagerMKII.Settings[100] == 233)
                        {
                            FengGameManagerMKII.Settings[233] = text3;
                            FengGameManagerMKII.Settings[100] = 0;
                            FengGameManagerMKII.InputRC.SetInputHuman(InputCodeRC.MapToggle, text3);
                        }
                        else if ((int)FengGameManagerMKII.Settings[100] == 234)
                        {
                            FengGameManagerMKII.Settings[234] = text3;
                            FengGameManagerMKII.Settings[100] = 0;
                            FengGameManagerMKII.InputRC.SetInputHuman(InputCodeRC.MapReset, text3);
                        }
                        else if ((int)FengGameManagerMKII.Settings[100] == 236)
                        {
                            FengGameManagerMKII.Settings[236] = text3;
                            FengGameManagerMKII.Settings[100] = 0;
                            FengGameManagerMKII.InputRC.SetInputHuman(InputCodeRC.Chat, text3);
                        }
                        else if ((int)FengGameManagerMKII.Settings[100] == 262)
                        {
                            FengGameManagerMKII.Settings[262] = text3;
                            FengGameManagerMKII.Settings[100] = 0;
                            FengGameManagerMKII.InputRC.SetInputHuman(InputCodeRC.LiveCamera, text3);
                        }
                        else
                        {
                            for (int j = 0; j < 22; j++)
                            {
                                int num16 = j + 1;
                                if ((int)FengGameManagerMKII.Settings[100] == num16)
                                {
                                    fgm.inputManager.SetKeyRC(j, text3);
                                    FengGameManagerMKII.Settings[100] = 0;
                                }
                            }
                        }
                    }
                }
            }
            else if ((int)FengGameManagerMKII.Settings[190] == 1)
            {
                List<string> list8 = new List<string>
                                {
                                    "Forward:",
                                    "Back:",
                                    "Left:",
                                    "Right:",
                                    "Walk:",
                                    "Jump:",
                                    "Punch:",
                                    "Slam:",
                                    "Grab (front):",
                                    "Grab (back):",
                                    "Grab (nape):",
                                    "Slap:",
                                    "Bite:",
                                    "Cover Nape:"
                                };

                for (int j = 0; j < list8.Count; j++)
                {
                    int k = j;
                    float num31 = 80f;
                    if (k > 6)
                    {
                        num31 = 390f;
                        k -= 7;
                    }
                    GUI.Label(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 145f, 22f), list8[j], "Label");
                }
                for (int j = 0; j < 14; j++)
                {
                    int num16 = 101 + j;
                    int k = j;
                    float num31 = 190f;
                    if (k > 6)
                    {
                        num31 = 500f;
                        k -= 7;
                    }
                    if (GUI.Button(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 120f, 20f), (string)FengGameManagerMKII.Settings[num16]))
                    {
                        FengGameManagerMKII.Settings[num16] = "waiting...";
                        FengGameManagerMKII.Settings[100] = num16;
                    }
                }
                if ((int)FengGameManagerMKII.Settings[100] != 0)
                {
                    Event current = Event.current;
                    bool flag3 = false;
                    string text3 = "waiting...";
                    if (current.type == EventType.KeyDown && current.keyCode != 0)
                    {
                        flag3 = true;
                        text3 = current.keyCode.ToString();
                    }
                    else if (Input.GetKey(KeyCode.LeftShift))
                    {
                        flag3 = true;
                        text3 = KeyCode.LeftShift.ToString();
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        flag3 = true;
                        text3 = KeyCode.RightShift.ToString();
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                    {
                        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                        {
                            flag3 = true;
                            text3 = "Scroll Up";
                        }
                        else
                        {
                            flag3 = true;
                            text3 = "Scroll Down";
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (Input.GetKeyDown((KeyCode)(323 + j)))
                            {
                                flag3 = true;
                                text3 = "Mouse" + Convert.ToString(j);
                            }
                        }
                    }
                    if (flag3)
                    {
                        for (int j = 0; j < 14; j++)
                        {
                            int num16 = 101 + j;
                            if ((int)FengGameManagerMKII.Settings[100] == num16)
                            {
                                FengGameManagerMKII.Settings[num16] = text3;
                                FengGameManagerMKII.Settings[100] = 0;
                                FengGameManagerMKII.InputRC.SetInputTitan(j, text3);
                            }
                        }
                    }
                }
            }
            else if ((int)FengGameManagerMKII.Settings[190] == 2)
            {
                List<string> list9 = new List<string>
                                {
                                    "Forward:",
                                    "Back:",
                                    "Left:",
                                    "Right:",
                                    "Walk:",
                                    "Jump:",
                                    "Mount:"
                                };

                for (int j = 0; j < list9.Count; j++)
                {
                    int k = j;
                    float num31 = 80f;
                    if (k > 3)
                    {
                        num31 = 390f;
                        k -= 4;
                    }
                    GUI.Label(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 145f, 22f), list9[j], "Label");
                }
                for (int j = 0; j < 7; j++)
                {
                    int num16 = 237 + j;
                    int k = j;
                    float num31 = 190f;
                    if (k > 3)
                    {
                        num31 = 500f;
                        k -= 4;
                    }
                    if (GUI.Button(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 120f, 20f), (string)FengGameManagerMKII.Settings[num16]))
                    {
                        FengGameManagerMKII.Settings[num16] = "waiting...";
                        FengGameManagerMKII.Settings[100] = num16;
                    }
                }
                if ((int)FengGameManagerMKII.Settings[100] != 0)
                {
                    Event current = Event.current;
                    bool flag3 = false;
                    string text3 = "waiting...";
                    if (current.type == EventType.KeyDown && current.keyCode != 0)
                    {
                        flag3 = true;
                        text3 = current.keyCode.ToString();
                    }
                    else if (Input.GetKey(KeyCode.LeftShift))
                    {
                        flag3 = true;
                        text3 = KeyCode.LeftShift.ToString();
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        flag3 = true;
                        text3 = KeyCode.RightShift.ToString();
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                    {
                        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                        {
                            flag3 = true;
                            text3 = "Scroll Up";
                        }
                        else
                        {
                            flag3 = true;
                            text3 = "Scroll Down";
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (Input.GetKeyDown((KeyCode)(323 + j)))
                            {
                                flag3 = true;
                                text3 = "Mouse" + Convert.ToString(j);
                            }
                        }
                    }
                    if (flag3)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            int num16 = 237 + j;
                            if ((int)FengGameManagerMKII.Settings[100] == num16)
                            {
                                FengGameManagerMKII.Settings[num16] = text3;
                                FengGameManagerMKII.Settings[100] = 0;
                                FengGameManagerMKII.InputRC.SetInputHorse(j, text3);
                            }
                        }
                    }
                }
            }
            else if ((int)FengGameManagerMKII.Settings[190] == 3)
            {
                List<string> list10 = new List<string>
                                {
                                    "Rotate Up:",
                                    "Rotate Down:",
                                    "Rotate Left:",
                                    "Rotate Right:",
                                    "Fire:",
                                    "Mount:",
                                    "Slow Rotate:"
                                };

                for (int j = 0; j < list10.Count; j++)
                {
                    int k = j;
                    float num31 = 80f;
                    if (k > 3)
                    {
                        num31 = 390f;
                        k -= 4;
                    }
                    GUI.Label(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 145f, 22f), list10[j], "Label");
                }
                for (int j = 0; j < 7; j++)
                {
                    int num16 = 254 + j;
                    int k = j;
                    float num31 = 190f;
                    if (k > 3)
                    {
                        num31 = 500f;
                        k -= 4;
                    }
                    if (GUI.Button(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 120f, 20f), (string)FengGameManagerMKII.Settings[num16]))
                    {
                        FengGameManagerMKII.Settings[num16] = "waiting...";
                        FengGameManagerMKII.Settings[100] = num16;
                    }
                }
                if ((int)FengGameManagerMKII.Settings[100] != 0)
                {
                    Event current = Event.current;
                    bool flag3 = false;
                    string text3 = "waiting...";
                    if (current.type == EventType.KeyDown && current.keyCode != 0)
                    {
                        flag3 = true;
                        text3 = current.keyCode.ToString();
                    }
                    else if (Input.GetKey(KeyCode.LeftShift))
                    {
                        flag3 = true;
                        text3 = KeyCode.LeftShift.ToString();
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        flag3 = true;
                        text3 = KeyCode.RightShift.ToString();
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                    {
                        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                        {
                            flag3 = true;
                            text3 = "Scroll Up";
                        }
                        else
                        {
                            flag3 = true;
                            text3 = "Scroll Down";
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if (Input.GetKeyDown((KeyCode)(323 + j)))
                            {
                                flag3 = true;
                                text3 = "Mouse" + Convert.ToString(j);
                            }
                        }
                    }
                    if (flag3)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            int num16 = 254 + j;
                            if ((int)FengGameManagerMKII.Settings[100] == num16)
                            {
                                FengGameManagerMKII.Settings[num16] = text3;
                                FengGameManagerMKII.Settings[100] = 0;
                                FengGameManagerMKII.InputRC.SetInputCannon(j, text3);
                            }
                        }
                    }
                }
            }
        }
    }
}
