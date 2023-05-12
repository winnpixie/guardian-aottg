using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

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
        public static extern bool SetWindowTitle([In] int hWnd, [In] string lpString);

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

            // TODO: Testing for bRuki.
            if (!locked)
            {
                RestrictCursor();
            }
        }

        public static void HandleWindowFocusEvent(bool hasFocus)
        {
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
                    Application.targetFrameRate = GuardianClient.Properties.MaxUnfocusedFPS.Value;
                }
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
                            mainCamera.StartCoroutine(MarkHudDirty(mainCamera));
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

        private static IEnumerator MarkHudDirty(IN_GAME_MAIN_CAMERA mainCamera)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            mainCamera.needSetHUD = true;
        }
    }
}
