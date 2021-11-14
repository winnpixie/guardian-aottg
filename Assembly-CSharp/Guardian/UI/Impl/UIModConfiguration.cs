using Guardian.Features.Properties;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Guardian.UI.Impl
{
    class UIModConfiguration : UIBase
    {
        private Regex NumericPattern = new Regex("-?([0-9]*\\.?)?[0-9]+", RegexOptions.IgnoreCase);
        private int _width = 440;
        private int _height = 320;
        private bool _save = false;

        private Dictionary<Property, bool> _boolDict = new Dictionary<Property, bool>();
        private Dictionary<Property, int> _intDict = new Dictionary<Property, int>();
        private Dictionary<Property, float> _floatDict = new Dictionary<Property, float>();
        private Dictionary<Property, string> _stringDict = new Dictionary<Property, string>();

        private List<string> _sections = new List<string>();
        private string _currentSection = "MC";

        private Vector2 _scrollPos = new Vector2(0, 0);

        public override void OnOpen()
        {
            foreach (Property property in Mod.Properties.Elements)
            {
                string section = property.Name.Substr(0, property.Name.IndexOf("_") - 1);
                if (!_sections.Contains(section))
                {
                    _sections.Add(section);
                }

                if (property.Value is bool)
                {
                    _boolDict.Add(property, (bool)property.Value);
                }
                else if (property.Value is int)
                {
                    _intDict.Add(property, (int)property.Value);
                }
                else if (property.Value is float)
                {
                    _floatDict.Add(property, (float)property.Value);
                }
                else if (property.Value is string)
                {
                    _stringDict.Add(property, (string)property.Value);
                }
            }
        }

        public override void Draw()
        {
            GUILayout.BeginArea(new Rect(5, Screen.height - _height - 5, _width, _height), GSkins.Box);
            GUILayout.Label("Mod Configuration", GUILayout.Width(_width));
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            foreach (string section in _sections)
            {
                if (GUILayout.Button(section, GUILayout.Height(25)))
                {
                    _currentSection = section;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Label(_currentSection.AsBold());

            foreach (Property property in Mod.Properties.Elements)
            {
                if (property.Name.StartsWith(_currentSection))
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(property.Name.Substr(_currentSection.Length + 1, property.Name.Length), GUILayout.MaxWidth(_width / 2));

                    GUI.SetNextControlName(property.Name);
                    if (property.Value is bool)
                    {
                        bool state = _boolDict[property];
                        _boolDict[property] = GUILayout.Toggle(_boolDict[property], " " + state.ToString(), GUILayout.Width(_width / 2));
                    }
                    else if (property.Value is int)
                    {
                        string input = GUILayout.TextField(_intDict[property].ToString(), GUILayout.Width(_width / 2));
                        if (NumericPattern.IsMatch(input))
                        {
                            if (input.Equals("-"))
                            {
                                input += "0";
                            }

                            if (int.TryParse(input, out int val))
                            {
                                _intDict[property] = val;
                            }
                        }
                    }
                    else if (property.Value is float)
                    {
                        string input = GUILayout.TextField(_floatDict[property].ToString(), GUILayout.Width(_width / 2));
                        if (NumericPattern.IsMatch(input))
                        {
                            if (input.StartsWith("."))
                            {
                                input = "0" + input;
                            }
                            else if (input.StartsWith("-."))
                            {
                                input = "-0" + input.Substring(1);
                            }

                            if (float.TryParse(input, out float val))
                            {
                                _floatDict[property] = val;
                            }
                        }
                    }
                    else if (property.Value is string)
                    {
                        _stringDict[property] = GUILayout.TextField(_stringDict[property], GUILayout.Width(_width / 2));
                    }

                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            if (GUILayout.Button("Save & Close", GUILayout.Height(25)) || (KeyCode.Escape.WasKeyDownInGUI() && GUI.GetNameOfFocusedControl().Length == 0))
            {
                _save = true;
                Mod.Menus.OpenScreen(null);
            }

            GUILayout.EndArea();
        }

        public override void OnClose()
        {
            if (_save)
            {
                foreach (KeyValuePair<Property, bool> pair in _boolDict)
                {
                    ((Property<bool>)pair.Key).Value = pair.Value;
                }

                foreach (KeyValuePair<Property, int> pair in _intDict)
                {
                    ((Property<int>)pair.Key).Value = pair.Value;
                }

                foreach (KeyValuePair<Property, float> pair in _floatDict)
                {
                    ((Property<float>)pair.Key).Value = pair.Value;
                }

                foreach (KeyValuePair<Property, string> pair in _stringDict)
                {
                    ((Property<string>)pair.Key).Value = pair.Value;
                }

                Mod.Properties.Save();
            }
        }
    }
}
