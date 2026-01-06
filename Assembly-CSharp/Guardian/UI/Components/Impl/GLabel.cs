using UnityEngine;

namespace Guardian.UI.Components.Impl
{
    class GLabel : GComponent
    {
        public string Text;

        public GLabel(string text)
        {
            Text = text;
        }

        public GLabel(Rect bounds, string text) : base(bounds)
        {
            Text = text;
        }

        public override void Tick()
        {
            if (Relative)
            {
                GUILayout.Label(Text);
                return;
            }

            GUI.Label(Bounds, Text);
        }
    }
}