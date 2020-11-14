using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : Photon.MonoBehaviour
{
    private Vector3 correctPos;
    private Vector3 correctVelocity;
    public float SmoothingDelay = 10f;
    public Transform firingPoint;
    public List<TitanTrigger> myTitanTriggers;
    public bool disabled;
    public bool isCollider;
    public HERO myHero;

    private void Awake()
    {
        if (base.photonView != null)
        {
            base.photonView.observed = this;
            correctPos = base.transform.position;
            correctVelocity = Vector3.zero;
            GetComponent<SphereCollider>().enabled = false;
            if (base.photonView.isMine)
            {
                StartCoroutine(WaitAndDestroy(10f));
                myTitanTriggers = new List<TitanTrigger>();
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(base.transform.position);
            stream.SendNext(base.rigidbody.velocity);
        }
        else
        {
            correctPos = (Vector3)stream.ReceiveNext();
            correctVelocity = (Vector3)stream.ReceiveNext();
        }
    }

    public void Update()
    {
        if (!base.photonView.isMine)
        {
            base.transform.position = Vector3.Lerp(base.transform.position, correctPos, Time.deltaTime * SmoothingDelay);
            base.rigidbody.velocity = correctVelocity;
        }
    }

    public void FixedUpdate()
    {
        if (!base.photonView.isMine || disabled)
        {
            return;
        }
        LayerMask mask = 1 << LayerMask.NameToLayer("PlayerAttackBox");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = (int)mask | (int)mask2;
        if (!isCollider)
        {
            LayerMask mask4 = 1 << LayerMask.NameToLayer("Ground");
            mask3 = ((int)mask3 | (int)mask4);
        }
        Collider[] array = Physics.OverlapSphere(base.transform.position, 0.6f, mask3.value);
        bool flag = false;
        for (int i = 0; i < array.Length; i++)
        {
            GameObject gameObject = array[i].gameObject;
            if (gameObject.layer == 16)
            {
                TitanTrigger component = gameObject.GetComponent<TitanTrigger>();
                if (component != null && !myTitanTriggers.Contains(component))
                {
                    component.isCollide = true;
                    myTitanTriggers.Add(component);
                }
            }
            else if (gameObject.layer == 10)
            {
                TITAN component2 = gameObject.transform.root.gameObject.GetComponent<TITAN>();
                if (!(component2 != null))
                {
                    continue;
                }
                if (component2.abnormalType == AbnormalType.TYPE_CRAWLER)
                {
                    if (gameObject.name == "head")
                    {
                        component2.photonView.RPC("DieByCannon", component2.photonView.owner, myHero.photonView.viewID);
                        component2.dieBlow(base.transform.position, 0.2f);
                        i = array.Length;
                    }
                }
                else if (gameObject.name == "head")
                {
                    component2.photonView.RPC("DieByCannon", component2.photonView.owner, myHero.photonView.viewID);
                    component2.dieHeadBlow(base.transform.position, 0.2f);
                    i = array.Length;
                }
                else if (Random.Range(0f, 1f) < 0.5f)
                {
                    component2.hitL(base.transform.position, 0.05f);
                }
                else
                {
                    component2.hitR(base.transform.position, 0.05f);
                }
                destroyMe();
            }
            else if (gameObject.layer == 9 && (gameObject.transform.root.name.Contains("CannonWall") || gameObject.transform.root.name.Contains("CannonGround")))
            {
                flag = true;
            }
        }
        if (!isCollider && !flag)
        {
            isCollider = true;
            GetComponent<SphereCollider>().enabled = true;
        }
    }

    public void OnCollisionEnter(Collision myCollision)
    {
        if (base.photonView.isMine)
        {
            destroyMe();
        }
    }

    public IEnumerator WaitAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        destroyMe();
    }

    public void destroyMe()
    {
        if (disabled)
        {
            return;
        }
        disabled = true;
        GameObject gameObject = PhotonNetwork.Instantiate("FX/boom4", base.transform.position, base.transform.rotation, 0);
        EnemyCheckCollider[] componentsInChildren = gameObject.GetComponentsInChildren<EnemyCheckCollider>();
        foreach (EnemyCheckCollider enemyCheckCollider in componentsInChildren)
        {
            enemyCheckCollider.dmg = 0;
        }
        if (RCSettings.DeadlyCannons == 1)
        {
            foreach (HERO player in FengGameManagerMKII.Instance.heroes)
            {
                if (player != null && Vector3.Distance(player.transform.position, base.transform.position) <= 20f && !player.photonView.isMine)
                {
                    GameObject gameObject2 = player.gameObject;
                    PhotonPlayer owner = gameObject2.GetPhotonView().owner;
                    if (RCSettings.TeamMode > 0 && PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCTeam] != null && owner.customProperties[PhotonPlayerProperty.RCTeam] != null)
                    {
                        int num = GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCTeam]);
                        int num2 = GExtensions.AsInt(owner.customProperties[PhotonPlayerProperty.RCTeam]);
                        if (num == 0 || num != num2)
                        {
                            gameObject2.GetComponent<HERO>().MarkDead();
                            gameObject2.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]) + " ");
                            FengGameManagerMKII.Instance.UpdatePlayerKillInfo(0, PhotonNetwork.player);
                        }
                    }
                    else
                    {
                        gameObject2.GetComponent<HERO>().MarkDead();
                        gameObject2.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]) + " ");
                        FengGameManagerMKII.Instance.UpdatePlayerKillInfo(0, PhotonNetwork.player);
                    }
                }
            }
        }
        if (myTitanTriggers != null)
        {
            for (int j = 0; j < myTitanTriggers.Count; j++)
            {
                if (myTitanTriggers[j] != null)
                {
                    myTitanTriggers[j].isCollide = false;
                }
            }
        }
        PhotonNetwork.Destroy(base.gameObject);
    }
}
