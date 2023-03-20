using UnityEngine;

namespace RC.UI.Impl
{
    class RCGuiAbilitiesSettings : RCGui
    {
        public override void Draw(FengGameManagerMKII fgm, float halfMenuWidth, float halfMenuHeight)
        {
            GUI.Label(new Rect(halfMenuWidth + 150f, halfMenuHeight + 80f, 185f, 22f), "Bomb Mode", "Label");
            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 110f, 80f, 22f), "Color:", "Label");
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
            texture2D.SetPixel(0, 0, new Color((float)FengGameManagerMKII.Settings[246], (float)FengGameManagerMKII.Settings[247], (float)FengGameManagerMKII.Settings[248], (float)FengGameManagerMKII.Settings[249]));
            texture2D.Apply();
            GUI.DrawTexture(new Rect(halfMenuWidth + 120f, halfMenuHeight + 113f, 40f, 15f), texture2D, ScaleMode.StretchToFill);
            UnityEngine.Object.Destroy(texture2D);

            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 135f, 20f, 22f), "R:", "Label");
            FengGameManagerMKII.Settings[246] = GUI.HorizontalSlider(new Rect(halfMenuWidth + 92f, halfMenuHeight + 138f, 100f, 20f), (float)FengGameManagerMKII.Settings[246], 0f, 1f);
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 160f, 20f, 22f), "G:", "Label");
            FengGameManagerMKII.Settings[247] = GUI.HorizontalSlider(new Rect(halfMenuWidth + 92f, halfMenuHeight + 163f, 100f, 20f), (float)FengGameManagerMKII.Settings[247], 0f, 1f);
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 185f, 20f, 22f), "B:", "Label");
            FengGameManagerMKII.Settings[248] = GUI.HorizontalSlider(new Rect(halfMenuWidth + 92f, halfMenuHeight + 188f, 100f, 20f), (float)FengGameManagerMKII.Settings[248], 0f, 1f);
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 210f, 20f, 22f), "A:", "Label");
            FengGameManagerMKII.Settings[249] = GUI.HorizontalSlider(new Rect(halfMenuWidth + 92f, halfMenuHeight + 213f, 100f, 20f), (float)FengGameManagerMKII.Settings[249], 0.5f, 1f);

            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 235f, 95f, 22f), "Bomb Radius:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 168f, halfMenuHeight + 235f, 20f, 22f), ((float)FengGameManagerMKII.Settings[250]).ToString(), "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 260f, 95f, 22f), "Bomb Range:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 168f, halfMenuHeight + 260f, 20f, 22f), ((float)FengGameManagerMKII.Settings[251]).ToString(), "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 285f, 95f, 22f), "Bomb Speed:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 168f, halfMenuHeight + 285f, 20f, 22f), ((float)FengGameManagerMKII.Settings[252]).ToString(), "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 310f, 95f, 22f), "Bomb CD:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 168f, halfMenuHeight + 310f, 20f, 22f), ((float)FengGameManagerMKII.Settings[253]).ToString(), "Label");

            float unusedBombPoints = 20 - (float)FengGameManagerMKII.Settings[250] - (float)FengGameManagerMKII.Settings[251] - (float)FengGameManagerMKII.Settings[252] - (float)FengGameManagerMKII.Settings[253];
            /* Syal's bomb stat limitations
             * Range <= 3
             * CD >= 4
             */
            // BOMB RAD
            float oldRad = (float)FengGameManagerMKII.Settings[250];
            FengGameManagerMKII.Settings[250] = (float)Guardian.Utilities.MathHelper.Floor(GUI.HorizontalSlider(new Rect(halfMenuWidth + 190f, halfMenuHeight + 235f, 100f, 20f), (float)FengGameManagerMKII.Settings[250], 0f, 10f));
            if ((float)FengGameManagerMKII.Settings[250] - oldRad > unusedBombPoints)
            {
                FengGameManagerMKII.Settings[250] = oldRad;
            }

            // BOMB RANGE
            float oldRange = (float)FengGameManagerMKII.Settings[251];
            FengGameManagerMKII.Settings[251] = (float)Guardian.Utilities.MathHelper.Floor(GUI.HorizontalSlider(new Rect(halfMenuWidth + 190f, halfMenuHeight + 260f, 100f, 20f), (float)FengGameManagerMKII.Settings[251], 0f, 3f));
            if ((float)FengGameManagerMKII.Settings[251] - oldRange > unusedBombPoints)
            {
                FengGameManagerMKII.Settings[251] = oldRange;
            }

            // BOMB SPEED
            float oldSpd = (float)FengGameManagerMKII.Settings[252];
            FengGameManagerMKII.Settings[252] = (float)Guardian.Utilities.MathHelper.Floor(GUI.HorizontalSlider(new Rect(halfMenuWidth + 190f, halfMenuHeight + 285f, 100f, 20f), (float)FengGameManagerMKII.Settings[252], 0f, 10f));
            if ((float)FengGameManagerMKII.Settings[252] - oldSpd > unusedBombPoints)
            {
                FengGameManagerMKII.Settings[252] = oldSpd;
            }

            // BOMB CD
            float oldCd = (float)FengGameManagerMKII.Settings[253];
            FengGameManagerMKII.Settings[253] = (float)Guardian.Utilities.MathHelper.Floor(GUI.HorizontalSlider(new Rect(halfMenuWidth + 190f, halfMenuHeight + 310f, 100f, 20f), (float)FengGameManagerMKII.Settings[253], 4f, 10f));
            if ((float)FengGameManagerMKII.Settings[253] - oldCd > unusedBombPoints)
            {
                FengGameManagerMKII.Settings[253] = oldCd;
            }

            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 335f, 95f, 22f), "Unused Points:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 168f, halfMenuHeight + 335f, 20f, 22f), unusedBombPoints.ToString(), "Label");

            // Reset Button
            if (GUI.Button(new Rect(halfMenuWidth + 72f, halfMenuHeight + 360f, 123f, 20f), "Reset Points"))
            {
                FengGameManagerMKII.Settings[250] = 0f;
                FengGameManagerMKII.Settings[251] = 0f;
                FengGameManagerMKII.Settings[252] = 0f;
                FengGameManagerMKII.Settings[253] = 4f;
            }

            // Reset Button
            if (GUI.Button(new Rect(halfMenuWidth + 200f, halfMenuHeight + 360f, 123f, 20f), "Randomize Points"))
            {
                float maxPoints = 20;

                float bombRad;
                float bombRange;
                float bombSpd;
                float bombCd;

                maxPoints -= bombCd = 4 + Mathf.FloorToInt(((float)Guardian.Utilities.MathHelper.Random()) * 7); // CD 4 - 10
                maxPoints -= bombRad = Mathf.FloorToInt(((float)Guardian.Utilities.MathHelper.Random()) * (Mathf.Min(10, maxPoints) + 1)); // RAD 0 - 10 (or maxPoints)
                maxPoints -= bombRange = Mathf.FloorToInt(((float)Guardian.Utilities.MathHelper.Random()) * (Mathf.Min(3, maxPoints) + 1)); // RNG 0 - 3 (or maxPoints)
                bombSpd = Mathf.FloorToInt(((float)Guardian.Utilities.MathHelper.Random()) * (Mathf.Min(10, maxPoints) + 1)); // SPD 0 - 10 (or maxPoints)

                FengGameManagerMKII.Settings[250] = bombRad;
                FengGameManagerMKII.Settings[251] = bombRange;
                FengGameManagerMKII.Settings[252] = bombSpd;
                FengGameManagerMKII.Settings[253] = bombCd;
            }
        }
    }
}
