using Guardian.Utilities;
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
                if (!ResourceLoader.TryGetAsset("Custom/Textures/UI/Boxes/normal.png", out Texture2D normalTexture))
                {
                    ResourceLoader.TryGetAsset("Textures/UI/Boxes/normal.png", out normalTexture);
                }

                Box = new GUIStyle(GUI.skin.box);
                Box.normal.background = normalTexture;
                GUI.skin.box = Box;
            }

            // Buttons
            {
                if (!ResourceLoader.TryGetAsset("Custom/Textures/UI/Buttons/normal.png", out Texture2D normalTexture))
                {
                    ResourceLoader.TryGetAsset("Textures/UI/Buttons/normal.png", out normalTexture);
                }
                if (!ResourceLoader.TryGetAsset("Custom/Textures/UI/Buttons/hover.png", out Texture2D hoverTexture))
                {
                    ResourceLoader.TryGetAsset("Textures/UI/Buttons/hover.png", out hoverTexture);
                }
                if (!ResourceLoader.TryGetAsset("Custom/Textures/UI/Buttons/active.png", out Texture2D activeTexture))
                {
                    ResourceLoader.TryGetAsset("Textures/UI/Buttons/active.png", out activeTexture);
                }

                Button = new GUIStyle(GUI.skin.button);
                Button.normal.background = normalTexture;
                Button.hover.background = hoverTexture;
                Button.active.background = activeTexture;
                GUI.skin.button = Button;
            }

            // Text
            {
                if (!ResourceLoader.TryGetAsset("Custom/Textures/UI/Text/normal.png", out Texture2D normalTexture))
                {
                    ResourceLoader.TryGetAsset("Textures/UI/Text/normal.png", out normalTexture);
                }
                if (!ResourceLoader.TryGetAsset("Custom/Textures/UI/Text/hover.png", out Texture2D hoverTexture))
                {
                    ResourceLoader.TryGetAsset("Textures/UI/Text/hover.png", out hoverTexture);
                }
                if (!ResourceLoader.TryGetAsset("Custom/Textures/UI/Text/active.png", out Texture2D activeTexture))
                {
                    ResourceLoader.TryGetAsset("Textures/UI/Text/active.png", out activeTexture);
                }

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
                if (!ResourceLoader.TryGetAsset("Custom/Textures/UI/Scrollbars/background.png", out Texture2D backgroundTexture))
                {
                    ResourceLoader.TryGetAsset("Textures/UI/Scrollbars/background.png", out backgroundTexture);
                }
                if (!ResourceLoader.TryGetAsset("Custom/Textures/UI/Scrollbars/bar.png", out Texture2D barTexture))
                {
                    ResourceLoader.TryGetAsset("Textures/UI/Scrollbars/bar.png", out barTexture);
                }

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
