using UnityEngine;
using Guardian.Utilities;
using Guardian.UI.Impl;

namespace Guardian.UI
{
    class ModUI : MonoBehaviour
    {

        void OnGUI()
        {
            GSkins.InitSkins();

            if (Input.GetKeyDown(KeyCode.Escape) && GUI.GetNameOfFocusedControl().Length == 0 && Mod.UI.CurrentScreen == null)
            {
                Mod.UI.OpenScreen(new UIModSettings());
            }

            if (Mod.UI.CurrentScreen != null)
            {
                Mod.UI.CurrentScreen.Draw();
            }

            if (Mod.Properties.ShowLog.Value && !Application.loadedLevelName.Equals("SnapShot"))
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
                GUILayout.Label($"FPS: {MathHelper.Floor(1f / Time.smoothDeltaTime)}");

                string coords = "n/a";
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
                {
                    if (!GameHelper.IsDead(PhotonNetwork.player))
                    {
                        Photon.MonoBehaviour mb = GameHelper.IsPT(PhotonNetwork.player) ? (Photon.MonoBehaviour)GameHelper.GetPT(PhotonNetwork.player)
                            : (Photon.MonoBehaviour)GameHelper.GetHero(PhotonNetwork.player);
                        if (mb != null)
                        {
                            coords = $"{MathHelper.Floor(mb.transform.position.x)} / {MathHelper.Floor(mb.transform.position.y)} / {MathHelper.Floor(mb.transform.position.z)}";
                        }
                    }
                }
                GUILayout.Label($"X/Y/Z {coords}");

                GUILayout.Label($"Playtime: {GameHelper.FormatTime(GameHelper.CurrentTimeMillis() - Mod.LaunchTime, false, false)}");

                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
        }
    }
}
