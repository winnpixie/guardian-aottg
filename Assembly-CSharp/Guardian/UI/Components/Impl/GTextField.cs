using UnityEngine;

namespace Guardian.UI.Components.Impl
{
    class GTextField : GComponent
    {
        public string Text = string.Empty;

        public GTextField(string text) : base()
        {
            Text = text;
        }

        public GTextField(Rect bounds, string text) : base(bounds)
        {
            Text = text;
        }

        public override void Tick()
        {
            Text = base.Relative ? GUILayout.TextField(Text) : GUI.TextField(base.Bounds, Text);
        }
    }
}
