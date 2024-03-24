using UnityEngine;

namespace RC.UI.Impl
{
    class RCGuiLevelSkinSettings : RCGui
    {
        public override void Draw(FengGameManagerMKII fgm, float halfMenuWidth, float halfMenuHeight)
        {
            float num28 = 22f;
            GUI.Label(new Rect(halfMenuWidth + 205f, halfMenuHeight + 52f, 145f, 30f), "Level Skin Mode:", "Label");
            bool flag11 = false;
            if ((int)FengGameManagerMKII.Settings[2] == 1)
            {
                flag11 = true;
            }
            bool flag12 = GUI.Toggle(new Rect(halfMenuWidth + 325f, halfMenuHeight + 52f, 40f, 20f), flag11, "On");
            if (flag11 != flag12)
            {
                if (flag12)
                {
                    FengGameManagerMKII.Settings[2] = 1;
                }
                else
                {
                    FengGameManagerMKII.Settings[2] = 0;
                }
            }
            if ((int)FengGameManagerMKII.Settings[188] == 0)
            {
                if (GUI.Button(new Rect(halfMenuWidth + 375f, halfMenuHeight + 51f, 120f, 22f), "Outside Skins"))
                {
                    FengGameManagerMKII.Settings[188] = 1;
                }
                GUI.Label(new Rect(halfMenuWidth + 205f, halfMenuHeight + 77f, 145f, 30f), "Randomized Pairs:", "Label");
                flag11 = false;
                if ((int)FengGameManagerMKII.Settings[50] == 1)
                {
                    flag11 = true;
                }
                flag12 = GUI.Toggle(new Rect(halfMenuWidth + 325f, halfMenuHeight + 77f, 40f, 20f), flag11, "On");
                if (flag11 != flag12)
                {
                    if (flag12)
                    {
                        FengGameManagerMKII.Settings[50] = 1;
                    }
                    else
                    {
                        FengGameManagerMKII.Settings[50] = 0;
                    }
                }
                fgm.scroll = GUI.BeginScrollView(new Rect(halfMenuWidth, halfMenuHeight + 115f, 700f, 340f), fgm.scroll, new Rect(halfMenuWidth, halfMenuHeight + 115f, 700f, 475f));
                GUI.Label(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 0f, 150f, 20f), "Ground:", "Label");
                FengGameManagerMKII.Settings[49] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 1f, 227f, 20f), (string)FengGameManagerMKII.Settings[49]);
                GUI.Label(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 2f, 150f, 20f), "Forest Trunks", "Label");
                FengGameManagerMKII.Settings[33] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 3f, 227f, 20f), (string)FengGameManagerMKII.Settings[33]);
                FengGameManagerMKII.Settings[34] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 4f, 227f, 20f), (string)FengGameManagerMKII.Settings[34]);
                FengGameManagerMKII.Settings[35] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 5f, 227f, 20f), (string)FengGameManagerMKII.Settings[35]);
                FengGameManagerMKII.Settings[36] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 6f, 227f, 20f), (string)FengGameManagerMKII.Settings[36]);
                FengGameManagerMKII.Settings[37] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 7f, 227f, 20f), (string)FengGameManagerMKII.Settings[37]);
                FengGameManagerMKII.Settings[38] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 8f, 227f, 20f), (string)FengGameManagerMKII.Settings[38]);
                FengGameManagerMKII.Settings[39] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 9f, 227f, 20f), (string)FengGameManagerMKII.Settings[39]);
                FengGameManagerMKII.Settings[40] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 10f, 227f, 20f), (string)FengGameManagerMKII.Settings[40]);
                GUI.Label(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 11f, 150f, 20f), "Forest Leaves:", "Label");
                FengGameManagerMKII.Settings[41] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 12f, 227f, 20f), (string)FengGameManagerMKII.Settings[41]);
                FengGameManagerMKII.Settings[42] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 13f, 227f, 20f), (string)FengGameManagerMKII.Settings[42]);
                FengGameManagerMKII.Settings[43] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 14f, 227f, 20f), (string)FengGameManagerMKII.Settings[43]);
                FengGameManagerMKII.Settings[44] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 15f, 227f, 20f), (string)FengGameManagerMKII.Settings[44]);
                FengGameManagerMKII.Settings[45] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 16f, 227f, 20f), (string)FengGameManagerMKII.Settings[45]);
                FengGameManagerMKII.Settings[46] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 17f, 227f, 20f), (string)FengGameManagerMKII.Settings[46]);
                FengGameManagerMKII.Settings[47] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 18f, 227f, 20f), (string)FengGameManagerMKII.Settings[47]);
                FengGameManagerMKII.Settings[48] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 19f, 227f, 20f), (string)FengGameManagerMKII.Settings[48]);
                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 0f, 150f, 20f), "Skybox Front:", "Label");
                FengGameManagerMKII.Settings[163] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 1f, 227f, 20f), (string)FengGameManagerMKII.Settings[163]);
                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 2f, 150f, 20f), "Skybox Back:", "Label");
                FengGameManagerMKII.Settings[164] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 3f, 227f, 20f), (string)FengGameManagerMKII.Settings[164]);
                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 4f, 150f, 20f), "Skybox Left:", "Label");
                FengGameManagerMKII.Settings[165] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 5f, 227f, 20f), (string)FengGameManagerMKII.Settings[165]);
                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 6f, 150f, 20f), "Skybox Right:", "Label");
                FengGameManagerMKII.Settings[166] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 7f, 227f, 20f), (string)FengGameManagerMKII.Settings[166]);
                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 8f, 150f, 20f), "Skybox Up:", "Label");
                FengGameManagerMKII.Settings[167] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 9f, 227f, 20f), (string)FengGameManagerMKII.Settings[167]);
                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 10f, 150f, 20f), "Skybox Down:", "Label");
                FengGameManagerMKII.Settings[168] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 11f, 227f, 20f), (string)FengGameManagerMKII.Settings[168]);
                GUI.EndScrollView();
            }
            else if ((int)FengGameManagerMKII.Settings[188] == 1)
            {
                if (GUI.Button(new Rect(halfMenuWidth + 375f, halfMenuHeight + 51f, 120f, 22f), "City Skins"))
                {
                    FengGameManagerMKII.Settings[188] = 0;
                }
                GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 0f, 150f, 20f), "Ground:", "Label");
                FengGameManagerMKII.Settings[59] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 1f, 230f, 20f), (string)FengGameManagerMKII.Settings[59]);
                GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 2f, 150f, 20f), "Wall:", "Label");
                FengGameManagerMKII.Settings[60] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 3f, 230f, 20f), (string)FengGameManagerMKII.Settings[60]);
                GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 4f, 150f, 20f), "Gate:", "Label");
                FengGameManagerMKII.Settings[61] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 5f, 230f, 20f), (string)FengGameManagerMKII.Settings[61]);
                GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 6f, 150f, 20f), "Houses:", "Label");
                FengGameManagerMKII.Settings[51] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 7f, 230f, 20f), (string)FengGameManagerMKII.Settings[51]);
                FengGameManagerMKII.Settings[52] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 8f, 230f, 20f), (string)FengGameManagerMKII.Settings[52]);
                FengGameManagerMKII.Settings[53] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 9f, 230f, 20f), (string)FengGameManagerMKII.Settings[53]);
                FengGameManagerMKII.Settings[54] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 10f, 230f, 20f), (string)FengGameManagerMKII.Settings[54]);
                FengGameManagerMKII.Settings[55] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 11f, 230f, 20f), (string)FengGameManagerMKII.Settings[55]);
                FengGameManagerMKII.Settings[56] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 12f, 230f, 20f), (string)FengGameManagerMKII.Settings[56]);
                FengGameManagerMKII.Settings[57] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 13f, 230f, 20f), (string)FengGameManagerMKII.Settings[57]);
                FengGameManagerMKII.Settings[58] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 14f, 230f, 20f), (string)FengGameManagerMKII.Settings[58]);
                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 0f, 150f, 20f), "Skybox Front:", "Label");
                FengGameManagerMKII.Settings[169] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 1f, 230f, 20f), (string)FengGameManagerMKII.Settings[169]);
                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 2f, 150f, 20f), "Skybox Back:", "Label");
                FengGameManagerMKII.Settings[170] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 3f, 230f, 20f), (string)FengGameManagerMKII.Settings[170]);
                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 4f, 150f, 20f), "Skybox Left:", "Label");
                FengGameManagerMKII.Settings[171] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 5f, 230f, 20f), (string)FengGameManagerMKII.Settings[171]);
                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 6f, 150f, 20f), "Skybox Right:", "Label");
                FengGameManagerMKII.Settings[172] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 7f, 230f, 20f), (string)FengGameManagerMKII.Settings[172]);
                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 8f, 150f, 20f), "Skybox Up:", "Label");
                FengGameManagerMKII.Settings[173] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 9f, 230f, 20f), (string)FengGameManagerMKII.Settings[173]);
                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 10f, 150f, 20f), "Skybox Down:", "Label");
                FengGameManagerMKII.Settings[174] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 11f, 230f, 20f), (string)FengGameManagerMKII.Settings[174]);
            }
        }
    }
}
