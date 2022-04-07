using Guardian.Ui.Impl;
using Guardian.Ui.Impl.Logging;
using UnityEngine;

namespace Guardian.Ui
{
    class GuiController : MonoBehaviour
    {
        public Gui CurrentScreen;

        private GuiDebug DebugScreen = new GuiDebug();

        public void OpenScreen(Gui screen)
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.OnClose();
            }

            CurrentScreen = screen;

            if (CurrentScreen != null)
            {
                CurrentScreen.OnOpen();
            }
        }

        void OnGUI()
        {
            GuiSkins.InitSkins();

            if (KeyCode.Escape.IsKeyUp() && GUI.GetNameOfFocusedControl().Length == 0 && CurrentScreen == null)
            {
                OpenScreen(new GuiModConfiguration());
            }

            if (CurrentScreen != null)
            {
                CurrentScreen.Draw();
            }

            DebugScreen.Draw();
        }
    }
}
