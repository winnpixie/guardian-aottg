using Guardian.Features.Properties;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Guardian.UI.Impl
{
    class GuiModConfiguration : Gui
    {
        private Regex NumericPattern = new Regex("-?(\\d*\\.?)?\\d+", RegexOptions.IgnoreCase);
        private int Width = 640;
        private int Height = 480;
        private bool ShouldSave = false;
        private Dictionary<Property, bool> TempBoolProps = new Dictionary<Property, bool>();
        private Dictionary<Property, string> TempIntProps = new Dictionary<Property, string>();
        private Dictionary<Property, string> TempFloatProps = new Dictionary<Property, string>();
        private Dictionary<Property, string> TempStringProps = new Dictionary<Property, string>();
        private List<string> Sections = new List<string>();
        private string CurrentSection = "MC";
        private Vector2 ScrollPosition = new Vector2(0, 0);

        public override void OnOpen()
        {
            foreach (Property property in GuardianClient.Properties.Elements)
            {
                string section = property.Name.Substr(0, property.Name.IndexOf("_") - 1);
                if (!Sections.Contains(section))
                {
                    Sections.Add(section);
                }

                if (property.Value is bool)
                {
                    TempBoolProps.Add(property, (bool)property.Value);
                }
                else if (property.Value is int)
                {
                    TempIntProps.Add(property, property.Value.ToString());
                }
                else if (property.Value is float)
                {
                    TempFloatProps.Add(property, property.Value.ToString());
                }
                else if (property.Value is string)
                {
                    TempStringProps.Add(property, (string)property.Value);
                }
            }
        }

        public override void Draw()
        {
            //GUILayout.BeginArea(new Rect(5, Screen.height - Height - 5, Width, Height), GuiSkins.Box);
            GUILayout.BeginArea(new Rect((Screen.width / 2f) - (Width / 2f), (Screen.height / 2f) - (Height / 2f), Width, Height), GuiSkins.Box);
            GUILayout.Label("Mod Configuration", GUILayout.Width(Width));
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            foreach (string section in Sections)
            {
                if (!GUILayout.Button(section, GUILayout.Height(25))) continue;

                CurrentSection = section;
            }
            GUILayout.EndHorizontal();

            GUILayout.Label(CurrentSection.AsBold());

            foreach (Property property in GuardianClient.Properties.Elements)
            {
                if (!property.Name.StartsWith(CurrentSection)) continue;

                GUILayout.BeginHorizontal();
                GUILayout.Label(property.Name.Substr(CurrentSection.Length + 1, property.Name.Length), GUILayout.MaxWidth(Width / 2));

                GUI.SetNextControlName(property.Name);
                if (property.Value is bool)
                {
                    bool state = TempBoolProps[property];
                    TempBoolProps[property] = GUILayout.Toggle(TempBoolProps[property], " " + state.ToString(), GUILayout.Width(Width / 2));
                }
                else if (property.Value is int)
                {
                    string input = GUILayout.TextField(TempIntProps[property].ToString(), GUILayout.Width(Width / 2));
                    if (input.Equals("-"))
                    {
                        input += "0";
                    }

                    if (NumericPattern.IsMatch(input))
                    {
                        TempIntProps[property] = input;
                    }
                }
                else if (property.Value is float)
                {
                    string input = GUILayout.TextField(TempFloatProps[property].ToString(), GUILayout.Width(Width / 2));
                    if (input.Equals("-"))
                    {
                        input += "0";
                    }
                    else if (input.StartsWith("-."))
                    {
                        input = "-0" + input.Substring(1);
                    }

                    if (NumericPattern.IsMatch(input))
                    {
                        TempFloatProps[property] = input;
                    }
                }
                else if (property.Value is string)
                {
                    TempStringProps[property] = GUILayout.TextField(TempStringProps[property], GUILayout.Width(Width / 2));
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            if (GUILayout.Button("Save & Close", GUILayout.Height(25)) || (KeyCode.Escape.IsKeyUp() && GUI.GetNameOfFocusedControl().Length == 0))
            {
                ShouldSave = true;
                GuardianClient.GuiController.OpenScreen(null);
            }

            GUILayout.EndArea();
        }

        public override void OnClose()
        {
            if (!ShouldSave) return;

            foreach (KeyValuePair<Property, bool> pair in TempBoolProps)
            {
                ((Property<bool>)pair.Key).Value = pair.Value;
            }

            foreach (KeyValuePair<Property, string> pair in TempIntProps)
            {
                if (int.TryParse(pair.Value, out int val))
                {
                    ((Property<int>)pair.Key).Value = val;
                }
            }

            foreach (KeyValuePair<Property, string> pair in TempFloatProps)
            {
                if (float.TryParse(pair.Value, out float val))
                {
                    ((Property<float>)pair.Key).Value = val;
                }
            }

            foreach (KeyValuePair<Property, string> pair in TempStringProps)
            {
                ((Property<string>)pair.Key).Value = pair.Value;
            }

            GuardianClient.Properties.Save();
        }
    }
}
