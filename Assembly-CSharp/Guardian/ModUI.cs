using UnityEngine;

namespace Guardian
{
    class ModUI : MonoBehaviour
    {
        void OnGUI()
        {
            if (Mod.Properties.ShowLog.Value)
            {
                GUI.SetNextControlName(string.Empty);
                GUILayout.BeginArea(new Rect(Screen.width - 331f, Screen.height - 255f, 330f, 225f), GUI.skin.box);
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
