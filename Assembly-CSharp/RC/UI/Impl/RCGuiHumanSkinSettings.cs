using UnityEngine;

namespace RC.UI.Impl
{
    class RCGuiHumanSkinSettings : RCGui
    {
        public override void Draw(FengGameManagerMKII fgm, float halfMenuWidth, float halfMenuHeight)
        {
            GUI.Label(new Rect(halfMenuWidth + 205f, halfMenuHeight + 52f, 120f, 30f), "Human Skin Mode:", "Label");
            bool flag = false;
            if ((int)FengGameManagerMKII.Settings[0] == 1)
            {
                flag = true;
            }
            bool flag4 = GUI.Toggle(new Rect(halfMenuWidth + 325f, halfMenuHeight + 52f, 40f, 20f), flag, "On");
            if (flag != flag4)
            {
                if (flag4)
                {
                    FengGameManagerMKII.Settings[0] = 1;
                }
                else
                {
                    FengGameManagerMKII.Settings[0] = 0;
                }
            }
            float num28 = 44f;
            if ((int)FengGameManagerMKII.Settings[133] == 0)
            {
                if (GUI.Button(new Rect(halfMenuWidth + 375f, halfMenuHeight + 51f, 120f, 22f), "Human Set 1"))
                {
                    FengGameManagerMKII.Settings[133] = 1;
                }
                FengGameManagerMKII.Settings[3] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)FengGameManagerMKII.Settings[3]);
                FengGameManagerMKII.Settings[4] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)FengGameManagerMKII.Settings[4]);
                FengGameManagerMKII.Settings[5] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)FengGameManagerMKII.Settings[5]);
                FengGameManagerMKII.Settings[6] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)FengGameManagerMKII.Settings[6]);
                FengGameManagerMKII.Settings[7] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)FengGameManagerMKII.Settings[7]);
                FengGameManagerMKII.Settings[8] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)FengGameManagerMKII.Settings[8]);
                FengGameManagerMKII.Settings[14] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 6f, 230f, 20f), (string)FengGameManagerMKII.Settings[14]);
                FengGameManagerMKII.Settings[9] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)FengGameManagerMKII.Settings[9]);
                FengGameManagerMKII.Settings[10] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)FengGameManagerMKII.Settings[10]);
                FengGameManagerMKII.Settings[11] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)FengGameManagerMKII.Settings[11]);
                FengGameManagerMKII.Settings[12] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)FengGameManagerMKII.Settings[12]);
                FengGameManagerMKII.Settings[13] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)FengGameManagerMKII.Settings[13]);
                FengGameManagerMKII.Settings[94] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)FengGameManagerMKII.Settings[94]);
            }
            else if ((int)FengGameManagerMKII.Settings[133] == 1)
            {
                if (GUI.Button(new Rect(halfMenuWidth + 375f, halfMenuHeight + 51f, 120f, 22f), "Human Set 2"))
                {
                    FengGameManagerMKII.Settings[133] = 2;
                }
                FengGameManagerMKII.Settings[134] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)FengGameManagerMKII.Settings[134]);
                FengGameManagerMKII.Settings[135] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)FengGameManagerMKII.Settings[135]);
                FengGameManagerMKII.Settings[136] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)FengGameManagerMKII.Settings[136]);
                FengGameManagerMKII.Settings[137] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)FengGameManagerMKII.Settings[137]);
                FengGameManagerMKII.Settings[138] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)FengGameManagerMKII.Settings[138]);
                FengGameManagerMKII.Settings[139] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)FengGameManagerMKII.Settings[139]);
                FengGameManagerMKII.Settings[145] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 6f, 230f, 20f), (string)FengGameManagerMKII.Settings[145]);
                FengGameManagerMKII.Settings[140] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)FengGameManagerMKII.Settings[140]);
                FengGameManagerMKII.Settings[141] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)FengGameManagerMKII.Settings[141]);
                FengGameManagerMKII.Settings[142] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)FengGameManagerMKII.Settings[142]);
                FengGameManagerMKII.Settings[143] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)FengGameManagerMKII.Settings[143]);
                FengGameManagerMKII.Settings[144] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)FengGameManagerMKII.Settings[144]);
                FengGameManagerMKII.Settings[146] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)FengGameManagerMKII.Settings[146]);
            }
            else if ((int)FengGameManagerMKII.Settings[133] == 2)
            {
                if (GUI.Button(new Rect(halfMenuWidth + 375f, halfMenuHeight + 51f, 120f, 22f), "Human Set 3"))
                {
                    FengGameManagerMKII.Settings[133] = 0;
                }
                FengGameManagerMKII.Settings[147] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)FengGameManagerMKII.Settings[147]);
                FengGameManagerMKII.Settings[148] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)FengGameManagerMKII.Settings[148]);
                FengGameManagerMKII.Settings[149] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)FengGameManagerMKII.Settings[149]);
                FengGameManagerMKII.Settings[150] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)FengGameManagerMKII.Settings[150]);
                FengGameManagerMKII.Settings[151] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)FengGameManagerMKII.Settings[151]);
                FengGameManagerMKII.Settings[152] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)FengGameManagerMKII.Settings[152]);
                FengGameManagerMKII.Settings[158] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 6f, 230f, 20f), (string)FengGameManagerMKII.Settings[158]);
                FengGameManagerMKII.Settings[153] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)FengGameManagerMKII.Settings[153]);
                FengGameManagerMKII.Settings[154] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)FengGameManagerMKII.Settings[154]);
                FengGameManagerMKII.Settings[155] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)FengGameManagerMKII.Settings[155]);
                FengGameManagerMKII.Settings[156] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)FengGameManagerMKII.Settings[156]);
                FengGameManagerMKII.Settings[157] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)FengGameManagerMKII.Settings[157]);
                FengGameManagerMKII.Settings[159] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)FengGameManagerMKII.Settings[159]);
            }
            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 0f, 150f, 20f), "Horse:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 1f, 227f, 20f), "Hair (model dependent):", "Label");
            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 2f, 150f, 20f), "Eyes:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 3f, 240f, 20f), "Glass (must have a glass enabled):", "Label");
            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 4f, 150f, 20f), "Face:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 5f, 150f, 20f), "Skin:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 6f, 240f, 20f), "Hoodie (costume dependent):", "Label");
            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 0f, 240f, 20f), "Costume (model dependent):", "Label");
            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 1f, 150f, 20f), "Logo & Cape:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 2f, 240f, 20f), "3DMG Center & 3DMG/Blade/Gun(left):", "Label");
            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 3f, 227f, 20f), "3DMG/Blade/Gun(right):", "Label");
            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 4f, 150f, 20f), "Gas:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 5f, 150f, 20f), "Weapon Trail:", "Label");
        }
    }
}
