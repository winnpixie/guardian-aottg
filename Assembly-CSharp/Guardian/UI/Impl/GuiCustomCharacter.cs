using Guardian.UI.Components.Impl;
using UnityEngine;

namespace Guardian.UI.Impl
{
    class GuiCustomCharacter : Gui
    {
        private readonly string[] _presets =
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
        private CustomCharacterManager _characterManagerInst;

        private readonly GTextField _hairRedField = new GTextField("255");
        private readonly GTextField _hairGreenField = new GTextField("255");
        private readonly GTextField _hairBlueField = new GTextField("255");

        private readonly GButton _setRedBtn = new GButton("Set Red Value");
        private readonly GButton _setGreenBtn = new GButton("Set Green Value");
        private readonly GButton _setBlueBtn = new GButton("Set Blue Value");

        public override void OnOpen()
        {
            _setRedBtn.OnClick = () =>
            {
                if (int.TryParse(_hairRedField.Text, out int r))
                {
                    if (r < 0) r = 0;

                    float rf = r / 255f;
                    _characterManagerInst.hairR.GetComponent<UISlider>().sliderValue = rf;
                    _characterManagerInst.OnHairRChange(rf);
                }
            };

            _setGreenBtn.OnClick = () =>
            {
                if (int.TryParse(_hairGreenField.Text, out int g))
                {
                    if (g < 0) g = 0;

                    float gf = g / 255f;
                    _characterManagerInst.hairG.GetComponent<UISlider>().sliderValue = gf;
                    _characterManagerInst.OnHairRChange(gf);
                }
            };

            _setBlueBtn.OnClick = () =>
            {
                if (int.TryParse(_hairBlueField.Text, out int b))
                {
                    if (b < 0) b = 0;

                    float bf = b / 255f;
                    _characterManagerInst.hairB.GetComponent<UISlider>().sliderValue = bf;
                    _characterManagerInst.OnHairBChange(bf);
                }
            };
        }

        public override void Draw()
        {
            if (_characterManagerInst != null)
            {
                if (_characterManagerInst.setup == null) return;

                GUILayout.BeginArea(new Rect((Screen.width / 2) + (Screen.width / 4) - 150, (Screen.height / 2) + (Screen.height / 4) - 150, 300, 300), GuiSkins.Box);
                GUILayout.Label("Extended Configuration");

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Label("Set Stat Preset");

                foreach (string character in _presets)
                {
                    HeroStat stat = HeroStat.GetInfo(character);
                    if (stat == null || !GUILayout.Button(stat.Name + " Stats")) continue;

                    _characterManagerInst.SetStatPoint(CreateStat.Speed, stat.Speed);
                    _characterManagerInst.SetStatPoint(CreateStat.Gas, stat.Gas);
                    _characterManagerInst.SetStatPoint(CreateStat.Blades, stat.Blade);
                    _characterManagerInst.SetStatPoint(CreateStat.Acceleration, stat.Accel);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("Set Hair R/G/B");

                // Red
                _hairRedField.Tick();
                _setRedBtn.Tick();

                // Green
                GUILayout.Label(string.Empty);
                _hairGreenField.Tick();
                _setGreenBtn.Tick();

                // Blue
                GUILayout.Label(string.Empty);
                _hairBlueField.Tick();
                _setBlueBtn.Tick();

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                GUILayout.EndArea();
            }
            else
            {
                _characterManagerInst = UnityEngine.Object.FindObjectOfType<CustomCharacterManager>();
            }
        }
    }
}
