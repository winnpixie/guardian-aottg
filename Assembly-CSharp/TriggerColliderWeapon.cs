using System.Collections;
using UnityEngine;

public class TriggerColliderWeapon : MonoBehaviour
{
    public bool active_me;
    public GameObject currentCamera;
    public ArrayList currentHits = new ArrayList();
    public ArrayList currentHitsII = new ArrayList();
    public AudioSource meatDie;
    public float scoreMulti = 1f;
    public int myTeam = 1;

    private void Start()
    {
        currentCamera = GameObject.Find("MainCamera");
    }

    private bool checkIfBehind(GameObject titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
        Vector3 to = base.transform.position - transform.transform.position;
        if (Vector3.Angle(-transform.transform.forward, to) < 70f)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!active_me)
        {
            return;
        }
        if (!currentHitsII.Contains(other.gameObject))
        {
            currentHitsII.Add(other.gameObject);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.1f, 0.1f);
            if (other.gameObject.transform.root.gameObject.tag == "titan")
            {
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<HERO>().slashHit.Play();
                GameObject gameObject = (IN_GAME_MAIN_CAMERA.Gametype == GameType.SINGLE) ? ((GameObject)Object.Instantiate(Resources.Load("hitMeat"))) : PhotonNetwork.Instantiate("hitMeat", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
                gameObject.transform.position = base.transform.position;
                base.transform.root.GetComponent<HERO>().useBlade();
            }
        }
        switch (other.gameObject.tag)
        {
            case "playerHitbox":
                if (!FengGameManagerMKII.Level.PVP)
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
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.SINGLE)
                {
                    if (!component.transform.root.GetComponent<HERO>().isGrabbed)
                    {
                        component.transform.root.GetComponent<HERO>().die((component.transform.root.transform.position - base.transform.position).normalized * b * 1000f + Vector3.up * 50f, isBite: false);
                    }
                }
                else if (IN_GAME_MAIN_CAMERA.Gametype == GameType.MULTIPLAYER && !component.transform.root.GetComponent<HERO>().HasDied() && !component.transform.root.GetComponent<HERO>().isGrabbed)
                {
                    if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                    {
                        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(component.transform.position, 0, component.transform.root.gameObject, 0.02f);
                    }
                    component.transform.root.GetComponent<HERO>().markDie();
                    component.transform.root.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, (component.transform.root.position - base.transform.position).normalized * b * 1000f + Vector3.up * 50f, false, base.transform.root.gameObject.GetPhotonView().viewID, PhotonView.Find(base.transform.root.gameObject.GetPhotonView().viewID).owner.customProperties[PhotonPlayerProperty.Name], false);
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
                meatDie.Play();
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.SINGLE)
                {
                    if ((bool)component2.transform.root.GetComponent<TITAN>() && !component2.transform.root.GetComponent<TITAN>().hasDie)
                    {
                        int b2 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                        b2 = Mathf.Max(10, b2);
                        if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                        {
                            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(component2.transform.position, b2, component2.transform.root.gameObject, 0.02f);
                        }
                        component2.transform.root.GetComponent<TITAN>().die();
                        napeMeat(currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, component2.transform.root);
                        FengGameManagerMKII fengGame = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
                        fengGame.netShowDamage(b2);
                        fengGame.UpdatePlayerKillInfo(b2);
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
                            if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                            {
                                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(component2.transform.position, b3, component2.transform.root.gameObject, 0.02f);
                                component2.transform.root.GetComponent<TITAN>().asClientLookTarget = false;
                            }
                            component2.transform.root.GetComponent<TITAN>().photonView.RPC("titanGetHit", component2.transform.root.GetComponent<TITAN>().photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, b3);
                        }
                    }
                    else if ((bool)component2.transform.root.GetComponent<FEMALE_TITAN>())
                    {
                        base.transform.root.GetComponent<HERO>().useBlade(int.MaxValue);
                        int b4 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                        b4 = Mathf.Max(10, b4);
                        if (!component2.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            component2.transform.root.GetComponent<FEMALE_TITAN>().photonView.RPC("titanGetHit", component2.transform.root.GetComponent<FEMALE_TITAN>().photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, b4);
                        }
                    }
                    else if ((bool)component2.transform.root.GetComponent<COLOSSAL_TITAN>())
                    {
                        base.transform.root.GetComponent<HERO>().useBlade(int.MaxValue);
                        if (!component2.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                        {
                            int b5 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                            b5 = Mathf.Max(10, b5);
                            component2.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.RPC("titanGetHit", component2.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, b5);
                        }
                    }
                }
                else if ((bool)component2.transform.root.GetComponent<TITAN>())
                {
                    if (!component2.transform.root.GetComponent<TITAN>().hasDie)
                    {
                        int b6 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                        b6 = Mathf.Max(10, b6);
                        if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                        {
                            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(component2.transform.position, b6, component2.transform.root.gameObject, 0.02f);
                        }
                        component2.transform.root.GetComponent<TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, b6);
                    }
                }
                else if ((bool)component2.transform.root.GetComponent<FEMALE_TITAN>())
                {
                    base.transform.root.GetComponent<HERO>().useBlade(int.MaxValue);
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
                else if ((bool)component2.transform.root.GetComponent<COLOSSAL_TITAN>())
                {
                    base.transform.root.GetComponent<HERO>().useBlade(int.MaxValue);
                    if (!component2.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                    {
                        int b8 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - component2.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                        b8 = Mathf.Max(10, b8);
                        if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                        {
                            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(component2.transform.position, b8, null, 0.02f);
                        }
                        component2.transform.root.GetComponent<COLOSSAL_TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, b8);
                    }
                }
                showCriticalHitFX();
                break;
            case "titaneye":
                if (currentHits.Contains(other.gameObject))
                {
                    return;
                }
                currentHits.Add(other.gameObject);
                GameObject gameObject2 = other.gameObject.transform.root.gameObject;
                if ((bool)gameObject2.GetComponent<FEMALE_TITAN>())
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.SINGLE)
                    {
                        if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<FEMALE_TITAN>().hitEye();
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<FEMALE_TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                    }
                    else if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                    {
                        gameObject2.GetComponent<FEMALE_TITAN>().hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
                    }
                }
                else
                {
                    if (gameObject2.GetComponent<TITAN>().abnormalType == AbnormalType.TYPE_CRAWLER)
                    {
                        return;
                    }
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.SINGLE)
                    {
                        if (!gameObject2.GetComponent<TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<TITAN>().hitEye();
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (!gameObject2.GetComponent<TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                    }
                    else if (!gameObject2.GetComponent<TITAN>().hasDie)
                    {
                        gameObject2.GetComponent<TITAN>().hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
                    }
                    showCriticalHitFX();
                }
                break;
            case "titanankle":
                if (currentHits.Contains(other.gameObject))
                {
                    return;
                }
                currentHits.Add(other.gameObject);
                GameObject gameObject3 = other.gameObject.transform.root.gameObject;
                int b9 = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - gameObject3.rigidbody.velocity).magnitude * 10f * scoreMulti);
                b9 = Mathf.Max(10, b9);
                if ((bool)gameObject3.GetComponent<TITAN>() && gameObject3.GetComponent<TITAN>().abnormalType != AbnormalType.TYPE_CRAWLER)
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.SINGLE)
                    {
                        if (!gameObject3.GetComponent<TITAN>().hasDie)
                        {
                            gameObject3.GetComponent<TITAN>().hitAnkle();
                        }
                        return;
                    }
                    if (!PhotonNetwork.isMasterClient)
                    {
                        if (!gameObject3.GetComponent<TITAN>().hasDie)
                        {
                            gameObject3.GetComponent<TITAN>().photonView.RPC("hitAnkleRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                    }
                    else if (!gameObject3.GetComponent<TITAN>().hasDie)
                    {
                        gameObject3.GetComponent<TITAN>().hitAnkle();
                    }
                    showCriticalHitFX();
                }
                else
                {
                    if (!gameObject3.GetComponent<FEMALE_TITAN>())
                    {
                        return;
                    }
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.SINGLE)
                    {
                        if (other.gameObject.name == "ankleR")
                        {
                            if ((bool)gameObject3.GetComponent<FEMALE_TITAN>() && !gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                gameObject3.GetComponent<FEMALE_TITAN>().hitAnkleR(b9);
                            }
                        }
                        else if ((bool)gameObject3.GetComponent<FEMALE_TITAN>() && !gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject3.GetComponent<FEMALE_TITAN>().hitAnkleL(b9);
                        }
                    }
                    else if (other.gameObject.name == "ankleR")
                    {
                        if (!PhotonNetwork.isMasterClient)
                        {
                            if (!gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                gameObject3.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID, b9);
                            }
                        }
                        else if (!gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject3.GetComponent<FEMALE_TITAN>().hitAnkleRRPC(base.transform.root.gameObject.GetPhotonView().viewID, b9);
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (!gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject3.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID, b9);
                        }
                    }
                    else if (!gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
                    {
                        gameObject3.GetComponent<FEMALE_TITAN>().hitAnkleLRPC(base.transform.root.gameObject.GetPhotonView().viewID, b9);
                    }
                    showCriticalHitFX();
                }
                break;
        }
    }


    private void napeMeat(Vector3 vkill, Transform titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
        GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("titanNapeMeat"), transform.position, transform.rotation);
        gameObject.transform.localScale = titan.localScale;
        gameObject.rigidbody.AddForce(vkill.normalized * 15f, ForceMode.Impulse);
        gameObject.rigidbody.AddForce(-titan.forward * 10f, ForceMode.Impulse);
        gameObject.rigidbody.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), ForceMode.Impulse);
    }

    private void showCriticalHitFX()
    {
        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f);
        GameObject gameObject = (IN_GAME_MAIN_CAMERA.Gametype == GameType.SINGLE) ? ((GameObject)Object.Instantiate(Resources.Load("redCross"))) : PhotonNetwork.Instantiate("redCross", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        gameObject.transform.position = base.transform.position;
    }

    public void clearHits()
    {
        currentHitsII = new ArrayList();
        currentHits = new ArrayList();
    }
}
