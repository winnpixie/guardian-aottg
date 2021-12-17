using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Guardian.Ui
{
    class WindowManager
    {
        private static bool IsFullscreen;

        [DllImport("user32.dll")]
        public static extern int GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern void ShowWindow(int hWnd, int nCmdShow);

        public static void HandleWindowFocusEvent(bool hasFocus)
        {
            // FIXME: Exclusive Fullscreen requires more testing before being properly implemented again.
            /*
            if (hasFocus)
            {
                if (s_isFullscreen)
                {
                    s_isFullscreen = false;

                    ShowWindow(GetActiveWindow(), 1); // SW_SHOWNORMAL

                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

                    GameObject mainCam = GameObject.Find("MainCamera");
                    if (mainCam != null)
                    {
                        IN_GAME_MAIN_CAMERA mainCamera = mainCam.GetComponent<IN_GAME_MAIN_CAMERA>();
                        mainCamera.StartCoroutine(CoMarkHudDirty(mainCamera));
                    }
                }
            }
            else if (!s_isFullscreen)
            {
                if (Screen.fullScreen)
                {
                    s_isFullscreen = true;
                    Screen.SetResolution(960, 600, false);

                    ShowWindow(GetActiveWindow(), 2); // SW_SHOWMINIMIZED
                }
            }*/
        }

        private static IEnumerator CoMarkHudDirty(IN_GAME_MAIN_CAMERA mainCamera)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            mainCamera.needSetHUD = true;
        }
    }
}
