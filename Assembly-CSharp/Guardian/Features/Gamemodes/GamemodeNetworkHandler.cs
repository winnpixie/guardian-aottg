using UnityEngine;

namespace Guardian.Features.Gamemodes
{
    public class GamemodeNetworkHandler : MonoBehaviour
    {
        void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            if (PhotonNetwork.isMasterClient)
            {
                GuardianClient.Gamemodes.CurrentMode.OnPlayerJoin(player);
            }
        }

        void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            if (PhotonNetwork.isMasterClient)
            {
                GuardianClient.Gamemodes.CurrentMode.OnPlayerLeave(player);
            }
        }

        void OnLeftRoom()
        {
            GuardianClient.Gamemodes.CurrentMode.CleanUp();
        }
    }
}