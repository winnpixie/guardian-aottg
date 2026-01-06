using System.Collections.Generic;
using Guardian.UI.Components;

namespace Guardian.UI
{
    class Gui
    {
        public readonly List<GComponent> Components = new List<GComponent>();

        public virtual void Draw()
        {
            Components.ForEach(c => c.Tick());
        }

        public virtual void OnOpen()
        {
        }

        public virtual void OnClose()
        {
        }
    }
}