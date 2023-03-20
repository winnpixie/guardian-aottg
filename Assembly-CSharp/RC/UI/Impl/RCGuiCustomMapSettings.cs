using UnityEngine;

namespace RC.UI.Impl
{
    class RCGuiCustomMapSettings : RCGui
    {
        public override void Draw(FengGameManagerMKII fgm, float halfMenuWidth, float halfMenuHeight)
        {
            GUI.Label(new Rect(halfMenuWidth + 150f, halfMenuHeight + 51f, 120f, 22f), "Map Settings", "Label");
            GUI.Label(new Rect(halfMenuWidth + 50f, halfMenuHeight + 81f, 140f, 20f), "Titan Spawn Cap:", "Label");
            FengGameManagerMKII.Settings[85] = GUI.TextField(new Rect(halfMenuWidth + 155f, halfMenuHeight + 81f, 30f, 20f), (string)FengGameManagerMKII.Settings[85]);
            string[] texts = new string[5]
            {
                "1 Round",
                "Waves",
                "PVP",
                "Racing",
                "Custom"
            };
            RCSettings.GameType = GUI.SelectionGrid(new Rect(halfMenuWidth + 190f, halfMenuHeight + 80f, 140f, 60f), RCSettings.GameType, texts, 2, GUI.skin.toggle);
            GUI.Label(new Rect(halfMenuWidth + 150f, halfMenuHeight + 155f, 150f, 20f), "Level Script:", "Label");
            FengGameManagerMKII.CurrentScript = GUI.TextField(new Rect(halfMenuWidth + 50f, halfMenuHeight + 180f, 275f, 220f), FengGameManagerMKII.CurrentScript);
            if (GUI.Button(new Rect(halfMenuWidth + 100f, halfMenuHeight + 410f, 50f, 25f), "Copy"))
            {
                TextEditor textEditor = new TextEditor()
                {
                    content = new GUIContent(FengGameManagerMKII.CurrentScript)
                };
                textEditor.SelectAll();
                textEditor.Copy();
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 225f, halfMenuHeight + 410f, 50f, 25f), "Clear"))
            {
                FengGameManagerMKII.CurrentScript = string.Empty;
            }
            GUI.Label(new Rect(halfMenuWidth + 455f, halfMenuHeight + 51f, 180f, 20f), "Custom Textures", "Label");
            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 81f, 180f, 20f), "Ground Skin:", "Label");
            FengGameManagerMKII.Settings[162] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 103f, 275f, 20f), (string)FengGameManagerMKII.Settings[162]);
            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 125f, 150f, 20f), "Skybox Front:", "Label");
            FengGameManagerMKII.Settings[175] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 147f, 275f, 20f), (string)FengGameManagerMKII.Settings[175]);
            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 169f, 150f, 20f), "Skybox Back:", "Label");
            FengGameManagerMKII.Settings[176] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 191f, 275f, 20f), (string)FengGameManagerMKII.Settings[176]);
            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 213f, 150f, 20f), "Skybox Left:", "Label");
            FengGameManagerMKII.Settings[177] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 235f, 275f, 20f), (string)FengGameManagerMKII.Settings[177]);
            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 257f, 150f, 20f), "Skybox Right:", "Label");
            FengGameManagerMKII.Settings[178] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 279f, 275f, 20f), (string)FengGameManagerMKII.Settings[178]);
            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 301f, 150f, 20f), "Skybox Up:", "Label");
            FengGameManagerMKII.Settings[179] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 323f, 275f, 20f), (string)FengGameManagerMKII.Settings[179]);
            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 345f, 150f, 20f), "Skybox Down:", "Label");
            FengGameManagerMKII.Settings[180] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 367f, 275f, 20f), (string)FengGameManagerMKII.Settings[180]);
        }
    }
}
