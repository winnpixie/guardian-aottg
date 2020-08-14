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
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && gameObject.GetPhotonView() != null && gameObject.GetPhotonView().isMine)
        {
            HERO component = gameObject.GetComponent<HERO>();
            if (component != null)
            {
                FengGameManagerMKII.Instance.chatRoom.AddLine("<color=#00ff00>Checkpoint set.</color>");
                gameObject.GetComponent<HERO>().FillGas();
                FengGameManagerMKII.Instance.racingSpawnPoint = base.gameObject.transform.position;
                FengGameManagerMKII.Instance.racingSpawnPointSet = true;
            }
        }
    }
}
