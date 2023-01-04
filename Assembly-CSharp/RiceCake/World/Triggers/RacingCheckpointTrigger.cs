using UnityEngine;

public class RacingCheckpointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.layer != 8)
        {
            return;
        }

        gameObject = gameObject.transform.root.gameObject;
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && gameObject.GetPhotonView() != null && gameObject.GetPhotonView().isMine)
        {
            HERO component = gameObject.GetComponent<HERO>();
            if (component != null)
            {
                InRoomChat.Instance.AddLine("Checkpoint set.".AsColor("00FF00"));
                component.FillGas();
                FengGameManagerMKII.Instance.racingSpawnPoint = base.gameObject.transform.position;
                FengGameManagerMKII.Instance.racingSpawnPointSet = true;
            }
        }
    }
}
