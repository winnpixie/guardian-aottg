using UnityEngine;

public class supplyCheck : MonoBehaviour
{
    private float stepTime = 1f;
    private float elapsedTime;

    private readonly float maxDistanceSq = 1.5f * 1.5f; // 2.25

    private void Start()
    {
        if (Minimap.Instance != null)
        {
            Minimap.Instance.TrackGameObjectOnMinimap(base.gameObject, Color.white, trackOrientation: false, depthAboveAll: true, Minimap.IconStyle.SUPPLY);
        }

        stepTime = 15f / 1000f; // Check 15 times a second for players instead of once per second
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < stepTime) return;

        elapsedTime = 0;

        foreach (HERO hero in FengGameManagerMKII.Instance.Heroes)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && !hero.photonView.isMine) continue;

            if ((hero.transform.position - base.transform.position).sqrMagnitude < maxDistanceSq)
            {
                hero.GetSupply();
            }
        }
    }
}
