using Guardian.Utilities;
using UnityEngine;

namespace Guardian.UI.Impl.Logging
{
    // Unconventional UI that won't actually be invoked by .OpenScreen, but rather as an always-on UI
    class GuiDebug : Gui
    {
        public override void Draw()
        {
            if (Mod.Properties.ShowLog.Value && !Application.loadedLevelName.Equals("SnapShot") && !Application.loadedLevelName.Equals("characterCreation"))
            {
                if (Mod.Properties.DrawDebugBackground.Value)
                {
                    GUILayout.BeginArea(new Rect(Screen.width - 331f, Screen.height - 255f, 330f, 225f), GuiSkins.Box);
                }
                else
                {
                    GUILayout.BeginArea(new Rect(Screen.width - 331f, Screen.height - 255f, 330f, 225f));
                }

                GUILayout.FlexibleSpace();
                Mod.Logger.ScrollPosition = GUILayout.BeginScrollView(Mod.Logger.ScrollPosition);

                GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
                {
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 0, 0, 0),
                    border = new RectOffset(0, 0, 0, 0)
                };

                foreach (Logger.Entry entry in Mod.Logger.Entries)
                {
                    try
                    {
                        string entryText = $"[{entry.Timestamp}] " + entry.ToString();
                        GUILayout.Label(entryText, labelStyle);
                    }
                    catch { }
                }
                GUILayout.EndScrollView();

                GUILayout.BeginHorizontal();
                // FPS
                if (Mod.Properties.ShowFramerate.Value)
                {
                    GUILayout.Label($"{Mod.FpsCounter.FrameCount} FPS");
                }

                // XYZ
                if (Mod.Properties.ShowCoordinates.Value)
                {
                    string coords = "Coordinates Unavailable";
                    if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Stop)
                    {
                        Photon.MonoBehaviour myObj = null;

                        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                        {
                            if (FengGameManagerMKII.Instance.Heroes.Count > 0)
                            {
                                myObj = FengGameManagerMKII.Instance.Heroes[0];
                            }
                        }
                        else if (PhotonNetwork.player.IsTitan)
                        {
                            myObj = PhotonNetwork.player.GetTitan();
                        }
                        else
                        {
                            myObj = PhotonNetwork.player.GetHero();
                        }

                        if (myObj != null)
                        {
                            coords = $"X {MathHelper.Floor(myObj.transform.position.x)} Y {MathHelper.Floor(myObj.transform.position.y)} Z {MathHelper.Floor(myObj.transform.position.z)}";
                        }
                    }
                    GUILayout.Label(coords);
                }

                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
        }
    }
}
