using System;
using System.Collections;
using System.Runtime.InteropServices;
using Guardian.Utilities;
using UnityEngine;

namespace Guardian.UI
{
    class WindowManager
    {
        private static bool _fullScreen;

        public static int WindowWidth = 1280;
        public static int WindowHeight = 720;

        public static int ScreenWidth;
        public static int ScreenHeight;

        [DllImport("user32.dll")]
        public static extern int GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow([In] int hWnd, [In] int nCmdShow);

        public struct RECT
        {
            long left, top, right, bottom;
        }

        [DllImport("user32.dll")]
        public static extern bool ClipCursor([In, Optional] ref RECT rect);

        [DllImport("user32.dll")]
        public static extern bool ClipCursor();

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect([In] int hWnd, [Out] out RECT rect);

        public static bool HackCursor(bool restrain)
        {
            if (restrain)
            {
                return GetWindowRect(GetActiveWindow(), out RECT rect) && ClipCursor(ref rect);
            }

            return ClipCursor();
        }

        public static void SetCursorStates(bool shown, bool locked)
        {
            Screen.showCursor = shown;
            Screen.lockCursor = locked;

            if (!locked)
            {
                HackCursor(GuardianClient.Properties.RestrainCursor.Value);
            }
        }

        public static void Init()
        {
            if (Screen.fullScreen)
            {
                WindowWidth = 1280;
                WindowHeight = 720;

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

            string[] cliArgs = Environment.GetCommandLineArgs();
            for (int i = 1; i < cliArgs.Length; i++)
            {
                if (!cliArgs[i].Equals("-screen-fullscreen")) continue;
                if (i + 1 == cliArgs.Length) continue;

                bool fullScreen = cliArgs[i + 1].Equals("1");
                if (Screen.fullScreen != fullScreen) ToggleFullscreen();
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

        // TODO: I believe Exclusive Fullscreen still requires more testing.
        private static void HandleExclusiveFullscreen(bool hasFocus)
        {
            if (hasFocus)
            {
                if (_fullScreen)
                {
                    _fullScreen = false;
                    Screen.SetResolution(ScreenWidth, ScreenHeight, true);
                }
            }
            else if (!_fullScreen)
            {
                if (Screen.fullScreen)
                {
                    ScreenWidth = Screen.width;
                    ScreenHeight = Screen.height;

                    _fullScreen = true;
                    Screen.SetResolution(960, 600, false);

                    ShowWindow(GetActiveWindow(), 2); // SW_SHOWMINIMIZED
                }
            }
        }

        public static void HandleWindowFocusEvent(bool hasFocus)
        {
            // HandleExclusiveFullscreen(hasFocus);

            if (hasFocus)
            {
                GameObject mainCam = GameObject.Find("MainCamera");
                if (mainCam != null)
                {
                    IN_GAME_MAIN_CAMERA mainCamera = mainCam.GetComponent<IN_GAME_MAIN_CAMERA>();
                    mainCamera.StartCoroutine(MarkHudDirty(mainCamera));
                }
            }

            if (GuardianClient.Properties.RestrainCursor.Value)
            {
                HackCursor(hasFocus);
            }

            if (hasFocus)
            {
                Application.targetFrameRate = -1;
                if (int.TryParse((string)FengGameManagerMKII.Settings[184], out int targetFps) && targetFps > 0)
                {
                    Application.targetFrameRate = targetFps;
                }
            }
            else if (GuardianClient.Properties.LimitUnfocusedFPS.Value &&
                     GuardianClient.Properties.MaxUnfocusedFPS.Value > 0)
            {
                // FPS under 51 could potentially affect Physics-related logic, ensure the MasterClient does not attempt to go below that.
                Application.targetFrameRate =
                    MathHelper.MaxInt(GuardianClient.Properties.MaxUnfocusedFPS.Value,
                        PhotonNetwork.isMasterClient ? 51 : 1);
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