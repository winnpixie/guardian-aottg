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
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
        {
            if (!base.transform.root.gameObject.GetPhotonView().isMine)
            {
                base.enabled = false;
                return;
            }

            EnemyfxIDcontainer efxId = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>();
            if (efxId != null)
            {
                viewID = efxId.myOwnerViewID;
                ownerName = efxId.titanName;
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

<<<<<<< HEAD
    private bool CheckIfBehind(GameObject titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
        Vector3 vector = base.transform.position - transform.transform.position;

=======
    private bool checkIfBehind(GameObject titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
        Vector3 vector = base.transform.position - transform.transform.position;
        Debug.DrawRay(transform.transform.position, -transform.transform.forward * 10f, Color.white, 5f);
        Debug.DrawRay(transform.transform.position, vector * 10f, Color.green, 5f);
>>>>>>> old-repo/master
        if (Vector3.Angle(-transform.transform.forward, vector) < 100f)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && !base.transform.root.gameObject.GetPhotonView().isMine) || !active_me)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
<<<<<<< HEAD
            case "playerHitbox": // Another player
=======
            case "playerHitbox":
>>>>>>> old-repo/master
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
                        }
                        else if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && !hero.HasDied() && !hero.isGrabbed)
                        {
                            hero.MarkDead();
                            hero.photonView.RPC("netDie", PhotonTargets.All, (hitbox.transform.root.position - base.transform.position).normalized * damage * 1000f + Vector3.up * 50f, false, viewID, ownerName, false);
                        }
                    }
                    break;
                }
<<<<<<< HEAD
            case "erenHitbox": // Eren, idk where his hitbox is LOL
                {
                    if (dmg > 0 && !other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().isHit)
                    {
                        other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().HitByTitan();
                    }
                    break;
                }
            case "titanneck": // Normal/Female/Colossal Titan nape
                {
                    HitBox hitbox = other.gameObject.GetComponent<HitBox>();
                    if (hitbox == null || !CheckIfBehind(hitbox.transform.root.gameObject) || currentHits.Contains(hitbox))
=======
            case "erenHitbox":
                {
                    if (dmg > 0 && !other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().isHit)
                    {
                        other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByTitan();
                    }
                    break;
                }
            case "titanneck":
                {
                    HitBox hitbox = other.gameObject.GetComponent<HitBox>();
                    if (hitbox == null || !checkIfBehind(hitbox.transform.root.gameObject) || currentHits.Contains(hitbox))
>>>>>>> old-repo/master
                    {
                        return;
                    }
                    hitbox.hitPosition = (base.transform.position + hitbox.transform.position) * 0.5f;
                    currentHits.Add(hitbox);

                    int damage = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - hitbox.transform.root.rigidbody.velocity).magnitude * 10f * scoreMulti);
                    damage = Mathf.Max(10, damage);

<<<<<<< HEAD
                    // TODO: Mod
                    if (damage < Guardian.Mod.Properties.LocalMinDamage.Value)
                    {
                        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ShowDamage(damage);
                        return;
                    }

=======
>>>>>>> old-repo/master
                    TITAN titan = hitbox.transform.root.GetComponent<TITAN>();
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                    {
                        if (titan != null && !titan.hasDie)
                        {
                            FengGameManagerMKII fgmkii = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
                            fgmkii.netShowDamage(damage);

                            if ((float)damage > titan.myLevel * 100f)
                            {
<<<<<<< HEAD
                                titan.Die();
=======
                                titan.die();
>>>>>>> old-repo/master
                                if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                                {
                                    currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(hitbox.transform.position, damage, hitbox.transform.root.gameObject, 0.02f);
                                }
                                fgmkii.UpdatePlayerKillInfo(damage);
                            }
                        }

                        return;
                    }

                    if (titan != null)
                    {
                        if (!titan.hasDie)
                        {
                            if ((float)damage > titan.myLevel * 100f)
                            {
                                if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
                                {
                                    currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(hitbox.transform.position, damage, hitbox.transform.root.gameObject, 0.02f);
                                    titan.asClientLookTarget = false;
                                }
                                titan.photonView.RPC("titanGetHit", titan.photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, damage);
                            }
                        }
                    }
                    else if ((bool)hitbox.transform.root.GetComponent<FEMALE_TITAN>())
                    {
                        FEMALE_TITAN ft = hitbox.transform.root.GetComponent<FEMALE_TITAN>();
                        if (!ft.hasDie)
                        {
                            ft.photonView.RPC("titanGetHit", ft.photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, damage);
                        }
                    }
                    else if ((bool)hitbox.transform.root.GetComponent<COLOSSAL_TITAN>() && !hitbox.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                    {
                        hitbox.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.RPC("titanGetHit", hitbox.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.owner, base.transform.root.gameObject.GetPhotonView().viewID, damage);
                    }

                    ShowCriticalHitFX(other.gameObject.transform.position);
                    break;
                }
<<<<<<< HEAD
            case "titaneye": // Titan/Female Titan eyes
=======
            case "titaneye":
>>>>>>> old-repo/master
                if (currentHits.Contains(other.gameObject))
                {
                    return;
                }
                currentHits.Add(other.gameObject);
                GameObject gameObject = other.gameObject.transform.root.gameObject;
                if ((bool)gameObject.GetComponent<FEMALE_TITAN>())
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                    {
                        if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject.GetComponent<FEMALE_TITAN>().hitEye();
                        }

                        return;
                    }

                    if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                    {
                        gameObject.GetComponent<FEMALE_TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                    }
                }
                else if (gameObject.GetComponent<TITAN>().abnormalType != TitanClass.Crawler)
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                    {
                        if (!gameObject.GetComponent<TITAN>().hasDie)
                        {
<<<<<<< HEAD
                            gameObject.GetComponent<TITAN>().HitEye();
=======
                            gameObject.GetComponent<TITAN>().hitEye();
>>>>>>> old-repo/master
                        }

                        return;
                    }

                    if (!gameObject.GetComponent<TITAN>().hasDie)
                    {
                        gameObject.GetComponent<TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                    }
                }

                ShowCriticalHitFX(other.gameObject.transform.position);
                break;
<<<<<<< HEAD
            case "titanankle": // Normal/Female Titan ankles
=======
            case "titanankle":
>>>>>>> old-repo/master
                {
                    if (currentHits.Contains(other.gameObject))
                    {
                        return;
                    }
                    currentHits.Add(other.gameObject);

                    GameObject gameObject2 = other.gameObject.transform.root.gameObject;

                    TITAN titan = gameObject2.GetComponent<TITAN>();
                    if (titan != null && titan.abnormalType != TitanClass.Crawler)
                    {
                        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                        {
                            if (!titan.hasDie)
                            {
<<<<<<< HEAD
                                titan.HitAnkle();
=======
                                titan.hitAnkle();
>>>>>>> old-repo/master
                            }

                            return;
                        }

                        if (!titan.hasDie)
                        {
                            titan.photonView.RPC("hitAnkleRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                    }
                    else if (gameObject2.GetComponent<FEMALE_TITAN>())
                    {
                        int damage = (int)((currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity - gameObject2.rigidbody.velocity).magnitude * 10f * scoreMulti);
                        damage = Mathf.Max(10, damage);

<<<<<<< HEAD
                        // TODO: Mod
                        if (damage < Guardian.Mod.Properties.LocalMinDamage.Value)
                        {
                            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ShowDamage(damage);
                            return;
                        }

=======
>>>>>>> old-repo/master
                        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                        {
                            if (other.gameObject.name == "ankleR")
                            {
                                if ((bool)gameObject2.GetComponent<FEMALE_TITAN>() && !gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                                {
                                    gameObject2.GetComponent<FEMALE_TITAN>().hitAnkleR(damage);
                                }
                            }
                            else if ((bool)gameObject2.GetComponent<FEMALE_TITAN>() && !gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                gameObject2.GetComponent<FEMALE_TITAN>().hitAnkleL(damage);
                            }

                            return;
                        }

                        if (other.gameObject.name == "ankleR")
                        {
                            if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                gameObject2.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID, damage);
                            }
                        }
                        else if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject2.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, base.transform.root.gameObject.GetPhotonView().viewID, damage);
                        }
                    }

                    ShowCriticalHitFX(other.gameObject.transform.position);
                    break;
                }
        }
    }

    private void ShowCriticalHitFX(Vector3 position)
    {
        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartShake(0.2f, 0.3f);
        GameObject gameObject = (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer) ? ((GameObject)Object.Instantiate(Resources.Load("redCross1"))) : PhotonNetwork.Instantiate("redCross1", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        gameObject.transform.position = position;
    }
}
