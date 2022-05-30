using UnityEngine;

public class BombExplode : Photon.MonoBehaviour
{
    public GameObject myExplosion;

    public void Start()
    {
        if (base.photonView == null)
        {
            return;
        }
        PhotonPlayer owner = base.photonView.owner;
        if (RCSettings.TeamMode > 0)
        {
            switch (GExtensions.AsInt(owner.customProperties[PhotonPlayerProperty.RCTeam]))
            {
                case 1:
                    GetComponentInChildren<ParticleSystem>().startColor = Color.cyan;
                    break;
                case 2:
                    GetComponentInChildren<ParticleSystem>().startColor = Color.magenta;
                    break;
                default:
                    {
                        float r = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombR]);
                        float g = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombG]);
                        float b = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombB]);
                        float a = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombA]);
                        a = Mathf.Max(0.5f, a);
                        GetComponentInChildren<ParticleSystem>().startColor = new Color(r, g, b, a);
                        break;
                    }
            }
        }
        else
        {
            float r = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombR]);
            float g = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombG]);
            float b = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombB]);
            float a = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombA]);
            a = Mathf.Max(0.5f, a);
            GetComponentInChildren<ParticleSystem>().startColor = new Color(r, g, b, a);
        }
        float value = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombRadius]) * 2f;
        GetComponentInChildren<ParticleSystem>().startSize = Mathf.Clamp(value, 40f, 120f);

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

                    FengGameManagerMKII.Instance.SendKillInfo(false, GExtensions.AsString(base.photonView.owner.customProperties[PhotonPlayerProperty.Name]), true, titanName);
                    FengGameManagerMKII.Instance.UpdatePlayerKillInfo(0, base.photonView.owner);
                    break;
                }
            }
        }
    }
}
