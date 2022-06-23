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
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                fengGame.FinishGame();
                disable = true;
            }
            else if (other.gameObject.GetComponent<HERO>().photonView.isMine)
            {
                fengGame.FinishRaceMulti();
                disable = true;
            }
        }
    }
}
