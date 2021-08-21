using Guardian.Utilities;
using Guardian.UI.Impl;
using UnityEngine;

namespace Guardian.UI
{
    class UIManager : MonoBehaviour
    {
        public UIBase CurrentScreen;

        public void OpenScreen(UIBase screen)
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.OnClose();
            }

            CurrentScreen = screen;

            if (CurrentScreen != null)
            {
                CurrentScreen.OnOpen();
            }
        }

        void OnGUI()
        {
            GSkins.InitSkins();

            if (KeyCode.Escape.WasPressedInGUI() && GUI.GetNameOfFocusedControl().Length == 0 && Mod.Menus.CurrentScreen == null)
            {
                Mod.Menus.OpenScreen(new UIModConfiguration());
            }

            if (Mod.Menus.CurrentScreen != null)
            {
                Mod.Menus.CurrentScreen.Draw();
            }

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

                foreach (string message in Mod.Logger.Messages)
                {
                    try
                    {
                        GUILayout.Label(message, labelStyle);
                    }
                    catch { }
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
