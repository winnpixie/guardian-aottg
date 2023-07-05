using Guardian.UI.Components;
using System.Collections.Generic;

namespace Guardian.UI
{
    class Gui
    {
        public readonly List<GComponent> Components = new List<GComponent>();

        public virtual void Draw()
        {
            Components.ForEach(c => c.Tick());
        }

        public virtual void OnOpen() { }

        public virtual void OnClose() { }
    }
}
