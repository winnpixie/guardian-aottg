using Guardian.UI.Impl;
using Guardian.UI.Impl.Logging;
using UnityEngine;

namespace Guardian.UI
{
    class UIManager : MonoBehaviour
    {
        public UIBase CurrentScreen;

        private UILogger _logUI = new UILogger();

        public void OpenScreen(UIBase screen)
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
            GSkins.InitSkins();

            if (KeyCode.Escape.WasPressedInGUI() && GUI.GetNameOfFocusedControl().Length == 0 && Mod.Menus.CurrentScreen == null)
            {
                Mod.Menus.OpenScreen(new UIModConfiguration());
            }

            if (Mod.Menus.CurrentScreen != null)
            {
                Mod.Menus.CurrentScreen.Draw();
            }

            _logUI.Draw();
        }
    }
}
