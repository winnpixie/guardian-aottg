using UnityEngine;

public class LevelBottom : MonoBehaviour
{
    public GameObject link;
    public BottomType type;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (type)
            {
                case BottomType.Die:
                    HERO hero = other.gameObject.GetComponent<HERO>();

                    if (hero != null && !hero.HasDied())
                    {
                        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                        {
                            hero.Die(other.gameObject.rigidbody.velocity * 50f, false);
                        }
                        else if (hero.photonView.isMine)
                        {
                            hero.NetDieLocal2(other.gameObject.rigidbody.velocity * 50f, false, -1, Guardian.GuardianClient.Properties.LavaDeathMessage.Value, true);
                        }
                    }
                    break;
                case BottomType.Teleport:
                    other.gameObject.transform.position = link != null ? link.transform.position : Vector3.zero;
                    break;
            }
        }
    }
}
