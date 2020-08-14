using Photon;
using UnityEngine;

public class RockThrow : Photon.MonoBehaviour
{
    private bool launched;
    private Vector3 oldP;
    private Vector3 v;
    private Vector3 r;

    private void Start()
    {
        r = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
    }

    private void Update()
    {
        if (!launched)
        {
            return;
        }
        base.transform.Rotate(r);
        v -= 20f * Vector3.up * Time.deltaTime;
        base.transform.position += v * Time.deltaTime;
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && !PhotonNetwork.isMasterClient)
        {
            return;
        }
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("Players");
        LayerMask mask3 = 1 << LayerMask.NameToLayer("EnemyAABB");
        LayerMask mask4 = (int)mask2 | (int)mask | (int)mask3;
        Vector3 position = base.transform.position;
        Vector3 lossyScale = base.transform.lossyScale;
        RaycastHit[] array = Physics.SphereCastAll(position, 2.5f * lossyScale.x, base.transform.position - oldP, Vector3.Distance(base.transform.position, oldP), mask4);
        for (int i = 0; i < array.Length; i++)
        {
            RaycastHit raycastHit = array[i];
            switch (LayerMask.LayerToName(raycastHit.collider.gameObject.layer))
            {
                case "EnemyAABB":
                    GameObject gameObject = raycastHit.collider.gameObject.transform.root.gameObject;
                    TITAN titan = gameObject.GetComponent<TITAN>();
                    if (titan != null && !titan.hasDie)
                    {
                        titan.hitAnkle();
                        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER)
                        {
                            titan.photonView.RPC("hitAnkleRPC", PhotonTargets.Others, titan.photonView.ownerId);
                        }
                    }
                    explode();
                    break;
                case "Players":
                    GameObject gameObject2 = raycastHit.collider.gameObject.transform.root.gameObject;
                    TITAN_EREN eren = gameObject2.GetComponent<TITAN_EREN>();
                    if (eren != null)
                    {
                        if (!eren.isHit)
                        {
                            eren.hitByTitan();
                        }
                    }
                    else
                    {
                        HERO hero = gameObject2.GetComponent<HERO>();
                        if (hero != null && !hero.HasDied() && !hero.isInvincible() && !hero.isGrabbed)
                        {
                            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                            {
                                hero.die(v.normalized * 1000f + Vector3.up * 50f, isBite: false);
                            }
                            else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER)
                            {
                                hero.markDie();
                                int num = -1;
                                string text = "Rock";
                                EnemyfxIDcontainer efxIDContainer = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>();
                                if (efxIDContainer != null)
                                {
                                    num = efxIDContainer.myOwnerViewID;
                                    text = efxIDContainer.titanName;
                                }
                                hero.photonView.RPC("netDie", PhotonTargets.All, v.normalized * 1000f + Vector3.up * 50f, false, num, text, true);
                            }
                        }
                    }
                    break;
                case "Ground":
                    explode();
                    break;
            }
        }
        oldP = base.transform.position;
    }

    private void explode()
    {
        GameObject gameObject;
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
        {
            gameObject = PhotonNetwork.Instantiate("FX/boom6", base.transform.position, base.transform.rotation, 0);
            if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null)
            {
                gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
                gameObject.GetComponent<EnemyfxIDcontainer>().titanName = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
            }
        }
        else
        {
            gameObject = (GameObject)Object.Instantiate(Resources.Load("FX/boom6"), base.transform.position, base.transform.rotation);
        }
        gameObject.transform.localScale = base.transform.localScale;
        float b = 1f - Vector3.Distance(GameObject.Find("MainCamera").transform.position, gameObject.transform.position) * 0.05f;
        b = Mathf.Min(1f, b);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startShake(b, b);
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
        {
            Object.Destroy(base.gameObject);
        }
        else
        {
            PhotonNetwork.Destroy(base.photonView);
        }
    }

    public void launch(Vector3 v1)
    {
        launched = true;
        oldP = base.transform.position;
        v = v1;
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
        {
            base.photonView.RPC("launchRPC", PhotonTargets.Others, v, oldP);
        }
    }

    [RPC]
    private void launchRPC(Vector3 v, Vector3 p)
    {
        launched = true;
        base.transform.position = p;
        oldP = p;
        base.transform.parent = null;
        launch(v);
    }

    [RPC]
    private void initRPC(int viewID, Vector3 scale, Vector3 pos, float level)
    {
        GameObject gameObject = PhotonView.Find(viewID).gameObject;
        Transform parent = gameObject.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        base.transform.localScale = gameObject.transform.localScale;
        base.transform.parent = parent;
        base.transform.localPosition = pos;
    }
}
