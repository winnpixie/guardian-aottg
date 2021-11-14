using UnityEngine;

public class RacingKillTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 8)
        {
            return;
        }

        GameObject otherGo = other.gameObject.transform.root.gameObject;
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && otherGo.GetPhotonView() != null && otherGo.GetPhotonView().isMine)
        {
            HERO component = otherGo.GetComponent<HERO>();
            if (component != null)
            {
                component.MarkDead();
                component.photonView.RPC("netDie2", PhotonTargets.All, -1, "Server");
            }
        }
    }
}
