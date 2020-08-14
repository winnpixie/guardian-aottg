using Photon;
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
                    GetComponent<ParticleSystem>().startColor = Color.cyan;
                    break;
                case 2:
                    GetComponent<ParticleSystem>().startColor = Color.magenta;
                    break;
                default:
                    {
                        float r = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombR]);
                        float g = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombG]);
                        float b = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombB]);
                        float b2 = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombA]);
                        b2 = Mathf.Max(0.5f, b2);
                        GetComponent<ParticleSystem>().startColor = new Color(r, g, b, b2);
                        break;
                    }
            }
        }
        else
        {
            float r = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombR]);
            float g = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombG]);
            float b = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombB]);
            float b2 = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombA]);
            b2 = Mathf.Max(0.5f, b2);
            GetComponent<ParticleSystem>().startColor = new Color(r, g, b, b2);
        }
        float value = GExtensions.AsFloat(owner.customProperties[PhotonPlayerProperty.RCBombRadius]) * 2f;
        GetComponent<ParticleSystem>().startSize = Mathf.Clamp(value, 40f, 120f);
    }
}
