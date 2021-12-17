using UnityEngine;

namespace Guardian.Ui.Impl
{
    class GuiCustomCharacter : Gui
    {
        private string[] OriginalCharacters =
        {
            "MIKASA",
            "LEVI",
            "PETRA",
            "ARMIN",
            "EREN",
            "MARCO",
            "JEAN",
            "SASHA"
        };
        private CustomCharacterManager _CharacterManager;
        private string hairColorRed = "255";
        private string hairColorGreen = "255";
        private string hairColorBlue = "255";

        public override void Draw()
        {
            if (_CharacterManager != null)
            {
                if (_CharacterManager.setup == null) return;

                GUILayout.BeginArea(new Rect((Screen.width / 2) + (Screen.width / 4) - 150, (Screen.height / 2) + (Screen.height / 4) - 150, 300, 300), GuiSkins.Box);
                GUILayout.Label("Extended Configuration");

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Label("Set Stat Preset");

                foreach (string character in OriginalCharacters)
                {
                    HeroStat stat = HeroStat.GetInfo(character);
                    if (stat == null || !GUILayout.Button(stat.Name + " Stats")) continue;

                    _CharacterManager.SetStatPoint(CreateStat.Speed, stat.Speed);
                    _CharacterManager.SetStatPoint(CreateStat.Gas, stat.Gas);
                    _CharacterManager.SetStatPoint(CreateStat.Blades, stat.Blade);
                    _CharacterManager.SetStatPoint(CreateStat.Acceleration, stat.Accel);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("Set Hair R/G/B");

                // Red
                hairColorRed = GUILayout.TextField(hairColorRed);
                if (GUILayout.Button("Set Red Value"))
                {
                    if (int.TryParse(hairColorRed, out int r))
                    {
                        if (r < 0) r = 0;

                        float rf = r / 255f;
                        _CharacterManager.hairR.GetComponent<UISlider>().sliderValue = rf;
                        _CharacterManager.OnHairRChange(rf);
                    }
                }

                // Green
                GUILayout.Label(string.Empty);
                hairColorGreen = GUILayout.TextField(hairColorGreen);
                if (GUILayout.Button("Set Green Value"))
                {
                    if (int.TryParse(hairColorGreen, out int g))
                    {
                        if (g < 0) g = 0;

                        float gf = g / 255f;
                        _CharacterManager.hairG.GetComponent<UISlider>().sliderValue = gf;
                        _CharacterManager.OnHairGChange(gf);
                    }
                }

                // Blue
                GUILayout.Label(string.Empty);
                hairColorBlue = GUILayout.TextField(hairColorBlue);
                if (GUILayout.Button("Set Blue Value"))
                {
                    if (int.TryParse(hairColorBlue, out int b))
                    {
                        if (b < 0) b = 0;

                        float bf = b / 255f;
                        _CharacterManager.hairB.GetComponent<UISlider>().sliderValue = bf;
                        _CharacterManager.OnHairBChange(bf);
                    }
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                GUILayout.EndArea();
            }
            else
            {
                _CharacterManager = UnityEngine.Object.FindObjectOfType<CustomCharacterManager>();
            }
        }
    }
}
