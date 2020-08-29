using System.Collections;
using UnityEngine;

public class AHSSShotGunCollider : MonoBehaviour
{
    public bool active_me;
    public int dmg = 1;
    private int count;
    public ArrayList currentHits = new ArrayList();
    public GameObject currentCamera;
    private int myTeam = 1;
    public float scoreMulti;
    private int viewID = -1;
    private string ownerName = string.Empty;

    private void Start()
    {
        currentCamera = GameObject.Find("MainCamera");
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER)
        {
            if (!base.transform.root.gameObject.GetPhotonView().isMine)
            {
                base.enabled = false;
                return;
            }
            if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null)
            {
                viewID = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
                ownerName = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
                myTeam = PhotonView.Find(viewID).gameObject.GetComponent<HERO>().myTeam;
            }
        }
        else
        {
            myTeam = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<HERO>().myTeam;
        }
        active_me = true;
        count = 0;
    }

    private void FixedUpdate()
    {
        if (count > 1)
        {
            active_me = false;
        }
        else
        {
            count++;
        }
    }

    private bool checkIfBehind(GameObject titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
        Vector3 vector = base.transform.position - transform.transform.position;
        Debug.DrawRay(transform.transform.position, -transform.transform.forward * 10f, Color.white, 5f);
        Debug.DrawRay(transform.transform.position, vector * 10f, Color.green, 5f);
        if (Vector3.Angle(-transform.transform.forward, vector) < 100f)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && !base.transform.root.gameObject.GetPhotonView().isMine) || !active_me)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "playerHitbox":
                if (!LevelInfo.GetInfo(FengGameManagerMKII.level).pvp)
                {
                    return;
                }
                float b = 1f - Vector3.Distance(other.gameObject.transform.position, base.transform.position) * 0.05f;
                b = Mathf.Min(1f, b);
                HitBox component = other.gameObject.GetComponent<HitBox>();
                if (!(component != null) || !(component.transform.root != null) || component.transform.root.GetComponent<HERO>().myTeam == myTeam || component.transform.root.GetComponent<HERO>().isInvincible())
                {
                    return;
                }
                if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                {
                    if (!component.transform.root.GetComponent<HERO>().isGrabbed)
                    {
                        component.transform.root.GetComponent<HERO>().die((component.transform.root.transform.position - base.transform.position).normalized * b * 1000f + Vector3.up * 50f, isBite: false);
                    }
                }
                else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && !component.transform.root.GetComponent<HERO>().HasDied() && !component.transform.root.GetComponent<HERO>().isGrabbed)
                {
                    component.transform.root.GetComponent<HERO>().markDie();
                    component.transform.root.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, (component.transform.root.position - base.transform.position).normalized * b * 1000f + Vector3.up * 50f, false, viewID, ownerName, false);
                }
                break;
            case "erenHitbox":
                if (dmg > 0 && !other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().isHit)
                {
                    other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByTitan();
                }
                break;
            case "titanneck":
                HitBox component2 = other.gameObject.GetComponent<HitBox>();
                if (!(component2 != null) || !checkIfBehind(component2.transform.root.gameObject) || currentHits.Contains(component2))
                {
                    return;
                }
                component2.hitPosition = (base.transform.position + component2.transform.position) * 0.5f;
                currentHits.Add(component2);
                if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                {
                    if ((bool)component2.transform.root.GetComponent<TITAN>() && !component2.transform.root.GetComponent<TITAN>().hasDie)
                    {
                        int b2 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                        b2 = Mathf.Max(10, b2);
                        FengGameManagerMKII fgmkii = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
                        fgmkii.netShowDamage(b2);
                        if ((float)b2 > component2.transform.root.GetComponent<TITAN>().myLevel * 100f)
                        {
                            component2.transform.root.GetComponent<TITAN>().die();
                            if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                            {
                                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(component2.transform.position, b2, component2.transform.root.gameObject, 0.02f);
                            }
                            fgmkii.UpdatePlayerKillInfo(b2);
                        }
                    }
                }
                else if (!PhotonNetwork.isMasterClient)
                {
                    if ((bool)component2.transform.root.GetComponent<TITAN>())
                    {
                        if (!component2.transform.root.GetComponent<TITAN>().hasDie)
                        {
                            int b3 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                            b3 = Mathf.Max(10, b3);
                            if ((float)b3 > component2.transform.root.GetComponent<TITAN>().myLevel * 100f)
                            {
                                if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                                {
                                    currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(component2.transform.position, b3, component2.transform.root.gameObject, 0.02f);
                                    component2.transform.root.GetComponent<TITAN>().asClientLookTarget = false;
                                }
                                component2.transform.root.GetComponent<TITAN>().photonView.RPC("titanGetHit", component2.transform.root.GetComponent<TITAN>().photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, b3);
                            }
                        }
                    }
                    else if ((bool)component2.transform.root.GetComponent<FEMALE_TITAN>())
                    {
                        int b4 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                        b4 = Mathf.Max(10, b4);
                        if (!component2.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            component2.transform.root.GetComponent<FEMALE_TITAN>().photonView.RPC("titanGetHit", component2.transform.root.GetComponent<FEMALE_TITAN>().photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, b4);
                        }
                    }
                    else if ((bool)component2.transform.root.GetComponent<COLOSSAL_TITAN>() && !component2.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                    {
                        int b5 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                        b5 = Mathf.Max(10, b5);
                        component2.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.RPC("titanGetHit", component2.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, b5);
                    }
                }
                else if ((bool)component2.transform.root.GetComponent<TITAN>())
                {
                    if (!component2.transform.root.GetComponent<TITAN>().hasDie)
                    {
                        int b6 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                        b6 = Mathf.Max(10, b6);
                        if ((float)b6 > component2.transform.root.GetComponent<TITAN>().myLevel * 100f)
                        {
                            if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                            {
                                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(component2.transform.position, b6, component2.transform.root.gameObject, 0.02f);
                            }
                            component2.transform.root.GetComponent<TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, b6);
                        }
                    }
                }
                else if ((bool)component2.transform.root.GetComponent<FEMALE_TITAN>())
                {
                    if (!component2.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                    {
                        int b7 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                        b7 = Mathf.Max(10, b7);
                        if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                        {
                            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(component2.transform.position, b7, null, 0.02f);
                        }
                        component2.transform.root.GetComponent<FEMALE_TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, b7);
                    }
                }
                else if ((bool)component2.transform.root.GetComponent<COLOSSAL_TITAN>() && !component2.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                {
                    int b8 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                    b8 = Mathf.Max(10, b8);
                    if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                    {
                        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(component2.transform.position, b8, null, 0.02f);
                    }
                    component2.transform.root.GetComponent<COLOSSAL_TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, b8);
                }
                showCriticalHitFX(other.gameObject.transform.position);
                break;
            case "titaneye":
                if (currentHits.Contains(other.gameObject))
                {
                    return;
                }
                currentHits.Add(other.gameObject);
                GameObject gameObject = other.gameObject.transform.root.gameObject;
                if ((bool)gameObject.GetComponent<FEMALE_TITAN>())
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                    {
                        if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject.GetComponent<FEMALE_TITAN>().hitEye();
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject.GetComponent<FEMALE_TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                    }
                    else if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                    {
                        gameObject.GetComponent<FEMALE_TITAN>().hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
                    }
                }
                else
                {
                    if (gameObject.GetComponent<TITAN>().abnormalType == AbnormalType.TYPE_CRAWLER)
                    {
                        return;
                    }
                    if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                    {
                        if (!gameObject.GetComponent<TITAN>().hasDie)
                        {
                            gameObject.GetComponent<TITAN>().hitEye();
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (!gameObject.GetComponent<TITAN>().hasDie)
                        {
                            gameObject.GetComponent<TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                    }
                    else if (!gameObject.GetComponent<TITAN>().hasDie)
                    {
                        gameObject.GetComponent<TITAN>().hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
                    }
                    showCriticalHitFX(other.gameObject.transform.position);
                }
                break;
            case "titanankle":
                if (currentHits.Contains(other.gameObject))
                {
                    return;
                }
                currentHits.Add(other.gameObject);
                GameObject gameObject2 = other.gameObject.transform.root.gameObject;
                int b9 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - gameObject2.rigidbody.velocity).magnitude * 10f * scoreMulti);
                b9 = Mathf.Max(10, b9);
                if ((bool)gameObject2.GetComponent<TITAN>() && gameObject2.GetComponent<TITAN>().abnormalType != AbnormalType.TYPE_CRAWLER)
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                    {
                        if (!gameObject2.GetComponent<TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<TITAN>().hitAnkle();
                        }
                        return;
                    }
                    if (!PhotonNetwork.isMasterClient)
                    {
                        if (!gameObject2.GetComponent<TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<TITAN>().photonView.RPC("hitAnkleRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                    }
                    else if (!gameObject2.GetComponent<TITAN>().hasDie)
                    {
                        gameObject2.GetComponent<TITAN>().hitAnkle();
                    }
                    showCriticalHitFX(other.gameObject.transform.position);
                }
                else
                {
                    if (!gameObject2.GetComponent<FEMALE_TITAN>())
                    {
                        return;
                    }
                    if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                    {
                        if (other.gameObject.name == "ankleR")
                        {
                            if ((bool)gameObject2.GetComponent<FEMALE_TITAN>() && !gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                gameObject2.GetComponent<FEMALE_TITAN>().hitAnkleR(b9);
                            }
                        }
                        else if ((bool)gameObject2.GetComponent<FEMALE_TITAN>() && !gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<FEMALE_TITAN>().hitAnkleL(b9);
                        }
                    }
                    else if (other.gameObject.name == "ankleR")
                    {
                        if (!PhotonNetwork.isMasterClient)
                        {
                            if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                gameObject2.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID, b9);
                            }
                        }
                        else if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<FEMALE_TITAN>().hitAnkleRRPC(base.transform.root.gameObject.GetPhotonView().viewID, b9);
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID, b9);
                        }
                    }
                    else if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                    {
                        gameObject2.GetComponent<FEMALE_TITAN>().hitAnkleLRPC(base.transform.root.gameObject.GetPhotonView().viewID, b9);
                    }
                    showCriticalHitFX(other.gameObject.transform.position);
                }
                break;
        }
    }

    private void showCriticalHitFX(Vector3 position)
    {
        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f);
        GameObject gameObject = (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE) ? ((GameObject)Object.Instantiate(Resources.Load("redCross1"))) : PhotonNetwork.Instantiate("redCross1", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        gameObject.transform.position = position;
    }
}
