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

        private static bool s_initialized;

        public static void InitSkins()
        {
            if (!s_initialized)
            {
                s_initialized = true;

                Texture2D flatDark = new Texture2D(1, 1);
                flatDark.SetPixel(0, 0, 0x010101AA.ToColor());
                flatDark.Apply();

                Texture2D flatNormal = new Texture2D(1, 1);
                flatNormal.SetPixel(0, 0, 0x121212AA.ToColor());
                flatNormal.Apply();

                Texture2D flatLight = new Texture2D(1, 1);
                flatLight.SetPixel(0, 0, 0x232323AA.ToColor());
                flatLight.Apply();

                Texture2D flatLighter = new Texture2D(1, 1);
                flatLighter.SetPixel(0, 0, 0x343434AA.ToColor());
                flatLighter.Apply();

                // Rectangles
                Box = new GUIStyle(GUI.skin.box);
                Box.normal.background = flatNormal;
                GUI.skin.box = Box;

                // Buttons
                Button = new GUIStyle(GUI.skin.button);
                Button.normal.background = flatLight;
                Button.hover.background = flatLighter;
                Button.active.background = flatDark;
                GUI.skin.button = Button;

                // Text fields
                TextField = new GUIStyle(GUI.skin.textField);
                TextField.normal.background = flatDark;
                TextField.hover.background = flatLighter;
                TextField.focused.background = flatLight;
                TextField.focused.textColor = Color.white;
                TextField.active.background = flatLight;
                GUI.skin.textField = TextField;

                // Text areas
                TextArea = new GUIStyle(GUI.skin.textArea);
                TextArea.normal.background = flatDark;
                TextArea.hover.background = flatLighter;
                TextArea.focused.background = flatLight;
                TextArea.focused.textColor = Color.white;
                TextArea.active.background = flatLight;
                GUI.skin.textArea = TextArea;

                // Horizontal scrollbar
                HorizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar);
                HorizontalScrollbar.normal.background = flatDark;
                GUI.skin.horizontalScrollbar = HorizontalScrollbar;

                HorizontalScrollbarThumb = new GUIStyle(GUI.skin.horizontalScrollbarThumb);
                HorizontalScrollbarThumb.normal.background = flatLight;
                GUI.skin.horizontalScrollbarThumb = HorizontalScrollbarThumb;

                // Vertical scrollbar
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
