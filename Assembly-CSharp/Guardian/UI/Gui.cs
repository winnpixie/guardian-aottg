using System.Collections.Generic;
using Guardian.UI.Components;

namespace Guardian.UI
{
    class Gui
    {
        public readonly List<GComponent> Components = new List<GComponent>();

        public virtual void Draw()
        {
            foreach (GComponent component in Components)
            {
                component.Tick();
            }
        }

        public virtual void OnOpen()
        {
        }

        public virtual void OnClose()
        {
        }
    }
}