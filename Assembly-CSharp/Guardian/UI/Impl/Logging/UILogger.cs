using Guardian.Utilities;
using UnityEngine;

namespace Guardian.UI.Impl.Logging
{
    // Unconventional UI that won't actually be invoked by .OpenScreen, but rather as an always-on UI
    class UILogger : UIBase
    {
        private Logger.LogType _currentType = Logger.LogType.Info;

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

                GUILayout.BeginHorizontal();
                foreach (Logger.LogType type in System.Enum.GetValues(typeof(Logger.LogType)))
                {
                    if (GUILayout.Button(type.ToString()))
                    {
                        _currentType = type;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Label(_currentType.ToString().AsBold());

                GUILayout.FlexibleSpace();
                Mod.Logger.ScrollPosition = GUILayout.BeginScrollView(Mod.Logger.ScrollPosition);

                GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
                {
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 0, 0, 0),
                    border = new RectOffset(0, 0, 0, 0)
                };

                if (Mod.Logger.EntryDict.TryGetValue(_currentType, out System.Collections.Generic.List<string> entries))
                {
                    foreach (string message in entries)
                    {
                        try
                        {
                            GUILayout.Label(message, labelStyle);
                        }
                        catch { }
                    }
                }
                GUILayout.EndScrollView();


                GUILayout.BeginHorizontal();
                GUILayout.Label($"{MathHelper.Floor(1f / Time.smoothDeltaTime)} FPS");

                if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Stop)
                {
                    string coords = "n/a";
                    Photon.MonoBehaviour myObj = null;

                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                    {
                        if (FengGameManagerMKII.Instance.heroes.Count > 0)
                        {
                            myObj = FengGameManagerMKII.Instance.heroes[0] as HERO;
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
