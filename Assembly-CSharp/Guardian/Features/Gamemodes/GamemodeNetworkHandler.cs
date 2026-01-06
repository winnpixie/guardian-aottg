using UnityEngine;

namespace Guardian.Features.Gamemodes
{
    public class GamemodeNetworkHandler : MonoBehaviour
    {
        private void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            if (PhotonNetwork.isMasterClient)
            {
                GuardianClient.Gamemodes.CurrentMode.OnPlayerJoin(player);
            }
        }

        private void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            if (PhotonNetwork.isMasterClient)
            {
                GuardianClient.Gamemodes.CurrentMode.OnPlayerLeave(player);
            }
        }

        private void OnLeftRoom()
        {
            GuardianClient.Gamemodes.CurrentMode.CleanUp();
        }
    }
}