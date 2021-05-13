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
                    if (other.gameObject.GetComponent<HERO>() != null)
                    {
                        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                        {
                            other.gameObject.GetComponent<HERO>().Die(other.gameObject.rigidbody.velocity * 50f, false);
                        }
                        else if (other.gameObject.GetPhotonView().isMine)
                        {
                            other.gameObject.GetComponent<HERO>().NetDieLocal2(base.rigidbody.velocity * 50f, isBite: false, -1, Guardian.Mod.Properties.LavaDeathMessage.Value);
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
