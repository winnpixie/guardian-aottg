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
        {
            if (Guardian.Utilities.Gesources.TryGetAsset("Custom/Audio/titan_die.wav", out AudioClip deathClip))
            {
                Object.Destroy(base.gameObject.GetComponent<AudioSource>());
                meatDie = gameObject.AddComponent<AudioSource>();
                meatDie.clip = deathClip;
            }
        }

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
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartShake(0.1f, 0.1f);

            if (other.gameObject.transform.root.gameObject.tag == "titan")
            {
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<HERO>().slashHit.Play();
                GameObject gameObject = (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer) ?
                    ((GameObject)Object.Instantiate(Resources.Load("hitMeat"))) : PhotonNetwork.Instantiate("hitMeat", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
                gameObject.transform.position = base.transform.position;
                base.transform.root.GetComponent<HERO>().UseBlade();
            }
        }

        switch (other.gameObject.tag)
        {
            case "playerHitbox": // Another player
                {
                    if (!FengGameManagerMKII.Level.PVP)
                    {
                        return;
                    }

                    HitBox hitbox = other.gameObject.GetComponent<HitBox>();
                    if (hitbox == null || hitbox.transform.root == null)
                    {
                        return;
                    }

                    HERO hero = hitbox.transform.root.GetComponent<HERO>();
                    if (hero != null && hero.myTeam != myTeam && !hero.IsInvincible())
                    {
                        float damage = 1f - Vector3.Distance(other.gameObject.transform.position, base.transform.position) * 0.05f;
                        damage = Mathf.Min(1f, damage);

                        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                        {
                            if (!hero.isGrabbed)
                            {
                                hero.Die((hitbox.transform.root.transform.position - base.transform.position).normalized * damage * 1000f + Vector3.up * 50f, isBite: false);
                            }

                            return;
                        }

                        if (!hero.HasDied() && !hero.isGrabbed)
                        {
                            if (PlayerPrefs.GetInt("EnableSS", 0) == 1)
                            {
                                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartSnapshot2(hitbox.transform.position, 0, hitbox.transform.root.gameObject, 0.02f);
                            }
                            hero.MarkDead();
                            hero.photonView.RPC("netDie", PhotonTargets.All, (hitbox.transform.root.position - base.transform.position).normalized * damage * 1000f + Vector3.up * 50f, false, base.transform.root.gameObject.GetPhotonView().viewID, PhotonView.Find(base.transform.root.gameObject.GetPhotonView().viewID).owner.customProperties[PhotonPlayerProperty.Name], false);
                        }
                    }
                    break;
                }
            case "titanneck": // Normal/Female/Colossal Titan nape
                {
                    HitBox hitbox = other.gameObject.GetComponent<HitBox>();
                    if (hitbox == null || !checkIfBehind(hitbox.transform.root.gameObject) || currentHits.Contains(hitbox))
                    {
                        return;
                    }
                    hitbox.hitPosition = (base.transform.position + hitbox.transform.position) * 0.5f;
                    currentHits.Add(hitbox);
                    meatDie.Play();

                    int damage = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - hitbox.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                    damage = Mathf.Max(10, damage);

                    // TODO: Mod
                    if (damage < Guardian.Mod.Properties.LocalMinDamage.Value)
                    {
                        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ShowDamage(damage);
                        return;
                    }

                    TITAN titan = hitbox.transform.root.GetComponent<TITAN>();
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                    {
                        if (titan != null && !titan.hasDie)
                        {
                            if (PlayerPrefs.GetInt("EnableSS", 0) == 1)
                            {
                                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartSnapshot2(hitbox.transform.position, damage, hitbox.transform.root.gameObject, 0.02f);
                            }
                            titan.Die();
                            SpawnNapeMeat(currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, hitbox.transform.root);
                            FengGameManagerMKII fengGame = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
                            fengGame.netShowDamage(damage);
                            fengGame.UpdatePlayerKillInfo(damage);
                        }

                        return;
                    }

                    if (titan != null)
                    {
                        if (!titan.hasDie)
                        {
                            if (PlayerPrefs.GetInt("EnableSS", 0) == 1)
                            {
                                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartSnapshot2(hitbox.transform.position, damage, hitbox.transform.root.gameObject, 0.02f);
                            }
                            titan.photonView.RPC("titanGetHit", titan.photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, damage);

                            if (Guardian.Mod.Properties.MultiplayerNapeMeat.Value)
                            {
                                SpawnNapeMeat(currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, hitbox.transform.root);
                            }
                        }
                    }
                    else if ((bool)hitbox.transform.root.GetComponent<FEMALE_TITAN>())
                    {
                        base.transform.root.GetComponent<HERO>().UseBlade(int.MaxValue);

                        if (!hitbox.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            hitbox.transform.root.GetComponent<FEMALE_TITAN>().photonView.RPC("titanGetHit", hitbox.transform.root.GetComponent<FEMALE_TITAN>().photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, damage);
                        }
                    }
                    else if ((bool)hitbox.transform.root.GetComponent<COLOSSAL_TITAN>())
                    {
                        base.transform.root.GetComponent<HERO>().UseBlade(int.MaxValue);

                        if (!hitbox.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                        {
                            hitbox.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.RPC("titanGetHit", hitbox.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, damage);
                        }
                    }

                    ShowCriticalHitFX();
                    break;
                }
            case "titaneye": // Titan/Female Titan eyes
                if (currentHits.Contains(other.gameObject))
                {
                    return;
                }
                currentHits.Add(other.gameObject);

                GameObject gameObject2 = other.gameObject.transform.root.gameObject;
                if ((bool)gameObject2.GetComponent<FEMALE_TITAN>())
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                    {
                        if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<FEMALE_TITAN>().hitEye();
                        }

                        return;
                    }

                    if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                    {
                        gameObject2.GetComponent<FEMALE_TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                    }
                }
                else if (gameObject2.GetComponent<TITAN>().abnormalType != TitanClass.Crawler)
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                    {
                        if (!gameObject2.GetComponent<TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<TITAN>().HitEye();
                        }

                        return;
                    }

                    if (!gameObject2.GetComponent<TITAN>().hasDie)
                    {
                        gameObject2.GetComponent<TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                    }
                }

                ShowCriticalHitFX();
                break;
            case "titanankle": // Normal/Female Titan ankles
                {
                    if (currentHits.Contains(other.gameObject))
                    {
                        return;
                    }
                    currentHits.Add(other.gameObject);

                    GameObject gameObject3 = other.gameObject.transform.root.gameObject;

                    TITAN titan = gameObject3.GetComponent<TITAN>();
                    if (titan != null && titan.abnormalType != TitanClass.Crawler)
                    {
                        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                        {
                            if (!titan.hasDie)
                            {
                                titan.HitAnkle();
                            }

                            return;
                        }

                        if (!titan.hasDie)
                        {
                            titan.photonView.RPC("hitAnkleRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                    }
                    else if (gameObject3.GetComponent<FEMALE_TITAN>())
                    {
                        int damage = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - gameObject3.rigidbody.velocity).magnitude * 10f * scoreMulti);
                        damage = Mathf.Max(10, damage);

                        // TODO: Mod
                        if (damage < Guardian.Mod.Properties.LocalMinDamage.Value)
                        {
                            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ShowDamage(damage);
                            return;
                        }

                        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                        {
                            if (other.gameObject.name == "ankleR")
                            {
                                if ((bool)gameObject3.GetComponent<FEMALE_TITAN>() && !gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
                                {
                                    gameObject3.GetComponent<FEMALE_TITAN>().hitAnkleR(damage);
                                }
                            }
                            else if ((bool)gameObject3.GetComponent<FEMALE_TITAN>() && !gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                gameObject3.GetComponent<FEMALE_TITAN>().hitAnkleL(damage);
                            }

                            return;
                        }

                        if (other.gameObject.name == "ankleR")
                        {
                            if (!gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                gameObject3.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID, damage);
                            }
                        }
                        else if (!gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject3.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID, damage);
                        }
                    }

                    ShowCriticalHitFX();
                    break;
                }
        }
    }


    private void SpawnNapeMeat(Vector3 vkill, Transform titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
        GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("titanNapeMeat"), transform.position, transform.rotation);
        gameObject.transform.localScale = titan.localScale;
        gameObject.rigidbody.AddForce(vkill.normalized * 15f, ForceMode.Impulse);
        gameObject.rigidbody.AddForce(-titan.forward * 10f, ForceMode.Impulse);
        gameObject.rigidbody.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), ForceMode.Impulse);
    }

    private void ShowCriticalHitFX()
    {
        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartShake(0.2f, 0.3f);
        GameObject gameObject = (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer) ? ((GameObject)Object.Instantiate(Resources.Load("redCross"))) : PhotonNetwork.Instantiate("redCross", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        gameObject.transform.position = base.transform.position;
    }

    public void ClearHits()
    {
        currentHitsII = new ArrayList();
        currentHits = new ArrayList();
    }
}
