using UnityEngine;

public class LevelBottom : MonoBehaviour
{
    public GameObject link;
    public BottomType type;

    private void OnTriggerStay(Collider other)
    {
        if (!(other.gameObject.tag == "Player"))
        {
            return;
        }
        if (type == BottomType.Die)
        {
            if (other.gameObject.GetComponent<HERO>() == null)
            {
                return;
            }
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
            {
                if (other.gameObject.GetPhotonView().isMine)
                {
                    other.gameObject.GetComponent<HERO>().NetDieLocal2(base.rigidbody.velocity * 50f, isBite: false, -1, Guardian.Mod.Properties.LavaDeathMessage.Value);
                }
            }
            else
            {
                other.gameObject.GetComponent<HERO>().Die(other.gameObject.rigidbody.velocity * 50f, isBite: false);
            }
        }
        else if (type == BottomType.Teleport)
        {
            if (link != null)
            {
                other.gameObject.transform.position = link.transform.position;
            }
            else
            {
                other.gameObject.transform.position = Vector3.zero;
            }
        }
    }
}
