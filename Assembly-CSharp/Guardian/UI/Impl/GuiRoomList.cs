using Guardian.UI.Components.Impl;
using System;
using UnityEngine;

namespace Guardian.UI.Impl
{
    class GuiRoomList : Gui
    {
        private readonly PanelMultiJoin _roomListPanel;

        private readonly GLabel _filterLbl = new GLabel("=+= Extra Filters =+=");
        private readonly GLabel _activityLbL = new GLabel("=+= Server Activity =+=");
        private readonly GLabel _roomsLbl = new GLabel("? Room(s)");
        private readonly GLabel _playingLbl = new GLabel("? Playing");
        private readonly GLabel _browsingLbL = new GLabel("? Browsing");
        private readonly GLabel _totalLbl = new GLabel("? Connected");

        private readonly GCheckBox _hideFullOpt = new GCheckBox("Hide Full Rooms");
        private readonly GCheckBox _hideLockedOpt = new GCheckBox("Hide Locked Rooms");

        public GuiRoomList(PanelMultiJoin panelMultiJoin) : base()
        {
            this._roomListPanel = panelMultiJoin;
        }

        public override void Draw()
        {
            if (!PhotonNetwork.connected || _roomListPanel == null)
            {
                GuardianClient.GuiController.OpenScreen(null);
                return;
            }

            float height = Math.Max(250f, Screen.height / 4f);
            GUILayout.BeginArea(new Rect(5f, (Screen.height / 2f) - (height / 2f), 225f, height), GuiSkins.Box);
            _filterLbl.Tick();

            _hideFullOpt.Tick();
            _roomListPanel.HideFullRooms = _hideFullOpt.Selected;

            _hideLockedOpt.Tick();
            _roomListPanel.HideLockedRooms = _hideLockedOpt.Selected;

            GUILayout.FlexibleSpace();
            _activityLbL.Tick();

            _roomsLbl.Text = $"{PhotonNetwork.countOfRooms} Room(s)";
            _roomsLbl.Tick();

            _playingLbl.Text = $"{PhotonNetwork.countOfPlayersInRooms} Playing";
            _playingLbl.Tick();

            _browsingLbL.Text = $"{PhotonNetwork.countOfPlayersOnMaster} Browsing";
            _browsingLbL.Tick();

            _totalLbl.Text = $"{PhotonNetwork.countOfPlayers} Connected";
            _totalLbl.Tick();

            GUILayout.EndArea();
        }
    }
}