using UnityEngine;

namespace Guardian
{
    class ModUI : MonoBehaviour
    {
        private GUIStyle boxStyle;

        void OnGUI()
        {
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
                GUILayout.BeginArea(new Rect(Screen.width - 331f, Screen.height - 255f, 330f, 225f), boxStyle);
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

                GUILayout.Label($"FPS: {Utilities.MathHelper.Floor(1f / Time.smoothDeltaTime)}");
                GUILayout.EndArea();
            }
        }
    }
}
