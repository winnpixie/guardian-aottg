using UnityEngine;

namespace Guardian.UI.Components.Impl
{
    class GTextField : GComponent
    {
        public string Text;

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
            Text = Relative ? GUILayout.TextField(Text) : GUI.TextField(Bounds, Text);
        }
    }
}