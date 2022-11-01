using UnityEngine;

public class SelfDestroy : Photon.MonoBehaviour
{
    public float CountDown = 5f;

    private void Update()
    {
        CountDown -= Time.deltaTime;
        if (CountDown > 0) return;

        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView == null || base.photonView.viewID == 0)
        {
            Object.Destroy(base.gameObject);
        }
        else if (base.photonView != null && base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.gameObject);
        }
    }
}
