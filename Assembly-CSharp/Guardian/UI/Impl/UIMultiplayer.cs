using UnityEngine;

namespace Guardian.UI.Impl
{
    class UIMultiplayer : UIBase
    {
        public override void Draw()
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, 15, 100, 30), "Offline Mode"))
            {
                Mod.UI.OpenScreen(null);

                PhotonNetwork.Disconnect();
                PhotonNetwork.offlineMode = true;
                FengGameManagerMKII.Instance.OnJoinedLobby();
            }
        }
    }
}
