using System;
using UnityEngine;

namespace Guardian.UI.Components.Impl
{
    class GButton : GComponent
    {
        public string Text = string.Empty;
        public Action OnClick = () => { };

        public GButton(string text, Action onClick = null) : base()
        {
            Text = text;
            OnClick = onClick;
        }

        public GButton(Rect bounds, string text, Action onClick = null) : base(bounds)
        {
            Text = text;
            OnClick = onClick;
        }

        public override void Tick()
        {
            if (base.Relative)
            {
                if (GUILayout.Button(Text)) OnClick.Invoke();
                return;
            }

            if (GUI.Button(base.Bounds, Text)) OnClick.Invoke();
        }
    }
}
