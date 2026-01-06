using System;
using UnityEngine;

namespace Guardian.UI.Components.Impl
{
    class GButton : GComponent
    {
        public string Text;
        public Action OnClick;

        public GButton(string text, Action onClick = null)
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
            if (Relative)
            {
                if (GUILayout.Button(Text)) OnClick.Invoke();
                return;
            }

            if (GUI.Button(Bounds, Text)) OnClick.Invoke();
        }
    }
}