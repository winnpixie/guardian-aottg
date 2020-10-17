using Photon;
using UnityEngine;

public class SelfDestroy : Photon.MonoBehaviour
{
    public float CountDown = 5f;

    private void Update()
    {
        CountDown -= Time.deltaTime;
        if (!(CountDown <= 0f))
        {
            return;
        }
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.SINGLE)
        {
            Object.Destroy(base.gameObject);
        }
        else
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.MULTIPLAYER)
            {
                return;
            }
            if (base.photonView != null)
            {
                if (base.photonView.viewID == 0)
                {
                    Object.Destroy(base.gameObject);
                }
                else if (base.photonView.isMine)
                {
                    PhotonNetwork.Destroy(base.gameObject);
                }
            }
            else
            {
                Object.Destroy(base.gameObject);
            }
        }
    }
}
