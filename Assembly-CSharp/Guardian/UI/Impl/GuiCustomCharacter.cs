using Guardian.UI.Components.Impl;
using UnityEngine;

namespace Guardian.UI.Impl
{
    class GuiCustomCharacter : Gui
    {
        private readonly string[] OriginalCharacters =
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

        private readonly GTextField HairRedField = new GTextField("255");
        private readonly GTextField HairGreenField = new GTextField("255");
        private readonly GTextField HairBlueField = new GTextField("255");

        private readonly GButton SetRedBtn = new GButton("Set Red Value");
        private readonly GButton SetGreenBtn = new GButton("Set Green Value");
        private readonly GButton SetBlueBtn = new GButton("Set Blue Value");

        public override void OnOpen()
        {
            SetRedBtn.OnClick = () =>
            {
                if (int.TryParse(HairRedField.Text, out int r))
                {
                    if (r < 0) r = 0;

                    float rf = r / 255f;
                    _CharacterManager.hairR.GetComponent<UISlider>().sliderValue = rf;
                    _CharacterManager.OnHairRChange(rf);
                }
            };

            SetGreenBtn.OnClick = () =>
            {
                if (int.TryParse(HairGreenField.Text, out int g))
                {
                    if (g < 0) g = 0;

                    float gf = g / 255f;
                    _CharacterManager.hairG.GetComponent<UISlider>().sliderValue = gf;
                    _CharacterManager.OnHairRChange(gf);
                }
            };

            SetBlueBtn.OnClick = () =>
            {
                if (int.TryParse(HairBlueField.Text, out int b))
                {
                    if (b < 0) b = 0;

                    float bf = b / 255f;
                    _CharacterManager.hairB.GetComponent<UISlider>().sliderValue = bf;
                    _CharacterManager.OnHairBChange(bf);
                }
            };
        }

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
                HairRedField.Tick();
                SetRedBtn.Tick();

                // Green
                GUILayout.Label(string.Empty);
                HairGreenField.Tick();
                SetGreenBtn.Tick();

                // Blue
                GUILayout.Label(string.Empty);
                HairBlueField.Tick();
                SetBlueBtn.Tick();

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
