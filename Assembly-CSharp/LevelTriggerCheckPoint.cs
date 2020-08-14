using UnityEngine;

public class LevelTriggerCheckPoint : MonoBehaviour
{
    private FengGameManagerMKII fengGame;

    private void Start()
    {
        fengGame = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
            {
                fengGame.checkpoint = base.gameObject;
            }
            else if (other.gameObject.GetComponent<HERO>().photonView.isMine)
            {
                fengGame.checkpoint = base.gameObject;
            }
        }
    }
}
