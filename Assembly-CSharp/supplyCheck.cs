using UnityEngine;

public class supplyCheck : MonoBehaviour
{
    private float stepTime = 1f;
    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (!(elapsedTime > stepTime))
        {
            return;
        }
        elapsedTime -= stepTime;
        GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject gameObject in array)
        {
            HERO hero = gameObject.GetComponent<HERO>();
            if (hero != null)
            {
                if (Vector3.Distance(gameObject.transform.position, base.transform.position) < 1.5f)
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.SINGLE || hero.photonView.isMine)
                    {
                        hero.getSupply();
                    }
                }
            }
        }
    }

    private void Start()
    {
        if (Minimap.Instance != null)
        {
            Minimap.Instance.TrackGameObjectOnMinimap(base.gameObject, Color.white, trackOrientation: false, depthAboveAll: true, Minimap.IconStyle.SUPPLY);
        }
    }
}
