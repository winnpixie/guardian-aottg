using UnityEngine;

namespace Guardian.UI.Components.Impl
{
    class GCheckBox : GComponent
    {
        public string Text;
        public bool Selected;

        public GCheckBox(string text, bool selected = false)
        {
            this.Text = text;
            this.Selected = selected;
        }

        public GCheckBox(Rect bounds, string text, bool selected = false) : base(bounds)
        {
            this.Text = text;
            this.Selected = selected;
        }

        public override void Tick()
        {
            Selected = base.Relative ? GUILayout.Toggle(Selected, Text) : GUI.Toggle(base.Bounds, Selected, Text);
        }
    }
}