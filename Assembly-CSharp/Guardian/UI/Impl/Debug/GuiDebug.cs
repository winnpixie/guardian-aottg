using Guardian.Utilities;
using UnityEngine;

namespace Guardian.UI.Impl.Debug
{
    // Unconventional UI that won't actually be invoked by .OpenScreen, but rather as an always-on UI
    class GuiDebug : Gui
    {
        public override void Draw()
        {
            if (!GuardianClient.Properties.ShowLog.Value) return;
            if (Application.loadedLevelName.Equals("SnapShot")) return;
            if (Application.loadedLevelName.Equals("characterCreation")) return;

            if (GuardianClient.Properties.DrawDebugBackground.Value)
            {
                GUILayout.BeginArea(new Rect(Screen.width - 331f, Screen.height - 255f, 330f, 225f), GuiSkins.Box);
            }
            else
            {
                GUILayout.BeginArea(new Rect(Screen.width - 331f, Screen.height - 255f, 330f, 225f));
            }

            GUILayout.FlexibleSpace();
            GuardianClient.Logger.ScrollPosition = GUILayout.BeginScrollView(GuardianClient.Logger.ScrollPosition);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                border = new RectOffset(0, 0, 0, 0)
            };

            foreach (Logger.Entry entry in GuardianClient.Logger.Entries)
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
            if (GuardianClient.Properties.ShowFramerate.Value)
            {
                GUILayout.Label($"{GuardianClient.FpsCounter.FrameCount} FPS");
            }

            // XYZ
            if (GuardianClient.Properties.ShowCoordinates.Value)
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
