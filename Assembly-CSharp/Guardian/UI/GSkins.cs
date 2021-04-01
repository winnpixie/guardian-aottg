using UnityEngine;

namespace Guardian.UI
{
    class GSkins
    {
        public static GUIStyle Box;
        public static GUIStyle Button;
        public static GUIStyle TextField;
        public static GUIStyle TextArea;

        public static GUIStyle HorizontalScrollbar;
        public static GUIStyle HorizontalScrollbarThumb;

        public static GUIStyle VerticalScrollbar;
        public static GUIStyle VerticalScrollbarThumb;

        private static bool Loaded;

        public static void InitSkins()
        {
            if (!Loaded)
            {
                Loaded = true;

                Texture2D flatNormal = new Texture2D(1, 1);
                flatNormal.SetPixel(0, 0, "222222BB".ToColor());
                flatNormal.Apply();

                Texture2D flatDark = new Texture2D(1, 1);
                flatDark.SetPixel(0, 0, "111111BB".ToColor());
                flatDark.Apply();

                Texture2D flatLight = new Texture2D(1, 1);
                flatLight.SetPixel(0, 0, "333333BB".ToColor());
                flatLight.Apply();

                Texture2D flatLighter = new Texture2D(1, 1);
                flatLighter.SetPixel(0, 0, "444444BB".ToColor());
                flatLighter.Apply();

                Box = new GUIStyle(GUI.skin.box);
                Box.normal.background = flatNormal;
                GUI.skin.box = Box;

                Button = new GUIStyle(GUI.skin.button);
                Button.normal.background = flatLight;
                Button.hover.background = flatLighter;
                Button.active.background = flatDark;
                GUI.skin.button = Button;

                TextField = new GUIStyle(GUI.skin.textField);
                TextField.normal.background = flatDark;
                TextField.hover.background = flatLighter;
                TextField.focused.background = flatLight;
                TextField.focused.textColor = Color.white;
                TextField.active.background = flatLight;
                GUI.skin.textField = TextField;

                TextArea = new GUIStyle(GUI.skin.textArea);
                TextArea.normal.background = flatDark;
                TextArea.hover.background = flatLighter;
                TextArea.focused.background = flatLight;
                TextArea.focused.textColor = Color.white;
                TextArea.active.background = flatLight;
                GUI.skin.textArea = TextArea;

                HorizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar);
                HorizontalScrollbar.normal.background = flatDark;
                GUI.skin.horizontalScrollbar = HorizontalScrollbar;

                HorizontalScrollbarThumb = new GUIStyle(GUI.skin.horizontalScrollbarThumb);
                HorizontalScrollbarThumb.normal.background = flatLight;
                GUI.skin.horizontalScrollbarThumb = HorizontalScrollbarThumb;

                VerticalScrollbar = new GUIStyle(GUI.skin.verticalScrollbar);
                VerticalScrollbar.normal.background = flatDark;
                GUI.skin.verticalScrollbar = VerticalScrollbar;

                VerticalScrollbarThumb = new GUIStyle(GUI.skin.verticalScrollbarThumb);
                VerticalScrollbarThumb.normal.background = flatLight;
                GUI.skin.verticalScrollbarThumb = VerticalScrollbarThumb;
            }
        }
    }
}
