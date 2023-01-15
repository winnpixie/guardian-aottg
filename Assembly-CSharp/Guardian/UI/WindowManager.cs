using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Guardian.UI
{
    class WindowManager
    {
        private static bool IsFullscreen;
        private static bool UseExclusiveFullscreen = false; // Force no until I fix this

        [DllImport("user32.dll")]
        public static extern int GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern int GetFocus();

        [DllImport("user32.dll")]
        public static extern int GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetWindowTextW")]
        public static extern bool SetWindowTitle(int hWnd, string lpString);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(int hWnd, int nCmdShow);

        public static void HandleWindowFocusEvent(bool hasFocus)
        {
            if (hasFocus)
            {
                Application.targetFrameRate = -1;
                if (int.TryParse((string)FengGameManagerMKII.Settings[184], out int targetFps) && targetFps > 0)
                {
                    Application.targetFrameRate = targetFps;
                }
            }
            else if (GuardianClient.Properties.LimitUnfocusedFPS.Value && GuardianClient.Properties.MaxUnfocusedFPS.Value > 0)
            {
                Application.targetFrameRate = GuardianClient.Properties.MaxUnfocusedFPS.Value;
            }

            // FIXME: Exclusive Fullscreen requires more testing before being properly implemented again.
            if (UseExclusiveFullscreen)
            {
                if (hasFocus)
                {
                    if (IsFullscreen)
                    {
                        IsFullscreen = false;

                        ShowWindow(GetActiveWindow(), 9); // SW_RESTORE

                        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

                        GameObject mainCam = GameObject.Find("MainCamera");
                        if (mainCam != null)
                        {
                            IN_GAME_MAIN_CAMERA mainCamera = mainCam.GetComponent<IN_GAME_MAIN_CAMERA>();
                            mainCamera.StartCoroutine(CoMarkHudDirty(mainCamera));
                        }
                    }
                }
                else if (!IsFullscreen)
                {
                    if (Screen.fullScreen)
                    {
                        IsFullscreen = true;
                        Screen.SetResolution(960, 600, false);

                        ShowWindow(GetActiveWindow(), 2); // SW_SHOWMINIMIZED
                    }
                }
            }
        }

        private static IEnumerator CoMarkHudDirty(IN_GAME_MAIN_CAMERA mainCamera)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            mainCamera.needSetHUD = true;
        }
    }
}
