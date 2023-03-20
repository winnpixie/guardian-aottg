using UnityEngine;

namespace RC.UI.Impl
{
    class RCGuiCustomLogicSettings : RCGui
    {
        public override void Draw(FengGameManagerMKII fgm, float halfMenuWidth, float halfMenuHeight)
        {
            FengGameManagerMKII.CurrentScriptLogic = GUI.TextField(new Rect(halfMenuWidth + 50f, halfMenuHeight + 82f, 600f, 270f), FengGameManagerMKII.CurrentScriptLogic);
            if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 365f, 50f, 20f), "Copy"))
            {
                TextEditor textEditor = new TextEditor()
                {
                    content = new GUIContent(FengGameManagerMKII.CurrentScriptLogic)
                };
                textEditor.SelectAll();
                textEditor.Copy();
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 400f, halfMenuHeight + 365f, 50f, 20f), "Clear"))
            {
                FengGameManagerMKII.CurrentScriptLogic = string.Empty;
            }
        }
    }
}
