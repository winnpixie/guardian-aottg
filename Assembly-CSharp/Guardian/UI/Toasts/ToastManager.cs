using Guardian.Utilities;
using UnityEngine;

namespace Guardian.UI.Toasts
{
    class ToastManager
    {
        private readonly SynchronizedList<Toast> _toasts = new SynchronizedList<Toast>();

        public void Draw()
        {
            for (int i = _toasts.Count; i > 0; i--)
            {
                int offset = _toasts.Count - i;
                Toast toast = _toasts[i - 1];
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
            _toasts.RemoveAll(toast => (now - toast.Time) / 1000f >= toast.TimeToLive);
        }

        public void Add(Toast toast)
        {
            _toasts.Add(toast);
        }
    }
}