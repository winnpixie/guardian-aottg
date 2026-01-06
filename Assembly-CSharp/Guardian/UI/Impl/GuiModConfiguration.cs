using System.Collections.Generic;
using System.Text.RegularExpressions;
using Guardian.Features.Properties;
using UnityEngine;

namespace Guardian.UI.Impl
{
    class GuiModConfiguration : Gui
    {
        private readonly Regex _numericPattern = new Regex("[\\-\\.0-9]", RegexOptions.IgnoreCase);
        private readonly int _width = 480;
        private readonly int _height = 320;
        private readonly Dictionary<Property, bool> _tempBools = new Dictionary<Property, bool>();
        private readonly Dictionary<Property, string> _tempInts = new Dictionary<Property, string>();
        private readonly Dictionary<Property, string> _tempFloats = new Dictionary<Property, string>();
        private readonly Dictionary<Property, string> _tempStrings = new Dictionary<Property, string>();
        private readonly  List<string> _sections = new List<string>();
        
        private bool _shouldSave;
        private string _currentSection = "MC";
        private Vector2 _scrollPosition = new Vector2(0, 0);

        public override void OnOpen()
        {
            foreach (Property property in GuardianClient.Properties.Elements)
            {
                string section = property.Name.Substr(0, property.Name.IndexOf("_") - 1);
                if (!_sections.Contains(section))
                {
                    _sections.Add(section);
                }

                if (property.Value is bool boolValue)
                {
                    _tempBools.Add(property, boolValue);
                }
                else if (property.Value is int)
                {
                    _tempInts.Add(property, property.Value.ToString());
                }
                else if (property.Value is float)
                {
                    _tempFloats.Add(property, property.Value.ToString());
                }
                else if (property.Value is string strValue)
                {
                    _tempStrings.Add(property, strValue);
                }
            }
        }

        public override void Draw()
        {
            GUILayout.BeginArea(new Rect(5, Screen.height - _height - 5, _width, _height), GuiSkins.Box);
            //GUILayout.BeginArea(new Rect((Screen.width / 2f) - (Width / 2f), (Screen.height / 2f) - (Height / 2f), Width, Height), GuiSkins.Box);
            GUILayout.Label("Mod Configuration", GUILayout.Width(_width));
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            foreach (string section in _sections)
            {
                if (!GUILayout.Button(section, GUILayout.Height(25))) continue;

                _currentSection = section;
            }
            GUILayout.EndHorizontal();

            GUILayout.Label(_currentSection.AsBold());

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            foreach (Property property in GuardianClient.Properties.Elements)
            {
                if (!property.Name.StartsWith(_currentSection)) continue;

                GUILayout.BeginHorizontal();
                GUILayout.Label(property.Name.Substr(_currentSection.Length + 1, property.Name.Length), GUILayout.MaxWidth(_width / 2f));

                GUI.SetNextControlName(property.Name);
                if (property.Value is bool)
                {
                    bool state = _tempBools[property];
                    _tempBools[property] = GUILayout.Toggle(_tempBools[property], $" {state}", GUILayout.Width(_width / 2f));
                }
                else if (property.Value is int)
                {
                    string input = GUILayout.TextField(_tempInts[property], GUILayout.Width(_width / 2f));
                    if (input.Equals("-"))
                    {
                        input += "0";
                    }

                    if (_numericPattern.IsMatch(input))
                    {
                        _tempInts[property] = input;
                    }
                }
                else if (property.Value is float)
                {
                    string input = GUILayout.TextField(_tempFloats[property], GUILayout.Width(_width / 2f));
                    if (input.Equals("-"))
                    {
                        input += "0";
                    }
                    else if (input.StartsWith("-."))
                    {
                        input = "-0" + input.Substring(1);
                    }

                    if (_numericPattern.IsMatch(input))
                    {
                        _tempFloats[property] = input;
                    }
                }
                else if (property.Value is string)
                {
                    _tempStrings[property] = GUILayout.TextField(_tempStrings[property], GUILayout.Width(_width / 2f));
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            if (GUILayout.Button("Save & Close", GUILayout.Height(25)) || (KeyCode.Escape.IsKeyUp() && GUI.GetNameOfFocusedControl().Length == 0))
            {
                _shouldSave = true;
                GuardianClient.GuiController.OpenScreen(null);
            }

            GUILayout.EndArea();
        }

        public override void OnClose()
        {
            if (!_shouldSave) return;

            foreach (KeyValuePair<Property, bool> pair in _tempBools)
            {
                ((Property<bool>)pair.Key).Value = pair.Value;
            }

            foreach (KeyValuePair<Property, string> pair in _tempInts)
            {
                if (int.TryParse(pair.Value, out int val))
                {
                    ((Property<int>)pair.Key).Value = val;
                }
            }

            foreach (KeyValuePair<Property, string> pair in _tempFloats)
            {
                if (float.TryParse(pair.Value, out float val))
                {
                    ((Property<float>)pair.Key).Value = val;
                }
            }

            foreach (KeyValuePair<Property, string> pair in _tempStrings)
            {
                ((Property<string>)pair.Key).Value = pair.Value;
            }

            GuardianClient.Properties.Save();
        }
    }
}
