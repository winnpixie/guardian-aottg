using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Guardian.UI
{
    class WindowManager
    {
        private static bool Fullscreen;

        public static int WindowWidth = 960;
        public static int WindowHeight = 600;

        public static int ScreenWidth;
        public static int ScreenHeight;

        [DllImport("user32.dll")]
        public static extern int GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow([In] int hWnd, [In] int nCmdShow);

        public struct RECT
        {
            long left;
            long top;
            long right;
            long bottom;
        }

        [DllImport("user32.dll")]
        public static extern bool ClipCursor([In, Optional] ref RECT rect);

        [DllImport("user32.dll")]
        public static extern bool ClipCursor();

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect([In] int hWnd, [Out] out RECT rect);

        public static bool RestrictCursor()
        {
            return GetWindowRect(GetActiveWindow(), out RECT rect) && ClipCursor(ref rect);
        }

        public static bool UnlockCursor()
        {
            return ClipCursor();
        }

        public static void SetCursorStates(bool shown, bool locked)
        {
            Screen.showCursor = shown;
            Screen.lockCursor = locked;

            if (!locked)
            {
                RestrictCursor();
            }
        }

        public static void Init()
        {
            if (Screen.fullScreen)
            {
                WindowWidth = 960;
                WindowHeight = 600;

                ScreenWidth = Screen.width;
                ScreenHeight = Screen.height;
            }
            else
            {
                WindowWidth = Screen.width;
                WindowHeight = Screen.height;

                ScreenWidth = Screen.currentResolution.width;
                ScreenHeight = Screen.currentResolution.height;
            }
        }

        public static void ToggleFullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
            if (Screen.fullScreen)
            {
                Screen.SetResolution(WindowWidth, WindowHeight, fullscreen: false);
            }
            else
            {
                WindowWidth = Mathf.Max(640, Screen.width);
                WindowHeight = Mathf.Max(480, Screen.height);

                Screen.SetResolution(ScreenWidth, ScreenHeight, fullscreen: true);
            }
        }

        public static void HandleWindowFocusEvent(bool hasFocus)
        {
            // TODO: I believe Exclusive Fullscreen still requires more testing.
            if (hasFocus)
            {
                if (Fullscreen)
                {
                    Fullscreen = false;
                    Screen.SetResolution(ScreenWidth, ScreenHeight, true);

                    GameObject mainCam = GameObject.Find("MainCamera");
                    if (mainCam != null)
                    {
                        IN_GAME_MAIN_CAMERA mainCamera = mainCam.GetComponent<IN_GAME_MAIN_CAMERA>();
                        mainCamera.StartCoroutine(MarkHudDirty(mainCamera));
                    }
                }
            }
            else if (!Fullscreen)
            {
                if (Screen.fullScreen)
                {
                    ScreenWidth = Screen.width;
                    ScreenHeight = Screen.height;

                    Fullscreen = true;
                    Screen.SetResolution(960, 600, false);

                    ShowWindow(GetActiveWindow(), 2); // SW_SHOWMINIMIZED
                }
            }

            if (hasFocus)
            {
                RestrictCursor();

                Application.targetFrameRate = -1;
                if (int.TryParse((string)FengGameManagerMKII.Settings[184], out int targetFps) && targetFps > 0)
                {
                    Application.targetFrameRate = targetFps;
                }
            }
            else
            {
                UnlockCursor();

                if (GuardianClient.Properties.LimitUnfocusedFPS.Value && GuardianClient.Properties.MaxUnfocusedFPS.Value > 0)
                {
                    // FPS under ~15-30 as MasterClient provides a borderline non-playable experience to others, this should prevent that.
                    Application.targetFrameRate = Utilities.MathHelper.MaxInt(GuardianClient.Properties.MaxUnfocusedFPS.Value, PhotonNetwork.isMasterClient ? 51 : 1);
                }
            }
        }

        private static IEnumerator MarkHudDirty(IN_GAME_MAIN_CAMERA mainCamera)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            mainCamera.needSetHUD = true;
        }
    }
}
