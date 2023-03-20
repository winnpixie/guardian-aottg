using UnityEngine;

namespace RC.UI.Impl
{
    class RCGuiGeneralSettings : RCGui
    {
        public override void Draw(FengGameManagerMKII fgm, float halfMenuWidth, float halfMenuHeight)
        {
            GUI.Label(new Rect(halfMenuWidth + 150f, halfMenuHeight + 51f, 185f, 22f), "Graphics", "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 81f, 185f, 22f), "Disable custom gas textures:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 106f, 185f, 22f), "Disable weapon trail:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 131f, 185f, 22f), "Disable wind effect:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 156f, 185f, 22f), "Enable vSync:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 184f, 227f, 20f), "FPS Cap (0 for disabled):", "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 212f, 150f, 22f), "Texture Quality:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 242f, 150f, 22f), "Overall Quality:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 272f, 185f, 22f), "Disable Mipmapping:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 297f, 185f, 65f), "*Disabling mipmapping will increase custom texture quality at the cost of performance.", "Label");
            fgm.qualitySlider = GUI.HorizontalSlider(new Rect(halfMenuWidth + 199f, halfMenuHeight + 247f, 115f, 20f), fgm.qualitySlider, 0f, 1f);
            PlayerPrefs.SetFloat("GameQuality", fgm.qualitySlider);
            if (fgm.qualitySlider < 0.167f)
            {
                QualitySettings.SetQualityLevel(0, applyExpensiveChanges: true);
            }
            else if (fgm.qualitySlider < 0.33f)
            {
                QualitySettings.SetQualityLevel(1, applyExpensiveChanges: true);
            }
            else if (fgm.qualitySlider < 0.5f)
            {
                QualitySettings.SetQualityLevel(2, applyExpensiveChanges: true);
            }
            else if (fgm.qualitySlider < 0.67f)
            {
                QualitySettings.SetQualityLevel(3, applyExpensiveChanges: true);
            }
            else if (fgm.qualitySlider < 0.83f)
            {
                QualitySettings.SetQualityLevel(4, applyExpensiveChanges: true);
            }
            else if (fgm.qualitySlider <= 1f)
            {
                QualitySettings.SetQualityLevel(5, applyExpensiveChanges: true);
            }

            bool customGas = false;
            bool weaponTrails = false;
            bool showSpeedLines = false;
            bool mipmapping = false;
            bool vsyncEnabled = false;
            if ((int)FengGameManagerMKII.Settings[15] == 1)
            {
                customGas = true;
            }
            if ((int)FengGameManagerMKII.Settings[92] == 1)
            {
                weaponTrails = true;
            }
            if ((int)FengGameManagerMKII.Settings[93] == 1)
            {
                showSpeedLines = true;
            }
            if ((int)FengGameManagerMKII.Settings[63] == 1)
            {
                mipmapping = true;
            }
            if ((int)FengGameManagerMKII.Settings[183] == 1)
            {
                vsyncEnabled = true;
            }
            bool toggleCustomGas = GUI.Toggle(new Rect(halfMenuWidth + 274f, halfMenuHeight + 81f, 40f, 20f), customGas, "On");
            if (toggleCustomGas != customGas)
            {
                if (toggleCustomGas)
                {
                    FengGameManagerMKII.Settings[15] = 1;
                }
                else
                {
                    FengGameManagerMKII.Settings[15] = 0;
                }
            }
            bool toggleWeaponTrail = GUI.Toggle(new Rect(halfMenuWidth + 274f, halfMenuHeight + 106f, 40f, 20f), weaponTrails, "On");
            if (toggleWeaponTrail != weaponTrails)
            {
                if (toggleWeaponTrail)
                {
                    FengGameManagerMKII.Settings[92] = 1;
                }
                else
                {
                    FengGameManagerMKII.Settings[92] = 0;
                }
            }
            bool toggleSpeedLines = GUI.Toggle(new Rect(halfMenuWidth + 274f, halfMenuHeight + 131f, 40f, 20f), showSpeedLines, "On");
            if (toggleSpeedLines != showSpeedLines)
            {
                if (toggleSpeedLines)
                {
                    FengGameManagerMKII.Settings[93] = 1;
                }
                else
                {
                    FengGameManagerMKII.Settings[93] = 0;
                }
            }
            bool toggleVsync = GUI.Toggle(new Rect(halfMenuWidth + 274f, halfMenuHeight + 156f, 40f, 20f), vsyncEnabled, "On");
            if (toggleVsync != vsyncEnabled)
            {
                if (toggleVsync)
                {
                    FengGameManagerMKII.Settings[183] = 1;
                    QualitySettings.vSyncCount = 1;
                }
                else
                {
                    FengGameManagerMKII.Settings[183] = 0;
                    QualitySettings.vSyncCount = 0;
                }

                Minimap.WaitAndTryRecaptureInstance(0.5f);
            }
            bool toggleMipmaps = GUI.Toggle(new Rect(halfMenuWidth + 274f, halfMenuHeight + 272f, 40f, 20f), mipmapping, "On");
            if (toggleMipmaps != mipmapping)
            {
                if (toggleMipmaps)
                {
                    FengGameManagerMKII.Settings[63] = 1;
                }
                else
                {
                    FengGameManagerMKII.Settings[63] = 0;
                }
                FengGameManagerMKII.LinkHash[0].Clear();
                FengGameManagerMKII.LinkHash[1].Clear();
                FengGameManagerMKII.LinkHash[2].Clear();
            }
            if (GUI.Button(new Rect(halfMenuWidth + 199f, halfMenuHeight + 212f, 115f, 20f), MasterTextureType(QualitySettings.masterTextureLimit)))
            {
                if (QualitySettings.masterTextureLimit <= 0)
                {
                    QualitySettings.masterTextureLimit = 8;
                }
                else
                {
                    QualitySettings.masterTextureLimit--;
                }
                FengGameManagerMKII.LinkHash[0].Clear();
                FengGameManagerMKII.LinkHash[1].Clear();
                FengGameManagerMKII.LinkHash[2].Clear();
            }
            FengGameManagerMKII.Settings[184] = GUI.TextField(new Rect(halfMenuWidth + 234f, halfMenuHeight + 184f, 80f, 20f), (string)FengGameManagerMKII.Settings[184]);
            Application.targetFrameRate = -1;
            if (int.TryParse((string)FengGameManagerMKII.Settings[184], out int targetFps) && targetFps > 0)
            {
                Application.targetFrameRate = targetFps;
            }
            GUI.Label(new Rect(halfMenuWidth + 470f, halfMenuHeight + 51f, 185f, 22f), "Snapshots", "Label");
            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 81f, 185f, 22f), "Enable Snapshots:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 106f, 185f, 22f), "Show In Game:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 131f, 227f, 22f), "Snapshot Minimum Damage:", "Label");
            FengGameManagerMKII.Settings[95] = GUI.TextField(new Rect(halfMenuWidth + 563f, halfMenuHeight + 131f, 65f, 20f), (string)FengGameManagerMKII.Settings[95]);
            bool enableSnapshots = false;
            bool snowSnapshotsInGame = false;
            if (PlayerPrefs.GetInt("EnableSS", 0) == 1)
            {
                enableSnapshots = true;
            }
            if (PlayerPrefs.GetInt("showSSInGame", 0) == 1)
            {
                snowSnapshotsInGame = true;
            }
            bool toggleSnapshots = GUI.Toggle(new Rect(halfMenuWidth + 588f, halfMenuHeight + 81f, 40f, 20f), enableSnapshots, "On");
            if (toggleSnapshots != enableSnapshots)
            {
                if (toggleSnapshots)
                {
                    PlayerPrefs.SetInt("EnableSS", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("EnableSS", 0);
                }
            }
            bool toggleSnapshotsInGame = GUI.Toggle(new Rect(halfMenuWidth + 588f, halfMenuHeight + 106f, 40f, 20f), snowSnapshotsInGame, "On");
            if (snowSnapshotsInGame != toggleSnapshotsInGame)
            {
                if (toggleSnapshotsInGame)
                {
                    PlayerPrefs.SetInt("showSSInGame", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("showSSInGame", 0);
                }
            }
            GUI.Label(new Rect(halfMenuWidth + 485f, halfMenuHeight + 161f, 185f, 22f), "Other", "Label");
            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 186f, 80f, 20f), "Volume:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 211f, 95f, 20f), "Mouse Speed:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 236f, 95f, 20f), "Camera Dist:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 261f, 80f, 20f), "Camera Tilt:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 283f, 80f, 20f), "Invert Mouse:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 305f, 80f, 20f), "Speedometer:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 375f, 80f, 20f), "Minimap:", "Label");
            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 397f, 100f, 20f), "Game Feed:", "Label");
            string[] texts = new string[3]
            {
                                "Off",
                                "Speed",
                                "Damage"
            };
            FengGameManagerMKII.Settings[189] = GUI.SelectionGrid(new Rect(halfMenuWidth + 480f, halfMenuHeight + 305f, 140f, 60f), (int)FengGameManagerMKII.Settings[189], texts, 1, GUI.skin.toggle);
            AudioListener.volume = GUI.HorizontalSlider(new Rect(halfMenuWidth + 478f, halfMenuHeight + 191f, 150f, 20f), AudioListener.volume, 0f, 1f);
            fgm.mouseSlider = GUI.HorizontalSlider(new Rect(halfMenuWidth + 478f, halfMenuHeight + 216f, 150f, 20f), fgm.mouseSlider, 0.1f, 1f);
            PlayerPrefs.SetFloat("MouseSensitivity", fgm.mouseSlider);
            IN_GAME_MAIN_CAMERA.SensitivityMulti = PlayerPrefs.GetFloat("MouseSensitivity");
            fgm.distanceSlider = GUI.HorizontalSlider(new Rect(halfMenuWidth + 478f, halfMenuHeight + 241f, 150f, 20f), fgm.distanceSlider, 0f, 1f);
            PlayerPrefs.SetFloat("cameraDistance", fgm.distanceSlider);
            IN_GAME_MAIN_CAMERA.CameraDistance = 0.3f + fgm.distanceSlider;
            bool cameraTilt = false;
            bool invertMouse = false;
            bool flag28 = false;
            bool flag29 = false;
            if ((int)FengGameManagerMKII.Settings[231] == 1)
            {
                flag28 = true;
            }
            if ((int)FengGameManagerMKII.Settings[244] == 1)
            {
                flag29 = true;
            }
            if (PlayerPrefs.HasKey("cameraTilt"))
            {
                if (PlayerPrefs.GetInt("cameraTilt") == 1)
                {
                    cameraTilt = true;
                }
            }
            else
            {
                PlayerPrefs.SetInt("cameraTilt", 1);
            }
            if (PlayerPrefs.HasKey("invertMouseY"))
            {
                if (PlayerPrefs.GetInt("invertMouseY") == -1)
                {
                    invertMouse = true;
                }
            }
            else
            {
                PlayerPrefs.SetInt("invertMouseY", 1);
            }
            bool toggleCameraTilt = GUI.Toggle(new Rect(halfMenuWidth + 480f, halfMenuHeight + 261f, 40f, 20f), cameraTilt, "On");
            if (cameraTilt != toggleCameraTilt)
            {
                if (toggleCameraTilt)
                {
                    PlayerPrefs.SetInt("cameraTilt", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("cameraTilt", 0);
                }
            }
            bool toggleInvertMouse = GUI.Toggle(new Rect(halfMenuWidth + 480f, halfMenuHeight + 283f, 40f, 20f), invertMouse, "On");
            if (toggleInvertMouse != invertMouse)
            {
                if (toggleInvertMouse)
                {
                    PlayerPrefs.SetInt("invertMouseY", -1);
                }
                else
                {
                    PlayerPrefs.SetInt("invertMouseY", 1);
                }
            }
            bool flag32 = GUI.Toggle(new Rect(halfMenuWidth + 480f, halfMenuHeight + 375f, 40f, 20f), flag28, "On");
            if (flag28 != flag32)
            {
                if (flag32)
                {
                    FengGameManagerMKII.Settings[231] = 1;
                }
                else
                {
                    FengGameManagerMKII.Settings[231] = 0;
                }
            }
            bool flag33 = GUI.Toggle(new Rect(halfMenuWidth + 480f, halfMenuHeight + 397f, 40f, 20f), flag29, "On");
            if (flag29 != flag33)
            {
                if (flag33)
                {
                    FengGameManagerMKII.Settings[244] = 1;
                }
                else
                {
                    FengGameManagerMKII.Settings[244] = 0;
                }
            }
            IN_GAME_MAIN_CAMERA.CameraTilt = PlayerPrefs.GetInt("cameraTilt");
            IN_GAME_MAIN_CAMERA.InvertY = PlayerPrefs.GetInt("invertMouseY");
        }

        private static string MasterTextureType(int type)
        {
            return type switch
            {
                0 => "Highest",
                1 => "Medium",
                2 => "Low",
                3 => "Lower",
                4 => "Lowest",
                5 => "Ultra-Low",
                6 => "Ultra-Lower",
                7 => "Ultra-Lowest",
                8 => "NVIDIA GT 520",
                _ => type.ToString()
            };
        }
    }
}
