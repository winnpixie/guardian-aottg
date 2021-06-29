using UnityEngine;
using System.Runtime.InteropServices;

namespace Guardian.UI
{
    class WindowManager
    {
        public static bool IsExclusiveFullscreen;

        private static bool IsFullscreen;

        [DllImport("user32.dll")]
        public static extern int GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern void ShowWindow(int hWnd, int nCmdShow);

        public static void HandleWindowFocusEvent(bool hasFocus)
        {
            if (IsExclusiveFullscreen)
            {
                if (hasFocus)
                {
                    if (IsFullscreen)
                    {
                        IsFullscreen = false;

                        Screen.fullScreen = false;
                        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                    }
                }
                else if (!IsFullscreen)
                {
                    if(Screen.fullScreen)
                    {
                        IsFullscreen = true;
                        Screen.SetResolution(960, 600, false);

                        ShowWindow(GetActiveWindow(), 2); // SW_SHOWMINIMIZED
                    }
                }
            }
        }
    }
}
