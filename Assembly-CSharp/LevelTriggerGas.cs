using UnityEngine;

public class LevelTriggerGas : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.SINGLE)
            {
                other.gameObject.GetComponent<HERO>().FillGas();
                Object.Destroy(base.gameObject);
            }
            else if (other.gameObject.GetComponent<HERO>().photonView.isMine)
            {
                other.gameObject.GetComponent<HERO>().FillGas();
                Object.Destroy(base.gameObject);
            }
        }
    }
}
