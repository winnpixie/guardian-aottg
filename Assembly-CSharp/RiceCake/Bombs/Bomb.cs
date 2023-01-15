using RC;
using System.Collections;
using UnityEngine;

public class Bomb : Photon.MonoBehaviour
{
    public bool disabled;
    public GameObject myExplosion;
    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;
    private Vector3 correctPlayerVelocity = Vector3.zero;
    public float SmoothingDelay = 10f;

    public void Awake()
    {
        if (base.photonView == null) return;

        base.photonView.observed = this;
        correctPlayerPos = base.transform.position;
        correctPlayerRot = Quaternion.identity;
        PhotonPlayer owner = base.photonView.owner;

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
        GetComponentInChildren<ParticleSystem>().startColor = new Color(r, g, b, 1f);
    }

    public void Explode(float radius)
    {
        disabled = true;
        base.rigidbody.velocity = Vector3.zero;
        Vector3 position = base.transform.position;
        myExplosion = PhotonNetwork.Instantiate("RCAsset/BombExplodeMain", position, Quaternion.Euler(0f, 0f, 0f), 0);

        foreach (HERO player in FengGameManagerMKII.Instance.Heroes)
        {
            if (Vector3.Distance(player.transform.position, position) < radius && !player.photonView.isMine && !player.bombImmune)
            {
                PhotonPlayer owner = player.photonView.owner;
                if (RCSettings.TeamMode > 0 && PhotonNetwork.player.customProperties[RCPlayerProperty.RCTeam] != null && owner.customProperties[RCPlayerProperty.RCTeam] != null)
                {
                    int myTeam = GExtensions.AsInt(PhotonNetwork.player.customProperties[RCPlayerProperty.RCTeam]);
                    int theirTeam = GExtensions.AsInt(owner.customProperties[RCPlayerProperty.RCTeam]);
                    if (myTeam == 0 || myTeam != theirTeam)
                    {
                        player.MarkDead();
                        //gameObject.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]) + " ");
                        player.photonView.RPC("netDie", PhotonTargets.All, base.transform.position, false, base.photonView.viewID, GExtensions.AsString(PhotonNetwork.player.Username) + " ", false);
                        FengGameManagerMKII.Instance.UpdatePlayerKillInfo(0, PhotonNetwork.player);
                    }
                }
                else
                {
                    player.MarkDead();
                    //gameObject.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]) + " ");
                    player.photonView.RPC("netDie", PhotonTargets.All, base.transform.position, false, base.photonView.viewID, GExtensions.AsString(PhotonNetwork.player.Username) + " ", false);
                    FengGameManagerMKII.Instance.UpdatePlayerKillInfo(0, PhotonNetwork.player);
                }
            }
        }

        StartCoroutine(CoWaitAndFade(1.5f));
    }

    private IEnumerator CoWaitAndFade(float time)
    {
        yield return new WaitForSeconds(time);

        PhotonNetwork.Destroy(myExplosion);
        PhotonNetwork.Destroy(base.gameObject);
    }

    public void Update()
    {
        if (!disabled && !base.photonView.isMine)
        {
            base.transform.position = Vector3.Lerp(base.transform.position, correctPlayerPos, Time.deltaTime * SmoothingDelay);
            base.transform.rotation = Quaternion.Lerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * SmoothingDelay);
            base.rigidbody.velocity = correctPlayerVelocity;
        }
    }

    public void DestroyMe()
    {
        if (base.photonView.isMine)
        {
            if (myExplosion != null)
            {
                PhotonNetwork.Destroy(myExplosion);
            }

            PhotonNetwork.Destroy(base.gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(base.transform.position);
            stream.SendNext(base.transform.rotation);
            stream.SendNext(base.rigidbody.velocity);
        }
        else
        {
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
            correctPlayerVelocity = (Vector3)stream.ReceiveNext();
        }
    }
}
