using UnityEngine;

namespace RC.UI.Impl
{
    class RCGuiTitanSkinSettings : RCGui
    {
        public override void Draw(FengGameManagerMKII fgm, float halfMenuWidth, float halfMenuHeight)
        {
            GUI.Label(new Rect(halfMenuWidth + 270f, halfMenuHeight + 52f, 120f, 30f), "Titan Skin Mode:", "Label");
            bool flag5 = false;
            if ((int)FengGameManagerMKII.Settings[1] == 1)
            {
                flag5 = true;
            }
            bool flag10 = GUI.Toggle(new Rect(halfMenuWidth + 390f, halfMenuHeight + 52f, 40f, 20f), flag5, "On");
            if (flag5 != flag10)
            {
                if (flag10)
                {
                    FengGameManagerMKII.Settings[1] = 1;
                }
                else
                {
                    FengGameManagerMKII.Settings[1] = 0;
                }
            }
            GUI.Label(new Rect(halfMenuWidth + 270f, halfMenuHeight + 77f, 120f, 30f), "Randomized Pairs:", "Label");
            flag5 = false;
            if ((int)FengGameManagerMKII.Settings[32] == 1)
            {
                flag5 = true;
            }
            flag10 = GUI.Toggle(new Rect(halfMenuWidth + 390f, halfMenuHeight + 77f, 40f, 20f), flag5, "On");
            if (flag5 != flag10)
            {
                if (flag10)
                {
                    FengGameManagerMKII.Settings[32] = 1;
                }
                else
                {
                    FengGameManagerMKII.Settings[32] = 0;
                }
            }
            GUI.Label(new Rect(halfMenuWidth + 158f, halfMenuHeight + 112f, 150f, 20f), "Titan Hair:", "Label");
            FengGameManagerMKII.Settings[21] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 134f, 165f, 20f), (string)FengGameManagerMKII.Settings[21]);
            FengGameManagerMKII.Settings[22] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 156f, 165f, 20f), (string)FengGameManagerMKII.Settings[22]);
            FengGameManagerMKII.Settings[23] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 178f, 165f, 20f), (string)FengGameManagerMKII.Settings[23]);
            FengGameManagerMKII.Settings[24] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 200f, 165f, 20f), (string)FengGameManagerMKII.Settings[24]);
            FengGameManagerMKII.Settings[25] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 222f, 165f, 20f), (string)FengGameManagerMKII.Settings[25]);
            if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 134f, 60f, 20f), HairType((int)FengGameManagerMKII.Settings[16])))
            {
                int num29 = 16;
                int num30 = (int)FengGameManagerMKII.Settings[num29];
                num30 = ((num30 < 9) ? (num30 + 1) : (-1));
                FengGameManagerMKII.Settings[num29] = num30;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 156f, 60f, 20f), HairType((int)FengGameManagerMKII.Settings[17])))
            {
                int num29 = 17;
                int num30 = (int)FengGameManagerMKII.Settings[num29];
                num30 = ((num30 < 9) ? (num30 + 1) : (-1));
                FengGameManagerMKII.Settings[num29] = num30;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 178f, 60f, 20f), HairType((int)FengGameManagerMKII.Settings[18])))
            {
                int num29 = 18;
                int num30 = (int)FengGameManagerMKII.Settings[num29];
                num30 = ((num30 < 9) ? (num30 + 1) : (-1));
                FengGameManagerMKII.Settings[num29] = num30;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 200f, 60f, 20f), HairType((int)FengGameManagerMKII.Settings[19])))
            {
                int num29 = 19;
                int num30 = (int)FengGameManagerMKII.Settings[num29];
                num30 = ((num30 < 9) ? (num30 + 1) : (-1));
                FengGameManagerMKII.Settings[num29] = num30;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 222f, 60f, 20f), HairType((int)FengGameManagerMKII.Settings[20])))
            {
                int num29 = 20;
                int num30 = (int)FengGameManagerMKII.Settings[num29];
                num30 = ((num30 < 9) ? (num30 + 1) : (-1));
                FengGameManagerMKII.Settings[num29] = num30;
            }
            GUI.Label(new Rect(halfMenuWidth + 158f, halfMenuHeight + 252f, 150f, 20f), "Titan Eye:", "Label");
            FengGameManagerMKII.Settings[26] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 274f, 230f, 20f), (string)FengGameManagerMKII.Settings[26]);
            FengGameManagerMKII.Settings[27] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 296f, 230f, 20f), (string)FengGameManagerMKII.Settings[27]);
            FengGameManagerMKII.Settings[28] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 318f, 230f, 20f), (string)FengGameManagerMKII.Settings[28]);
            FengGameManagerMKII.Settings[29] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 340f, 230f, 20f), (string)FengGameManagerMKII.Settings[29]);
            FengGameManagerMKII.Settings[30] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 362f, 230f, 20f), (string)FengGameManagerMKII.Settings[30]);
            GUI.Label(new Rect(halfMenuWidth + 455f, halfMenuHeight + 112f, 150f, 20f), "Titan Body:", "Label");
            FengGameManagerMKII.Settings[86] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 134f, 230f, 20f), (string)FengGameManagerMKII.Settings[86]);
            FengGameManagerMKII.Settings[87] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 156f, 230f, 20f), (string)FengGameManagerMKII.Settings[87]);
            FengGameManagerMKII.Settings[88] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 178f, 230f, 20f), (string)FengGameManagerMKII.Settings[88]);
            FengGameManagerMKII.Settings[89] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 200f, 230f, 20f), (string)FengGameManagerMKII.Settings[89]);
            FengGameManagerMKII.Settings[90] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 222f, 230f, 20f), (string)FengGameManagerMKII.Settings[90]);
            GUI.Label(new Rect(halfMenuWidth + 472f, halfMenuHeight + 252f, 150f, 20f), "Eren:", "Label");
            FengGameManagerMKII.Settings[65] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 274f, 230f, 20f), (string)FengGameManagerMKII.Settings[65]);
            GUI.Label(new Rect(halfMenuWidth + 470f, halfMenuHeight + 296f, 150f, 20f), "Annie:", "Label");
            FengGameManagerMKII.Settings[66] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 318f, 230f, 20f), (string)FengGameManagerMKII.Settings[66]);
            GUI.Label(new Rect(halfMenuWidth + 465f, halfMenuHeight + 340f, 150f, 20f), "Colossal:", "Label");
            FengGameManagerMKII.Settings[67] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 362f, 230f, 20f), (string)FengGameManagerMKII.Settings[67]);
        }

        private static string HairType(int type)
        {
            if (type < 0)
            {
                return "Random";
            }
            return "Male " + type;
        }
    }
}
