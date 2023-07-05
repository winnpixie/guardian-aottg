using Guardian.UI.Components.Impl;
using UnityEngine;

namespace Guardian.UI.Impl
{
    class GuiRoomList : Gui
    {
        private readonly PanelMultiJoin roomListPanel;

        private readonly GLabel filterLabel = new GLabel("Room Filters ++");

        private readonly GCheckBox hideFullOption = new GCheckBox("Hide Full");
        private readonly GCheckBox hideLockedOption = new GCheckBox("Hide Locked");

        public GuiRoomList(PanelMultiJoin panelMultiJoin) : base()
        {
            this.roomListPanel = panelMultiJoin;
        }

        public override void Draw()
        {
            if (roomListPanel == null)
            {
                GuardianClient.GuiController.OpenScreen(null);
                return;
            }

            GUILayout.BeginArea(new Rect(Screen.width / 10 - 100, Screen.height / 10, 200, 100), GuiSkins.Box);
            filterLabel.Tick();

            hideFullOption.Tick();
            roomListPanel.g_hideFullRooms = hideFullOption.Selected;

            hideLockedOption.Tick();
            roomListPanel.g_hideLockedRooms = hideLockedOption.Selected;

            GUILayout.EndArea();
        }
    }
}
