using UnityEngine;

namespace RC.UI.Impl
{
    class RCGuiMasterClientSettings : RCGui
    {
        public override void Draw(FengGameManagerMKII fgm, float halfMenuWidth, float halfMenuHeight)
        {
            GUI.Label(new Rect(halfMenuWidth + 200f, halfMenuHeight + 382f, 400f, 22f), "Master Client only. Changes will take effect upon restart.");
            if (GUI.Button(new Rect(halfMenuWidth + 267.5f, halfMenuHeight + 50f, 60f, 25f), "Titans"))
            {
                FengGameManagerMKII.Settings[230] = 0;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 332.5f, halfMenuHeight + 50f, 40f, 25f), "PVP"))
            {
                FengGameManagerMKII.Settings[230] = 1;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 377.5f, halfMenuHeight + 50f, 50f, 25f), "Misc"))
            {
                FengGameManagerMKII.Settings[230] = 2;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 320f, halfMenuHeight + 415f, 60f, 30f), "Reset"))
            {
                FengGameManagerMKII.Settings[192] = 0;
                FengGameManagerMKII.Settings[193] = 0;
                FengGameManagerMKII.Settings[194] = 0;
                FengGameManagerMKII.Settings[195] = 0;
                FengGameManagerMKII.Settings[196] = "30";
                FengGameManagerMKII.Settings[197] = 0;
                FengGameManagerMKII.Settings[198] = "100";
                FengGameManagerMKII.Settings[199] = "200";
                FengGameManagerMKII.Settings[200] = 0;
                FengGameManagerMKII.Settings[201] = "1";
                FengGameManagerMKII.Settings[202] = 0;
                FengGameManagerMKII.Settings[203] = 0;
                FengGameManagerMKII.Settings[204] = "1";
                FengGameManagerMKII.Settings[205] = 0;
                FengGameManagerMKII.Settings[206] = "1000";
                FengGameManagerMKII.Settings[207] = 0;
                FengGameManagerMKII.Settings[208] = "1.0";
                FengGameManagerMKII.Settings[209] = "3.0";
                FengGameManagerMKII.Settings[210] = 0;
                FengGameManagerMKII.Settings[211] = "20.0";
                FengGameManagerMKII.Settings[212] = "20.0";
                FengGameManagerMKII.Settings[213] = "20.0";
                FengGameManagerMKII.Settings[214] = "20.0";
                FengGameManagerMKII.Settings[215] = "20.0";
                FengGameManagerMKII.Settings[216] = 0;
                FengGameManagerMKII.Settings[217] = 0;
                FengGameManagerMKII.Settings[218] = "1";
                FengGameManagerMKII.Settings[219] = 0;
                FengGameManagerMKII.Settings[220] = 0;
                FengGameManagerMKII.Settings[221] = 0;
                FengGameManagerMKII.Settings[222] = "20";
                FengGameManagerMKII.Settings[223] = 0;
                FengGameManagerMKII.Settings[224] = "10";
                FengGameManagerMKII.Settings[225] = string.Empty;
                FengGameManagerMKII.Settings[226] = 0;
                FengGameManagerMKII.Settings[227] = "50";
                FengGameManagerMKII.Settings[228] = 0;
                FengGameManagerMKII.Settings[229] = 0;
                FengGameManagerMKII.Settings[235] = 0;
            }
            if ((int)FengGameManagerMKII.Settings[230] == 0)
            {
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 90f, 160f, 22f), "Custom Titan Number:", "Label");
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 112f, 200f, 22f), "Amount (Integer):", "Label");
                FengGameManagerMKII.Settings[204] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 112f, 50f, 22f), (string)FengGameManagerMKII.Settings[204]);
                bool flag34 = false;
                if ((int)FengGameManagerMKII.Settings[203] == 1)
                {
                    flag34 = true;
                }
                bool flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 90f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[203] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[203] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 152f, 160f, 22f), "Custom Titan Spawns:", "Label");
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[210] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 152f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[210] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[210] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 174f, 150f, 22f), "Normal (Decimal):", "Label");
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 196f, 150f, 22f), "Aberrant (Decimal):", "Label");
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 218f, 150f, 22f), "Jumper (Decimal):", "Label");
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 240f, 150f, 22f), "Crawler (Decimal):", "Label");
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 262f, 150f, 22f), "Punk (Decimal):", "Label");
                FengGameManagerMKII.Settings[211] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 174f, 50f, 22f), (string)FengGameManagerMKII.Settings[211]);
                FengGameManagerMKII.Settings[212] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 196f, 50f, 22f), (string)FengGameManagerMKII.Settings[212]);
                FengGameManagerMKII.Settings[213] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 218f, 50f, 22f), (string)FengGameManagerMKII.Settings[213]);
                FengGameManagerMKII.Settings[214] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 240f, 50f, 22f), (string)FengGameManagerMKII.Settings[214]);
                FengGameManagerMKII.Settings[215] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 262f, 50f, 22f), (string)FengGameManagerMKII.Settings[215]);
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 302f, 160f, 22f), "Titan Size Mode:", "Label");
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 324f, 150f, 22f), "Minimum (Decimal):", "Label");
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 346f, 150f, 22f), "Maximum (Decimal):", "Label");
                FengGameManagerMKII.Settings[208] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 324f, 50f, 22f), (string)FengGameManagerMKII.Settings[208]);
                FengGameManagerMKII.Settings[209] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 346f, 50f, 22f), (string)FengGameManagerMKII.Settings[209]);
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[207] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 302f, 40f, 20f), flag34, "On");
                if (flag35 != flag34)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[207] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[207] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 90f, 160f, 22f), "Titan Health Mode:", "Label");
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 161f, 150f, 22f), "Minimum (Integer):", "Label");
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 183f, 150f, 22f), "Maximum (Integer):", "Label");
                FengGameManagerMKII.Settings[198] = GUI.TextField(new Rect(halfMenuWidth + 550f, halfMenuHeight + 161f, 50f, 22f), (string)FengGameManagerMKII.Settings[198]);
                FengGameManagerMKII.Settings[199] = GUI.TextField(new Rect(halfMenuWidth + 550f, halfMenuHeight + 183f, 50f, 22f), (string)FengGameManagerMKII.Settings[199]);
                string[] texts = new string[3]
                {
                            "Off",
                            "Fixed",
                            "Scaled"
                };
                FengGameManagerMKII.Settings[197] = GUI.SelectionGrid(new Rect(halfMenuWidth + 550f, halfMenuHeight + 90f, 100f, 66f), (int)FengGameManagerMKII.Settings[197], texts, 1, GUI.skin.toggle);
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 223f, 160f, 22f), "Titan Damage Mode:", "Label");
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 245f, 150f, 22f), "Damage (Integer):", "Label");
                FengGameManagerMKII.Settings[206] = GUI.TextField(new Rect(halfMenuWidth + 550f, halfMenuHeight + 245f, 50f, 22f), (string)FengGameManagerMKII.Settings[206]);
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[205] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 223f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[205] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[205] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 285f, 160f, 22f), "Titan Explode Mode:", "Label");
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 307f, 160f, 22f), "Radius (Integer):", "Label");
                FengGameManagerMKII.Settings[196] = GUI.TextField(new Rect(halfMenuWidth + 550f, halfMenuHeight + 307f, 50f, 22f), (string)FengGameManagerMKII.Settings[196]);
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[195] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 285f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[195] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[195] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 347f, 160f, 22f), "Disable Rock Throwing:", "Label");
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[194] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 347f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[194] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[194] = 0;
                    }
                }
            }
            else if ((int)FengGameManagerMKII.Settings[230] == 1)
            {
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 90f, 160f, 22f), "Point Mode:", "Label");
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 112f, 160f, 22f), "Max Points (Integer):", "Label");
                FengGameManagerMKII.Settings[227] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 112f, 50f, 22f), (string)FengGameManagerMKII.Settings[227]);
                bool flag34 = false;
                if ((int)FengGameManagerMKII.Settings[226] == 1)
                {
                    flag34 = true;
                }
                bool flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 90f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[226] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[226] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 152f, 160f, 22f), "PVP Bomb Mode:", "Label");
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[192] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 152f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[192] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[192] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 182f, 100f, 66f), "Team Mode:", "Label");
                string[] texts = new string[4]
                {
                            "Off",
                            "No Sort",
                            "Size-Lock",
                            "Skill-Lock"
                };
                FengGameManagerMKII.Settings[193] = GUI.SelectionGrid(new Rect(halfMenuWidth + 250f, halfMenuHeight + 182f, 120f, 88f), (int)FengGameManagerMKII.Settings[193], texts, 1, GUI.skin.toggle);
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 278f, 160f, 22f), "Infection Mode:", "Label");
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 300f, 160f, 22f), "Starting Titans (Integer):", "Label");
                FengGameManagerMKII.Settings[201] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 300f, 50f, 22f), (string)FengGameManagerMKII.Settings[201]);
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[200] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 278f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[200] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[200] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 330f, 160f, 22f), "Friendly Mode:", "Label");
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[219] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 330f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[219] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[219] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 90f, 160f, 22f), "Sword/AHSS PVP:", "Label");
                texts = new string[3]
                {
                            "Off",
                            "Teams",
                            "FFA"
                };
                FengGameManagerMKII.Settings[220] = GUI.SelectionGrid(new Rect(halfMenuWidth + 550f, halfMenuHeight + 90f, 100f, 66f), (int)FengGameManagerMKII.Settings[220], texts, 1, GUI.skin.toggle);
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 164f, 160f, 22f), "No AHSS Air-Reloading:", "Label");
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[228] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 164f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[228] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[228] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 194f, 160f, 22f), "Cannons kill humans:", "Label");
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[261] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 194f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[261] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[261] = 0;
                    }
                }
            }
            else if ((int)FengGameManagerMKII.Settings[230] == 2)
            {
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 90f, 160f, 22f), "Custom Titans/Wave:", "Label");
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 112f, 160f, 22f), "Amount (Integer):", "Label");
                FengGameManagerMKII.Settings[218] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 112f, 50f, 22f), (string)FengGameManagerMKII.Settings[218]);
                bool flag34 = false;
                if ((int)FengGameManagerMKII.Settings[217] == 1)
                {
                    flag34 = true;
                }
                bool flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 90f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[217] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[217] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 152f, 160f, 22f), "Maximum Waves:", "Label");
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 174f, 160f, 22f), "Amount (Integer):", "Label");
                FengGameManagerMKII.Settings[222] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 174f, 50f, 22f), (string)FengGameManagerMKII.Settings[222]);
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[221] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 152f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[221] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[221] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 214f, 160f, 22f), "Punks every 5 waves:", "Label");
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[229] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 214f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[229] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[229] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 244f, 160f, 22f), "Global Minimap Disable:", "Label");
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[235] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 244f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[235] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[235] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 90f, 160f, 22f), "Endless Respawn:", "Label");
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 112f, 160f, 22f), "Respawn Time (Integer):", "Label");
                FengGameManagerMKII.Settings[224] = GUI.TextField(new Rect(halfMenuWidth + 550f, halfMenuHeight + 112f, 50f, 22f), (string)FengGameManagerMKII.Settings[224]);
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[223] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 90f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[223] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[223] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 152f, 160f, 22f), "Kick Eren Titan:", "Label");
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[202] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 152f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[202] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[202] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 182f, 160f, 22f), "Allow Horses:", "Label");
                flag34 = false;
                if ((int)FengGameManagerMKII.Settings[216] == 1)
                {
                    flag34 = true;
                }
                flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 182f, 40f, 20f), flag34, "On");
                if (flag34 != flag35)
                {
                    if (flag35)
                    {
                        FengGameManagerMKII.Settings[216] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[216] = 0;
                    }
                }
                GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 212f, 180f, 22f), "Message of the Day:", "Label");
                FengGameManagerMKII.Settings[225] = GUI.TextArea(new Rect(halfMenuWidth + 400f, halfMenuHeight + 234f, 200f, 100f), (string)FengGameManagerMKII.Settings[225]);
            }
        }
    }
}
