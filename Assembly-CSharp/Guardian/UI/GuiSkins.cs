using UnityEngine;

namespace Guardian.Ui
{
    class GuiSkins
    {
        public static GUIStyle Box;
        public static GUIStyle Button;
        public static GUIStyle TextField;
        public static GUIStyle TextArea;

        public static GUIStyle HorizontalScrollbar;
        public static GUIStyle HorizontalScrollbarThumb;

        public static GUIStyle VerticalScrollbar;
        public static GUIStyle VerticalScrollbarThumb;

        private static bool IsFirstInit = true;

        public static void InitSkins()
        {
            if (!IsFirstInit) return;

            IsFirstInit = false;

            // Boxes
            {
                Utilities.ResourceLoader.TryGetAsset("Textures/UI/Boxes/normal.png", out Texture2D boxTexture);

                Box = new GUIStyle(GUI.skin.box);
                Box.normal.background = boxTexture;
                GUI.skin.box = Box;
            }

            // Buttons
            {
                Utilities.ResourceLoader.TryGetAsset("Textures/UI/Buttons/normal.png", out Texture2D normalTexture);
                Utilities.ResourceLoader.TryGetAsset("Textures/UI/Buttons/hover.png", out Texture2D hoverTexture);
                Utilities.ResourceLoader.TryGetAsset("Textures/UI/Buttons/active.png", out Texture2D activeTexture);

                Button = new GUIStyle(GUI.skin.button);
                Button.normal.background = normalTexture;
                Button.hover.background = hoverTexture;
                Button.active.background = activeTexture;
                GUI.skin.button = Button;
            }

            // Text
            {
                Utilities.ResourceLoader.TryGetAsset("Textures/UI/Text/normal.png", out Texture2D normalTexture);
                Utilities.ResourceLoader.TryGetAsset("Textures/UI/Text/hover.png", out Texture2D hoverTexture);
                Utilities.ResourceLoader.TryGetAsset("Textures/UI/Text/active.png", out Texture2D activeTexture);

                // Text fields
                TextField = new GUIStyle(GUI.skin.textField);
                TextField.normal.background = normalTexture;
                TextField.hover.background = hoverTexture;
                TextField.focused.background = activeTexture;
                TextField.focused.textColor = Color.white;
                TextField.active.background = activeTexture;
                GUI.skin.textField = TextField;

                // Text areas
                TextArea = new GUIStyle(GUI.skin.textArea);
                TextArea.normal.background = normalTexture;
                TextArea.hover.background = hoverTexture;
                TextArea.focused.background = activeTexture;
                TextArea.focused.textColor = Color.white;
                TextArea.active.background = activeTexture;
                GUI.skin.textArea = TextArea;
            }

            // Scrollbars
            {
                Utilities.ResourceLoader.TryGetAsset("Textures/UI/Scrollbars/background.png", out Texture2D backgroundTexture);
                Utilities.ResourceLoader.TryGetAsset("Textures/UI/Scrollbars/bar.png", out Texture2D barTexture);

                // Horizontal scrollbar
                HorizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar);
                HorizontalScrollbar.normal.background = backgroundTexture;
                GUI.skin.horizontalScrollbar = HorizontalScrollbar;

                HorizontalScrollbarThumb = new GUIStyle(GUI.skin.horizontalScrollbarThumb);
                HorizontalScrollbarThumb.normal.background = barTexture;
                GUI.skin.horizontalScrollbarThumb = HorizontalScrollbarThumb;

                // Vertical scrollbar
                VerticalScrollbar = new GUIStyle(GUI.skin.verticalScrollbar);
                VerticalScrollbar.normal.background = backgroundTexture;
                GUI.skin.verticalScrollbar = VerticalScrollbar;

                VerticalScrollbarThumb = new GUIStyle(GUI.skin.verticalScrollbarThumb);
                VerticalScrollbarThumb.normal.background = barTexture;
                GUI.skin.verticalScrollbarThumb = VerticalScrollbarThumb;
            }
        }
    }
}
