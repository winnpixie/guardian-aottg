using Guardian.Features.Properties;
using System.Collections.Generic;
using UnityEngine;

namespace Guardian.UI.Impl
{
    class UIModSettings : UIBase
    {
        private int width = 420;
        private int height = 320;
        private bool save = false;

        private Dictionary<Property, bool> boolDict = new Dictionary<Property, bool>();
        private Dictionary<Property, int> intDict = new Dictionary<Property, int>();
        private Dictionary<Property, float> floatDict = new Dictionary<Property, float>();
        private Dictionary<Property, string> stringDict = new Dictionary<Property, string>();

        private List<string> sections = new List<string>();
        private string currentSection = "MC";

        private Vector2 scrollView = new Vector2(0, 0);

        public override void OnOpen()
        {
            foreach (Property property in Mod.Properties.Elements)
            {
                string section = property.Name.Substr(0, property.Name.IndexOf("_") - 1);
                if (!sections.Contains(section))
                {
                    sections.Add(section);
                }

                if (property.Value is bool)
                {
                    boolDict.Add(property, (bool)property.Value);
                }
                else if (property.Value is int)
                {
                    intDict.Add(property, (int)property.Value);
                }
                else if (property.Value is float)
                {
                    floatDict.Add(property, (float)property.Value);
                }
                else if (property.Value is string)
                {
                    stringDict.Add(property, (string)property.Value);
                }
            }
        }

        public override void Draw()
        {
            GUILayout.BeginArea(new Rect(5, Screen.height - height - 5, width, height), GSkins.Box);
            GUILayout.Label("Guardian Settings", GUILayout.Width(width));
            scrollView = GUILayout.BeginScrollView(scrollView);
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            foreach (string section in sections)
            {
                if (GUILayout.Button(section, GUILayout.Height(25)))
                {
                    currentSection = section;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Label(currentSection);

            foreach (Property property in Mod.Properties.Elements)
            {
                if (property.Name.StartsWith(currentSection))
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(property.Name.Substr(currentSection.Length + 1, property.Name.Length), GUILayout.MaxWidth(width / 2));

                    GUI.SetNextControlName(property.Name);
                    if (property.Value is bool)
                    {
                        bool state = boolDict[property];
                        boolDict[property] = GUILayout.Toggle(boolDict[property], " " + state.ToString(), GUILayout.Width(width / 2));
                    }
                    else if (property.Value is int)
                    {
                        if (int.TryParse(GUILayout.TextField(intDict[property].ToString(), GUILayout.Width(width / 2)), out int val))
                        {
                            intDict[property] = val;
                        }
                    }
                    else if (property.Value is float)
                    {
                        if (float.TryParse(GUILayout.TextField(floatDict[property].ToString(), GUILayout.Width(width / 2)), out float val))
                        {
                            floatDict[property] = val;
                        }
                    }
                    else if (property.Value is string)
                    {
                        stringDict[property] = GUILayout.TextField(stringDict[property], GUILayout.Width(width / 2));
                    }

                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save & Close", GUILayout.Height(25)))
            {
                save = true;
                Mod.UI.OpenScreen(null);
            }

            if (GUILayout.Button("Close (No Saving)", GUILayout.Height(25)))
            {
                Mod.UI.OpenScreen(null);
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        public override void OnClose()
        {
            if (save)
            {
                foreach (KeyValuePair<Property, bool> pair in boolDict)
                {
                    ((Property<bool>)pair.Key).Value = pair.Value;
                }

                foreach (KeyValuePair<Property, int> pair in intDict)
                {
                    ((Property<int>)pair.Key).Value = pair.Value;
                }

                foreach (KeyValuePair<Property, float> pair in floatDict)
                {
                    ((Property<float>)pair.Key).Value = pair.Value;
                }

                foreach (KeyValuePair<Property, string> pair in stringDict)
                {
                    ((Property<string>)pair.Key).Value = pair.Value;
                }

                Mod.Properties.Save();

                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
                {
                    foreach (HERO hero in FengGameManagerMKII.Instance.heroes)
                    {
                        if (hero.myNetWorkName != null)
                        {
                            if (hero.photonView.isMine)
                            {
                                hero.myNetWorkName.GetComponent<UILabel>().alpha = Mod.Properties.OpacityOfOwnName.Value;
                            }
                            else
                            {
                                hero.myNetWorkName.GetComponent<UILabel>().alpha = Mod.Properties.OpacityOfOtherNames.Value;
                            }
                        }
                    }
                }
            }
        }
    }
}
