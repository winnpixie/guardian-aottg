using Guardian.UI.Components.Impl;
using UnityEngine;

namespace Guardian.UI.Impl
{
    class GuiRoomList : Gui
    {
        private readonly PanelMultiJoin _roomListPanel;

        private readonly GLabel _filterLbl = new GLabel("Room Filters ++");

        private readonly GCheckBox _hideFullOpt = new GCheckBox("Hide Full");
        private readonly GCheckBox _hideLockedOpt = new GCheckBox("Hide Locked");

        public GuiRoomList(PanelMultiJoin panelMultiJoin) : base()
        {
            this._roomListPanel = panelMultiJoin;
        }

        public override void Draw()
        {
            if (_roomListPanel == null)
            {
                GuardianClient.GuiController.OpenScreen(null);
                return;
            }

            GUILayout.BeginArea(new Rect(Screen.width / 10f - 100, Screen.height / 10f, 200, 100), GuiSkins.Box);
            _filterLbl.Tick();

            _hideFullOpt.Tick();
            _roomListPanel.g_hideFullRooms = _hideFullOpt.Selected;

            _hideLockedOpt.Tick();
            _roomListPanel.g_hideLockedRooms = _hideLockedOpt.Selected;

            GUILayout.EndArea();
        }
    }
}