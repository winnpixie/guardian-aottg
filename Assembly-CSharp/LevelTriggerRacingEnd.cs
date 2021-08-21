using UnityEngine;

public class LevelTriggerRacingEnd : MonoBehaviour
{
    private bool disable;
    private FengGameManagerMKII fengGame;

    private void Start()
    {
        fengGame = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
        disable = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!disable && other.gameObject.tag == "Player")
        {
            disable = true;
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                fengGame.WinGame();
            }
            else if (other.gameObject.GetComponent<HERO>().photonView.isMine)
            {
                fengGame.FinishRaceMulti();
            }
        }
    }
}
