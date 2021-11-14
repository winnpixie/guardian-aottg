using Guardian.Utilities;
using System;
using UnityEngine;

namespace Guardian.UI.Impl.Logging
{
    // Unconventional UI that won't actually be invoked by .OpenScreen, but rather as an always-on UI
    class UILogger : UIBase
    {
        public override void Draw()
        {
            if (Mod.Properties.ShowLog.Value && !Application.loadedLevelName.Equals("SnapShot") && !Application.loadedLevelName.Equals("characterCreation"))
            {
                if (Mod.Properties.LogBackground.Value)
                {
                    GUILayout.BeginArea(new Rect(Screen.width - 331f, Screen.height - 255f, 330f, 225f), GSkins.Box);
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
                        DateTime date = GameHelper.Epoch.AddMilliseconds(entry.Timestamp).ToLocalTime();
                        string entryText = "[" + date.ToString("HH:mm:ss") + "] " + entry.ToString();
                        GUILayout.Label(entryText, labelStyle);
                    }
                    catch { }
                }
                GUILayout.EndScrollView();

                GUILayout.BeginHorizontal();
                GUILayout.Label($"{Mod.FpsCounter.Frames} FPS");

                if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Stop)
                {
                    string coords = "n/a";
                    Photon.MonoBehaviour myObj = null;

                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                    {
                        if (FengGameManagerMKII.Instance.Heroes.Count > 0)
                        {
                            myObj = FengGameManagerMKII.Instance.Heroes[0];
                        }
                    }
                    else
                    {
                        if (GameHelper.IsPT(PhotonNetwork.player))
                        {
                            myObj = GameHelper.GetPT(PhotonNetwork.player);
                        }
                        else
                        {
                            myObj = GameHelper.GetHero(PhotonNetwork.player);
                        }
                    }

                    if (myObj != null)
                    {
                        coords = $"X {MathHelper.Floor(myObj.transform.position.x)} Y {MathHelper.Floor(myObj.transform.position.y)} Z {MathHelper.Floor(myObj.transform.position.z)}";
                    }

                    GUILayout.Label(coords);
                }

                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
        }
    }
}
