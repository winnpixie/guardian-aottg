using UnityEngine;

namespace Guardian.UI.Components
{
    abstract class GComponent
    {
        public Rect Bounds;
        public readonly bool Relative;
        public GUIStyle Style = GUIStyle.none;

        public GComponent()
        {
            Relative = true;
        }

        public GComponent(Rect bounds)
        {
            Relative = false;
            Bounds = bounds;
        }

        public abstract void Tick();
    }
}
