using UnityEngine;

public class CubeCollector : MonoBehaviour
{
    public int type;

    private void Update()
    {
        if (!(GameObject.FindGameObjectWithTag("Player") == null))
        {
            GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
            if (Vector3.Distance(gameObject.transform.position, base.transform.position) < 8f)
            {
                Object.Destroy(base.gameObject);
            }
        }
    }
}
