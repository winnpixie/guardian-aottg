using UnityEngine;

public class RacingKillTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 8) return;

        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Multiplayer) return;

        GameObject otherGo = other.gameObject.transform.root.gameObject;
        HERO component = otherGo.GetComponent<HERO>();

        if (component == null || !component.photonView.isMine || component.HasDied()) return;

        component.MarkDead();
        component.photonView.RPC("netDie2", PhotonTargets.All, -1, Guardian.Mod.Properties.LavaDeathMessage.Value);
    }
}
