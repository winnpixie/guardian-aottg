using Guardian.Utilities;
using UnityEngine;

namespace Guardian.UI.Toasts
{
    class ToastManager
    {
        private SynchronizedList<Toast> Toasts = new SynchronizedList<Toast>();

        public void Draw()
        {
            for (int i = Toasts.Count; i > 0; i--)
            {
                int offset = Toasts.Count - i;
                Toast toast = Toasts[i - 1];
                if (75 * offset > Screen.height)
                {
                    break;
                }

                GUILayout.BeginArea(new Rect(Screen.width - 305, 5 + (75 * offset), 300, 70), GuiSkins.Box);
                GUILayout.BeginHorizontal();
                GUILayout.Label(toast.Title.AsBold());
                GUILayout.FlexibleSpace();
                GUILayout.Label(toast.Timestamp);
                GUILayout.EndHorizontal();
                GUILayout.Label(toast.Message);
                GUILayout.EndArea();
            }

            long now = GameHelper.CurrentTimeMillis();
            Toasts.RemoveAll(toast => (now - toast.Time) / 1000f >= toast.TimeToLive);
        }

        public void Add(Toast toast)
        {
            Toasts.Add(toast);
        }
    }
}
