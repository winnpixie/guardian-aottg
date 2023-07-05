using UnityEngine;

namespace Guardian.UI.Components.Impl
{
    class GLabel : GComponent
    {
        public string Text = string.Empty;

        public GLabel(string text) : base()
        {
            Text = text;
        }

        public GLabel(Rect bounds, string text) : base(bounds)
        {
            Text = text;
        }

        public override void Tick()
        {
            if (base.Relative)
            {
                GUILayout.Label(Text);
                return;
            }

            GUI.Label(base.Bounds, Text);
        }
    }
}
