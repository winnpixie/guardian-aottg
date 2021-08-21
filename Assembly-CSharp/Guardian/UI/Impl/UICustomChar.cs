using UnityEngine;

namespace Guardian.UI.Impl
{
    class UICustomChar : UIBase
    {
        private string[] _originalCharacters =
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
        private CustomCharacterManager _characterManager;

        public override void Draw()
        {
            if (_characterManager != null)
            {
                if (_characterManager.setup != null)
                {
                    GUILayout.BeginArea(new Rect(Screen.width - 401, Screen.height / 2, 350, 250), GSkins.Box);

                    GUILayout.Label("Select Stat Preset");

                    foreach (string character in _originalCharacters)
                    {
                        HeroStat stat = HeroStat.GetInfo(character);
                        if (stat != null && GUILayout.Button(stat.Name))
                        {
                            _characterManager.SetStatPoint(CreateStat.Speed, stat.Speed);
                            _characterManager.SetStatPoint(CreateStat.Gas, stat.Gas);
                            _characterManager.SetStatPoint(CreateStat.Blades, stat.Blade);
                            _characterManager.SetStatPoint(CreateStat.Acceleration, stat.Accel);
                        }
                    }

                    GUILayout.EndArea();
                }
            }
            else
            {
                _characterManager = UnityEngine.Object.FindObjectOfType<CustomCharacterManager>();
            }
        }
    }
}
