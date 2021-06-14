using UnityEngine;
using Guardian.Utilities;

namespace Guardian.UI
{
    class ModUI : MonoBehaviour
    {
        public static UIBase CurrentScreen;

        private GUIStyle boxStyle;

        void OnGUI()
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.Draw();
            }

            if (Mod.Properties.ShowLog.Value)
            {
                if (boxStyle == null)
                {
                    boxStyle = new GUIStyle(GUI.skin.box);
                    Texture2D flat = new Texture2D(1, 1);
                    flat.SetPixel(0, 0, new Color(0.125f, 0.125f, 0.125f, 0.6f));
                    flat.Apply();
                    boxStyle.normal.background = flat;
                }

                GUI.SetNextControlName(string.Empty);

                if (Mod.Properties.LogBackground.Value)
                {
                    GUILayout.BeginArea(new Rect(Screen.width - 331f, Screen.height - 255f, 330f, 225f), boxStyle);
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

                foreach (var message in Mod.Logger.Messages)
                {
                    try
                    {
                        GUILayout.Label(message, labelStyle);
                    }
                    catch { }
                }
                GUILayout.EndScrollView();

                var coords = "N/A";
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
                {
                    if (!GameHelper.IsDead(PhotonNetwork.player))
                    {
                        var mb = GameHelper.IsPT(PhotonNetwork.player) ? (Photon.MonoBehaviour)GameHelper.GetPT(PhotonNetwork.player)
                            : (Photon.MonoBehaviour)GameHelper.GetHero(PhotonNetwork.player);
                        if (mb != null)
                        {
                            coords = $"{MathHelper.Floor(mb.transform.position.x)} / {MathHelper.Floor(mb.transform.position.y)} / {MathHelper.Floor(mb.transform.position.z)}";
                        }
                    }
                }

                GUILayout.Label($"FPS: {MathHelper.Floor(1f / Time.smoothDeltaTime)} X/Y/Z {coords}");
                GUILayout.EndArea();
            }
        }
    }
}
