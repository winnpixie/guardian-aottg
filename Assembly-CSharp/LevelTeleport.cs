using UnityEngine;

public class LevelTeleport : MonoBehaviour
{
    public GameObject link;
    public string levelname = string.Empty;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (levelname != string.Empty)
            {
                Application.LoadLevel(levelname);
            }
            else
            {
                other.gameObject.transform.position = link.transform.position;
            }
        }
    }
}
