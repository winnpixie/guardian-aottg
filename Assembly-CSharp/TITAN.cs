using System;
using System.Collections;
using System.Collections.Generic;
using Guardian;
using Guardian.Utilities;
using UnityEngine;

public class TITAN : Photon.MonoBehaviour
{
    public bool hasDie;
    public GameObject myHero;
    private TitanState state;
    public float speed = 7f;
    private float gravity = 120f;
    public float maxVelocityChange = 10f;
    public GameObject currentCamera;
    public float chaseDistance = 80f;
    public float attackDistance = 13f;
    public float attackWait = 1f;
    public float myDistance;
    public static float minusDistance = 99999f;
    public static GameObject minusDistanceEnemy;
    public float myLevel = 1f;
    public bool isAlarm;
    public AbnormalType abnormalType;
    private Vector3 oldCorePosition;
    private int attackCount;
    private float attackEndWait;
    private Vector3 abnorma_jump_bite_horizon_v;
    private float hitPause;
    public int activeRad = int.MaxValue;
    private Vector3 spawnPt;
    public ArrayList checkPoints = new ArrayList();
    private Vector3 targetCheckPt;
    private float targetR;
    private float angle;
    private float between2;
    private GameObject throwRock;
    public GROUP myGroup = GROUP.T;
    public PVPcheckPoint PVPfromCheckPt;
    public int myDifficulty;
    public TITAN_CONTROLLER controller;
    public bool nonAI;
    private float stamina = 320f;
    private float maxStamina = 320f;
    private string runAnimation;
    private Vector3 headscale = Vector3.one;
    public GameObject mainMaterial;
    private float tauntTime;
    private GameObject whoHasTauntMe;
    private bool hasDieSteam;
    private float rockInterval;
    private float random_run_time;
    private Transform head;
    private Transform neck;
    private float stuckTime;
    private bool stuck;
    private float stuckTurnAngle;
    private bool needFreshCorePosition;
    private int stepSoundPhase = 2;
    public bool asClientLookTarget;
    private Quaternion oldHeadRotation;
    private Quaternion targetHeadRotation;
    private bool grounded;
    private bool attacked;
    private string attackAnimation;
    private string hitAnimation;
    private string nextAttackAnimation;
    private bool isAttackMoveByCore;
    private string fxName;
    private Vector3 fxPosition;
    private Quaternion fxRotation;
    private float attackCheckTime;
    private float attackCheckTimeA;
    private float attackCheckTimeB;
    private Transform currentGrabHand;
    private bool isGrabHandLeft;
    public GameObject grabbedTarget;
    private bool nonAIcombo;
    private bool leftHandAttack;
    private float sbtime;
    private float turnDeg;
    private float desDeg;
    private string turnAnimation;
    public GameObject grabTF;
    private float getdownTime;
    private float dieTime;
    public bool hasExplode;
    public int currentHealth;
    public int maxHealth;
    public bool rockthrow;
    public int skin;
    public bool eye;
    public bool hasload;
    public GameObject healthLabel;
    public bool healthLabelEnabled;
    public float lagMax;
    public float healthTime;
    public bool colliderEnabled;
    public bool isHooked;
    public bool isLook;
    public TitanTrigger myTitanTrigger;
    public AudioSource baseAudioSource;
    public Animation baseAnimation;
    public Transform baseTransform;
    public Rigidbody baseRigidBody;
    public Transform baseGameObjectTransform;
    public List<Collider> baseColliders;
    public bool hasSetLevel;
    public bool hasSpawn;
    public int skinColor;
    private FengGameManagerMKII fengGame;

    private void playAnimation(string aniName)
    {
        base.animation.Play(aniName);
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
        {
            base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, aniName);
        }
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        base.animation.Play(aniName);
        base.animation[aniName].normalizedTime = normalizedTime;
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
        {
            base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, aniName, normalizedTime);
        }
    }

    public void crossFade(string aniName, float time)
    {
        base.animation.CrossFade(aniName, time);
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
        {
            base.photonView.RPC("netCrossFade", PhotonTargets.Others, aniName, time);
        }
    }

    [RPC]
    private void netPlayAnimation(string aniName, PhotonMessageInfo info)
    {
        if (!Guardian.AntiAbuse.TitanPatches.IsAnimationPlayValid(this, info) && PhotonNetwork.isMasterClient)
        {
            return;
        }
        base.animation.Play(aniName);
    }

    [RPC]
    private void netPlayAnimationAt(string aniName, float normalizedTime, PhotonMessageInfo info)
    {
        if (!Guardian.AntiAbuse.TitanPatches.IsAnimationSeekedPlayValid(this, info) && PhotonNetwork.isMasterClient)
        {
            return;
        }
        base.animation.Play(aniName);
        base.animation[aniName].normalizedTime = normalizedTime;
    }

    [RPC]
    private void netCrossFade(string aniName, float time, PhotonMessageInfo info)
    {
        if (!Guardian.AntiAbuse.TitanPatches.IsCrossFadeValid(this, info) && PhotonNetwork.isMasterClient)
        {
            return;
        }
        base.animation.CrossFade(aniName, time);
    }

    private int getPunkNumber()
    {
        int num = 0;
        GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
        foreach (GameObject gameObject in array)
        {
            if ((bool)gameObject.GetComponent<TITAN>() && gameObject.GetComponent<TITAN>().name == "Punk")
            {
                num++;
            }
        }
        return num;
    }

    [RPC]
    private void netSetAbnormalType(int type, PhotonMessageInfo info = null)
    {
        if (!Guardian.AntiAbuse.TitanPatches.IsTitanTypeSetValid(this, info) && PhotonNetwork.isMasterClient)
        {
            this.photonView.RPC("netSetAbnormalType", PhotonTargets.OthersBuffered, (int)abnormalType);
            return;
        }

        if (!hasload)
        {
            hasload = true;
            loadskin();
        }
        switch (type)
        {
            case 0:
                abnormalType = AbnormalType.NORMAL;
                base.name = "Titan";
                runAnimation = "run_walk";
                GetComponent<TITAN_SETUP>().setHair2();
                break;
            case 1:
                abnormalType = AbnormalType.TYPE_I;
                base.name = "Aberrant";
                runAnimation = "run_abnormal";
                GetComponent<TITAN_SETUP>().setHair2();
                break;
            case 2:
                abnormalType = AbnormalType.TYPE_JUMPER;
                base.name = "Jumper";
                runAnimation = "run_abnormal";
                GetComponent<TITAN_SETUP>().setHair2();
                break;
            case 3:
                abnormalType = AbnormalType.TYPE_CRAWLER;
                base.name = "Crawler";
                runAnimation = "crawler_run";
                GetComponent<TITAN_SETUP>().setHair2();
                break;
            case 4:
                abnormalType = AbnormalType.TYPE_PUNK;
                base.name = "Punk";
                runAnimation = "run_abnormal_1";
                GetComponent<TITAN_SETUP>().setHair2();
                break;
        }
        if (abnormalType == AbnormalType.TYPE_I || abnormalType == AbnormalType.TYPE_JUMPER || abnormalType == AbnormalType.TYPE_PUNK)
        {
            speed = 18f;
            if (myLevel > 1f)
            {
                speed *= Mathf.Sqrt(myLevel);
            }
            if (myDifficulty == 1)
            {
                speed *= 1.4f;
            }
            if (myDifficulty == 2)
            {
                speed *= 1.6f;
            }
            baseAnimation["turnaround1"].speed = 2f;
            baseAnimation["turnaround2"].speed = 2f;
        }
        if (abnormalType == AbnormalType.TYPE_CRAWLER)
        {
            chaseDistance += 50f;
            speed = 25f;
            if (myLevel > 1f)
            {
                speed *= Mathf.Sqrt(myLevel);
            }
            if (myDifficulty == 1)
            {
                speed *= 2f;
            }
            if (myDifficulty == 2)
            {
                speed *= 2.2f;
            }
            CapsuleCollider collider = baseTransform.Find("AABB").gameObject.GetComponent<CapsuleCollider>();
            collider.height = 10f;
            collider.radius = 5f;
            collider.center = new Vector3(0f, 5.05f, 0f);
        }
        if (nonAI)
        {
            if (abnormalType == AbnormalType.TYPE_CRAWLER)
            {
                speed = Mathf.Min(70f, speed);
            }
            else
            {
                speed = Mathf.Min(60f, speed);
            }
            baseAnimation["attack_jumper_0"].speed = 7f;
            baseAnimation["attack_crawler_jump_0"].speed = 4f;
        }
        baseAnimation["attack_combo_1"].speed = 1f;
        baseAnimation["attack_combo_2"].speed = 1f;
        baseAnimation["attack_combo_3"].speed = 1f;
        baseAnimation["attack_quick_turn_l"].speed = 1f;
        baseAnimation["attack_quick_turn_r"].speed = 1f;
        baseAnimation["attack_anti_AE_l"].speed = 1.1f;
        baseAnimation["attack_anti_AE_low_l"].speed = 1.1f;
        baseAnimation["attack_anti_AE_r"].speed = 1.1f;
        baseAnimation["attack_anti_AE_low_r"].speed = 1.1f;
        idle();
    }

    private void setmyLevel()
    {
        base.animation.cullingType = AnimationCullingType.BasedOnRenderers;
        if (IN_GAME_MAIN_CAMERA.Gametype != 0 && base.photonView.isMine)
        {
            base.photonView.RPC("netSetLevel", PhotonTargets.AllBuffered, myLevel, fengGame.difficulty, UnityEngine.Random.Range(0, 4));
            base.animation.cullingType = AnimationCullingType.AlwaysAnimate;
        }
        else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
        {
            setLevel2(myLevel, IN_GAME_MAIN_CAMERA.Difficulty, UnityEngine.Random.Range(0, 4));
        }
    }

    private void OnDestroy()
    {
        GameObject mm = GameObject.Find("MultiplayerManager");
        if (mm != null)
        {
            mm.GetComponent<FengGameManagerMKII>().RemoveTitan(this);
        }
    }

    [RPC]
    private void setMyTarget(int viewId, PhotonMessageInfo info)
    {
        if (!Guardian.AntiAbuse.TitanPatches.IsTargetSetValid(this, info) && PhotonNetwork.isMasterClient)
        {
            return;
        }

        if (viewId == -1)
        {
            myHero = null;
        }
        PhotonView photonView = PhotonView.Find(viewId);
        if (photonView != null)
        {
            myHero = photonView.gameObject;
        }
    }

    public bool IsGrounded()
    {
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyAABB");
        return Physics.Raycast(layerMask: ((LayerMask)((int)mask2 | (int)mask)).value, origin: base.gameObject.transform.position + Vector3.up * 0.1f, direction: -Vector3.up, distance: 0.3f);
    }

    private GameObject checkIfHitHand(Transform hand)
    {
        float num = 2.4f * myLevel;
        Collider[] array = Physics.OverlapSphere(hand.GetComponent<SphereCollider>().transform.position, num + 1f);
        Collider[] array2 = array;
        foreach (Collider collider in array2)
        {
            if (!(collider.transform.root.tag == "Player"))
            {
                continue;
            }
            GameObject gameObject = collider.transform.root.gameObject;
            if ((bool)gameObject.GetComponent<TITAN_EREN>())
            {
                if (!gameObject.GetComponent<TITAN_EREN>().isHit)
                {
                    gameObject.GetComponent<TITAN_EREN>().hitByTitan();
                }
            }
            else if ((bool)gameObject.GetComponent<HERO>() && !gameObject.GetComponent<HERO>().isInvincible())
            {
                return gameObject;
            }
        }
        return null;
    }

    private GameObject checkIfHitHead(Transform head, float rad)
    {
        float num = rad * myLevel;
        GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject gameObject in array)
        {
            if (!gameObject.GetComponent<TITAN_EREN>() && (!gameObject.GetComponent<HERO>() || !gameObject.GetComponent<HERO>().isInvincible()))
            {
                float num2 = gameObject.GetComponent<CapsuleCollider>().height * 0.5f;
                if (Vector3.Distance(gameObject.transform.position + Vector3.up * num2, head.position + Vector3.up * 1.5f * myLevel) < num + num2)
                {
                    return gameObject;
                }
            }
        }
        return null;
    }

    private GameObject checkIfHitCrawlerMouth(Transform head, float rad)
    {
        float num = rad * myLevel;
        GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject gameObject in array)
        {
            if (!gameObject.GetComponent<TITAN_EREN>() && (!gameObject.GetComponent<HERO>() || !gameObject.GetComponent<HERO>().isInvincible()))
            {
                float num2 = gameObject.GetComponent<CapsuleCollider>().height * 0.5f;
                if (Vector3.Distance(gameObject.transform.position + Vector3.up * num2, head.position - Vector3.up * 1.5f * myLevel) < num + num2)
                {
                    return gameObject;
                }
            }
        }
        return null;
    }

    public void beTauntedBy(GameObject target, float tauntTime)
    {
        whoHasTauntMe = target;
        this.tauntTime = tauntTime;
        isAlarm = true;
    }

    public void beLaughAttacked()
    {
        if (!hasDie && abnormalType != AbnormalType.TYPE_CRAWLER)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER)
            {
                base.photonView.RPC("laugh", PhotonTargets.All, 0f);
            }
            else if (state == TitanState.idle || state == TitanState.turn || state == TitanState.chase)
            {
                laugh();
            }
        }
    }

    [RPC]
    private void laugh(float sbtime = 0f)
    {
        if (state == TitanState.idle || state == TitanState.turn || state == TitanState.chase)
        {
            this.sbtime = sbtime;
            state = TitanState.laugh;
            crossFade("laugh", 0.2f);
        }
    }

    private string[] GetAttackStrategy()
    {
        string[] array = null;
        if (!isAlarm)
        {
            Vector3 position = myHero.transform.position;
            float num2 = position.y + 3f;
            Vector3 position2 = neck.position;
            if (num2 > position2.y + 10f * myLevel)
            {
                goto IL_0859;
            }
        }
        Vector3 position3 = myHero.transform.position;
        float y = position3.y;
        Vector3 position4 = neck.position;
        if (y > position4.y - 3f * myLevel)
        {
            if (myDistance < attackDistance * 0.5f)
            {
                if (Vector3.Distance(myHero.transform.position, base.transform.Find("chkOverHead").position) < 3.6f * myLevel)
                {
                    array = ((!(between2 > 0f)) ? new string[1]
                    {
                        "grab_head_front_l"
                    } : new string[1]
                    {
                        "grab_head_front_r"
                    });
                }
                else if (Mathf.Abs(between2) < 90f)
                {
                    if (Mathf.Abs(between2) < 30f)
                    {
                        if (Vector3.Distance(myHero.transform.position, base.transform.Find("chkFront").position) < 2.5f * myLevel)
                        {
                            array = new string[3]
                            {
                                "attack_bite",
                                "attack_bite",
                                "attack_slap_face"
                            };
                        }
                    }
                    else if (between2 > 0f)
                    {
                        if (Vector3.Distance(myHero.transform.position, base.transform.Find("chkFrontRight").position) < 2.5f * myLevel)
                        {
                            array = new string[1]
                            {
                                "attack_bite_r"
                            };
                        }
                    }
                    else if (Vector3.Distance(myHero.transform.position, base.transform.Find("chkFrontLeft").position) < 2.5f * myLevel)
                    {
                        array = new string[1]
                        {
                            "attack_bite_l"
                        };
                    }
                }
                else if (between2 > 0f)
                {
                    if (Vector3.Distance(myHero.transform.position, base.transform.Find("chkBackRight").position) < 2.8f * myLevel)
                    {
                        array = new string[3]
                        {
                            "grab_head_back_r",
                            "grab_head_back_r",
                            "attack_slap_back"
                        };
                    }
                }
                else if (Vector3.Distance(myHero.transform.position, base.transform.Find("chkBackLeft").position) < 2.8f * myLevel)
                {
                    array = new string[3]
                    {
                        "grab_head_back_l",
                        "grab_head_back_l",
                        "attack_slap_back"
                    };
                }
            }
            if (array == null)
            {
                switch (abnormalType)
                {
                    case AbnormalType.NORMAL:
                    case AbnormalType.TYPE_PUNK:
                        if ((myDifficulty > 0 || UnityEngine.Random.Range(0, 1000) < 3) && Mathf.Abs(between2) < 60f)
                        {
                            array = new string[1]
                            {
                            "attack_combo"
                            };
                        }
                        break;
                    case AbnormalType.TYPE_I:
                    case AbnormalType.TYPE_JUMPER:
                        if (myDifficulty > 0 || UnityEngine.Random.Range(0, 100) < 50)
                        {
                            array = new string[1]
                            {
                                "attack_abnormal_jump"
                            };
                        }
                        break;
                }
            }
        }
        else
        {
            switch ((Mathf.Abs(between2) < 90f) ? ((between2 > 0f) ? 1 : 2) : ((!(between2 > 0f)) ? 3 : 4))
            {
                case 2:
                    array = ((!(myDistance < attackDistance * 0.25f)) ? ((!(myDistance < attackDistance * 0.5f)) ? ((abnormalType != AbnormalType.TYPE_PUNK) ? ((abnormalType != 0) ? new string[1]
                    {
                    "attack_abnormal_jump"
                    } : ((myDifficulty <= 0) ? new string[5]
                    {
                    "attack_front_ground",
                    "attack_front_ground",
                    "attack_front_ground",
                    "attack_front_ground",
                    "attack_combo"
                    } : new string[3]
                    {
                    "attack_front_ground",
                    "attack_combo",
                    "attack_combo"
                    })) : new string[3]
                    {
                    "attack_combo",
                    "attack_combo",
                    "attack_abnormal_jump"
                    }) : ((abnormalType != AbnormalType.TYPE_PUNK) ? ((abnormalType != 0) ? new string[3]
                    {
                    "grab_ground_front_l",
                    "grab_ground_front_l",
                    "attack_abnormal_jump"
                    } : new string[3]
                    {
                    "grab_ground_front_l",
                    "grab_ground_front_l",
                    "attack_stomp"
                    }) : new string[3]
                    {
                    "grab_ground_front_l",
                    "grab_ground_front_l",
                    "attack_abnormal_jump"
                    })) : ((abnormalType != AbnormalType.TYPE_PUNK) ? ((abnormalType != 0) ? new string[1]
                    {
                    "attack_kick"
                    } : new string[2]
                    {
                    "attack_front_ground",
                    "attack_stomp"
                    }) : new string[2]
                    {
                    "attack_kick",
                    "attack_stomp"
                    }));
                    break;
                case 1:
                    array = ((!(myDistance < attackDistance * 0.25f)) ? ((!(myDistance < attackDistance * 0.5f)) ? ((abnormalType != AbnormalType.TYPE_PUNK) ? ((abnormalType != 0) ? new string[1]
                    {
                    "attack_abnormal_jump"
                    } : ((myDifficulty <= 0) ? new string[5]
                    {
                    "attack_front_ground",
                    "attack_front_ground",
                    "attack_front_ground",
                    "attack_front_ground",
                    "attack_combo"
                    } : new string[3]
                    {
                    "attack_front_ground",
                    "attack_combo",
                    "attack_combo"
                    })) : new string[3]
                    {
                    "attack_combo",
                    "attack_combo",
                    "attack_abnormal_jump"
                    }) : ((abnormalType != AbnormalType.TYPE_PUNK) ? ((abnormalType != 0) ? new string[3]
                    {
                    "grab_ground_front_r",
                    "grab_ground_front_r",
                    "attack_abnormal_jump"
                    } : new string[3]
                    {
                    "grab_ground_front_r",
                    "grab_ground_front_r",
                    "attack_stomp"
                    }) : new string[3]
                    {
                    "grab_ground_front_r",
                    "grab_ground_front_r",
                    "attack_abnormal_jump"
                    })) : ((abnormalType != AbnormalType.TYPE_PUNK) ? ((abnormalType != 0) ? new string[1]
                    {
                    "attack_kick"
                    } : new string[2]
                    {
                    "attack_front_ground",
                    "attack_stomp"
                    }) : new string[2]
                    {
                    "attack_kick",
                    "attack_stomp"
                    }));
                    break;
                case 3:
                    if (myDistance < attackDistance * 0.5f)
                    {
                        array = ((abnormalType != 0) ? new string[1]
                        {
                        "grab_ground_back_l"
                        } : new string[1]
                        {
                        "grab_ground_back_l"
                        });
                    }
                    break;
                case 4:
                    if (myDistance < attackDistance * 0.5f)
                    {
                        array = ((abnormalType != 0) ? new string[1]
                        {
                        "grab_ground_back_r"
                        } : new string[1]
                        {
                        "grab_ground_back_r"
                        });
                    }
                    break;
            }
        }
        goto IL_0859;
    IL_0859:
        return array;
    }

    private bool simpleHitTestLineAndBall(Vector3 line, Vector3 ball, float R)
    {
        Vector3 vector = Vector3.Project(ball, line);
        if ((ball - vector).magnitude > R)
        {
            return false;
        }
        if (Vector3.Dot(line, vector) < 0f)
        {
            return false;
        }
        if (vector.sqrMagnitude > line.sqrMagnitude)
        {
            return false;
        }
        return true;
    }

    public void setRoute(GameObject route)
    {
        checkPoints = new ArrayList();
        for (int i = 1; i <= 10; i++)
        {
            checkPoints.Add(route.transform.Find("r" + i).position);
        }
        checkPoints.Add("end");
    }

    public void toCheckPoint(Vector3 targetPt, float r)
    {
        state = TitanState.to_check_point;
        targetCheckPt = targetPt;
        targetR = r;
        crossFade(runAnimation, 0.5f);
    }

    public void randomRun(Vector3 targetPt, float r)
    {
        state = TitanState.random_run;
        targetCheckPt = targetPt;
        targetR = r;
        random_run_time = UnityEngine.Random.Range(1f, 2f);
        crossFade(runAnimation, 0.5f);
    }

    public void toPVPCheckPoint(Vector3 targetPt, float r)
    {
        state = TitanState.to_pvp_pt;
        targetCheckPt = targetPt;
        targetR = r;
        crossFade(runAnimation, 0.5f);
    }

    public void hitL(Vector3 attacker, float hitPauseTime)
    {
        if (abnormalType != AbnormalType.TYPE_CRAWLER)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
            {
                hit("hit_eren_L", attacker, hitPauseTime);
            }
            else
            {
                base.photonView.RPC("hitLRPC", PhotonTargets.All, attacker, hitPauseTime);
            }
        }
    }

    [RPC]
    private void hitLRPC(Vector3 attacker, float hitPauseTime)
    {
        if (base.photonView.isMine)
        {
            float magnitude = (attacker - base.transform.position).magnitude;
            if (magnitude < 80f)
            {
                hit("hit_eren_L", attacker, hitPauseTime);
            }
        }
    }

    private void hit(string animationName, Vector3 attacker, float hitPauseTime)
    {
        state = TitanState.hit;
        hitAnimation = animationName;
        hitPause = hitPauseTime;
        playAnimation(hitAnimation);
        base.animation[hitAnimation].time = 0f;
        base.animation[hitAnimation].speed = 0f;
        Transform transform = base.transform;
        Vector3 eulerAngles = Quaternion.LookRotation(attacker - base.transform.position).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
        needFreshCorePosition = true;
        if (base.photonView.isMine && grabbedTarget != null)
        {
            grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
        }
    }

    public void hitR(Vector3 attacker, float hitPauseTime)
    {
        if (abnormalType != AbnormalType.TYPE_CRAWLER)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
            {
                hit("hit_eren_R", attacker, hitPauseTime);
            }
            else
            {
                base.photonView.RPC("hitRRPC", PhotonTargets.All, attacker, hitPauseTime);
            }
        }
    }

    [RPC]
    private void hitRRPC(Vector3 attacker, float hitPauseTime)
    {
        if (base.photonView.isMine && !hasDie)
        {
            float magnitude = (attacker - base.transform.position).magnitude;
            if (magnitude < 80f)
            {
                hit("hit_eren_R", attacker, hitPauseTime);
            }
        }
    }

    public void dieBlow(Vector3 attacker, float hitPauseTime)
    {
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
        {
            dieBlowFunc(attacker, hitPauseTime);
            if (GameObject.FindGameObjectsWithTag("titan").Length <= 1)
            {
                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            }
        }
        else
        {
            base.photonView.RPC("dieBlowRPC", PhotonTargets.All, attacker, hitPauseTime);
        }
    }

    [RPC]
    private void dieBlowRPC(Vector3 attacker, float hitPauseTime)
    {
        if (base.photonView.isMine)
        {
            float magnitude = (attacker - base.transform.position).magnitude;
            if (magnitude < 80f)
            {
                dieBlowFunc(attacker, hitPauseTime);
            }
        }
    }

    public void dieBlowFunc(Vector3 attacker, float hitPauseTime)
    {
        if (hasDie)
        {
            return;
        }
        Transform transform = base.transform;
        Vector3 eulerAngles = Quaternion.LookRotation(attacker - base.transform.position).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
        hasDie = true;
        hitAnimation = "die_blow";
        hitPause = hitPauseTime;
        playAnimation(hitAnimation);
        base.animation[hitAnimation].time = 0f;
        base.animation[hitAnimation].speed = 0f;
        needFreshCorePosition = true;
        fengGame.oneTitanDown(string.Empty, onPlayerLeave: false);
        if (base.photonView.isMine)
        {
            if (grabbedTarget != null)
            {
                grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
            }
            if (nonAI)
            {
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
                PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                {
                    { PhotonPlayerProperty.Dead, true },
                    { PhotonPlayerProperty.Deaths, (int)PhotonNetwork.player.customProperties[PhotonPlayerProperty.Deaths] + 1 }
                });
            }
        }
    }

    public void dieHeadBlow(Vector3 attacker, float hitPauseTime)
    {
        if (abnormalType == AbnormalType.TYPE_CRAWLER)
        {
            return;
        }
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
        {
            dieHeadBlowFunc(attacker, hitPauseTime);
            if (GameObject.FindGameObjectsWithTag("titan").Length <= 1)
            {
                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            }
        }
        else
        {
            base.photonView.RPC("dieHeadBlowRPC", PhotonTargets.All, attacker, hitPauseTime);
        }
    }

    [RPC]
    private void dieHeadBlowRPC(Vector3 attacker, float hitPauseTime)
    {
        if (base.photonView.isMine && (attacker - neck.position).magnitude < lagMax)
        {
            dieHeadBlowFunc(attacker, hitPauseTime);
        }
    }

    private void playSound(string sndname)
    {
        playsoundRPC(sndname);
        if (base.photonView.isMine)
        {
            base.photonView.RPC("playsoundRPC", PhotonTargets.Others, sndname);
        }
    }

    [RPC]
    private void playsoundRPC(string sndname, PhotonMessageInfo info = null)
    {
        if (!Guardian.AntiAbuse.TitanPatches.IsSoundPlayValid(this, info))
        {
            return;
        }

        Transform transform = base.transform.Find(sndname);
        transform.GetComponent<AudioSource>().Play();
    }

    public void dieHeadBlowFunc(Vector3 attacker, float hitPauseTime)
    {
        if (hasDie)
        {
            return;
        }
        playSound("snd_titan_head_blow");
        Transform transform = base.transform;
        Vector3 eulerAngles = Quaternion.LookRotation(attacker - base.transform.position).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
        hasDie = true;
        hitAnimation = "die_headOff";
        hitPause = hitPauseTime;
        playAnimation(hitAnimation);
        base.animation[hitAnimation].time = 0f;
        base.animation[hitAnimation].speed = 0f;
        fengGame.oneTitanDown(string.Empty, onPlayerLeave: false);
        needFreshCorePosition = true;
        GameObject gameObject = (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("bloodExplore"), head.position + Vector3.up * 1f * myLevel, Quaternion.Euler(270f, 0f, 0f))) : PhotonNetwork.Instantiate("bloodExplore", head.position + Vector3.up * 1f * myLevel, Quaternion.Euler(270f, 0f, 0f), 0);
        gameObject.transform.localScale = base.transform.localScale;
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
        {
            Vector3 position = head.position;
            Vector3 eulerAngles2 = neck.rotation.eulerAngles;
            float x = 270f + eulerAngles2.x;
            float y = eulerAngles2.y;
            gameObject = PhotonNetwork.Instantiate("bloodsplatter", position, Quaternion.Euler(x, y, eulerAngles2.z), 0);
        }
        else
        {
            UnityEngine.Object original = Resources.Load("bloodsplatter");
            Vector3 position2 = head.position;
            Vector3 eulerAngles5 = neck.rotation.eulerAngles;
            float x2 = 270f + eulerAngles5.x;
            float y2 = eulerAngles5.y;
            gameObject = (GameObject)UnityEngine.Object.Instantiate(original, position2, Quaternion.Euler(x2, y2, eulerAngles5.z));
        }
        gameObject.transform.localScale = base.transform.localScale;
        gameObject.transform.parent = neck;
        gameObject = ((IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/justSmoke"), neck.position, Quaternion.Euler(270f, 0f, 0f))) : PhotonNetwork.Instantiate("FX/justSmoke", neck.position, Quaternion.Euler(270f, 0f, 0f), 0));
        gameObject.transform.parent = neck;
        if (base.photonView.isMine)
        {
            if (grabbedTarget != null)
            {
                grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
            }
            if (nonAI)
            {
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
                PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                {
                    { PhotonPlayerProperty.Dead, true },
                    { PhotonPlayerProperty.Deaths, (int)PhotonNetwork.player.customProperties[PhotonPlayerProperty.Deaths] + 1 }
                });
            }
        }
    }

    private void justEatHero(GameObject target, Transform hand)
    {
        if (target == null)
        {
            return;
        }
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
        {
            HERO hero = target.GetComponent<HERO>();
            if (!hero.HasDied())
            {
                hero.markDie();
                if (nonAI)
                {
                    hero.photonView.RPC("netDie2", PhotonTargets.All, base.photonView.viewID, base.name);
                }
                else
                {
                    hero.photonView.RPC("netDie2", PhotonTargets.All, -1, base.name);
                }
            }
        }
        else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
        {
            target.GetComponent<HERO>().die2(hand);
        }
    }

    [RPC]
    private void setIfLookTarget(bool bo)
    {
        asClientLookTarget = bo;
    }

    private void OnCollisionStay()
    {
        grounded = true;
    }

    private void grab(string type)
    {
        state = TitanState.grab;
        attacked = false;
        isAlarm = true;
        attackAnimation = type;
        crossFade("grab_" + type, 0.1f);
        isGrabHandLeft = true;
        grabbedTarget = null;
        switch (type)
        {
            case "ground_back_l":
                attackCheckTimeA = 0.34f;
                attackCheckTimeB = 0.49f;
                break;
            case "ground_back_r":
                attackCheckTimeA = 0.34f;
                attackCheckTimeB = 0.49f;
                isGrabHandLeft = false;
                break;
            case "ground_front_l":
                attackCheckTimeA = 0.37f;
                attackCheckTimeB = 0.6f;
                break;
            case "ground_front_r":
                attackCheckTimeA = 0.37f;
                attackCheckTimeB = 0.6f;
                isGrabHandLeft = false;
                break;
            case "head_back_l":
                attackCheckTimeA = 0.45f;
                attackCheckTimeB = 0.5f;
                isGrabHandLeft = false;
                break;
            case "head_back_r":
                attackCheckTimeA = 0.45f;
                attackCheckTimeB = 0.5f;
                break;
            case "head_front_l":
                attackCheckTimeA = 0.38f;
                attackCheckTimeB = 0.55f;
                break;
            case "head_front_r":
                attackCheckTimeA = 0.38f;
                attackCheckTimeB = 0.55f;
                isGrabHandLeft = false;
                break;
        }
        if (isGrabHandLeft)
        {
            currentGrabHand = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
        }
        else
        {
            currentGrabHand = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        }
    }

    private void eat()
    {
        state = TitanState.eat;
        attacked = false;
        if (isGrabHandLeft)
        {
            attackAnimation = "eat_l";
            crossFade("eat_l", 0.1f);
        }
        else
        {
            attackAnimation = "eat_r";
            crossFade("eat_r", 0.1f);
        }
    }

    private void chase()
    {
        state = TitanState.chase;
        isAlarm = true;
        crossFade(runAnimation, 0.5f);
    }

    private void idle(float sbtime = 0f)
    {
        stuck = false;
        this.sbtime = sbtime;
        if (myDifficulty == 2 && (abnormalType == AbnormalType.TYPE_JUMPER || abnormalType == AbnormalType.TYPE_I))
        {
            this.sbtime = UnityEngine.Random.Range(0f, 1.5f);
        }
        else if (myDifficulty >= 1)
        {
            this.sbtime = 0f;
        }
        this.sbtime = Mathf.Max(0.5f, this.sbtime);
        if (abnormalType == AbnormalType.TYPE_PUNK)
        {
            this.sbtime = 0.1f;
            if (myDifficulty == 1)
            {
                this.sbtime += 0.4f;
            }
        }
        state = TitanState.idle;
        if (abnormalType == AbnormalType.TYPE_CRAWLER)
        {
            crossFade("crawler_idle", 0.2f);
        }
        else
        {
            crossFade("idle", 0.2f);
        }
    }

    private void wander(float sbtime = 0f)
    {
        state = TitanState.wander;
        crossFade(runAnimation, 0.5f);
    }

    private void turn(float d)
    {
        if (abnormalType == AbnormalType.TYPE_CRAWLER)
        {
            if (d > 0f)
            {
                turnAnimation = "crawler_turnaround_R";
            }
            else
            {
                turnAnimation = "crawler_turnaround_L";
            }
        }
        else if (d > 0f)
        {
            turnAnimation = "turnaround2";
        }
        else
        {
            turnAnimation = "turnaround1";
        }
        playAnimation(turnAnimation);
        base.animation[turnAnimation].time = 0f;
        d = Mathf.Clamp(d, -120f, 120f);
        turnDeg = d;
        Vector3 eulerAngles = base.gameObject.transform.rotation.eulerAngles;
        desDeg = eulerAngles.y + turnDeg;
        state = TitanState.turn;
    }

    private void eatSet(GameObject grabTarget)
    {
        if ((IN_GAME_MAIN_CAMERA.Gametype != 0 && (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine)) || !grabTarget.GetComponent<HERO>().isGrabbed)
        {
            grabToRight();
            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
            {
                base.photonView.RPC("grabToRight", PhotonTargets.Others);
                grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, "grabbed");
                grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, base.photonView.viewID, false);
            }
            else
            {
                grabTarget.GetComponent<HERO>().grabbed(base.gameObject, leftHand: false);
                grabTarget.GetComponent<HERO>().animation.Play("grabbed");
            }
        }
    }

    private void eatSetL(GameObject grabTarget)
    {
        if ((IN_GAME_MAIN_CAMERA.Gametype != 0 && (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine)) || !grabTarget.GetComponent<HERO>().isGrabbed)
        {
            grabToLeft();
            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
            {
                base.photonView.RPC("grabToLeft", PhotonTargets.Others);
                grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, "grabbed");
                grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, base.photonView.viewID, true);
            }
            else
            {
                grabTarget.GetComponent<HERO>().grabbed(base.gameObject, leftHand: true);
                grabTarget.GetComponent<HERO>().animation.Play("grabbed");
            }
        }
    }

    [RPC]
    public void grabToRight(PhotonMessageInfo info = null)
    {
        if (!Guardian.AntiAbuse.TitanPatches.IsRightGrabValid(this, info))
        {
            return;
        }

        Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        grabTF.transform.parent = transform;
        grabTF.transform.position = transform.GetComponent<SphereCollider>().transform.position;
        grabTF.transform.rotation = transform.GetComponent<SphereCollider>().transform.rotation;
        grabTF.transform.localPosition -= Vector3.right * transform.GetComponent<SphereCollider>().radius * 0.3f;
        grabTF.transform.localPosition += Vector3.up * transform.GetComponent<SphereCollider>().radius * 0.51f;
        grabTF.transform.localPosition -= Vector3.forward * transform.GetComponent<SphereCollider>().radius * 0.3f;
        Transform transform2 = grabTF.transform;
        Vector3 eulerAngles = grabTF.transform.localRotation.eulerAngles;
        float x = eulerAngles.x;
        Vector3 eulerAngles2 = grabTF.transform.localRotation.eulerAngles;
        float y = eulerAngles2.y + 180f;
        Vector3 eulerAngles3 = grabTF.transform.localRotation.eulerAngles;
        transform2.localRotation = Quaternion.Euler(x, y, eulerAngles3.z);
    }

    [RPC]
    public void grabToLeft(PhotonMessageInfo info = null)
    {
        if (!Guardian.AntiAbuse.TitanPatches.IsRightGrabValid(this, info))
        {
            return;
        }

        Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
        grabTF.transform.parent = transform;
        grabTF.transform.position = transform.GetComponent<SphereCollider>().transform.position;
        grabTF.transform.rotation = transform.GetComponent<SphereCollider>().transform.rotation;
        grabTF.transform.localPosition -= Vector3.right * transform.GetComponent<SphereCollider>().radius * 0.3f;
        grabTF.transform.localPosition -= Vector3.up * transform.GetComponent<SphereCollider>().radius * 0.51f;
        grabTF.transform.localPosition -= Vector3.forward * transform.GetComponent<SphereCollider>().radius * 0.3f;
        Transform transform2 = grabTF.transform;
        Vector3 eulerAngles = grabTF.transform.localRotation.eulerAngles;
        float x = eulerAngles.x;
        Vector3 eulerAngles2 = grabTF.transform.localRotation.eulerAngles;
        float y = eulerAngles2.y + 180f;
        Vector3 eulerAngles3 = grabTF.transform.localRotation.eulerAngles;
        transform2.localRotation = Quaternion.Euler(x, y, eulerAngles3.z + 180f);
    }

    [RPC]
    public void grabbedTargetEscape(PhotonMessageInfo info = null)
    {
        grabbedTarget = null;
    }

    private void justHitEye()
    {
        if (state != TitanState.hit_eye)
        {
            switch (state)
            {
                case TitanState.down:
                case TitanState.sit:
                    playAnimation("sit_hit_eye");
                    break;
                default:
                    playAnimation("hit_eye");
                    break;
            }
            state = TitanState.hit_eye;
        }
    }

    public void hitEye()
    {
        if (!hasDie)
        {
            justHitEye();
        }
    }

    [RPC]
    public void hitEyeRPC(int viewId)
    {
        if (hasDie)
        {
            return;
        }
        float magnitude = (PhotonView.Find(viewId).gameObject.transform.position - neck.position).magnitude;
        if (magnitude < 20f)
        {
            if (base.photonView.isMine && grabbedTarget != null)
            {
                grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
            }
            if (!hasDie)
            {
                justHitEye();
            }
        }
    }

    private void getDown()
    {
        state = TitanState.down;
        isAlarm = true;
        playAnimation("sit_hunt_down");
        getdownTime = UnityEngine.Random.Range(3f, 5f);
    }

    private void sitdown()
    {
        state = TitanState.sit;
        playAnimation("sit_down");
        getdownTime = UnityEngine.Random.Range(10f, 30f);
    }

    private void remainSitdown()
    {
        state = TitanState.sit;
        playAnimation("sit_idle");
        getdownTime = UnityEngine.Random.Range(10f, 30f);
    }

    private void recover()
    {
        state = TitanState.recover;
        playAnimation("idle_recovery");
        getdownTime = UnityEngine.Random.Range(2f, 5f);
    }

    public void hitAnkle()
    {
        if (!hasDie && state != TitanState.down)
        {
            if (grabbedTarget != null)
            {
                grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
            }
            getDown();
        }
    }

    [RPC]
    public void hitAnkleRPC(int viewID)
    {
        if (hasDie || state == TitanState.down)
        {
            return;
        }
        PhotonView photonView = PhotonView.Find(viewID);
        if (photonView == null)
        {
            return;
        }
        float magnitude = (photonView.gameObject.transform.position - base.transform.position).magnitude;
        if (magnitude < 20f)
        {
            if (base.photonView.isMine && grabbedTarget != null)
            {
                grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
            }
            getDown();
        }
    }

    public bool die()
    {
        if (hasDie)
        {
            return false;
        }
        hasDie = true;
        fengGame.oneTitanDown(string.Empty, onPlayerLeave: false);
        dieAnimation();
        return true;
    }

    private IEnumerator DoEndless()
    {
        yield return new WaitForSeconds(4.5f);
        object[] respawnPoint = Guardian.Utilities.GameHelper.GetRandomTitanRespawnPoint();
        GameObject go = FengGameManagerMKII.Instance.SpawnTitanRaw((Vector3)respawnPoint[0], (Quaternion)respawnPoint[1]);
        AbnormalType type = this.abnormalType;
        go.GetComponent<TITAN>().setAbnormalType2(type, type.Equals(AbnormalType.TYPE_CRAWLER));
    }

    private IEnumerator DoDeathLaugh()
    {
        this.crossFade("laugh", 0.05f);
        yield return new WaitForSeconds(animation["laugh"].length - 0.5f);
        this.crossFade("die_back", 0.05f);
    }

    private void dieAnimation()
    {
        // Mod
        if ((PhotonNetwork.isMasterClient && base.photonView.isMine) || IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
        {
            // Endless
            if (Guardian.Mod.Properties.EndlessTitans.Value)
            {
                StartCoroutine(DoEndless());
            }

            // Laughing
            if (Guardian.Mod.Properties.LaughingTitans.Value)
            {
                StartCoroutine(DoDeathLaugh());
                return;
            }
        }

        if (base.animation.IsPlaying("sit_idle") || base.animation.IsPlaying("sit_hit_eye"))
        {
            crossFade("sit_die", 0.1f);
        }
        else if (abnormalType == AbnormalType.TYPE_CRAWLER)
        {
            crossFade("crawler_die", 0.2f);
        }
        else if (abnormalType == AbnormalType.NORMAL)
        {
            crossFade("die_front", 0.05f);
        }
        else if ((base.animation.IsPlaying("attack_abnormal_jump") && base.animation["attack_abnormal_jump"].normalizedTime > 0.7f) || (base.animation.IsPlaying("attack_abnormal_getup") && base.animation["attack_abnormal_getup"].normalizedTime < 0.7f) || base.animation.IsPlaying("tired"))
        {
            crossFade("die_ground", 0.2f);
        }
        else
        {
            crossFade("die_back", 0.05f);
        }
    }

    [RPC]
    public void titanGetHit(int viewID, int speed)
    {
        PhotonView photonView = PhotonView.Find(viewID);
        if (!(photonView != null) || !((photonView.gameObject.transform.position - neck.position).magnitude < lagMax) || hasDie || !(Time.time - healthTime > 0.2f))
        {
            return;
        }
        healthTime = Time.time;
        if (speed >= RCSettings.DamageMode || abnormalType == AbnormalType.TYPE_CRAWLER)
        {
            currentHealth -= speed;
        }
        if ((float)maxHealth > 0f)
        {
            base.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, currentHealth, maxHealth);
        }
        if ((float)currentHealth < 0f)
        {
            if (PhotonNetwork.isMasterClient)
            {
                OnTitanDie(photonView);
            }
            base.photonView.RPC("netDie", PhotonTargets.OthersBuffered);
            if (grabbedTarget != null)
            {
                grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
            }
            netDie();
            if (nonAI)
            {
                FengGameManagerMKII.Instance.titanGetKill(photonView.owner, speed, (string)PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]);
            }
            else
            {
                FengGameManagerMKII.Instance.titanGetKill(photonView.owner, speed, base.name);
            }
        }
        else
        {
            FengGameManagerMKII.Instance.photonView.RPC("netShowDamage", photonView.owner, speed);
        }
    }

    [RPC]
    private void netDie()
    {
        asClientLookTarget = false;
        if (!hasDie)
        {
            hasDie = true;
            if (nonAI)
            {
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
                PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                {
                    { PhotonPlayerProperty.Dead, true },
                    { PhotonPlayerProperty.Deaths, (int)PhotonNetwork.player.customProperties[PhotonPlayerProperty.Deaths] + 1 }
                });
            }
            dieAnimation();
        }
    }

    public void suicide()
    {
        netDie();
        if (nonAI)
        {
            fengGame.sendKillInfo(isKillerTitan: false, string.Empty, isVictimTitan: true, (string)PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]);
        }
        fengGame.needChooseSide = true;
        fengGame.justSuicide = true;
    }

    private void Awake()
    {
        cache();
        baseRigidBody.freezeRotation = true;
        baseRigidBody.useGravity = false;
    }

    private void Start()
    {
        FengGameManagerMKII.Instance.AddTitan(this);
        if (Minimap.Instance != null)
        {
            Minimap.Instance.TrackGameObjectOnMinimap(base.gameObject, Color.yellow, trackOrientation: false, depthAboveAll: true);
        }
        currentCamera = GameObject.Find("MainCamera");
        runAnimation = "run_walk";
        grabTF = new GameObject();
        grabTF.name = "titansTmpGrabTF";
        head = baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
        neck = baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
        oldHeadRotation = head.rotation;
        if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.MULTIPLAYER || base.photonView.isMine)
        {
            if (!hasSetLevel)
            {
                if (RCSettings.SizeMode > 0)
                {
                    float sizeLower = RCSettings.SizeLower;
                    float sizeUpper = RCSettings.SizeUpper;
                    myLevel = UnityEngine.Random.Range(sizeLower, sizeUpper);
                }
                else
                {
                    myLevel = UnityEngine.Random.Range(0.7f, 3f);
                }
                hasSetLevel = true;
            }
            spawnPt = baseTransform.position;
            setmyLevel();
            setAbnormalType2(abnormalType, forceCrawler: false);
            if (myHero == null)
            {
                findNearestHero2();
            }
            controller = base.gameObject.GetComponent<TITAN_CONTROLLER>();
            if (nonAI)
            {
                StartCoroutine(ReloadSky());
            }
        }
        if (maxHealth == 0 && RCSettings.HealthMode > 0)
        {
            if (RCSettings.HealthMode == 1)
            {
                maxHealth = (currentHealth = UnityEngine.Random.Range(RCSettings.HealthLower, RCSettings.HealthUpper + 1));
            }
            else if (RCSettings.HealthMode == 2)
            {
                maxHealth = (currentHealth = Mathf.Clamp(Mathf.RoundToInt(myLevel / 4f * (float)UnityEngine.Random.Range(RCSettings.HealthLower, RCSettings.HealthUpper + 1)), RCSettings.HealthLower, RCSettings.HealthUpper));
            }
        }
        lagMax = 150f + myLevel * 3f;
        healthTime = Time.time;
        if (currentHealth > 0 && base.photonView.isMine)
        {
            base.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, currentHealth, maxHealth);
        }
        hasExplode = false;
        colliderEnabled = true;
        isHooked = false;
        isLook = false;
        hasSpawn = true;
    }

    private GameObject getNearestHero2()
    {
        GameObject result = null;
        float num = float.PositiveInfinity;
        Vector3 position = baseTransform.position;
        foreach (HERO player in FengGameManagerMKII.Instance.heroes)
        {
            GameObject gameObject = player.gameObject;
            float num2 = Vector3.Distance(gameObject.transform.position, position);
            if (num2 < num)
            {
                result = gameObject;
                num = num2;
            }
        }
        return result;
    }

    private void findNearestFacingHero2()
    {
        GameObject target = null;
        float num = float.PositiveInfinity;
        Vector3 position = baseTransform.position;
        float num2 = 0f;
        float num3 = (abnormalType != 0) ? 180f : 100f;
        float num4 = 0f;
        foreach (HERO player in FengGameManagerMKII.Instance.heroes)
        {
            GameObject gameObject = player.gameObject;
            float num5 = Vector3.Distance(gameObject.transform.position, position);
            if (num5 < num)
            {
                Vector3 vector = gameObject.transform.position - baseTransform.position;
                num2 = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
                num4 = 0f - Mathf.DeltaAngle(num2, baseGameObjectTransform.rotation.eulerAngles.y - 90f);
                if (Mathf.Abs(num4) < num3)
                {
                    target = gameObject;
                    num = num5;
                }
            }
        }
        if (target == null)
        {
            return;
        }
        GameObject oldHero = myHero;
        myHero = target;
        if (oldHero != myHero && IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
        {
            if (myHero == null)
            {
                base.photonView.RPC("setMyTarget", PhotonTargets.Others, -1);
            }
            else
            {
                base.photonView.RPC("setMyTarget", PhotonTargets.Others, myHero.GetPhotonView().viewID);
            }
        }
        tauntTime = 5f;
    }

    private void findNearestHero2()
    {
        GameObject y = myHero;
        myHero = getNearestHero2();
        if (myHero != y && IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
        {
            if (myHero == null)
            {
                base.photonView.RPC("setMyTarget", PhotonTargets.Others, -1);
            }
            else
            {
                base.photonView.RPC("setMyTarget", PhotonTargets.Others, myHero.GetPhotonView().viewID);
            }
        }
        oldHeadRotation = head.rotation;
    }

    private void FixedUpdate()
    {
        if ((IN_GAME_MAIN_CAMERA.IsPausing && IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE) || (IN_GAME_MAIN_CAMERA.Gametype != 0 && !base.photonView.isMine))
        {
            return;
        }
        baseRigidBody.AddForce(new Vector3(0f, (0f - gravity) * baseRigidBody.mass, 0f));
        if (needFreshCorePosition)
        {
            oldCorePosition = baseTransform.position - baseTransform.Find("Amarture/Core").position;
            needFreshCorePosition = false;
        }
        if (hasDie)
        {
            if (hitPause <= 0f && baseAnimation.IsPlaying("die_headOff"))
            {
                Vector3 a = baseTransform.position - baseTransform.Find("Amarture/Core").position - oldCorePosition;
                baseRigidBody.velocity = a / Time.deltaTime + Vector3.up * baseRigidBody.velocity.y;
            }
            oldCorePosition = baseTransform.position - baseTransform.Find("Amarture/Core").position;
        }
        else if ((state == TitanState.attack && isAttackMoveByCore) || state == TitanState.hit)
        {
            Vector3 a2 = baseTransform.position - baseTransform.Find("Amarture/Core").position - oldCorePosition;
            baseRigidBody.velocity = a2 / Time.deltaTime + Vector3.up * baseRigidBody.velocity.y;
            oldCorePosition = baseTransform.position - baseTransform.Find("Amarture/Core").position;
        }
        if (hasDie)
        {
            if (hitPause > 0f)
            {
                hitPause -= Time.deltaTime;
                if (hitPause <= 0f)
                {
                    baseAnimation[hitAnimation].speed = 1f;
                    hitPause = 0f;
                }
            }
            else if (baseAnimation.IsPlaying("die_blow"))
            {
                if (baseAnimation["die_blow"].normalizedTime < 0.55f)
                {
                    baseRigidBody.velocity = -baseTransform.forward * 300f + Vector3.up * baseRigidBody.velocity.y;
                }
                else if (baseAnimation["die_blow"].normalizedTime < 0.83f)
                {
                    baseRigidBody.velocity = -baseTransform.forward * 100f + Vector3.up * baseRigidBody.velocity.y;
                }
                else
                {
                    baseRigidBody.velocity = Vector3.up * baseRigidBody.velocity.y;
                }
            }
            return;
        }
        if (nonAI && !IN_GAME_MAIN_CAMERA.IsPausing && (state == TitanState.idle || (state == TitanState.attack && attackAnimation == "jumper_1")))
        {
            Vector3 a3 = Vector3.zero;
            if (controller.targetDirection != -874f)
            {
                bool flag = false;
                if (stamina < 5f)
                {
                    flag = true;
                }
                else if (stamina < 40f && !baseAnimation.IsPlaying("run_abnormal") && !baseAnimation.IsPlaying("crawler_run"))
                {
                    flag = true;
                }
                a3 = ((!controller.isWALKDown && !flag) ? (baseTransform.forward * speed * Mathf.Sqrt(myLevel)) : (baseTransform.forward * speed * Mathf.Sqrt(myLevel) * 0.2f));
                baseGameObjectTransform.rotation = Quaternion.Lerp(baseGameObjectTransform.rotation, Quaternion.Euler(0f, controller.targetDirection, 0f), speed * 0.15f * Time.deltaTime);
                if (state == TitanState.idle)
                {
                    if (controller.isWALKDown || flag)
                    {
                        if (abnormalType == AbnormalType.TYPE_CRAWLER)
                        {
                            if (!baseAnimation.IsPlaying("crawler_run"))
                            {
                                crossFade("crawler_run", 0.1f);
                            }
                        }
                        else if (!baseAnimation.IsPlaying("run_walk"))
                        {
                            crossFade("run_walk", 0.1f);
                        }
                    }
                    else if (abnormalType == AbnormalType.TYPE_CRAWLER)
                    {
                        if (!baseAnimation.IsPlaying("crawler_run"))
                        {
                            crossFade("crawler_run", 0.1f);
                        }
                        GameObject gameObject = checkIfHitCrawlerMouth(head, 2.2f);
                        if (gameObject != null)
                        {
                            Vector3 position = baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
                            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                            {
                                gameObject.GetComponent<HERO>().die((gameObject.transform.position - position) * 15f * myLevel, isBite: false);
                            }
                            else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject.GetComponent<HERO>().HasDied())
                            {
                                gameObject.GetComponent<HERO>().markDie();
                                object[] parameters = new object[5]
                                {
                                    (gameObject.transform.position - position) * 15f * myLevel,
                                    true,
                                    (!nonAI) ? (-1) : base.photonView.viewID,
                                    base.name,
                                    true
                                };
                                gameObject.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters);
                            }
                        }
                    }
                    else if (!baseAnimation.IsPlaying("run_abnormal"))
                    {
                        crossFade("run_abnormal", 0.1f);
                    }
                }
            }
            else if (state == TitanState.idle)
            {
                if (abnormalType == AbnormalType.TYPE_CRAWLER)
                {
                    if (!baseAnimation.IsPlaying("crawler_idle"))
                    {
                        crossFade("crawler_idle", 0.1f);
                    }
                }
                else if (!baseAnimation.IsPlaying("idle"))
                {
                    crossFade("idle", 0.1f);
                }
                a3 = Vector3.zero;
            }
            if (state == TitanState.idle)
            {
                Vector3 velocity = baseRigidBody.velocity;
                Vector3 force = a3 - velocity;
                force.x = Mathf.Clamp(force.x, 0f - maxVelocityChange, maxVelocityChange);
                force.z = Mathf.Clamp(force.z, 0f - maxVelocityChange, maxVelocityChange);
                force.y = 0f;
                baseRigidBody.AddForce(force, ForceMode.VelocityChange);
            }
            else if (state == TitanState.attack && attackAnimation == "jumper_0")
            {
                Vector3 velocity2 = baseRigidBody.velocity;
                Vector3 force2 = a3 * 0.8f - velocity2;
                force2.x = Mathf.Clamp(force2.x, 0f - maxVelocityChange, maxVelocityChange);
                force2.z = Mathf.Clamp(force2.z, 0f - maxVelocityChange, maxVelocityChange);
                force2.y = 0f;
                baseRigidBody.AddForce(force2, ForceMode.VelocityChange);
            }
        }
        if ((abnormalType == AbnormalType.TYPE_I || abnormalType == AbnormalType.TYPE_JUMPER) && !nonAI && state == TitanState.attack && attackAnimation == "jumper_0")
        {
            Vector3 a4 = baseTransform.forward * speed * myLevel * 0.5f;
            Vector3 velocity3 = baseRigidBody.velocity;
            if (baseAnimation["attack_jumper_0"].normalizedTime <= 0.28f || baseAnimation["attack_jumper_0"].normalizedTime >= 0.8f)
            {
                a4 = Vector3.zero;
            }
            Vector3 force3 = a4 - velocity3;
            force3.x = Mathf.Clamp(force3.x, 0f - maxVelocityChange, maxVelocityChange);
            force3.z = Mathf.Clamp(force3.z, 0f - maxVelocityChange, maxVelocityChange);
            force3.y = 0f;
            baseRigidBody.AddForce(force3, ForceMode.VelocityChange);
        }
        if (state != TitanState.chase && state != TitanState.wander && state != TitanState.to_check_point && state != TitanState.to_pvp_pt && state != TitanState.random_run)
        {
            return;
        }
        Vector3 a5 = baseTransform.forward * speed;
        Vector3 velocity4 = baseRigidBody.velocity;
        Vector3 force4 = a5 - velocity4;
        force4.x = Mathf.Clamp(force4.x, 0f - maxVelocityChange, maxVelocityChange);
        force4.z = Mathf.Clamp(force4.z, 0f - maxVelocityChange, maxVelocityChange);
        force4.y = 0f;
        baseRigidBody.AddForce(force4, ForceMode.VelocityChange);
        if (!stuck && abnormalType != AbnormalType.TYPE_CRAWLER && !nonAI)
        {
            if (baseAnimation.IsPlaying(runAnimation) && baseRigidBody.velocity.magnitude < speed * 0.5f)
            {
                stuck = true;
                stuckTime = 2f;
                stuckTurnAngle = (float)UnityEngine.Random.Range(0, 2) * 140f - 70f;
            }
            if (state == TitanState.chase && myHero != null && myDistance > attackDistance && myDistance < 150f)
            {
                float num = 0.05f;
                if (myDifficulty > 1)
                {
                    num += 0.05f;
                }
                if (abnormalType != 0)
                {
                    num += 0.1f;
                }
                if (UnityEngine.Random.Range(0f, 1f) < num)
                {
                    stuck = true;
                    stuckTime = 1f;
                    float num2 = UnityEngine.Random.Range(20f, 50f);
                    stuckTurnAngle = (float)UnityEngine.Random.Range(0, 2) * num2 * 2f - num2;
                }
            }
        }
        float num3 = 0f;
        switch (state)
        {
            case TitanState.wander:
                num3 = baseTransform.rotation.eulerAngles.y - 90f;
                break;
            case TitanState.to_check_point:
            case TitanState.to_pvp_pt:
            case TitanState.random_run:
                Vector3 vector = targetCheckPt - baseTransform.position;
                num3 = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
                break;
            default:
                if (myHero == null)
                {
                    return;
                }
                Vector3 vector2 = myHero.transform.position - baseTransform.position;
                num3 = (0f - Mathf.Atan2(vector2.z, vector2.x)) * 57.29578f;
                break;
        }
        if (stuck)
        {
            stuckTime -= Time.deltaTime;
            if (stuckTime < 0f)
            {
                stuck = false;
            }
            if (stuckTurnAngle > 0f)
            {
                stuckTurnAngle -= Time.deltaTime * 10f;
            }
            else
            {
                stuckTurnAngle += Time.deltaTime * 10f;
            }
            num3 += stuckTurnAngle;
        }
        float num4 = 0f - Mathf.DeltaAngle(num3, baseGameObjectTransform.rotation.eulerAngles.y - 90f);
        if (abnormalType == AbnormalType.TYPE_CRAWLER)
        {
            baseGameObjectTransform.rotation = Quaternion.Lerp(baseGameObjectTransform.rotation, Quaternion.Euler(0f, baseGameObjectTransform.rotation.eulerAngles.y + num4, 0f), speed * 0.3f * Time.deltaTime / myLevel);
        }
        else
        {
            baseGameObjectTransform.rotation = Quaternion.Lerp(baseGameObjectTransform.rotation, Quaternion.Euler(0f, baseGameObjectTransform.rotation.eulerAngles.y + num4, 0f), speed * 0.5f * Time.deltaTime / myLevel);
        }
    }

    private void attack2(string type)
    {
        state = TitanState.attack;
        attacked = false;
        isAlarm = true;
        if (attackAnimation == type)
        {
            attackAnimation = type;
            playAnimationAt("attack_" + type, 0f);
        }
        else
        {
            attackAnimation = type;
            playAnimationAt("attack_" + type, 0f);
        }
        nextAttackAnimation = null;
        fxName = null;
        isAttackMoveByCore = false;
        attackCheckTime = 0f;
        attackCheckTimeA = 0f;
        attackCheckTimeB = 0f;
        attackEndWait = 0f;
        fxRotation = Quaternion.Euler(270f, 0f, 0f);
        switch (type)
        {
            case "abnormal_getup":
                attackCheckTime = 0f;
                fxName = string.Empty;
                break;
            case "abnormal_jump":
                nextAttackAnimation = "abnormal_getup";
                if (!nonAI)
                {
                    attackEndWait = ((myDifficulty <= 0) ? UnityEngine.Random.Range(1f, 4f) : UnityEngine.Random.Range(0f, 1f));
                }
                else
                {
                    attackEndWait = 0f;
                }
                attackCheckTime = 0.75f;
                fxName = "boom4";
                fxRotation = Quaternion.Euler(270f, baseTransform.rotation.eulerAngles.y, 0f);
                break;
            case "combo_1":
                nextAttackAnimation = "combo_2";
                attackCheckTimeA = 0.54f;
                attackCheckTimeB = 0.76f;
                nonAIcombo = false;
                isAttackMoveByCore = true;
                leftHandAttack = false;
                break;
            case "combo_2":
                if (abnormalType != AbnormalType.TYPE_PUNK && !nonAI)
                {
                    nextAttackAnimation = "combo_3";
                }
                attackCheckTimeA = 0.37f;
                attackCheckTimeB = 0.57f;
                nonAIcombo = false;
                isAttackMoveByCore = true;
                leftHandAttack = true;
                break;
            case "combo_3":
                nonAIcombo = false;
                isAttackMoveByCore = true;
                attackCheckTime = 0.21f;
                fxName = "boom1";
                break;
            case "front_ground":
                fxName = "boom1";
                attackCheckTime = 0.45f;
                break;
            case "kick":
                fxName = "boom5";
                fxRotation = baseTransform.rotation;
                attackCheckTime = 0.43f;
                break;
            case "slap_back":
                fxName = "boom3";
                attackCheckTime = 0.66f;
                break;
            case "slap_face":
                fxName = "boom3";
                attackCheckTime = 0.655f;
                break;
            case "stomp":
                fxName = "boom2";
                attackCheckTime = 0.42f;
                break;
            case "bite":
                fxName = "bite";
                attackCheckTime = 0.6f;
                break;
            case "bite_l":
                fxName = "bite";
                attackCheckTime = 0.4f;
                break;
            case "bite_r":
                fxName = "bite";
                attackCheckTime = 0.4f;
                break;
            case "jumper_0":
                abnorma_jump_bite_horizon_v = Vector3.zero;
                break;
            case "crawler_jump_0":
                abnorma_jump_bite_horizon_v = Vector3.zero;
                break;
            case "anti_AE_l":
                attackCheckTimeA = 0.31f;
                attackCheckTimeB = 0.4f;
                leftHandAttack = true;
                break;
            case "anti_AE_r":
                attackCheckTimeA = 0.31f;
                attackCheckTimeB = 0.4f;
                leftHandAttack = false;
                break;
            case "anti_AE_low_l":
                attackCheckTimeA = 0.31f;
                attackCheckTimeB = 0.4f;
                leftHandAttack = true;
                break;
            case "anti_AE_low_r":
                attackCheckTimeA = 0.31f;
                attackCheckTimeB = 0.4f;
                leftHandAttack = false;
                break;
            case "quick_turn_l":
                attackCheckTimeA = 2f;
                attackCheckTimeB = 2f;
                isAttackMoveByCore = true;
                break;
            case "quick_turn_r":
                attackCheckTimeA = 2f;
                attackCheckTimeB = 2f;
                isAttackMoveByCore = true;
                break;
            case "throw":
                isAlarm = true;
                chaseDistance = 99999f;
                break;
        }
        needFreshCorePosition = true;
    }

    private bool executeAttack2(string decidedAction)
    {
        switch (decidedAction)
        {
            case "grab_ground_front_l":
                grab("ground_front_l");
                return true;
            case "grab_ground_front_r":
                grab("ground_front_r");
                return true;
            case "grab_ground_back_l":
                grab("ground_back_l");
                return true;
            case "grab_ground_back_r":
                grab("ground_back_r");
                return true;
            case "grab_head_front_l":
                grab("head_front_l");
                return true;
            case "grab_head_front_r":
                grab("head_front_r");
                return true;
            case "grab_head_back_l":
                grab("head_back_l");
                return true;
            case "grab_head_back_r":
                grab("head_back_r");
                return true;
            case "attack_abnormal_jump":
                attack2("abnormal_jump");
                return true;
            case "attack_combo":
                attack2("combo_1");
                return true;
            case "attack_front_ground":
                attack2("front_ground");
                return true;
            case "attack_kick":
                attack2("kick");
                return true;
            case "attack_slap_back":
                attack2("slap_back");
                return true;
            case "attack_slap_face":
                attack2("slap_face");
                return true;
            case "attack_stomp":
                attack2("stomp");
                return true;
            case "attack_bite":
                attack2("bite");
                return true;
            case "attack_bite_l":
                attack2("bite_l");
                return true;
            case "attack_bite_r":
                attack2("bite_r");
                return true;
            default:
                return false;
        }
    }

    private bool longRangeAttackCheck2()
    {
        if (abnormalType == AbnormalType.TYPE_PUNK && myHero != null)
        {
            Vector3 vector = myHero.rigidbody.velocity * Time.deltaTime * 30f;
            if (vector.sqrMagnitude > 10f)
            {
                if (simpleHitTestLineAndBall(vector, baseTransform.Find("chkAeLeft").position - myHero.transform.position, 5f * myLevel))
                {
                    attack2("anti_AE_l");
                    return true;
                }
                if (simpleHitTestLineAndBall(vector, baseTransform.Find("chkAeLLeft").position - myHero.transform.position, 5f * myLevel))
                {
                    attack2("anti_AE_low_l");
                    return true;
                }
                if (simpleHitTestLineAndBall(vector, baseTransform.Find("chkAeRight").position - myHero.transform.position, 5f * myLevel))
                {
                    attack2("anti_AE_r");
                    return true;
                }
                if (simpleHitTestLineAndBall(vector, baseTransform.Find("chkAeLRight").position - myHero.transform.position, 5f * myLevel))
                {
                    attack2("anti_AE_low_r");
                    return true;
                }
            }
            Vector3 vector2 = myHero.transform.position - baseTransform.position;
            float current = (0f - Mathf.Atan2(vector2.z, vector2.x)) * 57.29578f;
            float f = 0f - Mathf.DeltaAngle(current, baseGameObjectTransform.rotation.eulerAngles.y - 90f);
            if (rockInterval > 0f)
            {
                rockInterval -= Time.deltaTime;
            }
            else if (Mathf.Abs(f) < 5f)
            {
                Vector3 a = myHero.transform.position + vector;
                float sqrMagnitude = (a - baseTransform.position).sqrMagnitude;
                if (sqrMagnitude > 8000f && sqrMagnitude < 90000f && RCSettings.DisableRock == 0)
                {
                    attack2("throw");
                    rockInterval = 2f;
                    return true;
                }
            }
        }
        return false;
    }

    [RPC]
    private void netSetLevel(float level, int AI, int skinColor, PhotonMessageInfo info)
    {
        if (!Guardian.AntiAbuse.TitanPatches.IsLevelSetValid(this, info) && base.photonView.isMine)
        {
            base.photonView.RPC("netSetLevel", PhotonTargets.OthersBuffered, this.myLevel, this.myDifficulty, this.skinColor);
            return;
        }

        setLevel2(level, AI, skinColor);
        if (level > 5f)
        {
            headscale = new Vector3(1f, 1f, 1f);
        }
        else if (level < 1f && FengGameManagerMKII.level.StartsWith("Custom"))
        {
            myTitanTrigger.GetComponent<CapsuleCollider>().radius *= 2.5f - level;
        }
    }

    public void lateUpdate2()
    {
        if (IN_GAME_MAIN_CAMERA.IsPausing && IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
        {
            return;
        }
        if (baseAnimation.IsPlaying("run_walk"))
        {
            if (baseAnimation["run_walk"].normalizedTime % 1f > 0.1f && baseAnimation["run_walk"].normalizedTime % 1f < 0.6f && stepSoundPhase == 2)
            {
                stepSoundPhase = 1;
                baseAudioSource.Stop();
                baseAudioSource.Play();
            }
            else if (baseAnimation["run_walk"].normalizedTime % 1f > 0.6f && stepSoundPhase == 1)
            {
                stepSoundPhase = 2;
                baseAudioSource.Stop();
                baseAudioSource.Play();
            }
        }
        else if (baseAnimation.IsPlaying("crawler_run"))
        {
            if (baseAnimation["crawler_run"].normalizedTime % 1f > 0.1f && baseAnimation["crawler_run"].normalizedTime % 1f < 0.56f && stepSoundPhase == 2)
            {
                stepSoundPhase = 1;
                baseAudioSource.Stop();
                baseAudioSource.Play();
            }
            else if (baseAnimation["crawler_run"].normalizedTime % 1f > 0.56f && stepSoundPhase == 1)
            {
                stepSoundPhase = 2;
                baseAudioSource.Stop();
                baseAudioSource.Play();
            }
        }
        else if (baseAnimation.IsPlaying("run_abnormal"))
        {
            if (baseAnimation["run_abnormal"].normalizedTime % 1f > 0.47f && baseAnimation["run_abnormal"].normalizedTime % 1f < 0.95f && stepSoundPhase == 2)
            {
                stepSoundPhase = 1;
                baseAudioSource.Stop();
                baseAudioSource.Play();
            }
            else if ((baseAnimation["run_abnormal"].normalizedTime % 1f > 0.95f || baseAnimation["run_abnormal"].normalizedTime % 1f < 0.47f) && stepSoundPhase == 1)
            {
                stepSoundPhase = 2;
                baseAudioSource.Stop();
                baseAudioSource.Play();
            }
        }
        headMovement2();
        grounded = false;
        updateLabel();
        updateCollider();
    }

    public void update2()
    {
        if ((IN_GAME_MAIN_CAMERA.IsPausing && IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE) || myDifficulty < 0 || (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE && !base.photonView.isMine))
        {
            return;
        }
        explode();
        if (!nonAI)
        {
            if (activeRad < int.MaxValue && (state == TitanState.idle || state == TitanState.wander || state == TitanState.chase))
            {
                if (checkPoints.Count > 1)
                {
                    if (Vector3.Distance((Vector3)checkPoints[0], baseTransform.position) > (float)activeRad)
                    {
                        toCheckPoint((Vector3)checkPoints[0], 10f);
                    }
                }
                else if (Vector3.Distance(spawnPt, baseTransform.position) > (float)activeRad)
                {
                    toCheckPoint(spawnPt, 10f);
                }
            }
            if (whoHasTauntMe != null)
            {
                tauntTime -= Time.deltaTime;
                if (tauntTime <= 0f)
                {
                    whoHasTauntMe = null;
                }
                myHero = whoHasTauntMe;
                if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
                {
                    base.photonView.RPC("setMyTarget", PhotonTargets.Others, myHero.GetPhotonView().viewID);
                }
            }
        }
        if (hasDie)
        {
            dieTime += Time.deltaTime;
            if (dieTime > 2f && !hasDieSteam)
            {
                hasDieSteam = true;
                if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                {
                    GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanDie1"));
                    gameObject.transform.position = baseTransform.Find("Amarture/Core/Controller_Body/hip").position;
                    gameObject.transform.localScale = baseTransform.localScale;
                }
                else if (base.photonView.isMine)
                {
                    PhotonNetwork.Instantiate("FX/FXtitanDie1", baseTransform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = baseTransform.localScale;
                }
            }
            if (dieTime > 5f)
            {
                if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                {
                    GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanDie"));
                    gameObject2.transform.position = baseTransform.Find("Amarture/Core/Controller_Body/hip").position;
                    gameObject2.transform.localScale = baseTransform.localScale;
                    UnityEngine.Object.Destroy(base.gameObject);
                }
                else if (base.photonView.isMine)
                {
                    PhotonNetwork.Instantiate("FX/FXtitanDie", baseTransform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = baseTransform.localScale;
                    PhotonNetwork.Destroy(base.gameObject);
                    myDifficulty = -1;
                }
            }
            return;
        }
        if (state == TitanState.hit)
        {
            if (hitPause > 0f)
            {
                hitPause -= Time.deltaTime;
                if (hitPause <= 0f)
                {
                    baseAnimation[hitAnimation].speed = 1f;
                    hitPause = 0f;
                }
            }
            if (baseAnimation[hitAnimation].normalizedTime >= 1f)
            {
                idle();
            }
        }
        if (!nonAI)
        {
            if (myHero == null)
            {
                findNearestHero2();
            }
            if ((state == TitanState.idle || state == TitanState.chase || state == TitanState.wander) && whoHasTauntMe == null && UnityEngine.Random.Range(0, 100) < 10)
            {
                findNearestFacingHero2();
            }
            if (myHero == null)
            {
                myDistance = float.MaxValue;
            }
            else
            {
                myDistance = Mathf.Sqrt((myHero.transform.position.x - baseTransform.position.x) * (myHero.transform.position.x - baseTransform.position.x) + (myHero.transform.position.z - baseTransform.position.z) * (myHero.transform.position.z - baseTransform.position.z));
            }
        }
        else
        {
            if (stamina < maxStamina)
            {
                if (baseAnimation.IsPlaying("idle"))
                {
                    stamina += Time.deltaTime * 30f;
                }
                if (baseAnimation.IsPlaying("crawler_idle"))
                {
                    stamina += Time.deltaTime * 35f;
                }
                if (baseAnimation.IsPlaying("run_walk"))
                {
                    stamina += Time.deltaTime * 10f;
                }
            }
            if (baseAnimation.IsPlaying("run_abnormal_1"))
            {
                stamina -= Time.deltaTime * 5f;
            }
            if (baseAnimation.IsPlaying("crawler_run"))
            {
                stamina -= Time.deltaTime * 15f;
            }
            if (stamina < 0f)
            {
                stamina = 0f;
            }
            if (!IN_GAME_MAIN_CAMERA.IsPausing)
            {
                GameObject.Find("stamina_titan").transform.localScale = new Vector3(stamina, 16f);
            }
        }
        switch (state)
        {
            case TitanState.laugh:
                if (baseAnimation["laugh"].normalizedTime >= 1f)
                {
                    idle(2f);
                }
                break;
            case TitanState.idle:
                if (nonAI)
                {
                    if (IN_GAME_MAIN_CAMERA.IsPausing)
                    {
                        return;
                    }
                    HandlePTInput();
                    if (abnormalType != AbnormalType.TYPE_CRAWLER)
                    {
                        if (controller.isAttackDown && stamina > 25f)
                        {
                            stamina -= 25f;
                            attack2("combo_1");
                        }
                        else if (controller.isAttackIIDown && stamina > 50f)
                        {
                            stamina -= 50f;
                            attack2("abnormal_jump");
                        }
                        else if (controller.isJumpDown && stamina > 15f)
                        {
                            stamina -= 15f;
                            attack2("jumper_0");
                        }
                    }
                    else if (controller.isAttackDown && stamina > 40f)
                    {
                        stamina -= 40f;
                        attack2("crawler_jump_0");
                    }
                    if (controller.isSuicide)
                    {
                        suicide();
                    }
                    return;
                }
                if (sbtime > 0f)
                {
                    sbtime -= Time.deltaTime;
                    return;
                }
                if (!isAlarm)
                {
                    float rng = UnityEngine.Random.Range(0f, 1f);
                    if (abnormalType != AbnormalType.TYPE_PUNK && abnormalType != AbnormalType.TYPE_CRAWLER && rng < 0.005f)
                    {
                        sitdown();
                        return;
                    }
                    if (rng < 0.02f)
                    {
                        wander();
                        return;
                    }
                    if (rng < 0.01f)
                    {
                        turn(UnityEngine.Random.Range(30, 120));
                        return;
                    }
                }
                angle = 0f;
                between2 = 0f;
                if (myDistance < chaseDistance || whoHasTauntMe != null)
                {
                    Vector3 vector = myHero.transform.position - baseTransform.position;
                    angle = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
                    between2 = 0f - Mathf.DeltaAngle(angle, baseGameObjectTransform.rotation.eulerAngles.y - 90f);
                    if (myDistance >= attackDistance)
                    {
                        if (isAlarm || Mathf.Abs(between2) < 90f)
                        {
                            chase();
                            return;
                        }
                        if (!isAlarm && myDistance < chaseDistance * 0.1f)
                        {
                            chase();
                            return;
                        }
                    }
                }
                if (longRangeAttackCheck2())
                {
                    return;
                }
                if (myDistance < chaseDistance)
                {
                    if (abnormalType == AbnormalType.TYPE_JUMPER && (myDistance > attackDistance || myHero.transform.position.y > head.position.y + 4f * myLevel) && Mathf.Abs(between2) < 120f && Vector3.Distance(baseTransform.position, myHero.transform.position) < 1.5f * myHero.transform.position.y)
                    {
                        attack2("jumper_0");
                        return;
                    }
                    if (abnormalType == AbnormalType.TYPE_CRAWLER && myDistance < attackDistance * 3f && Mathf.Abs(between2) < 90f && myHero.transform.position.y < neck.position.y + 30f * myLevel && myHero.transform.position.y > neck.position.y + 10f * myLevel)
                    {
                        attack2("crawler_jump_0");
                        return;
                    }
                }
                if (abnormalType == AbnormalType.TYPE_PUNK && myDistance < 90f && Mathf.Abs(between2) > 90f)
                {
                    float rng = UnityEngine.Random.Range(0f, 1f);
                    if (rng < 0.4f)
                    {
                        randomRun(baseTransform.position + new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f)), 10f);
                    }
                    if (rng < 0.2f)
                    {
                        recover();
                    }
                    else if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        attack2("quick_turn_l");
                    }
                    else
                    {
                        attack2("quick_turn_r");
                    }
                    return;
                }
                if (myDistance < attackDistance)
                {
                    if (abnormalType == AbnormalType.TYPE_CRAWLER)
                    {
                        if (myHero.transform.position.y + 3f <= neck.position.y + 20f * myLevel && UnityEngine.Random.Range(0f, 1f) < 0.1f)
                        {
                            chase();
                        }
                        return;
                    }
                    string text = string.Empty;
                    string[] attackStrategy = GetAttackStrategy();
                    if (attackStrategy != null)
                    {
                        text = attackStrategy[UnityEngine.Random.Range(0, attackStrategy.Length)];
                    }
                    if ((abnormalType == AbnormalType.TYPE_JUMPER || abnormalType == AbnormalType.TYPE_I) && Mathf.Abs(between2) > 40f)
                    {
                        if (text.Contains("grab") || text.Contains("kick") || text.Contains("slap") || text.Contains("bite"))
                        {
                            if (UnityEngine.Random.Range(0, 100) < 30)
                            {
                                turn(between2);
                                return;
                            }
                        }
                        else if (UnityEngine.Random.Range(0, 100) < 90)
                        {
                            turn(between2);
                            return;
                        }
                    }
                    if (executeAttack2(text))
                    {
                        return;
                    }
                    if (abnormalType == AbnormalType.NORMAL)
                    {
                        if (UnityEngine.Random.Range(0, 100) < 30 && Mathf.Abs(between2) > 45f)
                        {
                            turn(between2);
                            return;
                        }
                    }
                    else if (Mathf.Abs(between2) > 45f)
                    {
                        turn(between2);
                        return;
                    }
                }
                if (!(PVPfromCheckPt != null))
                {
                    return;
                }
                if (PVPfromCheckPt.state == CheckPointState.Titan)
                {
                    if (UnityEngine.Random.Range(0, 100) > 48)
                    {
                        GameObject chkPtNext = PVPfromCheckPt.chkPtNext;
                        if (chkPtNext != null && (chkPtNext.GetComponent<PVPcheckPoint>().state != CheckPointState.Titan || UnityEngine.Random.Range(0, 100) < 20))
                        {
                            toPVPCheckPoint(chkPtNext.transform.position, 5 + UnityEngine.Random.Range(0, 10));
                            PVPfromCheckPt = chkPtNext.GetComponent<PVPcheckPoint>();
                        }
                    }
                    else
                    {
                        GameObject chkPtNext = PVPfromCheckPt.chkPtPrevious;
                        if (chkPtNext != null && (chkPtNext.GetComponent<PVPcheckPoint>().state != CheckPointState.Titan || UnityEngine.Random.Range(0, 100) < 5))
                        {
                            toPVPCheckPoint(chkPtNext.transform.position, 5 + UnityEngine.Random.Range(0, 10));
                            PVPfromCheckPt = chkPtNext.GetComponent<PVPcheckPoint>();
                        }
                    }
                }
                else
                {
                    toPVPCheckPoint(PVPfromCheckPt.transform.position, 5 + UnityEngine.Random.Range(0, 10));
                }
                break;
            case TitanState.attack:
                if (attackAnimation == "combo")
                {
                    if (nonAI)
                    {
                        if (controller.isAttackDown)
                        {
                            nonAIcombo = true;
                        }
                        if (!nonAIcombo && baseAnimation["attack_" + attackAnimation].normalizedTime >= 0.385f)
                        {
                            idle();
                            return;
                        }
                    }
                    if (baseAnimation["attack_" + attackAnimation].normalizedTime >= 0.11f && baseAnimation["attack_" + attackAnimation].normalizedTime <= 0.16f)
                    {
                        GameObject gameObject3 = checkIfHitHand(baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001"));
                        if (gameObject3 != null)
                        {
                            Vector3 position = baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
                            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                            {
                                gameObject3.GetComponent<HERO>().die((gameObject3.transform.position - position) * 15f * myLevel, isBite: false);
                            }
                            else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject3.GetComponent<HERO>().HasDied())
                            {
                                gameObject3.GetComponent<HERO>().markDie();
                                object[] parameters2 = new object[5]
                                {
                                    (gameObject3.transform.position - position) * 15f * myLevel,
                                    false,
                                    (!nonAI) ? (-1) : base.photonView.viewID,
                                    base.name,
                                    true
                                };
                                gameObject3.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters2);
                            }
                        }
                    }
                    if (baseAnimation["attack_" + attackAnimation].normalizedTime >= 0.27f && baseAnimation["attack_" + attackAnimation].normalizedTime <= 0.32f)
                    {
                        GameObject gameObject4 = checkIfHitHand(baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001"));
                        if (gameObject4 != null)
                        {
                            Vector3 position2 = baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
                            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                            {
                                gameObject4.GetComponent<HERO>().die((gameObject4.transform.position - position2) * 15f * myLevel, isBite: false);
                            }
                            else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject4.GetComponent<HERO>().HasDied())
                            {
                                gameObject4.GetComponent<HERO>().markDie();
                                object[] parameters3 = new object[5]
                                {
                                (gameObject4.transform.position - position2) * 15f * myLevel,
                                false,
                                (!nonAI) ? (-1) : base.photonView.viewID,
                                base.name,
                                true
                                };
                                gameObject4.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters3);
                            }
                        }
                    }
                }
                if (attackCheckTimeA != 0f && baseAnimation["attack_" + attackAnimation].normalizedTime >= attackCheckTimeA && baseAnimation["attack_" + attackAnimation].normalizedTime <= attackCheckTimeB)
                {
                    if (leftHandAttack)
                    {
                        GameObject gameObject5 = checkIfHitHand(baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001"));
                        if (gameObject5 != null)
                        {
                            Vector3 position3 = baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
                            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                            {
                                gameObject5.GetComponent<HERO>().die((gameObject5.transform.position - position3) * 15f * myLevel, isBite: false);
                            }
                            else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject5.GetComponent<HERO>().HasDied())
                            {
                                gameObject5.GetComponent<HERO>().markDie();
                                object[] parameters4 = new object[5]
                                {
                                (gameObject5.transform.position - position3) * 15f * myLevel,
                                false,
                                (!nonAI) ? (-1) : base.photonView.viewID,
                                base.name,
                                true
                                };
                                gameObject5.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters4);
                            }
                        }
                    }
                    else
                    {
                        GameObject gameObject6 = checkIfHitHand(baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001"));
                        if (gameObject6 != null)
                        {
                            Vector3 position4 = baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
                            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                            {
                                gameObject6.GetComponent<HERO>().die((gameObject6.transform.position - position4) * 15f * myLevel, isBite: false);
                            }
                            else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject6.GetComponent<HERO>().HasDied())
                            {
                                gameObject6.GetComponent<HERO>().markDie();
                                object[] parameters5 = new object[5]
                                {
                                (gameObject6.transform.position - position4) * 15f * myLevel,
                                false,
                                (!nonAI) ? (-1) : base.photonView.viewID,
                                base.name,
                                true
                                };
                                gameObject6.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters5);
                            }
                        }
                    }
                }
                if (!attacked && attackCheckTime != 0f && baseAnimation["attack_" + attackAnimation].normalizedTime >= attackCheckTime)
                {
                    attacked = true;
                    fxPosition = baseTransform.Find("ap_" + attackAnimation).position;
                    GameObject gameObject7 = (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/" + fxName), fxPosition, fxRotation)) : PhotonNetwork.Instantiate("FX/" + fxName, fxPosition, fxRotation, 0);
                    if (nonAI)
                    {
                        gameObject7.transform.localScale = baseTransform.localScale * 1.5f;
                        if (gameObject7.GetComponent<EnemyfxIDcontainer>() != null)
                        {
                            gameObject7.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.photonView.viewID;
                        }
                    }
                    else
                    {
                        gameObject7.transform.localScale = baseTransform.localScale;
                    }
                    if (gameObject7.GetComponent<EnemyfxIDcontainer>() != null)
                    {
                        gameObject7.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
                    }
                    float b = 1f - Vector3.Distance(currentCamera.transform.position, gameObject7.transform.position) * 0.05f;
                    b = Mathf.Min(1f, b);
                    currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(b, b);
                }
                if (attackAnimation == "throw")
                {
                    if (!attacked && baseAnimation["attack_" + attackAnimation].normalizedTime >= 0.11f)
                    {
                        attacked = true;
                        Transform transform = baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
                        {
                            throwRock = PhotonNetwork.Instantiate("FX/rockThrow", transform.position, transform.rotation, 0);
                        }
                        else
                        {
                            throwRock = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/rockThrow"), transform.position, transform.rotation);
                        }
                        throwRock.transform.localScale = baseTransform.localScale;
                        throwRock.transform.position -= throwRock.transform.forward * 2.5f * myLevel;
                        if (throwRock.GetComponent<EnemyfxIDcontainer>() != null)
                        {
                            if (nonAI)
                            {
                                throwRock.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.photonView.viewID;
                            }
                            throwRock.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
                        }
                        throwRock.transform.parent = transform;
                        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
                        {
                            object[] parameters6 = new object[4]
                            {
                            base.photonView.viewID,
                            baseTransform.localScale,
                            throwRock.transform.localPosition,
                            myLevel
                            };
                            throwRock.GetPhotonView().RPC("initRPC", PhotonTargets.Others, parameters6);
                        }
                    }
                    if (baseAnimation["attack_" + attackAnimation].normalizedTime >= 0.11f)
                    {
                        float y = Mathf.Atan2(myHero.transform.position.x - baseTransform.position.x, myHero.transform.position.z - baseTransform.position.z) * 57.29578f;
                        baseGameObjectTransform.rotation = Quaternion.Euler(0f, y, 0f);
                    }
                    if (throwRock != null && baseAnimation["attack_" + attackAnimation].normalizedTime >= 0.62f)
                    {
                        float num = 1f;
                        float num2 = -20f;
                        Vector3 vector2;
                        if (myHero != null)
                        {
                            vector2 = (myHero.transform.position - throwRock.transform.position) / num + myHero.rigidbody.velocity;
                            float num3 = myHero.transform.position.y + 2f * myLevel;
                            float num4 = num3 - throwRock.transform.position.y;
                            vector2 = new Vector3(vector2.x, num4 / num - 0.5f * num2 * num, vector2.z);
                        }
                        else
                        {
                            vector2 = baseTransform.forward * 60f + Vector3.up * 10f;
                        }
                        throwRock.GetComponent<RockThrow>().launch(vector2);
                        throwRock.transform.parent = null;
                        throwRock = null;
                    }
                }
                switch (attackAnimation)
                {
                    case "jumper_0":
                    case "crawler_jump_0":
                        if (!attacked)
                        {
                            if (baseAnimation["attack_" + attackAnimation].normalizedTime >= 0.68f)
                            {
                                attacked = true;
                                if (myHero == null || nonAI)
                                {
                                    float d = 120f;
                                    Vector3 velocity = baseTransform.forward * speed + Vector3.up * d;
                                    if (nonAI && abnormalType == AbnormalType.TYPE_CRAWLER)
                                    {
                                        d = 100f;
                                        float a = speed * 2.5f;
                                        a = Mathf.Min(a, 100f);
                                        velocity = baseTransform.forward * a + Vector3.up * d;
                                    }
                                    baseRigidBody.velocity = velocity;
                                }
                                else
                                {
                                    float y2 = myHero.rigidbody.velocity.y;
                                    float num5 = -20f;
                                    float num6 = gravity;
                                    float y3 = neck.position.y;
                                    float num7 = (num5 - num6) * 0.5f;
                                    float num8 = y2;
                                    float num9 = myHero.transform.position.y - y3;
                                    float d2 = Mathf.Abs((Mathf.Sqrt(num8 * num8 - 4f * num7 * num9) - num8) / (2f * num7));
                                    Vector3 a2 = myHero.transform.position + myHero.rigidbody.velocity * d2 + Vector3.up * 0.5f * num5 * d2 * d2;
                                    float y4 = a2.y;
                                    float d3;
                                    if (num9 < 0f || y4 - y3 < 0f)
                                    {
                                        d3 = 60f;
                                        float a3 = speed * 2.5f;
                                        a3 = Mathf.Min(a3, 100f);
                                        Vector3 velocity2 = baseTransform.forward * a3 + Vector3.up * d3;
                                        baseRigidBody.velocity = velocity2;
                                        return;
                                    }
                                    float num10 = y4 - y3;
                                    float num11 = Mathf.Sqrt(2f * num10 / gravity);
                                    d3 = gravity * num11;
                                    d3 = Mathf.Max(30f, d3);
                                    Vector3 vector3 = (a2 - baseTransform.position) / d2;
                                    abnorma_jump_bite_horizon_v = new Vector3(vector3.x, 0f, vector3.z);
                                    Vector3 velocity3 = baseRigidBody.velocity;
                                    Vector3 force = new Vector3(abnorma_jump_bite_horizon_v.x, velocity3.y, abnorma_jump_bite_horizon_v.z) - velocity3;
                                    baseRigidBody.AddForce(force, ForceMode.VelocityChange);
                                    baseRigidBody.AddForce(Vector3.up * d3, ForceMode.VelocityChange);
                                    float num12 = Vector2.Angle(new Vector2(baseTransform.position.x, baseTransform.position.z), new Vector2(myHero.transform.position.x, myHero.transform.position.z));
                                    num12 = Mathf.Atan2(myHero.transform.position.x - baseTransform.position.x, myHero.transform.position.z - baseTransform.position.z) * 57.29578f;
                                    baseGameObjectTransform.rotation = Quaternion.Euler(0f, num12, 0f);
                                }
                            }
                            else
                            {
                                baseRigidBody.velocity = Vector3.zero;
                            }
                        }
                        if (!(baseAnimation["attack_" + attackAnimation].normalizedTime >= 1f))
                        {
                            return;
                        }
                        GameObject gameObject8 = checkIfHitHead(head, 3f);
                        if (gameObject8 != null)
                        {
                            Vector3 position5 = baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
                            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                            {
                                gameObject8.GetComponent<HERO>().die((gameObject8.transform.position - position5) * 15f * myLevel, isBite: false);
                            }
                            else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject8.GetComponent<HERO>().HasDied())
                            {
                                gameObject8.GetComponent<HERO>().markDie();
                                object[] parameters7 = new object[5]
                                {
                            (gameObject8.transform.position - position5) * 15f * myLevel,
                            true,
                            (!nonAI) ? (-1) : base.photonView.viewID,
                            base.name,
                            true
                                };
                                gameObject8.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters7);
                            }
                            if (abnormalType == AbnormalType.TYPE_CRAWLER)
                            {
                                attackAnimation = "crawler_jump_1";
                            }
                            else
                            {
                                attackAnimation = "jumper_1";
                            }
                            playAnimation("attack_" + attackAnimation);
                        }
                        if (Mathf.Abs(baseRigidBody.velocity.y) < 0.5f || baseRigidBody.velocity.y < 0f || IsGrounded())
                        {
                            if (abnormalType == AbnormalType.TYPE_CRAWLER)
                            {
                                attackAnimation = "crawler_jump_1";
                            }
                            else
                            {
                                attackAnimation = "jumper_1";
                            }
                            playAnimation("attack_" + attackAnimation);
                        }
                        break;
                    case "jumper_1":
                    case "crawler_jump_1":
                        if (baseAnimation["attack_" + attackAnimation].normalizedTime >= 1f && grounded)
                        {
                            if (abnormalType == AbnormalType.TYPE_CRAWLER)
                            {
                                attackAnimation = "crawler_jump_2";
                            }
                            else
                            {
                                attackAnimation = "jumper_2";
                            }
                            crossFade("attack_" + attackAnimation, 0.1f);
                            fxPosition = baseTransform.position;
                            GameObject gameObject9 = (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/boom2"), fxPosition, fxRotation)) : PhotonNetwork.Instantiate("FX/boom2", fxPosition, fxRotation, 0);
                            gameObject9.transform.localScale = baseTransform.localScale * 1.6f;
                            float b2 = 1f - Vector3.Distance(currentCamera.transform.position, gameObject9.transform.position) * 0.05f;
                            b2 = Mathf.Min(1f, b2);
                            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(b2, b2);
                        }
                        break;
                    case "jumper_2":
                    case "crawler_jump_2":
                        if (baseAnimation["attack_" + attackAnimation].normalizedTime >= 1f)
                        {
                            idle();
                        }
                        break;
                    default:
                        if (baseAnimation.IsPlaying("tired"))
                        {
                            if (baseAnimation["tired"].normalizedTime >= 1f + Mathf.Max(attackEndWait * 2f, 3f))
                            {
                                idle(UnityEngine.Random.Range(attackWait - 1f, 3f));
                            }
                        }
                        else
                        {
                            if (!(baseAnimation["attack_" + attackAnimation].normalizedTime >= 1f + attackEndWait))
                            {
                                return;
                            }
                            if (nextAttackAnimation != null)
                            {
                                attack2(nextAttackAnimation);
                            }
                            else if (attackAnimation == "quick_turn_l" || attackAnimation == "quick_turn_r")
                            {
                                baseTransform.rotation = Quaternion.Euler(baseTransform.rotation.eulerAngles.x, baseTransform.rotation.eulerAngles.y + 180f, baseTransform.rotation.eulerAngles.z);
                                idle(UnityEngine.Random.Range(0.5f, 1f));
                                playAnimation("idle");
                            }
                            else if (abnormalType == AbnormalType.TYPE_I || abnormalType == AbnormalType.TYPE_JUMPER)
                            {
                                attackCount++;
                                if (attackCount > 3 && attackAnimation == "abnormal_getup")
                                {
                                    attackCount = 0;
                                    crossFade("tired", 0.5f);
                                }
                                else
                                {
                                    idle(UnityEngine.Random.Range(attackWait - 1f, 3f));
                                }
                            }
                            else
                            {
                                idle(UnityEngine.Random.Range(attackWait - 1f, 3f));
                            }
                        }
                        break;
                }
                break;
            case TitanState.grab:
                if (baseAnimation["grab_" + attackAnimation].normalizedTime >= attackCheckTimeA && baseAnimation["grab_" + attackAnimation].normalizedTime <= attackCheckTimeB && grabbedTarget == null)
                {
                    GameObject gameObject10 = checkIfHitHand(currentGrabHand);
                    if (gameObject10 != null)
                    {
                        if (isGrabHandLeft)
                        {
                            eatSetL(gameObject10);
                            grabbedTarget = gameObject10;
                        }
                        else
                        {
                            eatSet(gameObject10);
                            grabbedTarget = gameObject10;
                        }
                    }
                }
                if (baseAnimation["grab_" + attackAnimation].normalizedTime >= 1f)
                {
                    if (grabbedTarget != null)
                    {
                        eat();
                    }
                    else
                    {
                        idle(UnityEngine.Random.Range(attackWait - 1f, 2f));
                    }
                }
                break;
            case TitanState.eat:
                if (!attacked && baseAnimation[attackAnimation].normalizedTime >= 0.48f)
                {
                    attacked = true;
                    justEatHero(grabbedTarget, currentGrabHand);
                }
                if (baseAnimation[attackAnimation].normalizedTime >= 1f)
                {
                    idle();
                }
                break;
            case TitanState.chase:
                if (myHero == null)
                {
                    idle();
                }
                else
                {
                    if (longRangeAttackCheck2())
                    {
                        return;
                    }
                    if (IN_GAME_MAIN_CAMERA.Gamemode == GAMEMODE.PVP_CAPTURE && PVPfromCheckPt != null && myDistance > chaseDistance)
                    {
                        idle();
                    }
                    else if (abnormalType == AbnormalType.TYPE_CRAWLER)
                    {
                        Vector3 vector4 = myHero.transform.position - baseTransform.position;
                        float current = (0f - Mathf.Atan2(vector4.z, vector4.x)) * 57.29578f;
                        float f = 0f - Mathf.DeltaAngle(current, baseGameObjectTransform.rotation.eulerAngles.y - 90f);
                        if (myDistance < attackDistance * 3f && UnityEngine.Random.Range(0f, 1f) < 0.1f && Mathf.Abs(f) < 90f && myHero.transform.position.y < neck.position.y + 30f * myLevel && myHero.transform.position.y > neck.position.y + 10f * myLevel)
                        {
                            attack2("crawler_jump_0");
                            return;
                        }
                        GameObject gameObject11 = checkIfHitCrawlerMouth(head, 2.2f);
                        if (gameObject11 != null)
                        {
                            Vector3 position6 = baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
                            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
                            {
                                gameObject11.GetComponent<HERO>().die((gameObject11.transform.position - position6) * 15f * myLevel, isBite: false);
                            }
                            else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
                            {
                                if (gameObject11.GetComponent<TITAN_EREN>() != null)
                                {
                                    gameObject11.GetComponent<TITAN_EREN>().hitByTitan();
                                }
                                else if (!gameObject11.GetComponent<HERO>().HasDied())
                                {
                                    gameObject11.GetComponent<HERO>().markDie();
                                    object[] parameters8 = new object[5]
                                    {
                                    (gameObject11.transform.position - position6) * 15f * myLevel,
                                    true,
                                    (!nonAI) ? (-1) : base.photonView.viewID,
                                    base.name,
                                    true
                                    };
                                    gameObject11.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters8);
                                }
                            }
                        }
                        if (myDistance < attackDistance && UnityEngine.Random.Range(0f, 1f) < 0.02f)
                        {
                            idle(UnityEngine.Random.Range(0.05f, 0.2f));
                        }
                    }
                    else if (abnormalType == AbnormalType.TYPE_JUMPER && ((myDistance > attackDistance && myHero.transform.position.y > head.position.y + 4f * myLevel) || myHero.transform.position.y > head.position.y + 4f * myLevel) && Vector3.Distance(baseTransform.position, myHero.transform.position) < 1.5f * myHero.transform.position.y)
                    {
                        attack2("jumper_0");
                    }
                    else if (myDistance < attackDistance)
                    {
                        idle(UnityEngine.Random.Range(0.05f, 0.2f));
                    }
                }
                break;
            case TitanState.wander:
                if (myDistance < chaseDistance || whoHasTauntMe != null)
                {
                    Vector3 vector5 = myHero.transform.position - baseTransform.position;
                    float num13 = (0f - Mathf.Atan2(vector5.z, vector5.x)) * 57.29578f;
                    float num14 = 0f - Mathf.DeltaAngle(num13, baseGameObjectTransform.rotation.eulerAngles.y - 90f);
                    if (isAlarm || Mathf.Abs(num14) < 90f)
                    {
                        chase();
                        return;
                    }
                    if (!isAlarm && myDistance < chaseDistance * 0.1f)
                    {
                        chase();
                        return;
                    }
                }
                if (UnityEngine.Random.Range(0f, 1f) < 0.01f)
                {
                    idle();
                }
                break;
            case TitanState.turn:
                baseGameObjectTransform.rotation = Quaternion.Lerp(baseGameObjectTransform.rotation, Quaternion.Euler(0f, desDeg, 0f), Time.deltaTime * Mathf.Abs(turnDeg) * 0.015f);
                if (baseAnimation[turnAnimation].normalizedTime >= 1f)
                {
                    idle();
                }
                break;
            case TitanState.hit_eye:
                if (baseAnimation.IsPlaying("sit_hit_eye") && baseAnimation["sit_hit_eye"].normalizedTime >= 1f)
                {
                    remainSitdown();
                }
                else if (baseAnimation.IsPlaying("hit_eye") && baseAnimation["hit_eye"].normalizedTime >= 1f)
                {
                    if (nonAI)
                    {
                        idle();
                    }
                    else
                    {
                        attack2("combo_1");
                    }
                }
                break;
            case TitanState.to_check_point:
                if (checkPoints.Count <= 0 && myDistance < attackDistance)
                {
                    string decidedAction = string.Empty;
                    string[] attackStrategy2 = GetAttackStrategy();
                    if (attackStrategy2 != null)
                    {
                        decidedAction = attackStrategy2[UnityEngine.Random.Range(0, attackStrategy2.Length)];
                    }
                    if (executeAttack2(decidedAction))
                    {
                        return;
                    }
                }
                if (!(Vector3.Distance(baseTransform.position, targetCheckPt) < targetR))
                {
                    return;
                }
                if (checkPoints.Count > 0)
                {
                    if (checkPoints.Count == 1)
                    {
                        if (IN_GAME_MAIN_CAMERA.Gamemode == GAMEMODE.BOSS_FIGHT_CT)
                        {
                            FengGameManagerMKII.Instance.gameLose2();
                            checkPoints = new ArrayList();
                            idle();
                        }
                    }
                    else
                    {
                        if (checkPoints.Count == 4)
                        {
                            FengGameManagerMKII.Instance.sendChatContentInfo("<color=#A8FF24>*WARNING* An abnormal titan is approaching the North gate!</color>");
                        }
                        checkPoints.RemoveAt(0);
                    }
                }
                else
                {
                    idle();
                }
                break;
            case TitanState.to_pvp_pt:
                if (myDistance < chaseDistance * 0.7f)
                {
                    chase();
                }
                if (Vector3.Distance(baseTransform.position, targetCheckPt) < targetR)
                {
                    idle();
                }
                break;
            case TitanState.random_run:
                random_run_time -= Time.deltaTime;
                if (Vector3.Distance(baseTransform.position, targetCheckPt) < targetR || random_run_time <= 0f)
                {
                    idle();
                }
                break;
            case TitanState.down:
                getdownTime -= Time.deltaTime;
                if (baseAnimation.IsPlaying("sit_hunt_down") && baseAnimation["sit_hunt_down"].normalizedTime >= 1f)
                {
                    playAnimation("sit_idle");
                }
                if (getdownTime <= 0f)
                {
                    crossFade("sit_getup", 0.1f);
                }
                if (baseAnimation.IsPlaying("sit_getup") && baseAnimation["sit_getup"].normalizedTime >= 1f)
                {
                    idle();
                }
                break;
            case TitanState.sit:
                getdownTime -= Time.deltaTime;
                angle = 0f;
                between2 = 0f;
                if (myDistance < chaseDistance || whoHasTauntMe != null)
                {
                    if (myDistance < 50f)
                    {
                        isAlarm = true;
                    }
                    else
                    {
                        Vector3 vector7 = myHero.transform.position - baseTransform.position;
                        angle = (0f - Mathf.Atan2(vector7.z, vector7.x)) * 57.29578f;
                        between2 = 0f - Mathf.DeltaAngle(angle, baseGameObjectTransform.rotation.eulerAngles.y - 90f);
                        if (Mathf.Abs(between2) < 100f)
                        {
                            isAlarm = true;
                        }
                    }
                }
                if (baseAnimation.IsPlaying("sit_down") && baseAnimation["sit_down"].normalizedTime >= 1f)
                {
                    playAnimation("sit_idle");
                }
                if ((getdownTime <= 0f || isAlarm) && baseAnimation.IsPlaying("sit_idle"))
                {
                    crossFade("sit_getup", 0.1f);
                }
                if (baseAnimation.IsPlaying("sit_getup") && baseAnimation["sit_getup"].normalizedTime >= 1f)
                {
                    idle();
                }
                break;
            case TitanState.recover:
                getdownTime -= Time.deltaTime;
                if (getdownTime <= 0f)
                {
                    idle();
                }
                if (baseAnimation.IsPlaying("idle_recovery") && baseAnimation["idle_recovery"].normalizedTime >= 1f)
                {
                    idle();
                }
                break;
        }
    }

    public void setAbnormalType2(AbnormalType type, bool forceCrawler)
    {
        bool flag = false;
        if (RCSettings.SpawnMode > 0 || ((int)FengGameManagerMKII.Settings[91] == 1 && IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient))
        {
            flag = true;
        }
        if (FengGameManagerMKII.level.StartsWith("Custom"))
        {
            flag = true;
        }
        int num = 0;
        float num2 = 0.02f * (float)(IN_GAME_MAIN_CAMERA.Difficulty + 1);
        if (IN_GAME_MAIN_CAMERA.Gamemode == GAMEMODE.PVP_AHSS)
        {
            num2 = 100f;
        }
        switch (type)
        {
            case AbnormalType.NORMAL:
                num = ((UnityEngine.Random.Range(0f, 1f) < num2) ? 4 : 0);
                if (flag)
                {
                    num = 0;
                }
                break;
            case AbnormalType.TYPE_I:
                num = ((!(UnityEngine.Random.Range(0f, 1f) < num2)) ? 1 : 4);
                if (flag)
                {
                    num = 1;
                }
                break;
            case AbnormalType.TYPE_JUMPER:
                num = ((!(UnityEngine.Random.Range(0f, 1f) < num2)) ? 2 : 4);
                if (flag)
                {
                    num = 2;
                }
                break;
            case AbnormalType.TYPE_CRAWLER:
                num = 3;
                if (GameObject.Find("Crawler") != null && UnityEngine.Random.Range(0, 1000) > 5)
                {
                    num = 2;
                }
                if (flag)
                {
                    num = 3;
                }
                break;
            case AbnormalType.TYPE_PUNK:
                num = 4;
                break;
        }
        if (forceCrawler)
        {
            num = 3;
        }
        if (num == 4)
        {
            if (!LevelInfo.GetInfo(FengGameManagerMKII.level).punk)
            {
                num = 1;
            }
            else
            {
                if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE && getPunkNumber() >= 3)
                {
                    num = 1;
                }
                if (IN_GAME_MAIN_CAMERA.Gamemode == GAMEMODE.SURVIVE_MODE)
                {
                    int wave = FengGameManagerMKII.Instance.wave;
                    int num3;
                    switch (wave)
                    {
                        default:
                            num3 = ((wave == 20) ? 1 : 0);
                            break;
                        case 15:
                            num3 = 1;
                            break;
                        case 5:
                        case 10:
                            num3 = 1;
                            break;
                    }
                    if (num3 == 0)
                    {
                        num = 1;
                    }
                }
            }
            if (flag)
            {
                num = 4;
            }
        }
        if (IN_GAME_MAIN_CAMERA.Gametype != 0 && base.photonView.isMine)
        {
            base.photonView.RPC("netSetAbnormalType", PhotonTargets.AllBuffered, num);
        }
        else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
        {
            netSetAbnormalType(num);
        }
    }

    private void setLevel2(float level, int AI, int skinColor)
    {
        myLevel = level;
        myLevel = Mathf.Clamp(myLevel, 0.1f, 50f);
        attackWait += UnityEngine.Random.Range(0f, 2f);
        chaseDistance += myLevel * 10f;
        base.transform.localScale = new Vector3(myLevel, myLevel, myLevel);
        float num = Mathf.Min(Mathf.Pow(2f / myLevel, 0.35f), 1.25f);
        headscale = new Vector3(num, num, num);
        head = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
        head.localScale = headscale;
        if (skinColor != 0)
        {
            Material material = mainMaterial.GetComponent<SkinnedMeshRenderer>().material;
            Color color;
            switch (skinColor)
            {
                case 1:
                    color = FengColor.TitanSkin1;
                    break;
                case 2:
                    color = FengColor.TitanSkin2;
                    break;
                default:
                    color = FengColor.TitanSkin3;
                    break;
            }
            material.color = color;
        }
        this.skinColor = skinColor;
        float value = 1.4f - (myLevel - 0.7f) * 0.15f;
        value = Mathf.Clamp(value, 0.9f, 1.5f);
        foreach (AnimationState item in base.animation)
        {
            item.speed = value;
        }
        base.rigidbody.mass *= myLevel;
        base.rigidbody.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0, 360), 0f);
        if (myLevel > 1f)
        {
            speed *= Mathf.Sqrt(myLevel);
        }
        myDifficulty = AI;
        if (myDifficulty == 1 || myDifficulty == 2)
        {
            foreach (AnimationState item2 in base.animation)
            {
                item2.speed = value * 1.05f;
            }
            if (nonAI)
            {
                speed *= 1.1f;
            }
            else
            {
                speed *= 1.4f;
            }
            chaseDistance *= 1.15f;
        }
        if (myDifficulty == 2)
        {
            foreach (AnimationState item3 in base.animation)
            {
                item3.speed = value * 1.05f;
            }
            if (nonAI)
            {
                speed *= 1.1f;
            }
            else
            {
                speed *= 1.5f;
            }
            chaseDistance *= 1.3f;
        }
        if (IN_GAME_MAIN_CAMERA.Gamemode == GAMEMODE.ENDLESS_TITAN || IN_GAME_MAIN_CAMERA.Gamemode == GAMEMODE.SURVIVE_MODE)
        {
            chaseDistance = 999999f;
        }
        if (nonAI)
        {
            if (abnormalType == AbnormalType.TYPE_CRAWLER)
            {
                speed = Mathf.Min(70f, speed);
            }
            else
            {
                speed = Mathf.Min(60f, speed);
            }
        }
        attackDistance = Vector3.Distance(base.transform.position, base.transform.Find("ap_front_ground").position) * 1.65f;
    }

    public void headMovement2()
    {
        if (!hasDie)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != 0)
            {
                if (base.photonView.isMine)
                {
                    targetHeadRotation = head.rotation;
                    bool flag = false;
                    if (abnormalType != AbnormalType.TYPE_CRAWLER && state != TitanState.attack && state != TitanState.down && state != TitanState.hit && state != TitanState.recover && state != TitanState.eat && state != TitanState.hit_eye && !hasDie && myDistance < 100f && myHero != null)
                    {
                        Vector3 vector = myHero.transform.position - base.transform.position;
                        angle = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
                        float value = 0f - Mathf.DeltaAngle(angle, base.transform.rotation.eulerAngles.y - 90f);
                        value = Mathf.Clamp(value, -40f, 40f);
                        float y = neck.position.y + myLevel * 2f - myHero.transform.position.y;
                        float value2 = Mathf.Atan2(y, myDistance) * 57.29578f;
                        value2 = Mathf.Clamp(value2, -40f, 30f);
                        targetHeadRotation = Quaternion.Euler(head.rotation.eulerAngles.x + value2, head.rotation.eulerAngles.y + value, head.rotation.eulerAngles.z);
                        if (!asClientLookTarget)
                        {
                            asClientLookTarget = true;
                            base.photonView.RPC("setIfLookTarget", PhotonTargets.Others, true);
                        }
                        flag = true;
                    }
                    if (!flag && asClientLookTarget)
                    {
                        asClientLookTarget = false;
                        base.photonView.RPC("setIfLookTarget", PhotonTargets.Others, false);
                    }
                    if (state == TitanState.attack || state == TitanState.hit || state == TitanState.hit_eye)
                    {
                        oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 20f);
                    }
                    else
                    {
                        oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 10f);
                    }
                }
                else
                {
                    bool flag2 = myHero != null;
                    if (flag2)
                    {
                        myDistance = (myHero.transform.position - baseTransform.position).magnitude;
                    }
                    else
                    {
                        myDistance = float.MaxValue;
                    }
                    targetHeadRotation = head.rotation;
                    if (asClientLookTarget && flag2 && myDistance < 100f)
                    {
                        Vector3 vector2 = myHero.transform.position - baseTransform.position;
                        angle = (0f - Mathf.Atan2(vector2.z, vector2.x)) * 57.29578f;
                        float value3 = 0f - Mathf.DeltaAngle(angle, baseTransform.rotation.eulerAngles.y - 90f);
                        value3 = Mathf.Clamp(value3, -40f, 40f);
                        float y2 = neck.position.y + myLevel * 2f - myHero.transform.position.y;
                        float value4 = Mathf.Atan2(y2, myDistance) * 57.29578f;
                        value4 = Mathf.Clamp(value4, -40f, 30f);
                        targetHeadRotation = Quaternion.Euler(head.rotation.eulerAngles.x + value4, head.rotation.eulerAngles.y + value3, head.rotation.eulerAngles.z);
                    }
                    if (!hasDie)
                    {
                        oldHeadRotation = Quaternion.Slerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 10f);
                    }
                }
            }
            else
            {
                targetHeadRotation = head.rotation;
                if (abnormalType != AbnormalType.TYPE_CRAWLER && state != TitanState.attack && state != TitanState.down && state != TitanState.hit && state != TitanState.recover && state != TitanState.hit_eye && !hasDie && myDistance < 100f && myHero != null)
                {
                    Vector3 vector3 = myHero.transform.position - base.transform.position;
                    angle = (0f - Mathf.Atan2(vector3.z, vector3.x)) * 57.29578f;
                    float value5 = 0f - Mathf.DeltaAngle(angle, base.transform.rotation.eulerAngles.y - 90f);
                    value5 = Mathf.Clamp(value5, -40f, 40f);
                    float y3 = neck.position.y + myLevel * 2f - myHero.transform.position.y;
                    float value6 = Mathf.Atan2(y3, myDistance) * 57.29578f;
                    value6 = Mathf.Clamp(value6, -40f, 30f);
                    targetHeadRotation = Quaternion.Euler(head.rotation.eulerAngles.x + value6, head.rotation.eulerAngles.y + value5, head.rotation.eulerAngles.z);
                }
                if (state == TitanState.attack || state == TitanState.hit || state == TitanState.hit_eye)
                {
                    oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 20f);
                }
                else
                {
                    oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 10f);
                }
            }
            head.rotation = oldHeadRotation;
        }
        if (!base.animation.IsPlaying("die_headOff"))
        {
            head.localScale = headscale;
        }
    }

    [RPC]
    public void DieByCannon(int viewID)
    {
        PhotonView photonView = PhotonView.Find(viewID);
        if (photonView != null)
        {
            int damage = 0;
            if (PhotonNetwork.isMasterClient)
            {
                OnTitanDie(photonView);
            }
            if (nonAI)
            {
                FengGameManagerMKII.Instance.titanGetKill(photonView.owner, damage, (string)PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]);
            }
            else
            {
                FengGameManagerMKII.Instance.titanGetKill(photonView.owner, damage, base.name);
            }
        }
        else
        {
            FengGameManagerMKII.Instance.photonView.RPC("netShowDamage", photonView.owner, speed);
        }
    }

    public void OnTitanDie(PhotonView view)
    {
        if (FengGameManagerMKII.LogicLoaded && FengGameManagerMKII.RCEvents.ContainsKey("OnTitanDie"))
        {
            RCEvent rCEvent = (RCEvent)FengGameManagerMKII.RCEvents["OnTitanDie"];
            string[] array = (string[])FengGameManagerMKII.RCVariableNames["OnTitanDie"];
            if (FengGameManagerMKII.TitanVariables.ContainsKey(array[0]))
            {
                FengGameManagerMKII.TitanVariables[array[0]] = this;
            }
            else
            {
                FengGameManagerMKII.TitanVariables.Add(array[0], this);
            }
            if (FengGameManagerMKII.PlayerVariables.ContainsKey(array[1]))
            {
                FengGameManagerMKII.PlayerVariables[array[1]] = view.owner;
            }
            else
            {
                FengGameManagerMKII.PlayerVariables.Add(array[1], view.owner);
            }
            rCEvent.CheckEvent();
        }
    }

    public void resetLevel(float level)
    {
        myLevel = level;
        setmyLevel();
    }

    [RPC]
    public void moveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            base.transform.position = new Vector3(posX, posY, posZ);
        }
    }

    public void moveTo(float posX, float posY, float posZ)
    {
        base.transform.position = new Vector3(posX, posY, posZ);
    }

    public IEnumerator ReloadSky()
    {
        yield return new WaitForSeconds(0.5f);
        if (FengGameManagerMKII.SkyMaterial != null && Camera.main.GetComponent<Skybox>().material != FengGameManagerMKII.SkyMaterial)
        {
            Camera.main.GetComponent<Skybox>().material = FengGameManagerMKII.SkyMaterial;
        }
    }

    public void cache()
    {
        fengGame = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
        baseAudioSource = base.transform.Find("snd_titan_foot").GetComponent<AudioSource>();
        baseAnimation = base.animation;
        baseTransform = base.transform;
        baseRigidBody = base.rigidbody;
        baseColliders = new List<Collider>();
        Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
        foreach (Collider collider in componentsInChildren)
        {
            if (collider.name != "AABB")
            {
                baseColliders.Add(collider);
            }
        }
        GameObject gameObject = new GameObject();
        gameObject.name = "PlayerDetectorRC";
        CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        CapsuleCollider component = baseTransform.Find("AABB").GetComponent<CapsuleCollider>();
        capsuleCollider.center = component.center;
        capsuleCollider.radius = Math.Abs(baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position.y - baseTransform.position.y);
        capsuleCollider.height = component.height * 1.2f;
        capsuleCollider.material = component.material;
        capsuleCollider.isTrigger = true;
        capsuleCollider.name = "PlayerDetectorRC";
        myTitanTrigger = gameObject.AddComponent<TitanTrigger>();
        myTitanTrigger.isCollide = false;
        gameObject.layer = 16;
        gameObject.transform.parent = baseTransform.Find("AABB");
        gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE || base.photonView.isMine)
        {
            baseGameObjectTransform = base.gameObject.transform;
        }
    }

    public void updateLabel()
    {
        if (healthLabel != null && healthLabel.GetComponent<UILabel>().isVisible)
        {
            healthLabel.transform.LookAt(2f * healthLabel.transform.position - Camera.main.transform.position);
        }
    }

    public void updateCollider()
    {
        if (colliderEnabled)
        {
            if (!isHooked && !myTitanTrigger.isCollide && !isLook)
            {
                foreach (Collider baseCollider in baseColliders)
                {
                    if (baseCollider != null)
                    {
                        baseCollider.enabled = false;
                    }
                }
                colliderEnabled = false;
            }
        }
        else if (isHooked || myTitanTrigger.isCollide || isLook)
        {
            foreach (Collider baseCollider2 in baseColliders)
            {
                if (baseCollider2 != null)
                {
                    baseCollider2.enabled = true;
                }
            }
            colliderEnabled = true;
        }
    }

    public void explode()
    {
        if (RCSettings.ExplodeMode <= 0 || !hasDie || !(dieTime >= 1f) || hasExplode)
        {
            return;
        }
        int num = 0;
        float d = myLevel * 10f;
        if (abnormalType == AbnormalType.TYPE_CRAWLER)
        {
            if (dieTime >= 2f)
            {
                hasExplode = true;
                d = 0f;
                num = 1;
            }
        }
        else
        {
            num = 1;
            hasExplode = true;
        }
        if (num != 1)
        {
            return;
        }
        Vector3 vector = baseTransform.position + Vector3.up * d;
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
        {
            GameObject.Instantiate(Resources.Load("FX/Thunder"), vector, Quaternion.Euler(270f, 0f, 0f));
            GameObject.Instantiate(Resources.Load("FX/boom1"), vector, Quaternion.Euler(270f, 0f, 0f));
        }
        else
        {
            PhotonNetwork.Instantiate("FX/Thunder", vector, Quaternion.Euler(270f, 0f, 0f), 0);
            PhotonNetwork.Instantiate("FX/boom1", vector, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject gameObject in array)
        {
            if ((gameObject.transform.position - vector).sqrMagnitude < (float)(RCSettings.ExplodeMode * RCSettings.ExplodeMode))
            {
                HERO hero = gameObject.GetComponent<HERO>();
                hero.markDie();

                if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE)
                {
                    hero.photonView.RPC("netDie2", PhotonTargets.All, -1, "[ff0000]Explosion ");
                }
                else
                {
                    hero.die2(base.transform);
                }
            }
        }
    }

    public void loadskin()
    {
        skin = 86;
        eye = false;
        if ((IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE || base.photonView.isMine) && (int)FengGameManagerMKII.Settings[1] == 1)
        {
            int num = (int)UnityEngine.Random.Range(86f, 90f);
            int num2 = num - 60;
            if ((int)FengGameManagerMKII.Settings[32] == 1)
            {
                num2 = UnityEngine.Random.Range(26, 30);
            }
            string text = (string)FengGameManagerMKII.Settings[num];
            string text2 = (string)FengGameManagerMKII.Settings[num2];
            skin = num;
            if (text2.EndsWith(".jpg") || text2.EndsWith(".png") || text2.EndsWith(".jpeg"))
            {
                eye = true;
            }
            GetComponent<TITAN_SETUP>().setVar(skin, eye);
            if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
            {
                StartCoroutine(loadskinE(text, text2));
            }
            else
            {
                base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, text, text2);
            }
        }
    }

    [RPC]
    public void loadskinRPC(string body, string eye)
    {
        if ((int)FengGameManagerMKII.Settings[1] == 1)
        {
            StartCoroutine(loadskinE(body, eye));
        }
    }

    [RPC]
    public void labelRPC(int health, int maxHealth, PhotonMessageInfo info)
    {
        if (health < 0)
        {
            if (healthLabel != null)
            {
                UnityEngine.Object.Destroy(healthLabel);
            }
            return;
        }
        if (healthLabel == null)
        {
            healthLabel = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
            healthLabel.name = "LabelNameOverHead";
            healthLabel.transform.parent = base.transform;
            healthLabel.transform.localPosition = new Vector3(0f, 20f + 1f / myLevel, 0f);
            if (abnormalType == AbnormalType.TYPE_CRAWLER)
            {
                healthLabel.transform.localPosition = new Vector3(0f, 10f + 1f / myLevel, 0f);
            }
            float num = 1f;
            if (myLevel < 1f)
            {
                num = 1f / myLevel;
            }
            healthLabel.transform.localScale = new Vector3(num, num, num);
            healthLabelEnabled = true;
        }
        string str = "[7FFF00]";
        float num2 = (float)health / (float)maxHealth;
        if (num2 < 0.75f && num2 >= 0.5f)
        {
            str = "[f2b50f]";
        }
        else if (num2 < 0.5f && num2 >= 0.25f)
        {
            str = "[ff8100]";
        }
        else if (num2 < 0.25f)
        {
            str = "[ff3333]";
        }
        healthLabel.GetComponent<UILabel>().text = str + Convert.ToString(health);
    }

    public IEnumerator loadskinE(string body, string eye)
    {
        while (!hasSpawn)
        {
            yield return null;
        }
        bool flag = true;
        bool unload = false;
        if ((int)FengGameManagerMKII.Settings[63] == 1)
        {
            flag = false;
        }
        try
        {
            Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in componentsInChildren)
            {
                if (renderer.name.Contains("eye"))
                {
                    if (eye.ToLower() == "transparent")
                    {
                        renderer.enabled = false;
                    }
                    else if (eye.EndsWith(".jpg") || eye.EndsWith(".png") || eye.EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(eye))
                        {
                            WWW link2 = GameHelper.CreateWWW(eye);
                            if (link2 != null)
                            {
                                yield return link2;
                                Texture2D tex2 = RCextensions.LoadImage(link2, flag, 200000);
                                link2.Dispose();
                                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(eye))
                                {
                                    unload = true;
                                    renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * 4f, renderer.material.mainTextureScale.y * 8f);
                                    renderer.material.mainTextureOffset = new Vector2(0f, 0f);
                                    renderer.material.mainTexture = tex2;
                                    FengGameManagerMKII.LinkHash[0].Add(eye, renderer.material);
                                    renderer.material = (Material)FengGameManagerMKII.LinkHash[0][eye];
                                }
                                else
                                {
                                    renderer.material = (Material)FengGameManagerMKII.LinkHash[0][eye];
                                }
                            }
                        }
                        else
                        {
                            renderer.material = (Material)FengGameManagerMKII.LinkHash[0][eye];
                        }
                    }
                }
                else if (renderer.name == "hair" && (body.EndsWith(".jpg") || body.EndsWith(".png") || body.EndsWith(".jpeg")))
                {
                    if (!FengGameManagerMKII.LinkHash[2].ContainsKey(body))
                    {
                        WWW link = GameHelper.CreateWWW(body);
                        if (link != null)
                        {
                            yield return link;
                            Texture2D tex = RCextensions.LoadImage(link, flag, 1000000);
                            link.Dispose();
                            if (!FengGameManagerMKII.LinkHash[2].ContainsKey(body))
                            {
                                unload = true;
                                renderer.material = mainMaterial.GetComponent<SkinnedMeshRenderer>().material;
                                renderer.material.mainTexture = tex;
                                FengGameManagerMKII.LinkHash[2].Add(body, renderer.material);
                                renderer.material = (Material)FengGameManagerMKII.LinkHash[2][body];
                            }
                            else
                            {
                                renderer.material = (Material)FengGameManagerMKII.LinkHash[2][body];
                            }
                        }
                    }
                    else
                    {
                        renderer.material = (Material)FengGameManagerMKII.LinkHash[2][body];
                    }
                }
            }
        }
        finally
        {
        }
        if (unload)
        {
            FengGameManagerMKII.Instance.UnloadAssets();
        }
    }

    public void HandlePTInput()
    {
        if (controller.bite)
        {
            attack2("bite");
        }
        if (controller.bitel)
        {
            attack2("bite_l");
        }
        if (controller.biter)
        {
            attack2("bite_r");
        }
        if (controller.chopl)
        {
            attack2("anti_AE_low_l");
        }
        if (controller.chopr)
        {
            attack2("anti_AE_low_r");
        }
        if (controller.choptl)
        {
            attack2("anti_AE_l");
        }
        if (controller.choptr)
        {
            attack2("anti_AE_r");
        }
        if (controller.cover && stamina > 75f)
        {
            RecoverPT();
            stamina -= 75f;
        }
        if (controller.grabbackl)
        {
            grab("ground_back_l");
        }
        if (controller.grabbackr)
        {
            grab("ground_back_r");
        }
        if (controller.grabfrontl)
        {
            grab("ground_front_l");
        }
        if (controller.grabfrontr)
        {
            grab("ground_front_r");
        }
        if (controller.grabnapel)
        {
            grab("head_back_l");
        }
        if (controller.grabnaper)
        {
            grab("head_back_r");
        }
    }

    private void RecoverPT()
    {
        state = TitanState.recover;
        playAnimation("idle_recovery");
        getdownTime = UnityEngine.Random.Range(1.8f, 2.5f);
    }
}
