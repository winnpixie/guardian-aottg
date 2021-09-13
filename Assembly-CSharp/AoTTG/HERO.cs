using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xft;

public class HERO : Photon.MonoBehaviour, Anarchy.Custom.Interfaces.IAnarchyScriptHero
{
    public FengCustomInputs inputManager;
    public Camera currentCamera;
    private string skillId;
    private ParticleSystem sparks;
    private ParticleSystem smoke_3dmg;
    private float invincible = 3f;
    public GameObject myHorse;
    private bool isMounted;
    // TODO: Mod, alternative idle stance
    private string _standAnimation = "stand";
    private string standAnimation
    {
        get
        {
            return (base.photonView.isMine || IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                && Guardian.Mod.Properties.AlternateIdle.Value ? "AHSS_stand_gun" : _standAnimation;
        }
        set { _standAnimation = value; }
    }
    public GameObject speedFX;
    public GameObject speedFX1;
    private ParticleSystem speedFXPS;
    public bool useGun;
    public XWeaponTrail leftbladetrail;
    public XWeaponTrail rightbladetrail;
    public XWeaponTrail leftbladetrail2;
    public XWeaponTrail rightbladetrail2;
    private Transform handL;
    private Transform handR;
    private Transform forearmL;
    private Transform forearmR;
    private Transform upperarmL;
    private Transform upperarmR;
    public GroupType myGroup;
    public int myTeam = 1;
    private bool leftGunHasBullet = true;
    private bool rightGunHasBullet = true;
    private int leftBulletLeft = 7;
    private int rightBulletLeft = 7;
    private int bulletMAX = 7;
    public GameObject hookRefL1;
    public GameObject hookRefR1;
    public GameObject hookRefL2;
    public GameObject hookRefR2;
    public float currentSpeed;
    private Quaternion targetRotation;
    private HeroState _state;
    public HERO_SETUP setup;
    private GameObject skillCD;
    public float skillCDLast;
    public float skillCDDuration;
    public GameObject bulletLeft;
    public GameObject bulletRight;
    public float speed = 10f;
    private float gravity = 20f;
    public float maxVelocityChange = 10f;
    public bool canJump = true;
    public float jumpHeight = 2f;
    private bool grounded;
    private float facingDirection;
    private bool justGrounded;
    private bool isLaunchLeft;
    private bool isLaunchRight;
    private Vector3 launchForce;
    private bool buttonAttackRelease;
    private bool attackReleased;
    private string attackAnimation;
    public GameObject checkBoxLeft;
    public GameObject checkBoxRight;
    public AudioSource slash;
    public AudioSource slashHit;
    public AudioSource rope;
    public AudioSource meatDie;
    public AudioSource audio_hitwall;
    public AudioSource audio_ally;
    private bool QHold;
    private bool EHold;
    public Transform lastHook;
    public bool hasDied;
    private Vector3 dashDirection;
    private int attackLoop;
    private bool attackMove;
    public float myScale = 1f;
    private float wallRunTime;
    private bool wallJump;
    public float totalGas = 100f;
    private float currentGas = 100f;
    private float useGasSpeed = 0.2f;
    public float totalBladeSta = 100f;
    private float currentBladeSta = 100f;
    private int totalBladeNum = 5;
    private int currentBladeNum = 5;
    private bool throwedBlades;
    private float flare1CD;
    private float flare2CD;
    private float flare3CD;
    private float flareTotalCD = 30f;
    public GameObject myNetWorkName;
    private int escapeTimes = 1;
    private GameObject eren_titan;
    private float buffTime;
    private BUFF currentBuff;
    public bool rightArmAim;
    public bool leftArmAim;
    private GameObject gunDummy;
    private GameObject titanWhoGrabMe;
    private int titanWhoGrabMeID;
    public string currentAnimation;
    private float uTapTime = -1f;
    private float dTapTime = -1f;
    private float lTapTime = -1f;
    private float rTapTime = -1f;
    private bool dashU;
    private bool dashD;
    private bool dashL;
    private bool dashR;
    public bool titanForm;
    private bool leanLeft;
    private bool needLean;
    private bool almostSingleHook;
    private string reloadAnimation = string.Empty;
    private float dashTime;
    private Vector3 dashV;
    private float originVM;
    private bool isLeftHandHooked;
    private bool isRightHandHooked;
    private GameObject hookTarget;
    private GameObject badGuy;
    private bool hookSomeOne;
    private bool hookBySomeOne = true;
    private Vector3 launchPointLeft;
    private Vector3 launchPointRight;
    private float launchElapsedTimeL;
    private float launchElapsedTimeR;
    private Quaternion oldHeadRotation;
    private Quaternion targetHeadRotation;
    private Vector3 gunTarget;
    public float bombCD;
    public float bombRadius;
    public float bombTimeMax;
    public float bombTime;
    public float bombSpeed;
    public Vector3 currentV;
    public Vector3 targetV;
    public bool detonate;
    public string skillIDHUD;
    public bool hasspawn;
    public List<TITAN> myTitans;
    public bool bombImmune;
    public Bomb myBomb;
    public Transform baseTransform;
    public Animation baseAnimation;
    public Rigidbody baseRigidBody;
    public GameObject cross1;
    public GameObject cross2;
    public GameObject crossL1;
    public GameObject crossL2;
    public GameObject crossR1;
    public GameObject crossR2;
    public GameObject LabelDistance;
    public Dictionary<string, UISprite> cachedSprites;
    public GameObject maincamera;
    public GameObject myCannon;
    public bool isCannon;
    public Transform myCannonBase;
    public Transform myCannonPlayer;
    public CannonPropRegion myCannonRegion;
    public float skillCDLastCannon;
    public float CameraMultiplier;
    public bool isPhotonCamera;
    public GameObject myFlashlight;
    public bool isGrabbed => state == HeroState.Grabbed;

    private Material oldEyeMaterial;
    private Material oldGlassesMaterial;

    // Anarchy
    private float gasMultiplier = 1f;
    public float GasUsageModifier
    {
        get
        {
            return gasMultiplier;
        }
        set
        {
            if (gasMultiplier >= 0f)
            {
                gasMultiplier = value;
            }
        }
    }

    private HeroState state
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state == HeroState.Dashing || _state == HeroState.Dodging)
            {
                dashTime = 0f;
            }

            _state = value;
        }
    }

    // TODO: Mod, RC's fix
    private bool cancelGasDisable;
    private bool areAnimationsPaused;

    // TODO: RC reel smoothing
    private float reelInAxis;
    private float reelOutAxis;
    private float reelOutScrollTimeLeft;

    private void UpdateReelInput()
    {
        float scrollDir = Input.GetAxis("Mouse ScrollWheel") * 5555f;

        reelOutScrollTimeLeft -= Time.deltaTime;
        if (reelOutScrollTimeLeft <= 0f)
        {
            reelOutAxis = 0f;
        }

        if (((int)FengGameManagerMKII.Settings[97] == 1 && FengGameManagerMKII.InputRC.isInputHuman(InputCodeRC.ReelIn)) || (scrollDir < 0f))
        {
            reelInAxis = -1f;
        }
        if (((int)FengGameManagerMKII.Settings[116] == 1 && FengGameManagerMKII.InputRC.isInputHuman(InputCodeRC.ReelOut)) || scrollDir > 0f)
        {
            reelOutAxis = 1f;

            if (scrollDir > 0f)
            {
                reelOutScrollTimeLeft = Guardian.Mod.Properties.ReelOutScrollSmoothing.Value;
            }
        }
    }

    private float GetReelAxis()
    {
        if (reelInAxis != 0f)
        {
            return reelInAxis;
        }
        return reelOutAxis;
    }

    public bool IsInvincible()
    {
        return invincible > 0f;
    }

    [RPC]
    private void setMyTeam(int teamId)
    {
        myTeam = teamId;
        checkBoxLeft.GetComponent<TriggerColliderWeapon>().myTeam = teamId;
        checkBoxRight.GetComponent<TriggerColliderWeapon>().myTeam = teamId;
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Multiplayer || !PhotonNetwork.isMasterClient)
        {
            return;
        }
        if (RCSettings.FriendlyMode > 0)
        {
            if (teamId != 1)
            {
                base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, 1);
            }
        }
        else if (RCSettings.PvPMode == 1)
        {
            int myTeam = 0;
            if (base.photonView.owner.customProperties[PhotonPlayerProperty.RCTeam] != null)
            {
                myTeam = GExtensions.AsInt(base.photonView.owner.customProperties[PhotonPlayerProperty.RCTeam]);
            }
            if (teamId != myTeam)
            {
                base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, myTeam);
            }
        }
        else if (RCSettings.PvPMode == 2 && teamId != base.photonView.owner.Id)
        {
            base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, base.photonView.owner.Id);
        }
    }

    private void UpdateLeftMagUI()
    {
        for (int i = 1; i <= bulletMAX; i++)
        {
            GameObject.Find("bulletL" + i).GetComponent<UISprite>().enabled = false;
        }

        for (int j = 1; j <= leftBulletLeft; j++)
        {
            GameObject.Find("bulletL" + j).GetComponent<UISprite>().enabled = true;
        }
    }

    private void UpdateRightMagUI()
    {
        for (int i = 1; i <= bulletMAX; i++)
        {
            GameObject.Find("bulletR" + i).GetComponent<UISprite>().enabled = false;
        }

        for (int j = 1; j <= rightBulletLeft; j++)
        {
            GameObject.Find("bulletR" + j).GetComponent<UISprite>().enabled = true;
        }
    }

    public bool IsGrounded()
    {
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        return Physics.Raycast(layerMask: ((LayerMask)((int)mask2 | (int)mask)).value, origin: base.gameObject.transform.position + Vector3.up * 0.1f, direction: -Vector3.up, distance: 0.3f);
    }

    private bool IsFrontGrounded()
    {
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        return Physics.Raycast(layerMask: ((LayerMask)((int)mask2 | (int)mask)).value, origin: base.gameObject.transform.position + base.gameObject.transform.up * 1f, direction: base.gameObject.transform.forward, distance: 1f);
    }

    private bool IsUpFrontGrounded()
    {
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        return Physics.Raycast(layerMask: ((LayerMask)((int)mask2 | (int)mask)).value, origin: base.gameObject.transform.position + base.gameObject.transform.up * 3f, direction: base.gameObject.transform.forward, distance: 1.2f);
    }

    public void GetGrabbed(GameObject titan, bool leftHand)
    {
        if (isMounted)
        {
            Unmount();
        }

        // TODO: Mod, remove hooks once grabbed.
        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().RemoveMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().RemoveMe();
        }

        state = HeroState.Grabbed;
        GetComponent<CapsuleCollider>().isTrigger = true;
        falseAttack();
        titanWhoGrabMe = titan;
        if (titanForm && eren_titan != null)
        {
            eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
        }
        if (!useGun && (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine))
        {
            leftbladetrail.Deactivate();
            rightbladetrail.Deactivate();
            leftbladetrail2.Deactivate();
            rightbladetrail2.Deactivate();
        }
        smoke_3dmg.enableEmission = false;
        sparks.enableEmission = false;
    }

    [RPC]
    private void netGrabbed(int id, bool leftHand, PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.Validators.Hero.IsGrabValid(id, info))
        {
            titanWhoGrabMeID = id;
            GetGrabbed(PhotonView.Find(id).gameObject, leftHand);
        }
    }

    public void Ungrab()
    {
        facingDirection = 0f;
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
        base.transform.parent = null;
        GetComponent<CapsuleCollider>().isTrigger = false;
        state = HeroState.Idle;
    }

    [RPC]
    private void netSetIsGrabbedFalse()
    {
        state = HeroState.Idle;
    }

    [RPC]
    private void netUngrabbed()
    {
        Ungrab();
        netPlayAnimation(standAnimation);
        falseAttack();
    }

    public void attackAccordingToMouse()
    {
        if (Input.mousePosition.x < Screen.width * 0.5)
        {
            attackAnimation = "attack2";
        }
        else
        {
            attackAnimation = "attack1";
        }
    }

    public void attackAccordingToTarget(Transform a)
    {
        Vector3 vector = a.position - base.transform.position;
        float current = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
        Vector3 eulerAngles = base.transform.rotation.eulerAngles;
        float num = 0f - Mathf.DeltaAngle(current, eulerAngles.y - 90f);
        if (Mathf.Abs(num) < 90f && vector.magnitude < 6f)
        {
            Vector3 position = a.position;
            float y = position.y;
            Vector3 position2 = base.transform.position;
            if (y <= position2.y + 2f)
            {
                Vector3 position3 = a.position;
                float y2 = position3.y;
                Vector3 position4 = base.transform.position;
                if (y2 >= position4.y - 5f)
                {
                    attackAnimation = "attack4";
                    return;
                }
            }
        }

        attackAnimation = num > 0f ? "attack1" : "attack2";
    }

    public void PlayAnimation(string aniName)
    {
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
        {
            base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, aniName);
        }

        LocalPlayAnimation(aniName);
    }

    private void PlayAnimationAt(string aniName, float normalizedTime)
    {
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
        {
            base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, aniName, normalizedTime);
        }

        LocalPlayAnimationAt(aniName, normalizedTime);
    }

    public void CrossFade(string aniName, float time)
    {
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
        {
            base.photonView.RPC("netCrossFade", PhotonTargets.Others, aniName, time);
        }

        LocalCrossFade(aniName, time);
    }

    private void LocalPlayAnimation(string aniName)
    {
        currentAnimation = aniName;
        if (base.animation != null)
        {
            base.animation.Play(aniName);
        }
    }

    private void LocalPlayAnimationAt(string aniName, float normalizedTime)
    {
        currentAnimation = aniName;
        if (base.animation != null)
        {
            base.animation.Play(aniName);
            base.animation[aniName].normalizedTime = normalizedTime;
        }
    }

    private void LocalCrossFade(string aniName, float time)
    {
        currentAnimation = aniName;
        if (base.animation != null)
        {
            base.animation.CrossFade(aniName, time);
        }
    }

    [RPC]
    private void netPlayAnimation(string aniName, PhotonMessageInfo info = null)
    {
        if (Guardian.AntiAbuse.Validators.Hero.IsAnimationPlayValid(this, info))
        {
            LocalPlayAnimation(aniName);
        }
    }

    [RPC]
    private void netPlayAnimationAt(string aniName, float normalizedTime, PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.Validators.Hero.IsAnimationSeekedPlayValid(this, info))
        {
            LocalPlayAnimationAt(aniName, normalizedTime);
        }
    }

    [RPC]
    private void netCrossFade(string aniName, float time, PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.Validators.Hero.IsCrossFadeValid(this, info))
        {
            LocalCrossFade(aniName, time);
        }
    }

    private GameObject FindNearestTitan()
    {
        GameObject result = null;
        float num = float.PositiveInfinity;
        Vector3 position = base.transform.position;
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("titan"))
        {
            float sqrMagnitude = (gameObject.transform.position - position).sqrMagnitude;
            if (sqrMagnitude < num)
            {
                result = gameObject;
                num = sqrMagnitude;
            }
        }
        return result;
    }

    public void TransformIntoEren()
    {
        skillCDDuration = skillCDLast;
        if ((bool)bulletLeft)
        {
            bulletLeft.GetComponent<Bullet>().RemoveMe();
        }
        if ((bool)bulletRight)
        {
            bulletRight.GetComponent<Bullet>().RemoveMe();
        }
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
        {
            eren_titan = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_EREN"), base.transform.position, base.transform.rotation);
        }
        else
        {
            eren_titan = PhotonNetwork.Instantiate("TITAN_EREN", base.transform.position, base.transform.rotation, 0);
        }
        eren_titan.GetComponent<TITAN_EREN>().realBody = base.gameObject;
        maincamera.GetComponent<IN_GAME_MAIN_CAMERA>().Flash();
        maincamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(eren_titan);
        eren_titan.GetComponent<TITAN_EREN>().born();
        eren_titan.rigidbody.velocity = base.rigidbody.velocity;
        base.rigidbody.velocity = Vector3.zero;
        base.transform.position = eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
        titanForm = true;
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
        {
            base.photonView.RPC("whoIsMyErenTitan", PhotonTargets.Others, eren_titan.GetPhotonView().viewID);
        }
        if (smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && base.photonView.isMine)
        {
            base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, false);
        }
        smoke_3dmg.enableEmission = false;
    }

    public void backToHuman()
    {
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
        base.rigidbody.velocity = Vector3.zero;
        titanForm = false;
        Ungrab();
        falseAttack();
        skillCDDuration = skillCDLast;
        maincamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(base.gameObject);
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
        {
            base.photonView.RPC("backToHumanRPC", PhotonTargets.Others);
        }
    }

    [RPC]
    private void backToHumanRPC()
    {
        titanForm = false;
        eren_titan = null;
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
    }

    [RPC]
    private void whoIsMyErenTitan(int id, PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.Validators.Hero.IsErenTitanDeclarationValid(id, info))
        {
            eren_titan = PhotonView.Find(id).gameObject;
            titanForm = true;
        }
    }

    private void CheckDoubleTapDash()
    {
        if (uTapTime >= 0f)
        {
            uTapTime += Time.deltaTime;
            if (uTapTime > 0.2f)
            {
                uTapTime = -1f;
            }
        }
        if (dTapTime >= 0f)
        {
            dTapTime += Time.deltaTime;
            if (dTapTime > 0.2f)
            {
                dTapTime = -1f;
            }
        }
        if (lTapTime >= 0f)
        {
            lTapTime += Time.deltaTime;
            if (lTapTime > 0.2f)
            {
                lTapTime = -1f;
            }
        }
        if (rTapTime >= 0f)
        {
            rTapTime += Time.deltaTime;
            if (rTapTime > 0.2f)
            {
                rTapTime = -1f;
            }
        }
        if (inputManager.isInputDown[InputCode.Up])
        {
            if (uTapTime == -1f)
            {
                uTapTime = 0f;
            }
            if (uTapTime != 0f)
            {
                dashU = true;
            }
        }
        if (inputManager.isInputDown[InputCode.Down])
        {
            if (dTapTime == -1f)
            {
                dTapTime = 0f;
            }
            if (dTapTime != 0f)
            {
                dashD = true;
            }
        }
        if (inputManager.isInputDown[InputCode.Left])
        {
            if (lTapTime == -1f)
            {
                lTapTime = 0f;
            }
            if (lTapTime != 0f)
            {
                dashL = true;
            }
        }
        if (inputManager.isInputDown[InputCode.Right])
        {
            if (rTapTime == -1f)
            {
                rTapTime = 0f;
            }
            if (rTapTime != 0f)
            {
                dashR = true;
            }
        }
    }

    private float getLeanAngle(Vector3 p, bool left)
    {
        if (!useGun && state == HeroState.Attack)
        {
            return 0f;
        }
        float y = p.y;
        Vector3 position = base.transform.position;
        float num = y - position.y;
        float num2 = Vector3.Distance(p, base.transform.position);
        float num3 = Mathf.Acos(num / num2) * 57.29578f;
        num3 *= 0.1f;
        num3 *= 1f + Mathf.Pow(base.rigidbody.velocity.magnitude, 0.2f);
        Vector3 vector = p - base.transform.position;
        float current = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
        Vector3 velocity = base.rigidbody.velocity;
        float x = velocity.x;
        Vector3 velocity2 = base.rigidbody.velocity;
        float target = Mathf.Atan2(x, velocity2.z) * 57.29578f;
        float num4 = Mathf.DeltaAngle(current, target);
        num3 += Mathf.Abs(num4 * 0.5f);
        if (state != HeroState.Attack)
        {
            num3 = Mathf.Min(num3, 80f);
        }
        if (num4 > 0f)
        {
            leanLeft = true;
        }
        else
        {
            leanLeft = false;
        }
        if (useGun)
        {
            return num3 * (float)((!(num4 < 0f)) ? 1 : (-1));
        }
        float num5 = 0f;
        num5 = (((!left || !(num4 < 0f)) && (left || !(num4 > 0f))) ? 0.5f : 0.1f);
        return num3 * ((!(num4 < 0f)) ? num5 : (0f - num5));
    }

    private void LeanBody()
    {
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && !base.photonView.isMine)
        {
            return;
        }
        float z = 0f;
        needLean = false;
        if (!useGun && state == HeroState.Attack && attackAnimation != "attack3_1" && attackAnimation != "attack3_2")
        {
            Vector3 velocity = base.rigidbody.velocity;
            float y = velocity.y;
            float x = velocity.x;
            float z2 = velocity.z;
            float x2 = Mathf.Sqrt(x * x + z2 * z2);
            float num = Mathf.Atan2(y, x2) * 57.29578f;
            targetRotation = Quaternion.Euler((0f - num) * (1f - Vector3.Angle(base.rigidbody.velocity, base.transform.forward) / 90f), facingDirection, 0f);
            if ((isLeftHandHooked && bulletLeft != null) || (isRightHandHooked && bulletRight != null))
            {
                base.transform.rotation = targetRotation;
            }
            return;
        }
        if (isLeftHandHooked && bulletLeft != null && isRightHandHooked && bulletRight != null)
        {
            if (almostSingleHook)
            {
                needLean = true;
                z = getLeanAngle(bulletRight.transform.position, left: true);
            }
        }
        else if (isLeftHandHooked && bulletLeft != null)
        {
            needLean = true;
            z = getLeanAngle(bulletLeft.transform.position, left: true);
        }
        else if (isRightHandHooked && bulletRight != null)
        {
            needLean = true;
            z = getLeanAngle(bulletRight.transform.position, left: false);
        }
        if (needLean)
        {
            float num2 = 0f;
            if (!useGun && state != HeroState.Attack)
            {
                num2 = currentSpeed * 0.1f;
                num2 = Mathf.Min(num2, 20f);
            }
            targetRotation = Quaternion.Euler(0f - num2, facingDirection, z);
        }
        else if (state != HeroState.Attack)
        {
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        }
    }

    private void SetHookedPplDirection()
    {
        almostSingleHook = false;
        if (isRightHandHooked && isLeftHandHooked)
        {
            if (!(bulletLeft != null) || !(bulletRight != null))
            {
                return;
            }
            Vector3 normal = bulletLeft.transform.position - bulletRight.transform.position;
            if (normal.sqrMagnitude < 4f)
            {
                Vector3 vector = (bulletLeft.transform.position + bulletRight.transform.position) * 0.5f - base.transform.position;
                facingDirection = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
                if (useGun && state != HeroState.Attack)
                {
                    Vector3 velocity = base.rigidbody.velocity;
                    float z = velocity.z;
                    Vector3 velocity2 = base.rigidbody.velocity;
                    float current = (0f - Mathf.Atan2(z, velocity2.x)) * 57.29578f;
                    float target = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
                    float num = 0f - Mathf.DeltaAngle(current, target);
                    facingDirection += num;
                }
                almostSingleHook = true;
                return;
            }
            Vector3 to = base.transform.position - bulletLeft.transform.position;
            Vector3 to2 = base.transform.position - bulletRight.transform.position;
            Vector3 vector2 = (bulletLeft.transform.position + bulletRight.transform.position) * 0.5f;
            Vector3 from = base.transform.position - vector2;
            if (Vector3.Angle(from, to) < 30f && Vector3.Angle(from, to2) < 30f)
            {
                almostSingleHook = true;
                Vector3 vector3 = vector2 - base.transform.position;
                facingDirection = Mathf.Atan2(vector3.x, vector3.z) * 57.29578f;
                return;
            }
            almostSingleHook = false;
            Vector3 tangent = base.transform.forward;
            Vector3.OrthoNormalize(ref normal, ref tangent);
            facingDirection = Mathf.Atan2(tangent.x, tangent.z) * 57.29578f;
            float current2 = Mathf.Atan2(to.x, to.z) * 57.29578f;
            float num2 = Mathf.DeltaAngle(current2, facingDirection);
            if (num2 > 0f)
            {
                facingDirection += 180f;
            }
            return;
        }
        almostSingleHook = true;
        Vector3 zero = Vector3.zero;
        if (isRightHandHooked && bulletRight != null)
        {
            zero = bulletRight.transform.position - base.transform.position;
        }
        else
        {
            if (!isLeftHandHooked || !(bulletLeft != null))
            {
                return;
            }
            zero = bulletLeft.transform.position - base.transform.position;
        }
        facingDirection = Mathf.Atan2(zero.x, zero.z) * 57.29578f;
        if (state != HeroState.Attack)
        {
            Vector3 velocity3 = base.rigidbody.velocity;
            float z2 = velocity3.z;
            Vector3 velocity4 = base.rigidbody.velocity;
            float current3 = (0f - Mathf.Atan2(z2, velocity4.x)) * 57.29578f;
            float target2 = (0f - Mathf.Atan2(zero.z, zero.x)) * 57.29578f;
            float num3 = 0f - Mathf.DeltaAngle(current3, target2);
            if (useGun)
            {
                facingDirection += num3;
                return;
            }
            float num4 = 0f;
            num4 = (((!isLeftHandHooked || !(num3 < 0f)) && (!isRightHandHooked || !(num3 > 0f))) ? 0.1f : (-0.1f));
            facingDirection += num3 * num4;
        }
    }

    private void Idle()
    {
        if (state == HeroState.Attack)
        {
            falseAttack();
        }
        state = HeroState.Idle;
        CrossFade(standAnimation, 0.1f);
    }

    private void UpdateBuffer()
    {
        if (!(buffTime > 0f))
        {
            return;
        }
        buffTime -= Time.deltaTime;
        if (buffTime <= 0f)
        {
            buffTime = 0f;
            if (currentBuff == BUFF.Speed && base.animation.IsPlaying("run_sasha"))
            {
                CrossFade("run", 0.1f);
            }
            currentBuff = BUFF.None;
        }
    }

    private void Salute()
    {
        state = HeroState.Salute;
        CrossFade("salute", 0.1f);
    }

    private void ChangeBlade()
    {
        if (useGun && !grounded && FengGameManagerMKII.Level.Mode == GameMode.TeamDeathmatch)
        {
            return;
        }
        state = HeroState.ChangeBlade;
        throwedBlades = false;
        if (useGun)
        {
            if (!leftGunHasBullet && !rightGunHasBullet)
            {
                if (grounded)
                {
                    reloadAnimation = "AHSS_gun_reload_both";
                }
                else
                {
                    reloadAnimation = "AHSS_gun_reload_both_air";
                }
            }
            else if (!leftGunHasBullet)
            {
                if (grounded)
                {
                    reloadAnimation = "AHSS_gun_reload_l";
                }
                else
                {
                    reloadAnimation = "AHSS_gun_reload_l_air";
                }
            }
            else if (!rightGunHasBullet)
            {
                if (grounded)
                {
                    reloadAnimation = "AHSS_gun_reload_r";
                }
                else
                {
                    reloadAnimation = "AHSS_gun_reload_r_air";
                }
            }
            else
            {
                if (grounded)
                {
                    reloadAnimation = "AHSS_gun_reload_both";
                }
                else
                {
                    reloadAnimation = "AHSS_gun_reload_both_air";
                }
                leftGunHasBullet = (rightGunHasBullet = false);
            }
            CrossFade(reloadAnimation, 0.05f);
        }
        else
        {
            if (!grounded)
            {
                reloadAnimation = "changeBlade_air";
            }
            else
            {
                reloadAnimation = "changeBlade";
            }
            CrossFade(reloadAnimation, 0.1f);
        }
    }

    private void Dash(float horizontal, float vertical)
    {
        if (!(dashTime > 0f) && !(currentGas <= 0f) && !isMounted)
        {
            UseGas(totalGas * 0.04f);
            facingDirection = GetGlobalFacingDirection(horizontal, vertical);
            dashV = GetGlobalFacingVector(facingDirection);
            originVM = currentSpeed;
            Quaternion rotation = Quaternion.Euler(0f, facingDirection, 0f);
            base.rigidbody.rotation = rotation;
            targetRotation = rotation;
            if (Guardian.Mod.Properties.AlternateBurst.Value)
            {
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                {
                    UnityEngine.Object.Instantiate(Resources.Load("redCross1"), base.transform.position, base.transform.rotation);
                    UnityEngine.Object.Instantiate(Resources.Load("redCross1"), base.transform.position, base.transform.rotation);
                }
                else
                {
                    PhotonNetwork.Instantiate("redCross1", base.transform.position, base.transform.rotation, 0);
                    PhotonNetwork.Instantiate("redCross1", base.transform.position, base.transform.rotation, 0);
                }
            }
            else
            {
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                {
                    UnityEngine.Object.Instantiate(Resources.Load("FX/boost_smoke"), base.transform.position, base.transform.rotation);
                }
                else
                {
                    PhotonNetwork.Instantiate("FX/boost_smoke", base.transform.position, base.transform.rotation, 0);
                }
            }
            dashTime = 0.5f;
            CrossFade("dash", 0.1f);
            base.animation["dash"].time = 0.1f;
            state = HeroState.Dashing;
            falseAttack();
            base.rigidbody.AddForce(dashV * 40f, ForceMode.VelocityChange);
        }
    }

    private void TickSkillCooldown()
    {
        if (skillCDDuration > 0f)
        {
            skillCDDuration -= Time.deltaTime;
            if (skillCDDuration < 0f)
            {
                skillCDDuration = 0f;
            }
        }
    }

    private void TickFlareCooldown()
    {
        if (flare1CD > 0f)
        {
            flare1CD -= Time.deltaTime;
            if (flare1CD < 0f)
            {
                flare1CD = 0f;
            }
        }
        if (flare2CD > 0f)
        {
            flare2CD -= Time.deltaTime;
            if (flare2CD < 0f)
            {
                flare2CD = 0f;
            }
        }
        if (flare3CD > 0f)
        {
            flare3CD -= Time.deltaTime;
            if (flare3CD < 0f)
            {
                flare3CD = 0f;
            }
        }
    }

    private void ShowSkillCountDown()
    {
        if ((bool)skillCD)
        {
            skillCD.GetComponent<UISprite>().fillAmount = (skillCDLast - skillCDDuration) / skillCDLast;
        }
    }

    private void UseGas(float amount = 0f)
    {
        if (amount == 0f)
        {
            amount = useGasSpeed;
        }

        // BEGIN Anarchy
        amount *= GasUsageModifier;
        // END Anarchy

        if (currentGas > 0f)
        {
            currentGas -= amount;
            if (currentGas < 0f)
            {
                currentGas = 0f;
            }
        }
    }

    public void GetSupply()
    {
        if ((base.animation.IsPlaying(standAnimation) || base.animation.IsPlaying("run") || base.animation.IsPlaying("run_sasha")) && (currentBladeSta != totalBladeSta || currentBladeNum != totalBladeNum || currentGas != totalGas || leftBulletLeft != bulletMAX || rightBulletLeft != bulletMAX))
        {
            state = HeroState.FillGas;
            CrossFade("supply", 0.1f);
        }
    }

    public void FillGas()
    {
        currentGas = totalGas;
    }

    public void UseBlade(int amount = 0)
    {
        if (amount == 0)
        {
            amount = 1;
        }
        amount *= 2;
        if (!(currentBladeSta > 0f))
        {
            return;
        }
        currentBladeSta -= amount;
        if (currentBladeSta <= 0f)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine)
            {
                leftbladetrail.Deactivate();
                rightbladetrail.Deactivate();
                leftbladetrail2.Deactivate();
                rightbladetrail2.Deactivate();
                checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
            }
            currentBladeSta = 0f;
            ThrowBlades();
        }
    }

    private void ThrowBlades()
    {
        Transform transform = setup.part_blade_l.transform;
        Transform transform2 = setup.part_blade_r.transform;
        GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
        GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
        gameObject.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        gameObject2.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        Vector3 force = base.transform.forward + base.transform.up * 2f - base.transform.right;
        gameObject.rigidbody.AddForce(force, ForceMode.Impulse);
        Vector3 force2 = base.transform.forward + base.transform.up * 2f + base.transform.right;
        gameObject2.rigidbody.AddForce(force2, ForceMode.Impulse);
        Vector3 torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
        torque.Normalize();
        gameObject.rigidbody.AddTorque(torque);
        torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
        torque.Normalize();
        gameObject2.rigidbody.AddTorque(torque);
        setup.part_blade_l.SetActive(value: false);
        setup.part_blade_r.SetActive(value: false);
        currentBladeNum--;
        if (currentBladeNum == 0)
        {
            currentBladeSta = 0f;
        }
        if (state == HeroState.Attack)
        {
            falseAttack();
        }
    }

    private void LaunchLeftHook(RaycastHit hit, bool single, int mode = 0)
    {
        if (currentGas != 0f)
        {
            UseGas(0f);
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                bulletLeft = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("hook"));
            }
            else if (base.photonView.isMine)
            {
                bulletLeft = PhotonNetwork.Instantiate("hook", base.transform.position, base.transform.rotation, 0);
            }
            GameObject gameObject = (!useGun) ? hookRefL1 : hookRefL2;
            string launcher_ref = (!useGun) ? "hookRefL1" : "hookRefL2";
            bulletLeft.transform.position = gameObject.transform.position;
            Bullet component = bulletLeft.GetComponent<Bullet>();
            float d = single ? 0f : ((!(hit.distance > 50f)) ? (hit.distance * 0.05f) : (hit.distance * 0.3f));
            Vector3 a = hit.point - base.transform.right * d - bulletLeft.transform.position;
            a.Normalize();
            component.Launch(a * 3f, base.rigidbody.velocity, launcher_ref, isLeft: true, base.gameObject, leviMode: mode == 1);
            launchPointLeft = Vector3.zero;
        }
    }

    private void LaunchRightHook(RaycastHit hit, bool single, int mode = 0)
    {
        if (currentGas != 0f)
        {
            UseGas(0f);
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                bulletRight = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("hook"));
            }
            else if (base.photonView.isMine)
            {
                bulletRight = PhotonNetwork.Instantiate("hook", base.transform.position, base.transform.rotation, 0);
            }
            GameObject gameObject = (!useGun) ? hookRefR1 : hookRefR2;
            string launcher_ref = (!useGun) ? "hookRefR1" : "hookRefR2";
            bulletRight.transform.position = gameObject.transform.position;
            Bullet component = bulletRight.GetComponent<Bullet>();
            float d = single ? 0f : !(hit.distance > 50f) ? (hit.distance * 0.05f) : (hit.distance * 0.3f);
            Vector3 a = hit.point + base.transform.right * d - bulletRight.transform.position;
            a.Normalize();
            component.Launch(a * (mode == 1 ? 5f : 3f), base.rigidbody.velocity, launcher_ref, isLeft: false, base.gameObject, leviMode: mode == 1);
            launchPointRight = Vector3.zero;
        }
    }

    public void falseAttack()
    {
        attackMove = false;
        if (useGun)
        {
            if (!attackReleased)
            {
                ContinueAnimation();
                attackReleased = true;
            }
            return;
        }
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine)
        {
            checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
            checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
            checkBoxLeft.GetComponent<TriggerColliderWeapon>().ClearHits();
            checkBoxRight.GetComponent<TriggerColliderWeapon>().ClearHits();
            leftbladetrail.StopSmoothly(0.2f);
            rightbladetrail.StopSmoothly(0.2f);
            leftbladetrail2.StopSmoothly(0.2f);
            rightbladetrail2.StopSmoothly(0.2f);
        }
        attackLoop = 0;
        if (!attackReleased)
        {
            ContinueAnimation();
            attackReleased = true;
        }
    }

    private bool isPressDirectionTowardsHero(float h, float v)
    {
        if (h == 0f && v == 0f)
        {
            return false;
        }
        float globalFacingDirection = GetGlobalFacingDirection(h, v);
        Vector3 eulerAngles = base.transform.rotation.eulerAngles;
        if (Mathf.Abs(Mathf.DeltaAngle(globalFacingDirection, eulerAngles.y)) < 45f)
        {
            return true;
        }
        return false;
    }

    private void CustomAnimationSpeed()
    {
        base.animation["attack5"].speed = 1.85f;
        base.animation["changeBlade"].speed = 1.2f;
        base.animation["air_release"].speed = 0.6f;
        base.animation["changeBlade_air"].speed = 0.8f;
        base.animation["AHSS_gun_reload_both"].speed = 0.38f;
        base.animation["AHSS_gun_reload_both_air"].speed = 0.5f;
        base.animation["AHSS_gun_reload_l"].speed = 0.4f;
        base.animation["AHSS_gun_reload_l_air"].speed = 0.5f;
        base.animation["AHSS_gun_reload_r"].speed = 0.4f;
        base.animation["AHSS_gun_reload_r_air"].speed = 0.5f;
    }

    public void PauseAnimation()
    {
        if (areAnimationsPaused)
        {
            return;
        }
        areAnimationsPaused = true;

        foreach (AnimationState anim in base.animation)
        {
            anim.speed = 0f;
        }

        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && base.photonView.isMine)
        {
            base.photonView.RPC("netPauseAnimation", PhotonTargets.Others);
        }
    }

    public void ResetAnimationSpeed()
    {
        foreach (AnimationState anim in base.animation)
        {
            anim.speed = 1f;
        }
        CustomAnimationSpeed();
    }

    public void ContinueAnimation()
    {
        if (!areAnimationsPaused)
        {
            return;
        }
        areAnimationsPaused = false;

        ResetAnimationSpeed();
        PlayAnimation(GetCurrentAnimationPlaying());
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && base.photonView.isMine)
        {
            base.photonView.RPC("netContinueAnimation", PhotonTargets.Others);
        }
    }

    [RPC]
    private void netPauseAnimation(PhotonMessageInfo info)
    {
        if (!Guardian.AntiAbuse.Validators.Hero.IsAnimationPauseValid(this, info))
        {
            return;
        }
        foreach (AnimationState item in base.animation)
        {
            item.speed = 0f;
        }
    }

    [RPC]
    private void netContinueAnimation(PhotonMessageInfo info)
    {
        if (!Guardian.AntiAbuse.Validators.Hero.IsAnimationResumeValid(this, info))
        {
            return;
        }
        foreach (AnimationState item in base.animation)
        {
            if (item.speed == 1f)
            {
                return;
            }
            item.speed = 1f;
        }
        PlayAnimation(GetCurrentAnimationPlaying());
    }

    public string GetCurrentAnimationPlaying()
    {
        foreach (AnimationState anim in base.animation)
        {
            if (base.animation.IsPlaying(anim.name))
            {
                return anim.name;
            }
        }
        return string.Empty;
    }

    public void HookToHuman(GameObject target, Vector3 hookPosition)
    {
        ReleaseHookedTarget();
        hookTarget = target;
        hookSomeOne = true;
        if ((bool)target.GetComponent<HERO>())
        {
            target.GetComponent<HERO>().HookedByHuman(base.photonView.viewID, hookPosition);
        }
        launchForce = hookPosition - base.transform.position;
        float d = Mathf.Pow(launchForce.magnitude, 0.1f);
        if (grounded)
        {
            base.rigidbody.AddForce(Vector3.up * Mathf.Min(launchForce.magnitude * 0.2f, 10f), ForceMode.Impulse);
        }
        base.rigidbody.AddForce(launchForce * d * 0.1f, ForceMode.Impulse);
    }

    public void HookedByHuman(int hooker, Vector3 hookPosition)
    {
        base.photonView.RPC("RPCHookedByHuman", base.photonView.owner, hooker, hookPosition);
    }

    [RPC]
    private void RPCHookedByHuman(int hooker, Vector3 hookPosition)
    {
        hookBySomeOne = true;
        PhotonView view = PhotonView.Find(hooker);
        badGuy = view.gameObject;
        if (Vector3.Distance(hookPosition, base.transform.position) < 15f)
        {
            launchForce = badGuy.gameObject.transform.position - base.transform.position;
            base.rigidbody.AddForce(-base.rigidbody.velocity * 0.9f, ForceMode.VelocityChange);
            float d = Mathf.Pow(launchForce.magnitude, 0.1f);
            if (grounded)
            {
                base.rigidbody.AddForce(Vector3.up * Mathf.Min(launchForce.magnitude * 0.2f, 10f), ForceMode.Impulse);
            }
            base.rigidbody.AddForce(launchForce * d * 0.1f, ForceMode.Impulse);
            if (state != HeroState.Grabbed)
            {
                dashTime = 1f;
                CrossFade("dash", 0.05f);
                base.animation["dash"].time = 0.1f;
                state = HeroState.Dashing;
                falseAttack();
                facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * 57.29578f;
                Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
                base.gameObject.transform.rotation = quaternion;
                base.rigidbody.rotation = quaternion;
                targetRotation = quaternion;
            }
        }
        else
        {
            hookBySomeOne = false;
            badGuy = null;
            view.RPC("hookFail", view.owner);
        }
    }

    [RPC]
    public void hookFail()
    {
        hookTarget = null;
        hookSomeOne = false;
    }

    [RPC]
    public void badGuyReleaseMe()
    {
        hookBySomeOne = false;
        badGuy = null;
    }

    private void ReleaseHookedTarget()
    {
        if (hookSomeOne && hookTarget != null)
        {
            hookTarget.GetPhotonView().RPC("badGuyReleaseMe", hookTarget.GetPhotonView().owner);
            hookTarget = null;
            hookSomeOne = false;
        }
    }

    public void Launch(Vector3 des, bool left = true, bool leviMode = false)
    {
        // TODO: Mod, this should fix noclipping when grabbed by a titan right after hooking an object
        if (state == HeroState.Grabbed)
        {
            return;
        }

        if (isMounted)
        {
            Unmount();
        }
        if (state != HeroState.Attack)
        {
            Idle();
        }
        Vector3 vector = des - base.transform.position;
        if (left)
        {
            launchPointLeft = des;
        }
        else
        {
            launchPointRight = des;
        }
        vector.Normalize();
        vector *= 20f;
        if (bulletLeft != null && bulletRight != null && bulletLeft.GetComponent<Bullet>().IsHooked() && bulletRight.GetComponent<Bullet>().IsHooked())
        {
            vector *= 0.8f;
        }
        leviMode = ((base.animation.IsPlaying("attack5") || base.animation.IsPlaying("special_petra")) ? true : false);
        if (!leviMode)
        {
            falseAttack();
            Idle();
            if (useGun)
            {
                CrossFade("AHSS_hook_forward_both", 0.1f);
            }
            else if (left && !isRightHandHooked)
            {
                CrossFade("air_hook_l_just", 0.1f);
            }
            else if (!left && !isLeftHandHooked)
            {
                CrossFade("air_hook_r_just", 0.1f);
            }
            else
            {
                CrossFade("dash", 0.1f);
                base.animation["dash"].time = 0f;
            }
        }
        if (left)
        {
            isLaunchLeft = true;
        }
        if (!left)
        {
            isLaunchRight = true;
        }
        launchForce = vector;
        if (!leviMode)
        {
            if (vector.y < 30f)
            {
                launchForce += Vector3.up * (30f - vector.y);
            }
            float y = des.y;
            Vector3 position = base.transform.position;
            if (y >= position.y)
            {
                Vector3 a = launchForce;
                Vector3 up = Vector3.up;
                float y2 = des.y;
                Vector3 position2 = base.transform.position;
                launchForce = a + up * (y2 - position2.y) * 10f;
            }
            base.rigidbody.AddForce(launchForce);
        }
        facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * 57.29578f;
        Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
        base.gameObject.transform.rotation = quaternion;
        base.rigidbody.rotation = quaternion;
        targetRotation = quaternion;
        if (left)
        {
            launchElapsedTimeL = 0f;
        }
        else
        {
            launchElapsedTimeR = 0f;
        }
        if (leviMode)
        {
            launchElapsedTimeR = -100f;
        }
        if (base.animation.IsPlaying("special_petra"))
        {
            launchElapsedTimeR = -100f;
            launchElapsedTimeL = -100f;
            if ((bool)bulletRight)
            {
                bulletRight.GetComponent<Bullet>().Disable();
                ReleaseHookedTarget();
            }
            if ((bool)bulletLeft)
            {
                bulletLeft.GetComponent<Bullet>().Disable();
                ReleaseHookedTarget();
            }
        }

        sparks.enableEmission = false;
        cancelGasDisable = true;
    }

    private void AimLeftArmTo(Vector3 target)
    {
        float x = target.x;
        Vector3 position = upperarmL.transform.position;
        float num = x - position.x;
        float y = target.y;
        Vector3 position2 = upperarmL.transform.position;
        float y2 = y - position2.y;
        float z = target.z;
        Vector3 position3 = upperarmL.transform.position;
        float num2 = z - position3.z;
        float x2 = Mathf.Sqrt(num * num + num2 * num2);
        handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
        forearmL.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        upperarmL.rotation = Quaternion.Euler(0f, 90f + Mathf.Atan2(num, num2) * 57.29578f, (0f - Mathf.Atan2(y2, x2)) * 57.29578f);
    }

    private void ArmRightArmTo(Vector3 target)
    {
        float x = target.x;
        Vector3 position = upperarmR.transform.position;
        float num = x - position.x;
        float y = target.y;
        Vector3 position2 = upperarmR.transform.position;
        float y2 = y - position2.y;
        float z = target.z;
        Vector3 position3 = upperarmR.transform.position;
        float num2 = z - position3.z;
        float x2 = Mathf.Sqrt(num * num + num2 * num2);
        handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        forearmR.localRotation = Quaternion.Euler(90f, 0f, 0f);
        upperarmR.rotation = Quaternion.Euler(180f, 90f + Mathf.Atan2(num, num2) * 57.29578f, Mathf.Atan2(y2, x2) * 57.29578f);
    }

    private void MoveHead()
    {
        Transform transform = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
        Transform transform2 = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck");
        float x = gunTarget.x;
        Vector3 position = base.transform.position;
        float num = x - position.x;
        float x2 = gunTarget.x;
        float num2 = num * (x2 - position.x);
        float z = gunTarget.z;
        float num3 = z - position.z;
        float z2 = gunTarget.z;
        float x3 = Mathf.Sqrt(num2 + num3 * (z2 - position.z));
        targetHeadRotation = transform.rotation;
        Vector3 vector = gunTarget - base.transform.position;
        float current = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
        Vector3 eulerAngles = base.transform.rotation.eulerAngles;
        float value = 0f - Mathf.DeltaAngle(current, eulerAngles.y - 90f);
        value = Mathf.Clamp(value, -40f, 40f);
        Vector3 position5 = transform2.position;
        float y = position5.y - gunTarget.y;
        float value2 = Mathf.Atan2(y, x3) * 57.29578f;
        value2 = Mathf.Clamp(value2, -40f, 30f);
        Vector3 eulerAngles2 = transform.rotation.eulerAngles;
        float x4 = eulerAngles2.x + value2;
        float y2 = eulerAngles2.y + value;
        targetHeadRotation = Quaternion.Euler(x4, y2, eulerAngles2.z);
        oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 60f);
        transform.rotation = oldHeadRotation;
    }

    public void ShootFlare(int type)
    {
        bool canShoot = false;
        if (type == 1 && flare1CD == 0f)
        {
            flare1CD = flareTotalCD;
            canShoot = true;
        }
        if (type == 2 && flare2CD == 0f)
        {
            flare2CD = flareTotalCD;
            canShoot = true;
        }
        if (type == 3 && flare3CD == 0f)
        {
            flare3CD = flareTotalCD;
            canShoot = true;
        }

        if (canShoot)
        {
            Quaternion firingDirection = base.transform.rotation;

            if (Guardian.Mod.Properties.DirectionalFlares.Value)
            {
                // Yes I took this from Anarchy-Expedition, hush.
                Quaternion cameraRot = Camera.main.transform.rotation;
                firingDirection = Quaternion.Euler(cameraRot.eulerAngles.x + 60f, cameraRot.eulerAngles.y, cameraRot.eulerAngles.z);
            }

            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/flareBullet" + type), base.transform.position, firingDirection);
                gameObject.GetComponent<FlareMovement>().DontShowHint();
                UnityEngine.Object.Destroy(gameObject, 25f);
            }
            else
            {
                GameObject gameObject2 = PhotonNetwork.Instantiate("FX/flareBullet" + type, base.transform.position, firingDirection, 0);
                gameObject2.GetComponent<FlareMovement>().DontShowHint();
            }
        }
    }

    public bool HasDied()
    {
        return hasDied || IsInvincible();
    }

    public void MarkDead()
    {
        hasDied = true;
        state = HeroState.Die;
    }

    public void Die(Vector3 v, bool isBite)
    {
        if (!(invincible > 0f))
        {
            if (titanForm && eren_titan != null)
            {
                eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if ((bool)bulletLeft)
            {
                bulletLeft.GetComponent<Bullet>().RemoveMe();
            }
            if ((bool)bulletRight)
            {
                bulletRight.GetComponent<Bullet>().RemoveMe();
            }
            meatDie.Play();
            if ((IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine) && !useGun)
            {
                leftbladetrail.Deactivate();
                rightbladetrail.Deactivate();
                leftbladetrail2.Deactivate();
                rightbladetrail2.Deactivate();
            }
            BreakApart2(v, isBite);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.Instance.LoseGame();
            falseAttack();
            hasDied = true;
            Transform transform = base.transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
            if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
            {
                maincamera.GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(base.transform.position, 0, null, 0.02f);
            }
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    private void ApplyForce(GameObject gameObject, Vector3 v)
    {
        gameObject.rigidbody.AddForce(v);
        gameObject.rigidbody.AddTorque(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
    }

    public void Die2(Transform tf)
    {
        if (!(invincible > 0f))
        {
            if (titanForm && eren_titan != null)
            {
                eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if ((bool)bulletLeft)
            {
                bulletLeft.GetComponent<Bullet>().RemoveMe();
            }
            if ((bool)bulletRight)
            {
                bulletRight.GetComponent<Bullet>().RemoveMe();
            }
            Transform transform = base.transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
            meatDie.Play();
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.Instance.LoseGame();
            falseAttack();
            hasDied = true;
            GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"));
            gameObject.transform.position = base.transform.position;
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    [RPC]
    private void netTauntAttack(float tauntTime, float distance = 100f)
    {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("titan"))
        {
            if (Vector3.Distance(gameObject.transform.position, base.transform.position) < distance && (bool)gameObject.GetComponent<TITAN>())
            {
                gameObject.GetComponent<TITAN>().beTauntedBy(base.gameObject, tauntTime);
            }
        }
    }

    [RPC]
    private void netlaughAttack()
    {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("titan"))
        {
            if (Vector3.Distance(gameObject.transform.position, base.transform.position) < 50f && Vector3.Angle(gameObject.transform.forward, base.transform.position - gameObject.transform.position) < 90f && (bool)gameObject.GetComponent<TITAN>())
            {
                gameObject.GetComponent<TITAN>().beLaughAttacked();
            }
        }
    }

    [RPC]
    private void net3DMGSMOKE(bool ifON)
    {
        if (smoke_3dmg != null)
        {
            smoke_3dmg.enableEmission = ifON;
        }
    }

    [RPC]
    private void showHitDamage(PhotonMessageInfo info)
    {
        if (!Guardian.AntiAbuse.Validators.Hero.IsHitDamageShowValid(info))
        {
            return;
        }
        GameObject gameObject = GameObject.Find("LabelScore");
        if ((bool)gameObject)
        {
            speed = Mathf.Max(10f, speed);
            gameObject.GetComponent<UILabel>().text = speed.ToString();
            gameObject.transform.localScale = Vector3.zero;
            speed = (int)(speed * 0.1f);
            speed = Mathf.Clamp(speed, 40f, 150f);
            iTween.Stop(gameObject);
            iTween.ScaleTo(gameObject, iTween.Hash("x", speed, "y", speed, "z", speed, "easetype", iTween.EaseType.easeOutElastic, "time", 1f));
            iTween.ScaleTo(gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "easetype", iTween.EaseType.easeInBounce, "time", 0.5f, "delay", 2f));
        }
    }

    [RPC]
    public void blowAway(Vector3 force, PhotonMessageInfo info = null)
    {
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine)
        {
            if (Guardian.AntiAbuse.Validators.Hero.IsBlowAwayValid(info))
            {
                base.rigidbody.AddForce(force, ForceMode.Impulse);
                base.transform.LookAt(base.transform.position);
            }
        }
    }

    [RPC]
    private void killObject(PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.Validators.Hero.IsKillObjectValid(info))
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    private float GetGlobalFacingDirection(float horizontal, float vertical)
    {
        if (vertical == 0f && horizontal == 0f)
        {
            Vector3 eulerAngles = base.transform.rotation.eulerAngles;
            return eulerAngles.y;
        }
        Vector3 eulerAngles2 = currentCamera.transform.rotation.eulerAngles;
        float y = eulerAngles2.y;
        float num = Mathf.Atan2(vertical, horizontal) * 57.29578f;
        num = 0f - num + 90f;
        return y + num;
    }

    private Vector3 GetGlobalFacingVector(float resultAngle)
    {
        float num = 0f - resultAngle + 90f;
        float x = Mathf.Cos(num * ((float)Math.PI / 180f));
        float z = Mathf.Sin(num * ((float)Math.PI / 180f));
        return new Vector3(x, 0f, z);
    }

    private void GetOnHorse()
    {
        PlayAnimation("horse_geton");
        Vector3 eulerAngles = myHorse.transform.rotation.eulerAngles;
        facingDirection = eulerAngles.y;
        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
    }

    private void GetOffHorse()
    {
        PlayAnimation("horse_getoff");
        base.rigidbody.AddForce(Vector3.up * 10f - base.transform.forward * 2f - base.transform.right * 1f, ForceMode.VelocityChange);
        Unmount();
    }

    private void Unmount()
    {
        myHorse.GetComponent<Horse>().Unmount();
        isMounted = false;
    }

    private void Awake()
    {
        Cache();
        setup = base.gameObject.GetComponent<HERO_SETUP>();
        baseRigidBody.freezeRotation = true;
        baseRigidBody.useGravity = false;
        handL = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
        handR = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
        forearmL = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L");
        forearmR = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
        upperarmL = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
        upperarmR = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");

        // Anarchy
        Anarchy.Custom.Level.CustomAnarchyLevel.Instance.AddScriptHero(this);
    }

    private void Start()
    {
        // TODO: Mod, load custom textures and audio clips
        {
            AudioClip hookShotClip = Guardian.Utilities.Gesources.Find<AudioClip>("Custom/Audio/hook_shot.wav");
            if (hookShotClip != null)
            {
                rope.clip = hookShotClip;
            }

            AudioClip swordSwingClip = Guardian.Utilities.Gesources.Find<AudioClip>("Custom/Audio/sword_swing.wav");
            if (swordSwingClip != null)
            {
                slash.clip = swordSwingClip;
            }

            AudioClip swordHitClip = Guardian.Utilities.Gesources.Find<AudioClip>("Custom/Audio/sword_hit.wav");
            if (swordHitClip != null)
            {
                slashHit.clip = swordHitClip;
            }

            AudioClip deathClip = Guardian.Utilities.Gesources.Find<AudioClip>("Custom/Audio/player_die.wav");
            if (deathClip != null)
            {
                meatDie.clip = deathClip;
            }
        }

        FengGameManagerMKII.Instance.AddHero(this);
        if ((FengGameManagerMKII.Level.Horses || RCSettings.HorseMode == 1) && IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
        {
            myHorse = PhotonNetwork.Instantiate("horse", baseTransform.position + Vector3.up * 5f, baseTransform.rotation, 0);
            myHorse.GetComponent<Horse>().myHero = base.gameObject;
            myHorse.GetComponent<TITAN_CONTROLLER>().isHorse = true;
        }

        sparks = baseTransform.Find("slideSparks").GetComponent<ParticleSystem>();
        smoke_3dmg = baseTransform.Find("3dmg_smoke").GetComponent<ParticleSystem>();
        baseTransform.localScale = new Vector3(myScale, myScale, myScale);
        facingDirection = baseTransform.rotation.eulerAngles.y;
        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        smoke_3dmg.enableEmission = false;
        sparks.enableEmission = false;
        speedFXPS = speedFX1.GetComponent<ParticleSystem>();
        speedFXPS.enableEmission = false;

        if ((int)FengGameManagerMKII.Settings[93] == 1)
        {
            speedFXPS.gameObject.SetActive(false);
        }

        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
        {
            if (PhotonNetwork.isMasterClient)
            {
                int playerId = base.photonView.owner.Id;
                if (FengGameManagerMKII.HeroHash.ContainsKey(playerId))
                {
                    FengGameManagerMKII.HeroHash[playerId] = this;
                }
                else
                {
                    FengGameManagerMKII.HeroHash.Add(playerId, this);
                }
            }
            GameObject gameObject = GameObject.Find("UI_IN_GAME");
            myNetWorkName = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
            myNetWorkName.name = "LabelNameOverHead";
            myNetWorkName.transform.parent = gameObject.GetComponent<UIReferArray>().panels[0].transform;
            myNetWorkName.transform.localScale = new Vector3(14f, 14f, 14f);
            myNetWorkName.GetComponent<UILabel>().text = string.Empty;
            myNetWorkName.GetComponent<UILabel>().alpha = (float)Guardian.Mod.Properties.OpacityOfOtherNames.Value;
            if (base.photonView.isMine)
            {
                myNetWorkName.GetComponent<UILabel>().alpha = (float)Guardian.Mod.Properties.OpacityOfOwnName.Value;

                if (Minimap.Instance != null)
                {
                    Minimap.Instance.TrackGameObjectOnMinimap(base.gameObject, Color.green, trackOrientation: false, depthAboveAll: true);
                }
                GetComponent<SmoothSyncMovement>().PhotonCamera = true;
                base.photonView.RPC("SetMyPhotonCamera", PhotonTargets.OthersBuffered, PlayerPrefs.GetFloat("cameraDistance") + 0.3f);
            }
            else
            {
                bool flag = false;
                if (base.photonView.owner.customProperties[PhotonPlayerProperty.RCTeam] != null)
                {
                    switch (GExtensions.AsInt(base.photonView.owner.customProperties[PhotonPlayerProperty.RCTeam]))
                    {
                        case 1:
                            flag = true;
                            if (Minimap.Instance != null)
                            {
                                Minimap.Instance.TrackGameObjectOnMinimap(base.gameObject, Color.cyan, trackOrientation: false, depthAboveAll: true);
                            }
                            break;
                        case 2:
                            flag = true;
                            if (Minimap.Instance != null)
                            {
                                Minimap.Instance.TrackGameObjectOnMinimap(base.gameObject, Color.magenta, trackOrientation: false, depthAboveAll: true);
                            }
                            break;
                    }
                }
                if (GExtensions.AsInt(base.photonView.owner.customProperties[PhotonPlayerProperty.Team]) == 2)
                {
                    if (!flag && Minimap.Instance != null)
                    {
                        Minimap.Instance.TrackGameObjectOnMinimap(base.gameObject, Color.red, trackOrientation: false, depthAboveAll: true);
                    }
                }
                else if (!flag && Minimap.Instance != null)
                {
                    Minimap.Instance.TrackGameObjectOnMinimap(base.gameObject, Color.blue, trackOrientation: false, depthAboveAll: true);
                }
            }

            string nametagContent = string.Empty;
            UILabel component = myNetWorkName.GetComponent<UILabel>();
            if (base.photonView.owner.customProperties[PhotonPlayerProperty.RCTeam] != null)
            {
                switch (GExtensions.AsInt(base.photonView.owner.customProperties[PhotonPlayerProperty.RCTeam]))
                {
                    case 1:
                        nametagContent += "[00FFFF][CYAN][FFFFFF]\n";
                        break;
                    case 2:
                        nametagContent += "[FF00FF][MAGENTA][FFFFFF]\n";
                        break;
                }
            }
            if (GExtensions.AsInt(base.photonView.owner.customProperties[PhotonPlayerProperty.Team]) == 2)
            {
                nametagContent += "[" + ColorSet.AHSS + "]AHSS\n[FFFFFF]";
            }
            string playerGuild = GExtensions.AsString(base.photonView.owner.customProperties[PhotonPlayerProperty.Guild]);
            if (playerGuild.Length > 0)
            {
                nametagContent += "[FFFF00]" + playerGuild + "\n[FFFFFF]";
            }
            nametagContent += GExtensions.AsString(base.photonView.owner.customProperties[PhotonPlayerProperty.Name]);
            component.text = nametagContent;
        }
        else if (Minimap.Instance != null)
        {
            Minimap.Instance.TrackGameObjectOnMinimap(base.gameObject, Color.green, trackOrientation: false, depthAboveAll: true);
        }

        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && !base.photonView.isMine)
        {
            base.gameObject.layer = LayerMask.NameToLayer("NetworkObject");

            setup.Init();
            setup.myCostume = new HeroCostume();
            setup.myCostume = CostumeConverter.FromPhotonData(base.photonView.owner);
            setup.CreateCharacterComponent();
            UnityEngine.Object.Destroy(checkBoxLeft);
            UnityEngine.Object.Destroy(checkBoxRight);
            UnityEngine.Object.Destroy(leftbladetrail);
            UnityEngine.Object.Destroy(rightbladetrail);
            UnityEngine.Object.Destroy(leftbladetrail2);
            UnityEngine.Object.Destroy(rightbladetrail2);
            hasspawn = true;
        }
        else
        {
            currentCamera = maincamera.GetComponent<Camera>();
            inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
            LoadSkin();
            hasspawn = true;
        }

        // TODO: Mod, flashlight Fix?
        if (IN_GAME_MAIN_CAMERA.Lighting == DayLight.Night)
        {
            myFlashlight = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("flashlight"));

            if (base.photonView.isMine || IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                myFlashlight.GetComponent<Light>().renderMode = LightRenderMode.ForcePixel;
                myFlashlight.transform.parent = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().transform;
                myFlashlight.transform.position = IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer ?
                    currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().transform.position
                    : (currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().transform.position + (Vector3.down * 5));
            }
            else
            {
                myFlashlight.transform.parent = baseTransform;
                myFlashlight.transform.position = baseTransform.position + Vector3.up;
            }
            myFlashlight.transform.rotation = Quaternion.Euler(353f, 0f, 0f);
        }

        bombImmune = false;
        if (RCSettings.BombMode == 1)
        {
            bombImmune = true;
            StartCoroutine(CoStopImmunity());
        }

        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine)
        {
            base.rigidbody.interpolation = Guardian.Mod.Properties.Interpolation.Value ?
                RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
        }
    }

    private void FixedUpdate()
    {
        if (titanForm || isCannon || (IN_GAME_MAIN_CAMERA.IsPausing && IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer))
        {
            return;
        }
        currentSpeed = baseRigidBody.velocity.magnitude;

        if (base.rigidbody.interpolation.Equals(RigidbodyInterpolation.Interpolate))
        {
            SetHookedPplDirection();
            LeanBody();
        }

        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && !base.photonView.isMine)
        {
            return;
        }

        if (!baseAnimation.IsPlaying("attack3_2") && !baseAnimation.IsPlaying("attack5") && !baseAnimation.IsPlaying("special_petra"))
        {
            baseRigidBody.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, targetRotation, Time.deltaTime * 6f);
        }
        if (state == HeroState.Grabbed)
        {
            baseRigidBody.AddForce(-baseRigidBody.velocity, ForceMode.VelocityChange);
            return;
        }
        if (IsGrounded())
        {
            if (!grounded)
            {
                justGrounded = true;
            }
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        if (hookSomeOne)
        {
            if (hookTarget != null)
            {
                Vector3 vector = hookTarget.transform.position - baseTransform.position;
                float magnitude = vector.magnitude;
                if (magnitude > 2f)
                {
                    baseRigidBody.AddForce(vector.normalized * Mathf.Pow(magnitude, 0.15f) * 30f - baseRigidBody.velocity * 0.95f, ForceMode.VelocityChange);
                }
            }
            else
            {
                hookSomeOne = false;
            }
        }
        else if (hookBySomeOne && badGuy != null)
        {
            Vector3 vector2 = badGuy.transform.position - baseTransform.position;
            float magnitude2 = vector2.magnitude;
            if (magnitude2 > 5f)
            {
                baseRigidBody.AddForce(vector2.normalized * Mathf.Pow(magnitude2, 0.15f) * 0.2f, ForceMode.Impulse);
            }
        }
        float num = 0f;
        float num2 = 0f;
        if (!IN_GAME_MAIN_CAMERA.IsTyping)
        {
            num2 = (inputManager.isInput[InputCode.Up] ? 1f : ((!inputManager.isInput[InputCode.Down]) ? 0f : (-1f)));
            num = (inputManager.isInput[InputCode.Left] ? (-1f) : ((!inputManager.isInput[InputCode.Right]) ? 0f : 1f));
        }
        bool flag = false;
        bool flag2 = false;
        bool flag3 = false;
        isLeftHandHooked = false;
        isRightHandHooked = false;
        if (isLaunchLeft)
        {
            if (bulletLeft != null && bulletLeft.GetComponent<Bullet>().IsHooked())
            {
                isLeftHandHooked = true;
                Vector3 vector3 = bulletLeft.transform.position - baseTransform.position;
                vector3.Normalize();
                vector3 *= 10f;
                if (!isLaunchRight)
                {
                    vector3 *= 2f;
                }
                if (Vector3.Angle(baseRigidBody.velocity, vector3) > 90f && inputManager.isInput[InputCode.Jump])
                {
                    flag2 = true;
                    flag = true;
                }
                if (!flag2)
                {
                    baseRigidBody.AddForce(vector3);
                    if (Vector3.Angle(baseRigidBody.velocity, vector3) > 90f)
                    {
                        baseRigidBody.AddForce(-baseRigidBody.velocity * 2f, ForceMode.Acceleration);
                    }
                }
            }
            launchElapsedTimeL += Time.deltaTime;
            if (QHold && currentGas > 0f)
            {
                UseGas(useGasSpeed * Time.deltaTime);
            }
            else if (launchElapsedTimeL > 0.3f)
            {
                isLaunchLeft = false;
                if (bulletLeft != null)
                {
                    bulletLeft.GetComponent<Bullet>().Disable();
                    ReleaseHookedTarget();
                    bulletLeft = null;
                    flag2 = false;
                }
            }
        }
        if (isLaunchRight)
        {
            if (bulletRight != null && bulletRight.GetComponent<Bullet>().IsHooked())
            {
                isRightHandHooked = true;
                Vector3 vector4 = bulletRight.transform.position - baseTransform.position;
                vector4.Normalize();
                vector4 *= 10f;
                if (!isLaunchLeft)
                {
                    vector4 *= 2f;
                }
                if (Vector3.Angle(baseRigidBody.velocity, vector4) > 90f && inputManager.isInput[InputCode.Jump])
                {
                    flag3 = true;
                    flag = true;
                }
                if (!flag3)
                {
                    baseRigidBody.AddForce(vector4);
                    if (Vector3.Angle(baseRigidBody.velocity, vector4) > 90f)
                    {
                        baseRigidBody.AddForce(-baseRigidBody.velocity * 2f, ForceMode.Acceleration);
                    }
                }
            }
            launchElapsedTimeR += Time.deltaTime;
            if (EHold && currentGas > 0f)
            {
                UseGas(useGasSpeed * Time.deltaTime);
            }
            else if (launchElapsedTimeR > 0.3f)
            {
                isLaunchRight = false;
                if (bulletRight != null)
                {
                    bulletRight.GetComponent<Bullet>().Disable();
                    ReleaseHookedTarget();
                    bulletRight = null;
                    flag3 = false;
                }
            }
        }
        if (grounded)
        {
            Vector3 a = Vector3.zero;
            if (state == HeroState.Attack)
            {
                switch (attackAnimation)
                {
                    case "attack5":
                        if (baseAnimation[attackAnimation].normalizedTime > 0.4f && baseAnimation[attackAnimation].normalizedTime < 0.61f)
                        {
                            baseRigidBody.AddForce(base.gameObject.transform.forward * 200f);
                        }
                        break;
                    case "special_petra":
                        if (baseAnimation[attackAnimation].normalizedTime > 0.35f && baseAnimation[attackAnimation].normalizedTime < 0.48f)
                        {
                            baseRigidBody.AddForce(base.gameObject.transform.forward * 200f);
                        }
                        break;
                    default:
                        if (baseAnimation.IsPlaying("attack1") || baseAnimation.IsPlaying("attack2"))
                        {
                            baseRigidBody.AddForce(base.gameObject.transform.forward * 200f);
                        }
                        break;
                }
            }
            if (justGrounded)
            {
                if (state != HeroState.Attack || (attackAnimation != "attack3_1" && attackAnimation != "attack5" && attackAnimation != "special_petra"))
                {
                    if (state != HeroState.Attack && num == 0f && num2 == 0f && bulletLeft == null && bulletRight == null && state != HeroState.FillGas)
                    {
                        if (state != HeroState.Land)
                        {
                            CrossFade("dash_land", 0.01f);
                        }
                        state = HeroState.Land;
                    }
                    else
                    {
                        buttonAttackRelease = true;
                        if (state != HeroState.Attack && baseRigidBody.velocity.x * baseRigidBody.velocity.x + baseRigidBody.velocity.z * baseRigidBody.velocity.z > speed * speed * 1.5f && state != HeroState.FillGas)
                        {
                            state = HeroState.Slide;
                            CrossFade("slide", 0.05f);
                            facingDirection = Mathf.Atan2(baseRigidBody.velocity.x, baseRigidBody.velocity.z) * 57.29578f;
                            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                            sparks.enableEmission = true;
                        }
                    }
                }
                justGrounded = false;
                a = baseRigidBody.velocity;
            }
            switch (state)
            {
                case HeroState.Attack:
                    if (attackAnimation == "attack3_1" && baseAnimation[attackAnimation].normalizedTime >= 1f)
                    {
                        PlayAnimation("attack3_2");
                        ResetAnimationSpeed();
                        Vector3 zero = Vector3.zero;
                        baseRigidBody.velocity = zero;
                        a = zero;
                        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartShake(0.2f, 0.3f);
                    }
                    break;
                case HeroState.Dodging:
                    if (baseAnimation["dodge"].normalizedTime >= 0.2f && baseAnimation["dodge"].normalizedTime < 0.8f)
                    {
                        a = -baseTransform.forward * 2.4f * speed;
                    }
                    if (baseAnimation["dodge"].normalizedTime > 0.8f)
                    {
                        a = baseRigidBody.velocity * 0.9f;
                    }
                    break;
                case HeroState.Idle:
                    Vector3 vector5 = new Vector3(num, 0f, num2);
                    float num3 = GetGlobalFacingDirection(num, num2);
                    a = GetGlobalFacingVector(num3);
                    float num4 = (!(vector5.magnitude <= 0.95f)) ? 1f : ((vector5.magnitude >= 0.25f) ? vector5.magnitude : 0f);
                    a *= num4;
                    a *= speed;
                    if (buffTime > 0f && currentBuff == BUFF.Speed)
                    {
                        a *= 4f;
                    }
                    if (num != 0f || num2 != 0f)
                    {
                        if (!baseAnimation.IsPlaying("run") && !baseAnimation.IsPlaying("jump") && !baseAnimation.IsPlaying("run_sasha") && (!baseAnimation.IsPlaying("horse_geton") || baseAnimation["horse_geton"].normalizedTime >= 0.5f))
                        {
                            if (buffTime > 0f && currentBuff == BUFF.Speed)
                            {
                                CrossFade("run_sasha", 0.1f);
                            }
                            else
                            {
                                CrossFade("run", 0.1f);
                            }
                        }
                    }
                    else
                    {
                        if (!baseAnimation.IsPlaying(standAnimation) && state != HeroState.Land && !baseAnimation.IsPlaying("jump") && !baseAnimation.IsPlaying("horse_geton") && !baseAnimation.IsPlaying("grabbed"))
                        {
                            CrossFade(standAnimation, 0.1f);
                            a *= 0f;
                        }
                        num3 = -874f;
                    }
                    if (num3 != -874f)
                    {
                        facingDirection = num3;
                        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                    }
                    break;
                case HeroState.Land:
                    a = baseRigidBody.velocity * 0.96f;
                    break;
                case HeroState.Slide:
                    a = baseRigidBody.velocity * 0.99f;
                    if (currentSpeed < speed * 1.2f)
                    {
                        Idle();
                        sparks.enableEmission = false;
                    }
                    break;
            }
            Vector3 velocity = baseRigidBody.velocity;
            Vector3 force = a - velocity;
            force.x = Mathf.Clamp(force.x, 0f - maxVelocityChange, maxVelocityChange);
            force.z = Mathf.Clamp(force.z, 0f - maxVelocityChange, maxVelocityChange);
            force.y = 0f;
            if (baseAnimation.IsPlaying("jump") && baseAnimation["jump"].normalizedTime > 0.18f)
            {
                force.y += 8f;
            }
            if (baseAnimation.IsPlaying("horse_geton") && baseAnimation["horse_geton"].normalizedTime > 0.18f && baseAnimation["horse_geton"].normalizedTime < 1f)
            {
                float num5 = 6f;
                force = -baseRigidBody.velocity;
                force.y = num5;
                float num6 = Vector3.Distance(myHorse.transform.position, baseTransform.position);
                float d = 0.6f * gravity * num6 / (2f * num5);
                force += d * (myHorse.transform.position - baseTransform.position).normalized;
            }
            if (state != HeroState.Attack || !useGun)
            {
                baseRigidBody.AddForce(force, ForceMode.VelocityChange);
                baseRigidBody.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
            }
        }
        else
        {
            if (sparks.enableEmission)
            {
                sparks.enableEmission = false;
            }
            if (myHorse != null && (baseAnimation.IsPlaying("horse_geton") || baseAnimation.IsPlaying("air_fall")) && baseRigidBody.velocity.y < 0f && Vector3.Distance(myHorse.transform.position + Vector3.up * 1.65f, baseTransform.position) < 0.5f)
            {
                baseTransform.position = myHorse.transform.position + Vector3.up * 1.65f;
                baseTransform.rotation = myHorse.transform.rotation;
                isMounted = true;

                if (!baseAnimation.IsPlaying("horse_idle"))
                {
                    CrossFade("horse_idle", 0.1f);
                }

                myHorse.GetComponent<Horse>().Mount();
            }
            if ((state == HeroState.Idle && !baseAnimation.IsPlaying("dash") && !baseAnimation.IsPlaying("wallrun") && !baseAnimation.IsPlaying("toRoof") && !baseAnimation.IsPlaying("horse_geton") && !baseAnimation.IsPlaying("horse_getoff") && !baseAnimation.IsPlaying("air_release") && !isMounted && (!baseAnimation.IsPlaying("air_hook_l_just") || baseAnimation["air_hook_l_just"].normalizedTime >= 1f) && (!baseAnimation.IsPlaying("air_hook_r_just") || baseAnimation["air_hook_r_just"].normalizedTime >= 1f)) || baseAnimation["dash"].normalizedTime >= 0.99f)
            {
                if (!isLeftHandHooked && !isRightHandHooked && (baseAnimation.IsPlaying("air_hook_l") || baseAnimation.IsPlaying("air_hook_r") || baseAnimation.IsPlaying("air_hook")) && baseRigidBody.velocity.y > 20f)
                {
                    baseAnimation.CrossFade("air_release");
                }
                else
                {
                    bool flag4 = Mathf.Abs(baseRigidBody.velocity.x) + Mathf.Abs(baseRigidBody.velocity.z) > 25f;
                    bool flag5 = baseRigidBody.velocity.y < 0f;
                    if (!flag4)
                    {
                        if (flag5)
                        {
                            if (!baseAnimation.IsPlaying("air_fall"))
                            {
                                CrossFade("air_fall", 0.2f);
                            }
                        }
                        else if (!baseAnimation.IsPlaying("air_rise"))
                        {
                            CrossFade("air_rise", 0.2f);
                        }
                    }
                    else if (!isLeftHandHooked && !isRightHandHooked)
                    {
                        float current = (0f - Mathf.Atan2(baseRigidBody.velocity.z, baseRigidBody.velocity.x)) * 57.29578f;
                        float num7 = 0f - Mathf.DeltaAngle(current, baseTransform.rotation.eulerAngles.y - 90f);
                        if (Mathf.Abs(num7) < 45f)
                        {
                            if (!baseAnimation.IsPlaying("air2"))
                            {
                                CrossFade("air2", 0.2f);
                            }
                        }
                        else if (num7 < 135f && num7 > 0f)
                        {
                            if (!baseAnimation.IsPlaying("air2_right"))
                            {
                                CrossFade("air2_right", 0.2f);
                            }
                        }
                        else if (num7 > -135f && num7 < 0f)
                        {
                            if (!baseAnimation.IsPlaying("air2_left"))
                            {
                                CrossFade("air2_left", 0.2f);
                            }
                        }
                        else if (!baseAnimation.IsPlaying("air2_backward"))
                        {
                            CrossFade("air2_backward", 0.2f);
                        }
                    }
                    else if (useGun)
                    {
                        if (!isRightHandHooked)
                        {
                            if (!baseAnimation.IsPlaying("AHSS_hook_forward_l"))
                            {
                                CrossFade("AHSS_hook_forward_l", 0.1f);
                            }
                        }
                        else if (!isLeftHandHooked)
                        {
                            if (!baseAnimation.IsPlaying("AHSS_hook_forward_r"))
                            {
                                CrossFade("AHSS_hook_forward_r", 0.1f);
                            }
                        }
                        else if (!baseAnimation.IsPlaying("AHSS_hook_forward_both"))
                        {
                            CrossFade("AHSS_hook_forward_both", 0.1f);
                        }
                    }
                    else if (!isRightHandHooked)
                    {
                        if (!baseAnimation.IsPlaying("air_hook_l"))
                        {
                            CrossFade("air_hook_l", 0.1f);
                        }
                    }
                    else if (!isLeftHandHooked)
                    {
                        if (!baseAnimation.IsPlaying("air_hook_r"))
                        {
                            CrossFade("air_hook_r", 0.1f);
                        }
                    }
                    else if (!baseAnimation.IsPlaying("air_hook"))
                    {
                        CrossFade("air_hook", 0.1f);
                    }
                }
            }
            if ((state == HeroState.Idle && baseAnimation.IsPlaying("air_release") && baseAnimation["air_release"].normalizedTime >= 1f)
                || (baseAnimation.IsPlaying("horse_getoff") && baseAnimation["horse_getoff"].normalizedTime >= 1f))
            {
                if (!baseAnimation.IsPlaying("air_rise"))
                {
                    CrossFade("air_rise", 0.2f);
                }
            }
            if (baseAnimation.IsPlaying("toRoof"))
            {
                if (baseAnimation["toRoof"].normalizedTime < 0.22f)
                {
                    baseRigidBody.velocity = Vector3.zero;
                    baseRigidBody.AddForce(new Vector3(0f, gravity * baseRigidBody.mass, 0f));
                }
                else
                {
                    if (!wallJump)
                    {
                        wallJump = true;
                        baseRigidBody.AddForce(Vector3.up * 8f, ForceMode.Impulse);
                    }
                    baseRigidBody.AddForce(baseTransform.forward * 0.05f, ForceMode.Impulse);
                }
                if (baseAnimation["toRoof"].normalizedTime >= 1f)
                {
                    PlayAnimation("air_rise");
                }
            }
            else if (state == HeroState.Idle && isPressDirectionTowardsHero(num, num2) && !inputManager.isInput[InputCode.Jump] && !inputManager.isInput[InputCode.HookLeft] && !inputManager.isInput[InputCode.HookRight] && !inputManager.isInput[InputCode.HookBoth] && IsFrontGrounded() && !baseAnimation.IsPlaying("wallrun") && !baseAnimation.IsPlaying("dodge"))
            {
                CrossFade("wallrun", 0.1f);
                wallRunTime = 0f;
            }
            else if (baseAnimation.IsPlaying("wallrun"))
            {
                baseRigidBody.AddForce(Vector3.up * speed - baseRigidBody.velocity, ForceMode.VelocityChange);
                wallRunTime += Time.deltaTime;
                if (wallRunTime > 1f || (num2 == 0f && num == 0f))
                {
                    baseRigidBody.AddForce(-baseTransform.forward * speed * 0.75f, ForceMode.Impulse);
                    Dodge2(offTheWall: true);
                }
                else if (!IsUpFrontGrounded())
                {
                    wallJump = false;
                    CrossFade("toRoof", 0.1f);
                }
                else if (!IsFrontGrounded())
                {
                    CrossFade("air_fall", 0.1f);
                }
            }
            else if (!baseAnimation.IsPlaying("attack5") && !baseAnimation.IsPlaying("special_petra") && !baseAnimation.IsPlaying("dash") && !baseAnimation.IsPlaying("jump"))
            {
                Vector3 vector6 = new Vector3(num, 0f, num2);
                float num8 = GetGlobalFacingDirection(num, num2);
                Vector3 globaleFacingVector = GetGlobalFacingVector(num8);
                float num9 = (!(vector6.magnitude <= 0.95f)) ? 1f : ((vector6.magnitude >= 0.25f) ? vector6.magnitude : 0f);
                globaleFacingVector *= num9;
                globaleFacingVector *= (float)setup.myCostume.stat.Accel / 10f * 2f;
                if (num == 0f && num2 == 0f)
                {
                    if (state == HeroState.Attack)
                    {
                        globaleFacingVector *= 0f;
                    }
                    num8 = -874f;
                }
                if (num8 != -874f)
                {
                    facingDirection = num8;
                    targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                }
                if (!flag2 && !flag3 && !isMounted && inputManager.isInput[InputCode.Jump] && currentGas > 0f)
                {
                    if (num != 0f || num2 != 0f)
                    {
                        baseRigidBody.AddForce(globaleFacingVector, ForceMode.Acceleration);
                    }
                    else
                    {
                        baseRigidBody.AddForce(baseTransform.forward * globaleFacingVector.magnitude, ForceMode.Acceleration);
                    }
                    flag = true;
                }
            }
            if (baseAnimation.IsPlaying("air_fall") && currentSpeed < 0.2f && IsFrontGrounded())
            {
                CrossFade("onWall", 0.3f);
            }
        }
        if (flag2 && flag3)
        {
            float d2 = currentSpeed + 0.1f;
            baseRigidBody.AddForce(-baseRigidBody.velocity, ForceMode.VelocityChange);
            Vector3 current2 = (bulletRight.transform.position + bulletLeft.transform.position) * 0.5f - baseTransform.position;
            float reelDir = GetReelAxis();
            reelDir = Mathf.Clamp(reelDir, -0.8f, 0.8f);
            float num11 = 1f + reelDir;
            Vector3 a2 = Vector3.RotateTowards(current2, baseRigidBody.velocity, 1.53938f * num11, 1.53938f * num11);
            a2.Normalize();
            baseRigidBody.velocity = a2 * d2;
        }
        else if (flag2)
        {
            float d3 = currentSpeed + 0.1f;
            baseRigidBody.AddForce(-baseRigidBody.velocity, ForceMode.VelocityChange);
            Vector3 current3 = bulletLeft.transform.position - baseTransform.position;
            float reelDir = GetReelAxis();
            reelDir = Mathf.Clamp(reelDir, -0.8f, 0.8f);
            float num13 = 1f + reelDir;
            Vector3 a3 = Vector3.RotateTowards(current3, baseRigidBody.velocity, 1.53938f * num13, 1.53938f * num13);
            a3.Normalize();
            baseRigidBody.velocity = a3 * d3;
        }
        else if (flag3)
        {
            float d4 = currentSpeed + 0.1f;
            baseRigidBody.AddForce(-baseRigidBody.velocity, ForceMode.VelocityChange);
            Vector3 current4 = bulletRight.transform.position - baseTransform.position;
            float reelDir = GetReelAxis();
            reelDir = Mathf.Clamp(reelDir, -0.8f, 0.8f);
            float num15 = 1f + reelDir;
            Vector3 a4 = Vector3.RotateTowards(current4, baseRigidBody.velocity, 1.53938f * num15, 1.53938f * num15);
            a4.Normalize();
            baseRigidBody.velocity = a4 * d4;
        }
        if (state == HeroState.Attack && (attackAnimation == "attack5" || attackAnimation == "special_petra") && baseAnimation[attackAnimation].normalizedTime > 0.4f && !attackMove)
        {
            attackMove = true;
            if (launchPointRight.sqrMagnitude > 0f)
            {
                Vector3 force2 = launchPointRight - baseTransform.position;
                force2.Normalize();
                force2 *= 13f;
                baseRigidBody.AddForce(force2, ForceMode.Impulse);
            }
            if (attackAnimation == "special_petra" && launchPointLeft.sqrMagnitude > 0f)
            {
                Vector3 force3 = launchPointLeft - baseTransform.position;
                force3.Normalize();
                force3 *= 13f;
                baseRigidBody.AddForce(force3, ForceMode.Impulse);
                if (bulletRight != null)
                {
                    bulletRight.GetComponent<Bullet>().Disable();
                    ReleaseHookedTarget();
                }
                if (bulletLeft != null)
                {
                    bulletLeft.GetComponent<Bullet>().Disable();
                    ReleaseHookedTarget();
                }
            }
            baseRigidBody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        }
        bool hookedToSomething = false;
        if (bulletLeft != null || bulletRight != null)
        {
            if (bulletLeft != null && bulletLeft.transform.position.y > base.gameObject.transform.position.y && isLaunchLeft && bulletLeft.GetComponent<Bullet>().IsHooked())
            {
                hookedToSomething = true;
            }
            if (bulletRight != null && bulletRight.transform.position.y > base.gameObject.transform.position.y && isLaunchRight && bulletRight.GetComponent<Bullet>().IsHooked())
            {
                hookedToSomething = true;
            }
        }
        if (hookedToSomething)
        {
            baseRigidBody.AddForce(new Vector3(0f, -10f * baseRigidBody.mass, 0f));
        }
        else
        {
            baseRigidBody.AddForce(new Vector3(0f, (0f - gravity) * baseRigidBody.mass, 0f));
        }
        if (currentSpeed > 10f)
        {
            currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, Mathf.Min(100f, currentSpeed + 40f), 0.1f);
        }
        else
        {
            currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, 50f, 0.1f);
        }

        // TODO: Mod, gas spam fix
        if (!cancelGasDisable)
        {
            if (flag)
            {
                UseGas(useGasSpeed * Time.deltaTime);
                if (!smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && base.photonView.isMine)
                {
                    base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, true);
                }
                smoke_3dmg.enableEmission = true;
            }
            else
            {
                if (smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && base.photonView.isMine)
                {
                    base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, false);
                }
                smoke_3dmg.enableEmission = false;
            }
        }
        else
        {
            cancelGasDisable = false;
        }

        if (currentSpeed > 80f)
        {
            if (!speedFXPS.enableEmission)
            {
                speedFXPS.enableEmission = true;
            }
            speedFXPS.startSpeed = currentSpeed;
            speedFX.transform.LookAt(baseTransform.position + baseRigidBody.velocity);
        }
        else if (speedFXPS.enableEmission)
        {
            speedFXPS.enableEmission = false;
        }

        reelInAxis = 0f;
    }

    private void UpdateResourceUI()
    {
        float gasPercent = currentGas / totalGas;
        cachedSprites["gasL1"].fillAmount = gasPercent;
        cachedSprites["gasR1"].fillAmount = gasPercent;

        if (gasPercent <= 0f)
        {
            cachedSprites["gasL"].color = Color.red;
            cachedSprites["gasR"].color = Color.red;
        }
        else if (gasPercent < 0.25f)
        {
            cachedSprites["gasL"].color = Guardian.Utilities.Colors.Orange;
            cachedSprites["gasR"].color = Guardian.Utilities.Colors.Orange;
        }
        else if (gasPercent < 0.5f)
        {
            cachedSprites["gasL"].color = Color.yellow;
            cachedSprites["gasR"].color = Color.yellow;
        }
        else
        {
            cachedSprites["gasL"].color = Color.white;
            cachedSprites["gasR"].color = Color.white;
        }

        if (!useGun)
        {
            float blaPercent = currentBladeSta / totalBladeSta;

            cachedSprites["bladeCL"].fillAmount = blaPercent;
            cachedSprites["bladeCR"].fillAmount = blaPercent;

            if (blaPercent <= 0f)
            {
                cachedSprites["bladel1"].color = Color.red;
                cachedSprites["blader1"].color = Color.red;
            }
            else if (blaPercent < 0.25f)
            {

                cachedSprites["bladel1"].color = Guardian.Utilities.Colors.Orange;
                cachedSprites["blader1"].color = Guardian.Utilities.Colors.Orange;
            }
            else if (blaPercent < 0.5f)
            {
                cachedSprites["bladel1"].color = Color.yellow;
                cachedSprites["blader1"].color = Color.yellow;
            }
            else
            {
                cachedSprites["bladel1"].color = Color.white;
                cachedSprites["blader1"].color = Color.white;
            }

            if (currentBladeNum <= 4)
            {
                cachedSprites["bladel5"].enabled = false;
                cachedSprites["blader5"].enabled = false;
            }
            else
            {
                cachedSprites["bladel5"].enabled = true;
                cachedSprites["blader5"].enabled = true;
            }

            if (currentBladeNum <= 3)
            {
                cachedSprites["bladel4"].enabled = false;
                cachedSprites["blader4"].enabled = false;
            }
            else
            {
                cachedSprites["bladel4"].enabled = true;
                cachedSprites["blader4"].enabled = true;
            }

            if (currentBladeNum <= 2)
            {
                cachedSprites["bladel3"].enabled = false;
                cachedSprites["blader3"].enabled = false;
            }
            else
            {
                cachedSprites["bladel3"].enabled = true;
                cachedSprites["blader3"].enabled = true;
            }

            if (currentBladeNum <= 1)
            {
                cachedSprites["bladel2"].enabled = false;
                cachedSprites["blader2"].enabled = false;
            }
            else
            {
                cachedSprites["bladel2"].enabled = true;
                cachedSprites["blader2"].enabled = true;
            }

            if (currentBladeNum <= 0)
            {
                cachedSprites["bladel1"].enabled = false;
                cachedSprites["blader1"].enabled = false;
            }
            else
            {
                cachedSprites["bladel1"].enabled = true;
                cachedSprites["blader1"].enabled = true;
            }
        }
        else
        {
            cachedSprites["bulletL"].enabled = leftGunHasBullet;
            cachedSprites["bulletR"].enabled = rightGunHasBullet;
        }
    }

    private void UpdateCrossHair()
    {
        if (Screen.showCursor)
        {
            Vector3 localPosition = Vector3.up * 10000f;
            crossR2.transform.localPosition = localPosition;
            crossR1.transform.localPosition = localPosition;
            crossL2.transform.localPosition = localPosition;
            crossL1.transform.localPosition = localPosition;
            cross2.transform.localPosition = localPosition;
            cross1.transform.localPosition = localPosition;

            LabelDistance.transform.localPosition = localPosition;
            return;
        }

        CheckTitan();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        LayerMask layerMask = (int)mask2 | (int)mask;
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, 1E+07f, layerMask.value))
        {
            // TODO: Mod, Stop cross-hair freezes?
            // return;
        }
        cross1.transform.localPosition = Input.mousePosition;
        cross1.transform.localPosition -= new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
        cross2.transform.localPosition = cross1.transform.localPosition;
        float magnitude = (hitInfo.point - baseTransform.position).magnitude;
        GameObject labelDistance2 = LabelDistance;
        string text = (magnitude <= 1000f) ? ((int)magnitude).ToString() : "???";
        if ((int)FengGameManagerMKII.Settings[189] == 1)
        {
            float _currentSpeed = myGroup.Equals(GroupType.AHSS) ? (currentSpeed * 0.588f) : currentSpeed;
            text += "\n" + _currentSpeed.ToString("F1") + "u/s";
        }
        else if ((int)FengGameManagerMKII.Settings[189] == 2)
        {
            // Adjustment for AHSS vs Blade damage to speed ratio
            float _currentSpeed = myGroup.Equals(GroupType.AHSS) ? (currentSpeed * 0.588f) : currentSpeed;
            text += "\n" + (_currentSpeed / 100f).ToString("F1") + "K";
        }
        labelDistance2.GetComponent<UILabel>().text = text;
        if (magnitude > 120f)
        {
            cross1.transform.localPosition += Vector3.up * 10000f;
            labelDistance2.transform.localPosition = cross2.transform.localPosition;
        }
        else
        {
            cross2.transform.localPosition += Vector3.up * 10000f;
            labelDistance2.transform.localPosition = cross1.transform.localPosition;
        }
        labelDistance2.transform.localPosition -= new Vector3(0f, 15f, 0f);

        // TODO: Mod, hide hook arrows
        if (Guardian.Mod.Properties.HideHookArrows.Value)
        {
            Vector3 localPosition = Vector3.up * 10000f;
            crossR2.transform.localPosition = localPosition;
            crossR1.transform.localPosition = localPosition;
            crossL2.transform.localPosition = localPosition;
            crossL1.transform.localPosition = localPosition;
            return;
        }

        Vector3 b = new Vector3(0f, 0.4f, 0f);
        b -= baseTransform.right * 0.3f;
        Vector3 b2 = new Vector3(0f, 0.4f, 0f);
        b2 += baseTransform.right * 0.3f;
        float d = (hitInfo.distance <= 50f) ? (hitInfo.distance * 0.05f) : (hitInfo.distance * 0.3f);
        Vector3 b3 = hitInfo.point - baseTransform.right * d - (baseTransform.position + b);
        Vector3 b4 = hitInfo.point + baseTransform.right * d - (baseTransform.position + b2);
        b3.Normalize();
        b4.Normalize();
        b3 *= 1000000f;
        b4 *= 1000000f;
        if (Physics.Linecast(baseTransform.position + b, baseTransform.position + b + b3, out RaycastHit hitInfo2, layerMask.value))
        {
            GameObject gameObject9 = crossL1;
            gameObject9.transform.localPosition = currentCamera.WorldToScreenPoint(hitInfo2.point);
            gameObject9.transform.localPosition -= new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
            gameObject9.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(gameObject9.transform.localPosition.y - (Input.mousePosition.y - (float)Screen.height * 0.5f), gameObject9.transform.localPosition.x - (Input.mousePosition.x - (float)Screen.width * 0.5f)) * 57.29578f + 180f);
            GameObject gameObject10 = crossL2;
            gameObject10.transform.localPosition = gameObject9.transform.localPosition;
            gameObject10.transform.localRotation = gameObject9.transform.localRotation;
            if (hitInfo2.distance > 120f)
            {
                gameObject9.transform.localPosition += Vector3.up * 10000f;
            }
            else
            {
                gameObject10.transform.localPosition += Vector3.up * 10000f;
            }
        }
        if (Physics.Linecast(baseTransform.position + b2, baseTransform.position + b2 + b4, out hitInfo2, layerMask.value))
        {
            GameObject gameObject11 = crossR1;
            gameObject11.transform.localPosition = currentCamera.WorldToScreenPoint(hitInfo2.point);
            gameObject11.transform.localPosition -= new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
            gameObject11.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(gameObject11.transform.localPosition.y - (Input.mousePosition.y - (float)Screen.height * 0.5f), gameObject11.transform.localPosition.x - (Input.mousePosition.x - (float)Screen.width * 0.5f)) * 57.29578f);
            GameObject gameObject12 = crossR2;
            gameObject12.transform.localPosition = gameObject11.transform.localPosition;
            gameObject12.transform.localRotation = gameObject11.transform.localRotation;
            if (hitInfo2.distance > 120f)
            {
                gameObject11.transform.localPosition += Vector3.up * 10000f;
            }
            else
            {
                gameObject12.transform.localPosition += Vector3.up * 10000f;
            }
        }
    }

    private void UpdateFlareCooldown()
    {
        if (cachedSprites["UIflare1"] != null)
        {
            cachedSprites["UIflare1"].fillAmount = (flareTotalCD - flare1CD) / flareTotalCD;
            cachedSprites["UIflare2"].fillAmount = (flareTotalCD - flare2CD) / flareTotalCD;
            cachedSprites["UIflare3"].fillAmount = (flareTotalCD - flare3CD) / flareTotalCD;
        }
    }

    private void OnDestroy()
    {
        // Anarchy
        Anarchy.Custom.Level.CustomAnarchyLevel.Instance.RemoveHero(this);

        if (myFlashlight != null)
        {
            UnityEngine.Object.Destroy(myFlashlight);
        }
        if (myNetWorkName != null)
        {
            UnityEngine.Object.Destroy(myNetWorkName);
        }
        if (gunDummy != null)
        {
            UnityEngine.Object.Destroy(gunDummy);
        }
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
        {
            ReleaseHookedTarget();
        }
        GameObject mm = GameObject.Find("MultiplayerManager");
        if (mm != null)
        {
            mm.GetComponent<FengGameManagerMKII>().RemoveHero(this);
        }
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
        {
            Vector3 localPosition = Vector3.up * 5000f;
            cross1.transform.localPosition = localPosition;
            cross2.transform.localPosition = localPosition;
            crossL1.transform.localPosition = localPosition;
            crossL2.transform.localPosition = localPosition;
            crossR1.transform.localPosition = localPosition;
            crossR2.transform.localPosition = localPosition;
            LabelDistance.transform.localPosition = localPosition;
        }
        if (setup.part_cape != null)
        {
            ClothFactory.DisposeObject(setup.part_cape);
        }
        if (setup.part_hair_1 != null)
        {
            ClothFactory.DisposeObject(setup.part_hair_1);
        }
        if (setup.part_hair_2 != null)
        {
            ClothFactory.DisposeObject(setup.part_hair_2);
        }
    }

    public void SetSkillHUDPosition2()
    {
        skillCD = GameObject.Find("skill_cd_" + skillIDHUD);
        if (skillCD != null)
        {
            skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        }

        if (useGun && RCSettings.BombMode == 0)
        {
            skillCD.transform.localPosition = Vector3.up * 5000f;
        }
    }

    public void lateUpdate2()
    {
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && myNetWorkName != null)
        {
            if (titanForm && eren_titan != null)
            {
                myNetWorkName.transform.localPosition = Vector3.up * Screen.height * 2f;
            }
            Vector3 vector = new Vector3(baseTransform.position.x, baseTransform.position.y + 2f, baseTransform.position.z);
            GameObject gameObject = maincamera;
            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = (int)mask2 | (int)mask;
            if (Vector3.Angle(gameObject.transform.forward, vector - gameObject.transform.position) > 90f || Physics.Linecast(vector, gameObject.transform.position, mask3))
            {
                myNetWorkName.transform.localPosition = Vector3.up * Screen.height * 2f;
            }
            else
            {
                Vector2 vector2 = maincamera.GetComponent<Camera>().WorldToScreenPoint(vector);
                myNetWorkName.transform.localPosition = new Vector3((int)(vector2.x - (float)Screen.width * 0.5f), (int)(vector2.y - (float)Screen.height * 0.5f), 0f);
            }
        }
        if (titanForm || isCannon)
        {
            return;
        }
        if (IN_GAME_MAIN_CAMERA.CameraTilt == 1 && (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine))
        {
            Vector3 vector3 = Vector3.zero;
            Vector3 vector4 = Vector3.zero;
            if (isLaunchLeft && bulletLeft != null && bulletLeft.GetComponent<Bullet>().IsHooked())
            {
                vector3 = bulletLeft.transform.position;
            }
            if (isLaunchRight && bulletRight != null && bulletRight.GetComponent<Bullet>().IsHooked())
            {
                vector4 = bulletRight.transform.position;
            }
            Vector3 a = Vector3.zero;
            if (vector3.sqrMagnitude != 0f && vector4.sqrMagnitude == 0f)
            {
                a = vector3;
            }
            else if (vector3.sqrMagnitude == 0f && vector4.sqrMagnitude != 0f)
            {
                a = vector4;
            }
            else if (vector3.sqrMagnitude != 0f && vector4.sqrMagnitude != 0f)
            {
                a = (vector3 + vector4) * 0.5f;
            }
            Vector3 vector5 = Vector3.Project(a - baseTransform.position, maincamera.transform.up);
            Vector3 b = Vector3.Project(a - baseTransform.position, maincamera.transform.right);
            Quaternion to2;
            if (a.sqrMagnitude > 0f)
            {
                Vector3 to = vector5 + b;
                float num = Vector3.Angle(a - baseTransform.position, baseRigidBody.velocity) * 0.005f;
                to2 = Quaternion.Euler(z: ((maincamera.transform.right + b.normalized).sqrMagnitude >= 1f) ? ((0f - Vector3.Angle(vector5, to)) * num) : (Vector3.Angle(vector5, to) * num), x: maincamera.transform.rotation.eulerAngles.x, y: maincamera.transform.rotation.eulerAngles.y);
            }
            else
            {
                to2 = Quaternion.Euler(maincamera.transform.rotation.eulerAngles.x, maincamera.transform.rotation.eulerAngles.y, 0f);
            }
            maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, to2, Time.deltaTime * 2f);
        }
        if (state == HeroState.Grabbed && titanWhoGrabMe != null)
        {
            TITAN titanObj = titanWhoGrabMe.GetComponent<TITAN>();
            if (titanObj != null)
            {
                baseTransform.position = titanObj.grabTF.transform.position;
                baseTransform.rotation = titanObj.grabTF.transform.rotation;
            }
            else
            {
                FEMALE_TITAN annie = titanWhoGrabMe.GetComponent<FEMALE_TITAN>();
                if (annie != null)
                {
                    baseTransform.position = annie.grabTF.transform.position;
                    baseTransform.rotation = annie.grabTF.transform.rotation;
                }
            }
        }
        if (useGun)
        {
            if (leftArmAim || rightArmAim)
            {
                Vector3 vector6 = gunTarget - baseTransform.position;
                float current = (0f - Mathf.Atan2(vector6.z, vector6.x)) * 57.29578f;
                float num2 = 0f - Mathf.DeltaAngle(current, baseTransform.rotation.eulerAngles.y - 90f);
                MoveHead();
                if (!isLeftHandHooked && leftArmAim && num2 < 40f && num2 > -90f)
                {
                    AimLeftArmTo(gunTarget);
                }
                if (!isRightHandHooked && rightArmAim && num2 > -40f && num2 < 90f)
                {
                    ArmRightArmTo(gunTarget);
                }
            }
            else if (!grounded)
            {
                handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
                handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
            }
            if (isLeftHandHooked && bulletLeft != null)
            {
                AimLeftArmTo(bulletLeft.transform.position);
            }
            if (isRightHandHooked && bulletRight != null)
            {
                ArmRightArmTo(bulletRight.transform.position);
            }
        }

        if (!base.rigidbody.interpolation.Equals(RigidbodyInterpolation.Interpolate))
        {
            SetHookedPplDirection();
            LeanBody();
        }
    }

    public void update2()
    {
        if (IN_GAME_MAIN_CAMERA.IsPausing)
        {
            return;
        }
        if (invincible > 0f)
        {
            invincible -= Time.deltaTime;
        }
        if (hasDied)
        {
            return;
        }
        if (titanForm && eren_titan != null)
        {
            baseTransform.position = eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
            base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        }
        else if (isCannon && myCannon != null)
        {
            updateCannon();
            base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        }
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && !base.photonView.isMine)
        {
            return;
        }

        UpdateReelInput();

        if (myCannonRegion != null)
        {
            FengGameManagerMKII.Instance.SetTextCenter("Press 'Cannon Mount' key to use Cannon.");
            if (FengGameManagerMKII.InputRC.isInputCannonDown(InputCodeRC.CannonMount))
            {
                myCannonRegion.photonView.RPC("RequestControlRPC", PhotonTargets.MasterClient, base.photonView.viewID);
            }
        }
        if (state == HeroState.Grabbed && !useGun)
        {
            if (skillId == "jean")
            {
                if (state != HeroState.Attack && (inputManager.isInputDown[InputCode.Attack0] || inputManager.isInputDown[InputCode.Attack1]) && escapeTimes > 0 && !baseAnimation.IsPlaying("grabbed_jean"))
                {
                    PlayAnimation("grabbed_jean");
                    baseAnimation["grabbed_jean"].time = 0f;
                    escapeTimes--;
                }
                if (!baseAnimation.IsPlaying("grabbed_jean") || !(baseAnimation["grabbed_jean"].normalizedTime > 0.64f) || !(titanWhoGrabMe.GetComponent<TITAN>() != null))
                {
                    return;
                }
                Ungrab();
                baseRigidBody.velocity = Vector3.up * 30f;
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                {
                    titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                    return;
                }
                base.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All);
                if (PhotonNetwork.isMasterClient)
                {
                    titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                }
                else
                {
                    PhotonView.Find(titanWhoGrabMeID).RPC("grabbedTargetEscape", PhotonTargets.MasterClient);
                }
            }
            else
            {
                if (!(skillId == "eren"))
                {
                    return;
                }
                ShowSkillCountDown();
                if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer || !IN_GAME_MAIN_CAMERA.IsPausing)
                {
                    TickSkillCooldown();
                    TickFlareCooldown();
                }
                if (!inputManager.isInputDown[InputCode.Attack1])
                {
                    return;
                }
                if (skillCDDuration > 0f)
                {
                    return;
                }
                skillCDDuration = skillCDLast;
                TITAN titanObj = titanWhoGrabMe.GetComponent<TITAN>();
                if (!(skillId == "eren") || titanObj == null)
                {
                    return;
                }
                Ungrab();
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                {
                    titanObj.grabbedTargetEscape();
                }
                else
                {
                    base.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All);
                    if (PhotonNetwork.isMasterClient)
                    {
                        titanObj.grabbedTargetEscape();
                    }
                    else
                    {
                        PhotonView.Find(titanWhoGrabMeID).photonView.RPC("grabbedTargetEscape", PhotonTargets.MasterClient);
                    }
                }
                TransformIntoEren();
            }
        }
        else if (!titanForm && !isCannon)
        {
            UpdateBuffer();
            ExtendedUpdate();
            if (!grounded && state != HeroState.Dashing)
            {
                // TODO: Mod, fix rebind disabling double-tap
                if ((int)FengGameManagerMKII.Settings[181] == 1)
                {
                    CheckDashRebind();
                }

                if (Guardian.Mod.Properties.DoubleTapBurst.Value)
                {
                    CheckDoubleTapDash();
                }

                if (dashD)
                {
                    dashD = false;
                    Dash(0f, -1f);
                    return;
                }
                if (dashU)
                {
                    dashU = false;
                    Dash(0f, 1f);
                    return;
                }
                if (dashL)
                {
                    dashL = false;
                    Dash(-1f, 0f);
                    return;
                }
                if (dashR)
                {
                    dashR = false;
                    Dash(1f, 0f);
                    return;
                }
            }
            if (grounded && (state == HeroState.Idle || state == HeroState.Slide))
            {
                if (inputManager.isInputDown[InputCode.Jump] && !baseAnimation.IsPlaying("jump") && !baseAnimation.IsPlaying("horse_geton"))
                {
                    Idle();
                    CrossFade("jump", 0.1f);
                    sparks.enableEmission = false;
                }
                if (FengGameManagerMKII.InputRC.isInputHorseDown(InputCodeRC.HorseMount) && !baseAnimation.IsPlaying("jump") && !baseAnimation.IsPlaying("horse_geton") && myHorse != null && !isMounted && Vector3.Distance(myHorse.transform.position, base.transform.position) < 15f)
                {
                    GetOnHorse();
                }
                if (inputManager.isInputDown[InputCode.Dodge] && !baseAnimation.IsPlaying("jump") && !baseAnimation.IsPlaying("horse_geton"))
                {
                    Dodge2();
                    return;
                }
            }
            switch (state)
            {
                case HeroState.Idle:
                    if (inputManager.isInputDown[InputCode.Flare1])
                    {
                        ShootFlare(1);
                    }
                    if (inputManager.isInputDown[InputCode.Flare2])
                    {
                        ShootFlare(2);
                    }
                    if (inputManager.isInputDown[InputCode.Flare3])
                    {
                        ShootFlare(3);
                    }
                    if (inputManager.isInputDown[InputCode.Restart])
                    {
                        Suicide();
                    }
                    if (myHorse != null && isMounted && FengGameManagerMKII.InputRC.isInputHorseDown(InputCodeRC.HorseMount))
                    {
                        GetOffHorse();
                    }
                    if ((base.animation.IsPlaying(standAnimation) || !grounded) && inputManager.isInputDown[InputCode.Reload] && (!useGun || RCSettings.AhssReload != 1 || grounded))
                    {
                        ChangeBlade();
                        return;
                    }
                    if (baseAnimation.IsPlaying(standAnimation) && inputManager.isInputDown[InputCode.Salute])
                    {
                        Salute();
                        return;
                    }
                    if (!isMounted && (inputManager.isInputDown[InputCode.Attack0] || inputManager.isInputDown[InputCode.Attack1]) && !useGun)
                    {
                        bool flag2 = false;
                        if (inputManager.isInputDown[InputCode.Attack1])
                        {
                            if (skillCDDuration > 0f || flag2)
                            {
                                flag2 = true;
                            }
                            else
                            {
                                skillCDDuration = skillCDLast;
                                switch (skillId)
                                {
                                    case "eren":
                                        TransformIntoEren();
                                        return;
                                    case "marco":
                                        if (IsGrounded())
                                        {
                                            attackAnimation = ((UnityEngine.Random.Range(0, 2) != 0) ? "special_marco_1" : "special_marco_0");
                                            PlayAnimation(attackAnimation);
                                        }
                                        else
                                        {
                                            flag2 = true;
                                            skillCDDuration = 0f;
                                        }
                                        break;
                                    case "armin":
                                        if (IsGrounded())
                                        {
                                            attackAnimation = "special_armin";
                                            PlayAnimation("special_armin");
                                        }
                                        else
                                        {
                                            flag2 = true;
                                            skillCDDuration = 0f;
                                        }
                                        break;
                                    case "sasha":
                                        if (IsGrounded())
                                        {
                                            attackAnimation = "special_sasha";
                                            PlayAnimation("special_sasha");
                                            currentBuff = BUFF.Speed;
                                            buffTime = 10f;
                                        }
                                        else
                                        {
                                            flag2 = true;
                                            skillCDDuration = 0f;
                                        }
                                        break;
                                    case "mikasa":
                                        attackAnimation = "attack3_1";
                                        PlayAnimation("attack3_1");
                                        baseRigidBody.velocity = Vector3.up * 10f;
                                        break;
                                    case "levi":
                                        attackAnimation = "attack5";
                                        PlayAnimation("attack5");
                                        baseRigidBody.velocity += Vector3.up * 5f;
                                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                                        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
                                        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1E+07f, ((LayerMask)((int)mask2 | (int)mask)).value))
                                        {
                                            if (bulletRight != null)
                                            {
                                                bulletRight.GetComponent<Bullet>().Disable();
                                                ReleaseHookedTarget();
                                            }
                                            dashDirection = hitInfo.point - baseTransform.position;
                                            LaunchRightHook(hitInfo, single: true, 1);
                                            rope.Play();
                                        }
                                        facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * 57.29578f;
                                        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                        attackLoop = 3;
                                        break;
                                    case "petra":
                                        attackAnimation = "special_petra";
                                        PlayAnimation("special_petra");
                                        baseRigidBody.velocity += Vector3.up * 5f;
                                        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                        LayerMask mask3 = 1 << LayerMask.NameToLayer("Ground");
                                        LayerMask mask4 = 1 << LayerMask.NameToLayer("EnemyBox");
                                        if (Physics.Raycast(ray2, out RaycastHit hitInfo2, 1E+07f, ((LayerMask)((int)mask4 | (int)mask3)).value))
                                        {
                                            if (bulletRight != null)
                                            {
                                                bulletRight.GetComponent<Bullet>().Disable();
                                                ReleaseHookedTarget();
                                            }
                                            if (bulletLeft != null)
                                            {
                                                bulletLeft.GetComponent<Bullet>().Disable();
                                                ReleaseHookedTarget();
                                            }
                                            dashDirection = hitInfo2.point - baseTransform.position;
                                            LaunchLeftHook(hitInfo2, single: true);
                                            LaunchRightHook(hitInfo2, single: true);
                                            rope.Play();
                                        }
                                        facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * 57.29578f;
                                        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                        attackLoop = 3;
                                        break;
                                    default:
                                        if (needLean)
                                        {
                                            if (leanLeft)
                                            {
                                                attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2");
                                            }
                                            else
                                            {
                                                attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2");
                                            }
                                        }
                                        else
                                        {
                                            attackAnimation = "attack1";
                                        }
                                        PlayAnimation(attackAnimation);
                                        break;
                                }
                            }
                        }
                        else if (inputManager.isInputDown[InputCode.Attack0])
                        {
                            // TODO: Mod, weapon trail for when you're holding attack down
                            if ((int)FengGameManagerMKII.Settings[92] == 0 && Guardian.Mod.Properties.HoldForBladeTrails.Value)
                            {
                                leftbladetrail2.Activate();
                                rightbladetrail2.Activate();
                                leftbladetrail.Activate();
                                rightbladetrail.Activate();
                            }

                            if (needLean)
                            {
                                if (inputManager.isInput[InputCode.Left])
                                {
                                    attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2");
                                }
                                else if (inputManager.isInput[InputCode.Right])
                                {
                                    attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2");
                                }
                                else if (leanLeft)
                                {
                                    attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2");
                                }
                                else
                                {
                                    attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2");
                                }
                            }
                            else if (inputManager.isInput[InputCode.Left])
                            {
                                attackAnimation = "attack2";
                            }
                            else if (inputManager.isInput[InputCode.Right])
                            {
                                attackAnimation = "attack1";
                            }
                            else if (lastHook != null)
                            {
                                Transform neckObj = lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                if (neckObj != null)
                                {
                                    attackAccordingToTarget(neckObj);
                                }
                                else
                                {
                                    flag2 = true;
                                }
                            }
                            else if (bulletLeft != null && bulletLeft.transform.parent != null)
                            {
                                Transform transform = bulletLeft.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                if (transform != null)
                                {
                                    attackAccordingToTarget(transform);
                                }
                                else
                                {
                                    attackAccordingToMouse();
                                }
                            }
                            else if (bulletRight != null && bulletRight.transform.parent != null)
                            {
                                Transform transform2 = bulletRight.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                if (transform2 != null)
                                {
                                    attackAccordingToTarget(transform2);
                                }
                                else
                                {
                                    attackAccordingToMouse();
                                }
                            }
                            else
                            {
                                GameObject gameObject = FindNearestTitan();
                                if (gameObject != null)
                                {
                                    Transform transform3 = gameObject.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                    if (transform3 != null)
                                    {
                                        attackAccordingToTarget(transform3);
                                    }
                                    else
                                    {
                                        attackAccordingToMouse();
                                    }
                                }
                                else
                                {
                                    attackAccordingToMouse();
                                }
                            }
                        }
                        if (!flag2)
                        {
                            checkBoxLeft.GetComponent<TriggerColliderWeapon>().ClearHits();
                            checkBoxRight.GetComponent<TriggerColliderWeapon>().ClearHits();
                            if (grounded)
                            {
                                baseRigidBody.AddForce(base.gameObject.transform.forward * 200f);
                            }
                            PlayAnimation(attackAnimation);
                            baseAnimation[attackAnimation].time = 0f;
                            buttonAttackRelease = false;
                            state = HeroState.Attack;
                            if (grounded || attackAnimation == "attack3_1" || attackAnimation == "attack5" || attackAnimation == "special_petra")
                            {
                                attackReleased = true;
                                buttonAttackRelease = true;
                            }
                            else
                            {
                                attackReleased = false;
                            }
                            sparks.enableEmission = false;
                        }
                    }
                    if (useGun)
                    {
                        if (inputManager.isInput[InputCode.Attack1])
                        {
                            leftArmAim = true;
                            rightArmAim = true;
                        }
                        else if (inputManager.isInput[InputCode.Attack0])
                        {
                            if (leftGunHasBullet)
                            {
                                leftArmAim = true;
                                rightArmAim = false;
                            }
                            else
                            {
                                leftArmAim = false;
                                if (rightGunHasBullet)
                                {
                                    rightArmAim = true;
                                }
                                else
                                {
                                    rightArmAim = false;
                                }
                            }
                        }
                        else
                        {
                            leftArmAim = false;
                            rightArmAim = false;
                        }
                        if (leftArmAim || rightArmAim)
                        {
                            Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
                            LayerMask mask5 = 1 << LayerMask.NameToLayer("Ground");
                            LayerMask mask6 = 1 << LayerMask.NameToLayer("EnemyBox");
                            if (Physics.Raycast(ray3, out RaycastHit hitInfo3, 1E+07f, ((LayerMask)((int)mask6 | (int)mask5)).value))
                            {
                                gunTarget = hitInfo3.point;
                            }
                        }
                        bool flag3 = false;
                        bool flag4 = false;
                        bool flag5 = false;
                        if (inputManager.isInputUp[InputCode.Attack1] && skillId != "bomb")
                        {
                            if (leftGunHasBullet && rightGunHasBullet)
                            {
                                if (grounded)
                                {
                                    attackAnimation = "AHSS_shoot_both";
                                }
                                else
                                {
                                    attackAnimation = "AHSS_shoot_both_air";
                                }
                                flag3 = true;
                            }
                            else if (!leftGunHasBullet && !rightGunHasBullet)
                            {
                                flag4 = true;
                            }
                            else
                            {
                                flag5 = true;
                            }
                        }
                        if (flag5 || inputManager.isInputUp[InputCode.Attack0])
                        {
                            if (grounded)
                            {
                                if (leftGunHasBullet && rightGunHasBullet)
                                {
                                    if (isLeftHandHooked)
                                    {
                                        attackAnimation = "AHSS_shoot_r";
                                    }
                                    else
                                    {
                                        attackAnimation = "AHSS_shoot_l";
                                    }
                                }
                                else if (leftGunHasBullet)
                                {
                                    attackAnimation = "AHSS_shoot_l";
                                }
                                else if (rightGunHasBullet)
                                {
                                    attackAnimation = "AHSS_shoot_r";
                                }
                            }
                            else if (leftGunHasBullet && rightGunHasBullet)
                            {
                                if (isLeftHandHooked)
                                {
                                    attackAnimation = "AHSS_shoot_r_air";
                                }
                                else
                                {
                                    attackAnimation = "AHSS_shoot_l_air";
                                }
                            }
                            else if (leftGunHasBullet)
                            {
                                attackAnimation = "AHSS_shoot_l_air";
                            }
                            else if (rightGunHasBullet)
                            {
                                attackAnimation = "AHSS_shoot_r_air";
                            }
                            if (leftGunHasBullet || rightGunHasBullet)
                            {
                                flag3 = true;
                            }
                            else
                            {
                                flag4 = true;
                            }
                        }
                        if (flag3)
                        {
                            state = HeroState.Attack;
                            CrossFade(attackAnimation, 0.05f);
                            gunDummy.transform.position = baseTransform.position;
                            gunDummy.transform.rotation = baseTransform.rotation;
                            gunDummy.transform.LookAt(gunTarget);
                            attackReleased = false;
                            facingDirection = gunDummy.transform.rotation.eulerAngles.y;
                            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                        }
                        else if (flag4 && (grounded || (FengGameManagerMKII.Level.Mode != GameMode.TeamDeathmatch && RCSettings.AhssReload == 0)))
                        {
                            ChangeBlade();
                        }
                    }
                    break;
                case HeroState.Attack:
                    if (!useGun)
                    {
                        if (!inputManager.isInput[InputCode.Attack0])
                        {
                            buttonAttackRelease = true;
                        }
                        if (!attackReleased)
                        {
                            if (buttonAttackRelease)
                            {
                                ContinueAnimation();
                                attackReleased = true;
                            }
                            else if (baseAnimation[attackAnimation].normalizedTime >= 0.32f)
                            {
                                PauseAnimation();
                            }
                        }
                        if (attackAnimation == "attack3_1" && currentBladeSta > 0f)
                        {
                            TriggerColliderWeapon weaponColliderLeft = checkBoxLeft.GetComponent<TriggerColliderWeapon>();
                            TriggerColliderWeapon weaponColliderRight = checkBoxRight.GetComponent<TriggerColliderWeapon>();
                            if (baseAnimation[attackAnimation].normalizedTime >= 0.8f)
                            {
                                if (!weaponColliderLeft.active_me)
                                {
                                    weaponColliderLeft.active_me = true;
                                    if ((int)FengGameManagerMKII.Settings[92] == 0)
                                    {
                                        leftbladetrail2.Activate();
                                        rightbladetrail2.Activate();
                                        leftbladetrail.Activate();
                                        rightbladetrail.Activate();
                                    }
                                    baseRigidBody.velocity = -Vector3.up * 30f;
                                }
                                if (!weaponColliderRight.active_me)
                                {
                                    weaponColliderRight.active_me = true;
                                    slash.Play();
                                }
                            }
                            else if (weaponColliderLeft.active_me)
                            {
                                weaponColliderLeft.active_me = false;
                                weaponColliderRight.active_me = false;
                                weaponColliderLeft.ClearHits();
                                weaponColliderRight.ClearHits();
                                leftbladetrail.StopSmoothly(0.1f);
                                rightbladetrail.StopSmoothly(0.1f);
                                leftbladetrail2.StopSmoothly(0.1f);
                                rightbladetrail2.StopSmoothly(0.1f);
                            }
                        }
                        else
                        {
                            float num2;
                            float num;
                            if (currentBladeSta == 0f)
                            {
                                num2 = (num = -1f);
                            }
                            else
                            {
                                switch (attackAnimation)
                                {
                                    case "attack5":
                                        num2 = 0.35f;
                                        num = 0.5f;
                                        break;
                                    case "special_petra":
                                        num2 = 0.35f;
                                        num = 0.48f;
                                        break;
                                    case "special_armin":
                                        num2 = 0.25f;
                                        num = 0.35f;
                                        break;
                                    case "attack4":
                                        num2 = 0.6f;
                                        num = 0.9f;
                                        break;
                                    case "special_sasha":
                                        num2 = (num = -1f);
                                        break;
                                    default:
                                        num2 = 0.5f;
                                        num = 0.85f;
                                        break;
                                }
                            }
                            TriggerColliderWeapon weaponColliderLeft = checkBoxLeft.GetComponent<TriggerColliderWeapon>();
                            TriggerColliderWeapon weaponColliderRight = checkBoxRight.GetComponent<TriggerColliderWeapon>();
                            if (baseAnimation[attackAnimation].normalizedTime > num2 && baseAnimation[attackAnimation].normalizedTime < num)
                            {
                                if (!weaponColliderLeft.active_me)
                                {
                                    weaponColliderLeft.active_me = true;
                                    slash.Play();
                                    if ((int)FengGameManagerMKII.Settings[92] == 0)
                                    {
                                        leftbladetrail2.Activate();
                                        rightbladetrail2.Activate();
                                        leftbladetrail.Activate();
                                        rightbladetrail.Activate();
                                    }
                                }
                                if (!weaponColliderRight.active_me)
                                {
                                    weaponColliderRight.active_me = true;
                                }
                            }
                            else if (weaponColliderLeft.active_me)
                            {
                                weaponColliderLeft.active_me = false;
                                weaponColliderRight.active_me = false;
                                weaponColliderLeft.ClearHits();
                                weaponColliderRight.ClearHits();
                                leftbladetrail2.StopSmoothly(0.1f);
                                rightbladetrail2.StopSmoothly(0.1f);
                                leftbladetrail.StopSmoothly(0.1f);
                                rightbladetrail.StopSmoothly(0.1f);
                            }
                            if (attackLoop > 0 && baseAnimation[attackAnimation].normalizedTime > num)
                            {
                                attackLoop--;
                                PlayAnimationAt(attackAnimation, num2);
                            }
                        }
                        if (baseAnimation[attackAnimation].normalizedTime >= 1f)
                        {
                            switch (attackAnimation)
                            {
                                case "special_marco_0":
                                case "special_marco_1":
                                    if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
                                    {
                                        if (!PhotonNetwork.isMasterClient)
                                        {
                                            base.photonView.RPC("netTauntAttack", PhotonTargets.MasterClient, 5f, 100f);
                                        }
                                        else
                                        {
                                            netTauntAttack(5f);
                                        }
                                    }
                                    else
                                    {
                                        netTauntAttack(5f);
                                    }
                                    falseAttack();
                                    Idle();
                                    break;
                                case "special_armin":
                                    if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
                                    {
                                        if (!PhotonNetwork.isMasterClient)
                                        {
                                            base.photonView.RPC("netlaughAttack", PhotonTargets.MasterClient);
                                        }
                                        else
                                        {
                                            netlaughAttack();
                                        }
                                    }
                                    else
                                    {
                                        foreach (GameObject gameObject2 in GameObject.FindGameObjectsWithTag("titan"))
                                        {
                                            if (Vector3.Distance(gameObject2.transform.position, baseTransform.position) < 50f && Vector3.Angle(gameObject2.transform.forward, baseTransform.position - gameObject2.transform.position) < 90f && gameObject2.GetComponent<TITAN>() != null)
                                            {
                                                gameObject2.GetComponent<TITAN>().beLaughAttacked();
                                            }
                                        }
                                    }
                                    falseAttack();
                                    Idle();
                                    break;
                                case "attack3_1":
                                    baseRigidBody.velocity -= Vector3.up * Time.deltaTime * 30f;
                                    break;
                                default:
                                    falseAttack();
                                    Idle();
                                    break;
                            }
                        }
                        if (baseAnimation.IsPlaying("attack3_2") && baseAnimation["attack3_2"].normalizedTime >= 1f)
                        {
                            falseAttack();
                            Idle();
                        }
                    }
                    else
                    {
                        baseTransform.rotation = Quaternion.Lerp(baseTransform.rotation, gunDummy.transform.rotation, Time.deltaTime * 30f);
                        if (!attackReleased && baseAnimation[attackAnimation].normalizedTime > 0.167f)
                        {
                            attackReleased = true;
                            bool flag6 = false;
                            if (attackAnimation == "AHSS_shoot_both" || attackAnimation == "AHSS_shoot_both_air")
                            {
                                flag6 = true;
                                leftGunHasBullet = false;
                                rightGunHasBullet = false;
                                baseRigidBody.AddForce(-baseTransform.forward * 1000f, ForceMode.Acceleration);
                            }
                            else
                            {
                                if (attackAnimation == "AHSS_shoot_l" || attackAnimation == "AHSS_shoot_l_air")
                                {
                                    leftGunHasBullet = false;
                                }
                                else
                                {
                                    rightGunHasBullet = false;
                                }
                                baseRigidBody.AddForce(-baseTransform.forward * 600f, ForceMode.Acceleration);
                            }
                            baseRigidBody.AddForce(Vector3.up * 200f, ForceMode.Acceleration);
                            string text = !flag6 ? "FX/shotGun" : "FX/shotGun 1";
                            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
                            {
                                GameObject gameObject3 = PhotonNetwork.Instantiate(text, baseTransform.position + baseTransform.up * 0.8f - baseTransform.right * 0.1f, baseTransform.rotation, 0);
                                if (gameObject3.GetComponent<EnemyfxIDcontainer>() != null)
                                {
                                    gameObject3.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.photonView.viewID;
                                }
                            }
                            else
                            {
                                UnityEngine.Object.Instantiate(Resources.Load(text), baseTransform.position + baseTransform.up * 0.8f - baseTransform.right * 0.1f, baseTransform.rotation);
                            }
                        }

                        if (!baseAnimation.IsPlaying(attackAnimation) || baseAnimation[attackAnimation].normalizedTime >= 1f)
                        {
                            falseAttack();
                            Idle();
                        }
                    }
                    break;
                case HeroState.ChangeBlade:
                    if (useGun)
                    {
                        if (baseAnimation[reloadAnimation].normalizedTime > 0.22f)
                        {
                            if (!leftGunHasBullet && setup.part_blade_l.activeSelf)
                            {
                                setup.part_blade_l.SetActive(value: false);
                                Transform transform4 = setup.part_blade_l.transform;
                                GameObject gameObject4 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform4.position, transform4.rotation);
                                gameObject4.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
                                Vector3 force = -baseTransform.forward * 10f + baseTransform.up * 5f - baseTransform.right;
                                gameObject4.rigidbody.AddForce(force, ForceMode.Impulse);
                                Vector3 torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
                                gameObject4.rigidbody.AddTorque(torque, ForceMode.Acceleration);
                            }
                            if (!rightGunHasBullet && setup.part_blade_r.activeSelf)
                            {
                                setup.part_blade_r.SetActive(value: false);
                                Transform transform5 = setup.part_blade_r.transform;
                                GameObject gameObject5 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform5.position, transform5.rotation);
                                gameObject5.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
                                Vector3 force2 = -baseTransform.forward * 10f + baseTransform.up * 5f + baseTransform.right;
                                gameObject5.rigidbody.AddForce(force2, ForceMode.Impulse);
                                Vector3 torque2 = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300));
                                gameObject5.rigidbody.AddTorque(torque2, ForceMode.Acceleration);
                            }
                        }
                        if (baseAnimation[reloadAnimation].normalizedTime > 0.62f && !throwedBlades)
                        {
                            throwedBlades = true;
                            if (leftBulletLeft > 0 && !leftGunHasBullet)
                            {
                                leftBulletLeft--;
                                setup.part_blade_l.SetActive(value: true);
                                leftGunHasBullet = true;
                            }
                            if (rightBulletLeft > 0 && !rightGunHasBullet)
                            {
                                setup.part_blade_r.SetActive(value: true);
                                rightBulletLeft--;
                                rightGunHasBullet = true;
                            }
                            UpdateRightMagUI();
                            UpdateLeftMagUI();
                        }
                        if (baseAnimation[reloadAnimation].normalizedTime > 1f)
                        {
                            Idle();
                        }
                    }
                    else
                    {
                        if (!grounded)
                        {
                            if (base.animation[reloadAnimation].normalizedTime >= 0.2f && !throwedBlades)
                            {
                                throwedBlades = true;
                                if (setup.part_blade_l.activeSelf)
                                {
                                    ThrowBlades();
                                }
                            }
                            if (base.animation[reloadAnimation].normalizedTime >= 0.56f && currentBladeNum > 0)
                            {
                                setup.part_blade_l.SetActive(value: true);
                                setup.part_blade_r.SetActive(value: true);
                                currentBladeSta = totalBladeSta;
                            }
                        }
                        else
                        {
                            if (baseAnimation[reloadAnimation].normalizedTime >= 0.13f && !throwedBlades)
                            {
                                throwedBlades = true;
                                if (setup.part_blade_l.activeSelf)
                                {
                                    ThrowBlades();
                                }
                            }
                            if (baseAnimation[reloadAnimation].normalizedTime >= 0.37f && currentBladeNum > 0)
                            {
                                setup.part_blade_l.SetActive(value: true);
                                setup.part_blade_r.SetActive(value: true);
                                currentBladeSta = totalBladeSta;
                            }
                        }
                        if (baseAnimation[reloadAnimation].normalizedTime >= 1f)
                        {
                            Idle();
                        }
                    }
                    break;
                case HeroState.Salute:
                    if (baseAnimation["salute"].normalizedTime >= 1f)
                    {
                        Idle();
                    }
                    break;
                case HeroState.Dodging:
                    if (baseAnimation.IsPlaying("dodge"))
                    {
                        if ((!grounded && baseAnimation["dodge"].normalizedTime > 0.6f) || baseAnimation["dodge"].normalizedTime >= 1f)
                        {
                            Idle();
                        }
                    }
                    break;
                case HeroState.Land:
                    if (baseAnimation.IsPlaying("dash_land") && baseAnimation["dash_land"].normalizedTime >= 1f)
                    {
                        Idle();
                    }
                    break;
                case HeroState.FillGas:
                    if (baseAnimation.IsPlaying("supply") && baseAnimation["supply"].normalizedTime >= 1f)
                    {
                        currentBladeSta = totalBladeSta;
                        currentBladeNum = totalBladeNum;
                        currentGas = totalGas;
                        if (!useGun)
                        {
                            setup.part_blade_l.SetActive(value: true);
                            setup.part_blade_r.SetActive(value: true);
                        }
                        else
                        {
                            leftBulletLeft = (rightBulletLeft = bulletMAX);
                            leftGunHasBullet = (rightGunHasBullet = true);
                            setup.part_blade_l.SetActive(value: true);
                            setup.part_blade_r.SetActive(value: true);
                            UpdateRightMagUI();
                            UpdateLeftMagUI();
                        }
                        Idle();
                    }
                    break;
                case HeroState.Slide:
                    if (!grounded)
                    {
                        Idle();
                    }
                    break;
                case HeroState.Dashing:
                    if (dashTime > 0f)
                    {
                        dashTime -= Time.deltaTime;
                        if (currentSpeed > originVM)
                        {
                            baseRigidBody.AddForce(-baseRigidBody.velocity * Time.deltaTime * 1.7f, ForceMode.VelocityChange);
                        }
                    }
                    else
                    {
                        dashTime = 0f;
                        Idle();
                    }
                    break;
            }
            if (inputManager.isInput[InputCode.HookLeft] && ((!baseAnimation.IsPlaying("attack3_1") && !baseAnimation.IsPlaying("attack5") && !baseAnimation.IsPlaying("special_petra") && state != HeroState.Grabbed) || state == HeroState.Idle))
            {
                if (bulletLeft != null)
                {
                    QHold = true;
                }
                else
                {
                    Ray ray4 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    LayerMask mask7 = 1 << LayerMask.NameToLayer("Ground");
                    LayerMask mask8 = 1 << LayerMask.NameToLayer("EnemyBox");
                    if (Physics.Raycast(ray4, out RaycastHit hitInfo4, 10000f, ((LayerMask)((int)mask8 | (int)mask7)).value))
                    {
                        LaunchLeftHook(hitInfo4, single: true);
                        rope.Play();
                    }
                }
            }
            else
            {
                QHold = false;
            }
            if (inputManager.isInput[InputCode.HookRight] && ((!baseAnimation.IsPlaying("attack3_1") && !baseAnimation.IsPlaying("attack5") && !baseAnimation.IsPlaying("special_petra") && state != HeroState.Grabbed) || state == HeroState.Idle))
            {
                if (bulletRight != null)
                {
                    EHold = true;
                }
                else
                {
                    Ray ray5 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    LayerMask mask9 = 1 << LayerMask.NameToLayer("Ground");
                    LayerMask mask10 = 1 << LayerMask.NameToLayer("EnemyBox");
                    if (Physics.Raycast(ray5, out RaycastHit hitInfo5, 10000f, ((LayerMask)((int)mask10 | (int)mask9)).value))
                    {
                        LaunchRightHook(hitInfo5, single: true);
                        rope.Play();
                    }
                }
            }
            else
            {
                EHold = false;
            }
            if (inputManager.isInput[InputCode.HookBoth] && ((!baseAnimation.IsPlaying("attack3_1") && !baseAnimation.IsPlaying("attack5") && !baseAnimation.IsPlaying("special_petra") && state != HeroState.Grabbed) || state == HeroState.Idle))
            {
                QHold = true;
                EHold = true;
                if (bulletLeft == null && bulletRight == null)
                {
                    Ray ray6 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    LayerMask mask11 = 1 << LayerMask.NameToLayer("Ground");
                    LayerMask mask12 = 1 << LayerMask.NameToLayer("EnemyBox");
                    if (Physics.Raycast(ray6, out RaycastHit hitInfo6, 1000000f, ((LayerMask)((int)mask12 | (int)mask11)).value))
                    {
                        LaunchLeftHook(hitInfo6, single: false);
                        LaunchRightHook(hitInfo6, single: false);
                        rope.Play();
                    }
                }
            }
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer || (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer && !IN_GAME_MAIN_CAMERA.IsPausing))
            {
                TickSkillCooldown();
                TickFlareCooldown();
            }
            if (!useGun)
            {
                if (leftbladetrail.gameObject.GetActive())
                {
                    leftbladetrail.update();
                    rightbladetrail.update();
                }
                if (leftbladetrail2.gameObject.GetActive())
                {
                    leftbladetrail2.update();
                    rightbladetrail2.update();
                }
                if (leftbladetrail.gameObject.GetActive())
                {
                    leftbladetrail.lateUpdate();
                    rightbladetrail.lateUpdate();
                }
                if (leftbladetrail2.gameObject.GetActive())
                {
                    leftbladetrail2.lateUpdate();
                    rightbladetrail2.lateUpdate();
                }
            }
            if (!IN_GAME_MAIN_CAMERA.IsPausing)
            {
                ShowSkillCountDown();
                UpdateFlareCooldown();
                UpdateResourceUI();
                UpdateCrossHair();
            }
        }
        else if (isCannon && !IN_GAME_MAIN_CAMERA.IsPausing)
        {
            UpdateCrossHair();
            TickSkillCooldown();
            ShowSkillCountDown();
        }
    }

    public void SetStat2()
    {
        skillCDLast = 1.5f;
        skillId = setup.myCostume.stat.SkillId;
        CustomAnimationSpeed();

        switch (skillId)
        {
            case "levi":
                skillCDLast = 3.5f;
                break;
            case "armin":
                skillCDLast = 5f;
                break;
            case "marco":
                skillCDLast = 10f;
                break;
            case "jean":
                skillCDLast = 0.001f;
                break;
            case "eren":
                skillCDLast = 120f;

                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
                {
                    if (FengGameManagerMKII.Level.PlayerTitans || FengGameManagerMKII.Level.Mode == GameMode.Racing
                        || FengGameManagerMKII.Level.Mode == GameMode.PvPCapture || FengGameManagerMKII.Level.Mode == GameMode.Trost
                        || RCSettings.BanEren > 0)
                    {
                        skillId = "petra";
                        skillCDLast = 1f;
                    }
                    else
                    {
                        foreach (PhotonPlayer photonPlayer in PhotonNetwork.otherPlayers)
                        {
                            if (GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.IsTitan]) == 1
                                && GExtensions.AsString(photonPlayer.customProperties[PhotonPlayerProperty.Character]).ToUpper() == "EREN")
                            {
                                skillId = "petra";
                                skillCDLast = 1f;
                                break;
                            }
                        }
                    }
                }
                break;
            case "sasha":
                skillCDLast = 20f;
                break;
            case "petra":
                skillCDLast = 3.5f;
                break;
        }
        bombInit();
        speed = (float)setup.myCostume.stat.Speed / 10f;
        totalGas = (currentGas = setup.myCostume.stat.Gas);
        totalBladeSta = (currentBladeSta = setup.myCostume.stat.Blade);
        baseRigidBody.mass = 0.5f - (float)(setup.myCostume.stat.Accel - 100) * 0.001f;

        GameObject skillCdBottom = GameObject.Find("skill_cd_bottom");
        skillCdBottom.transform.localPosition = new Vector3(0f, (float)(-Screen.height) * 0.5f + 5f, 0f);
        skillCD = GameObject.Find("skill_cd_" + skillIDHUD);
        skillCD.transform.localPosition = skillCdBottom.transform.localPosition;
        GameObject.Find("GasUI").transform.localPosition = skillCdBottom.transform.localPosition;
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine)
        {
            GameObject.Find("bulletL").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = false;
        }
        if (setup.myCostume.uniform_type == UNIFORM_TYPE.CasualAHSS)
        {
            standAnimation = "AHSS_stand_gun";
            useGun = true;
            gunDummy = new GameObject();
            gunDummy.name = "gunDummy";
            gunDummy.transform.position = baseTransform.position;
            gunDummy.transform.rotation = baseTransform.rotation;
            myGroup = GroupType.AHSS;
            SetTeam2(2);
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine)
            {
                GameObject.Find("bladeCL").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladeCR").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel1").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader1").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel2").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader2").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel3").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader3").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel4").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader4").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel5").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader5").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bulletL").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = true;
                if (skillId != "bomb")
                {
                    skillCD.transform.localPosition = Vector3.up * 5000f;
                }
            }
        }
        else if (setup.myCostume.sex == Sex.Female)
        {
            standAnimation = "stand";
            SetTeam2(1);
        }
        else
        {
            standAnimation = "stand_levi";
            SetTeam2(1);
        }
    }

    private void Suicide()
    {
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
        {
            NetDieLocal2(base.rigidbody.velocity * 50f, isBite: false, -1, Guardian.Mod.Properties.SuicideMessage.Value);

            FengGameManagerMKII.Instance.needChooseSide = true;
            FengGameManagerMKII.Instance.justSuicide = true;
        }
    }

    public void NetDieLocal2(Vector3 v, bool isBite, int viewId = -1, string titanName = "", bool killByTitan = true)
    {
        Vector3 localPosition = Vector3.up * 5000f;
        if (titanForm && eren_titan != null)
        {
            eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
        }
        if (myBomb != null)
        {
            myBomb.DestroyMe();
        }
        if (myCannon != null)
        {
            PhotonNetwork.Destroy(myCannon);
        }
        if (skillCD != null)
        {
            skillCD.transform.localPosition = localPosition;
        }

        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().RemoveMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().RemoveMe();
        }

        meatDie.Play();

        if (!useGun && (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine))
        {
            leftbladetrail.Deactivate();
            rightbladetrail.Deactivate();
            leftbladetrail2.Deactivate();
            rightbladetrail2.Deactivate();
        }
        falseAttack();
        BreakApart2(v, isBite);

        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(val: false);
        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        FengGameManagerMKII.Instance.myRespawnTime = 0f;

        hasDied = true;
        Transform transform = base.transform.Find("audio_die");
        transform.parent = null;
        transform.GetComponent<AudioSource>().Play();
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;

        PhotonNetwork.RemoveRPCs(base.photonView);
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add(PhotonPlayerProperty.Dead, true);
        hashtable.Add(PhotonPlayerProperty.Deaths, GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Deaths]) + 1);
        PhotonNetwork.player.SetCustomProperties(hashtable);
        FengGameManagerMKII.Instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, 0);

        FengGameManagerMKII.Instance.SendKillInfo(false, titanName, false, GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]));

        PhotonNetwork.Destroy(base.photonView);

        if (PhotonNetwork.isMasterClient)
        {
            OnDeathEvent(viewId, killByTitan);

            // TODO: Mod
            Guardian.Mod.Gamemodes.Current.OnPlayerKilled(this, viewId, killByTitan);

            if (FengGameManagerMKII.HeroHash.ContainsKey(base.photonView.owner.Id))
            {
                FengGameManagerMKII.HeroHash.Remove(base.photonView.owner.Id);
            }
        }
    }

    private void Dodge2(bool offTheWall = false)
    {
        if (FengGameManagerMKII.InputRC.isInputHorse(InputCodeRC.HorseMount) && myHorse != null && !isMounted && Vector3.Distance(myHorse.transform.position, base.transform.position) < 15f)
        {
            return;
        }
        state = HeroState.Dodging;
        if (!offTheWall)
        {
            float num = inputManager.isInput[InputCode.Up] ? 1f : ((!inputManager.isInput[InputCode.Down]) ? 0f : (-1f));
            float num2 = inputManager.isInput[InputCode.Left] ? (-1f) : ((!inputManager.isInput[InputCode.Right]) ? 0f : 1f);
            float globalFacingDirection = GetGlobalFacingDirection(num2, num);
            if (num2 != 0f || num != 0f)
            {
                facingDirection = globalFacingDirection + 180f;
                targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
            }
            CrossFade("dodge", 0.1f);
        }
        else
        {
            PlayAnimation("dodge");
            PlayAnimationAt("dodge", 0.2f);
        }
        sparks.enableEmission = false;
    }

    private void BreakApart2(Vector3 v, bool isBite)
    {
        GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
        gameObject.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
        gameObject.GetComponent<HERO_SETUP>().isDeadBody = true;
        gameObject.GetComponent<HERO_DEAD_BODY_SETUP>().Init(currentAnimation, base.animation[currentAnimation].normalizedTime, BODY_PARTS.ARM_R);
        if (!isBite)
        {
            GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            GameObject gameObject4 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            gameObject2.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            gameObject3.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            gameObject4.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            gameObject2.GetComponent<HERO_SETUP>().isDeadBody = true;
            gameObject3.GetComponent<HERO_SETUP>().isDeadBody = true;
            gameObject4.GetComponent<HERO_SETUP>().isDeadBody = true;
            gameObject2.GetComponent<HERO_DEAD_BODY_SETUP>().Init(currentAnimation, base.animation[currentAnimation].normalizedTime, BODY_PARTS.UPPER);
            gameObject3.GetComponent<HERO_DEAD_BODY_SETUP>().Init(currentAnimation, base.animation[currentAnimation].normalizedTime, BODY_PARTS.LOWER);
            gameObject4.GetComponent<HERO_DEAD_BODY_SETUP>().Init(currentAnimation, base.animation[currentAnimation].normalizedTime, BODY_PARTS.ARM_L);
            ApplyForce(gameObject2, v);
            ApplyForce(gameObject3, v);
            ApplyForce(gameObject4, v);
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine)
            {
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(gameObject2, resetRotation: false);
            }
        }
        else if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(gameObject, resetRotation: false);
        }
        ApplyForce(gameObject, v);
        Transform transform = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L").transform;
        Transform transform2 = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R").transform;
        GameObject gameObject5;
        GameObject gameObject6;
        GameObject gameObject7;
        GameObject gameObject8;
        GameObject gameObject9;
        if (useGun)
        {
            gameObject5 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
            gameObject6 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform2.position, transform2.rotation);
            gameObject7 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_2"), base.transform.position, base.transform.rotation);
            gameObject8 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_l"), base.transform.position, base.transform.rotation);
            gameObject9 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_r"), base.transform.position, base.transform.rotation);
        }
        else
        {
            gameObject5 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
            gameObject6 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
            gameObject7 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg"), base.transform.position, base.transform.rotation);
            gameObject8 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_l"), base.transform.position, base.transform.rotation);
            gameObject9 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_r"), base.transform.position, base.transform.rotation);
        }
        gameObject5.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        gameObject6.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        gameObject7.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        gameObject8.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        gameObject9.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        ApplyForce(gameObject5, v);
        ApplyForce(gameObject6, v);
        ApplyForce(gameObject7, v);
        ApplyForce(gameObject8, v);
        ApplyForce(gameObject9, v);
    }

    [RPC]
    public void netDie(Vector3 v, bool isBite, int viewId = -1, string titanName = "", bool killByTitan = true, PhotonMessageInfo info = null)
    {
        if (base.photonView.isMine && info != null && FengGameManagerMKII.Level.Mode != GameMode.Colossal)
        {
            if (FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                base.photonView.RPC("backToHumanRPC", PhotonTargets.Others);
                return;
            }
            if (!info.sender.isLocal && !info.sender.isMasterClient)
            {
                if (info.sender.customProperties[PhotonPlayerProperty.Name] == null || info.sender.customProperties[PhotonPlayerProperty.IsTitan] == null)
                {
                    InRoomChat.Instance.AddLine("Unusual Kill from ID ".AsColor("FFCC00") + info.sender.Id.ToString());
                    return;
                }
                else if (viewId < 0)
                {
                    if (titanName == string.Empty)
                    {
                        InRoomChat.Instance.AddLine("Unusual Kill from ID ".AsColor("FFCC00") + info.sender.Id.ToString() + " (possibly valid).");
                        return;
                    }
                    else
                    {
                        InRoomChat.Instance.AddLine("Unusual Kill from ID ".AsColor("FFCC00") + info.sender.Id.ToString());
                        return;
                    }
                }
                else if (PhotonView.Find(viewId) == null)
                {
                    InRoomChat.Instance.AddLine("Unusual Kill from ID ".AsColor("FFCC00") + info.sender.Id.ToString());
                    return;
                }
                else
                {
                    PhotonView photonView = PhotonView.Find(viewId);
                    if (photonView.owner.Id != info.sender.Id)
                    {
                        InRoomChat.Instance.AddLine("Unusual Kill from ID ".AsColor("FFCC00") + info.sender.Id.ToString());
                        return;
                    }
                }
            }
        }
        if (PhotonNetwork.isMasterClient)
        {
            OnDeathEvent(viewId, killByTitan);

            // TODO: Mod
            Guardian.Mod.Gamemodes.Current.OnPlayerKilled(this, viewId, killByTitan);

            int id = base.photonView.owner.Id;
            if (FengGameManagerMKII.HeroHash.ContainsKey(id))
            {
                FengGameManagerMKII.HeroHash.Remove(id);
            }
        }
        if (base.photonView.isMine)
        {
            Vector3 localPosition = Vector3.up * 5000f;
            if (myBomb != null)
            {
                myBomb.DestroyMe();
            }
            if (myCannon != null)
            {
                PhotonNetwork.Destroy(myCannon);
            }
            if (titanForm && eren_titan != null)
            {
                eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (skillCD != null)
            {
                skillCD.transform.localPosition = localPosition;
            }
        }
        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().RemoveMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().RemoveMe();
        }
        meatDie.Play();
        if (!useGun && (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine))
        {
            leftbladetrail.Deactivate();
            rightbladetrail.Deactivate();
            leftbladetrail2.Deactivate();
            rightbladetrail2.Deactivate();
        }
        falseAttack();
        BreakApart2(v, isBite);
        if (base.photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(val: false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.Instance.myRespawnTime = 0f;
        }
        hasDied = true;
        Transform transform = base.transform.Find("audio_die");
        if (transform != null)
        {
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
        }
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        if (base.photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(base.photonView);
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.Dead, true);
            hashtable.Add(PhotonPlayerProperty.Deaths, GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(hashtable);
            FengGameManagerMKII.Instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, (!(titanName == string.Empty)) ? 1 : 0);
            if (viewId != -1)
            {
                PhotonView photonView2 = PhotonView.Find(viewId);
                if (photonView2 != null)
                {
                    FengGameManagerMKII.Instance.SendKillInfo(killByTitan, "[FFCC00][" + info.sender.Id.ToString() + "][FFFFFF] " + GExtensions.AsString(photonView2.owner.customProperties[PhotonPlayerProperty.Name]), isVictimTitan: false, GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]));
                    hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.Kills, GExtensions.AsInt(photonView2.owner.customProperties[PhotonPlayerProperty.Kills]) + 1);
                    photonView2.owner.SetCustomProperties(hashtable);
                }
            }
            else
            {
                FengGameManagerMKII.Instance.SendKillInfo(titanName.Length > 0, "[FFCC00][" + info.sender.Id.ToString() + "][FFFFFF] " + titanName, isVictimTitan: false, GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]));
            }
        }
        if (base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.photonView);
        }
    }

    [RPC]
    private void netDie2(int viewId = -1, string titanName = "", PhotonMessageInfo info = null)
    {
        if (base.photonView.isMine && info != null && FengGameManagerMKII.Level.Mode != GameMode.Colossal)
        {
            if (FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                base.photonView.RPC("backToHumanRPC", PhotonTargets.Others);
                return;
            }
            if (!info.sender.isLocal && !info.sender.isMasterClient)
            {
                if (info.sender.customProperties[PhotonPlayerProperty.Name] == null || info.sender.customProperties[PhotonPlayerProperty.IsTitan] == null)
                {
                    InRoomChat.Instance.AddLine("Unusual Kill from ID ".AsColor("FFCC00") + info.sender.Id.ToString());
                }
                else if (viewId < 0)
                {
                    if (titanName == string.Empty)
                    {
                        InRoomChat.Instance.AddLine("Unusual Kill from ID ".AsColor("FFCC00") + info.sender.Id.ToString() + " (possibly valid).");
                    }
                    else if (RCSettings.BombMode == 0 && RCSettings.DeadlyCannons == 0)
                    {
                        InRoomChat.Instance.AddLine("Unusual Kill from ID ".AsColor("FFCC00") + info.sender.Id.ToString());
                    }
                }
                else if (PhotonView.Find(viewId) == null)
                {
                    InRoomChat.Instance.AddLine("Unusual Kill from ID ".AsColor("FFCC00") + info.sender.Id.ToString());
                }
                else
                {
                    PhotonView photonView = PhotonView.Find(viewId);
                    if (photonView.owner.Id != info.sender.Id)
                    {
                        InRoomChat.Instance.AddLine("Unusual Kill from ID ".AsColor("FFCC00") + info.sender.Id.ToString());
                    }
                }
            }
        }
        if (base.photonView.isMine)
        {
            Vector3 localPosition = Vector3.up * 5000f;
            if (myBomb != null)
            {
                myBomb.DestroyMe();
            }
            if (myCannon != null)
            {
                PhotonNetwork.Destroy(myCannon);
            }
            PhotonNetwork.RemoveRPCs(base.photonView);
            if (titanForm && eren_titan != null)
            {
                eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (skillCD != null)
            {
                skillCD.transform.localPosition = localPosition;
            }
        }
        meatDie.Play();
        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().RemoveMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().RemoveMe();
        }
        Transform transform = base.transform.Find("audio_die");
        transform.parent = null;
        transform.GetComponent<AudioSource>().Play();
        if (base.photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(val: true);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.Instance.myRespawnTime = 0f;
        }
        falseAttack();
        hasDied = true;
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(base.photonView);
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.Dead, true);
            hashtable.Add(PhotonPlayerProperty.Deaths, (int)PhotonNetwork.player.customProperties[PhotonPlayerProperty.Deaths] + 1);
            PhotonNetwork.player.SetCustomProperties(hashtable);
            if (viewId != -1)
            {
                PhotonView photonView2 = PhotonView.Find(viewId);
                if (photonView2 != null)
                {
                    FengGameManagerMKII.Instance.SendKillInfo(isKillerTitan: true, "[FFCC00][" + info.sender.Id + "] [-]" + GExtensions.AsString(photonView2.owner.customProperties[PhotonPlayerProperty.Name]), isVictimTitan: false, GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]));
                    hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.Kills, GExtensions.AsInt(photonView2.owner.customProperties[PhotonPlayerProperty.Kills]) + 1);
                    photonView2.owner.SetCustomProperties(hashtable);
                }
            }
            else
            {
                FengGameManagerMKII.Instance.SendKillInfo(isKillerTitan: true, "[FFCC00][" + info.sender.Id + "] [-]" + titanName, isVictimTitan: false, GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]));
            }
            FengGameManagerMKII.Instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, (!(titanName == string.Empty)) ? 1 : 0);
        }
        GameObject gameObject = (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || !base.photonView.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"))) : PhotonNetwork.Instantiate("hitMeat2", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        gameObject.transform.position = base.transform.position;
        if (base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.photonView);
        }
        if (PhotonNetwork.isMasterClient)
        {
            OnDeathEvent(viewId, isTitan: true);

            // TODO: Mod
            Guardian.Mod.Gamemodes.Current.OnPlayerKilled(this, viewId, true);

            int id = base.photonView.owner.Id;
            if (FengGameManagerMKII.HeroHash.ContainsKey(id))
            {
                FengGameManagerMKII.HeroHash.Remove(id);
            }
        }
    }

    [RPC]
    public void SetMyPhotonCamera(float offset, PhotonMessageInfo info)
    {
        if (base.photonView.owner == info.sender)
        {
            CameraMultiplier = offset;
            GetComponent<SmoothSyncMovement>().PhotonCamera = true;
            isPhotonCamera = true;
        }
    }

    [RPC]
    public void SetMyCannon(int viewID, PhotonMessageInfo info)
    {
        if (info.sender != base.photonView.owner)
        {
            return;
        }
        if (!Guardian.AntiAbuse.Validators.Hero.IsCannonSetValid(viewID, info))
        {
            return;
        }
        PhotonView photonView = PhotonView.Find(viewID);
        if (photonView != null)
        {
            myCannon = photonView.gameObject;
            if (myCannon != null)
            {
                myCannonBase = myCannon.transform;
                myCannonPlayer = myCannonBase.Find("PlayerPoint");
                isCannon = true;
            }
        }
    }

    [RPC]
    public void ReturnFromCannon(PhotonMessageInfo info)
    {
        if (info.sender == base.photonView.owner)
        {
            isCannon = false;
            base.gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
        }
    }

    public void ClearPopup()
    {
        FengGameManagerMKII.Instance.SetTextCenter(string.Empty);
    }

    [RPC]
    public void SpawnCannonRPC(string settings, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient && base.photonView.isMine && myCannon == null)
        {
            if (myHorse != null && isMounted)
            {
                GetOffHorse();
            }
            Idle();
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().RemoveMe();
            }
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().RemoveMe();
            }
            if (smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && base.photonView.isMine)
            {
                base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, false);
            }
            smoke_3dmg.enableEmission = false;
            base.rigidbody.velocity = Vector3.zero;
            string[] array = settings.Split(',');
            if (array.Length > 15)
            {
                myCannon = PhotonNetwork.Instantiate("RCAsset/" + array[1], new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])), 0);
            }
            else
            {
                myCannon = PhotonNetwork.Instantiate("RCAsset/" + array[1], new Vector3(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]), Convert.ToSingle(array[4])), new Quaternion(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8])), 0);
            }
            myCannonBase = myCannon.transform;
            myCannonPlayer = myCannon.transform.Find("PlayerPoint");
            isCannon = true;
            myCannon.GetComponent<Cannon>().myHero = this;
            myCannonRegion = null;
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(myCannon.transform.Find("Barrel").Find("FiringPoint").gameObject);
            Camera.main.fieldOfView = 55f;
            base.photonView.RPC("SetMyCannon", PhotonTargets.OthersBuffered, myCannon.GetPhotonView().viewID);
            skillCDLastCannon = skillCDLast;
            skillCDLast = 3.5f;
            skillCDDuration = 3.5f;
        }
    }

    public void updateCannon()
    {
        baseTransform.position = myCannonPlayer.position;
        baseTransform.rotation = myCannonBase.rotation;
    }

    public IEnumerator CoStopImmunity()
    {
        yield return new WaitForSeconds(5f);
        bombImmune = false;
    }

    public void OnDeathEvent(int viewId, bool isTitan)
    {
        if (isTitan)
        {
            if (FengGameManagerMKII.RCEvents.ContainsKey("OnPlayerDieByTitan"))
            {
                RCEvent rCEvent = (RCEvent)FengGameManagerMKII.RCEvents["OnPlayerDieByTitan"];
                string[] array = (string[])FengGameManagerMKII.RCVariableNames["OnPlayerDieByTitan"];
                if (FengGameManagerMKII.PlayerVariables.ContainsKey(array[0]))
                {
                    FengGameManagerMKII.PlayerVariables[array[0]] = base.photonView.owner;
                }
                else
                {
                    FengGameManagerMKII.PlayerVariables.Add(array[0], base.photonView.owner);
                }
                if (FengGameManagerMKII.TitanVariables.ContainsKey(array[1]))
                {
                    FengGameManagerMKII.TitanVariables[array[1]] = PhotonView.Find(viewId).gameObject.GetComponent<TITAN>();
                }
                else
                {
                    FengGameManagerMKII.TitanVariables.Add(array[1], PhotonView.Find(viewId).gameObject.GetComponent<TITAN>());
                }
                rCEvent.CheckEvent();
            }
        }
        else if (FengGameManagerMKII.RCEvents.ContainsKey("OnPlayerDieByPlayer"))
        {
            RCEvent rcEvent = (RCEvent)FengGameManagerMKII.RCEvents["OnPlayerDieByPlayer"];
            string[] array = (string[])FengGameManagerMKII.RCVariableNames["OnPlayerDieByPlayer"];
            if (FengGameManagerMKII.PlayerVariables.ContainsKey(array[0]))
            {
                FengGameManagerMKII.PlayerVariables[array[0]] = base.photonView.owner;
            }
            else
            {
                FengGameManagerMKII.PlayerVariables.Add(array[0], base.photonView.owner);
            }
            if (FengGameManagerMKII.PlayerVariables.ContainsKey(array[1]))
            {
                FengGameManagerMKII.PlayerVariables[array[1]] = PhotonView.Find(viewId).owner;
            }
            else
            {
                FengGameManagerMKII.PlayerVariables.Add(array[1], PhotonView.Find(viewId).owner);
            }
            rcEvent.CheckEvent();
        }
    }

    [RPC]
    public void moveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            base.transform.position = new Vector3(posX, posY, posZ);
        }
    }

    public void SetTeam2(int team)
    {
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
        {
            base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, team);
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.Team, team);
            PhotonNetwork.player.SetCustomProperties(hashtable);
        }
        else
        {
            setMyTeam(team);
        }
    }

    public void Cache()
    {
        baseTransform = base.transform;
        baseRigidBody = base.rigidbody;
        maincamera = GameObject.Find("MainCamera");
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && !base.photonView.isMine)
        {
            return;
        }
        baseAnimation = base.animation;

        cross1 = GameObject.Find("cross1");
        cross2 = GameObject.Find("cross2");

        crossL1 = GameObject.Find("crossL1");
        crossL2 = GameObject.Find("crossL2");
        crossR1 = GameObject.Find("crossR1");
        crossR2 = GameObject.Find("crossR2");
        LabelDistance = GameObject.Find("LabelDistance");
        cachedSprites = new Dictionary<string, UISprite>();

        foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (gameObject.GetComponent<UISprite>() != null && gameObject.activeInHierarchy)
            {
                string name = gameObject.name;
                if ((name.Contains("blade") || name.Contains("bullet") || name.Contains("gas") || name.Contains("flare") || name.Contains("skill_cd")) && !cachedSprites.ContainsKey(name))
                {
                    cachedSprites.Add(name, gameObject.GetComponent<UISprite>());
                }
            }
        }
    }

    public void CheckTitan()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = 1 << LayerMask.NameToLayer("PlayerAttackBox");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask3 = 1 << LayerMask.NameToLayer("EnemyBox");
        RaycastHit[] array = Physics.RaycastAll(ray, 180f, ((LayerMask)((int)mask | (int)mask2 | (int)mask3)).value);
        List<RaycastHit> list = new List<RaycastHit>();
        List<TITAN> titans = new List<TITAN>();
        foreach (RaycastHit item in array)
        {
            list.Add(item);
        }
        list.Sort((RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
        float num = 180f;
        for (int i = 0; i < list.Count; i++)
        {
            GameObject gameObject = list[i].collider.gameObject;
            if (gameObject.layer == 16)
            {
                if (gameObject.name.Contains("PlayerDetectorRC") && list[i].distance < num)
                {
                    num -= 60f;
                    if (num <= 60f)
                    {
                        i = list.Count;
                    }
                    TITAN component = gameObject.transform.root.gameObject.GetComponent<TITAN>();
                    if (component != null)
                    {
                        titans.Add(component);
                    }
                }
            }
            else
            {
                i = list.Count;
            }
        }
        for (int i = 0; i < myTitans.Count; i++)
        {
            TITAN tITAN = myTitans[i];
            if (!titans.Contains(tITAN))
            {
                tITAN.isLook = false;
            }
        }
        for (int i = 0; i < titans.Count; i++)
        {
            TITAN tITAN2 = titans[i];
            tITAN2.isLook = true;
        }
        myTitans = titans;
    }

    private void CheckDashRebind()
    {
        if (FengGameManagerMKII.InputRC.isInputHuman(InputCodeRC.Dash))
        {
            if (inputManager.isInput[InputCode.Up])
            {
                dashU = true;
            }
            else if (inputManager.isInput[InputCode.Down])
            {
                dashD = true;
            }
            else if (inputManager.isInput[InputCode.Left])
            {
                dashL = true;
            }
            else if (inputManager.isInput[InputCode.Right])
            {
                dashR = true;
            }
        }
    }

    public void ExtendedUpdate()
    {
        if (!(skillId == "bomb"))
        {
            return;
        }
        if (inputManager.isInputDown[InputCode.Attack1] && skillCDDuration <= 0f)
        {
            if (myBomb != null && !myBomb.disabled)
            {
                myBomb.Explode(bombRadius);
            }
            detonate = false;
            skillCDDuration = bombCD;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
            LayerMask layerMask = (int)mask2 | (int)mask;
            currentV = baseTransform.position;
            targetV = currentV + Vector3.forward * 200f;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000000f, layerMask.value))
            {
                targetV = hitInfo.point;
            }
            Vector3 a = Vector3.Normalize(targetV - currentV);
            GameObject gameObject = PhotonNetwork.Instantiate("RCAsset/BombMain", currentV + a * 4f, new Quaternion(0f, 0f, 0f, 1f), 0);
            gameObject.rigidbody.velocity = a * bombSpeed;
            myBomb = gameObject.GetComponent<Bomb>();
            bombTime = 0f;
        }
        else if (myBomb != null && !myBomb.disabled)
        {
            bombTime += Time.deltaTime;
            bool flag = false;
            if (inputManager.isInputUp[InputCode.Attack1])
            {
                detonate = true;
            }
            else if (inputManager.isInputDown[InputCode.Attack1] && detonate)
            {
                detonate = false;
                flag = true;
            }
            if (bombTime >= bombTimeMax)
            {
                flag = true;
            }
            if (flag)
            {
                myBomb.Explode(bombRadius);
                detonate = false;
            }
        }
    }

    public void bombInit()
    {
        skillIDHUD = skillId;
        skillCDDuration = skillCDLast;
        if (RCSettings.BombMode == 1)
        {
            int num = (int)FengGameManagerMKII.Settings[250];
            int num2 = (int)FengGameManagerMKII.Settings[251];
            int num3 = (int)FengGameManagerMKII.Settings[252];
            int num4 = (int)FengGameManagerMKII.Settings[253];
            if (num < 0 || num > 10)
            {
                num = 5;
                FengGameManagerMKII.Settings[250] = 5;
            }
            if (num2 < 0 || num2 > 10)
            {
                num2 = 5;
                FengGameManagerMKII.Settings[251] = 5;
            }
            if (num3 < 0 || num3 > 10)
            {
                num3 = 5;
                FengGameManagerMKII.Settings[252] = 5;
            }
            if (num4 < 0 || num4 > 10)
            {
                num4 = 5;
                FengGameManagerMKII.Settings[253] = 5;
            }
            if (num + num2 + num3 + num4 > 20)
            {
                num = 5;
                num2 = 5;
                num3 = 5;
                num4 = 5;
                FengGameManagerMKII.Settings[250] = 5;
                FengGameManagerMKII.Settings[251] = 5;
                FengGameManagerMKII.Settings[252] = 5;
                FengGameManagerMKII.Settings[253] = 5;
            }
            bombTimeMax = ((float)num2 * 60f + 200f) / ((float)num3 * 60f + 200f);
            bombRadius = (float)num * 4f + 20f;
            bombCD = (float)num4 * -0.4f + 5f;
            bombSpeed = (float)num3 * 60f + 200f;
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.RCBombR, (float)FengGameManagerMKII.Settings[246]);
            hashtable.Add(PhotonPlayerProperty.RCBombG, (float)FengGameManagerMKII.Settings[247]);
            hashtable.Add(PhotonPlayerProperty.RCBombB, (float)FengGameManagerMKII.Settings[248]);
            hashtable.Add(PhotonPlayerProperty.RCBombA, (float)FengGameManagerMKII.Settings[249]);
            hashtable.Add(PhotonPlayerProperty.RCBombRadius, bombRadius);
            PhotonNetwork.player.SetCustomProperties(hashtable);
            skillId = "bomb";
            skillIDHUD = "armin";
            skillCDLast = bombCD;
            skillCDDuration = 10f;
            if (FengGameManagerMKII.Instance.roundTime > 10f)
            {
                skillCDDuration = 5f;
            }
        }
    }

    public void LoadSkin()
    {
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && !base.photonView.isMine)
        {
            return;
        }
        if ((int)FengGameManagerMKII.Settings[0] != 1)
        {
            return;
        }

        int num = 14;
        int num2 = 4;
        int num3 = 5;
        int num4 = 6;
        int num5 = 7;
        int num6 = 8;
        int num7 = 9;
        int num8 = 10;
        int num9 = 11;
        int num10 = 12;
        int num11 = 13;
        int num12 = 3;
        int num13 = 94;
        if ((int)FengGameManagerMKII.Settings[133] == 1)
        {
            num12 = 134;
            num2 = 135;
            num3 = 136;
            num4 = 137;
            num5 = 138;
            num6 = 139;
            num7 = 140;
            num8 = 141;
            num9 = 142;
            num10 = 143;
            num11 = 144;
            num = 145;
            num13 = 146;
        }
        else if ((int)FengGameManagerMKII.Settings[133] == 2)
        {
            num12 = 147;
            num2 = 148;
            num3 = 149;
            num4 = 150;
            num5 = 151;
            num6 = 152;
            num7 = 153;
            num8 = 154;
            num9 = 155;
            num10 = 156;
            num11 = 157;
            num = 158;
            num13 = 159;
        }
        string text = (string)FengGameManagerMKII.Settings[num];
        string text2 = (string)FengGameManagerMKII.Settings[num2];
        string text3 = (string)FengGameManagerMKII.Settings[num3];
        string text4 = (string)FengGameManagerMKII.Settings[num4];
        string text5 = (string)FengGameManagerMKII.Settings[num5];
        string text6 = (string)FengGameManagerMKII.Settings[num6];
        string text7 = (string)FengGameManagerMKII.Settings[num7];
        string text8 = (string)FengGameManagerMKII.Settings[num8];
        string text9 = (string)FengGameManagerMKII.Settings[num9];
        string text10 = (string)FengGameManagerMKII.Settings[num10];
        string text11 = (string)FengGameManagerMKII.Settings[num11];
        string text12 = (string)FengGameManagerMKII.Settings[num12];
        string text13 = (string)FengGameManagerMKII.Settings[num13];
        string text14 = text12 + "," + text2 + "," + text3 + "," + text4 + "," + text5 + "," + text6 + "," + text7 + "," + text8 + "," + text9 + "," + text10 + "," + text11 + "," + text + "," + text13;
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
        {
            StartCoroutine(CoLoadSkin(-1, text14));
            return;
        }
        int horseId = -1;
        if (myHorse != null)
        {
            horseId = myHorse.GetPhotonView().viewID;
        }
        base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, horseId, text14);
    }

    [RPC]
    public void loadskinRPC(int horse, string url, PhotonMessageInfo info)
    {
        if ((int)FengGameManagerMKII.Settings[0] == 1)
        {
            if (Guardian.AntiAbuse.Validators.Hero.IsSkinLoadValid(this, info))
            {
                StartCoroutine(CoLoadSkin(horse, url));
            }
        }
    }

    public IEnumerator CoLoadSkin(int horseViewId, string url)
    {
        while (!hasspawn)
        {
            yield return null;
        }
        bool mipmapping = true;
        bool unload = false;
        if ((int)FengGameManagerMKII.Settings[63] == 1)
        {
            mipmapping = false;
        }
        string[] strArray = url.Split(',');
        bool gasSkinsEnabled = false;
        if ((int)FengGameManagerMKII.Settings[15] == 0)
        {
            gasSkinsEnabled = true;
        }
        bool hasHorses = false;
        if (FengGameManagerMKII.Level.Horses || RCSettings.HorseMode == 1)
        {
            hasHorses = true;
        }
        bool trailSkinsEnabled = false;
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine)
        {
            trailSkinsEnabled = true;
        }
        if (setup.part_hair_1 != null) // Hair
        {
            Renderer renderer = setup.part_hair_1.renderer;
            renderer.enabled = true;
            if (strArray[1].EndsWith(".jpg") || strArray[1].EndsWith(".png") || strArray[1].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[1]))
                {
                    WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[1]);
                    if (www != null)
                    {
                        yield return www;
                        Texture2D tex3 = RCextensions.LoadImage(www, mipmapping, 200000);
                        www.Dispose();
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[1]))
                        {
                            unload = true;
                            if (setup.myCostume.hairInfo.id >= 0)
                            {
                                renderer.material = CharacterMaterials.materials[setup.myCostume.hairInfo.texture];
                            }
                            renderer.material.mainTexture = tex3;
                            FengGameManagerMKII.LinkHash[0].Add(strArray[1], renderer.material);
                            renderer.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[1]];
                        }
                        else
                        {
                            renderer.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[1]];
                        }
                    }
                }
                else
                {
                    renderer.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[1]];
                }
            }
            else if (strArray[1].ToLower() == "transparent")
            {
                renderer.enabled = false;
            }
        }
        if (setup.part_cape != null) // Cape
        {
            Renderer renderer2 = setup.part_cape.renderer;
            renderer2.enabled = true;
            if (strArray[7].EndsWith(".jpg") || strArray[7].EndsWith(".png") || strArray[7].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[7]))
                {
                    WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[7]);
                    if (www != null)
                    {
                        yield return www;
                        Texture2D tex4 = RCextensions.LoadImage(www, mipmapping, 200000);
                        www.Dispose();
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[7]))
                        {
                            unload = true;
                            renderer2.material.mainTexture = tex4;
                            FengGameManagerMKII.LinkHash[0].Add(strArray[7], renderer2.material);
                            renderer2.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[7]];
                        }
                        else
                        {
                            renderer2.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[7]];
                        }
                    }
                }
                else
                {
                    renderer2.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[7]];
                }
            }
            else if (strArray[7].ToLower() == "transparent")
            {
                renderer2.enabled = false;
            }
        }
        if (setup.part_chest_3 != null)
        {
            Renderer renderer3 = setup.part_chest_3.renderer;
            renderer3.enabled = true;
            if (strArray[6].EndsWith(".jpg") || strArray[6].EndsWith(".png") || strArray[6].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.LinkHash[1].ContainsKey(strArray[6]))
                {
                    WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[6]);
                    if (www != null)
                    {
                        yield return www;
                        Texture2D tex5 = RCextensions.LoadImage(www, mipmapping, 500000);
                        www.Dispose();
                        if (!FengGameManagerMKII.LinkHash[1].ContainsKey(strArray[6]))
                        {
                            unload = true;
                            renderer3.material.mainTexture = tex5;
                            FengGameManagerMKII.LinkHash[1].Add(strArray[6], renderer3.material);
                            renderer3.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[6]];
                        }
                        else
                        {
                            renderer3.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[6]];
                        }
                    }
                }
                else
                {
                    renderer3.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[6]];
                }
            }
            else if (strArray[6].ToLower() == "transparent")
            {
                renderer3.enabled = false;
            }
        }
        try
        {
            foreach (Renderer renderer4 in GetComponentsInChildren<Renderer>())
            {
                renderer4.enabled = true;

                if (renderer4.name.Contains(FengGameManagerMKII.S[1])) // Hair
                {
                    if (strArray[1].EndsWith(".jpg") || strArray[1].EndsWith(".png") || strArray[1].EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[1]))
                        {
                            WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[1]);
                            if (www != null)
                            {
                                yield return www;

                                // TODO: Old limit: 200KB
                                Texture2D tex6 = RCextensions.LoadImage(www, mipmapping, 1000000);
                                www.Dispose();
                                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[1]))
                                {
                                    unload = true;
                                    if (setup.myCostume.hairInfo.id >= 0)
                                    {
                                        renderer4.material = CharacterMaterials.materials[setup.myCostume.hairInfo.texture];
                                    }
                                    renderer4.material.mainTexture = tex6;
                                    FengGameManagerMKII.LinkHash[0].Add(strArray[1], renderer4.material);
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[1]];
                                }
                                else
                                {
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[1]];
                                }
                            }
                        }
                        else
                        {
                            renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[1]];
                        }
                    }
                    else if (strArray[1].ToLower() == "transparent")
                    {
                        renderer4.enabled = false;
                    }
                }
                else if (renderer4.name.Contains(FengGameManagerMKII.S[2])) // Eyes
                {
                    if (oldEyeMaterial != null)
                    {
                        renderer4.material = oldEyeMaterial;
                    }
                    else
                    {
                        oldEyeMaterial = new Material(renderer4.material);
                    }

                    if (strArray[2].EndsWith(".jpg") || strArray[2].EndsWith(".png") || strArray[2].EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[2]))
                        {
                            WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[2]);
                            if (www != null)
                            {
                                yield return www;

                                // TODO: Old limit: 200KB
                                Texture2D tex7 = RCextensions.LoadImage(www, mipmapping, 500000);
                                www.Dispose();
                                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[2]))
                                {
                                    unload = true;
                                    Material eyeMat = new Material(renderer4.material);
                                    eyeMat.mainTextureScale = eyeMat.mainTextureScale * 8f;
                                    eyeMat.mainTextureOffset = new Vector2(0f, 0f);
                                    eyeMat.mainTexture = tex7;
                                    FengGameManagerMKII.LinkHash[0].Add(strArray[2], eyeMat);
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[2]];
                                }
                                else
                                {
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[2]];
                                }
                            }
                        }
                        else
                        {
                            renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[2]];
                        }
                    }
                    else if (strArray[2].ToLower() == "transparent")
                    {
                        renderer4.enabled = false;
                    }
                }
                else if (renderer4.name.Contains(FengGameManagerMKII.S[3])) // Glasses
                {
                    if (oldGlassesMaterial != null)
                    {
                        renderer4.material = oldGlassesMaterial;
                    }
                    else
                    {
                        oldGlassesMaterial = new Material(renderer4.material);
                    }

                    if (strArray[3].EndsWith(".jpg") || strArray[3].EndsWith(".png") || strArray[3].EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[3]))
                        {
                            WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[3]);
                            if (www != null)
                            {
                                yield return www;

                                // TODO: Old limit: 200KB
                                Texture2D tex8 = RCextensions.LoadImage(www, mipmapping, 500000);
                                www.Dispose();
                                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[3]))
                                {
                                    unload = true;
                                    Material glassesMat = new Material(renderer4.material);
                                    glassesMat.mainTextureScale = glassesMat.mainTextureScale * 8f;
                                    glassesMat.mainTextureOffset = new Vector2(0f, 0f);
                                    glassesMat.mainTexture = tex8;
                                    FengGameManagerMKII.LinkHash[0].Add(strArray[3], glassesMat);
                                }
                                renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[3]];
                            }
                        }
                        else
                        {
                            renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[3]];
                        }
                    }
                    else if (strArray[3].ToLower() == "transparent")
                    {
                        renderer4.enabled = false;
                    }
                }
                else if (renderer4.name.Contains(FengGameManagerMKII.S[4])) // Face
                {
                    if (strArray[4].EndsWith(".jpg") || strArray[4].EndsWith(".png") || strArray[4].EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[4]))
                        {
                            WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[4]);
                            if (www != null)
                            {
                                yield return www;

                                // TODO: Old limit: 200KB
                                Texture2D tex16 = RCextensions.LoadImage(www, mipmapping, 500000);
                                www.Dispose();
                                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[4]))
                                {
                                    unload = true;
                                    renderer4.material.mainTextureScale = renderer4.material.mainTextureScale * 8f;
                                    renderer4.material.mainTextureOffset = new Vector2(0f, 0f);
                                    renderer4.material.mainTexture = tex16;
                                    FengGameManagerMKII.LinkHash[0].Add(strArray[4], renderer4.material);
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[4]];
                                }
                                else
                                {
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[4]];
                                }
                            }
                        }
                        else
                        {
                            renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[4]];
                        }
                    }
                    else if (strArray[4].ToLower() == "transparent")
                    {
                        renderer4.enabled = false;
                    }
                }
                else if (renderer4.name.Contains(FengGameManagerMKII.S[5]) || renderer4.name.Contains(FengGameManagerMKII.S[6]) || renderer4.name.Contains(FengGameManagerMKII.S[10])) // Skin?
                {
                    if (strArray[5].EndsWith(".jpg") || strArray[5].EndsWith(".png") || strArray[5].EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[5]))
                        {
                            WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[5]);
                            if (www != null)
                            {
                                yield return www;

                                // TODO: Old limit: 200KB
                                Texture2D tex15 = RCextensions.LoadImage(www, mipmapping, 1000000);
                                www.Dispose();
                                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[5]))
                                {
                                    unload = true;
                                    renderer4.material.mainTexture = tex15;
                                    FengGameManagerMKII.LinkHash[0].Add(strArray[5], renderer4.material);
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[5]];
                                }
                                else
                                {
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[5]];
                                }
                            }
                        }
                        else
                        {
                            renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[5]];
                        }
                    }
                    else if (strArray[5].ToLower() == "transparent")
                    {
                        renderer4.enabled = false;
                    }
                }
                else if (renderer4.name.Contains(FengGameManagerMKII.S[7]) || renderer4.name.Contains(FengGameManagerMKII.S[8]) || renderer4.name.Contains(FengGameManagerMKII.S[9]) || renderer4.name.Contains(FengGameManagerMKII.S[24])) // Costume?
                {
                    if (strArray[6].EndsWith(".jpg") || strArray[6].EndsWith(".png") || strArray[6].EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[1].ContainsKey(strArray[6]))
                        {
                            WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[6]);
                            if (www != null)
                            {
                                yield return www;

                                // TODO: Old limit: 500KB
                                Texture2D tex14 = RCextensions.LoadImage(www, mipmapping, 1000000);
                                www.Dispose();
                                if (!FengGameManagerMKII.LinkHash[1].ContainsKey(strArray[6]))
                                {
                                    unload = true;
                                    renderer4.material.mainTexture = tex14;
                                    FengGameManagerMKII.LinkHash[1].Add(strArray[6], renderer4.material);
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[6]];
                                }
                                else
                                {
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[6]];
                                }
                            }
                        }
                        else
                        {
                            renderer4.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[6]];
                        }
                    }
                    else if (strArray[6].ToLower() == "transparent")
                    {
                        renderer4.enabled = false;
                    }
                }
                else if (renderer4.name.Contains(FengGameManagerMKII.S[11]) || renderer4.name.Contains(FengGameManagerMKII.S[12])) // Cape
                {
                    if (strArray[7].EndsWith(".jpg") || strArray[7].EndsWith(".png") || strArray[7].EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[7]))
                        {
                            WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[7]);
                            if (www != null)
                            {
                                yield return www;

                                // TODO: Old limit: 200KB
                                Texture2D tex13 = RCextensions.LoadImage(www, mipmapping, 500000);
                                www.Dispose();
                                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[7]))
                                {
                                    unload = true;
                                    renderer4.material.mainTexture = tex13;
                                    FengGameManagerMKII.LinkHash[0].Add(strArray[7], renderer4.material);
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[7]];
                                }
                                else
                                {
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[7]];
                                }
                            }
                        }
                        else
                        {
                            renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[7]];
                        }
                    }
                    else if (strArray[7].ToLower() == "transparent")
                    {
                        renderer4.enabled = false;
                    }
                }
                else if (renderer4.name.Contains(FengGameManagerMKII.S[15]) || ((renderer4.name.Contains(FengGameManagerMKII.S[13]) || renderer4.name.Contains(FengGameManagerMKII.S[26])) && !renderer4.name.Contains("_r"))) // Blade/Gun & 3DMG (Left)
                {
                    if (strArray[8].EndsWith(".jpg") || strArray[8].EndsWith(".png") || strArray[8].EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[1].ContainsKey(strArray[8]))
                        {
                            WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[8]);
                            if (www != null)
                            {
                                yield return www;

                                // TODO: Old limit: 500KB
                                Texture2D tex12 = RCextensions.LoadImage(www, mipmapping, 1000000);
                                www.Dispose();
                                if (!FengGameManagerMKII.LinkHash[1].ContainsKey(strArray[8]))
                                {
                                    unload = true;
                                    renderer4.material.mainTexture = tex12;
                                    FengGameManagerMKII.LinkHash[1].Add(strArray[8], renderer4.material);
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[8]];
                                }
                                else
                                {
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[8]];
                                }
                            }
                        }
                        else
                        {
                            renderer4.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[8]];
                        }
                    }
                    else if (strArray[8].ToLower() == "transparent")
                    {
                        renderer4.enabled = false;
                    }
                }
                else if (renderer4.name.Contains(FengGameManagerMKII.S[16]) || renderer4.name.Contains(FengGameManagerMKII.S[17]) || (renderer4.name.Contains(FengGameManagerMKII.S[26]) && renderer4.name.Contains("_r"))) // Blade/Gun & 3DMG (Right)
                {
                    if (strArray[9].EndsWith(".jpg") || strArray[9].EndsWith(".png") || strArray[9].EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[1].ContainsKey(strArray[9]))
                        {
                            WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[9]);
                            if (www != null)
                            {
                                yield return www;

                                // TODO: Old limit: 500KB
                                Texture2D tex11 = RCextensions.LoadImage(www, mipmapping, 1000000);
                                www.Dispose();
                                if (!FengGameManagerMKII.LinkHash[1].ContainsKey(strArray[9]))
                                {
                                    unload = true;
                                    renderer4.material.mainTexture = tex11;
                                    FengGameManagerMKII.LinkHash[1].Add(strArray[9], renderer4.material);
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[9]];
                                }
                                else
                                {
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[9]];
                                }
                            }
                        }
                        else
                        {
                            renderer4.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[9]];
                        }
                    }
                    else if (strArray[9].ToLower() == "transparent")
                    {
                        renderer4.enabled = false;
                    }
                }
                else if (renderer4.name == FengGameManagerMKII.S[18] && gasSkinsEnabled) // Gas
                {
                    if (strArray[10].EndsWith(".jpg") || strArray[10].EndsWith(".png") || strArray[10].EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[10]))
                        {
                            WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[10]);
                            if (www != null)
                            {
                                yield return www;

                                // TODO: Old limit: 200KB
                                Texture2D tex10 = RCextensions.LoadImage(www, mipmapping, 500000);
                                www.Dispose();
                                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[10]))
                                {
                                    unload = true;
                                    renderer4.material.mainTexture = tex10;
                                    FengGameManagerMKII.LinkHash[0].Add(strArray[10], renderer4.material);
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[10]];
                                }
                                else
                                {
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[10]];
                                }
                            }
                        }
                        else
                        {
                            renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[10]];
                        }
                    }
                    else if (strArray[10].ToLower() == "transparent")
                    {
                        renderer4.enabled = false;
                    }
                }
                else if (renderer4.name.Contains(FengGameManagerMKII.S[25])) // Hoodie?
                {
                    if (strArray[11].EndsWith(".jpg") || strArray[11].EndsWith(".png") || strArray[11].EndsWith(".jpeg"))
                    {
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[11]))
                        {
                            WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[11]);
                            if (www != null)
                            {
                                yield return www;

                                // TODO: Old limit: 200KB
                                Texture2D tex9 = RCextensions.LoadImage(www, mipmapping, 500000);
                                www.Dispose();
                                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[11]))
                                {
                                    unload = true;
                                    renderer4.material.mainTexture = tex9;
                                    FengGameManagerMKII.LinkHash[0].Add(strArray[11], renderer4.material);
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[11]];
                                }
                                else
                                {
                                    renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[11]];
                                }
                            }
                        }
                        else
                        {
                            renderer4.material = (Material)FengGameManagerMKII.LinkHash[0][strArray[11]];
                        }
                    }
                    else if (strArray[11].ToLower() == "transparent")
                    {
                        renderer4.enabled = false;
                    }
                }
            }

            if (trailSkinsEnabled && (strArray[12].EndsWith(".jpg") || strArray[12].EndsWith(".png") || strArray[12].EndsWith(".jpeg"))) // Weapon Trail
            {
                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(strArray[12]))
                {
                    WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[12]);
                    if (www != null)
                    {
                        yield return www;

                        // TODO: Old limit: 200KB
                        Texture2D tex = RCextensions.LoadImage(www, mipmapping, 500000);
                        www.Dispose();
                        unload = true;
                        FengGameManagerMKII.LinkHash[0].Add(strArray[12], tex);
                        leftbladetrail.MyMaterial.mainTexture = (Texture)FengGameManagerMKII.LinkHash[0][strArray[12]];
                        rightbladetrail.MyMaterial.mainTexture = (Texture)FengGameManagerMKII.LinkHash[0][strArray[12]];
                        leftbladetrail2.MyMaterial.mainTexture = (Texture)FengGameManagerMKII.LinkHash[0][strArray[12]];
                        rightbladetrail2.MyMaterial.mainTexture = (Texture)FengGameManagerMKII.LinkHash[0][strArray[12]];
                    }
                }
                else
                {
                    leftbladetrail.MyMaterial.mainTexture = (Texture)FengGameManagerMKII.LinkHash[0][strArray[12]];
                    rightbladetrail.MyMaterial.mainTexture = (Texture)FengGameManagerMKII.LinkHash[0][strArray[12]];
                    leftbladetrail2.MyMaterial.mainTexture = (Texture)FengGameManagerMKII.LinkHash[0][strArray[12]];
                    rightbladetrail2.MyMaterial.mainTexture = (Texture)FengGameManagerMKII.LinkHash[0][strArray[12]];
                }
            }
        }
        finally { }

        // Horse
        if (hasHorses && horseViewId >= 0)
        {
            GameObject theHorse = PhotonView.Find(horseViewId).gameObject;
            if (theHorse != null)
            {
                try
                {
                    Renderer[] componentsInChildren2 = theHorse.GetComponentsInChildren<Renderer>();
                    foreach (Renderer renderer5 in componentsInChildren2)
                    {
                        renderer5.enabled = true;
                        if (renderer5.name.Contains(FengGameManagerMKII.S[19]))
                        {
                            if (strArray[0].EndsWith(".jpg") || strArray[0].EndsWith(".png") || strArray[0].EndsWith(".jpeg"))
                            {
                                if (!FengGameManagerMKII.LinkHash[1].ContainsKey(strArray[0]))
                                {
                                    WWW www = Guardian.Utilities.GameHelper.CreateWWW(strArray[0]);
                                    if (www != null)
                                    {
                                        yield return www;

                                        // TODO: Old limit: 500KB
                                        Texture2D tex2 = RCextensions.LoadImage(www, mipmapping, 1000000);
                                        www.Dispose();
                                        if (!FengGameManagerMKII.LinkHash[1].ContainsKey(strArray[0]))
                                        {
                                            unload = true;
                                            renderer5.material.mainTexture = tex2;
                                            FengGameManagerMKII.LinkHash[1].Add(strArray[0], renderer5.material);
                                            renderer5.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[0]];
                                        }
                                        else
                                        {
                                            renderer5.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[0]];
                                        }
                                    }
                                }
                                else
                                {
                                    renderer5.material = (Material)FengGameManagerMKII.LinkHash[1][strArray[0]];
                                }
                            }
                            else if (strArray[0].ToLower() == "transparent")
                            {
                                renderer5.enabled = false;
                            }
                        }
                    }
                }
                finally { }
            }
        }

        if (unload)
        {
            FengGameManagerMKII.Instance.UnloadAssets();
        }
    }
}