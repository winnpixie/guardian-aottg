using RC;
using UnityEngine;

public class BombExplode : Photon.MonoBehaviour
{
    public GameObject myExplosion;

    public void Start()
    {
        if (base.photonView == null) return;

        PhotonPlayer owner = base.photonView.owner;

        // Guardian
        if (PhotonNetwork.isMasterClient && Guardian.GuardianClient.Properties.BombsKillTitans.Value)
        {
            foreach (TITAN titan in FengGameManagerMKII.Instance.Titans)
            {
                if ((titan.neck.position - base.transform.position).sqrMagnitude <= 400) // 20 units
                {
                    if (titan.abnormalType == TitanClass.Crawler)
                    {
                        titan.DieBlow(base.transform.position, 0.2f);
                    }
                    else
                    {
                        titan.DieHeadBlow(base.transform.position, 0.2f);
                    }

                    string titanName = titan.name;
                    if (titan.nonAI)
                    {
                        titanName = GExtensions.AsString(titan.photonView.owner.customProperties[PhotonPlayerProperty.Name]);
                    }

                    FengGameManagerMKII.Instance.SendKillInfo(false, GExtensions.AsString(owner.customProperties[PhotonPlayerProperty.Name]), true, titanName);
                    FengGameManagerMKII.Instance.UpdatePlayerKillInfo(0, owner);
                    break;
                }
            }
        }

        float value = GExtensions.AsFloat(owner.customProperties[RCPlayerProperty.RCBombRadius]) * 2f;
        GetComponentInChildren<ParticleSystem>().startSize = Mathf.Clamp(value, 40f, 120f);

        if (RCSettings.TeamMode > 0)
        {
            switch (GExtensions.AsInt(owner.customProperties[RCPlayerProperty.RCTeam]))
            {
                case 1:
                    GetComponentInChildren<ParticleSystem>().startColor = Color.cyan;
                    return;
                case 2:
                    GetComponentInChildren<ParticleSystem>().startColor = Color.magenta;
                    return;
            }
        }

        float r = GExtensions.AsFloat(owner.customProperties[RCPlayerProperty.RCBombR]);
        float g = GExtensions.AsFloat(owner.customProperties[RCPlayerProperty.RCBombG]);
        float b = GExtensions.AsFloat(owner.customProperties[RCPlayerProperty.RCBombB]);
        float a = GExtensions.AsFloat(owner.customProperties[RCPlayerProperty.RCBombA]);
        a = Mathf.Max(0.5f, a);
        GetComponentInChildren<ParticleSystem>().startColor = new Color(r, g, b, a);
    }
}
