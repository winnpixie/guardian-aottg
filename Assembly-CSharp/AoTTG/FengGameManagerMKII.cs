using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class FengGameManagerMKII : Photon.MonoBehaviour, Anarchy.Custom.Interfaces.IGameManager
{
    // Adapt to the new AoTTG2 applicationId
    public static string ApplicationId
    {
        get { return Guardian.Networking.NetworkHelper.App.Id; }
    }
    public static LevelInfo Level;
    // public static bool LAN;
    public static ExitGames.Client.Photon.Hashtable BanHash;
    public static ExitGames.Client.Photon.Hashtable ImATitan;
    public static ExitGames.Client.Photon.Hashtable[] LinkHash;
    public static object[] Settings;
    public static string[] S =
    {
        "verified343",          // 0
        "hair",                 // 1
        "character_eye",        // 2
        "glass",                // 3
        "character_face",       // 4
        "character_head",       // 5
        "character_hand",       // 6
        "character_body",       // 7
        "character_arm",        // 8
        "character_leg",        // 9
        "character_chest",      // 10
        "character_cape",       // 11
        "character_brand",      // 12
        "character_3dmg",       // 13
        "r",                    // 14
        "character_blade_l",    // 15
        "character_3dmg_gas_r", // 16
        "character_blade_r",    // 17
        "3dmg_smoke",           // 18
        "HORSE",                // 19
        "hair",                 // 20
        "body_001",             // 21
        "Cube",                 // 22
        "Plane_031",            // 23
        "mikasa_asset",         // 24
        "character_cap_",       // 25
        "character_gun"         // 26
    };
    public static AssetBundle RCAssets;
    public static bool IsAssetLoaded;
    public static InputManagerRC InputRC;
    public static string CurrentScript;
    public static Material SkyMaterial;
    public static string OldScript;
    public static string CurrentLevel;
    public static string NameField;
    public static string UsernameField;
    public static string PasswordField;
    public static string PrivateServerField;
    public static ExitGames.Client.Photon.Hashtable IntVariables;
    public static ExitGames.Client.Photon.Hashtable HeroHash;
    public static ExitGames.Client.Photon.Hashtable BoolVariables;
    public static ExitGames.Client.Photon.Hashtable StringVariables;
    public static ExitGames.Client.Photon.Hashtable FloatVariables;
    public static ExitGames.Client.Photon.Hashtable GlobalVariables;
    public static ExitGames.Client.Photon.Hashtable RCRegions;
    public static ExitGames.Client.Photon.Hashtable RCEvents;
    public static ExitGames.Client.Photon.Hashtable RCVariableNames;
    public static ExitGames.Client.Photon.Hashtable PlayerVariables;
    public static ExitGames.Client.Photon.Hashtable TitanVariables;
    public static ExitGames.Client.Photon.Hashtable RCRegionTriggers;
    public static bool LogicLoaded;
    public static bool CustomLevelLoaded;
    public static string OldScriptLogic;
    public static string CurrentScriptLogic;
    public static List<int> IgnoreList;
    public static bool NoRestart;
    public static bool MasterRC;
    public static bool HasLogged;
    public static FengGameManagerMKII Instance;
    public static Dictionary<string, GameObject> CachedPrefabs;
    public static bool OnPrivateServer;
    public static string PrivateServerAuthPass;

    public FengCustomInputs inputManager;
    public int difficulty;
    private GameObject ui;
    public bool needChooseSide;
    public bool justSuicide;
    private ArrayList chatContent;
    private string myLastHero;
    private string myLastRespawnTag = "playerRespawn";
    public float myRespawnTime;
    public int titanScore;
    public int humanScore;
    public int PVPtitanScore;
    public int PVPhumanScore;
    private int PVPtitanScoreMax = 200;
    private int PVPhumanScoreMax = 200;
    private bool isWinning;
    private bool isLosing;
    private int teamWinner;
    private int[] teamScores;
    private float gameEndCD;
    private float gameEndTotalCDtime = 9f;
    public int wave = 1;
    private int highestWave = 1;
    public int time = 600;
    private float timeElapse;
    public float roundTime;

    // Infinite room time
    private float _timeTotalServer;
    public float timeTotalServer
    {
        get { return (PhotonNetwork.isMasterClient && Guardian.Mod.Properties.InfiniteRoom.Value) ? time - Guardian.Utilities.MathHelper.Abs(time) : _timeTotalServer; }
        set { _timeTotalServer = value; }
    }

    private float maxSpeed;
    private float currentSpeed;
    private bool startRacing;
    private bool endRacing;
    public GameObject checkpoint;
    private ArrayList racingResult;
    private bool gameTimesUp;
    public IN_GAME_MAIN_CAMERA mainCamera;
    public bool gameStart;
    private ArrayList heroes;
    private ArrayList eT;
    private ArrayList hooks;
    private ArrayList titans;
    private ArrayList fT;
    private ArrayList cT;
    private string localRacingResult;
    private int single_kills;
    private int single_maxDamage;
    private int single_totalDamage;
    private ArrayList killInfoGO = new ArrayList();
    public List<Vector3> playerSpawnsC;
    public List<Vector3> playerSpawnsM;
    public List<Vector3> titanSpawns;
    public List<PhotonPlayer> otherUsers;
    public List<string[]> levelCache;
    public List<TitanSpawner> titanSpawners;
    public int cyanKills;
    public int magentaKills;
    public Vector2 scroll;
    public Vector2 scroll2;
    public GameObject selectedObj;
    public bool isSpawning;
    public float updateTime;
    public Vector3 racingSpawnPoint;
    public bool racingSpawnPointSet;
    public List<GameObject> racingDoors;
    public List<float> restartCount;
    public Dictionary<int, CannonValues> allowedToCannon;
    public bool restartingMC;
    public bool restartingBomb;
    public bool restartingTitan;
    public bool restartingEren;
    public bool restartingHorse;
    public bool isRestarting;
    public List<GameObject> groundList;
    public Dictionary<string, Texture2D> assetCacheTextures;
    public bool isUnloading;
    public string playerList;
    public bool isRecompiling;
    public List<GameObject> spectateSprites;
    public Dictionary<string, int[]> PreservedPlayerKDR;
    public float qualitySlider;
    public float mouseSlider;
    public float distanceSlider;
    public float transparencySlider;
    public float retryTime;
    public bool isFirstLoad = true;
    public float pauseWaitTime;

    // BEGIN: Guardian
    public List<HERO> Heroes = new List<HERO>();
    public List<TITAN_EREN> Erens = new List<TITAN_EREN>();
    public List<GameObject> Players = new List<GameObject>();
    public List<Bullet> Hooks = new List<Bullet>();
    public List<TITAN> Titans = new List<TITAN>();
    public List<FEMALE_TITAN> Annies = new List<FEMALE_TITAN>();
    public List<GameObject> AllTitans = new List<GameObject>();
    public List<COLOSSAL_TITAN> Colossals = new List<COLOSSAL_TITAN>();

    private long RoundStartTime;
    private long WaveStartTime;
    // END: Guardian

    // BEGIN: TLW/RRC
    [RPC]
    public void TheirPing(int ping, PhotonMessageInfo info)
    {
        info.sender.Ping = ping;
    }
    // END: TLW/RRC

    public void AddHero(HERO hero)
    {
        heroes.Add(hero);

        Heroes.Add(hero);
        Players.Add(hero.gameObject);
    }

    public void RemoveHero(HERO hero)
    {
        heroes.Remove(hero);

        Heroes.Remove(hero);
        Players.Remove(hero.gameObject);
    }

    public void AddHook(Bullet hook)
    {
        hooks.Add(hook);

        Hooks.Add(hook);
    }

    public void RemoveHook(Bullet hook)
    {
        hooks.Remove(hook);

        Hooks.Remove(hook);
    }

    public void AddEren(TITAN_EREN eren)
    {
        eT.Add(eren);

        Erens.Add(eren);
        Players.Add(eren.gameObject);
    }

    public void RemoveEren(TITAN_EREN eren)
    {
        eT.Remove(eren);

        Erens.Remove(eren);
        Players.Remove(eren.gameObject);
    }

    public void AddTitan(TITAN titan)
    {
        titans.Add(titan);

        Titans.Add(titan);
        AllTitans.Add(titan.gameObject);
    }

    public void RemoveTitan(TITAN titan)
    {
        titans.Remove(titan);

        Titans.Remove(titan);
        AllTitans.Remove(titan.gameObject);
    }

    public void AddAnnie(FEMALE_TITAN annie)
    {
        fT.Add(annie);

        Annies.Add(annie);
        AllTitans.Add(annie.gameObject);
    }

    public void RemoveAnnie(FEMALE_TITAN annie)
    {
        fT.Remove(annie);

        Annies.Remove(annie);
        AllTitans.Remove(annie.gameObject);
    }

    public void AddColossal(COLOSSAL_TITAN colossal)
    {
        cT.Add(colossal);

        Colossals.Add(colossal);
    }

    public void RemoveColossal(COLOSSAL_TITAN colossal)
    {
        cT.Remove(colossal);

        Colossals.Remove(colossal);
    }

    public void SetCamera(IN_GAME_MAIN_CAMERA camera)
    {
        mainCamera = camera;
    }

    private void LateUpdate()
    {
        if (gameStart)
        {
            foreach (HERO hero in heroes)
            {
                hero.lateUpdate2();
            }
            foreach (TITAN_EREN eren in eT)
            {
                eren.lateUpdate();
            }
            foreach (TITAN titan in titans)
            {
                titan.lateUpdate2();
            }
            foreach (FEMALE_TITAN annie in fT)
            {
                annie.lateUpdate2();
            }
            Core2();
        }
    }

    private void Update()
    {
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
        {
            GameObject netStatus = GameObject.Find("LabelNetworkStatus");
            if (netStatus)
            {
                UILabel component = netStatus.GetComponent<UILabel>();
                component.text = PhotonNetwork.connectionStateDetailed.ToString();
                if (PhotonNetwork.connected)
                {
                    component.text = component.text + " Ping: " + PhotonNetwork.GetPing() + "ms";
                }
                else
                {
                    component.text = "Disconnected";
                }
            }
        }

        if (gameStart)
        {
            foreach (HERO hero in heroes)
            {
                hero.update2();
            }
            foreach (Bullet hook in hooks)
            {
                hook.update();
            }
            if (mainCamera != null)
            {
                mainCamera.SnapShotUpdate();
            }
            foreach (TITAN_EREN eren in eT)
            {
                eren.update();
            }
            foreach (TITAN titan in titans)
            {
                titan.update();
            }
            foreach (FEMALE_TITAN annie in fT)
            {
                annie.update();
            }
            foreach (COLOSSAL_TITAN colossal in cT)
            {
                colossal.update();
            }
            if (mainCamera != null)
            {
                mainCamera.update2();
            }
        }
    }

    public void SpawnPlayer(string id, string tag = "playerRespawn")
    {
        if (Level.Mode == GameMode.PvPCapture)
        {
            SpawnPlayerAt2(id, checkpoint);
            return;
        }
        myLastRespawnTag = tag;
        GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
        GameObject pos = array[UnityEngine.Random.Range(0, array.Length)];
        SpawnPlayerAt2(id, pos);
    }

    public void NOTSpawnPlayer(string id)
    {
        myLastHero = id.ToUpper();
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.IsDead, true },
            { PhotonPlayerProperty.IsTitan, 1 }
        };
        PhotonNetwork.player.SetCustomProperties(hashtable);
        Screen.lockCursor = IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS;
        Screen.showCursor = false;
        SetTextCenter("The game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
        mainCamera.enabled = true;
        mainCamera.SetMainObject(null);
        mainCamera.SetSpectorMode(val: true);
        mainCamera.gameOver = true;
    }

    public void NOTSpawnNonAITitan(string id)
    {
        myLastHero = id.ToUpper();
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.IsDead, true },
            { PhotonPlayerProperty.IsTitan, 2 }
        };
        PhotonNetwork.player.SetCustomProperties(hashtable);
        Screen.lockCursor = IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS;
        Screen.showCursor = true;
        SetTextCenter("The game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
        mainCamera.enabled = true;
        mainCamera.SetMainObject(null);
        mainCamera.SetSpectorMode(val: true);
        mainCamera.gameOver = true;
    }

    public void SpawnNonAITitan(string id, string tag = "titanRespawn")
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
        GameObject gameObject = array[UnityEngine.Random.Range(0, array.Length)];
        myLastHero = id.ToUpper();
        GameObject gameObject2 = (Level.Mode != GameMode.PvPCapture) ? PhotonNetwork.Instantiate("TITAN_VER3.1", gameObject.transform.position, gameObject.transform.rotation, 0) : PhotonNetwork.Instantiate("TITAN_VER3.1", checkpoint.transform.position + new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20)), checkpoint.transform.rotation, 0);
        mainCamera.SetMainObjectTitan(gameObject2);
        gameObject2.GetComponent<TITAN>().nonAI = true;
        gameObject2.GetComponent<TITAN>().speed = 30f;
        gameObject2.GetComponent<TITAN_CONTROLLER>().enabled = true;
        if (id == "RANDOM" && UnityEngine.Random.Range(0, 100) < 7)
        {
            gameObject2.GetComponent<TITAN>().setAbnormalType2(TitanClass.Crawler, forceCrawler: true);
        }
        mainCamera.enabled = true;
        GameObject mainCam = GameObject.Find("MainCamera");
        mainCam.GetComponent<SpectatorMovement>().disable = true;
        mainCam.GetComponent<MouseLook>().disable = true;
        mainCamera.gameOver = false;
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.IsDead, false },
            { PhotonPlayerProperty.IsTitan, 2 }
        };
        PhotonNetwork.player.SetCustomProperties(hashtable);
        Screen.lockCursor = IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS;
        Screen.showCursor = true;
        SetTextCenter(string.Empty);
    }


    public void OnCreatedRoom()
    {
        racingResult = new ArrayList();
        teamScores = new int[2];
        UnityEngine.MonoBehaviour.print("OnCreatedRoom");
    }


    public void OnDisconnectedFromPhoton()
    {
        UnityEngine.MonoBehaviour.print("OnDisconnectedFromPhoton");
        Screen.lockCursor = false;
        Screen.showCursor = true;
    }

    public void OnConnectionFail(DisconnectCause cause)
    {
        UnityEngine.MonoBehaviour.print("OnConnectionFail: " + cause.ToString());
        Screen.lockCursor = false;
        Screen.showCursor = true;
        IN_GAME_MAIN_CAMERA.Gametype = GameType.Stop;
        gameStart = false;
        NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[0], state: false);
        NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[1], state: false);
        NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[2], state: false);
        NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[3], state: false);
        NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[4], state: true);
        GameObject.Find("LabelDisconnectInfo").GetComponent<UILabel>().text = "OnConnectionFail: " + cause.ToString();
    }

    [RPC]
    private void RequireStatus(PhotonMessageInfo info = null)
    {
        if (Guardian.AntiAbuse.Validators.FGMChecker.IsStatusRequestValid(info))
        {
            base.photonView.RPC("refreshStatus", PhotonTargets.Others, humanScore, titanScore, wave, highestWave, roundTime, timeTotalServer, startRacing, endRacing);
            base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, PVPhumanScore, PVPtitanScore);
            base.photonView.RPC("refreshPVPStatus_AHSS", PhotonTargets.Others, teamScores);
        }
    }

    [RPC]
    private void refreshStatus(int score1, int score2, int theWave, int theHighestWave, float time1, float time2, bool startRacin, bool shouldEndRacing, PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.Validators.FGMChecker.IsStatusRefreshValid(info))
        {
            humanScore = score1;
            titanScore = score2;
            wave = theWave;
            highestWave = theHighestWave;
            roundTime = time1;
            timeTotalServer = time2;
            startRacing = startRacin;
            endRacing = shouldEndRacing;

            if (startRacing && (bool)GameObject.Find("door"))
            {
                GameObject.Find("door").SetActive(value: false);
            }
        }
    }

    [RPC]
    private void refreshPVPStatus(int score1, int score2, PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.Validators.FGMChecker.IsPVPStatusRefreshValid(info))
        {
            PVPhumanScore = score1;
            PVPtitanScore = score2;
        }
    }

    [RPC]
    private void refreshPVPStatus_AHSS(int[] score1, PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.Validators.FGMChecker.IsAHSSStatusRefreshValid(info))
        {
            teamScores = score1;
        }
    }

    [RPC]
    public void someOneIsDead(int id = -1)
    {
        switch (Level.Mode)
        {
            case GameMode.PvPCapture:
                if (id != 0)
                {
                    PVPtitanScore += 2;
                }
                CheckPvPPoints();

                if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && PhotonNetwork.isMasterClient)
                {
                    base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, PVPhumanScore, PVPtitanScore);
                }
                break;
            case GameMode.Endless:
                titanScore++;
                break;
            case GameMode.KillTitans:
            case GameMode.Survival:
            case GameMode.Colossal:
            case GameMode.Trost:
                if (AreAllPlayersDead())
                {
                    FinishGame(true);
                }
                break;
            case GameMode.TeamDeathmatch:
                if (RCSettings.PvPMode == 0 && RCSettings.BombMode == 0)
                {
                    if (AreAllPlayersDead())
                    {
                        FinishGame(true);
                        teamWinner = 0;
                    }
                    if (IsTeamDead(1))
                    {
                        teamWinner = 2;
                        FinishGame();
                    }
                    if (IsTeamDead(2))
                    {
                        teamWinner = 1;
                        FinishGame();
                    }
                }
                break;
        }
    }

    public void CheckPvPPoints()
    {
        if (PVPtitanScore >= PVPtitanScoreMax)
        {
            PVPtitanScore = PVPtitanScoreMax;
            FinishGame(true);
        }
        else if (PVPhumanScore >= PVPhumanScoreMax)
        {
            PVPhumanScore = PVPhumanScoreMax;
            FinishGame();
        }
    }

    public void FinishRaceMulti()
    {
        float num = roundTime - 20f;
        if (PhotonNetwork.isMasterClient)
        {
            getRacingResult(LoginFengKAI.Player.Name, num);
        }
        else
        {
            base.photonView.RPC("getRacingResult", PhotonTargets.MasterClient, LoginFengKAI.Player.Name, num);
        }
        FinishGame();
    }

    [RPC]
    private void getRacingResult(string player, float time)
    {
        RacingResult racingResult = new RacingResult();
        racingResult.name = player;
        racingResult.time = time;
        this.racingResult.Add(racingResult);
        RefreshRacingResult();
    }

    [RPC]
    private void netRefreshRacingResult(string result)
    {
        localRacingResult = result;
    }

    public GameObject SpawnTitanRandom(string place, int rate, bool punk = false)
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag(place);
        int num = UnityEngine.Random.Range(0, array.Length);
        GameObject gameObject = array[num];
        while (array[num] == null)
        {
            num = UnityEngine.Random.Range(0, array.Length);
            gameObject = array[num];
        }
        array[num] = null;
        return SpawnTitan(rate, gameObject.transform.position, gameObject.transform.rotation, punk);
    }

    public GameObject SpawnTitanRaw(Vector3 position, Quaternion rotation)
    {
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
        {
            return (GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_VER3.1"), position, rotation);
        }
        return PhotonNetwork.Instantiate("TITAN_VER3.1", position, rotation, 0);
    }

    public GameObject SpawnTitan(int rate, Vector3 position, Quaternion rotation, bool punk = false)
    {
        GameObject gameObject = SpawnTitanRaw(position, rotation);
        if (punk)
        {
            gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Punk, forceCrawler: false);
        }
        else if (UnityEngine.Random.Range(0, 100) < rate)
        {
            if (IN_GAME_MAIN_CAMERA.Difficulty == 2)
            {
                if (UnityEngine.Random.Range(0f, 1f) < 0.7f || Level.NoCrawlers)
                {
                    gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Jumper, forceCrawler: false);
                }
                else
                {
                    gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Crawler, forceCrawler: false);
                }
            }
        }
        else if (IN_GAME_MAIN_CAMERA.Difficulty == 2)
        {
            if (UnityEngine.Random.Range(0f, 1f) < 0.7f || Level.NoCrawlers)
            {
                gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Jumper, forceCrawler: false);
            }
            else
            {
                gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Crawler, forceCrawler: false);
            }
        }
        else if (UnityEngine.Random.Range(0, 100) < rate)
        {
            if (UnityEngine.Random.Range(0f, 1f) < 0.8f || Level.NoCrawlers)
            {
                gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Aberrant, forceCrawler: false);
            }
            else
            {
                gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Crawler, forceCrawler: false);
            }
        }
        else if (UnityEngine.Random.Range(0f, 1f) < 0.8f || Level.NoCrawlers)
        {
            gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Jumper, forceCrawler: false);
        }
        else
        {
            gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Crawler, forceCrawler: false);
        }
        GameObject gameObject2 = (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer) ? PhotonNetwork.Instantiate("FX/FXtitanSpawn", gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f), 0) : ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanSpawn"), gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f)));
        gameObject2.transform.localScale = gameObject.transform.localScale;
        return gameObject;
    }

    [RPC]
    public void titanGetKill(PhotonPlayer player, int damage, string name, PhotonMessageInfo info = null)
    {
        if (Guardian.AntiAbuse.Validators.FGMChecker.IsTitanKillValid(info))
        {
            damage = Mathf.Max(10, damage);
            base.photonView.RPC("netShowDamage", player, damage);
            base.photonView.RPC("oneTitanDown", PhotonTargets.MasterClient, name, false);
            SendKillInfo(isKillerTitan: false, (string)player.customProperties[PhotonPlayerProperty.Name], isVictimTitan: true, name, damage);
            UpdatePlayerKillInfo(damage, player);
        }
    }

    public void UpdatePlayerKillInfo(int damage, PhotonPlayer player = null)
    {
        if (player != null)
        {
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.Kills, (int)player.customProperties[PhotonPlayerProperty.Kills] + 1 },
                { PhotonPlayerProperty.MaxDamage, Mathf.Max(damage, (int)player.customProperties[PhotonPlayerProperty.MaxDamage]) },
                { PhotonPlayerProperty.TotalDamage, (int)player.customProperties[PhotonPlayerProperty.TotalDamage] + damage }
            });
        }
        else
        {
            single_kills++;
            single_maxDamage = Mathf.Max(damage, single_maxDamage);
            single_totalDamage += damage;
        }
    }

    [RPC]
    public void oneTitanDown(string titanName, bool onPlayerLeave)
    {
        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && !PhotonNetwork.isMasterClient)
        {
            return;
        }

        switch (Level.Mode)
        {
            case GameMode.PvPCapture:
                switch (titanName)
                {
                    case "Titan":
                        PVPhumanScore++;
                        break;
                    case "Aberrant":
                        PVPhumanScore += 2;
                        break;
                    case "Jumper":
                        PVPhumanScore += 3;
                        break;
                    case "Crawler":
                        PVPhumanScore += 4;
                        break;
                    case "Female Titan":
                        PVPhumanScore += 10;
                        break;
                    default:
                        if (titanName.Length != 0)
                        {
                            PVPhumanScore += 3;
                        }
                        break;
                }
                CheckPvPPoints();
                base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, PVPhumanScore, PVPtitanScore);
                break;
            case GameMode.KillTitans:
                if (AreAllTitansDead())
                {
                    FinishGame();
                }
                break;
            case GameMode.Survival:
                if (!AreAllTitansDead())
                {
                    return;
                }

                // BEGIN: Guardian
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
                {
                    long currentTime = Guardian.Utilities.GameHelper.CurrentTimeMillis();
                    if (Guardian.Mod.Properties.AnnounceWaveTime.Value)
                    {
                        Guardian.Utilities.GameHelper.Broadcast($"This wave lasted for <b>{(currentTime - WaveStartTime) / 1000f}</b> second(s)!");
                    }
                    WaveStartTime = currentTime;
                }
                // END: Guardian

                if (++wave > RCSettings.GetMaxWave())
                {
                    FinishGame();
                    return;
                }

                if ((Level.RespawnMode == RespawnMode.NewRound || (Level.Name.StartsWith("Custom") && RCSettings.GameType == 1)) && IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
                {
                    foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
                    {
                        if (GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.IsTitan]) != 2)
                        {
                            base.photonView.RPC("respawnHeroInNewRound", photonPlayer);
                        }
                    }
                }

                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
                {
                    Guardian.Utilities.GameHelper.Broadcast("Wave ".AsColor("AAFF00") + $"{wave} / {RCSettings.GetMaxWave()}");
                }

                if (wave > highestWave)
                {
                    highestWave = wave;
                }

                if (PhotonNetwork.isMasterClient)
                {
                    RequireStatus();
                }

                int abnormal = 90;
                if (difficulty == 1)
                {
                    abnormal = 70;
                }

                if (!Level.Punks || wave % 5 != 0)
                {
                    SpawnTitanCustom("titanRespawn", abnormal, wave + 2, punk: false);
                }
                else
                {
                    SpawnTitanCustom("titanRespawn", abnormal, wave / 5, punk: true);
                }
                break;
            case GameMode.Endless:
                if (!onPlayerLeave)
                {
                    humanScore++;
                    int abnormal2 = 90;
                    if (difficulty == 1)
                    {
                        abnormal2 = 70;
                    }
                    SpawnTitanCustom("titanRespawn", abnormal2, 1, punk: false);
                }
                break;
        }
    }

    [RPC]
    private void respawnHeroInNewRound(PhotonMessageInfo info)
    {
        if (!info.sender.isMasterClient)
        {
            Guardian.Mod.Logger.Info($"Non-MC revive from #{info.sender.Id}.");
        }

        if (!needChooseSide && mainCamera.gameOver)
        {
            SpawnPlayer(myLastHero, myLastRespawnTag);
            mainCamera.gameOver = false;
            SetTextCenter(string.Empty);
        }
    }

    private bool AreAllTitansDead()
    {
        foreach (TITAN titan in titans)
        {
            if (!titan.hasDie)
            {
                return false;
            }
        }

        return fT.Count == 0;
    }

    [RPC]
    public void netShowDamage(int speed, PhotonMessageInfo info = null)
    {
        if (!Guardian.AntiAbuse.Validators.FGMChecker.IsNetShowDamageValid(info))
        {
            return;
        }

        ShowDamage(speed);
    }

    public void ShowDamage(int speed)
    {
        GameObject.Find("Stylish").GetComponent<StylishComponent>().Style(speed);

        GameObject scoreObj = GameObject.Find("LabelScore");
        if ((bool)scoreObj)
        {
            scoreObj.GetComponent<UILabel>().text = speed.ToString();
            scoreObj.transform.localScale = Vector3.zero;
            speed = (int)((float)speed * 0.1f);
            speed = Mathf.Max(40, speed);
            speed = Mathf.Min(150, speed);
            iTween.Stop(scoreObj);
            iTween.ScaleTo(scoreObj, iTween.Hash("x", speed, "y", speed, "z", speed, "easetype", iTween.EaseType.easeOutElastic, "time", 1f));
            iTween.ScaleTo(scoreObj, iTween.Hash("x", 0, "y", 0, "z", 0, "easetype", iTween.EaseType.easeInBounce, "time", 0.5f, "delay", 2f));
        }
    }

    public void SendKillInfo(bool isKillerTitan, string killer, bool isVictimTitan, string victim, int damage = 0)
    {
        base.photonView.RPC("updateKillInfo", PhotonTargets.All, isKillerTitan, killer, isVictimTitan, victim, damage);
    }

    [RPC]
    private void showChatContent(string content, PhotonMessageInfo info)
    {
        if (!Guardian.AntiAbuse.Validators.FGMChecker.IsChatContentShowValid(info))
        {
            return;
        }

        chatContent.Add(content);

        if (chatContent.Count > 10)
        {
            chatContent.RemoveAt(0);
        }

        UILabel chatTextLabel = GameObject.Find("LabelChatContent").GetComponent<UILabel>();
        chatTextLabel.text = string.Empty;
        for (int i = 0; i < chatContent.Count; i++)
        {
            chatTextLabel.text += chatContent[i];
        }
    }

    private void SetTextTopCenter(string content)
    {
        GameObject gameObject = GameObject.Find("LabelInfoTopCenter");
        if ((bool)gameObject)
        {
            gameObject.GetComponent<UILabel>().text = content;
        }
    }

    private void AddTextTopCenter(string content)
    {
        GameObject gameObject = GameObject.Find("LabelInfoTopCenter");
        if ((bool)gameObject)
        {
            gameObject.GetComponent<UILabel>().text += content;
        }
    }

    private void SetTextTopLeft(string content)
    {
        GameObject tlLbl = GameObject.Find("LabelInfoTopLeft");
        if ((bool)tlLbl)
        {
            tlLbl.GetComponent<UILabel>().text = content;
        }
    }

    private void SetTextTopRight(string content)
    {
        GameObject trLbl = GameObject.Find("LabelInfoTopRight");
        if ((bool)trLbl)
        {
            trLbl.GetComponent<UILabel>().text = content;
        }
    }

    private void AddTextTopRight(string content)
    {
        GameObject trLbl = GameObject.Find("LabelInfoTopRight");
        if ((bool)trLbl)
        {
            trLbl.GetComponent<UILabel>().text += content;
        }
    }

    public void SetTextCenter(string content)
    {
        GameObject cLbl = GameObject.Find("LabelInfoCenter");
        if ((bool)cLbl)
        {
            cLbl.GetComponent<UILabel>().text = content;
        }
    }

    public void AddTextCenter(string content)
    {
        GameObject cLbl = GameObject.Find("LabelInfoCenter");
        if ((bool)cLbl)
        {
            cLbl.GetComponent<UILabel>().text += content;
        }
    }

    public static GameObject InstantiateCustomAsset(string key)
    {
        key = key.Substring(8); // "RCAsset/"
        return (GameObject)RCAssets.Load(key);
    }

    void Awake()
    {
        Instance = this;

        // BEGIN: Anarchy
        Anarchy.Custom.Level.CustomAnarchyLevel anarchyLevel = gameObject.AddComponent<Anarchy.Custom.Level.CustomAnarchyLevel>();
        anarchyLevel.GameManager = this;
        // END: Anarchy
    }

    private void Start()
    {
        base.gameObject.name = "MultiplayerManager";
        HeroCostume.Init();
        CharacterMaterials.InitData();
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        heroes = new ArrayList();
        eT = new ArrayList();
        titans = new ArrayList();
        fT = new ArrayList();
        cT = new ArrayList();
        hooks = new ArrayList();
        if (NameField == null)
        {
            NameField = "GUEST" + UnityEngine.Random.Range(0, 100000);
        }
        if (PrivateServerField == null)
        {
            PrivateServerField = string.Empty;
        }
        UsernameField = string.Empty;
        PasswordField = string.Empty;
        ResetGameSettings();
        BanHash = new ExitGames.Client.Photon.Hashtable();
        ImATitan = new ExitGames.Client.Photon.Hashtable();
        OldScript = string.Empty;
        CurrentLevel = string.Empty;
        if (CurrentScript == null)
        {
            CurrentScript = string.Empty;
        }
        titanSpawns = new List<Vector3>();
        playerSpawnsC = new List<Vector3>();
        playerSpawnsM = new List<Vector3>();
        otherUsers = new List<PhotonPlayer>();
        levelCache = new List<string[]>();
        titanSpawners = new List<TitanSpawner>();
        restartCount = new List<float>();
        IgnoreList = new List<int>();
        groundList = new List<GameObject>();
        NoRestart = false;
        MasterRC = false;
        isSpawning = false;
        IntVariables = new ExitGames.Client.Photon.Hashtable();
        HeroHash = new ExitGames.Client.Photon.Hashtable();
        BoolVariables = new ExitGames.Client.Photon.Hashtable();
        StringVariables = new ExitGames.Client.Photon.Hashtable();
        FloatVariables = new ExitGames.Client.Photon.Hashtable();
        GlobalVariables = new ExitGames.Client.Photon.Hashtable();
        RCRegions = new ExitGames.Client.Photon.Hashtable();
        RCEvents = new ExitGames.Client.Photon.Hashtable();
        RCVariableNames = new ExitGames.Client.Photon.Hashtable();
        RCRegionTriggers = new ExitGames.Client.Photon.Hashtable();
        PlayerVariables = new ExitGames.Client.Photon.Hashtable();
        TitanVariables = new ExitGames.Client.Photon.Hashtable();
        LogicLoaded = false;
        CustomLevelLoaded = false;
        OldScriptLogic = string.Empty;
        CurrentScriptLogic = string.Empty;
        retryTime = 0f;
        playerList = string.Empty;
        updateTime = 0f;
        LoadConfig();

        List<string> list = new List<string>
        {
            "PanelLogin",
            "LOGIN"
        };
        foreach (GameObject go in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            foreach (string item in list)
            {
                if (go.name.Contains(item))
                {
                    UnityEngine.Object.Destroy(go);
                }
            }
        }

        ChangeQuality.SetCurrentQuality();

        // Register our Mod as a component
        base.gameObject.AddComponent<Guardian.Mod>();
    }

    // BEGIN: Anarchy
    public float GetRoundTime()
    {
        return roundTime;
    }

    public string GetLevelName()
    {
        return Level.Name;
    }

    public bool IsCustomMapLoaded()
    {
        return CustomLevelLoaded;
    }
    // END: Anarchy

    [RPC]
    private void Chat(string message, string sender, PhotonMessageInfo info)
    {
        if (info.sender.Muted) { return; }

        if (Guardian.Mod.Properties.TranslateIncoming.Value && !info.sender.isLocal)
        {
            StartCoroutine(Guardian.Utilities.Translator.Translate(message, Guardian.Mod.Properties.IncomingLanguage.Value, Guardian.Mod.SystemLanguage, result =>
            {
                if (result.Length > 1 && !result[0].Equals(Guardian.Mod.SystemLanguage, StringComparison.OrdinalIgnoreCase))
                {
                    message = $"[gt] ".AsColor("0099ff") + result[1];
                }

                if (sender.Length == 0)
                {
                    InRoomChat.Instance.AddMessage(("[" + info.sender.Id + "]").AsColor("FFCC00"), message);
                }
                else
                {
                    InRoomChat.Instance.AddMessage(("[" + info.sender.Id + "] ").AsColor("FFCC00") + sender, message);
                }
            }));

            return;
        }

        if (sender.Length == 0)
        {
            InRoomChat.Instance.AddMessage(("[" + info.sender.Id + "]").AsColor("FFCC00"), message);
        }
        else
        {
            InRoomChat.Instance.AddMessage(("[" + info.sender.Id + "] ").AsColor("FFCC00") + sender, message);
        }
    }

    public void OnJoinedRoom()
    {
        PhotonNetwork.room.expectedMaxPlayers = PhotonNetwork.room.maxPlayers;
        PhotonNetwork.room.expectedJoinability = PhotonNetwork.room.open;
        PhotonNetwork.room.expectedVisibility = PhotonNetwork.room.visible;

        string[] roomInfo = PhotonNetwork.room.name.Split('`');
        LevelInfo levelInfo = LevelInfo.GetInfo(roomInfo[1]);
        playerList = string.Empty;
        UnityEngine.MonoBehaviour.print("OnJoinedRoom " + PhotonNetwork.room.name + " >>>> " + levelInfo.Map);
        gameTimesUp = false;

        difficulty = roomInfo[2] switch
        {
            "normal" => 0,
            "hard" => 1,
            "abnormal" => 2,
            _ => -1
        };

        IN_GAME_MAIN_CAMERA.Difficulty = difficulty;
        time = int.Parse(roomInfo[3]) * 60;

        if (PhotonNetwork.room.customProperties.ContainsKey("Lighting")
           && PhotonNetwork.room.customProperties["Lighting"] is string lighting)
        {
            if (GExtensions.TryParseEnum(lighting, out DayLight customDayLight))
            {
                IN_GAME_MAIN_CAMERA.Lighting = customDayLight;
            }
        }
        else if (GExtensions.TryParseEnum(roomInfo[4], out DayLight dayLight))
        {
            IN_GAME_MAIN_CAMERA.Lighting = dayLight;
        }

        if (PhotonNetwork.room.customProperties.ContainsKey("Map")
            && PhotonNetwork.room.customProperties["Map"] is string map)
        {
            levelInfo = LevelInfo.GetInfo(map);
        }

        Level = levelInfo;
        PhotonNetwork.LoadLevel(levelInfo.Map);

        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();

        string playerName = LoginFengKAI.Player.Name;
        if (LoginFengKAI.LoginState != LoginState.LoggedIn && NameField.StripNGUI().Length > 0)
        {
            LoginFengKAI.Player.Name = playerName = NameField;
        }
        hashtable.Add(PhotonPlayerProperty.Name, playerName);

        hashtable.Add(PhotonPlayerProperty.Guild, LoginFengKAI.Player.Guild);
        hashtable.Add(PhotonPlayerProperty.Kills, 0);
        hashtable.Add(PhotonPlayerProperty.MaxDamage, 0);
        hashtable.Add(PhotonPlayerProperty.TotalDamage, 0);
        hashtable.Add(PhotonPlayerProperty.Deaths, 0);
        hashtable.Add(PhotonPlayerProperty.IsDead, true);
        hashtable.Add(PhotonPlayerProperty.IsTitan, 0);
        hashtable.Add(PhotonPlayerProperty.RCTeam, 0);
        hashtable.Add(PhotonPlayerProperty.CurrentLevel, string.Empty);
        PhotonNetwork.player.SetCustomProperties(hashtable);

        humanScore = 0;
        titanScore = 0;
        PVPtitanScore = 0;
        PVPhumanScore = 0;
        wave = 1;
        highestWave = 1;
        localRacingResult = string.Empty;
        needChooseSide = true;
        chatContent = new ArrayList();
        killInfoGO = new ArrayList();
        InRoomChat.Messages = new List<InRoomChat.Message>();

        assetCacheTextures = new Dictionary<string, Texture2D>();
        isFirstLoad = true;

        if (OnPrivateServer)
        {
            ServerRequestAuthentication(PrivateServerAuthPass);
        }
    }

    public void OnLeftRoom()
    {
        if (Application.loadedLevel == 0) return;

        Time.timeScale = 1f;
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Disconnect();
        }
        ResetSettings(isLeave: true);
        LoadConfig();
        IN_GAME_MAIN_CAMERA.Gametype = GameType.Stop;
        gameStart = false;
        Screen.lockCursor = false;
        Screen.showCursor = true;
        inputManager.menuOn = false;
        DestroyAllExistingCloths();
        UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
        Application.LoadLevel("menu");
    }

    public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        if (!NoRestart)
        {
            if (PhotonNetwork.isMasterClient)
            {
                restartingMC = true;
                if (RCSettings.InfectionMode > 0)
                {
                    restartingTitan = true;
                }
                if (RCSettings.BombMode > 0)
                {
                    restartingBomb = true;
                }
                if (RCSettings.HorseMode > 0)
                {
                    restartingHorse = true;
                }
                if (RCSettings.BanEren == 0)
                {
                    restartingEren = true;
                }
            }
            ResetSettings(isLeave: false);
            if (!Level.PlayerTitans)
            {
                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable
                {
                    { PhotonPlayerProperty.IsTitan, 1 }
                };
                PhotonNetwork.player.SetCustomProperties(hashtable);
            }
            if (!gameTimesUp && PhotonNetwork.isMasterClient)
            {
                RestartGame(masterClientSwitched: true);
                base.photonView.RPC("setMasterRC", PhotonTargets.All);
            }
        }

        NoRestart = false;
    }

    [RPC]
    private void RPCLoadLevel(PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            DestroyAllExistingCloths();
            PhotonNetwork.LoadLevel(Level.Map);
        }
        else if (PhotonNetwork.isMasterClient)
        {
            KickPlayer(info.sender, ban: true, "False restart.");
        }
        else if (!MasterRC)
        {
            restartCount.Add(Time.time);
            foreach (float item in restartCount)
            {
                float num = item;
                if (Time.time - num > 60f)
                {
                    restartCount.Remove(num);
                }
            }

            if (restartCount.Count < 6)
            {
                DestroyAllExistingCloths();
                PhotonNetwork.LoadLevel(Level.Map);
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0 || Application.loadedLevelName == "characterCreation" || Application.loadedLevelName == "SnapShot") return;

        ChangeQuality.SetCurrentQuality();

        difficulty = IN_GAME_MAIN_CAMERA.Difficulty;

        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("titan"))
        {
            if (gameObject.GetPhotonView() == null || !gameObject.GetPhotonView().owner.isMasterClient)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        isWinning = false;
        gameStart = true;
        SetTextCenter(string.Empty);

        GameObject mainCamGameObj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("MainCamera_mono"), GameObject.Find("cameraDefaultPosition").transform.position, GameObject.Find("cameraDefaultPosition").transform.rotation);
        UnityEngine.Object.Destroy(GameObject.Find("cameraDefaultPosition"));
        mainCamGameObj.name = "MainCamera";
        SetCamera(mainCamGameObj.GetComponent<IN_GAME_MAIN_CAMERA>());

        Screen.lockCursor = true;
        Screen.showCursor = true;

        ui = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI_IN_GAME"));
        ui.name = "UI_IN_GAME";
        ui.SetActive(value: true);
        NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[0], state: true);
        NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[1], state: false);
        NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[2], state: false);
        NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[3], state: false);

        Cache();
        LoadSkin();

        mainCamGameObj.GetComponent<IN_GAME_MAIN_CAMERA>().SetHUDPosition();
        mainCamGameObj.GetComponent<IN_GAME_MAIN_CAMERA>().SetLighting(IN_GAME_MAIN_CAMERA.Lighting);

        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
        {
            single_kills = 0;
            single_maxDamage = 0;
            single_totalDamage = 0;
            mainCamGameObj.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
            mainCamGameObj.GetComponent<SpectatorMovement>().disable = true;
            mainCamGameObj.GetComponent<MouseLook>().disable = true;
            SpawnPlayer(IN_GAME_MAIN_CAMERA.SingleCharacter.ToUpper());
            Screen.lockCursor = IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS;
            Screen.showCursor = false;
            int abnormal = 90;
            if (difficulty == 1)
            {
                abnormal = 70;
            }
            SpawnTitanCustom("titanRespawn", abnormal, Level.Enemies, punk: false);
            return;
        }

        PVPcheckPoint.chkPts = new ArrayList();
        mainCamGameObj.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
        mainCamGameObj.GetComponent<CameraShake>().enabled = false;
        IN_GAME_MAIN_CAMERA.Gametype = GameType.Multiplayer;

        if (Level.Mode == GameMode.Trost)
        {
            GameObject.Find("playerRespawn").SetActive(value: false);
            UnityEngine.Object.Destroy(GameObject.Find("playerRespawn"));
            GameObject.Find("rock").animation["lift"].speed = 0f;
            GameObject.Find("door_fine").SetActive(false);
            GameObject.Find("door_broke").SetActive(true);
            UnityEngine.Object.Destroy(GameObject.Find("ppl"));
        }
        else if (Level.Mode == GameMode.Colossal)
        {
            GameObject.Find("playerRespawnTrost").SetActive(value: false);
            UnityEngine.Object.Destroy(GameObject.Find("playerRespawnTrost"));
        }

        if (needChooseSide)
        {
            AddTextTopCenter("\n\nPRESS 1 TO ENTER GAME");
        }
        else if ((int)Settings[245] == 0)
        {
            Screen.lockCursor = IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS;
            if (Level.Mode == GameMode.PvPCapture)
            {
                if (GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.IsTitan]) == 2)
                {
                    checkpoint = GameObject.Find("PVPchkPtT");
                }
                else
                {
                    checkpoint = GameObject.Find("PVPchkPtH");
                }
            }
            if (GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.IsTitan]) == 2)
            {
                SpawnNonAITitan2(myLastHero);
            }
            else
            {
                SpawnPlayer(myLastHero, myLastRespawnTag);
            }
        }

        if (Level.Mode == GameMode.Colossal)
        {
            UnityEngine.Object.Destroy(GameObject.Find("rock"));
        }

        if (PhotonNetwork.isMasterClient)
        {
            switch (Level.Mode)
            {
                case GameMode.Trost:
                    if (!AreAllPlayersDead())
                    {
                        PhotonNetwork.Instantiate("TITAN_EREN_trost", new Vector3(-200f, 0f, -194f), Quaternion.Euler(0f, 180f, 0f), 0).GetComponent<TITAN_EREN>().rockLift = true;
                        int rate = 90;
                        if (difficulty == 1)
                        {
                            rate = 70;
                        }

                        GameObject trostRespawn = GameObject.Find("titanRespawnTrost");
                        if (trostRespawn != null)
                        {
                            foreach (GameObject titanRespawn in GameObject.FindGameObjectsWithTag("titanRespawn"))
                            {
                                if (titanRespawn.transform.parent.gameObject == trostRespawn)
                                {
                                    SpawnTitan(rate, titanRespawn.transform.position, titanRespawn.transform.rotation);
                                }
                            }
                        }
                    }
                    break;
                case GameMode.Colossal:
                    if (!AreAllPlayersDead())
                    {
                        PhotonNetwork.Instantiate("COLOSSAL_TITAN", -Vector3.up * 10000f, Quaternion.Euler(0f, 180f, 0f), 0);
                    }
                    break;
                case GameMode.KillTitans:
                case GameMode.Endless:
                case GameMode.Survival:
                    if (Level.Name == "Annie" || Level.Name == "Annie II")
                    {
                        GameObject titanRespawnPoint = GameObject.Find("titanRespawn");
                        PhotonNetwork.Instantiate("FEMALE_TITAN", titanRespawnPoint.transform.position, titanRespawnPoint.transform.rotation, 0);
                    }
                    else
                    {
                        int abnormalRate = 90;
                        if (difficulty == 1)
                        {
                            abnormalRate = 70;
                        }
                        SpawnTitanCustom("titanRespawn", abnormalRate, Level.Enemies, punk: false);
                    }
                    break;
                case GameMode.PvPCapture:
                    if (Level.Map == "OutSide")
                    {
                        foreach (GameObject respawnPoint in GameObject.FindGameObjectsWithTag("titanRespawn"))
                        {
                            SpawnTitanRaw(respawnPoint.transform.position, respawnPoint.transform.rotation).GetComponent<TITAN>().setAbnormalType2(TitanClass.Crawler, forceCrawler: true);
                        }
                    }
                    break;
            }

            // Guardian
            RoundStartTime = Guardian.Utilities.GameHelper.CurrentTimeMillis();
            WaveStartTime = RoundStartTime;
        }
        else
        {
            base.photonView.RPC("RequireStatus", PhotonTargets.MasterClient);
        }

        if (!Level.HasSupply)
        {
            UnityEngine.Object.Destroy(GameObject.Find("aot_supply"));
        }

        if (Level.Lava)
        {
            UnityEngine.Object.Instantiate(Resources.Load("levelBottom"), new Vector3(0f, -29.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
            GameObject supplyObject = GameObject.Find("aot_supply");
            GameObject lavaSupplyObject = GameObject.Find("aot_supply_lava_position");
            supplyObject.transform.position = lavaSupplyObject.transform.position;
            supplyObject.transform.rotation = lavaSupplyObject.transform.rotation;
        }

        if ((int)Settings[245] == 1)
        {
            EnterSpecMode(enter: true);
        }
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (BanHash.ContainsValue(GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name])))
            {
                KickPlayer(player, ban: false, "Banned.");
            }
            else
            {
                int acl = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.StatAccel]);
                int bla = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.StatBlade]);
                int gas = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.StatGas]);
                int spd = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.StatSpeed]);
                if (acl > 150 || bla > 125 || gas > 150 || spd > 140)
                {
                    KickPlayer(player, ban: true, "Excessive stats.");
                    return;
                }

                if (RCSettings.AsoPreserveKDR == 1)
                {
                    StartCoroutine(CoWaitAndReloadKD(player));
                }

                if (Level.Name.StartsWith("Custom"))
                {
                    StartCoroutine(CoLoadCustomLevel(new List<PhotonPlayer> { player }));
                }

                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                if (RCSettings.BombMode == 1)
                {
                    hashtable.Add("bomb", 1);
                }
                if (RCSettings.GlobalDisableMinimap == 1)
                {
                    hashtable.Add("globalDisableMinimap", 1);
                }
                if (RCSettings.TeamMode > 0)
                {
                    hashtable.Add("team", RCSettings.TeamMode);
                }
                if (RCSettings.PointMode > 0)
                {
                    hashtable.Add("point", RCSettings.PointMode);
                }
                if (RCSettings.DisableRock > 0)
                {
                    hashtable.Add("rock", RCSettings.DisableRock);
                }
                if (RCSettings.ExplodeMode > 0)
                {
                    hashtable.Add("explode", RCSettings.ExplodeMode);
                }
                if (RCSettings.HealthMode > 0)
                {
                    hashtable.Add("healthMode", RCSettings.HealthMode);
                    hashtable.Add("healthLower", RCSettings.HealthLower);
                    hashtable.Add("healthUpper", RCSettings.HealthUpper);
                }
                if (RCSettings.InfectionMode > 0)
                {
                    hashtable.Add("infection", RCSettings.InfectionMode);
                }
                if (RCSettings.BanEren == 1)
                {
                    hashtable.Add("eren", RCSettings.BanEren);
                }
                if (RCSettings.MoreTitans > 0)
                {
                    hashtable.Add("titanc", RCSettings.MoreTitans);
                }
                if (RCSettings.MinimumDamage > 0)
                {
                    hashtable.Add("damage", RCSettings.MinimumDamage);
                }
                if (RCSettings.SizeMode > 0)
                {
                    hashtable.Add("sizeMode", RCSettings.SizeMode);
                    hashtable.Add("sizeLower", RCSettings.SizeLower);
                    hashtable.Add("sizeUpper", RCSettings.SizeUpper);
                }
                if (RCSettings.SpawnMode > 0)
                {
                    hashtable.Add("spawnMode", RCSettings.SpawnMode);
                    hashtable.Add("nRate", RCSettings.NormalRate);
                    hashtable.Add("aRate", RCSettings.AberrantRate);
                    hashtable.Add("jRate", RCSettings.JumperRate);
                    hashtable.Add("cRate", RCSettings.CrawlerRate);
                    hashtable.Add("pRate", RCSettings.PunkRate);
                }
                if (RCSettings.WaveModeOn > 0)
                {
                    hashtable.Add("waveModeOn", 1);
                    hashtable.Add("waveModeNum", RCSettings.WaveModeNum);
                }
                if (RCSettings.FriendlyMode > 0)
                {
                    hashtable.Add("friendly", 1);
                }
                if (RCSettings.PvPMode > 0)
                {
                    hashtable.Add("pvp", RCSettings.PvPMode);
                }
                if (RCSettings.MaxWave > 0)
                {
                    hashtable.Add("maxwave", RCSettings.MaxWave);
                }
                if (RCSettings.EndlessMode > 0)
                {
                    hashtable.Add("endless", RCSettings.EndlessMode);
                }
                if (RCSettings.Motd.Length > 0)
                {
                    hashtable.Add("motd", RCSettings.Motd);
                }
                if (RCSettings.HorseMode > 0)
                {
                    hashtable.Add("horse", RCSettings.HorseMode);
                }
                if (RCSettings.AhssReload > 0)
                {
                    hashtable.Add("ahssReload", RCSettings.AhssReload);
                }
                if (RCSettings.PunkWaves > 0)
                {
                    hashtable.Add("punkWaves", RCSettings.PunkWaves);
                }
                if (RCSettings.DeadlyCannons > 0)
                {
                    hashtable.Add("deadlycannons", RCSettings.DeadlyCannons);
                }
                if (RCSettings.RacingStatic > 0)
                {
                    hashtable.Add("asoracing", RCSettings.RacingStatic);
                }
                if (IgnoreList != null && IgnoreList.Count > 0)
                {
                    photonView.RPC("ignorePlayerArray", player, IgnoreList.ToArray());
                }
                photonView.RPC("settingRPC", player, hashtable);
                photonView.RPC("setMasterRC", player);

                if (Time.timeScale <= 0.1f && pauseWaitTime > 3f)
                {
                    base.photonView.RPC("pauseRPC", player, true);
                    base.photonView.RPC("Chat", player, "MasterClient has paused the game.".AsColor("FFCC00"), string.Empty);
                }

            }
        }

        RecompilePlayerList(0.1f);
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (!gameTimesUp)
        {
            oneTitanDown(string.Empty, onPlayerLeave: true);
            someOneIsDead(0);
        }
        if (IgnoreList.Contains(player.Id))
        {
            IgnoreList.Remove(player.Id);
        }
        InstantiateTracker.Instance.TryRemovePlayer(player.Id);
        if (PhotonNetwork.isMasterClient)
        {
            base.photonView.RPC("verifyPlayerHasLeft", PhotonTargets.All, player.Id);
        }
        if (RCSettings.AsoPreserveKDR == 1)
        {
            string key = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]);
            if (PreservedPlayerKDR.ContainsKey(key))
            {
                PreservedPlayerKDR.Remove(key);
            }
            int[] value = new int[4]
            {
                GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Kills]),
                GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Deaths]),
                GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.MaxDamage]),
                GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.TotalDamage])
            };
            PreservedPlayerKDR.Add(key, value);
        }
        RecompilePlayerList(0.1f);
    }

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        RecompilePlayerList(0.1f);

        PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
        ExitGames.Client.Photon.Hashtable properties = (ExitGames.Client.Photon.Hashtable)playerAndUpdatedProps[1];

        if (!properties.ContainsKey("sender") || !(properties["sender"] is PhotonPlayer sender)
            || !player.isLocal || sender.isLocal) return;

        ExitGames.Client.Photon.Hashtable restored = new ExitGames.Client.Photon.Hashtable();
        if (properties.ContainsKey(PhotonPlayerProperty.Name)
            && player.Username != LoginFengKAI.Player.Name)
        {
            restored.Add(PhotonPlayerProperty.Name, LoginFengKAI.Player.Name);
        }

        if (properties.ContainsKey(PhotonPlayerProperty.Guild)
            && player.Guild != LoginFengKAI.Player.Guild)
        {
            restored.Add(PhotonPlayerProperty.Guild, LoginFengKAI.Player.Guild);
        }

        if (properties.ContainsKey(PhotonPlayerProperty.StatSpeed)
            && player.SpeedStat > 140)
        {
            restored.Add(PhotonPlayerProperty.StatSpeed, 100);
        }

        if (properties.ContainsKey(PhotonPlayerProperty.StatBlade)
            && player.BladeStat > 125)
        {
            restored.Add(PhotonPlayerProperty.StatBlade, 100);
        }

        if (properties.ContainsKey(PhotonPlayerProperty.StatGas)
            && player.GasStat > 150)
        {
            restored.Add(PhotonPlayerProperty.StatGas, 100);
        }

        if (properties.ContainsKey(PhotonPlayerProperty.StatAccel)
            && player.AccelStat > 150)
        {
            restored.Add(PhotonPlayerProperty.StatAccel, 100);
        }

        if (restored.Count < 1) return;

        PhotonNetwork.player.SetCustomProperties(restored);
    }

    public void OnPhotonCustomRoomPropertiesChanged()
    {
        if (PhotonNetwork.isMasterClient) return;

        PhotonNetwork.room.expectedMaxPlayers = PhotonNetwork.room.maxPlayers;
        PhotonNetwork.room.expectedJoinability = PhotonNetwork.room.open;
        PhotonNetwork.room.expectedVisibility = PhotonNetwork.room.visible;
    }

    [RPC]
    private void showResult(string players, string kills, string deaths, string maxDamage, string totalDamage, string gameResult, PhotonMessageInfo info)
    {
        if (!gameTimesUp && info.sender.isMasterClient)
        {
            gameTimesUp = true;
            NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[0], state: false);
            NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[1], state: false);
            NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[2], state: true);
            NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[3], state: false);
            GameObject.Find("LabelName").GetComponent<UILabel>().text = players;
            GameObject.Find("LabelKill").GetComponent<UILabel>().text = kills;
            GameObject.Find("LabelDead").GetComponent<UILabel>().text = deaths;
            GameObject.Find("LabelMaxDmg").GetComponent<UILabel>().text = maxDamage;
            GameObject.Find("LabelTotalDmg").GetComponent<UILabel>().text = totalDamage;
            GameObject.Find("LabelResultTitle").GetComponent<UILabel>().text = gameResult;
            Screen.lockCursor = false;
            Screen.showCursor = true;
            IN_GAME_MAIN_CAMERA.Gametype = GameType.Stop;
            gameStart = false;
        }
        else if (!info.sender.isMasterClient && PhotonNetwork.player.isMasterClient)
        {
            KickPlayer(info.sender, ban: true, "false game end.");
        }
    }

    [RPC]
    private void restartGameByClient()
    {
        // RestartGame();
    }

    [RPC]
    private void updateKillInfo(bool isKillerTitan, string killer, bool isVictimTitan, string victim, int damage, PhotonMessageInfo info)
    {
        if (!Guardian.AntiAbuse.Validators.FGMChecker.IsKillInfoUpdateValid(isKillerTitan, isVictimTitan, damage, info)) return;

        GameObject infoObj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/KillInfo"));
        for (int i = 0; i < killInfoGO.Count; i++)
        {
            GameObject killInfoObj = (GameObject)killInfoGO[i];
            if (killInfoObj != null)
            {
                killInfoObj.GetComponent<KillInfoComponent>().MoveOn();
            }
        }

        if (killInfoGO.Count > 4)
        {
            GameObject gameObject3 = (GameObject)killInfoGO[0];
            if (gameObject3 != null)
            {
                gameObject3.GetComponent<KillInfoComponent>().EndLifeTime();
            }
            killInfoGO.RemoveAt(0);
        }
        infoObj.transform.parent = ui.GetComponent<UIReferArray>().panels[0].transform;
        infoObj.GetComponent<KillInfoComponent>().Show(isKillerTitan, killer, isVictimTitan, victim, damage);
        killInfoGO.Add(infoObj);

        if ((int)Settings[244] == 1)
        {
            InRoomChat.Instance.AddLine(("(" + roundTime.ToString("F2") + ") ").AsColor("FFCC00") + killer.NGUIToUnity() + " killed " + victim.NGUIToUnity() + " for " + damage + " damage.");
        }
    }

    [RPC]
    private void netGameLose(int score, PhotonMessageInfo info)
    {
        isLosing = true;
        titanScore = score;
        gameEndCD = gameEndTotalCDtime;
        if ((int)Settings[244] == 1)
        {
            InRoomChat.Instance.AddLine(("(" + roundTime.ToString("F2") + ") ").AsColor("FFCC00") + "Round ended (game lose).");
        }
        if (!info.sender.isMasterClient && !info.sender.isLocal && PhotonNetwork.isMasterClient)
        {
            InRoomChat.Instance.AddLine("Round end sent from #".AsColor("FFCC00") + info.sender.Id);
        }
    }

    [RPC]
    private void netGameWin(int score, PhotonMessageInfo info)
    {
        humanScore = score;
        isWinning = true;

        switch (Level.Mode)
        {
            case GameMode.TeamDeathmatch:
                teamWinner = score;
                teamScores[teamWinner - 1]++;
                gameEndCD = gameEndTotalCDtime;
                break;
            case GameMode.Racing:
                if (RCSettings.RacingStatic == 1)
                {
                    gameEndCD = 1000f;
                }
                else
                {
                    gameEndCD = 20f;
                }
                break;
            default:
                gameEndCD = gameEndTotalCDtime;
                break;
        }
        if ((int)Settings[244] == 1)
        {
            InRoomChat.Instance.AddLine(("(" + roundTime.ToString("F2") + ") ").AsColor("FFCC00") + "Round ended (game win).");
        }
        if (!info.sender.isMasterClient && !info.sender.isLocal)
        {
            InRoomChat.Instance.AddLine("Round end sent from #".AsColor("FFCC00") + info.sender.Id);
        }
    }

    public void OnJoinedLobby()
    {
        UIMainReferences references = GameObject.Find("UIRefer").GetComponent<UIMainReferences>();
        NGUITools.SetActive(references.panelMultiStart, state: false);
        NGUITools.SetActive(references.panelMultiROOM, state: true);
        NGUITools.SetActive(references.PanelMultiJoinPrivate, state: false);
    }

    public void RestartGame(bool masterClientSwitched = false)
    {
        if (!gameTimesUp)
        {
            PVPtitanScore = 0;
            PVPhumanScore = 0;
            startRacing = false;
            endRacing = false;
            checkpoint = null;
            timeElapse = 0f;
            roundTime = 0f;
            isWinning = false;
            isLosing = false;
            wave = 1;
            myRespawnTime = 0f;
            killInfoGO = new ArrayList();
            racingResult = new ArrayList();
            SetTextCenter(string.Empty);
            isRestarting = true;
            DestroyAllExistingCloths();
            PhotonNetwork.DestroyAll();
            ExitGames.Client.Photon.Hashtable gameSettings = CheckGameGUI();
            base.photonView.RPC("settingRPC", PhotonTargets.Others, gameSettings);
            base.photonView.RPC("RPCLoadLevel", PhotonTargets.All);
            SetGameSettings(gameSettings);

            if (masterClientSwitched)
            {
                Guardian.Utilities.GameHelper.Broadcast("MasterClient has switched to " + ((string)PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity().AsBold());
            }
        }
    }

    private void Core2()
    {
        if ((int)Settings[64] >= 100)
        {
            CoreEditor();
            return;
        }

        if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && needChooseSide)
        {
            if (inputManager.isInputDown[InputCode.Flare1])
            {
                if (NGUITools.GetActive(ui.GetComponent<UIReferArray>().panels[3]))
                {
                    Screen.lockCursor = true;
                    Screen.showCursor = true;
                    NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[0], state: true);
                    NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[1], state: false);
                    NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[2], state: false);
                    NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[3], state: false);
                    Camera.main.GetComponent<SpectatorMovement>().disable = false;
                    Camera.main.GetComponent<MouseLook>().disable = false;
                }
                else
                {
                    Screen.lockCursor = false;
                    Screen.showCursor = true;
                    NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[0], state: false);
                    NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[1], state: false);
                    NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[2], state: false);
                    NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[3], state: true);
                    Camera.main.GetComponent<SpectatorMovement>().disable = true;
                    Camera.main.GetComponent<MouseLook>().disable = true;
                }
            }

            if (inputManager.isInputDown[15] && !inputManager.menuOn)
            {
                Screen.showCursor = true;
                Screen.lockCursor = false;
                Camera.main.GetComponent<SpectatorMovement>().disable = true;
                Camera.main.GetComponent<MouseLook>().disable = true;
                inputManager.menuOn = true;
            }
        }

        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Stop)
        {
            return;
        }

        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer) // Multiplayer messages
        {
            CoreAdd();
            SetTextTopLeft(playerList);

            // Respawn message
            if (Camera.main != null && Level.Mode != GameMode.Racing && mainCamera.gameOver && !needChooseSide && (int)Settings[245] == 0)
            {
                SetTextCenter("Press [F7D358]" + inputManager.inputString[InputCode.Flare1] + "[-] to spectate the next player. \nPress [F7D358]" + inputManager.inputString[InputCode.Flare2] + "[-] to spectate the previous player.\nPress [F7D358]" + inputManager.inputString[InputCode.Attack1] + "[-] to enter the spectator mode.\n\n\n\n");
                if (Level.RespawnMode == RespawnMode.Deathmatch || RCSettings.EndlessMode > 0 || ((RCSettings.BombMode == 1 || RCSettings.PvPMode > 0) && RCSettings.PointMode > 0))
                {
                    myRespawnTime += Time.deltaTime;
                    int respawnDelay = 10;
                    if (GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.IsTitan]) == 2)
                    {
                        respawnDelay = 15;
                    }
                    if (RCSettings.EndlessMode > 0)
                    {
                        respawnDelay = RCSettings.EndlessMode;
                    }
                    AddTextCenter("Respawn in " + (respawnDelay - (int)myRespawnTime) + "s.");
                    if (myRespawnTime > (float)respawnDelay)
                    {
                        myRespawnTime = 0f;
                        if (GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.IsTitan]) == 2)
                        {
                            SpawnNonAITitan2(myLastHero);
                        }
                        else
                        {
                            StartCoroutine(CoWaitAndRespawn1(0.1f, myLastRespawnTag));
                        }
                        mainCamera.gameOver = false;
                        SetTextCenter(string.Empty);
                    }
                }
            }

            // Game lose messages
            if (isLosing && Level.Mode != GameMode.Racing)
            {
                if (Level.Mode == GameMode.Survival)
                {
                    SetTextCenter("Survived " + wave + " Waves!\nGame Restart in " + (int)gameEndCD + "s");
                }
                else
                {
                    SetTextCenter("Humanity Failed!\nGame Restart in " + (int)gameEndCD + "s");
                }

                if (gameEndCD <= 0f)
                {
                    gameEndCD = 0f;
                    if (PhotonNetwork.isMasterClient)
                    {
                        RestartRC();
                    }
                    SetTextCenter(string.Empty);
                }
                else
                {
                    gameEndCD -= Time.deltaTime;
                }
            }

            // Game win messages
            if (isWinning)
            {
                switch (Level.Mode)
                {
                    case GameMode.Racing:
                        SetTextCenter(localRacingResult + "\n\nGame Restart in " + (int)gameEndCD + "s");
                        break;
                    case GameMode.Survival:
                        SetTextCenter("Survived All Waves!\nGame Restart in " + (int)gameEndCD + "s");
                        break;
                    case GameMode.TeamDeathmatch:
                        if (RCSettings.PvPMode == 0 && RCSettings.BombMode == 0)
                        {
                            SetTextCenter("Team " + teamWinner + " Wins!\nGame Restart in " + (int)gameEndCD + "s");
                        }
                        else
                        {
                            SetTextCenter("Round Ended!\nGame Restart in " + (int)gameEndCD + "s");
                        }
                        break;
                    default:
                        SetTextCenter("Humanity Wins!\nGame Restart in " + (int)gameEndCD + "s");
                        break;
                }

                if (gameEndCD <= 0f)
                {
                    gameEndCD = 0f;
                    if (PhotonNetwork.isMasterClient)
                    {
                        RestartRC();
                    }
                    SetTextCenter(string.Empty);
                }
                else
                {
                    gameEndCD -= Time.deltaTime;
                }
            }

            timeTotalServer += Time.deltaTime;
        }
        else if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer) // Singleplayer messages
        {
            if (Level.Mode == GameMode.Racing)
            {
                if (!isLosing)
                {
                    currentSpeed = mainCamera.main_object.rigidbody.velocity.magnitude;
                    maxSpeed = Mathf.Max(maxSpeed, currentSpeed);
                    SetTextTopLeft("Current Speed: " + (int)currentSpeed + "\nMax Speed: " + maxSpeed);
                }
            }
            else
            {
                SetTextTopLeft("Kills: " + single_kills + "\nMax Damage: " + single_maxDamage + "\nTotal Damage: " + single_totalDamage);

                // Game lose messages
                if (isLosing)
                {
                    if (Level.Mode == GameMode.Survival)
                    {
                        SetTextCenter("Survived " + wave + " Waves!\nPress " + inputManager.inputString[InputCode.Restart] + " to restart.");
                    }
                    else
                    {
                        SetTextCenter("Humanity Fail!\nPress " + inputManager.inputString[InputCode.Restart] + " to restart.");
                    }
                }
            }

            // Game win messages
            if (isWinning)
            {
                switch (Level.Mode)
                {
                    case GameMode.Racing:
                        SetTextCenter(((timeTotalServer * 10f) * 0.1f - 5f) + "!\nPress " + inputManager.inputString[InputCode.Restart] + " to restart.");
                        break;
                    case GameMode.Survival:
                        SetTextCenter("Survived All Waves!\nPress " + inputManager.inputString[InputCode.Restart] + " to restart.");
                        break;
                    default:
                        SetTextCenter("Humanity Wins!\nPress " + inputManager.inputString[InputCode.Restart] + " to restart.");
                        break;
                }
            }

            if (Level.Mode == GameMode.Racing)
            {
                if (!isWinning)
                {
                    timeTotalServer += Time.deltaTime;
                }
            }
            else if (!isLosing && !isWinning)
            {
                timeTotalServer += Time.deltaTime;
            }
        }

        timeElapse += Time.deltaTime;
        roundTime += Time.deltaTime;

        if (Level.Mode == GameMode.Racing)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                if (!isWinning)
                {
                    SetTextTopCenter("Time: " + (timeTotalServer * 10 * 0.1f - 5f));
                }
                if (timeTotalServer < 5f)
                {
                    SetTextCenter("RACE START IN " + (5f - timeTotalServer));
                }
                else if (!startRacing)
                {
                    SetTextCenter(string.Empty);
                    startRacing = true;
                    endRacing = false;
                    GameObject.Find("door").SetActive(value: false);
                }
            }
            else
            {
                SetTextTopCenter("Time: " + (roundTime >= 20f ? (roundTime * 10f * 0.1f - 20f).ToString() : "WAITING"));
                if (roundTime < 20f)
                {
                    SetTextCenter("RACE START IN " + (20f - roundTime) + (localRacingResult.Length > 0 ? ("\nLast Round\n" + localRacingResult) : string.Empty));
                }
                else if (!startRacing)
                {
                    SetTextCenter(string.Empty);
                    startRacing = true;
                    endRacing = false;
                    GameObject gameObject = GameObject.Find("door");
                    if (gameObject != null)
                    {
                        gameObject.SetActive(value: false);
                    }
                    if (racingDoors != null && CustomLevelLoaded)
                    {
                        foreach (GameObject racingDoor in racingDoors)
                        {
                            racingDoor.SetActive(value: false);
                        }
                        racingDoors = null;
                    }
                }
                else if (racingDoors != null && CustomLevelLoaded)
                {
                    foreach (GameObject racingDoor2 in racingDoors)
                    {
                        racingDoor2.SetActive(value: false);
                    }
                    racingDoors = null;
                }
            }

            if (mainCamera.gameOver && !needChooseSide && CustomLevelLoaded)
            {
                myRespawnTime += Time.deltaTime;
                if (myRespawnTime > 1.5f)
                {
                    myRespawnTime = 0f;
                    if (checkpoint != null)
                    {
                        StartCoroutine(CoWaitAndRespawn2(0.1f, checkpoint));
                    }
                    else
                    {
                        StartCoroutine(CoWaitAndRespawn1(0.1f, myLastRespawnTag));
                    }
                    mainCamera.gameOver = false;
                    SetTextCenter(string.Empty);
                }
            }
        }

        if (timeElapse > 1f)
        {
            timeElapse -= 1f;
            string text = string.Empty;
            switch (Level.Mode)
            {
                case GameMode.Endless:
                    text = "Time: " + (int)(time - timeTotalServer);
                    break;
                case GameMode.KillTitans:
                case GameMode.None:
                    text = "Titan Left: " + AllTitans.Count
                        + " Time: " + (int)(IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer ? (time - timeTotalServer) : timeTotalServer);
                    break;
                case GameMode.Survival:
                    text = "Titan Left: " + AllTitans.Count + " Wave: " + wave;
                    break;
                case GameMode.Colossal:
                    text = "Time: " + (int)(time - timeTotalServer) + "\nDefeat the Colossal Titan\nand prevent abnormal titans from reaching the north gate!";
                    break;
                case GameMode.PvPCapture:
                    string str = "| ";
                    for (int i = 0; i < PVPcheckPoint.chkPts.Count; i++)
                    {
                        str += (PVPcheckPoint.chkPts[i] as PVPcheckPoint).GetState() + " ";
                    }
                    text = $"[{ColorSet.TitanPlayer}]{PVPtitanScoreMax - PVPtitanScore} [-]{str}| [{ColorSet.Human}]{PVPhumanScoreMax - PVPhumanScore}\n[-]Time: {(int)(time - timeTotalServer)}";
                    break;
            }

            if (RCSettings.TeamMode > 0)
            {
                text += "\n[00FFFF]Cyan: " + Convert.ToString(cyanKills) + " | [FF00FF]Magenta: " + Convert.ToString(magentaKills) + "[FFFFFF]";
            }
            SetTextTopCenter(text);
            text = string.Empty;

            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                if (Level.Mode == GameMode.Survival)
                {
                    text = "Time: " + (int)timeTotalServer;
                }
            }
            else
            {
                switch (Level.Mode)
                {
                    case GameMode.Endless:
                    case GameMode.KillTitans:
                    case GameMode.Colossal:
                    case GameMode.PvPCapture:
                        text = "Humanity " + humanScore + " | Titan " + titanScore;
                        break;
                    case GameMode.Survival:
                        text = "Time: " + (int)(time - timeTotalServer);
                        break;
                    case GameMode.TeamDeathmatch:
                        for (int i = 0; i < teamScores.Length; i++)
                        {
                            text += string.Concat(
                                (i == 0) ? string.Empty : " | ",
                                "Team",
                                i + 1,
                                " ",
                                teamScores[i]
                            );
                        }
                        text += "\nTime: " + (int)(time - timeTotalServer);
                        break;
                }
            }
            SetTextTopRight(text);

            string difficultyTxt = IN_GAME_MAIN_CAMERA.Difficulty switch
            {
                -1 => "[FFCC00]Training",
                0 => "[00FF00]Normal",
                1 => "[FFFF00]Hard",
                2 => "[FF0000]Abnormal",
                _ => "[000000]Unknown"
            };

            AddTextTopRight("\n" + Level.Name + ": " + difficultyTxt);
            AddTextTopRight("[-]\nCamera: [AAFF00]" + IN_GAME_MAIN_CAMERA.CameraMode + "[-]");

            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
            {
                string[] array = PhotonNetwork.room.name.Split('`');
                string topRightText = "\n\n" + array[0] + "\n";

                int playerCount = PhotonNetwork.room.playerCount;
                int maxPlayers = PhotonNetwork.room.maxPlayers;

                if (!PhotonNetwork.room.open || (maxPlayers != 0 && playerCount >= maxPlayers) || !PhotonNetwork.room.open)
                {
                    topRightText += "[FF4444]";
                }
                else
                {
                    topRightText += "[AAFF00]";
                }
                topRightText += $"({playerCount}/{maxPlayers})";

                if (!PhotonNetwork.room.visible)
                {
                    topRightText += " [ff6600](hidden)[-]";
                }
                AddTextTopRight(topRightText);

                if (needChooseSide)
                {
                    AddTextTopCenter("\n\nPRESS 1 TO ENTER GAME");
                }
            }

            // Display rigidbody interpolation status
            if (Guardian.Mod.Properties.Interpolation.Value)
            {
                AddTextTopCenter("\nInterpolation is [00FF00]ON[-]");
            }
            else
            {
                AddTextTopCenter("\nInterpolation is [FF0000]OFF[-]");
            }
        }

        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && killInfoGO.Count > 0 && killInfoGO[0] == null)
        {
            killInfoGO.RemoveAt(0);
        }
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || PhotonNetwork.offlineMode || !PhotonNetwork.isMasterClient || (timeTotalServer < (float)time))
        {
            return;
        }
        IN_GAME_MAIN_CAMERA.Gametype = GameType.Stop;
        gameStart = false;
        Screen.lockCursor = false;
        Screen.showCursor = true;
        string players = string.Empty;
        string kills = string.Empty;
        string deaths = string.Empty;
        string maxDamage = string.Empty;
        string totalDamage = string.Empty;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            players += player.customProperties[PhotonPlayerProperty.Name] + "\n";
            kills += player.customProperties[PhotonPlayerProperty.Kills] + "\n";
            deaths += player.customProperties[PhotonPlayerProperty.Deaths] + "\n";
            maxDamage += player.customProperties[PhotonPlayerProperty.MaxDamage] + "\n";
            totalDamage += player.customProperties[PhotonPlayerProperty.TotalDamage] + "\n";
        }
        string gameResults = string.Empty;
        switch (Level.Mode)
        {
            case GameMode.TeamDeathmatch:
                for (int i = 0; i < teamScores.Length; i++)
                {
                    gameResults += string.Concat(
                        (i == 0) ? string.Empty : " | ",
                        "Team",
                        i + 1,
                        " ",
                        teamScores[i]
                    );
                }
                break;
            case GameMode.Survival:
                gameResults = "Highest Wave: " + highestWave;
                break;
            default:
                gameResults = string.Concat(
                    "Humanity ",
                    humanScore,
                    " | Titan ",
                    titanScore
                );
                break;
        }
        base.photonView.RPC("showResult", PhotonTargets.AllBuffered, players, kills, deaths, maxDamage, totalDamage, gameResults);
    }

    public void SpawnPlayerAt2(string id, GameObject pos)
    {
        if (LogicLoaded && CustomLevelLoaded)
        {
            Vector3 position = pos.transform.position;
            if (racingSpawnPointSet)
            {
                position = racingSpawnPoint;
            }
            else if (Level.Name.StartsWith("Custom"))
            {
                if (GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCTeam]) == 0)
                {
                    List<Vector3> list = new List<Vector3>();
                    foreach (Vector3 item in playerSpawnsC)
                    {
                        list.Add(item);
                    }
                    foreach (Vector3 item2 in playerSpawnsM)
                    {
                        list.Add(item2);
                    }
                    if (list.Count > 0)
                    {
                        position = list[UnityEngine.Random.Range(0, list.Count)];
                    }
                }
                else if (GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCTeam]) == 1)
                {
                    if (playerSpawnsC.Count > 0)
                    {
                        position = playerSpawnsC[UnityEngine.Random.Range(0, playerSpawnsC.Count)];
                    }
                }
                else if (GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCTeam]) == 2 && playerSpawnsM.Count > 0)
                {
                    position = playerSpawnsM[UnityEngine.Random.Range(0, playerSpawnsM.Count)];
                }
            }

            GameObject mainCam = GameObject.Find("MainCamera");
            myLastHero = id.ToUpper();
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                if (IN_GAME_MAIN_CAMERA.SingleCharacter == "TITAN_EREN")
                {
                    mainCamera.SetMainObject((GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_EREN"), pos.transform.position, pos.transform.rotation));
                }
                else
                {
                    mainCamera.SetMainObject((GameObject)UnityEngine.Object.Instantiate(Resources.Load("AOTTG_HERO 1"), pos.transform.position, pos.transform.rotation));
                    HERO hero = mainCamera.main_object.GetComponent<HERO>();
                    HERO_SETUP setup = hero.GetComponent<HERO_SETUP>();
                    if (IN_GAME_MAIN_CAMERA.SingleCharacter == "SET 1" || IN_GAME_MAIN_CAMERA.SingleCharacter == "SET 2" || IN_GAME_MAIN_CAMERA.SingleCharacter == "SET 3")
                    {
                        HeroCostume heroCostume = CostumeConverter.FromLocalData(IN_GAME_MAIN_CAMERA.SingleCharacter);
                        CostumeConverter.ToLocalData(heroCostume, IN_GAME_MAIN_CAMERA.SingleCharacter);
                        setup.Init();
                        if (heroCostume != null)
                        {
                            setup.myCostume = heroCostume;
                            setup.myCostume.stat = heroCostume.stat;
                        }
                        else
                        {
                            heroCostume = HeroCostume.CostumeOptions[3];
                            setup.myCostume = heroCostume;
                            setup.myCostume.stat = HeroStat.GetInfo(heroCostume.name.ToUpper());
                        }
                        setup.CreateCharacterComponent();
                        hero.SetStat2();
                        hero.SetSkillHUDPosition2();
                    }
                    else
                    {
                        for (int i = 0; i < HeroCostume.Costumes.Length; i++)
                        {
                            if (HeroCostume.Costumes[i].name.ToUpper() == IN_GAME_MAIN_CAMERA.SingleCharacter.ToUpper())
                            {
                                int num = HeroCostume.Costumes[i].id + CheckBoxCostume.CostumeSet - 1;
                                if (HeroCostume.Costumes[num].name != HeroCostume.Costumes[i].name)
                                {
                                    num = HeroCostume.Costumes[i].id + 1;
                                }
                                setup.Init();
                                setup.myCostume = HeroCostume.Costumes[num];
                                setup.myCostume.stat = HeroStat.GetInfo(HeroCostume.Costumes[num].name.ToUpper());
                                setup.CreateCharacterComponent();
                                hero.SetStat2();
                                hero.SetSkillHUDPosition2();
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                mainCamera.SetMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, pos.transform.rotation, 0));
                HERO hero = mainCamera.main_object.GetComponent<HERO>();
                HERO_SETUP setup = hero.GetComponent<HERO_SETUP>();
                id = id.ToUpper();
                if (id == "SET 1" || id == "SET 2" || id == "SET 3")
                {
                    HeroCostume heroCostume2 = CostumeConverter.FromLocalData(id);
                    CostumeConverter.ToLocalData(heroCostume2, id);
                    setup.Init();
                    if (heroCostume2 != null)
                    {
                        setup.myCostume = heroCostume2;
                        setup.myCostume.stat = heroCostume2.stat;
                    }
                    else
                    {
                        heroCostume2 = HeroCostume.CostumeOptions[3];
                        setup.myCostume = heroCostume2;
                        setup.myCostume.stat = HeroStat.GetInfo(heroCostume2.name.ToUpper());
                    }
                    setup.CreateCharacterComponent();
                    hero.SetStat2();
                    hero.SetSkillHUDPosition2();
                }
                else
                {
                    for (int j = 0; j < HeroCostume.Costumes.Length; j++)
                    {
                        if (HeroCostume.Costumes[j].name.ToUpper() == id.ToUpper())
                        {
                            int num2 = HeroCostume.Costumes[j].id;
                            if (id.ToUpper() != "AHSS")
                            {
                                num2 += CheckBoxCostume.CostumeSet - 1;
                            }
                            if (HeroCostume.Costumes[num2].name != HeroCostume.Costumes[j].name)
                            {
                                num2 = HeroCostume.Costumes[j].id + 1;
                            }
                            setup.Init();
                            setup.myCostume = HeroCostume.Costumes[num2];
                            setup.myCostume.stat = HeroStat.GetInfo(HeroCostume.Costumes[num2].name.ToUpper());
                            setup.CreateCharacterComponent();
                            hero.SetStat2();
                            hero.SetSkillHUDPosition2();
                            break;
                        }
                    }
                }
                CostumeConverter.ToPhotonData(setup.myCostume, PhotonNetwork.player);
                if (Level.Mode == GameMode.PvPCapture)
                {
                    mainCamera.main_object.transform.position += new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20));
                }
                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable.Add("dead", false);
                hashtable.Add(PhotonPlayerProperty.IsTitan, 1);
                PhotonNetwork.player.SetCustomProperties(hashtable);
            }
            mainCamera.enabled = true;
            mainCamera.SetHUDPosition();
            mainCam.GetComponent<SpectatorMovement>().disable = true;
            mainCam.GetComponent<MouseLook>().disable = true;
            mainCamera.gameOver = false;
            Screen.lockCursor = IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS;
            Screen.showCursor = false;
            isLosing = false;
            SetTextCenter(string.Empty);
        }
        else
        {
            NOTSpawnPlayerRC(id);
        }
    }

    public void SpawnNonAITitan2(string id, string tag = "titanRespawn")
    {
        if (LogicLoaded && CustomLevelLoaded)
        {
            GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
            GameObject gameObject = array[UnityEngine.Random.Range(0, array.Length)];
            Vector3 position = gameObject.transform.position;
            if (Level.Name.StartsWith("Custom") && titanSpawns.Count > 0)
            {
                position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
            }
            myLastHero = id.ToUpper();
            GameObject gameObject2 = (Level.Mode != GameMode.PvPCapture) ? PhotonNetwork.Instantiate("TITAN_VER3.1", position, gameObject.transform.rotation, 0) : PhotonNetwork.Instantiate("TITAN_VER3.1", checkpoint.transform.position + new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20)), checkpoint.transform.rotation, 0);
            mainCamera.SetMainObjectTitan(gameObject2);
            gameObject2.GetComponent<TITAN>().nonAI = true;
            gameObject2.GetComponent<TITAN>().speed = 30f;
            gameObject2.GetComponent<TITAN_CONTROLLER>().enabled = true;
            if (id == "RANDOM" && UnityEngine.Random.Range(0, 100) < 7)
            {
                gameObject2.GetComponent<TITAN>().setAbnormalType2(TitanClass.Crawler, forceCrawler: true);
            }
            mainCamera.enabled = true;
            GameObject mainCam = GameObject.Find("MainCamera");
            mainCam.GetComponent<SpectatorMovement>().disable = true;
            mainCam.GetComponent<MouseLook>().disable = true;
            mainCamera.gameOver = false;
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("dead", false);
            hashtable.Add(PhotonPlayerProperty.IsTitan, 2);
            PhotonNetwork.player.SetCustomProperties(hashtable);
            Screen.lockCursor = IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS;
            Screen.showCursor = true;
            SetTextCenter(string.Empty);
        }
        else
        {
            NOTSpawnNonAITitanRC(id);
        }
    }

    public void FinishGame(bool isLoss = false)
    {
        if (isLosing || isWinning || (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer && !PhotonNetwork.isMasterClient))
        {
            return;
        }

        // BEGIN: Guardian
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && Guardian.Mod.Properties.AnnounceRoundTime.Value)
        {
            float elapsedRoundTime = (Guardian.Utilities.GameHelper.CurrentTimeMillis() - RoundStartTime) / 1000f;
            Guardian.Utilities.GameHelper.Broadcast($"This round lasted for <b>{elapsedRoundTime}</b> second(s)!");
        }
        // END: Guardian

        if (isLoss)
        {
            isLosing = true;
            titanScore++;
            gameEndCD = gameEndTotalCDtime;

            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
            {
                base.photonView.RPC("netGameLose", PhotonTargets.Others, titanScore);

                if ((int)Settings[244] == 1)
                {
                    InRoomChat.Instance.AddLine(("(" + roundTime.ToString("F2") + ") ").AsColor("FFCC00") + "Round ended (game lose).");
                }
            }
        }
        else
        {
            isWinning = true;
            humanScore++;

            int finalScore;
            switch (Level.Mode)
            {
                case GameMode.Racing:
                    finalScore = 0;
                    gameEndCD = RCSettings.RacingStatic == 1 ? 1000f : 20f;
                    break;
                case GameMode.TeamDeathmatch:
                    teamScores[teamWinner - 1]++;
                    finalScore = teamWinner;
                    gameEndCD = gameEndTotalCDtime;
                    break;
                default:
                    humanScore++;
                    finalScore = humanScore;
                    gameEndCD = gameEndTotalCDtime;
                    break;
            }

            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
            {
                base.photonView.RPC("netGameWin", PhotonTargets.Others, finalScore);

                if ((int)Settings[244] == 1)
                {
                    InRoomChat.Instance.AddLine(("(" + roundTime.ToString("F2") + ") ").AsColor("FFCC00") + "Round ended (game win).");
                }
            }
        }
    }

    public bool AreAllPlayersDead()
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            if (!photonPlayer.IsTitan && !photonPlayer.IsDead)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsTeamDead(int team)
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            if (!photonPlayer.IsTitan && photonPlayer.Team == team && !photonPlayer.IsDead)
            {
                return false;
            }
        }
        return true;
    }

    private void RefreshRacingResult()
    {
        localRacingResult = "Result\n";
        IComparer comparer = new RacingResultComparer();
        racingResult.Sort(comparer);

        int num = Mathf.Min(racingResult.Count, 10);
        for (int i = 0; i < num; i++)
        {
            RacingResult currentRacingResult = racingResult[i] as RacingResult;
            localRacingResult += "[FFFFFF]#" + (i + 1) + ": "
                + currentRacingResult.name + " - "
                + (currentRacingResult.time * 100f * 0.01f) + "s\n";
        }

        base.photonView.RPC("netRefreshRacingResult", PhotonTargets.All, localRacingResult);
    }

    public void RestartGameSingle()
    {
        startRacing = false;
        endRacing = false;
        checkpoint = null;
        single_kills = 0;
        single_maxDamage = 0;
        single_totalDamage = 0;
        timeElapse = 0f;
        roundTime = 0f;
        timeTotalServer = 0f;
        isWinning = false;
        isLosing = false;
        wave = 1;
        myRespawnTime = 0f;
        SetTextCenter(string.Empty);
        DestroyAllExistingCloths();
        Application.LoadLevel(Application.loadedLevel);
    }

    public IEnumerator CoWaitAndRespawn1(float time, string str)
    {
        yield return new WaitForSeconds(time);
        SpawnPlayer(myLastHero, str);
    }

    public IEnumerator CoWaitAndRespawn2(float time, GameObject pos)
    {
        yield return new WaitForSeconds(time);
        SpawnPlayerAt2(myLastHero, pos);
    }

    public void DestroyAllExistingCloths()
    {
        foreach (Cloth cloth in UnityEngine.Object.FindObjectsOfType<Cloth>())
        {
            ClothFactory.DisposeObject(cloth.gameObject);
        }
    }

    public IEnumerator CoWaitAndResetRestarts()
    {
        yield return new WaitForSeconds(10f);
        restartingBomb = false;
        restartingEren = false;
        restartingHorse = false;
        restartingMC = false;
        restartingTitan = false;
    }

    public IEnumerator CoWaitAndReloadKD(PhotonPlayer player)
    {
        yield return new WaitForSeconds(5f);
        string name = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]);

        if (PreservedPlayerKDR.ContainsKey(name))
        {
            int[] array = PreservedPlayerKDR[name];
            PreservedPlayerKDR.Remove(name);
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.Kills, array[0]);
            hashtable.Add(PhotonPlayerProperty.Deaths, array[1]);
            hashtable.Add(PhotonPlayerProperty.MaxDamage, array[2]);
            hashtable.Add(PhotonPlayerProperty.TotalDamage, array[3]);
            player.SetCustomProperties(hashtable);
        }
    }

    public void EnterSpecMode(bool enter)
    {
        if (enter)
        {
            spectateSprites = new List<GameObject>();

            foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (!(gameObject.GetComponent<UISprite>() != null) || !gameObject.activeInHierarchy)
                {
                    continue;
                }
                string text = gameObject.name;
                if (text.Contains("blade") || text.Contains("bullet") || text.Contains("gas") || text.Contains("flare") || text.Contains("skill_cd"))
                {
                    if (!spectateSprites.Contains(gameObject))
                    {
                        spectateSprites.Add(gameObject);
                    }
                    gameObject.SetActive(value: false);
                }
            }
            string[] array2 = new string[2]
            {
                "Flare",
                "LabelInfoBottomRight"
            };
            foreach (string text2 in array2)
            {
                GameObject gameObject2 = GameObject.Find(text2);
                if (gameObject2 != null)
                {
                    if (!spectateSprites.Contains(gameObject2))
                    {
                        spectateSprites.Add(gameObject2);
                    }
                    gameObject2.SetActive(value: false);
                }
            }
            foreach (HERO player in heroes)
            {
                if (player.photonView.isMine)
                {
                    PhotonNetwork.Destroy(player.photonView);
                }
            }
            if (GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.IsTitan]) == 2 && !GExtensions.AsBool(PhotonNetwork.player.customProperties[PhotonPlayerProperty.IsDead]))
            {
                foreach (TITAN titan in titans)
                {
                    if (titan.photonView.isMine && titan.nonAI)
                    {
                        PhotonNetwork.Destroy(titan.photonView);
                    }
                }
            }
            NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[1], state: false);
            NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[2], state: false);
            NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[3], state: false);
            needChooseSide = false;
            mainCamera.enabled = true;
            if (IN_GAME_MAIN_CAMERA.CameraMode == CameraType.Original)
            {
                Screen.lockCursor = false;
                Screen.showCursor = false;
            }
            GameObject gameObject3 = GameObject.FindGameObjectWithTag("Player");
            if (gameObject3 != null && gameObject3.GetComponent<HERO>() != null)
            {
                mainCamera.SetMainObject(gameObject3);
            }
            else
            {
                mainCamera.SetMainObject(null);
            }
            mainCamera.SetSpectorMode(val: false);
            mainCamera.gameOver = true;

            Screen.lockCursor = !Screen.lockCursor;
            Screen.lockCursor = !Screen.lockCursor;
        }
        else
        {
            if (GameObject.Find("cross1") != null)
            {
                GameObject.Find("cross1").transform.localPosition = Vector3.up * 5000f;
            }
            if (spectateSprites != null)
            {
                foreach (GameObject spectateSprite in spectateSprites)
                {
                    if (spectateSprite != null)
                    {
                        spectateSprite.SetActive(value: true);
                    }
                }
            }
            spectateSprites = new List<GameObject>();
            NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[1], state: false);
            NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[2], state: false);
            NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[3], state: false);
            needChooseSide = true;
            mainCamera.SetMainObject(null);
            mainCamera.SetSpectorMode(val: true);
            mainCamera.gameOver = true;
        }
    }

    public void RecompilePlayerList(float time)
    {
        if (!isRecompiling)
        {
            isRecompiling = true;
            StartCoroutine(CoWaitAndRecompilePlayerList(time));
        }
    }

    private string GetPlayerTextForList(PhotonPlayer player)
    {
        string content = "[FFFFFF]";

        if (IgnoreList.Contains(player.Id))
        {
            content += "[990000]X[-] ";
        }
        if (player.isMasterClient)
        {
            content += "[AAFF00]";
        }
        else if (player.isLocal)
        {
            content += "[0099FF]";
        }
        else
        {
            content += "[FFCC00]";
        }
        content += player.Id + " ";

        bool dead = false;
        if (player.customProperties[PhotonPlayerProperty.IsDead] == null)
        {
            content += player.Id < 0 ? "[FFCC00](Joining) " : "[FF0000](Invis) ";
        }
        else
        {
            dead = GExtensions.AsBool(player.customProperties[PhotonPlayerProperty.IsDead]);
        }

        if (GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.IsTitan]) == 2)
        {
            content += "[" + (dead ? ColorSet.Red : ColorSet.TitanPlayer) + "][T] ";
        }
        else if (GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Team]) == 2)
        {
            content += "[" + (dead ? ColorSet.Red : ColorSet.AHSS) + "][A] ";
        }
        else
        {
            content += "[" + (dead ? ColorSet.Red : ColorSet.Human) + "][H] ";
        }

        content += "[FFFFFF]" + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]);

        int kills = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Kills]);
        int deaths = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Deaths]);
        int maxDmg = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.MaxDamage]);
        int totalDmg = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.TotalDamage]);
        content += string.Concat(
            " [AAAAAA]:[FFFFFF] ",
            kills,
            " / ",
            deaths,
            " / ",
            maxDmg,
            " / ",
            totalDmg
        );

        if (Guardian.Mod.Properties.ShowPlayerMods.Value)
        {
            List<string> detectedMods = Guardian.AntiAbuse.ModDetector.GetMods(player);
            if (detectedMods.Count > 0)
            {
                content += " " + detectedMods[0];
            }
        }

        if (Guardian.Mod.Properties.ShowPlayerPings.Value && player.Ping >= 0)
        {
            content += " [FFFFFF][" + player.Ping + "ms]";
        }

        return content + '\n';
    }

    public IEnumerator CoWaitAndRecompilePlayerList(float time)
    {
        yield return new WaitForSeconds(time);

        string content = string.Empty;
        Comparison<PhotonPlayer> comparator = new Comparison<PhotonPlayer>((p1, p2) => p1.Id - p2.Id);
        PhotonPlayer[] array = PhotonNetwork.playerList.Sorted(comparator);
        if (RCSettings.TeamMode != 0)
        {
            int _cyanKills = 0;
            int _magentaKills = 0;
            int _cyanDeaths = 0;
            int _magentaDeaths = 0;
            int _cyanMaxDmg = 0;
            int _magentaMaxDmg = 0;
            int _cyanDmgSum = 0;
            int _magentaDmgSum = 0;
            Dictionary<int, PhotonPlayer> cyanPlayers = new Dictionary<int, PhotonPlayer>();
            Dictionary<int, PhotonPlayer> magentaPlayers = new Dictionary<int, PhotonPlayer>();
            Dictionary<int, PhotonPlayer> individualPlayers = new Dictionary<int, PhotonPlayer>();
            foreach (PhotonPlayer photonPlayer in array)
            {
                if (photonPlayer.customProperties[PhotonPlayerProperty.IsDead] != null && !IgnoreList.Contains(photonPlayer.Id))
                {
                    switch (GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.RCTeam]))
                    {
                        case 0:
                            individualPlayers.Add(photonPlayer.Id, photonPlayer);
                            break;
                        case 1:
                            cyanPlayers.Add(photonPlayer.Id, photonPlayer);
                            _cyanKills += GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.Kills]);
                            _cyanDeaths += GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.Deaths]);
                            _cyanMaxDmg += GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.MaxDamage]);
                            _cyanDmgSum += GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.TotalDamage]);
                            break;
                        case 2:
                            magentaPlayers.Add(photonPlayer.Id, photonPlayer);
                            _magentaKills += GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.Kills]);
                            _magentaDeaths += GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.Deaths]);
                            _magentaMaxDmg += GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.MaxDamage]);
                            _magentaDmgSum += GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.TotalDamage]);
                            break;
                    }
                }
            }
            cyanKills = _cyanKills;
            magentaKills = _magentaKills;
            if (PhotonNetwork.isMasterClient)
            {
                if (RCSettings.TeamMode == 2)
                {
                    foreach (PhotonPlayer player in array)
                    {
                        int rcTeam = 0;
                        if (cyanPlayers.Count > magentaPlayers.Count + 1)
                        {
                            rcTeam = 2;
                            if (cyanPlayers.ContainsKey(player.Id))
                            {
                                cyanPlayers.Remove(player.Id);
                            }
                            if (!magentaPlayers.ContainsKey(player.Id))
                            {
                                magentaPlayers.Add(player.Id, player);
                            }
                        }
                        else if (magentaPlayers.Count > cyanPlayers.Count + 1)
                        {
                            rcTeam = 1;
                            if (!cyanPlayers.ContainsKey(player.Id))
                            {
                                cyanPlayers.Add(player.Id, player);
                            }
                            if (magentaPlayers.ContainsKey(player.Id))
                            {
                                magentaPlayers.Remove(player.Id);
                            }
                        }
                        if (rcTeam > 0)
                        {
                            base.photonView.RPC("setTeamRPC", player, rcTeam);
                        }
                    }
                }
                else if (RCSettings.TeamMode == 3)
                {
                    foreach (PhotonPlayer player in array)
                    {
                        int rcTeam = 0;
                        int team = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.RCTeam]);
                        if (team <= 0)
                        {
                            continue;
                        }
                        switch (team)
                        {
                            case 1:
                                {
                                    int kills = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Kills]);
                                    if (_magentaKills + kills + 7 < _cyanKills - kills)
                                    {
                                        rcTeam = 2;
                                        _magentaKills += kills;
                                        _cyanKills -= kills;
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    int kills = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Kills]);
                                    if (_cyanKills + kills + 7 < _magentaKills - kills)
                                    {
                                        rcTeam = 1;
                                        _cyanKills += kills;
                                        _magentaKills -= kills;
                                    }
                                    break;
                                }
                        }
                        if (rcTeam > 0)
                        {
                            base.photonView.RPC("setTeamRPC", player, rcTeam);
                        }
                    }
                }
            }
            content += "[00FFFF]TEAM CYAN[FFFFFF]: " + cyanKills + " [AAAAAA]/[-] " + _cyanDeaths + " [AAAAAA]/[-] " + _cyanMaxDmg + " [AAAAAA]/[-] " + _cyanDmgSum + "\n";
            foreach (PhotonPlayer player in cyanPlayers.Values)
            {
                int team = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.RCTeam]);
                if (team == 1)
                {
                    content += GetPlayerTextForList(player);
                }
            }

            content += " \n[FF00FF]TEAM MAGENTA[FFFFFF]: " + magentaKills + " [AAAAAA]/[-] " + _magentaDeaths + " [AAAAAA]/[-] " + _magentaMaxDmg + " [AAAAAA]/[-] " + _magentaDmgSum + "\n";
            foreach (PhotonPlayer player in magentaPlayers.Values)
            {
                int team = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.RCTeam]);
                if (team == 2)
                {
                    content += GetPlayerTextForList(player);
                }
            }

            content += " \n[00FF00]INDIVIDUAL\n";
            foreach (PhotonPlayer player in individualPlayers.Values)
            {
                int team = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.RCTeam]);
                if (team == 0)
                {
                    content += GetPlayerTextForList(player);
                }
            }
        }
        else
        {
            content += GetPlayerTextForList(PhotonNetwork.player);
            PhotonPlayer[] array2 = PhotonNetwork.otherPlayers.Sorted(comparator);
            foreach (PhotonPlayer player in array2)
            {
                content += GetPlayerTextForList(player);
            }
        }
        playerList = content;

        if (PhotonNetwork.isMasterClient && !isWinning && !isLosing && roundTime >= 5f)
        {
            if (RCSettings.InfectionMode > 0)
            {
                int num19 = 0;
                for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                {
                    PhotonPlayer player = PhotonNetwork.playerList[j];
                    if (IgnoreList.Contains(player.Id) || player.customProperties[PhotonPlayerProperty.IsDead] == null || player.customProperties[PhotonPlayerProperty.IsTitan] == null)
                    {
                        continue;
                    }
                    if (GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.IsTitan]) == 1)
                    {
                        if (GExtensions.AsBool(player.customProperties[PhotonPlayerProperty.IsDead]) && GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Deaths]) > 0)
                        {
                            if (!ImATitan.ContainsKey(player.Id))
                            {
                                ImATitan.Add(player.Id, 2);
                            }
                            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                            hashtable.Add(PhotonPlayerProperty.IsTitan, 2);
                            player.SetCustomProperties(hashtable);
                            base.photonView.RPC("spawnTitanRPC", player);
                        }
                        else
                        {
                            if (!ImATitan.ContainsKey(player.Id))
                            {
                                continue;
                            }

                            foreach (HERO hero in heroes)
                            {
                                if (hero.photonView.owner == player)
                                {
                                    hero.MarkDead();
                                    hero.photonView.RPC("netDie2", PhotonTargets.All, -1, "No Switching."); // noswitchingfagt
                                }
                            }
                        }
                    }
                    else if (GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.IsTitan]) == 2 && !GExtensions.AsBool(player.customProperties[PhotonPlayerProperty.IsDead]))
                    {
                        num19++;
                    }
                }
                if (num19 <= 0 && Level.Mode != 0)
                {
                    FinishGame();
                }
            }
            else if (RCSettings.PointMode > 0)
            {
                if (RCSettings.TeamMode > 0)
                {
                    if (cyanKills >= RCSettings.PointMode)
                    {
                        base.photonView.RPC("Chat", PhotonTargets.All, "Team Cyan wins!".AsColor("00FFFF"), string.Empty);
                        FinishGame();
                    }
                    else if (magentaKills >= RCSettings.PointMode)
                    {
                        base.photonView.RPC("Chat", PhotonTargets.All, "Team Magenta wins!".AsColor("FF00FF"), string.Empty);
                        FinishGame();
                    }
                }
                else if (RCSettings.TeamMode == 0)
                {
                    for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                    {
                        PhotonPlayer photonPlayer6 = PhotonNetwork.playerList[j];
                        if (GExtensions.AsInt(photonPlayer6.customProperties[PhotonPlayerProperty.Kills]) >= RCSettings.PointMode)
                        {
                            base.photonView.RPC("Chat", PhotonTargets.All, GExtensions.AsString(photonPlayer6.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity() + " wins!".AsColor("FFCC00"), string.Empty);
                            FinishGame();
                        }
                    }
                }
            }
            else if (RCSettings.PointMode <= 0 && (RCSettings.BombMode == 1 || RCSettings.PvPMode > 0))
            {
                if (RCSettings.TeamMode > 0 && PhotonNetwork.playerList.Length > 1)
                {
                    int num20 = 0;
                    int num21 = 0;
                    int num22 = 0;
                    int num23 = 0;
                    for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                    {
                        PhotonPlayer photonPlayer7 = PhotonNetwork.playerList[j];
                        if (IgnoreList.Contains(photonPlayer7.Id) || photonPlayer7.customProperties[PhotonPlayerProperty.RCTeam] == null || photonPlayer7.customProperties[PhotonPlayerProperty.IsDead] == null)
                        {
                            continue;
                        }
                        if (GExtensions.AsInt(photonPlayer7.customProperties[PhotonPlayerProperty.RCTeam]) == 1)
                        {
                            num22++;
                            if (!GExtensions.AsBool(photonPlayer7.customProperties[PhotonPlayerProperty.IsDead]))
                            {
                                num20++;
                            }
                        }
                        else if (GExtensions.AsInt(photonPlayer7.customProperties[PhotonPlayerProperty.RCTeam]) == 2)
                        {
                            num23++;
                            if (!GExtensions.AsBool(photonPlayer7.customProperties[PhotonPlayerProperty.IsDead]))
                            {
                                num21++;
                            }
                        }
                    }
                    if (num22 > 0 && num23 > 0)
                    {
                        if (num20 == 0)
                        {
                            base.photonView.RPC("Chat", PhotonTargets.All, "Team Magenta wins!".AsColor("FF00FF"), string.Empty);
                            FinishGame();
                        }
                        else if (num21 == 0)
                        {
                            base.photonView.RPC("Chat", PhotonTargets.All, "Team Cyan wins!".AsColor("00FFFF"), string.Empty);
                            FinishGame();
                        }
                    }
                }
                else if (RCSettings.TeamMode == 0 && PhotonNetwork.playerList.Length > 1)
                {
                    int num24 = 0;
                    string winnerName = "Nobody";
                    PhotonPlayer player = PhotonNetwork.playerList[0];
                    for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                    {
                        PhotonPlayer photonPlayer8 = PhotonNetwork.playerList[j];
                        if (photonPlayer8.customProperties[PhotonPlayerProperty.IsDead] != null && !GExtensions.AsBool(photonPlayer8.customProperties[PhotonPlayerProperty.IsDead]))
                        {
                            winnerName = GExtensions.AsString(photonPlayer8.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity();
                            player = photonPlayer8;
                            num24++;
                        }
                    }
                    if (num24 <= 1)
                    {
                        base.photonView.RPC("Chat", PhotonTargets.All, winnerName.NGUIToUnity() + " wins.".AsColor("FFCC00"), string.Empty);
                        FinishGame();
                    }
                }
            }
        }
        isRecompiling = false;
    }

    public void UnloadAssets()
    {
        if (!isUnloading)
        {
            isUnloading = true;
            StartCoroutine(CoWaitAndUnloadAssets(10f));
        }
    }

    public void UnloadAssetsEditor()
    {
        if (!isUnloading)
        {
            isUnloading = true;
            StartCoroutine(CoWaitAndUnloadAssets(30f));
        }
    }

    public IEnumerator CoWaitAndUnloadAssets(float time)
    {
        yield return new WaitForSeconds(time);
        Resources.UnloadUnusedAssets();
        isUnloading = false;
    }

    public Texture2D LoadTextureRC(string tex)
    {
        if (assetCacheTextures == null)
        {
            assetCacheTextures = new Dictionary<string, Texture2D>();
        }
        if (assetCacheTextures.ContainsKey(tex))
        {
            return assetCacheTextures[tex];
        }
        Texture2D texture2D = (Texture2D)RCAssets.Load(tex);
        assetCacheTextures.Add(tex, texture2D);
        return texture2D;
    }

    [RPC]
    public void verifyPlayerHasLeft(int id, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            PhotonPlayer player = PhotonPlayer.Find(id);
            if (player != null)
            {
                string playerName = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]);
                BanHash.Add(id, playerName);
            }
        }
    }

    public static void ServerRequestAuthentication(string authPassword)
    {
        if (!string.IsNullOrEmpty(authPassword))
        {
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable[(byte)0] = authPassword;
            PhotonNetwork.RaiseEvent(198, hashtable, sendReliable: true, new RaiseEventOptions());
        }
    }

    public static void ServerCloseConnection(PhotonPlayer targetPlayer, bool requestIpBan, string inGameName = null)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            TargetActors = new int[] { targetPlayer.Id }
        };
        if (requestIpBan)
        {
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable[(byte)0] = true;
            if (inGameName != null && inGameName.Length > 0)
            {
                hashtable[(byte)1] = inGameName;
            }
            PhotonNetwork.RaiseEvent(203, hashtable, sendReliable: true, raiseEventOptions);
        }
        else
        {
            PhotonNetwork.RaiseEvent(203, null, sendReliable: true, raiseEventOptions);
        }
    }

    public static void ServerRequestUnban(string bannedAddress)
    {
        if (!string.IsNullOrEmpty(bannedAddress))
        {
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable[(byte)0] = bannedAddress;
            PhotonNetwork.RaiseEvent(199, hashtable, sendReliable: true, new RaiseEventOptions());
        }
    }

    private ExitGames.Client.Photon.Hashtable CheckGameGUI()
    {
        ExitGames.Client.Photon.Hashtable gameSettings = new ExitGames.Client.Photon.Hashtable();
        if ((int)Settings[200] > 0)
        {
            Settings[192] = 0;
            Settings[193] = 0;
            Settings[226] = 0;
            Settings[220] = 0;
            int result = 1;
            if (!int.TryParse((string)Settings[201], out result) || result > PhotonNetwork.countOfPlayers || result < 0)
            {
                Settings[201] = "1";
            }
            gameSettings.Add("infection", result);
            if (RCSettings.InfectionMode != result)
            {
                ImATitan.Clear();
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
                    ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
                    hashtable2.Add(PhotonPlayerProperty.IsTitan, 1);
                    photonPlayer.SetCustomProperties(hashtable2);
                }
                int num = PhotonNetwork.playerList.Length;
                int num2 = result;
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    PhotonPlayer photonPlayer2 = PhotonNetwork.playerList[i];
                    if (num > 0 && UnityEngine.Random.Range(0f, 1f) <= (float)num2 / (float)num)
                    {
                        ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
                        hashtable3.Add(PhotonPlayerProperty.IsTitan, 2);
                        photonPlayer2.SetCustomProperties(hashtable3);
                        ImATitan.Add(photonPlayer2.Id, 2);
                        num2--;
                    }
                    num--;
                }
            }
        }
        if ((int)Settings[192] > 0)
        {
            gameSettings.Add("bomb", (int)Settings[192]);
        }
        if ((int)Settings[235] > 0)
        {
            gameSettings.Add("globalDisableMinimap", (int)Settings[235]);
        }
        if ((int)Settings[193] > 0)
        {
            gameSettings.Add("team", (int)Settings[193]);
            if (RCSettings.TeamMode != (int)Settings[193])
            {
                int num2 = 1;
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
                    switch (num2)
                    {
                        case 1:
                            base.photonView.RPC("setTeamRPC", photonPlayer, 1);
                            num2 = 2;
                            break;
                        case 2:
                            base.photonView.RPC("setTeamRPC", photonPlayer, 2);
                            num2 = 1;
                            break;
                    }
                }
            }
        }
        if ((int)Settings[226] > 0)
        {
            int maxPoints = 50;
            if (!int.TryParse((string)Settings[227], out maxPoints) || maxPoints > 1000 || maxPoints < 0)
            {
                Settings[227] = "50";
            }
            gameSettings.Add("point", maxPoints);
        }
        if ((int)Settings[194] > 0)
        {
            gameSettings.Add("rock", (int)Settings[194]);
        }
        if ((int)Settings[195] > 0)
        {
            int explodeRadius = 30;
            if (!int.TryParse((string)Settings[196], out explodeRadius) || explodeRadius > 100 || explodeRadius < 0)
            {
                Settings[196] = "30";
            }
            gameSettings.Add("explode", explodeRadius);
        }
        if ((int)Settings[197] > 0)
        {
            int minHealth = 100;
            int maxHealth = 200;
            if (!int.TryParse((string)Settings[198], out minHealth) || minHealth > 100000 || minHealth < 0)
            {
                Settings[198] = "100";
            }
            if (!int.TryParse((string)Settings[199], out maxHealth) || maxHealth > 100000 || maxHealth < 0)
            {
                Settings[199] = "200";
            }
            gameSettings.Add("healthMode", (int)Settings[197]);
            gameSettings.Add("healthLower", minHealth);
            gameSettings.Add("healthUpper", maxHealth);
        }
        if ((int)Settings[202] > 0)
        {
            gameSettings.Add("eren", (int)Settings[202]);
        }
        if ((int)Settings[203] > 0)
        {
            int titanCount = 1;
            if (!int.TryParse((string)Settings[204], out titanCount) || titanCount > 50 || titanCount < 0)
            {
                Settings[204] = "1";
            }
            gameSettings.Add("titanc", titanCount);
        }
        if ((int)Settings[205] > 0)
        {
            int minDamage = 1000;
            if (!int.TryParse((string)Settings[206], out minDamage) || minDamage > 100000 || minDamage < 0)
            {
                Settings[206] = "1000";
            }
            gameSettings.Add("damage", minDamage);
        }
        if ((int)Settings[207] > 0)
        {
            float minSize = 1f;
            float maxSize = 3f;
            if (!float.TryParse((string)Settings[208], out minSize) || !(minSize <= 100f) || !(minSize >= 0f))
            {
                Settings[208] = "1.0";
            }
            if (!float.TryParse((string)Settings[209], out maxSize) || !(maxSize <= 100f) || !(maxSize >= 0f))
            {
                Settings[209] = "3.0";
            }
            gameSettings.Add("sizeMode", (int)Settings[207]);
            gameSettings.Add("sizeLower", minSize);
            gameSettings.Add("sizeUpper", maxSize);
        }
        if ((int)Settings[210] > 0)
        {
            float normalRate = 20f;
            float abbyRate = 20f;
            float jumperRate = 20f;
            float crawlerRate = 20f;
            float punkRate = 20f;
            if (!float.TryParse((string)Settings[211], out normalRate) || !(normalRate >= 0f))
            {
                Settings[211] = "20.0";
            }
            if (!float.TryParse((string)Settings[212], out abbyRate) || !(abbyRate >= 0f))
            {
                Settings[212] = "20.0";
            }
            if (!float.TryParse((string)Settings[213], out jumperRate) || !(jumperRate >= 0f))
            {
                Settings[213] = "20.0";
            }
            if (!float.TryParse((string)Settings[214], out crawlerRate) || !(crawlerRate >= 0f))
            {
                Settings[214] = "20.0";
            }
            if (!float.TryParse((string)Settings[215], out punkRate) || !(punkRate >= 0f))
            {
                Settings[215] = "20.0";
            }
            if (normalRate + abbyRate + jumperRate + crawlerRate + punkRate > 100f)
            {
                Settings[211] = "20.0";
                Settings[212] = "20.0";
                Settings[213] = "20.0";
                Settings[214] = "20.0";
                Settings[215] = "20.0";
                normalRate = 20f;
                abbyRate = 20f;
                jumperRate = 20f;
                crawlerRate = 20f;
                punkRate = 20f;
            }
            gameSettings.Add("spawnMode", (int)Settings[210]);
            gameSettings.Add("nRate", normalRate);
            gameSettings.Add("aRate", abbyRate);
            gameSettings.Add("jRate", jumperRate);
            gameSettings.Add("cRate", crawlerRate);
            gameSettings.Add("pRate", punkRate);
        }
        if ((int)Settings[216] > 0)
        {
            gameSettings.Add("horse", (int)Settings[216]);
        }
        if ((int)Settings[217] > 0)
        {
            int result = 1;
            if (!int.TryParse((string)Settings[218], out result) || result > 50)
            {
                Settings[218] = "1";
            }
            gameSettings.Add("waveModeOn", (int)Settings[217]);
            gameSettings.Add("waveModeNum", result);
        }
        if ((int)Settings[219] > 0)
        {
            gameSettings.Add("friendly", (int)Settings[219]);
        }
        if ((int)Settings[220] > 0)
        {
            gameSettings.Add("pvp", (int)Settings[220]);
        }
        if ((int)Settings[221] > 0)
        {
            int maxWaves = 20;
            if (!int.TryParse((string)Settings[222], out maxWaves) || maxWaves > 1000000 || maxWaves < 0)
            {
                Settings[222] = "20";
            }
            gameSettings.Add("maxwave", maxWaves);
        }
        if ((int)Settings[223] > 0)
        {
            int respawnTime = 5;
            if (!int.TryParse((string)Settings[224], out respawnTime) || respawnTime > 1000000 || respawnTime < 5)
            {
                Settings[224] = "5";
            }
            gameSettings.Add("endless", respawnTime);
        }
        if ((string)Settings[225] != string.Empty)
        {
            gameSettings.Add("motd", (string)Settings[225]);
        }
        if ((int)Settings[228] > 0)
        {
            gameSettings.Add("ahssReload", (int)Settings[228]);
        }
        if ((int)Settings[229] > 0)
        {
            gameSettings.Add("punkWaves", (int)Settings[229]);
        }
        if ((int)Settings[261] > 0)
        {
            gameSettings.Add("deadlycannons", (int)Settings[261]);
        }
        if (RCSettings.RacingStatic > 0)
        {
            gameSettings.Add("asoracing", 1);
        }

        return gameSettings;
    }

    private IEnumerator CoLogin()
    {
        WWWForm myForm = new WWWForm();
        myForm.AddField("userid", UsernameField);
        myForm.AddField("password", PasswordField);

        using (WWW www = new WWW("http://fenglee.com/game/aog/require_user_info.php", myForm))
        {
            yield return www;
            if (www.error == null && !www.text.Contains("Error,please sign in again."))
            {
                string[] array = www.text.Split('|');
                LoginFengKAI.Player.Name = UsernameField;
                LoginFengKAI.Player.Guild = array[0];
                LoginFengKAI.LoginState = LoginState.LoggedIn;
            }
            else
            {
                LoginFengKAI.LoginState = LoginState.Failed;
            }
        }
    }

    private IEnumerator CoSetGuild()
    {
        WWWForm myForm = new WWWForm();
        myForm.AddField("name", LoginFengKAI.Player.Name);
        myForm.AddField("guildname", LoginFengKAI.Player.Guild);

        using (WWW www = new WWW("http://fenglee.com/game/aog/change_guild_name.php", myForm))
        {
            yield return www;
            if (www.error != null)
            {
                print(www.error);
            }
        }
    }

    public void CompileScript(string str)
    {
        string[] array = str.Replace(" ", string.Empty).Split(new string[2]
        {
            "\n",
            "\r\n"
        }, StringSplitOptions.RemoveEmptyEntries);
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        int num = 0;
        int num2 = 0;
        bool flag = false;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == "{")
            {
                num++;
                continue;
            }
            if (array[i] == "}")
            {
                num2++;
                continue;
            }
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            string text = array[i];
            for (int j = 0; j < text.Length; j++)
            {
                switch (text[j])
                {
                    case '(':
                        num3++;
                        break;
                    case ')':
                        num4++;
                        break;
                    case '"':
                        num5++;
                        break;
                }
            }
            if (num3 != num4)
            {
                InRoomChat.Instance.AddLine("Script Error: Parentheses not equal! (line " + (i + 1).ToString() + ")");
                flag = true;
            }
            if (num5 % 2 != 0)
            {
                InRoomChat.Instance.AddLine("Script Error: Quotations not equal! (line " + (i + 1).ToString() + ")");
                flag = true;
            }
        }
        if (num != num2)
        {
            InRoomChat.Instance.AddLine("Script Error: Bracket count not equivalent!");
            flag = true;
        }
        if (!flag)
        {
            try
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].StartsWith("On") && array[i + 1] == "{")
                    {
                        int num6 = i;
                        int num7 = i + 2;
                        int num8 = 0;
                        for (int k = i + 2; k < array.Length; k++)
                        {
                            if (array[k] == "{")
                            {
                                num8++;
                            }
                            if (array[k] == "}")
                            {
                                if (num8 > 0)
                                {
                                    num8--;
                                }
                                else
                                {
                                    num7 = k - 1;
                                    k = array.Length;
                                }
                            }
                        }
                        hashtable.Add(num6, num7);
                        i = num7;
                    }
                }
                foreach (int key in hashtable.Keys)
                {
                    string text2 = array[key];
                    int num7 = (int)hashtable[key];
                    string[] array2 = new string[num7 - key + 1];
                    int num9 = 0;
                    for (int i = key; i <= num7; i++)
                    {
                        array2[num9] = array[i];
                        num9++;
                    }
                    RCEvent rCEvent = ParseBlock(array2, 0, 0, null);
                    if (text2.StartsWith("OnPlayerEnterRegion"))
                    {
                        int num10 = text2.IndexOf('[');
                        int num11 = text2.IndexOf(']');
                        string text3 = text2.Substring(num10 + 2, num11 - num10 - 3);
                        num10 = text2.IndexOf('(');
                        num11 = text2.IndexOf(')');
                        string value = text2.Substring(num10 + 2, num11 - num10 - 3);
                        if (RCRegionTriggers.ContainsKey(text3))
                        {
                            RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
                            regionTrigger.playerEventEnter = rCEvent;
                            regionTrigger.myName = text3;
                            RCRegionTriggers[text3] = regionTrigger;
                        }
                        else
                        {
                            RegionTrigger regionTrigger = new RegionTrigger();
                            regionTrigger.playerEventEnter = rCEvent;
                            regionTrigger.myName = text3;
                            RCRegionTriggers.Add(text3, regionTrigger);
                        }
                        RCVariableNames.Add("OnPlayerEnterRegion[" + text3 + "]", value);
                    }
                    else if (text2.StartsWith("OnPlayerLeaveRegion"))
                    {
                        int num10 = text2.IndexOf('[');
                        int num11 = text2.IndexOf(']');
                        string text3 = text2.Substring(num10 + 2, num11 - num10 - 3);
                        num10 = text2.IndexOf('(');
                        num11 = text2.IndexOf(')');
                        string value = text2.Substring(num10 + 2, num11 - num10 - 3);
                        if (RCRegionTriggers.ContainsKey(text3))
                        {
                            RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
                            regionTrigger.playerEventExit = rCEvent;
                            regionTrigger.myName = text3;
                            RCRegionTriggers[text3] = regionTrigger;
                        }
                        else
                        {
                            RegionTrigger regionTrigger = new RegionTrigger();
                            regionTrigger.playerEventExit = rCEvent;
                            regionTrigger.myName = text3;
                            RCRegionTriggers.Add(text3, regionTrigger);
                        }
                        RCVariableNames.Add("OnPlayerExitRegion[" + text3 + "]", value);
                    }
                    else if (text2.StartsWith("OnTitanEnterRegion"))
                    {
                        int num10 = text2.IndexOf('[');
                        int num11 = text2.IndexOf(']');
                        string text3 = text2.Substring(num10 + 2, num11 - num10 - 3);
                        num10 = text2.IndexOf('(');
                        num11 = text2.IndexOf(')');
                        string value = text2.Substring(num10 + 2, num11 - num10 - 3);
                        if (RCRegionTriggers.ContainsKey(text3))
                        {
                            RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
                            regionTrigger.titanEventEnter = rCEvent;
                            regionTrigger.myName = text3;
                            RCRegionTriggers[text3] = regionTrigger;
                        }
                        else
                        {
                            RegionTrigger regionTrigger = new RegionTrigger();
                            regionTrigger.titanEventEnter = rCEvent;
                            regionTrigger.myName = text3;
                            RCRegionTriggers.Add(text3, regionTrigger);
                        }
                        RCVariableNames.Add("OnTitanEnterRegion[" + text3 + "]", value);
                    }
                    else if (text2.StartsWith("OnTitanLeaveRegion"))
                    {
                        int num10 = text2.IndexOf('[');
                        int num11 = text2.IndexOf(']');
                        string text3 = text2.Substring(num10 + 2, num11 - num10 - 3);
                        num10 = text2.IndexOf('(');
                        num11 = text2.IndexOf(')');
                        string value = text2.Substring(num10 + 2, num11 - num10 - 3);
                        if (RCRegionTriggers.ContainsKey(text3))
                        {
                            RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
                            regionTrigger.titanEventExit = rCEvent;
                            regionTrigger.myName = text3;
                            RCRegionTriggers[text3] = regionTrigger;
                        }
                        else
                        {
                            RegionTrigger regionTrigger = new RegionTrigger();
                            regionTrigger.titanEventExit = rCEvent;
                            regionTrigger.myName = text3;
                            RCRegionTriggers.Add(text3, regionTrigger);
                        }
                        RCVariableNames.Add("OnTitanExitRegion[" + text3 + "]", value);
                    }
                    else if (text2.StartsWith("OnFirstLoad()"))
                    {
                        RCEvents.Add("OnFirstLoad", rCEvent);
                    }
                    else if (text2.StartsWith("OnRoundStart()"))
                    {
                        RCEvents.Add("OnRoundStart", rCEvent);
                    }
                    else if (text2.StartsWith("OnUpdate()"))
                    {
                        RCEvents.Add("OnUpdate", rCEvent);
                    }
                    else if (text2.StartsWith("OnTitanDie"))
                    {
                        int num10 = text2.IndexOf('(');
                        int num11 = text2.LastIndexOf(')');
                        string[] array3 = text2.Substring(num10 + 1, num11 - num10 - 1).Split(',');
                        array3[0] = array3[0].Substring(1, array3[0].Length - 2);
                        array3[1] = array3[1].Substring(1, array3[1].Length - 2);
                        RCVariableNames.Add("OnTitanDie", array3);
                        RCEvents.Add("OnTitanDie", rCEvent);
                    }
                    else if (text2.StartsWith("OnPlayerDieByTitan"))
                    {
                        RCEvents.Add("OnPlayerDieByTitan", rCEvent);
                        int num10 = text2.IndexOf('(');
                        int num11 = text2.LastIndexOf(')');
                        string[] array3 = text2.Substring(num10 + 1, num11 - num10 - 1).Split(',');
                        array3[0] = array3[0].Substring(1, array3[0].Length - 2);
                        array3[1] = array3[1].Substring(1, array3[1].Length - 2);
                        RCVariableNames.Add("OnPlayerDieByTitan", array3);
                    }
                    else if (text2.StartsWith("OnPlayerDieByPlayer"))
                    {
                        RCEvents.Add("OnPlayerDieByPlayer", rCEvent);
                        int num10 = text2.IndexOf('(');
                        int num11 = text2.LastIndexOf(')');
                        string[] array3 = text2.Substring(num10 + 1, num11 - num10 - 1).Split(',');
                        array3[0] = array3[0].Substring(1, array3[0].Length - 2);
                        array3[1] = array3[1].Substring(1, array3[1].Length - 2);
                        RCVariableNames.Add("OnPlayerDieByPlayer", array3);
                    }
                    else if (text2.StartsWith("OnChatInput"))
                    {
                        RCEvents.Add("OnChatInput", rCEvent);
                        int num10 = text2.IndexOf('(');
                        int num11 = text2.LastIndexOf(')');
                        string value = text2.Substring(num10 + 1, num11 - num10 - 1);
                        RCVariableNames.Add("OnChatInput", value.Substring(1, value.Length - 2));
                    }
                }
            }
            catch (UnityException ex)
            {
                InRoomChat.Instance.AddLine(ex.Message);
            }
        }
    }

    public RCEvent ParseBlock(string[] stringArray, int eventClass, int eventType, RCCondition condition)
    {
        List<RCAction> list = new List<RCAction>();
        RCEvent rCEvent = new RCEvent(null, null, 0, 0);
        for (int i = 0; i < stringArray.Length; i++)
        {
            if (stringArray[i].StartsWith("If") && stringArray[i + 1] == "{")
            {
                int num = i + 2;
                int num2 = i + 2;
                int num3 = 0;
                for (int j = i + 2; j < stringArray.Length; j++)
                {
                    if (stringArray[j] == "{")
                    {
                        num3++;
                    }
                    if (stringArray[j] == "}")
                    {
                        if (num3 > 0)
                        {
                            num3--;
                            continue;
                        }
                        num2 = j - 1;
                        j = stringArray.Length;
                    }
                }
                string[] array = new string[num2 - num + 1];
                int num4 = 0;
                for (int k = num; k <= num2; k++)
                {
                    array[num4] = stringArray[k];
                    num4++;
                }
                int num5 = stringArray[i].IndexOf("(");
                int num6 = stringArray[i].LastIndexOf(")");
                string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
                int num7 = ConditionType(text);
                int num8 = text.IndexOf('.');
                text = text.Substring(num8 + 1);
                int sentOperand = OperandType(text, num7);
                num5 = text.IndexOf('(');
                num6 = text.LastIndexOf(")");
                text = text.Substring(num5 + 1, num6 - num5 - 1);
                string[] array2 = text.Split(',');
                RCCondition condition2 = new RCCondition(sentOperand, num7, ReturnHelper(array2[0]), ReturnHelper(array2[1]));
                RCEvent rCEvent2 = ParseBlock(array, 1, 0, condition2);
                RCAction item = new RCAction(0, 0, rCEvent2, null);
                rCEvent = rCEvent2;
                list.Add(item);
                i = num2;
            }
            else if (stringArray[i].StartsWith("While") && stringArray[i + 1] == "{")
            {
                int num = i + 2;
                int num2 = i + 2;
                int num3 = 0;
                for (int j = i + 2; j < stringArray.Length; j++)
                {
                    if (stringArray[j] == "{")
                    {
                        num3++;
                    }
                    if (stringArray[j] == "}")
                    {
                        if (num3 > 0)
                        {
                            num3--;
                            continue;
                        }
                        num2 = j - 1;
                        j = stringArray.Length;
                    }
                }
                string[] array = new string[num2 - num + 1];
                int num4 = 0;
                for (int k = num; k <= num2; k++)
                {
                    array[num4] = stringArray[k];
                    num4++;
                }
                int num5 = stringArray[i].IndexOf("(");
                int num6 = stringArray[i].LastIndexOf(")");
                string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
                int num7 = ConditionType(text);
                int num8 = text.IndexOf('.');
                text = text.Substring(num8 + 1);
                int sentOperand = OperandType(text, num7);
                num5 = text.IndexOf('(');
                num6 = text.LastIndexOf(")");
                text = text.Substring(num5 + 1, num6 - num5 - 1);
                string[] array2 = text.Split(',');
                RCCondition condition2 = new RCCondition(sentOperand, num7, ReturnHelper(array2[0]), ReturnHelper(array2[1]));
                RCEvent rCEvent2 = ParseBlock(array, 3, 0, condition2);
                RCAction item = new RCAction(0, 0, rCEvent2, null);
                list.Add(item);
                i = num2;
            }
            else if (stringArray[i].StartsWith("ForeachTitan") && stringArray[i + 1] == "{")
            {
                int num = i + 2;
                int num2 = i + 2;
                int num3 = 0;
                for (int j = i + 2; j < stringArray.Length; j++)
                {
                    if (stringArray[j] == "{")
                    {
                        num3++;
                    }
                    if (stringArray[j] == "}")
                    {
                        if (num3 > 0)
                        {
                            num3--;
                            continue;
                        }
                        num2 = j - 1;
                        j = stringArray.Length;
                    }
                }
                string[] array = new string[num2 - num + 1];
                int num4 = 0;
                for (int k = num; k <= num2; k++)
                {
                    array[num4] = stringArray[k];
                    num4++;
                }
                int num5 = stringArray[i].IndexOf("(");
                int num6 = stringArray[i].LastIndexOf(")");
                string text = stringArray[i].Substring(num5 + 2, num6 - num5 - 3);
                int num7 = 0;
                RCEvent rCEvent2 = ParseBlock(array, 2, num7, null);
                rCEvent2.foreachVariableName = text;
                RCAction item = new RCAction(0, 0, rCEvent2, null);
                list.Add(item);
                i = num2;
            }
            else if (stringArray[i].StartsWith("ForeachPlayer") && stringArray[i + 1] == "{")
            {
                int num = i + 2;
                int num2 = i + 2;
                int num3 = 0;
                for (int j = i + 2; j < stringArray.Length; j++)
                {
                    if (stringArray[j] == "{")
                    {
                        num3++;
                    }
                    if (stringArray[j] == "}")
                    {
                        if (num3 > 0)
                        {
                            num3--;
                            continue;
                        }
                        num2 = j - 1;
                        j = stringArray.Length;
                    }
                }
                string[] array = new string[num2 - num + 1];
                int num4 = 0;
                for (int k = num; k <= num2; k++)
                {
                    array[num4] = stringArray[k];
                    num4++;
                }
                int num5 = stringArray[i].IndexOf("(");
                int num6 = stringArray[i].LastIndexOf(")");
                string text = stringArray[i].Substring(num5 + 2, num6 - num5 - 3);
                int num7 = 1;
                RCEvent rCEvent2 = ParseBlock(array, 2, num7, null);
                rCEvent2.foreachVariableName = text;
                RCAction item = new RCAction(0, 0, rCEvent2, null);
                list.Add(item);
                i = num2;
            }
            else if (stringArray[i].StartsWith("Else") && stringArray[i + 1] == "{")
            {
                int num = i + 2;
                int num2 = i + 2;
                int num3 = 0;
                for (int j = i + 2; j < stringArray.Length; j++)
                {
                    if (stringArray[j] == "{")
                    {
                        num3++;
                    }
                    if (stringArray[j] == "}")
                    {
                        if (num3 > 0)
                        {
                            num3--;
                            continue;
                        }
                        num2 = j - 1;
                        j = stringArray.Length;
                    }
                }
                string[] array = new string[num2 - num + 1];
                int num4 = 0;
                for (int k = num; k <= num2; k++)
                {
                    array[num4] = stringArray[k];
                    num4++;
                }
                if (stringArray[i] == "Else")
                {
                    RCEvent rCEvent2 = ParseBlock(array, 0, 0, null);
                    RCAction item = new RCAction(0, 0, rCEvent2, null);
                    rCEvent.SetElse(item);
                    i = num2;
                }
                else if (stringArray[i].StartsWith("Else If"))
                {
                    int num5 = stringArray[i].IndexOf("(");
                    int num6 = stringArray[i].LastIndexOf(")");
                    string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
                    int num7 = ConditionType(text);
                    int num8 = text.IndexOf('.');
                    text = text.Substring(num8 + 1);
                    int sentOperand = OperandType(text, num7);
                    num5 = text.IndexOf('(');
                    num6 = text.LastIndexOf(")");
                    text = text.Substring(num5 + 1, num6 - num5 - 1);
                    string[] array2 = text.Split(',');
                    RCCondition condition2 = new RCCondition(sentOperand, num7, ReturnHelper(array2[0]), ReturnHelper(array2[1]));
                    RCEvent rCEvent2 = ParseBlock(array, 1, 0, condition2);
                    RCAction item = new RCAction(0, 0, rCEvent2, null);
                    rCEvent.SetElse(item);
                    i = num2;
                }
            }
            else if (stringArray[i].StartsWith("VariableInt"))
            {
                int category = 1;
                int num9 = stringArray[i].IndexOf('.');
                int num10 = stringArray[i].IndexOf('(');
                int num11 = stringArray[i].LastIndexOf(')');
                string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
                string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
                if (text2.StartsWith("SetRandom"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCActionHelper rCActionHelper3 = ReturnHelper(array3[2]);
                    RCAction item = new RCAction(category, 12, null, new RCActionHelper[3]
                    {
                        rCActionHelper,
                        rCActionHelper2,
                        rCActionHelper3
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Set"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 0, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Add"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 1, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Subtract"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 2, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Multiply"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 3, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Divide"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 4, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Modulo"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 5, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Power"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 6, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
            }
            else if (stringArray[i].StartsWith("VariableBool"))
            {
                int category = 2;
                int num9 = stringArray[i].IndexOf('.');
                int num10 = stringArray[i].IndexOf('(');
                int num11 = stringArray[i].LastIndexOf(')');
                string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
                string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
                if (text2.StartsWith("SetToOpposite"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCAction item = new RCAction(category, 11, null, new RCActionHelper[1]
                    {
                        rCActionHelper
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetRandom"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCAction item = new RCAction(category, 12, null, new RCActionHelper[1]
                    {
                        rCActionHelper
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Set"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 0, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
            }
            else if (stringArray[i].StartsWith("VariableString"))
            {
                int category = 3;
                int num9 = stringArray[i].IndexOf('.');
                int num10 = stringArray[i].IndexOf('(');
                int num11 = stringArray[i].LastIndexOf(')');
                string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
                string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
                if (text2.StartsWith("Set"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 0, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Concat"))
                {
                    RCActionHelper[] array4 = new RCActionHelper[array3.Length];
                    for (int j = 0; j < array3.Length; j++)
                    {
                        array4[j] = ReturnHelper(array3[j]);
                    }
                    RCAction item = new RCAction(category, 7, null, array4);
                    list.Add(item);
                }
                else if (text2.StartsWith("Append"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 8, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Replace"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCActionHelper rCActionHelper3 = ReturnHelper(array3[2]);
                    RCAction item = new RCAction(category, 10, null, new RCActionHelper[3]
                    {
                        rCActionHelper,
                        rCActionHelper2,
                        rCActionHelper3
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Remove"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 9, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
            }
            else if (stringArray[i].StartsWith("VariableFloat"))
            {
                int category = 4;
                int num9 = stringArray[i].IndexOf('.');
                int num10 = stringArray[i].IndexOf('(');
                int num11 = stringArray[i].LastIndexOf(')');
                string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
                string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
                if (text2.StartsWith("SetRandom"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCActionHelper rCActionHelper3 = ReturnHelper(array3[2]);
                    RCAction item = new RCAction(category, 12, null, new RCActionHelper[3]
                    {
                        rCActionHelper,
                        rCActionHelper2,
                        rCActionHelper3
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Set"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 0, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Add"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 1, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Subtract"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 2, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Multiply"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 3, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Divide"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 4, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Modulo"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 5, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Power"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 6, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
            }
            else if (stringArray[i].StartsWith("VariablePlayer"))
            {
                int category = 5;
                int num9 = stringArray[i].IndexOf('.');
                int num10 = stringArray[i].IndexOf('(');
                int num11 = stringArray[i].LastIndexOf(')');
                string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
                string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
                if (text2.StartsWith("Set"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 0, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
            }
            else if (stringArray[i].StartsWith("VariableTitan"))
            {
                int category = 6;
                int num9 = stringArray[i].IndexOf('.');
                int num10 = stringArray[i].IndexOf('(');
                int num11 = stringArray[i].LastIndexOf(')');
                string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
                string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
                if (text2.StartsWith("Set"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 0, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
            }
            else if (stringArray[i].StartsWith("Player"))
            {
                int category = 7;
                int num9 = stringArray[i].IndexOf('.');
                int num10 = stringArray[i].IndexOf('(');
                int num11 = stringArray[i].LastIndexOf(')');
                string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
                string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
                if (text2.StartsWith("KillPlayer"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 0, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SpawnPlayerAt"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCActionHelper rCActionHelper3 = ReturnHelper(array3[2]);
                    RCActionHelper rCActionHelper4 = ReturnHelper(array3[3]);
                    RCAction item = new RCAction(category, 2, null, new RCActionHelper[4]
                    {
                        rCActionHelper,
                        rCActionHelper2,
                        rCActionHelper3,
                        rCActionHelper4
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SpawnPlayer"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCAction item = new RCAction(category, 1, null, new RCActionHelper[1]
                    {
                        rCActionHelper
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("MovePlayer"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCActionHelper rCActionHelper3 = ReturnHelper(array3[2]);
                    RCActionHelper rCActionHelper4 = ReturnHelper(array3[3]);
                    RCAction item = new RCAction(category, 3, null, new RCActionHelper[4]
                    {
                        rCActionHelper,
                        rCActionHelper2,
                        rCActionHelper3,
                        rCActionHelper4
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetKills"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 4, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetDeaths"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 5, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetMaxDmg"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 6, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetTotalDmg"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 7, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetName"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 8, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetGuildName"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 9, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetTeam"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 10, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetCustomInt"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 11, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetCustomBool"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 12, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetCustomString"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 13, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetCustomFloat"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 14, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
            }
            else if (stringArray[i].StartsWith("Titan"))
            {
                int category = 8;
                int num9 = stringArray[i].IndexOf('.');
                int num10 = stringArray[i].IndexOf('(');
                int num11 = stringArray[i].LastIndexOf(')');
                string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
                string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
                if (text2.StartsWith("KillTitan"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCActionHelper rCActionHelper3 = ReturnHelper(array3[2]);
                    RCAction item = new RCAction(category, 0, null, new RCActionHelper[3]
                    {
                        rCActionHelper,
                        rCActionHelper2,
                        rCActionHelper3
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SpawnTitanAt"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCActionHelper rCActionHelper3 = ReturnHelper(array3[2]);
                    RCActionHelper rCActionHelper4 = ReturnHelper(array3[3]);
                    RCActionHelper rCActionHelper5 = ReturnHelper(array3[4]);
                    RCActionHelper rCActionHelper6 = ReturnHelper(array3[5]);
                    RCActionHelper rCActionHelper7 = ReturnHelper(array3[6]);
                    RCAction item = new RCAction(category, 2, null, new RCActionHelper[7]
                    {
                        rCActionHelper,
                        rCActionHelper2,
                        rCActionHelper3,
                        rCActionHelper4,
                        rCActionHelper5,
                        rCActionHelper6,
                        rCActionHelper7
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SpawnTitan"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCActionHelper rCActionHelper3 = ReturnHelper(array3[2]);
                    RCActionHelper rCActionHelper4 = ReturnHelper(array3[3]);
                    RCAction item = new RCAction(category, 1, null, new RCActionHelper[4]
                    {
                        rCActionHelper,
                        rCActionHelper2,
                        rCActionHelper3,
                        rCActionHelper4
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("SetHealth"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCAction item = new RCAction(category, 3, null, new RCActionHelper[2]
                    {
                        rCActionHelper,
                        rCActionHelper2
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("MoveTitan"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCActionHelper rCActionHelper2 = ReturnHelper(array3[1]);
                    RCActionHelper rCActionHelper3 = ReturnHelper(array3[2]);
                    RCActionHelper rCActionHelper4 = ReturnHelper(array3[3]);
                    RCAction item = new RCAction(category, 4, null, new RCActionHelper[4]
                    {
                        rCActionHelper,
                        rCActionHelper2,
                        rCActionHelper3,
                        rCActionHelper4
                    });
                    list.Add(item);
                }
            }
            else if (stringArray[i].StartsWith("Game"))
            {
                int category = 9;
                int num9 = stringArray[i].IndexOf('.');
                int num10 = stringArray[i].IndexOf('(');
                int num11 = stringArray[i].LastIndexOf(')');
                string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
                string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
                if (text2.StartsWith("PrintMessage"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCAction item = new RCAction(category, 0, null, new RCActionHelper[1]
                    {
                        rCActionHelper
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("LoseGame"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCAction item = new RCAction(category, 2, null, new RCActionHelper[1]
                    {
                        rCActionHelper
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("WinGame"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCAction item = new RCAction(category, 1, null, new RCActionHelper[1]
                    {
                        rCActionHelper
                    });
                    list.Add(item);
                }
                else if (text2.StartsWith("Restart"))
                {
                    RCActionHelper rCActionHelper = ReturnHelper(array3[0]);
                    RCAction item = new RCAction(category, 3, null, new RCActionHelper[1]
                    {
                        rCActionHelper
                    });
                    list.Add(item);
                }
            }
        }
        return new RCEvent(condition, list, eventClass, eventType);
    }

    public RCActionHelper ReturnHelper(string str)
    {
        string[] array = str.Split('.');
        if (float.TryParse(str, out float _))
        {
            array = new string[1]
            {
                str
            };
        }
        List<RCActionHelper> list = new List<RCActionHelper>();
        int sentType = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (list.Count == 0)
            {
                string text = array[i];
                int result2;
                float result3;
                if (text.StartsWith("\"") && text.EndsWith("\""))
                {
                    RCActionHelper item = new RCActionHelper(0, 0, text.Substring(1, text.Length - 2));
                    list.Add(item);
                    sentType = 2;
                }
                else if (int.TryParse(text, out result2))
                {
                    RCActionHelper item = new RCActionHelper(0, 0, result2);
                    list.Add(item);
                    sentType = 0;
                }
                else if (float.TryParse(text, out result3))
                {
                    RCActionHelper item = new RCActionHelper(0, 0, result3);
                    list.Add(item);
                    sentType = 3;
                }
                else if (text.ToLower() == "true" || text.ToLower() == "false")
                {
                    RCActionHelper item = new RCActionHelper(0, 0, Convert.ToBoolean(text.ToLower()));
                    list.Add(item);
                    sentType = 1;
                }
                else if (text.StartsWith("Variable"))
                {
                    int num = text.IndexOf('(');
                    int num2 = text.LastIndexOf(')');
                    if (text.StartsWith("VariableInt"))
                    {
                        text = text.Substring(num + 1, num2 - num - 1);
                        RCActionHelper item = new RCActionHelper(1, 0, ReturnHelper(text));
                        list.Add(item);
                        sentType = 0;
                    }
                    else if (text.StartsWith("VariableBool"))
                    {
                        text = text.Substring(num + 1, num2 - num - 1);
                        RCActionHelper item = new RCActionHelper(1, 1, ReturnHelper(text));
                        list.Add(item);
                        sentType = 1;
                    }
                    else if (text.StartsWith("VariableString"))
                    {
                        text = text.Substring(num + 1, num2 - num - 1);
                        RCActionHelper item = new RCActionHelper(1, 2, ReturnHelper(text));
                        list.Add(item);
                        sentType = 2;
                    }
                    else if (text.StartsWith("VariableFloat"))
                    {
                        text = text.Substring(num + 1, num2 - num - 1);
                        RCActionHelper item = new RCActionHelper(1, 3, ReturnHelper(text));
                        list.Add(item);
                        sentType = 3;
                    }
                    else if (text.StartsWith("VariablePlayer"))
                    {
                        text = text.Substring(num + 1, num2 - num - 1);
                        RCActionHelper item = new RCActionHelper(1, 4, ReturnHelper(text));
                        list.Add(item);
                        sentType = 4;
                    }
                    else if (text.StartsWith("VariableTitan"))
                    {
                        text = text.Substring(num + 1, num2 - num - 1);
                        RCActionHelper item = new RCActionHelper(1, 5, ReturnHelper(text));
                        list.Add(item);
                        sentType = 5;
                    }
                }
                else if (text.StartsWith("Region"))
                {
                    int num = text.IndexOf('(');
                    int num2 = text.LastIndexOf(')');
                    if (text.StartsWith("RegionRandomX"))
                    {
                        text = text.Substring(num + 1, num2 - num - 1);
                        RCActionHelper item = new RCActionHelper(4, 0, ReturnHelper(text));
                        list.Add(item);
                        sentType = 3;
                    }
                    else if (text.StartsWith("RegionRandomY"))
                    {
                        text = text.Substring(num + 1, num2 - num - 1);
                        RCActionHelper item = new RCActionHelper(4, 1, ReturnHelper(text));
                        list.Add(item);
                        sentType = 3;
                    }
                    else if (text.StartsWith("RegionRandomZ"))
                    {
                        text = text.Substring(num + 1, num2 - num - 1);
                        RCActionHelper item = new RCActionHelper(4, 2, ReturnHelper(text));
                        list.Add(item);
                        sentType = 3;
                    }
                }
            }
            else
            {
                if (list.Count <= 0)
                {
                    continue;
                }
                string text = array[i];
                int helperClass = list[list.Count - 1].helperClass;
                if (helperClass == 1)
                {
                    switch (list[list.Count - 1].helperType)
                    {
                        case 4:
                            if (text.StartsWith("GetTeam()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 1, null);
                                list.Add(item);
                                sentType = 0;
                            }
                            else if (text.StartsWith("GetType()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 0, null);
                                list.Add(item);
                                sentType = 0;
                            }
                            else if (text.StartsWith("GetIsAlive()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 2, null);
                                list.Add(item);
                                sentType = 1;
                            }
                            else if (text.StartsWith("GetTitan()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 3, null);
                                list.Add(item);
                                sentType = 0;
                            }
                            else if (text.StartsWith("GetKills()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 4, null);
                                list.Add(item);
                                sentType = 0;
                            }
                            else if (text.StartsWith("GetDeaths()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 5, null);
                                list.Add(item);
                                sentType = 0;
                            }
                            else if (text.StartsWith("GetMaxDmg()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 6, null);
                                list.Add(item);
                                sentType = 0;
                            }
                            else if (text.StartsWith("GetTotalDmg()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 7, null);
                                list.Add(item);
                                sentType = 0;
                            }
                            else if (text.StartsWith("GetCustomInt()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 8, null);
                                list.Add(item);
                                sentType = 0;
                            }
                            else if (text.StartsWith("GetCustomBool()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 9, null);
                                list.Add(item);
                                sentType = 1;
                            }
                            else if (text.StartsWith("GetCustomString()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 10, null);
                                list.Add(item);
                                sentType = 2;
                            }
                            else if (text.StartsWith("GetCustomFloat()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 11, null);
                                list.Add(item);
                                sentType = 3;
                            }
                            else if (text.StartsWith("GetPositionX()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 14, null);
                                list.Add(item);
                                sentType = 3;
                            }
                            else if (text.StartsWith("GetPositionY()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 15, null);
                                list.Add(item);
                                sentType = 3;
                            }
                            else if (text.StartsWith("GetPositionZ()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 16, null);
                                list.Add(item);
                                sentType = 3;
                            }
                            else if (text.StartsWith("GetName()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 12, null);
                                list.Add(item);
                                sentType = 2;
                            }
                            else if (text.StartsWith("GetGuildName()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 13, null);
                                list.Add(item);
                                sentType = 2;
                            }
                            else if (text.StartsWith("GetSpeed()"))
                            {
                                RCActionHelper item = new RCActionHelper(2, 17, null);
                                list.Add(item);
                                sentType = 3;
                            }
                            break;
                        case 5:
                            if (text.StartsWith("GetType()"))
                            {
                                RCActionHelper item = new RCActionHelper(3, 0, null);
                                list.Add(item);
                                sentType = 0;
                            }
                            else if (text.StartsWith("GetSize()"))
                            {
                                RCActionHelper item = new RCActionHelper(3, 1, null);
                                list.Add(item);
                                sentType = 3;
                            }
                            else if (text.StartsWith("GetHealth()"))
                            {
                                RCActionHelper item = new RCActionHelper(3, 2, null);
                                list.Add(item);
                                sentType = 0;
                            }
                            else if (text.StartsWith("GetPositionX()"))
                            {
                                RCActionHelper item = new RCActionHelper(3, 3, null);
                                list.Add(item);
                                sentType = 3;
                            }
                            else if (text.StartsWith("GetPositionY()"))
                            {
                                RCActionHelper item = new RCActionHelper(3, 4, null);
                                list.Add(item);
                                sentType = 3;
                            }
                            else if (text.StartsWith("GetPositionZ()"))
                            {
                                RCActionHelper item = new RCActionHelper(3, 5, null);
                                list.Add(item);
                                sentType = 3;
                            }
                            break;
                        default:
                            if (text.StartsWith("ConvertToInt()"))
                            {
                                RCActionHelper item = new RCActionHelper(5, sentType, null);
                                list.Add(item);
                                sentType = 0;
                            }
                            else if (text.StartsWith("ConvertToBool()"))
                            {
                                RCActionHelper item = new RCActionHelper(5, sentType, null);
                                list.Add(item);
                                sentType = 1;
                            }
                            else if (text.StartsWith("ConvertToString()"))
                            {
                                RCActionHelper item = new RCActionHelper(5, sentType, null);
                                list.Add(item);
                                sentType = 2;
                            }
                            else if (text.StartsWith("ConvertToFloat()"))
                            {
                                RCActionHelper item = new RCActionHelper(5, sentType, null);
                                list.Add(item);
                                sentType = 3;
                            }
                            break;
                    }
                }
                else if (text.StartsWith("ConvertToInt()"))
                {
                    RCActionHelper item = new RCActionHelper(5, sentType, null);
                    list.Add(item);
                    sentType = 0;
                }
                else if (text.StartsWith("ConvertToBool()"))
                {
                    RCActionHelper item = new RCActionHelper(5, sentType, null);
                    list.Add(item);
                    sentType = 1;
                }
                else if (text.StartsWith("ConvertToString()"))
                {
                    RCActionHelper item = new RCActionHelper(5, sentType, null);
                    list.Add(item);
                    sentType = 2;
                }
                else if (text.StartsWith("ConvertToFloat()"))
                {
                    RCActionHelper item = new RCActionHelper(5, sentType, null);
                    list.Add(item);
                    sentType = 3;
                }
            }
        }
        for (int i = list.Count - 1; i > 0; i--)
        {
            list[i - 1].SetNextHelper(list[i]);
        }
        return list[0];
    }

    public int OperandType(string str, int condition)
    {
        switch (condition)
        {
            case 0:
            case 3:
                if (str.StartsWith("Equals"))
                {
                    return 2;
                }
                if (str.StartsWith("NotEquals"))
                {
                    return 5;
                }
                if (str.StartsWith("LessThan"))
                {
                    return 0;
                }
                if (str.StartsWith("LessThanOrEquals"))
                {
                    return 1;
                }
                if (str.StartsWith("GreaterThanOrEquals"))
                {
                    return 3;
                }
                if (str.StartsWith("GreaterThan"))
                {
                    return 4;
                }
                return 0;
            case 1:
            case 4:
            case 5:
                if (str.StartsWith("Equals"))
                {
                    return 2;
                }
                if (str.StartsWith("NotEquals"))
                {
                    return 5;
                }
                return 0;
            case 2:
                if (str.StartsWith("Equals"))
                {
                    return 0;
                }
                if (str.StartsWith("NotEquals"))
                {
                    return 1;
                }
                if (str.StartsWith("Contains"))
                {
                    return 2;
                }
                if (str.StartsWith("NotContains"))
                {
                    return 3;
                }
                if (str.StartsWith("StartsWith"))
                {
                    return 4;
                }
                if (str.StartsWith("NotStartsWith"))
                {
                    return 5;
                }
                if (str.StartsWith("EndsWith"))
                {
                    return 6;
                }
                if (str.StartsWith("NotEndsWith"))
                {
                    return 7;
                }
                return 0;
            default:
                return 0;
        }
    }

    public int ConditionType(string str)
    {
        if (str.StartsWith("Int"))
        {
            return 0;
        }
        if (str.StartsWith("Bool"))
        {
            return 1;
        }
        if (str.StartsWith("String"))
        {
            return 2;
        }
        if (str.StartsWith("Float"))
        {
            return 3;
        }
        if (str.StartsWith("Titan"))
        {
            return 5;
        }
        if (str.StartsWith("Player"))
        {
            return 4;
        }
        return 0;
    }

    public void OnUpdate()
    {
        if (RCEvents.ContainsKey("OnUpdate"))
        {
            if (updateTime > 0f)
            {
                updateTime -= Time.deltaTime;
                return;
            }
            RCEvent rCEvent = (RCEvent)RCEvents["OnUpdate"];
            rCEvent.CheckEvent();
            updateTime = 1f;
        }
    }

    [RPC]
    public void spawnPlayerAtRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
    {
        if (!info.sender.isMasterClient || !LogicLoaded || !CustomLevelLoaded || needChooseSide || !mainCamera.gameOver)
        {
            return;
        }
        Vector3 position = new Vector3(posX, posY, posZ);
        mainCamera.SetMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, new Quaternion(0f, 0f, 0f, 1f), 0));
        HERO hero = mainCamera.main_object.GetComponent<HERO>();
        HERO_SETUP setup = hero.GetComponent<HERO_SETUP>();
        string text = myLastHero;
        text = text.ToUpper();
        if (text == "SET 1" || text == "SET 2" || text == "SET 3")
        {
            HeroCostume heroCostume = CostumeConverter.FromLocalData(text);
            CostumeConverter.ToLocalData(heroCostume, text);
            setup.Init();
            if (heroCostume != null)
            {
                setup.myCostume = heroCostume;
                setup.myCostume.stat = heroCostume.stat;
            }
            else
            {
                heroCostume = HeroCostume.CostumeOptions[3];
                setup.myCostume = heroCostume;
                setup.myCostume.stat = HeroStat.GetInfo(heroCostume.name.ToUpper());
            }
            setup.CreateCharacterComponent();
            hero.SetStat2();
            hero.SetSkillHUDPosition2();
        }
        else
        {
            for (int i = 0; i < HeroCostume.Costumes.Length; i++)
            {
                if (HeroCostume.Costumes[i].name.ToUpper() == text.ToUpper())
                {
                    int num = HeroCostume.Costumes[i].id;
                    if (text.ToUpper() != "AHSS")
                    {
                        num += CheckBoxCostume.CostumeSet - 1;
                    }
                    if (HeroCostume.Costumes[num].name != HeroCostume.Costumes[i].name)
                    {
                        num = HeroCostume.Costumes[i].id + 1;
                    }
                    setup.Init();
                    setup.myCostume = HeroCostume.Costumes[num];
                    setup.myCostume.stat = HeroStat.GetInfo(HeroCostume.Costumes[num].name.ToUpper());
                    setup.CreateCharacterComponent();
                    hero.SetStat2();
                    hero.SetSkillHUDPosition2();
                    break;
                }
            }
        }
        CostumeConverter.ToPhotonData(setup.myCostume, PhotonNetwork.player);
        if (Level.Mode == GameMode.PvPCapture)
        {
            mainCamera.main_object.transform.position += new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20));
        }
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add("dead", false);
        hashtable.Add(PhotonPlayerProperty.IsTitan, 1);
        PhotonNetwork.player.SetCustomProperties(hashtable);
        mainCamera.enabled = true;
        GameObject mainCam = GameObject.Find("MainCamera");
        mainCamera.SetHUDPosition();
        mainCam.GetComponent<SpectatorMovement>().disable = true;
        mainCam.GetComponent<MouseLook>().disable = true;
        mainCamera.gameOver = false;
        Screen.lockCursor = IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS;
        Screen.showCursor = false;
        isLosing = false;
        SetTextCenter(string.Empty);
    }

    private void spawnPlayerCustomMap()
    {
        if (!needChooseSide && mainCamera.gameOver)
        {
            mainCamera.gameOver = false;
            if (GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.IsTitan]) == 2)
            {
                SpawnNonAITitan2(myLastHero);
            }
            else
            {
                SpawnPlayer(myLastHero, myLastRespawnTag);
            }
            SetTextCenter(string.Empty);
        }
    }

    public void NOTSpawnPlayerRC(string id)
    {
        myLastHero = id.ToUpper();
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add("dead", true);
        hashtable.Add(PhotonPlayerProperty.IsTitan, 1);
        PhotonNetwork.player.SetCustomProperties(hashtable);
        Screen.lockCursor = IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS;
        Screen.showCursor = false;
        mainCamera.enabled = true;
        mainCamera.SetMainObject(null);
        mainCamera.SetSpectorMode(val: true);
        mainCamera.gameOver = true;
    }

    public void NOTSpawnNonAITitanRC(string id)
    {
        myLastHero = id.ToUpper();
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable
        {
            { "dead", true },
            { PhotonPlayerProperty.IsTitan, 2 }
        };
        PhotonNetwork.player.SetCustomProperties(hashtable);
        Screen.lockCursor = IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS;
        Screen.showCursor = true;
        SetTextCenter("Syncing spawn locations...");
        mainCamera.enabled = true;
        mainCamera.SetMainObject(null);
        mainCamera.SetSpectorMode(val: true);
        mainCamera.gameOver = true;
    }

    private IEnumerator CoRespawn(float seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            if (isLosing || isWinning)
            {
                continue;
            }

            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (player.customProperties[PhotonPlayerProperty.RCTeam] == null && GExtensions.AsBool(player.customProperties[PhotonPlayerProperty.IsDead]) && GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.IsTitan]) != 2)
                {
                    base.photonView.RPC("respawnHeroInNewRound", player);
                }
            }

        }
    }

    private void EndGameRC()
    {
        if (RCSettings.PointMode > 0)
        {
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable.Add(PhotonPlayerProperty.Kills, 0);
                hashtable.Add(PhotonPlayerProperty.Deaths, 0);
                hashtable.Add(PhotonPlayerProperty.MaxDamage, 0);
                hashtable.Add(PhotonPlayerProperty.TotalDamage, 0);
                photonPlayer.SetCustomProperties(hashtable);
            }
        }
        gameEndCD = 0f;
        RestartGame();
    }

    private void EndGameInfection()
    {
        ImATitan.Clear();

        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add(PhotonPlayerProperty.IsTitan, 1);
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
            photonPlayer.SetCustomProperties(hashtable);
        }

        int num = PhotonNetwork.playerList.Length;
        int num2 = RCSettings.InfectionMode;

        ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
        hashtable2.Add(PhotonPlayerProperty.IsTitan, 2);
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            PhotonPlayer photonPlayer2 = PhotonNetwork.playerList[i];
            if (num > 0 && UnityEngine.Random.Range(0f, 1f) <= (float)num2 / (float)num)
            {
                photonPlayer2.SetCustomProperties(hashtable2);
                ImATitan.Add(photonPlayer2.Id, 2);
                num2--;
            }
            num--;
        }
        gameEndCD = 0f;

        RestartGame();
    }

    public void KickPlayer(PhotonPlayer player, bool ban, string reason)
    {
        if (OnPrivateServer)
        {
            string playerName = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]);
            ServerCloseConnection(player, ban, playerName);
            return;
        }
        PhotonNetwork.DestroyPlayerObjects(player);
        PhotonNetwork.CloseConnection(player);
        base.photonView.RPC("ignorePlayer", PhotonTargets.Others, player.Id);
        if (!IgnoreList.Contains(player.Id))
        {
            IgnoreList.Add(player.Id);
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                TargetActors = new int[] { player.Id }
            };
            PhotonNetwork.RaiseEvent(254, null, sendReliable: true, raiseEventOptions);
        }
        if (ban && !BanHash.ContainsKey(player.Id))
        {
            string playerName = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]);
            BanHash.Add(player.Id, playerName);
        }
        if (reason.Length > 0)
        {
            InRoomChat.Instance.AddLine($"Player #{player.Id} was {(ban ? "banned" : "kicked")}. Reason: " + reason);
        }
        RecompilePlayerList(0.1f);
    }

    [RPC]
    private void ignorePlayer(int id, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            PhotonPlayer player = PhotonPlayer.Find(id);
            if (player != null && !IgnoreList.Contains(id))
            {
                IgnoreList.Add(id);
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
                {
                    TargetActors = new int[] { id }
                };
                PhotonNetwork.RaiseEvent(254, null, sendReliable: true, raiseEventOptions);
            }
        }
        RecompilePlayerList(0.1f);
    }

    [RPC]
    private void ignorePlayerArray(int[] ids, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            foreach (int id in ids)
            {
                PhotonPlayer photonPlayer = PhotonPlayer.Find(id);
                if (photonPlayer != null && !IgnoreList.Contains(id))
                {
                    IgnoreList.Add(id);
                    PhotonNetwork.RaiseEvent(254, null, true, new RaiseEventOptions
                    {
                        TargetActors = new int[] { id }
                    });
                }
            }
        }
        RecompilePlayerList(0.1f);
    }

    private void ResetSettings(bool isLeave)
    {
        MasterRC = false;
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add(PhotonPlayerProperty.RCTeam, 0);
        if (isLeave)
        {
            CurrentLevel = string.Empty;
            hashtable.Add(PhotonPlayerProperty.CurrentLevel, string.Empty);
            levelCache = new List<string[]>();
            titanSpawns.Clear();
            playerSpawnsC.Clear();
            playerSpawnsM.Clear();
            titanSpawners.Clear();
            IntVariables.Clear();
            BoolVariables.Clear();
            StringVariables.Clear();
            FloatVariables.Clear();
            GlobalVariables.Clear();
            RCRegions.Clear();
            RCEvents.Clear();
            RCVariableNames.Clear();
            PlayerVariables.Clear();
            TitanVariables.Clear();
            RCRegionTriggers.Clear();
            CurrentScriptLogic = string.Empty;
            hashtable.Add(PhotonPlayerProperty.StatAccel, 100);
            hashtable.Add(PhotonPlayerProperty.StatBlade, 100);
            hashtable.Add(PhotonPlayerProperty.StatGas, 100);
            hashtable.Add(PhotonPlayerProperty.StatSpeed, 100);
            restartingTitan = false;
            restartingMC = false;
            restartingHorse = false;
            restartingEren = false;
            restartingBomb = false;
        }
        PhotonNetwork.player.SetCustomProperties(hashtable);
        ResetGameSettings();
        BanHash = new ExitGames.Client.Photon.Hashtable();
        ImATitan = new ExitGames.Client.Photon.Hashtable();
        OldScript = string.Empty;
        IgnoreList = new List<int>();
        restartCount = new List<float>();
        HeroHash = new ExitGames.Client.Photon.Hashtable();
    }

    private void ResetGameSettings()
    {
        RCSettings.BombMode = 0;
        RCSettings.TeamMode = 0;
        RCSettings.PointMode = 0;
        RCSettings.DisableRock = 0;
        RCSettings.ExplodeMode = 0;
        RCSettings.HealthMode = 0;
        RCSettings.HealthLower = 0;
        RCSettings.HealthUpper = 0;
        RCSettings.InfectionMode = 0;
        RCSettings.BanEren = 0;
        RCSettings.MoreTitans = 0;
        RCSettings.MinimumDamage = 0;
        RCSettings.SizeMode = 0;
        RCSettings.SizeLower = 0f;
        RCSettings.SizeUpper = 0f;
        RCSettings.SpawnMode = 0;
        RCSettings.NormalRate = 0f;
        RCSettings.AberrantRate = 0f;
        RCSettings.JumperRate = 0f;
        RCSettings.CrawlerRate = 0f;
        RCSettings.PunkRate = 0f;
        RCSettings.HorseMode = 0;
        RCSettings.WaveModeOn = 0;
        RCSettings.WaveModeNum = 0;
        RCSettings.FriendlyMode = 0;
        RCSettings.PvPMode = 0;
        RCSettings.MaxWave = 0;
        RCSettings.EndlessMode = 0;
        RCSettings.AhssReload = 0;
        RCSettings.PunkWaves = 0;
        RCSettings.GlobalDisableMinimap = 0;
        RCSettings.Motd = string.Empty;
        RCSettings.DeadlyCannons = 0;
        RCSettings.AsoPreserveKDR = 0;
        RCSettings.RacingStatic = 0;
    }

    public void AddTime(float time)
    {
        timeTotalServer -= time;
    }

    [Obsolete("Use FengGameManagerMKII.Chat instead.")]
    [RPC]
    private void ChatPM(string sender, string content, PhotonMessageInfo info)
    {
        Chat(content, $"=> You".AsColor("FFCC00"), info);
    }

    [RPC]
    private void pauseRPC(bool paused, PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.Validators.FGMChecker.IsPauseStateChangeValid(info))
        {
            if (paused)
            {
                pauseWaitTime = 4f;
                Time.timeScale = 0.000001f;
            }
            else
            {
                pauseWaitTime = 3f;
            }
        }
    }

    private void CoreAdd()
    {
        if (PhotonNetwork.isMasterClient)
        {
            OnUpdate();
            if (CustomLevelLoaded)
            {
                for (int i = 0; i < titanSpawners.Count; i++)
                {
                    TitanSpawner titanSpawner = titanSpawners[i];
                    titanSpawner.time -= Time.deltaTime;
                    if (!(titanSpawner.time <= 0f) || titans.Count + fT.Count >= Math.Min(RCSettings.TitanCap, 80))
                    {
                        continue;
                    }

                    if (titanSpawner.name == "spawnAnnie")
                    {
                        PhotonNetwork.Instantiate("FEMALE_TITAN", titanSpawner.location, new Quaternion(0f, 0f, 0f, 1f), 0);
                    }
                    else
                    {
                        GameObject gameObject = PhotonNetwork.Instantiate("TITAN_VER3.1", titanSpawner.location, new Quaternion(0f, 0f, 0f, 1f), 0);

                        switch (titanSpawner.name)
                        {
                            case "spawnAbnormal":
                                gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Aberrant, forceCrawler: false);
                                break;
                            case "spawnJumper":
                                gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Jumper, forceCrawler: false);
                                break;
                            case "spawnCrawler":
                                gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Crawler, forceCrawler: true);
                                break;
                            case "spawnPunk":
                                gameObject.GetComponent<TITAN>().setAbnormalType2(TitanClass.Punk, forceCrawler: false);
                                break;
                        }
                    }

                    if (titanSpawner.endless)
                    {
                        titanSpawner.time = titanSpawner.delay;
                    }
                    else
                    {
                        titanSpawners.Remove(titanSpawner);
                    }
                }
            }
        }

        if (Time.timeScale <= 0.1f)
        {
            if (pauseWaitTime <= 3f)
            {
                pauseWaitTime -= Time.deltaTime * 1000000f;
                if (pauseWaitTime <= 1f)
                {
                    Camera.main.farClipPlane = 1500f;
                }
                if (pauseWaitTime <= 0f)
                {
                    pauseWaitTime = 0f;
                    Time.timeScale = 1f;
                }
            }
            CoWaitAndRecompilePlayerList(0.1f);
        }

    }

    private void Cache()
    {
        ClothFactory.ClearClothCache();
        inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
        otherUsers.Clear();
        titanSpawners.Clear();
        groundList.Clear();
        PreservedPlayerKDR = new Dictionary<string, int[]>();
        NoRestart = false;
        SkyMaterial = null;
        isSpawning = false;
        retryTime = 0f;
        LogicLoaded = false;
        CustomLevelLoaded = true;
        isUnloading = false;
        isRecompiling = false;
        Time.timeScale = 1f;
        Camera.main.farClipPlane = 1500f;
        pauseWaitTime = 0f;
        spectateSprites = new List<GameObject>();
        isRestarting = false;

        if ((int)Settings[64] >= 100)
        {
            return;
        }

        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(CoWaitAndResetRestarts());
        }
        roundTime = 0f;
        if (Level.Name.StartsWith("Custom"))
        {
            CustomLevelLoaded = false;
        }
        if (PhotonNetwork.isMasterClient)
        {
            if (isFirstLoad)
            {
                SetGameSettings(CheckGameGUI());
            }
            if (RCSettings.EndlessMode > 0)
            {
                StartCoroutine(CoRespawn(RCSettings.EndlessMode));
            }
        }
        if ((int)Settings[244] == 1)
        {
            InRoomChat.Instance.AddLine(("(" + roundTime.ToString("F2") + ") ").AsColor("FFCC00") + "Round Start.");
        }
        isFirstLoad = false;
        RecompilePlayerList(0.5f);
    }

    private void CoreEditor()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            GUI.FocusControl(null);
        }

        if (selectedObj != null)
        {
            float d = 0.2f;
            if (InputRC.isInputLevel(InputCodeRC.LevelSlow))
            {
                d = 0.04f;
            }
            else if (InputRC.isInputLevel(InputCodeRC.LevelFast))
            {
                d = 0.6f;
            }
            if (InputRC.isInputLevel(InputCodeRC.LevelForward))
            {
                selectedObj.transform.position += d * new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
            }
            else if (InputRC.isInputLevel(InputCodeRC.LevelBack))
            {
                selectedObj.transform.position -= d * new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
            }
            if (InputRC.isInputLevel(InputCodeRC.LevelLeft))
            {
                selectedObj.transform.position -= d * new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);
            }
            else if (InputRC.isInputLevel(InputCodeRC.LevelRight))
            {
                selectedObj.transform.position += d * new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);
            }
            if (InputRC.isInputLevel(InputCodeRC.LevelDown))
            {
                selectedObj.transform.position -= Vector3.up * d;
            }
            else if (InputRC.isInputLevel(InputCodeRC.LevelUp))
            {
                selectedObj.transform.position += Vector3.up * d;
            }
            if (!selectedObj.name.StartsWith("misc,region"))
            {
                if (InputRC.isInputLevel(InputCodeRC.LevelRRight))
                {
                    selectedObj.transform.Rotate(Vector3.up * d);
                }
                else if (InputRC.isInputLevel(InputCodeRC.LevelRLeft))
                {
                    selectedObj.transform.Rotate(Vector3.down * d);
                }
                if (InputRC.isInputLevel(InputCodeRC.LevelRCCW))
                {
                    selectedObj.transform.Rotate(Vector3.forward * d);
                }
                else if (InputRC.isInputLevel(InputCodeRC.LevelRCW))
                {
                    selectedObj.transform.Rotate(Vector3.back * d);
                }
                if (InputRC.isInputLevel(InputCodeRC.LevelRBack))
                {
                    selectedObj.transform.Rotate(Vector3.left * d);
                }
                else if (InputRC.isInputLevel(InputCodeRC.LevelRForward))
                {
                    selectedObj.transform.Rotate(Vector3.right * d);
                }
            }

            if (InputRC.isInputLevel(InputCodeRC.LevelPlace))
            {
                LinkHash[3].Add(selectedObj.GetInstanceID(), selectedObj.name + "," + Convert.ToString(selectedObj.transform.position.x) + "," + Convert.ToString(selectedObj.transform.position.y) + "," + Convert.ToString(selectedObj.transform.position.z) + "," + Convert.ToString(selectedObj.transform.rotation.x) + "," + Convert.ToString(selectedObj.transform.rotation.y) + "," + Convert.ToString(selectedObj.transform.rotation.z) + "," + Convert.ToString(selectedObj.transform.rotation.w));
                selectedObj = null;
                Camera.main.GetComponent<MouseLook>().enabled = true;
                Screen.lockCursor = true;
            }

            if (InputRC.isInputLevel(InputCodeRC.LevelDelete))
            {
                UnityEngine.Object.Destroy(selectedObj);
                selectedObj = null;
                Camera.main.GetComponent<MouseLook>().enabled = true;
                Screen.lockCursor = true;
                LinkHash[3].Remove(selectedObj.GetInstanceID());
            }
            return;
        }

        if (Screen.lockCursor)
        {
            float d2 = 100f;
            if (InputRC.isInputLevel(InputCodeRC.LevelSlow))
            {
                d2 = 20f;
            }
            else if (InputRC.isInputLevel(InputCodeRC.LevelFast))
            {
                d2 = 400f;
            }
            Transform transform = Camera.main.transform;
            if (InputRC.isInputLevel(InputCodeRC.LevelForward))
            {
                transform.position += transform.forward * d2 * Time.deltaTime;
            }
            else if (InputRC.isInputLevel(InputCodeRC.LevelBack))
            {
                transform.position -= transform.forward * d2 * Time.deltaTime;
            }
            if (InputRC.isInputLevel(InputCodeRC.LevelLeft))
            {
                transform.position -= transform.right * d2 * Time.deltaTime;
            }
            else if (InputRC.isInputLevel(InputCodeRC.LevelRight))
            {
                transform.position += transform.right * d2 * Time.deltaTime;
            }
            if (InputRC.isInputLevel(InputCodeRC.LevelUp))
            {
                transform.position += transform.up * d2 * Time.deltaTime;
            }
            else if (InputRC.isInputLevel(InputCodeRC.LevelDown))
            {
                transform.position -= transform.up * d2 * Time.deltaTime;
            }
        }

        if (InputRC.isInputLevelDown(InputCodeRC.LevelCursor))
        {
            if (Screen.lockCursor)
            {
                Camera.main.GetComponent<MouseLook>().enabled = false;
                Screen.lockCursor = false;
            }
            else
            {
                Camera.main.GetComponent<MouseLook>().enabled = true;
                Screen.lockCursor = true;
            }
        }

        if (!Input.GetKeyDown(KeyCode.Mouse0) || Screen.lockCursor || GUIUtility.hotControl != 0 || ((!(Input.mousePosition.x > 300f) || !(Input.mousePosition.x < (float)Screen.width - 300f)) && !((float)Screen.height - Input.mousePosition.y > 600f)))
        {
            return;
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
        {
            Transform transform2 = hitInfo.transform;
            if (transform2.gameObject.name.StartsWith("custom") || transform2.gameObject.name.StartsWith("base") || transform2.gameObject.name.StartsWith("racing") || transform2.gameObject.name.StartsWith("photon") || transform2.gameObject.name.StartsWith("spawnpoint") || transform2.gameObject.name.StartsWith("misc"))
            {
                selectedObj = transform2.gameObject;
                Camera.main.GetComponent<MouseLook>().enabled = false;
                Screen.lockCursor = true;
                LinkHash[3].Remove(selectedObj.GetInstanceID());
            }
            else if (transform2.parent.gameObject.name.StartsWith("custom") || transform2.parent.gameObject.name.StartsWith("base") || transform2.parent.gameObject.name.StartsWith("racing") || transform2.parent.gameObject.name.StartsWith("photon"))
            {
                selectedObj = transform2.parent.gameObject;
                Camera.main.GetComponent<MouseLook>().enabled = false;
                Screen.lockCursor = true;
                LinkHash[3].Remove(selectedObj.GetInstanceID());
            }
        }
    }

    private void LoadConfig()
    {
        object[] configArray = new object[270]
        {
            PlayerPrefs.GetInt("human", 1),
            PlayerPrefs.GetInt("titan", 1),
            PlayerPrefs.GetInt("level", 1),
            PlayerPrefs.GetString("horse", string.Empty),
            PlayerPrefs.GetString("hair", string.Empty),
            PlayerPrefs.GetString("eye", string.Empty),
            PlayerPrefs.GetString("glass", string.Empty),
            PlayerPrefs.GetString("face", string.Empty),
            PlayerPrefs.GetString("skin", string.Empty),
            PlayerPrefs.GetString("costume", string.Empty),
            PlayerPrefs.GetString("logo", string.Empty),
            PlayerPrefs.GetString("bladel", string.Empty),
            PlayerPrefs.GetString("blader", string.Empty),
            PlayerPrefs.GetString("gas", string.Empty),
            PlayerPrefs.GetString("hoodie", string.Empty),
            PlayerPrefs.GetInt("gasenable", 0),
            PlayerPrefs.GetInt("titantype1", -1),
            PlayerPrefs.GetInt("titantype2", -1),
            PlayerPrefs.GetInt("titantype3", -1),
            PlayerPrefs.GetInt("titantype4", -1),
            PlayerPrefs.GetInt("titantype5", -1),
            PlayerPrefs.GetString("titanhair1", string.Empty),
            PlayerPrefs.GetString("titanhair2", string.Empty),
            PlayerPrefs.GetString("titanhair3", string.Empty),
            PlayerPrefs.GetString("titanhair4", string.Empty),
            PlayerPrefs.GetString("titanhair5", string.Empty),
            PlayerPrefs.GetString("titaneye1", string.Empty),
            PlayerPrefs.GetString("titaneye2", string.Empty),
            PlayerPrefs.GetString("titaneye3", string.Empty),
            PlayerPrefs.GetString("titaneye4", string.Empty),
            PlayerPrefs.GetString("titaneye5", string.Empty),
            0,
            PlayerPrefs.GetInt("titanR", 0),
            PlayerPrefs.GetString("tree1", string.Empty),
            PlayerPrefs.GetString("tree2", string.Empty),
            PlayerPrefs.GetString("tree3", string.Empty),
            PlayerPrefs.GetString("tree4", string.Empty),
            PlayerPrefs.GetString("tree5", string.Empty),
            PlayerPrefs.GetString("tree6", string.Empty),
            PlayerPrefs.GetString("tree7", string.Empty),
            PlayerPrefs.GetString("tree8", string.Empty),
            PlayerPrefs.GetString("leaf1", string.Empty),
            PlayerPrefs.GetString("leaf2", string.Empty),
            PlayerPrefs.GetString("leaf3", string.Empty),
            PlayerPrefs.GetString("leaf4", string.Empty),
            PlayerPrefs.GetString("leaf5", string.Empty),
            PlayerPrefs.GetString("leaf6", string.Empty),
            PlayerPrefs.GetString("leaf7", string.Empty),
            PlayerPrefs.GetString("leaf8", string.Empty),
            PlayerPrefs.GetString("forestG", string.Empty),
            PlayerPrefs.GetInt("forestR", 0),
            PlayerPrefs.GetString("house1", string.Empty),
            PlayerPrefs.GetString("house2", string.Empty),
            PlayerPrefs.GetString("house3", string.Empty),
            PlayerPrefs.GetString("house4", string.Empty),
            PlayerPrefs.GetString("house5", string.Empty),
            PlayerPrefs.GetString("house6", string.Empty),
            PlayerPrefs.GetString("house7", string.Empty),
            PlayerPrefs.GetString("house8", string.Empty),
            PlayerPrefs.GetString("cityG", string.Empty),
            PlayerPrefs.GetString("cityW", string.Empty),
            PlayerPrefs.GetString("cityH", string.Empty),
            PlayerPrefs.GetInt("skinQ", 0),
            PlayerPrefs.GetInt("skinQL", 0),
            0,
            PlayerPrefs.GetString("eren", string.Empty),
            PlayerPrefs.GetString("annie", string.Empty),
            PlayerPrefs.GetString("colossal", string.Empty),
            100,
            "default",
            "1",
            "1",
            "1",
            1f,
            1f,
            1f,
            0,
            string.Empty,
            0,
            "1.0",
            "1.0",
            0,
            PlayerPrefs.GetString("cnumber", "1"),
            "30",
            0,
            PlayerPrefs.GetString("cmax", "20"),
            PlayerPrefs.GetString("titanbody1", string.Empty),
            PlayerPrefs.GetString("titanbody2", string.Empty),
            PlayerPrefs.GetString("titanbody3", string.Empty),
            PlayerPrefs.GetString("titanbody4", string.Empty),
            PlayerPrefs.GetString("titanbody5", string.Empty),
            0,
            PlayerPrefs.GetInt("traildisable", 0),
            PlayerPrefs.GetInt("wind", 0),
            PlayerPrefs.GetString("trailskin", string.Empty),
            PlayerPrefs.GetString("snapshot", "0"),
            PlayerPrefs.GetString("trailskin2", string.Empty),
            PlayerPrefs.GetInt("reel", 0),
            PlayerPrefs.GetString("reelin", "LeftControl"),
            PlayerPrefs.GetString("reelout", "LeftAlt"),
            0,
            PlayerPrefs.GetString("tforward", "W"),
            PlayerPrefs.GetString("tback", "S"),
            PlayerPrefs.GetString("tleft", "A"),
            PlayerPrefs.GetString("tright", "D"),
            PlayerPrefs.GetString("twalk", "LeftShift"),
            PlayerPrefs.GetString("tjump", "Space"),
            PlayerPrefs.GetString("tpunch", "Q"),
            PlayerPrefs.GetString("tslam", "E"),
            PlayerPrefs.GetString("tgrabfront", "Alpha1"),
            PlayerPrefs.GetString("tgrabback", "Alpha3"),
            PlayerPrefs.GetString("tgrabnape", "Mouse1"),
            PlayerPrefs.GetString("tantiae", "Mouse0"),
            PlayerPrefs.GetString("tbite", "Alpha2"),
            PlayerPrefs.GetString("tcover", "Z"),
            PlayerPrefs.GetString("tsit", "X"),
            PlayerPrefs.GetInt("reel2", 0),
            PlayerPrefs.GetString("lforward", "W"),
            PlayerPrefs.GetString("lback", "S"),
            PlayerPrefs.GetString("lleft", "A"),
            PlayerPrefs.GetString("lright", "D"),
            PlayerPrefs.GetString("lup", "Mouse1"),
            PlayerPrefs.GetString("ldown", "Mouse0"),
            PlayerPrefs.GetString("lcursor", "X"),
            PlayerPrefs.GetString("lplace", "Space"),
            PlayerPrefs.GetString("ldel", "Backspace"),
            PlayerPrefs.GetString("lslow", "LeftShift"),
            PlayerPrefs.GetString("lrforward", "R"),
            PlayerPrefs.GetString("lrback", "F"),
            PlayerPrefs.GetString("lrleft", "Q"),
            PlayerPrefs.GetString("lrright", "E"),
            PlayerPrefs.GetString("lrccw", "Z"),
            PlayerPrefs.GetString("lrcw", "C"),
            PlayerPrefs.GetInt("humangui", 0),
            PlayerPrefs.GetString("horse2", string.Empty),
            PlayerPrefs.GetString("hair2", string.Empty),
            PlayerPrefs.GetString("eye2", string.Empty),
            PlayerPrefs.GetString("glass2", string.Empty),
            PlayerPrefs.GetString("face2", string.Empty),
            PlayerPrefs.GetString("skin2", string.Empty),
            PlayerPrefs.GetString("costume2", string.Empty),
            PlayerPrefs.GetString("logo2", string.Empty),
            PlayerPrefs.GetString("bladel2", string.Empty),
            PlayerPrefs.GetString("blader2", string.Empty),
            PlayerPrefs.GetString("gas2", string.Empty),
            PlayerPrefs.GetString("hoodie2", string.Empty),
            PlayerPrefs.GetString("trail2", string.Empty),
            PlayerPrefs.GetString("horse3", string.Empty),
            PlayerPrefs.GetString("hair3", string.Empty),
            PlayerPrefs.GetString("eye3", string.Empty),
            PlayerPrefs.GetString("glass3", string.Empty),
            PlayerPrefs.GetString("face3", string.Empty),
            PlayerPrefs.GetString("skin3", string.Empty),
            PlayerPrefs.GetString("costume3", string.Empty),
            PlayerPrefs.GetString("logo3", string.Empty),
            PlayerPrefs.GetString("bladel3", string.Empty),
            PlayerPrefs.GetString("blader3", string.Empty),
            PlayerPrefs.GetString("gas3", string.Empty),
            PlayerPrefs.GetString("hoodie3", string.Empty),
            PlayerPrefs.GetString("trail3", string.Empty),
            null,
            PlayerPrefs.GetString("lfast", "LeftControl"),
            PlayerPrefs.GetString("customGround", string.Empty),
            PlayerPrefs.GetString("forestskyfront", string.Empty),
            PlayerPrefs.GetString("forestskyback", string.Empty),
            PlayerPrefs.GetString("forestskyleft", string.Empty),
            PlayerPrefs.GetString("forestskyright", string.Empty),
            PlayerPrefs.GetString("forestskyup", string.Empty),
            PlayerPrefs.GetString("forestskydown", string.Empty),
            PlayerPrefs.GetString("cityskyfront", string.Empty),
            PlayerPrefs.GetString("cityskyback", string.Empty),
            PlayerPrefs.GetString("cityskyleft", string.Empty),
            PlayerPrefs.GetString("cityskyright", string.Empty),
            PlayerPrefs.GetString("cityskyup", string.Empty),
            PlayerPrefs.GetString("cityskydown", string.Empty),
            PlayerPrefs.GetString("customskyfront", string.Empty),
            PlayerPrefs.GetString("customskyback", string.Empty),
            PlayerPrefs.GetString("customskyleft", string.Empty),
            PlayerPrefs.GetString("customskyright", string.Empty),
            PlayerPrefs.GetString("customskyup", string.Empty),
            PlayerPrefs.GetString("customskydown", string.Empty),
            PlayerPrefs.GetInt("dashenable", 0),
            PlayerPrefs.GetString("dashkey", "RightControl"),
            PlayerPrefs.GetInt("vsync", 0),
            PlayerPrefs.GetString("fpscap", "0"),
            0,
            0,
            0,
            0,
            PlayerPrefs.GetInt("speedometer", 0),
            0,
            string.Empty,
            PlayerPrefs.GetInt("bombMode", 0),
            PlayerPrefs.GetInt("teamMode", 0),
            PlayerPrefs.GetInt("rockThrow", 0),
            PlayerPrefs.GetInt("explodeModeOn", 0),
            PlayerPrefs.GetString("explodeModeNum", "30"),
            PlayerPrefs.GetInt("healthMode", 0),
            PlayerPrefs.GetString("healthLower", "100"),
            PlayerPrefs.GetString("healthUpper", "200"),
            PlayerPrefs.GetInt("infectionModeOn", 0),
            PlayerPrefs.GetString("infectionModeNum", "1"),
            PlayerPrefs.GetInt("banEren", 0),
            PlayerPrefs.GetInt("moreTitanOn", 0),
            PlayerPrefs.GetString("moreTitanNum", "1"),
            PlayerPrefs.GetInt("damageModeOn", 0),
            PlayerPrefs.GetString("damageModeNum", "1000"),
            PlayerPrefs.GetInt("sizeMode", 0),
            PlayerPrefs.GetString("sizeLower", "1.0"),
            PlayerPrefs.GetString("sizeUpper", "3.0"),
            PlayerPrefs.GetInt("spawnModeOn", 0),
            PlayerPrefs.GetString("nRate", "20.0"),
            PlayerPrefs.GetString("aRate", "20.0"),
            PlayerPrefs.GetString("jRate", "20.0"),
            PlayerPrefs.GetString("cRate", "20.0"),
            PlayerPrefs.GetString("pRate", "20.0"),
            PlayerPrefs.GetInt("horseMode", 0),
            PlayerPrefs.GetInt("waveModeOn", 0),
            PlayerPrefs.GetString("waveModeNum", "1"),
            PlayerPrefs.GetInt("friendlyMode", 0),
            PlayerPrefs.GetInt("pvpMode", 0),
            PlayerPrefs.GetInt("maxWaveOn", 0),
            PlayerPrefs.GetString("maxWaveNum", "20"),
            PlayerPrefs.GetInt("endlessModeOn", 0),
            PlayerPrefs.GetString("endlessModeNum", "10"),
            PlayerPrefs.GetString("motd", string.Empty),
            PlayerPrefs.GetInt("pointModeOn", 0),
            PlayerPrefs.GetString("pointModeNum", "50"),
            PlayerPrefs.GetInt("ahssReload", 0),
            PlayerPrefs.GetInt("punkWaves", 0),
            0,
            PlayerPrefs.GetInt("mapOn", 0),
            PlayerPrefs.GetString("mapMaximize", "Tab"),
            PlayerPrefs.GetString("mapToggle", "M"),
            PlayerPrefs.GetString("mapReset", "K"),
            PlayerPrefs.GetInt("globalDisableMinimap", 0),
            PlayerPrefs.GetString("chatRebind", "None"),
            PlayerPrefs.GetString("hforward", "W"),
            PlayerPrefs.GetString("hback", "S"),
            PlayerPrefs.GetString("hleft", "A"),
            PlayerPrefs.GetString("hright", "D"),
            PlayerPrefs.GetString("hwalk", "LeftShift"),
            PlayerPrefs.GetString("hjump", "Q"),
            PlayerPrefs.GetString("hmount", "LeftControl"),
            PlayerPrefs.GetInt("chatfeed", 0),
            0,
            PlayerPrefs.GetFloat("bombR", 1f),
            PlayerPrefs.GetFloat("bombG", 1f),
            PlayerPrefs.GetFloat("bombB", 1f),
            PlayerPrefs.GetFloat("bombA", 1f),
            PlayerPrefs.GetFloat("bombRadius", 5),
            PlayerPrefs.GetFloat("bombRange", 5),
            PlayerPrefs.GetFloat("bombSpeed", 5),
            PlayerPrefs.GetFloat("bombCD", 5),
            PlayerPrefs.GetString("cannonUp", "W"),
            PlayerPrefs.GetString("cannonDown", "S"),
            PlayerPrefs.GetString("cannonLeft", "A"),
            PlayerPrefs.GetString("cannonRight", "D"),
            PlayerPrefs.GetString("cannonFire", "Q"),
            PlayerPrefs.GetString("cannonMount", "G"),
            PlayerPrefs.GetString("cannonSlow", "LeftShift"),
            PlayerPrefs.GetInt("deadlyCannon", 0),
            PlayerPrefs.GetString("liveCam", "Y"),
            0,
            null,
            null,
            null,
            null,
            null,
            null
        };
        InputRC = new InputManagerRC();
        InputRC.setInputHuman(InputCodeRC.ReelIn, (string)configArray[98]);
        InputRC.setInputHuman(InputCodeRC.ReelOut, (string)configArray[99]);
        InputRC.setInputHuman(InputCodeRC.Dash, (string)configArray[182]);
        InputRC.setInputHuman(InputCodeRC.MapMaximize, (string)configArray[232]);
        InputRC.setInputHuman(InputCodeRC.MapToggle, (string)configArray[233]);
        InputRC.setInputHuman(InputCodeRC.MapReset, (string)configArray[234]);
        InputRC.setInputHuman(InputCodeRC.Chat, (string)configArray[236]);
        InputRC.setInputHuman(InputCodeRC.LiveCamera, (string)configArray[262]);
        if (!Enum.IsDefined(typeof(KeyCode), (string)configArray[232]))
        {
            configArray[232] = "None";
        }
        if (!Enum.IsDefined(typeof(KeyCode), (string)configArray[233]))
        {
            configArray[233] = "None";
        }
        if (!Enum.IsDefined(typeof(KeyCode), (string)configArray[234]))
        {
            configArray[234] = "None";
        }
        for (int i = 0; i < 15; i++)
        {
            InputRC.setInputTitan(i, (string)configArray[101 + i]);
        }
        for (int i = 0; i < 16; i++)
        {
            InputRC.setInputLevel(i, (string)configArray[117 + i]);
        }
        for (int i = 0; i < 7; i++)
        {
            InputRC.setInputHorse(i, (string)configArray[237 + i]);
        }
        for (int i = 0; i < 7; i++)
        {
            InputRC.setInputCannon(i, (string)configArray[254 + i]);
        }
        InputRC.setInputLevel(InputCodeRC.LevelFast, (string)configArray[161]);
        Application.targetFrameRate = -1;
        if (int.TryParse((string)configArray[184], out int result) && result > 0)
        {
            Application.targetFrameRate = result;
        }
        QualitySettings.vSyncCount = (int)configArray[183] == 1 ? 1 : 0;
        AudioListener.volume = PlayerPrefs.GetFloat("vol", 1f);
        QualitySettings.masterTextureLimit = PlayerPrefs.GetInt("skinQ", 0);
        LinkHash = new ExitGames.Client.Photon.Hashtable[5]
        {
            new ExitGames.Client.Photon.Hashtable(),
            new ExitGames.Client.Photon.Hashtable(),
            new ExitGames.Client.Photon.Hashtable(),
            new ExitGames.Client.Photon.Hashtable(),
            new ExitGames.Client.Photon.Hashtable()
        };
        Settings = configArray;
        scroll = Vector2.zero;
        scroll2 = Vector2.zero;
        distanceSlider = PlayerPrefs.GetFloat("cameraDistance", 1f);
        mouseSlider = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        qualitySlider = PlayerPrefs.GetFloat("GameQuality", 0f);
        transparencySlider = 1f;
    }

    public void LoadSkin()
    {
        if ((int)Settings[64] >= 100)
        {
            string[] array = new string[5]
            {
                "Flare",
                "LabelInfoBottomRight",
                "LabelNetworkStatus",
                "skill_cd_bottom",
                "GasUI"
            };
            foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (gameObject.name.Contains("TREE") || gameObject.name.Contains("aot_supply") || gameObject.name.Contains("gameobjectOutSide"))
                {
                    UnityEngine.Object.Destroy(gameObject);
                }
            }
            GameObject.Find("Cube_001").renderer.material.mainTexture = ((Material)RCAssets.Load("grass")).mainTexture;
            UnityEngine.Object.Instantiate(RCAssets.Load("spawnPlayer"), new Vector3(-10f, 1f, -10f), new Quaternion(0f, 0f, 0f, 1f));
            foreach (string text in array)
            {
                GameObject gameObject2 = GameObject.Find(text);
                if (gameObject2 != null)
                {
                    UnityEngine.Object.Destroy(gameObject2);
                }
            }
            Camera.main.GetComponent<SpectatorMovement>().disable = true;
            return;
        }
        InstantiateTracker.Instance.Dispose();
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && PhotonNetwork.isMasterClient)
        {
            updateTime = 1f;
            if (OldScriptLogic != CurrentScriptLogic)
            {
                IntVariables.Clear();
                BoolVariables.Clear();
                StringVariables.Clear();
                FloatVariables.Clear();
                GlobalVariables.Clear();
                RCEvents.Clear();
                RCVariableNames.Clear();
                PlayerVariables.Clear();
                TitanVariables.Clear();
                RCRegionTriggers.Clear();
                OldScriptLogic = CurrentScriptLogic;
                CompileScript(CurrentScriptLogic);
                if (RCEvents.ContainsKey("OnFirstLoad"))
                {
                    RCEvent rCEvent = (RCEvent)RCEvents["OnFirstLoad"];
                    rCEvent.CheckEvent();
                }
            }
            if (RCEvents.ContainsKey("OnRoundStart"))
            {
                RCEvent rCEvent = (RCEvent)RCEvents["OnRoundStart"];
                rCEvent.CheckEvent();
            }
            base.photonView.RPC("setMasterRC", PhotonTargets.All);
        }
        LogicLoaded = true;
        racingSpawnPoint = new Vector3(0f, 0f, 0f);
        racingSpawnPointSet = false;
        racingDoors = new List<GameObject>();
        allowedToCannon = new Dictionary<int, CannonValues>();
        if (!Level.Name.StartsWith("Custom") && (int)Settings[2] == 1)
        {
            GameObject gameObject3 = GameObject.Find("aot_supply");
            if (gameObject3 != null && Minimap.Instance != null)
            {
                Minimap.Instance.TrackGameObjectOnMinimap(gameObject3, Color.white, trackOrientation: false, depthAboveAll: true, Minimap.IconStyle.SUPPLY);
            }
            string text2 = string.Empty;
            string text3 = string.Empty;
            string text4 = string.Empty;
            string[] array3 = new string[6] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
            if (Level.Map.Contains("City"))
            {
                for (int i = 51; i < 59; i++)
                {
                    text2 = text2 + (string)Settings[i] + ",";
                }
                text2.TrimEnd(',');
                for (int j = 0; j < 250; j++)
                {
                    text4 += Convert.ToString((int)UnityEngine.Random.Range(0f, 8f));
                }
                text3 = (string)Settings[59] + "," + (string)Settings[60] + "," + (string)Settings[61];
                for (int i = 0; i < 6; i++)
                {
                    array3[i] = (string)Settings[i + 169];
                }
            }
            else if (Level.Map.Contains("Forest"))
            {
                for (int k = 33; k < 41; k++)
                {
                    text2 = text2 + (string)Settings[k] + ",";
                }
                text2.TrimEnd(',');
                for (int l = 41; l < 49; l++)
                {
                    text3 = text3 + (string)Settings[l] + ",";
                }
                text3 += (string)Settings[49];
                for (int m = 0; m < 150; m++)
                {
                    string str = Convert.ToString((int)UnityEngine.Random.Range(0f, 8f));
                    text4 += str;
                    text4 = (((int)Settings[50] != 0) ? (text4 + Convert.ToString((int)UnityEngine.Random.Range(0f, 8f))) : (text4 + str));
                }
                for (int i = 0; i < 6; i++)
                {
                    array3[i] = (string)Settings[i + 163];
                }
            }

            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                StartCoroutine(CoLoadSkin(text4, text2, text3, array3));
            }
            else if (PhotonNetwork.isMasterClient)
            {
                base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, text4, text2, text3, array3);
            }
        }
        else if (Level.Name.StartsWith("Custom"))
        {
            foreach (GameObject gameObject3 in GameObject.FindGameObjectsWithTag("playerRespawn"))
            {
                gameObject3.transform.position = new Vector3(UnityEngine.Random.Range(-5f, 5f), 0f, UnityEngine.Random.Range(-5f, 5f));
            }
            foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (gameObject.name.Contains("TREE") || gameObject.name.Contains("aot_supply"))
                {
                    UnityEngine.Object.Destroy(gameObject);
                }
                else if (gameObject.name == "Cube_001" && gameObject.transform.parent.gameObject.tag != "player" && gameObject.renderer != null)
                {
                    groundList.Add(gameObject);
                    gameObject.renderer.material.mainTexture = ((Material)RCAssets.Load("grass")).mainTexture;
                }
            }
            if (!PhotonNetwork.isMasterClient)
            {
                return;
            }
            string[] array3 = new string[7]
            {
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty
            };
            for (int i = 0; i < 6; i++)
            {
                array3[i] = (string)Settings[i + 175];
            }
            array3[6] = (string)Settings[162];
            if (int.TryParse((string)Settings[85], out int result))
            {
                RCSettings.TitanCap = result;
            }
            else
            {
                RCSettings.TitanCap = 0;
                Settings[85] = "0";
            }
            RCSettings.TitanCap = Math.Min(50, RCSettings.TitanCap);
            base.photonView.RPC("clearlevel", PhotonTargets.AllBuffered, array3, RCSettings.GameType);
            RCRegions.Clear();
            if (OldScript != CurrentScript)
            {
                levelCache.Clear();
                titanSpawns.Clear();
                playerSpawnsC.Clear();
                playerSpawnsM.Clear();
                titanSpawners.Clear();
                CurrentLevel = string.Empty;
                if (CurrentScript == string.Empty)
                {
                    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.CurrentLevel, CurrentLevel);
                    PhotonNetwork.player.SetCustomProperties(hashtable);
                    OldScript = CurrentScript;
                }
                else
                {
                    string[] array5 = Regex.Replace(CurrentScript, "\\s+", string.Empty).Replace("\r\n", string.Empty).Replace('\n', '\0').Replace('\r', '\0').Split(';');
                    for (int i = 0; i < Mathf.FloorToInt((array5.Length - 1) / 100) + 1; i++)
                    {
                        string[] array6;
                        int num;
                        if (i < Mathf.FloorToInt(array5.Length / 100))
                        {
                            array6 = new string[101];
                            num = 0;
                            for (int j = 100 * i; j < 100 * i + 100; j++)
                            {
                                if (array5[j].StartsWith("spawnpoint"))
                                {
                                    string[] array7 = array5[j].Split(',');

                                    switch (array7[1])
                                    {
                                        case "titan":
                                            titanSpawns.Add(new Vector3(Convert.ToSingle(array7[2]), Convert.ToSingle(array7[3]), Convert.ToSingle(array7[4])));
                                            break;
                                        case "playerC":
                                            playerSpawnsC.Add(new Vector3(Convert.ToSingle(array7[2]), Convert.ToSingle(array7[3]), Convert.ToSingle(array7[4])));
                                            break;
                                        case "playerM":
                                            playerSpawnsM.Add(new Vector3(Convert.ToSingle(array7[2]), Convert.ToSingle(array7[3]), Convert.ToSingle(array7[4])));
                                            break;
                                    }
                                }
                                array6[num] = array5[j];
                                num++;
                            }
                            CurrentLevel += (array6[100] = UnityEngine.Random.Range(10000, 99999).ToString());
                            levelCache.Add(array6);
                            continue;
                        }
                        array6 = new string[array5.Length % 100 + 1];
                        num = 0;
                        for (int j = 100 * i; j < 100 * i + array5.Length % 100; j++)
                        {
                            if (array5[j].StartsWith("spawnpoint"))
                            {
                                string[] array7 = array5[j].Split(',');
                                switch (array7[1])
                                {
                                    case "titan":
                                        titanSpawns.Add(new Vector3(Convert.ToSingle(array7[2]), Convert.ToSingle(array7[3]), Convert.ToSingle(array7[4])));
                                        break;
                                    case "playerC":
                                        playerSpawnsC.Add(new Vector3(Convert.ToSingle(array7[2]), Convert.ToSingle(array7[3]), Convert.ToSingle(array7[4])));
                                        break;
                                    case "playerM":
                                        playerSpawnsM.Add(new Vector3(Convert.ToSingle(array7[2]), Convert.ToSingle(array7[3]), Convert.ToSingle(array7[4])));
                                        break;
                                }
                            }
                            array6[num] = array5[j];
                            num++;
                        }
                        string text5 = UnityEngine.Random.Range(10000, 99999).ToString();
                        array6[array5.Length % 100] = text5;
                        CurrentLevel += text5;
                        levelCache.Add(array6);
                    }
                    List<string> list = new List<string>();
                    foreach (Vector3 titanSpawn in titanSpawns)
                    {
                        string[] array8 = new string[6] { "titan,", titanSpawn.x.ToString(), ",", titanSpawn.y.ToString(), ",", titanSpawn.z.ToString() };
                        list.Add(string.Concat(array8));
                    }
                    foreach (Vector3 item in playerSpawnsC)
                    {
                        string[] array8 = new string[6] { "playerC,", item.x.ToString(), ",", item.y.ToString(), ",", item.z.ToString() };
                        list.Add(string.Concat(array8));
                    }
                    foreach (Vector3 item2 in playerSpawnsM)
                    {
                        string[] array8 = new string[6] { "playerM,", item2.x.ToString(), ",", item2.y.ToString(), ",", item2.z.ToString() };
                        list.Add(string.Concat(array8));
                    }
                    string text6 = "a" + UnityEngine.Random.Range(10000, 99999).ToString();
                    list.Add(text6);
                    CurrentLevel = text6 + CurrentLevel;
                    levelCache.Insert(0, list.ToArray());
                    string text7 = "z" + UnityEngine.Random.Range(10000, 99999).ToString();
                    levelCache.Add(new string[1] { text7 });
                    CurrentLevel += text7;
                    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.CurrentLevel, CurrentLevel);
                    PhotonNetwork.player.SetCustomProperties(hashtable);
                    OldScript = CurrentScript;
                }
            }
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (player.isMasterClient)
                {
                    continue;
                }
                otherUsers.Add(player);
            }
            StartCoroutine(CoLoadCustomLevel(otherUsers));
            StartCoroutine(CoCacheCustomLevel());
        }
    }

    // Replaced with Anarchy's Custom Level code
    private void CustomLevelClientE(string[] content, bool renewHash)
    {
        int num;
        string[] strArray;
        bool flag = false;
        bool flag2 = false;
        if (content[content.Length - 1].StartsWith("a"))
        {
            flag = true;
        }
        else if (content[content.Length - 1].StartsWith("z"))
        {
            flag2 = true;
            CustomLevelLoaded = true;
            this.spawnPlayerCustomMap();
            Minimap.TryRecaptureInstance();
            this.UnloadAssets();
            Camera.main.GetComponent<TiltShift>().enabled = false;
        }

        if (renewHash)
        {
            if (flag)
            {
                CurrentLevel = string.Empty;
                this.levelCache.Clear();
                this.titanSpawns.Clear();
                this.playerSpawnsC.Clear();
                this.playerSpawnsM.Clear();
                for (num = 0; num < content.Length; num++)
                {
                    strArray = content[num].Split(new char[] { ',' });
                    if (strArray[0] == "titan")
                    {
                        this.titanSpawns.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                    else if (strArray[0] == "playerC")
                    {
                        this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                    else if (strArray[0] == "playerM")
                    {
                        this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                }
                this.spawnPlayerCustomMap();
            }
            CurrentLevel = CurrentLevel + content[content.Length - 1];
            this.levelCache.Add(content);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.CurrentLevel, CurrentLevel);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }

        if (!flag && !flag2)
        {
            for (num = 0; num < content.Length; num++)
            {
                float num2;
                GameObject obj2;
                float num3;
                float num5;
                float num6;
                float num7;
                Color color;
                Mesh mesh;
                Color[] colorArray;
                int num8;

                GameObject resultObject = null;
                strArray = content[num].Split(new char[] { ',' });
                if (strArray[0].StartsWith("custom"))
                {
                    num2 = 1f;
                    obj2 = null;
                    obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])));
                    if (strArray[2] != "default")
                    {
                        if (strArray[2].StartsWith("transparent"))
                        {
                            if (float.TryParse(strArray[2].Substring(11), out num3))
                            {
                                num2 = num3;
                            }
                            foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material = (Material)RCAssets.Load("transparent");
                                if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                {
                                    renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                }
                            }
                        }
                        else
                        {
                            foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material = (Material)RCAssets.Load(strArray[2]);
                                if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                {
                                    renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                }
                            }
                        }
                    }
                    num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[3]);
                    num5 -= 0.001f;
                    num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[4]);
                    num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[5]);
                    obj2.transform.localScale = new Vector3(num5, num6, num7);
                    if (strArray[6] != "0")
                    {
                        color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), num2);
                        foreach (MeshFilter filter in obj2.GetComponentsInChildren<MeshFilter>())
                        {
                            mesh = filter.mesh;
                            colorArray = new Color[mesh.vertexCount];
                            num8 = 0;
                            while (num8 < mesh.vertexCount)
                            {
                                colorArray[num8] = color;
                                num8++;
                            }
                            mesh.colors = colorArray;
                        }
                    }
                    resultObject = obj2;
                }
                else if (strArray[0].StartsWith("base"))
                {
                    if (strArray.Length < 15)
                    {
                        resultObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])));
                    }
                    else
                    {
                        num2 = 1f;
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])));
                        if (strArray[2] != "default")
                        {
                            if (strArray[2].StartsWith("transparent"))
                            {
                                if (float.TryParse(strArray[2].Substring(11), out num3))
                                {
                                    num2 = num3;
                                }
                                foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                                {
                                    renderer.material = (Material)RCAssets.Load("transparent");
                                    if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                    {
                                        renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                    }
                                }
                            }
                            else
                            {
                                foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                                {
                                    if (!renderer.name.Contains("Particle System") || !obj2.name.Contains("aot_supply"))
                                    {
                                        renderer.material = (Material)RCAssets.Load(strArray[2]);
                                        if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                        {
                                            renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                        }
                                    }
                                }
                            }
                        }
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[3]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[4]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[5]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (strArray[6] != "0")
                        {
                            color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), num2);
                            foreach (MeshFilter filter in obj2.GetComponentsInChildren<MeshFilter>())
                            {
                                mesh = filter.mesh;
                                colorArray = new Color[mesh.vertexCount];
                                for (num8 = 0; num8 < mesh.vertexCount; num8++)
                                {
                                    colorArray[num8] = color;
                                }
                                mesh.colors = colorArray;
                            }
                        }
                        resultObject = obj2;
                    }
                }
                else if (strArray[0].StartsWith("misc"))
                {
                    if (strArray[1].StartsWith("barrier"))
                    {
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        resultObject = obj2;
                    }
                    else if (strArray[1].StartsWith("racingStart"))
                    {
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (this.racingDoors != null)
                        {
                            this.racingDoors.Add(obj2);
                        }
                        resultObject = obj2;
                    }
                    else if (strArray[1].StartsWith("racingEnd"))
                    {
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.AddComponent<LevelTriggerRacingEnd>();
                        resultObject = obj2;
                    }
                    else if (strArray[1].StartsWith("region") && PhotonNetwork.isMasterClient)
                    {
                        Vector3 loc = new Vector3(Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]));
                        RCRegion region = new RCRegion(loc, Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4]), Convert.ToSingle(strArray[5]));
                        string key = strArray[2];
                        if (RCRegionTriggers.ContainsKey(key))
                        {
                            GameObject obj3 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load("region"));
                            obj3.transform.position = loc;
                            obj3.AddComponent<RegionTrigger>();
                            obj3.GetComponent<RegionTrigger>().CopyTrigger((RegionTrigger)RCRegionTriggers[key]);
                            num5 = obj3.transform.localScale.x * Convert.ToSingle(strArray[3]);
                            num5 -= 0.001f;
                            num6 = obj3.transform.localScale.y * Convert.ToSingle(strArray[4]);
                            num7 = obj3.transform.localScale.z * Convert.ToSingle(strArray[5]);
                            obj3.transform.localScale = new Vector3(num5, num6, num7);
                            region.myBox = obj3;
                        }
                        RCRegions.Add(key, region);
                    }
                }
                else if (strArray[0].StartsWith("racing"))
                {
                    if (strArray[1].StartsWith("start"))
                    {
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (this.racingDoors != null)
                        {
                            this.racingDoors.Add(obj2);
                        }
                        resultObject = obj2;
                    }
                    else if (strArray[1].StartsWith("end"))
                    {
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<LevelTriggerRacingEnd>();
                        resultObject = obj2;
                    }
                    else if (strArray[1].StartsWith("kill"))
                    {
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingKillTrigger>();
                        resultObject = obj2;
                    }
                    else if (strArray[1].StartsWith("checkpoint"))
                    {
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingCheckpointTrigger>();
                        resultObject = obj2;
                    }
                }
                else if (strArray[0].StartsWith("map"))
                {
                    if (strArray[1].StartsWith("disablebounds"))
                    {
                        UnityEngine.Object.Destroy(GameObject.Find("gameobjectOutSide"));
                        UnityEngine.Object.Instantiate(RCAssets.Load("outside"));
                    }
                }
                else if (PhotonNetwork.isMasterClient && strArray[0].StartsWith("photon"))
                {
                    if (strArray[1].StartsWith("Cannon"))
                    {
                        if (strArray.Length > 15)
                        {
                            GameObject go = PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])), 0);
                            go.GetComponent<CannonPropRegion>().settings = content[num];
                            go.GetPhotonView().RPC("SetSize", PhotonTargets.AllBuffered, new object[] { content[num] });
                        }
                        else
                        {
                            PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0).GetComponent<CannonPropRegion>().settings = content[num];
                        }
                    }
                    else
                    {
                        TitanSpawner item = new TitanSpawner();
                        num5 = 30f;
                        if (float.TryParse(strArray[2], out num3))
                        {
                            num5 = Mathf.Max(Convert.ToSingle(strArray[2]), 1f);
                        }
                        item.time = num5;
                        item.delay = num5;
                        item.name = strArray[1];
                        if (strArray[3] == "1")
                        {
                            item.endless = true;
                        }
                        else
                        {
                            item.endless = false;
                        }
                        item.location = new Vector3(Convert.ToSingle(strArray[4]), Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]));
                        this.titanSpawners.Add(item);
                    }
                }

                if (resultObject != null)
                {
                    Anarchy.Custom.Level.CustomAnarchyLevel.Instance.TryAddAnarchyScripts(resultObject, strArray);
                }
            }
        }
    }

    private IEnumerator CoCacheCustomLevel()
    {
        for (int i = 0; i < levelCache.Count; i++)
        {
            CustomLevelClientE(levelCache[i], renewHash: false);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CoLoadCustomLevel(List<PhotonPlayer> players)
    {
        if (CurrentLevel == string.Empty)
        {
            string[] array = new string[1] { "loadempty" };
            foreach (PhotonPlayer player in players)
            {
                base.photonView.RPC("customlevelRPC", player, new object[] { array });
            }
            CustomLevelLoaded = true;
            yield break;
        }
        for (int i = 0; i < levelCache.Count; i++)
        {
            foreach (PhotonPlayer player in players)
            {
                if (player.customProperties[PhotonPlayerProperty.CurrentLevel] != null && CurrentLevel.Length > 0 && GExtensions.AsString(player.customProperties[PhotonPlayerProperty.CurrentLevel]) == CurrentLevel)
                {
                    if (i == 0)
                    {
                        string[] array = new string[1] { "loadcached" };
                        base.photonView.RPC("customlevelRPC", player, new object[] { array });
                    }
                }
                else
                {
                    base.photonView.RPC("customlevelRPC", player, new object[] { levelCache[i] });
                }
            }

            if (i > 0)
            {
                yield return new WaitForSeconds(0.75f);
            }
            else
            {
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    private IEnumerator CoClearLevel(string[] skybox)
    {
        string linkGround = skybox[6];
        bool mipmapping = true;
        bool unload = false;
        if ((int)Settings[63] == 1)
        {
            mipmapping = false;
        }

        // Load skybox skin
        if (skybox[0] != string.Empty || skybox[1] != string.Empty || skybox[2] != string.Empty || skybox[3] != string.Empty || skybox[4] != string.Empty || skybox[5] != string.Empty)
        {
            string key = string.Join(",", skybox);
            if (!LinkHash[1].ContainsKey(key))
            {
                unload = true;
                Material newSky = new Material(Camera.main.GetComponent<Skybox>().material);
                string skyFront = skybox[0];
                string skyBack = skybox[1];
                string skyLeft = skybox[2];
                string skyRight = skybox[3];
                string skyUp = skybox[4];
                string skyDown = skybox[5];

                // Old limit: 500KB
                if (skyFront.EndsWith(".jpg") || skyFront.EndsWith(".png") || skyFront.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyFront);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;

                            Texture2D frontSkyTex = RCextensions.LoadImage(www, mipmapping, 2000000);
                            newSky.SetTexture("_FrontTex", frontSkyTex);
                        }
                    }
                }
                if (skyBack.EndsWith(".jpg") || skyBack.EndsWith(".png") || skyBack.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyBack);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;

                            Texture2D backSkyTex = RCextensions.LoadImage(www, mipmapping, 2000000);
                            newSky.SetTexture("_BackTex", backSkyTex);
                        }
                    }
                }
                if (skyLeft.EndsWith(".jpg") || skyLeft.EndsWith(".png") || skyLeft.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyLeft);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;

                            Texture2D leftSkyTex = RCextensions.LoadImage(www, mipmapping, 2000000);
                            newSky.SetTexture("_LeftTex", leftSkyTex);
                        }
                    }
                }
                if (skyRight.EndsWith(".jpg") || skyRight.EndsWith(".png") || skyRight.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyRight);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;

                            Texture2D rightSkyTex = RCextensions.LoadImage(www, mipmapping, 2000000);
                            newSky.SetTexture("_RightTex", rightSkyTex);
                        }
                    }
                }
                if (skyUp.EndsWith(".jpg") || skyUp.EndsWith(".png") || skyUp.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyUp);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;

                            Texture2D upSkyTex = RCextensions.LoadImage(www, mipmapping, 2000000);
                            newSky.SetTexture("_UpTex", upSkyTex);
                        }
                    }
                }
                if (skyDown.EndsWith(".jpg") || skyDown.EndsWith(".png") || skyDown.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyDown);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;

                            Texture2D downSkyTex = RCextensions.LoadImage(www, mipmapping, 2000000);
                            newSky.SetTexture("_DownTex", downSkyTex);
                        }
                    }
                }
                Camera.main.GetComponent<Skybox>().material = newSky;
                LinkHash[1].Add(key, newSky);
                SkyMaterial = newSky;
            }
            else
            {
                Camera.main.GetComponent<Skybox>().material = (Material)LinkHash[1][key];
                SkyMaterial = (Material)LinkHash[1][key];
            }
        }

        // Load ground skin
        if (linkGround.EndsWith(".jpg") || linkGround.EndsWith(".png") || linkGround.EndsWith(".jpeg"))
        {
            foreach (GameObject ground in groundList)
            {
                if (ground != null && ground.renderer != null)
                {
                    try
                    {
                        foreach (Renderer renderer in ground.GetComponentsInChildren<Renderer>())
                        {
                            if (!LinkHash[0].ContainsKey(linkGround))
                            {
                                WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(linkGround);
                                if (www != null)
                                {
                                    using (www)
                                    {
                                        yield return www;

                                        Texture2D groundTex = RCextensions.LoadImage(www, mipmapping, 200000);

                                        unload = true;
                                        renderer.material.mainTexture = groundTex;
                                        LinkHash[0].Add(linkGround, renderer.material);
                                    }
                                    renderer.material = (Material)LinkHash[0][linkGround];
                                }
                            }
                            else
                            {
                                renderer.material = (Material)LinkHash[0][linkGround];
                            }
                        }
                    }
                    finally
                    {
                    }
                }
            }
        }
        else if (linkGround.ToLower() == "transparent")
        {
            foreach (GameObject ground in groundList)
            {
                if (ground != null && ground.renderer != null)
                {
                    foreach (Renderer renderer in ground.GetComponentsInChildren<Renderer>())
                    {
                        renderer.enabled = false;
                    }
                }
            }
        }

        if (unload)
        {
            UnloadAssets();
        }
    }

    [RPC]
    private void customlevelRPC(string[] content, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            if (content.Length == 1 && content[0] == "loadcached")
            {
                StartCoroutine(CoCacheCustomLevel());
            }
            else if (content.Length == 1 && content[0] == "loadempty")
            {
                CurrentLevel = string.Empty;
                levelCache.Clear();
                titanSpawns.Clear();
                playerSpawnsC.Clear();
                playerSpawnsM.Clear();
                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable.Add(PhotonPlayerProperty.CurrentLevel, CurrentLevel);
                PhotonNetwork.player.SetCustomProperties(hashtable);
                CustomLevelLoaded = true;
                spawnPlayerCustomMap();
            }
            else
            {
                CustomLevelClientE(content, renewHash: true);
            }
        }
    }

    [RPC]
    private void clearlevel(string[] link, int gametype, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            switch (gametype)
            {
                case 0:
                    Level.Mode = GameMode.KillTitans;
                    break;
                case 1:
                    Level.Mode = GameMode.Survival;
                    break;
                case 2:
                    Level.Mode = GameMode.TeamDeathmatch;
                    break;
                case 3:
                    Level.Mode = GameMode.Racing;
                    break;
                case 4:
                    Level.Mode = GameMode.None;
                    break;
            }

            if (link.Length > 6 && (int)Settings[2] == 1)
            {
                StartCoroutine(CoClearLevel(link));
            }
        }
    }

    [RPC]
    private void loadskinRPC(string n, string url, string url2, string[] skybox, PhotonMessageInfo info)
    {
        if ((int)Settings[2] == 1 && info.sender.isMasterClient)
        {
            StartCoroutine(CoLoadSkin(n, url, url2, skybox));
        }
    }

    private IEnumerator CoLoadSkin(string n, string url, string url2, string[] skybox)
    {
        bool mipmapping = true;
        bool unload = false;

        if ((int)Settings[63] == 1)
        {
            mipmapping = false;
        }

        // Load skybox skin
        if (skybox.Length > 5 && (skybox[0] != string.Empty || skybox[1] != string.Empty || skybox[2] != string.Empty || skybox[3] != string.Empty || skybox[4] != string.Empty || skybox[5] != string.Empty))
        {
            string key = string.Join(",", skybox);
            if (!LinkHash[1].ContainsKey(key))
            {
                unload = true;
                Material newSky = new Material(Camera.main.GetComponent<Skybox>().material);
                string skyFront = skybox[0];
                string skyBack = skybox[1];
                string skyLeft = skybox[2];
                string skyRight = skybox[3];
                string skyUp = skybox[4];
                string skyDown = skybox[5];

                // Old limit: 500KB
                if (skyFront.EndsWith(".jpg") || skyFront.EndsWith(".png") || skyFront.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyFront);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;
                            Texture2D skytex = RCextensions.LoadImage(www, mipmapping, 2000000);
                            skytex.wrapMode = TextureWrapMode.Clamp;
                            newSky.SetTexture("_FrontTex", skytex);
                        }
                    }
                }
                if (skyBack.EndsWith(".jpg") || skyBack.EndsWith(".png") || skyBack.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyBack);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;
                            Texture2D skytex2 = RCextensions.LoadImage(www, mipmapping, 2000000);
                            skytex2.wrapMode = TextureWrapMode.Clamp;
                            newSky.SetTexture("_BackTex", skytex2);
                        }
                    }
                }
                if (skyLeft.EndsWith(".jpg") || skyLeft.EndsWith(".png") || skyLeft.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyLeft);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;
                            Texture2D skytex3 = RCextensions.LoadImage(www, mipmapping, 2000000);
                            skytex3.wrapMode = TextureWrapMode.Clamp;
                            newSky.SetTexture("_LeftTex", skytex3);
                        }
                    }
                }
                if (skyRight.EndsWith(".jpg") || skyRight.EndsWith(".png") || skyRight.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyRight);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;
                            Texture2D skytex4 = RCextensions.LoadImage(www, mipmapping, 2000000);
                            skytex4.wrapMode = TextureWrapMode.Clamp;
                            newSky.SetTexture("_RightTex", skytex4);
                        }
                    }
                }
                if (skyUp.EndsWith(".jpg") || skyUp.EndsWith(".png") || skyUp.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyUp);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;
                            Texture2D skytex5 = RCextensions.LoadImage(www, mipmapping, 2000000);
                            skytex5.wrapMode = TextureWrapMode.Clamp;
                            newSky.SetTexture("_UpTex", skytex5);
                        }
                    }
                }
                if (skyDown.EndsWith(".jpg") || skyDown.EndsWith(".png") || skyDown.EndsWith(".jpeg"))
                {
                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(skyDown);
                    if (www != null)
                    {
                        using (www)
                        {
                            yield return www;
                            Texture2D skytex6 = RCextensions.LoadImage(www, mipmapping, 2000000);
                            skytex6.wrapMode = TextureWrapMode.Clamp;
                            newSky.SetTexture("_DownTex", skytex6);
                        }
                    }
                }
                Camera.main.GetComponent<Skybox>().material = newSky;
                SkyMaterial = newSky;
                LinkHash[1].Add(key, newSky);
            }
            else
            {
                Camera.main.GetComponent<Skybox>().material = (Material)LinkHash[1][key];
                SkyMaterial = (Material)LinkHash[1][key];
            }
        }

        if (Level.Map.Contains("Forest")) // Load Forest skin
        {
            string[] strArray = url.Split(',');
            string[] strArray2 = url2.Split(',');
            int startIndex = 0;
            try
            {
                foreach (GameObject obj4 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
                {
                    if (obj4 != null)
                    {
                        if (obj4.name.Contains("TREE") && n.Length > startIndex + 1)
                        {
                            string str12 = n.Substring(startIndex, 1);
                            string str11 = n.Substring(startIndex + 1, 1);
                            if (int.TryParse(str12, out int num8) && int.TryParse(str11, out int num7) && num8 >= 0 && num8 < 8 && num7 >= 0 && num7 < 8 && strArray.Length >= 8 && strArray2.Length >= 8 && strArray[num8] != null && strArray2[num7] != null)
                            {
                                string key2 = strArray[num8];
                                string str10 = strArray2[num7];
                                try
                                {
                                    foreach (Renderer renderer6 in obj4.GetComponentsInChildren<Renderer>())
                                    {
                                        if (renderer6.name.Contains(S[22]))
                                        {
                                            if (key2.EndsWith(".jpg") || key2.EndsWith(".png") || key2.EndsWith(".jpeg"))
                                            {
                                                if (!LinkHash[2].ContainsKey(key2))
                                                {
                                                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(key2);
                                                    if (www != null)
                                                    {
                                                        using (www)
                                                        {
                                                            yield return www;

                                                            // Old limit: 1MB
                                                            Texture2D tex7 = RCextensions.LoadImage(www, mipmapping, 2000000);
                                                            if (!LinkHash[2].ContainsKey(key2))
                                                            {
                                                                unload = true;
                                                                renderer6.material.mainTexture = tex7;
                                                                LinkHash[2].Add(key2, renderer6.material);
                                                            }
                                                        }
                                                        renderer6.material = (Material)LinkHash[2][key2];
                                                    }
                                                }
                                                else
                                                {
                                                    renderer6.material = (Material)LinkHash[2][key2];
                                                }
                                            }
                                        }
                                        else if (renderer6.name.Contains(S[23]))
                                        {
                                            if (str10.EndsWith(".jpg") || str10.EndsWith(".png") || str10.EndsWith(".jpeg"))
                                            {
                                                if (!LinkHash[0].ContainsKey(str10))
                                                {
                                                    WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(str10);
                                                    if (www != null)
                                                    {
                                                        using (www)
                                                        {
                                                            yield return www;

                                                            // Old limit: 200KB
                                                            Texture2D tex6 = RCextensions.LoadImage(www, mipmapping, 500000);
                                                            if (!LinkHash[0].ContainsKey(str10))
                                                            {
                                                                unload = true;
                                                                renderer6.material.mainTexture = tex6;
                                                                LinkHash[0].Add(str10, renderer6.material);
                                                            }
                                                        }
                                                        renderer6.material = (Material)LinkHash[0][str10];
                                                    }
                                                }
                                                else
                                                {
                                                    renderer6.material = (Material)LinkHash[0][str10];
                                                }
                                            }
                                            else if (str10.ToLower() == "transparent")
                                            {
                                                renderer6.enabled = false;
                                            }
                                        }
                                    }
                                }
                                finally
                                {
                                }
                            }
                            startIndex += 2;
                        }
                        else if (obj4.name.Contains("Cube_001") && obj4.transform.parent.gameObject.tag != "Player" && strArray2.Length > 8 && strArray2[8] != null)
                        {
                            string str9 = strArray2[8];
                            if (str9.EndsWith(".jpg") || str9.EndsWith(".png") || str9.EndsWith(".jpeg"))
                            {
                                try
                                {
                                    foreach (Renderer renderer5 in obj4.GetComponentsInChildren<Renderer>())
                                    {
                                        if (!LinkHash[0].ContainsKey(str9))
                                        {
                                            WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(str9);
                                            if (www != null)
                                            {
                                                using (www)
                                                {
                                                    yield return www;

                                                    // Old limit: 200KB
                                                    Texture2D tex5 = RCextensions.LoadImage(www, mipmapping, 500000);
                                                    if (!LinkHash[0].ContainsKey(str9))
                                                    {
                                                        unload = true;
                                                        renderer5.material.mainTexture = tex5;
                                                        LinkHash[0].Add(str9, renderer5.material);
                                                    }
                                                }
                                                renderer5.material = (Material)LinkHash[0][str9];
                                            }
                                        }
                                        else
                                        {
                                            renderer5.material = (Material)LinkHash[0][str9];
                                        }
                                    }
                                }
                                finally
                                {
                                }
                            }
                            else if (str9.ToLower() == "transparent")
                            {
                                foreach (Renderer renderer7 in obj4.GetComponentsInChildren<Renderer>())
                                {
                                    renderer7.enabled = false;
                                }
                            }
                        }
                    }
                }
            }
            finally { }
        }
        else if (Level.Map.Contains("City")) // Load City skin
        {
            string[] strArray4 = url.Split(',');
            string[] strArray3 = url2.Split(',');
            int num6 = 0;
            foreach (GameObject obj3 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (obj3 != null && obj3.name.Contains("Cube_") && obj3.transform.parent.gameObject.tag != "Player")
                {
                    if (obj3.name.EndsWith("001"))
                    {
                        if (strArray3.Length > 0 && strArray3[0] != null)
                        {
                            string str8 = strArray3[0];
                            if (str8.EndsWith(".jpg") || str8.EndsWith(".png") || str8.EndsWith(".jpeg"))
                            {
                                try
                                {
                                    foreach (Renderer renderer4 in obj3.GetComponentsInChildren<Renderer>())
                                    {
                                        if (!LinkHash[0].ContainsKey(str8))
                                        {
                                            WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(str8);
                                            if (www != null)
                                            {
                                                using (www)
                                                {
                                                    yield return www;

                                                    // Old limit: 200KB
                                                    Texture2D tex4 = RCextensions.LoadImage(www, mipmapping, 500000);
                                                    if (!LinkHash[0].ContainsKey(str8))
                                                    {
                                                        unload = true;
                                                        renderer4.material.mainTexture = tex4;
                                                        LinkHash[0].Add(str8, renderer4.material);
                                                    }
                                                }
                                                renderer4.material = (Material)LinkHash[0][str8];
                                            }
                                        }
                                        else
                                        {
                                            renderer4.material = (Material)LinkHash[0][str8];
                                        }
                                    }
                                }
                                finally { }
                            }
                            else if (str8.ToLower() == "transparent")
                            {
                                foreach (Renderer renderer7 in obj3.GetComponentsInChildren<Renderer>())
                                {
                                    renderer7.enabled = false;
                                }
                            }
                        }
                    }
                    else if (obj3.name.EndsWith("006") || obj3.name.EndsWith("007") || obj3.name.EndsWith("015") || obj3.name.EndsWith("000") || (obj3.name.EndsWith("002") && obj3.transform.position.x == 0f && obj3.transform.position.y == 0f && obj3.transform.position.z == 0f))
                    {
                        if (strArray3.Length > 0 && strArray3[1] != null)
                        {
                            string str7 = strArray3[1];
                            if (str7.EndsWith(".jpg") || str7.EndsWith(".png") || str7.EndsWith(".jpeg"))
                            {
                                try
                                {
                                    foreach (Renderer renderer3 in obj3.GetComponentsInChildren<Renderer>())
                                    {
                                        if (!LinkHash[0].ContainsKey(str7))
                                        {
                                            WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(str7);
                                            if (www != null)
                                            {
                                                using (www)
                                                {
                                                    yield return www;

                                                    // Old limit: 200KB
                                                    Texture2D tex3 = RCextensions.LoadImage(www, mipmapping, 500000);
                                                    if (!LinkHash[0].ContainsKey(str7))
                                                    {
                                                        unload = true;
                                                        renderer3.material.mainTexture = tex3;
                                                        LinkHash[0].Add(str7, renderer3.material);
                                                    }
                                                }
                                                renderer3.material = (Material)LinkHash[0][str7];
                                            }
                                        }
                                        else
                                        {
                                            renderer3.material = (Material)LinkHash[0][str7];
                                        }
                                    }
                                }
                                finally
                                {
                                }
                            }
                        }
                    }
                    else if (obj3.name.EndsWith("005") || obj3.name.EndsWith("003") || (obj3.name.EndsWith("002") && (obj3.transform.position.x != 0f || obj3.transform.position.y != 0f || obj3.transform.position.z != 0f) && n.Length > num6))
                    {
                        string str6 = n.Substring(num6, 1);
                        if (int.TryParse(str6, out int num5) && num5 >= 0 && num5 < 8 && strArray4.Length >= 8 && strArray4[num5] != null)
                        {
                            string str5 = strArray4[num5];
                            if (str5.EndsWith(".jpg") || str5.EndsWith(".png") || str5.EndsWith(".jpeg"))
                            {
                                try
                                {
                                    foreach (Renderer renderer2 in obj3.GetComponentsInChildren<Renderer>())
                                    {
                                        if (!LinkHash[2].ContainsKey(str5))
                                        {
                                            WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(str5);
                                            if (www != null)
                                            {
                                                using (www)
                                                {
                                                    yield return www;

                                                    // Old limit: 1MB
                                                    Texture2D tex2 = RCextensions.LoadImage(www, mipmapping, 2000000);
                                                    if (!LinkHash[2].ContainsKey(str5))
                                                    {
                                                        unload = true;
                                                        renderer2.material.mainTexture = tex2;
                                                        LinkHash[2].Add(str5, renderer2.material);
                                                    }
                                                }
                                                renderer2.material = (Material)LinkHash[2][str5];
                                            }
                                        }
                                        else
                                        {
                                            renderer2.material = (Material)LinkHash[2][str5];
                                        }
                                    }
                                }
                                finally { }
                            }
                        }
                        num6++;
                    }
                    else if ((obj3.name.EndsWith("019") || obj3.name.EndsWith("020")) && strArray3.Length > 2 && strArray3[2] != null)
                    {
                        string str4 = strArray3[2];
                        if (str4.EndsWith(".jpg") || str4.EndsWith(".png") || str4.EndsWith(".jpeg"))
                        {
                            try
                            {
                                foreach (Renderer renderer in obj3.GetComponentsInChildren<Renderer>())
                                {
                                    if (!LinkHash[2].ContainsKey(str4))
                                    {
                                        WWW www = Guardian.AntiAbuse.Validators.SkinChecker.CreateWWW(str4);
                                        if (www != null)
                                        {
                                            using (www)
                                            {
                                                yield return www;

                                                // Old limit: 1MB
                                                Texture2D tex = RCextensions.LoadImage(www, mipmapping, 2000000);
                                                if (!LinkHash[2].ContainsKey(str4))
                                                {
                                                    unload = true;
                                                    renderer.material.mainTexture = tex;
                                                    LinkHash[2].Add(str4, renderer.material);
                                                }
                                            }
                                            renderer.material = (Material)LinkHash[2][str4];
                                        }
                                    }
                                    else
                                    {
                                        renderer.material = (Material)LinkHash[2][str4];
                                    }
                                }
                            }
                            finally { }
                        }
                    }
                }
            }
        }

        Minimap.TryRecaptureInstance();
        if (unload)
        {
            UnloadAssets();
        }
    }

    public void OnGUI()
    {
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Stop && Application.loadedLevelName != "characterCreation" && Application.loadedLevelName != "SnapShot")
        {
            if (IsAssetLoaded)
            {
                string text = GameObject.Find("VERSION").GetComponent<UILabel>().text;
                if (text == null)
                {
                    return;
                }
                if (!(GameObject.Find("ButtonCREDITS") != null) || !(GameObject.Find("ButtonCREDITS").transform.parent.gameObject != null) || !NGUITools.GetActive(GameObject.Find("ButtonCREDITS").transform.parent.gameObject))
                {
                    return;
                }

                // Ko-fi link
                if (Guardian.Utilities.ResourceLoader.TryGetAsset("Textures/ko-fi.png", out Texture2D kofi))
                {
                    if (GUI.Button(new Rect(10, 185, 220, 75), kofi))
                    {
                        Application.OpenURL("https://www.ko-fi.com/winnpixie");
                    }
                }

                // AoTTG-2 patreon link
                if (Guardian.Utilities.ResourceLoader.TryGetAsset("Textures/patreon.png", out Texture2D aot2Patreon))
                {
                    if (GUI.Button(new Rect(10, 265, 220, 150), aot2Patreon))
                    {
                        Application.OpenURL("https://www.patreon.com/aottg2");
                    }
                }

                float num7 = (float)Screen.width / 2f - 85f;
                float num8 = (float)Screen.height / 2f;
                GUI.Box(new Rect(num7, 5f, 150f, 105f), string.Empty);
                if (GUI.Button(new Rect(num7 + 11f, 15f, 128f, 25f), "Level Editor"))
                {
                    Settings[64] = 101;
                    Application.LoadLevel(2);
                }
                else if (GUI.Button(new Rect(num7 + 11f, 45f, 128f, 25f), $"Server: {Guardian.Networking.NetworkHelper.App.Name}"))
                {
                    // App-Id switcher
                    if (Guardian.Networking.NetworkHelper.App == Guardian.Networking.PhotonApplication.AoTTG2)
                    {
                        Guardian.Networking.NetworkHelper.App = Guardian.Networking.PhotonApplication.Custom;
                    }
                    else
                    {
                        Guardian.Networking.NetworkHelper.App = Guardian.Networking.PhotonApplication.AoTTG2;
                    }
                }
                else if (GUI.Button(new Rect(num7 + 11f, 75f, 128f, 25f), $"Protocol: {Guardian.Networking.NetworkHelper.Connection.Name}"))
                {
                    // Protocol switcher
                    switch (Guardian.Networking.NetworkHelper.Connection.Protocol)
                    {
                        case ExitGames.Client.Photon.ConnectionProtocol.Tcp:
                            Guardian.Networking.NetworkHelper.Connection = Guardian.Networking.PhotonConnection.UDP;
                            break;
                        case ExitGames.Client.Photon.ConnectionProtocol.Udp:
                            Guardian.Networking.NetworkHelper.Connection = Guardian.Networking.PhotonConnection.TCP;
                            break;
                    }
                    PhotonNetwork.SwitchToProtocol(Guardian.Networking.NetworkHelper.Connection.Protocol);
                }
                GUI.Box(new Rect(10f, 30f, 220f, 150f), string.Empty);
                if (GUI.Button(new Rect(17.5f, 40f, 40f, 25f), "Login"))
                {
                    Settings[187] = 0;
                }
                else if (GUI.Button(new Rect(65f, 40f, 95f, 25f), "Custom Name"))
                {
                    Settings[187] = 1;
                }
                else if (GUI.Button(new Rect(167.5f, 40f, 55f, 25f), "Servers"))
                {
                    Settings[187] = 2;
                }

                switch ((int)Settings[187])
                {
                    case 0:
                        if (LoginFengKAI.LoginState == LoginState.LoggedIn)
                        {
                            GUI.Label(new Rect(20f, 80f, 70f, 20f), "Username:", "Label");
                            GUI.Label(new Rect(90f, 80f, 90f, 20f), LoginFengKAI.Player.Name, "Label");
                            GUI.Label(new Rect(20f, 105f, 45f, 20f), "Guild:", "Label");
                            LoginFengKAI.Player.Guild = GUI.TextField(new Rect(65f, 105f, 145f, 20f), LoginFengKAI.Player.Guild, 40);
                            if (GUI.Button(new Rect(35f, 140f, 70f, 25f), "Set Guild"))
                            {
                                StartCoroutine(CoSetGuild());
                            }
                            else if (GUI.Button(new Rect(130f, 140f, 65f, 25f), "Logout"))
                            {
                                LoginFengKAI.LoginState = LoginState.LoggedOut;
                            }
                            return;
                        }
                        GUI.Label(new Rect(20f, 80f, 70f, 20f), "Username:", "Label");
                        UsernameField = GUI.TextField(new Rect(90f, 80f, 130f, 20f), UsernameField, 40);
                        GUI.Label(new Rect(20f, 105f, 70f, 20f), "Password:", "Label");
                        PasswordField = GUI.PasswordField(new Rect(90f, 105f, 130f, 20f), PasswordField, '*', 40);
                        if (GUI.Button(new Rect(30f, 140f, 50f, 25f), "Login") && LoginFengKAI.LoginState != LoginState.LoggingIn)
                        {
                            StartCoroutine(CoLogin());
                            LoginFengKAI.LoginState = LoginState.LoggingIn;
                        }
                        if (LoginFengKAI.LoginState == LoginState.LoggingIn)
                        {
                            GUI.Label(new Rect(100f, 140f, 120f, 25f), "Logging in...", "Label");
                        }
                        else if (LoginFengKAI.LoginState == LoginState.Failed)
                        {
                            GUI.Label(new Rect(100f, 140f, 120f, 25f), "Login Failed.", "Label");
                        }
                        break;
                    case 1:
                        if (LoginFengKAI.LoginState == LoginState.LoggedIn)
                        {
                            GUI.Label(new Rect(30f, 80f, 180f, 60f), "You're already logged in!", "Label");
                            return;
                        }
                        // Change max from 40 to 255 because why not
                        GUI.Label(new Rect(20f, 80f, 45f, 20f), "Name:", "Label");
                        NameField = GUI.TextField(new Rect(65f, 80f, 145f, 20f), NameField, 255);
                        GUI.Label(new Rect(20f, 105f, 45f, 20f), "Guild:", "Label");
                        LoginFengKAI.Player.Guild = GUI.TextField(new Rect(65f, 105f, 145f, 20f), LoginFengKAI.Player.Guild, 255);
                        if (GUI.Button(new Rect(42f, 140f, 50f, 25f), "Save"))
                        {
                            PlayerPrefs.SetString("name", NameField);
                            PlayerPrefs.SetString("guildname", LoginFengKAI.Player.Guild);
                        }
                        else if (GUI.Button(new Rect(128f, 140f, 50f, 25f), "Load"))
                        {
                            NameField = PlayerPrefs.GetString("name", string.Empty);
                            LoginFengKAI.Player.Guild = PlayerPrefs.GetString("guildname", string.Empty);
                        }
                        break;
                    case 2:
                        if (UIMainReferences.Version == UIMainReferences.FengVersion)
                        {
                            GUI.Label(new Rect(37f, 75f, 190f, 25f), "Connected to public server.", "Label");
                        }
                        else if (UIMainReferences.Version == S[0])
                        {
                            GUI.Label(new Rect(28f, 75f, 190f, 25f), "Connected to RC private server.", "Label");
                        }
                        else
                        {
                            GUI.Label(new Rect(37f, 75f, 190f, 25f), "Connected to custom server.", "Label");
                        }
                        GUI.Label(new Rect(20f, 100f, 90f, 25f), "Public Server:", "Label");
                        GUI.Label(new Rect(20f, 125f, 80f, 25f), "RC Private:", "Label");
                        GUI.Label(new Rect(20f, 150f, 60f, 25f), "Custom:", "Label");
                        if (GUI.Button(new Rect(160f, 100f, 60f, 20f), "Connect"))
                        {
                            UIMainReferences.Version = UIMainReferences.FengVersion;
                        }
                        else if (GUI.Button(new Rect(160f, 125f, 60f, 20f), "Connect"))
                        {
                            UIMainReferences.Version = S[0];
                        }
                        else if (GUI.Button(new Rect(160f, 150f, 60f, 20f), "Connect"))
                        {
                            UIMainReferences.Version = PrivateServerField;
                        }
                        PrivateServerField = GUI.TextField(new Rect(78f, 153f, 70f, 18f), PrivateServerField, 50);
                        break;
                }
            }
            else
            {
                float num9 = (float)(Screen.width / 2) - 115f;
                float num10 = (float)(Screen.height / 2) - 45f;
                GUI.Box(new Rect(num9, num10, 230f, 90f), string.Empty);
                GUI.Label(new Rect(num9 + 13f, num10 + 20f, 172f, 70f), "Downloading assets. Clear your cache or try another RC-based mod if this takes longer than 10 seconds.");
            }
        }
        else
        {
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Stop)
            {
                return;
            }
            if ((int)Settings[64] >= 100)
            {
                float num11 = (float)Screen.width - 300f;
                bool flag = false;
                bool flag2 = false;
                GUI.Box(new Rect(5f, 5f, 295f, 590f), string.Empty);
                GUI.Box(new Rect(num11, 5f, 295f, 590f), string.Empty);
                if (GUI.Button(new Rect(10f, 10f, 60f, 25f), "Script"))
                {
                    Settings[68] = 100;
                }
                if (GUI.Button(new Rect(75f, 10f, 65f, 25f), "Controls"))
                {
                    Settings[68] = 101;
                }
                if (GUI.Button(new Rect(210f, 10f, 80f, 25f), "Full Screen"))
                {
                    Screen.fullScreen = !Screen.fullScreen;
                    if (Screen.fullScreen)
                    {
                        Screen.SetResolution(960, 600, fullscreen: false);
                    }
                    else
                    {
                        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullscreen: true);
                    }
                }
                if ((int)Settings[68] == 100 || (int)Settings[68] == 102)
                {
                    GUI.Label(new Rect(115f, 40f, 100f, 20f), "Level Script:", "Label");
                    GUI.Label(new Rect(115f, 115f, 100f, 20f), "Import Data", "Label");
                    GUI.Label(new Rect(12f, 535f, 280f, 60f), "Warning: your current level will be lost if you quit or import data. Make sure to save the level to a text document.", "Label");
                    Settings[77] = GUI.TextField(new Rect(10f, 140f, 285f, 350f), (string)Settings[77]);
                    if (GUI.Button(new Rect(35f, 500f, 60f, 30f), "Apply"))
                    {
                        foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
                        {
                            if (gameObject.name.StartsWith("custom") || gameObject.name.StartsWith("base") || gameObject.name.StartsWith("photon") || gameObject.name.StartsWith("spawnpoint") || gameObject.name.StartsWith("misc") || gameObject.name.StartsWith("racing"))
                            {
                                UnityEngine.Object.Destroy(gameObject);
                            }
                        }
                        LinkHash[3].Clear();
                        Settings[186] = 0;
                        string[] array2 = Regex.Replace((string)Settings[77], "\\s+", string.Empty).Replace("\r\n", string.Empty).Replace("\n", string.Empty)
                            .Replace("\r", string.Empty)
                            .Split(';');
                        for (int j = 0; j < array2.Length; j++)
                        {
                            string[] array3 = array2[j].Split(',');
                            if (array3[0].StartsWith("custom") || array3[0].StartsWith("base") || array3[0].StartsWith("photon") || array3[0].StartsWith("spawnpoint") || array3[0].StartsWith("misc") || array3[0].StartsWith("racing"))
                            {
                                GameObject gameObject2 = null;
                                if (array3[0].StartsWith("custom"))
                                {
                                    gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(array3[1]), new Vector3(Convert.ToSingle(array3[12]), Convert.ToSingle(array3[13]), Convert.ToSingle(array3[14])), new Quaternion(Convert.ToSingle(array3[15]), Convert.ToSingle(array3[16]), Convert.ToSingle(array3[17]), Convert.ToSingle(array3[18])));
                                }
                                else if (array3[0].StartsWith("photon"))
                                {
                                    gameObject2 = ((!array3[1].StartsWith("Cannon")) ? ((GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(array3[1]), new Vector3(Convert.ToSingle(array3[4]), Convert.ToSingle(array3[5]), Convert.ToSingle(array3[6])), new Quaternion(Convert.ToSingle(array3[7]), Convert.ToSingle(array3[8]), Convert.ToSingle(array3[9]), Convert.ToSingle(array3[10])))) : ((array3.Length >= 15) ? ((GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(array3[1] + "Prop"), new Vector3(Convert.ToSingle(array3[12]), Convert.ToSingle(array3[13]), Convert.ToSingle(array3[14])), new Quaternion(Convert.ToSingle(array3[15]), Convert.ToSingle(array3[16]), Convert.ToSingle(array3[17]), Convert.ToSingle(array3[18])))) : ((GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(array3[1] + "Prop"), new Vector3(Convert.ToSingle(array3[2]), Convert.ToSingle(array3[3]), Convert.ToSingle(array3[4])), new Quaternion(Convert.ToSingle(array3[5]), Convert.ToSingle(array3[6]), Convert.ToSingle(array3[7]), Convert.ToSingle(array3[8]))))));
                                }
                                else if (array3[0].StartsWith("spawnpoint"))
                                {
                                    gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(array3[1]), new Vector3(Convert.ToSingle(array3[2]), Convert.ToSingle(array3[3]), Convert.ToSingle(array3[4])), new Quaternion(Convert.ToSingle(array3[5]), Convert.ToSingle(array3[6]), Convert.ToSingle(array3[7]), Convert.ToSingle(array3[8])));
                                }
                                else if (array3[0].StartsWith("base"))
                                {
                                    gameObject2 = ((array3.Length >= 15) ? ((GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load(array3[1]), new Vector3(Convert.ToSingle(array3[12]), Convert.ToSingle(array3[13]), Convert.ToSingle(array3[14])), new Quaternion(Convert.ToSingle(array3[15]), Convert.ToSingle(array3[16]), Convert.ToSingle(array3[17]), Convert.ToSingle(array3[18])))) : ((GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load(array3[1]), new Vector3(Convert.ToSingle(array3[2]), Convert.ToSingle(array3[3]), Convert.ToSingle(array3[4])), new Quaternion(Convert.ToSingle(array3[5]), Convert.ToSingle(array3[6]), Convert.ToSingle(array3[7]), Convert.ToSingle(array3[8])))));
                                }
                                else if (array3[0].StartsWith("misc"))
                                {
                                    if (array3[1].StartsWith("barrier"))
                                    {
                                        gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load("barrierEditor"), new Vector3(Convert.ToSingle(array3[5]), Convert.ToSingle(array3[6]), Convert.ToSingle(array3[7])), new Quaternion(Convert.ToSingle(array3[8]), Convert.ToSingle(array3[9]), Convert.ToSingle(array3[10]), Convert.ToSingle(array3[11])));
                                    }
                                    else if (array3[1].StartsWith("region"))
                                    {
                                        gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load("regionEditor"));
                                        gameObject2.transform.position = new Vector3(Convert.ToSingle(array3[6]), Convert.ToSingle(array3[7]), Convert.ToSingle(array3[8]));
                                        GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
                                        gameObject3.name = "RegionLabel";
                                        gameObject3.transform.parent = gameObject2.transform;
                                        float y = 1f;
                                        if (Convert.ToSingle(array3[4]) > 100f)
                                        {
                                            y = 0.8f;
                                        }
                                        else if (Convert.ToSingle(array3[4]) > 1000f)
                                        {
                                            y = 0.5f;
                                        }
                                        gameObject3.transform.localPosition = new Vector3(0f, y, 0f);
                                        gameObject3.transform.localScale = new Vector3(5f / Convert.ToSingle(array3[3]), 5f / Convert.ToSingle(array3[4]), 5f / Convert.ToSingle(array3[5]));
                                        gameObject3.GetComponent<UILabel>().text = array3[2];
                                        gameObject2.AddComponent<RCRegionLabel>();
                                        gameObject2.GetComponent<RCRegionLabel>().myLabel = gameObject3;
                                    }
                                    else if (array3[1].StartsWith("racingStart"))
                                    {
                                        gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load("racingStart"), new Vector3(Convert.ToSingle(array3[5]), Convert.ToSingle(array3[6]), Convert.ToSingle(array3[7])), new Quaternion(Convert.ToSingle(array3[8]), Convert.ToSingle(array3[9]), Convert.ToSingle(array3[10]), Convert.ToSingle(array3[11])));
                                    }
                                    else if (array3[1].StartsWith("racingEnd"))
                                    {
                                        gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load("racingEnd"), new Vector3(Convert.ToSingle(array3[5]), Convert.ToSingle(array3[6]), Convert.ToSingle(array3[7])), new Quaternion(Convert.ToSingle(array3[8]), Convert.ToSingle(array3[9]), Convert.ToSingle(array3[10]), Convert.ToSingle(array3[11])));
                                    }
                                }
                                else if (array3[0].StartsWith("racing"))
                                {
                                    gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCAssets.Load(array3[1]), new Vector3(Convert.ToSingle(array3[5]), Convert.ToSingle(array3[6]), Convert.ToSingle(array3[7])), new Quaternion(Convert.ToSingle(array3[8]), Convert.ToSingle(array3[9]), Convert.ToSingle(array3[10]), Convert.ToSingle(array3[11])));
                                }
                                if (array3[2] != "default" && (array3[0].StartsWith("custom") || (array3[0].StartsWith("base") && array3.Length > 15) || (array3[0].StartsWith("photon") && array3.Length > 15)))
                                {
                                    Renderer[] componentsInChildren = gameObject2.GetComponentsInChildren<Renderer>();
                                    foreach (Renderer renderer in componentsInChildren)
                                    {
                                        if (!renderer.name.Contains("Particle System") || !gameObject2.name.Contains("aot_supply"))
                                        {
                                            renderer.material = (Material)RCAssets.Load(array3[2]);
                                            renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(array3[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(array3[11]));
                                        }
                                    }
                                }
                                if (array3[0].StartsWith("custom") || (array3[0].StartsWith("base") && array3.Length > 15) || (array3[0].StartsWith("photon") && array3.Length > 15))
                                {
                                    float num12 = gameObject2.transform.localScale.x * Convert.ToSingle(array3[3]);
                                    num12 -= 0.001f;
                                    float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array3[4]);
                                    float z = gameObject2.transform.localScale.z * Convert.ToSingle(array3[5]);
                                    gameObject2.transform.localScale = new Vector3(num12, y2, z);
                                    if (array3[6] != "0")
                                    {
                                        Color color = new Color(Convert.ToSingle(array3[7]), Convert.ToSingle(array3[8]), Convert.ToSingle(array3[9]), 1f);
                                        MeshFilter[] componentsInChildren2 = gameObject2.GetComponentsInChildren<MeshFilter>();
                                        foreach (MeshFilter meshFilter in componentsInChildren2)
                                        {
                                            Mesh mesh = meshFilter.mesh;
                                            Color[] array4 = new Color[mesh.vertexCount];
                                            for (int k = 0; k < mesh.vertexCount; k++)
                                            {
                                                array4[k] = color;
                                            }
                                            mesh.colors = array4;
                                        }
                                    }
                                    gameObject2.name = array3[0] + "," + array3[1] + "," + array3[2] + "," + array3[3] + "," + array3[4] + "," + array3[5] + "," + array3[6] + "," + array3[7] + "," + array3[8] + "," + array3[9] + "," + array3[10] + "," + array3[11];
                                }
                                else if (array3[0].StartsWith("misc"))
                                {
                                    if (array3[1].StartsWith("barrier") || array3[1].StartsWith("racing"))
                                    {
                                        float num12 = gameObject2.transform.localScale.x * Convert.ToSingle(array3[2]);
                                        num12 -= 0.001f;
                                        float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array3[3]);
                                        float z = gameObject2.transform.localScale.z * Convert.ToSingle(array3[4]);
                                        gameObject2.transform.localScale = new Vector3(num12, y2, z);
                                        gameObject2.name = array3[0] + "," + array3[1] + "," + array3[2] + "," + array3[3] + "," + array3[4];
                                    }
                                    else if (array3[1].StartsWith("region"))
                                    {
                                        float num12 = gameObject2.transform.localScale.x * Convert.ToSingle(array3[3]);
                                        num12 -= 0.001f;
                                        float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array3[4]);
                                        float z = gameObject2.transform.localScale.z * Convert.ToSingle(array3[5]);
                                        gameObject2.transform.localScale = new Vector3(num12, y2, z);
                                        gameObject2.name = array3[0] + "," + array3[1] + "," + array3[2] + "," + array3[3] + "," + array3[4] + "," + array3[5];
                                    }
                                }
                                else if (array3[0].StartsWith("racing"))
                                {
                                    float num12 = gameObject2.transform.localScale.x * Convert.ToSingle(array3[2]);
                                    num12 -= 0.001f;
                                    float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array3[3]);
                                    float z = gameObject2.transform.localScale.z * Convert.ToSingle(array3[4]);
                                    gameObject2.transform.localScale = new Vector3(num12, y2, z);
                                    gameObject2.name = array3[0] + "," + array3[1] + "," + array3[2] + "," + array3[3] + "," + array3[4];
                                }
                                else if (array3[0].StartsWith("photon") && !array3[1].StartsWith("Cannon"))
                                {
                                    gameObject2.name = array3[0] + "," + array3[1] + "," + array3[2] + "," + array3[3];
                                }
                                else
                                {
                                    gameObject2.name = array3[0] + "," + array3[1];
                                }
                                LinkHash[3].Add(gameObject2.GetInstanceID(), array2[j]);
                            }
                            else if (array3[0].StartsWith("map") && array3[1].StartsWith("disablebounds"))
                            {
                                Settings[186] = 1;
                                if (!LinkHash[3].ContainsKey("mapbounds"))
                                {
                                    LinkHash[3].Add("mapbounds", "map,disablebounds");
                                }
                            }
                        }
                        UnloadAssets();
                        Settings[77] = string.Empty;
                    }
                    else if (GUI.Button(new Rect(205f, 500f, 60f, 30f), "Exit"))
                    {
                        Screen.lockCursor = false;
                        Screen.showCursor = true;
                        IN_GAME_MAIN_CAMERA.Gametype = GameType.Stop;
                        inputManager.menuOn = false;
                        UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
                        Application.LoadLevel("menu");
                    }
                    else if (GUI.Button(new Rect(15f, 70f, 115f, 30f), "Copy to Clipboard"))
                    {
                        string text2 = string.Empty;
                        int num13 = 0;
                        foreach (string value in LinkHash[3].Values)
                        {
                            num13++;
                            text2 = text2 + value + ";\n";
                        }
                        TextEditor textEditor = new TextEditor();
                        textEditor.content = new GUIContent(text2);
                        textEditor.SelectAll();
                        textEditor.Copy();
                    }
                    else if (GUI.Button(new Rect(175f, 70f, 115f, 30f), "View Script"))
                    {
                        Settings[68] = 102;
                    }
                    if ((int)Settings[68] == 102)
                    {
                        string text2 = string.Empty;
                        int num13 = 0;
                        foreach (string value2 in LinkHash[3].Values)
                        {
                            num13++;
                            text2 = text2 + value2 + ";\n";
                        }
                        float num14 = (float)(Screen.width / 2) - 110.5f;
                        float num15 = (float)(Screen.height / 2) - 250f;
                        GUI.Box(new Rect(num14, num15, 221f, 500f), string.Empty);
                        if (GUI.Button(new Rect(num14 + 10f, num15 + 460f, 60f, 30f), "Copy"))
                        {
                            TextEditor textEditor = new TextEditor();
                            textEditor.content = new GUIContent(text2);
                            textEditor.SelectAll();
                            textEditor.Copy();
                        }
                        else if (GUI.Button(new Rect(num14 + 151f, num15 + 460f, 60f, 30f), "Done"))
                        {
                            Settings[68] = 100;
                        }
                        GUI.TextArea(new Rect(num14 + 5f, num15 + 5f, 211f, 415f), text2);
                        GUI.Label(new Rect(num14 + 10f, num15 + 430f, 150f, 20f), "Object Count: " + Convert.ToString(num13), "Label");
                    }
                }
                else if ((int)Settings[68] == 101)
                {
                    GUI.Label(new Rect(92f, 50f, 180f, 20f), "Level Editor Rebinds:", "Label");
                    GUI.Label(new Rect(12f, 80f, 145f, 20f), "Forward:", "Label");
                    GUI.Label(new Rect(12f, 105f, 145f, 20f), "Back:", "Label");
                    GUI.Label(new Rect(12f, 130f, 145f, 20f), "Left:", "Label");
                    GUI.Label(new Rect(12f, 155f, 145f, 20f), "Right:", "Label");
                    GUI.Label(new Rect(12f, 180f, 145f, 20f), "Up:", "Label");
                    GUI.Label(new Rect(12f, 205f, 145f, 20f), "Down:", "Label");
                    GUI.Label(new Rect(12f, 230f, 145f, 20f), "Toggle Cursor:", "Label");
                    GUI.Label(new Rect(12f, 255f, 145f, 20f), "Place Object:", "Label");
                    GUI.Label(new Rect(12f, 280f, 145f, 20f), "Delete Object:", "Label");
                    GUI.Label(new Rect(12f, 305f, 145f, 20f), "Movement-Slow:", "Label");
                    GUI.Label(new Rect(12f, 330f, 145f, 20f), "Rotate Forward:", "Label");
                    GUI.Label(new Rect(12f, 355f, 145f, 20f), "Rotate Backward:", "Label");
                    GUI.Label(new Rect(12f, 380f, 145f, 20f), "Rotate Left:", "Label");
                    GUI.Label(new Rect(12f, 405f, 145f, 20f), "Rotate Right:", "Label");
                    GUI.Label(new Rect(12f, 430f, 145f, 20f), "Rotate CCW:", "Label");
                    GUI.Label(new Rect(12f, 455f, 145f, 20f), "Rotate CW:", "Label");
                    GUI.Label(new Rect(12f, 480f, 145f, 20f), "Movement-Speedup:", "Label");
                    for (int j = 0; j < 17; j++)
                    {
                        float top = 80f + 25f * (float)j;
                        int num16 = 117 + j;
                        if (j == 16)
                        {
                            num16 = 161;
                        }
                        if (GUI.Button(new Rect(135f, top, 60f, 20f), (string)Settings[num16]))
                        {
                            Settings[num16] = "waiting...";
                            Settings[100] = num16;
                        }
                    }
                    if ((int)Settings[100] != 0)
                    {
                        Event current = Event.current;
                        bool flag3 = false;
                        string text3 = "waiting...";
                        if (current.type == EventType.KeyDown && current.keyCode != KeyCode.None)
                        {
                            flag3 = true;
                            text3 = current.keyCode.ToString();
                        }
                        else if (Input.GetKey(KeyCode.LeftShift))
                        {
                            flag3 = true;
                            text3 = KeyCode.LeftShift.ToString();
                        }
                        else if (Input.GetKey(KeyCode.RightShift))
                        {
                            flag3 = true;
                            text3 = KeyCode.RightShift.ToString();
                        }
                        else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                        {
                            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                            {
                                flag3 = true;
                                text3 = "Scroll Up";
                            }
                            else
                            {
                                flag3 = true;
                                text3 = "Scroll Down";
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 7; j++)
                            {
                                if (Input.GetKeyDown((KeyCode)(323 + j)))
                                {
                                    flag3 = true;
                                    text3 = "Mouse" + Convert.ToString(j);
                                }
                            }
                        }
                        if (flag3)
                        {
                            for (int j = 0; j < 17; j++)
                            {
                                int num16 = 117 + j;
                                if (j == 16)
                                {
                                    num16 = 161;
                                }
                                if ((int)Settings[100] == num16)
                                {
                                    Settings[num16] = text3;
                                    Settings[100] = 0;
                                    InputRC.setInputLevel(j, text3);
                                }
                            }
                        }
                    }
                    if (GUI.Button(new Rect(100f, 515f, 110f, 30f), "Save Controls"))
                    {
                        PlayerPrefs.SetString("lforward", (string)Settings[117]);
                        PlayerPrefs.SetString("lback", (string)Settings[118]);
                        PlayerPrefs.SetString("lleft", (string)Settings[119]);
                        PlayerPrefs.SetString("lright", (string)Settings[120]);
                        PlayerPrefs.SetString("lup", (string)Settings[121]);
                        PlayerPrefs.SetString("ldown", (string)Settings[122]);
                        PlayerPrefs.SetString("lcursor", (string)Settings[123]);
                        PlayerPrefs.SetString("lplace", (string)Settings[124]);
                        PlayerPrefs.SetString("ldel", (string)Settings[125]);
                        PlayerPrefs.SetString("lslow", (string)Settings[126]);
                        PlayerPrefs.SetString("lrforward", (string)Settings[127]);
                        PlayerPrefs.SetString("lrback", (string)Settings[128]);
                        PlayerPrefs.SetString("lrleft", (string)Settings[129]);
                        PlayerPrefs.SetString("lrright", (string)Settings[130]);
                        PlayerPrefs.SetString("lrccw", (string)Settings[131]);
                        PlayerPrefs.SetString("lrcw", (string)Settings[132]);
                        PlayerPrefs.SetString("lfast", (string)Settings[161]);
                    }
                }
                if ((int)Settings[64] != 105 && (int)Settings[64] != 106)
                {
                    GUI.Label(new Rect(num11 + 13f, 445f, 125f, 20f), "Scale Multipliers:", "Label");
                    GUI.Label(new Rect(num11 + 13f, 470f, 50f, 22f), "Length:", "Label");
                    Settings[72] = GUI.TextField(new Rect(num11 + 58f, 470f, 40f, 20f), (string)Settings[72]);
                    GUI.Label(new Rect(num11 + 13f, 495f, 50f, 20f), "Width:", "Label");
                    Settings[70] = GUI.TextField(new Rect(num11 + 58f, 495f, 40f, 20f), (string)Settings[70]);
                    GUI.Label(new Rect(num11 + 13f, 520f, 50f, 22f), "Height:", "Label");
                    Settings[71] = GUI.TextField(new Rect(num11 + 58f, 520f, 40f, 20f), (string)Settings[71]);
                    if ((int)Settings[64] <= 106)
                    {
                        GUI.Label(new Rect(num11 + 155f, 554f, 50f, 22f), "Tiling:", "Label");
                        Settings[79] = GUI.TextField(new Rect(num11 + 200f, 554f, 40f, 20f), (string)Settings[79]);
                        Settings[80] = GUI.TextField(new Rect(num11 + 245f, 554f, 40f, 20f), (string)Settings[80]);
                        GUI.Label(new Rect(num11 + 219f, 570f, 10f, 22f), "x:", "Label");
                        GUI.Label(new Rect(num11 + 264f, 570f, 10f, 22f), "y:", "Label");
                        GUI.Label(new Rect(num11 + 155f, 445f, 50f, 20f), "Color:", "Label");
                        GUI.Label(new Rect(num11 + 155f, 470f, 10f, 20f), "R:", "Label");
                        GUI.Label(new Rect(num11 + 155f, 495f, 10f, 20f), "G:", "Label");
                        GUI.Label(new Rect(num11 + 155f, 520f, 10f, 20f), "B:", "Label");
                        Settings[73] = GUI.HorizontalSlider(new Rect(num11 + 170f, 475f, 100f, 20f), (float)Settings[73], 0f, 1f);
                        Settings[74] = GUI.HorizontalSlider(new Rect(num11 + 170f, 500f, 100f, 20f), (float)Settings[74], 0f, 1f);
                        Settings[75] = GUI.HorizontalSlider(new Rect(num11 + 170f, 525f, 100f, 20f), (float)Settings[75], 0f, 1f);
                        GUI.Label(new Rect(num11 + 13f, 554f, 57f, 22f), "Material:", "Label");
                        if (GUI.Button(new Rect(num11 + 66f, 554f, 60f, 20f), (string)Settings[69]))
                        {
                            Settings[78] = 1;
                        }
                        if ((int)Settings[78] == 1)
                        {
                            string[] item = new string[4]
                            {
                                "bark",
                                "bark2",
                                "bark3",
                                "bark4"
                            };
                            string[] item2 = new string[4]
                            {
                                "wood1",
                                "wood2",
                                "wood3",
                                "wood4"
                            };
                            string[] item3 = new string[4]
                            {
                                "grass",
                                "grass2",
                                "grass3",
                                "grass4"
                            };
                            string[] item4 = new string[4]
                            {
                                "brick1",
                                "brick2",
                                "brick3",
                                "brick4"
                            };
                            string[] item5 = new string[4]
                            {
                                "metal1",
                                "metal2",
                                "metal3",
                                "metal4"
                            };
                            string[] item6 = new string[3]
                            {
                                "rock1",
                                "rock2",
                                "rock3"
                            };
                            string[] item7 = new string[10]
                            {
                                "stone1",
                                "stone2",
                                "stone3",
                                "stone4",
                                "stone5",
                                "stone6",
                                "stone7",
                                "stone8",
                                "stone9",
                                "stone10"
                            };
                            string[] item8 = new string[7]
                            {
                                "earth1",
                                "earth2",
                                "ice1",
                                "lava1",
                                "crystal1",
                                "crystal2",
                                "empty"
                            };
                            string[] array5 = new string[0];
                            List<string[]> list = new List<string[]>();
                            list.Add(item);
                            list.Add(item2);
                            list.Add(item3);
                            list.Add(item4);
                            list.Add(item5);
                            list.Add(item6);
                            list.Add(item7);
                            list.Add(item8);
                            List<string[]> list2 = list;
                            string[] array6 = new string[9]
                            {
                                "bark",
                                "wood",
                                "grass",
                                "brick",
                                "metal",
                                "rock",
                                "stone",
                                "misc",
                                "transparent"
                            };
                            int num17 = 78;
                            int num18 = 69;
                            float num14 = (float)(Screen.width / 2) - 110.5f;
                            float num15 = (float)(Screen.height / 2) - 220f;
                            int num19 = (int)Settings[185];
                            float val = 10f + 104f * (float)(list2[num19].Length / 3 + 1);
                            val = Math.Max(val, 280f);
                            GUI.Box(new Rect(num14, num15, 212f, 450f), string.Empty);
                            for (int j = 0; j < list2.Count; j++)
                            {
                                int num20 = j / 3;
                                int num21 = j % 3;
                                if (GUI.Button(new Rect(num14 + 5f + 69f * (float)num21, num15 + 5f + (float)(30 * num20), 64f, 25f), array6[j]))
                                {
                                    Settings[185] = j;
                                }
                            }
                            scroll2 = GUI.BeginScrollView(new Rect(num14, num15 + 110f, 225f, 290f), scroll2, new Rect(num14, num15 + 110f, 212f, val));
                            if (num19 != 8)
                            {
                                for (int j = 0; j < list2[num19].Length; j++)
                                {
                                    int num20 = j / 3;
                                    int num21 = j % 3;
                                    GUI.DrawTexture(new Rect(num14 + 5f + 69f * (float)num21, num15 + 115f + 104f * (float)num20, 64f, 64f), LoadTextureRC("p" + list2[num19][j]));
                                    if (GUI.Button(new Rect(num14 + 5f + 69f * (float)num21, num15 + 184f + 104f * (float)num20, 64f, 30f), list2[num19][j]))
                                    {
                                        Settings[num18] = list2[num19][j];
                                        Settings[num17] = 0;
                                    }
                                }
                            }
                            GUI.EndScrollView();
                            if (GUI.Button(new Rect(num14 + 24f, num15 + 410f, 70f, 30f), "Default"))
                            {
                                Settings[num18] = "default";
                                Settings[num17] = 0;
                            }
                            else if (GUI.Button(new Rect(num14 + 118f, num15 + 410f, 70f, 30f), "Done"))
                            {
                                Settings[num17] = 0;
                            }
                        }
                        bool flag4 = false;
                        if ((int)Settings[76] == 1)
                        {
                            flag4 = true;
                            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
                            texture2D.SetPixel(0, 0, new Color((float)Settings[73], (float)Settings[74], (float)Settings[75], 1f));
                            texture2D.Apply();
                            GUI.DrawTexture(new Rect(num11 + 235f, 445f, 30f, 20f), texture2D, ScaleMode.StretchToFill);
                            UnityEngine.Object.Destroy(texture2D);
                        }
                        bool flag5 = GUI.Toggle(new Rect(num11 + 193f, 445f, 40f, 20f), flag4, "On");
                        if (flag4 != flag5)
                        {
                            if (flag5)
                            {
                                Settings[76] = 1;
                            }
                            else
                            {
                                Settings[76] = 0;
                            }
                        }
                    }
                }
                if (GUI.Button(new Rect(num11 + 5f, 10f, 60f, 25f), "General"))
                {
                    Settings[64] = 101;
                }
                else if (GUI.Button(new Rect(num11 + 70f, 10f, 70f, 25f), "Geometry"))
                {
                    Settings[64] = 102;
                }
                else if (GUI.Button(new Rect(num11 + 145f, 10f, 65f, 25f), "Buildings"))
                {
                    Settings[64] = 103;
                }
                else if (GUI.Button(new Rect(num11 + 215f, 10f, 50f, 25f), "Nature"))
                {
                    Settings[64] = 104;
                }
                else if (GUI.Button(new Rect(num11 + 5f, 45f, 70f, 25f), "Spawners"))
                {
                    Settings[64] = 105;
                }
                else if (GUI.Button(new Rect(num11 + 80f, 45f, 70f, 25f), "Racing"))
                {
                    Settings[64] = 108;
                }
                else if (GUI.Button(new Rect(num11 + 155f, 45f, 40f, 25f), "Misc"))
                {
                    Settings[64] = 107;
                }
                else if (GUI.Button(new Rect(num11 + 200f, 45f, 70f, 25f), "Credits"))
                {
                    Settings[64] = 106;
                }
                float result;

                switch ((int)Settings[64])
                {
                    case 101:
                        scroll = GUI.BeginScrollView(new Rect(num11, 80f, 305f, 350f), scroll, new Rect(num11, 80f, 300f, 470f));
                        GUI.Label(new Rect(num11 + 100f, 80f, 120f, 20f), "General Objects:", "Label");
                        GUI.Label(new Rect(num11 + 108f, 245f, 120f, 20f), "Spawn Points:", "Label");
                        GUI.Label(new Rect(num11 + 7f, 415f, 290f, 60f), "* The above titan spawn points apply only to randomly spawned titans specified by the Random Titan #.", "Label");
                        GUI.Label(new Rect(num11 + 7f, 470f, 290f, 60f), "* If team mode is disabled both cyan and magenta spawn points will be randomly chosen for players.", "Label");
                        GUI.DrawTexture(new Rect(num11 + 27f, 110f, 64f, 64f), LoadTextureRC("psupply"));
                        GUI.DrawTexture(new Rect(num11 + 118f, 110f, 64f, 64f), LoadTextureRC("pcannonwall"));
                        GUI.DrawTexture(new Rect(num11 + 209f, 110f, 64f, 64f), LoadTextureRC("pcannonground"));
                        GUI.DrawTexture(new Rect(num11 + 27f, 275f, 64f, 64f), LoadTextureRC("pspawnt"));
                        GUI.DrawTexture(new Rect(num11 + 118f, 275f, 64f, 64f), LoadTextureRC("pspawnplayerC"));
                        GUI.DrawTexture(new Rect(num11 + 209f, 275f, 64f, 64f), LoadTextureRC("pspawnplayerM"));
                        if (GUI.Button(new Rect(num11 + 27f, 179f, 64f, 60f), "Supply"))
                        {
                            flag = true;
                            GameObject original = (GameObject)Resources.Load("aot_supply");
                            selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
                            selectedObj.name = "base,aot_supply";
                        }
                        else if (GUI.Button(new Rect(num11 + 118f, 179f, 64f, 60f), "Cannon \nWall"))
                        {
                            flag = true;
                            GameObject original = (GameObject)RCAssets.Load("CannonWallProp");
                            selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
                            selectedObj.name = "photon,CannonWall";
                        }
                        else if (GUI.Button(new Rect(num11 + 209f, 179f, 64f, 60f), "Cannon\n Ground"))
                        {
                            flag = true;
                            GameObject original = (GameObject)RCAssets.Load("CannonGroundProp");
                            selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
                            selectedObj.name = "photon,CannonGround";
                        }
                        else if (GUI.Button(new Rect(num11 + 27f, 344f, 64f, 60f), "Titan"))
                        {
                            flag = true;
                            flag2 = true;
                            GameObject original = (GameObject)RCAssets.Load("titan");
                            selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
                            selectedObj.name = "spawnpoint,titan";
                        }
                        else if (GUI.Button(new Rect(num11 + 118f, 344f, 64f, 60f), "Player \nCyan"))
                        {
                            flag = true;
                            flag2 = true;
                            GameObject original = (GameObject)RCAssets.Load("playerC");
                            selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
                            selectedObj.name = "spawnpoint,playerC";
                        }
                        else if (GUI.Button(new Rect(num11 + 209f, 344f, 64f, 60f), "Player \nMagenta"))
                        {
                            flag = true;
                            flag2 = true;
                            GameObject original = (GameObject)RCAssets.Load("playerM");
                            selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
                            selectedObj.name = "spawnpoint,playerM";
                        }
                        GUI.EndScrollView();
                        break;
                    case 102:
                        {
                            string[] array7 = new string[12]
                            {
                                "cuboid",
                                "plane",
                                "sphere",
                                "cylinder",
                                "capsule",
                                "pyramid",
                                "cone",
                                "prism",
                                "arc90",
                                "arc180",
                                "torus",
                                "tube"
                            };
                            for (int j = 0; j < array7.Length; j++)
                            {
                                int num21 = j % 4;
                                int num20 = j / 4;
                                GUI.DrawTexture(new Rect(num11 + 7.8f + 71.8f * (float)num21, 90f + 114f * (float)num20, 64f, 64f), LoadTextureRC("p" + array7[j]));
                                if (GUI.Button(new Rect(num11 + 7.8f + 71.8f * (float)num21, 159f + 114f * (float)num20, 64f, 30f), array7[j]))
                                {
                                    flag = true;
                                    GameObject original2 = (GameObject)RCAssets.Load(array7[j]);
                                    selectedObj = (GameObject)UnityEngine.Object.Instantiate(original2);
                                    selectedObj.name = "custom," + array7[j];
                                }
                            }
                        }
                        break;
                    case 103:
                        {
                            List<string> list3 = new List<string>();
                            list3.Add("arch1");
                            list3.Add("house1");
                            List<string> list4 = list3;
                            string[] array7 = new string[44]
                            {
                                "tower1",
                                "tower2",
                                "tower3",
                                "tower4",
                                "tower5",
                                "house1",
                                "house2",
                                "house3",
                                "house4",
                                "house5",
                                "house6",
                                "house7",
                                "house8",
                                "house9",
                                "house10",
                                "house11",
                                "house12",
                                "house13",
                                "house14",
                                "pillar1",
                                "pillar2",
                                "village1",
                                "village2",
                                "windmill1",
                                "arch1",
                                "canal1",
                                "castle1",
                                "church1",
                                "cannon1",
                                "statue1",
                                "statue2",
                                "wagon1",
                                "elevator1",
                                "bridge1",
                                "dummy1",
                                "spike1",
                                "wall1",
                                "wall2",
                                "wall3",
                                "wall4",
                                "arena1",
                                "arena2",
                                "arena3",
                                "arena4"
                            };
                            float val = 110f + 114f * (float)((array7.Length - 1) / 4);
                            scroll = GUI.BeginScrollView(new Rect(num11, 90f, 303f, 350f), scroll, new Rect(num11, 90f, 300f, val));
                            for (int j = 0; j < array7.Length; j++)
                            {
                                int num21 = j % 4;
                                int num20 = j / 4;
                                GUI.DrawTexture(new Rect(num11 + 7.8f + 71.8f * (float)num21, 90f + 114f * (float)num20, 64f, 64f), LoadTextureRC("p" + array7[j]));
                                if (GUI.Button(new Rect(num11 + 7.8f + 71.8f * (float)num21, 159f + 114f * (float)num20, 64f, 30f), array7[j]))
                                {
                                    flag = true;
                                    GameObject original4 = (GameObject)RCAssets.Load(array7[j]);
                                    selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
                                    if (list4.Contains(array7[j]))
                                    {
                                        selectedObj.name = "customb," + array7[j];
                                    }
                                    else
                                    {
                                        selectedObj.name = "custom," + array7[j];
                                    }
                                }
                            }
                            GUI.EndScrollView();
                        }
                        break;
                    case 104:
                        {
                            List<string> list5 = new List<string>();
                            list5.Add("tree0");
                            List<string> list4 = list5;
                            string[] array7 = new string[23]
                            {
                                "leaf0",
                                "leaf1",
                                "leaf2",
                                "field1",
                                "field2",
                                "tree0",
                                "tree1",
                                "tree2",
                                "tree3",
                                "tree4",
                                "tree5",
                                "tree6",
                                "tree7",
                                "log1",
                                "log2",
                                "trunk1",
                                "boulder1",
                                "boulder2",
                                "boulder3",
                                "boulder4",
                                "boulder5",
                                "cave1",
                                "cave2"
                            };
                            float val = 110f + 114f * (float)((array7.Length - 1) / 4);
                            scroll = GUI.BeginScrollView(new Rect(num11, 90f, 303f, 350f), scroll, new Rect(num11, 90f, 300f, val));
                            for (int j = 0; j < array7.Length; j++)
                            {
                                int num21 = j % 4;
                                int num20 = j / 4;
                                GUI.DrawTexture(new Rect(num11 + 7.8f + 71.8f * (float)num21, 90f + 114f * (float)num20, 64f, 64f), LoadTextureRC("p" + array7[j]));
                                if (GUI.Button(new Rect(num11 + 7.8f + 71.8f * (float)num21, 159f + 114f * (float)num20, 64f, 30f), array7[j]))
                                {
                                    flag = true;
                                    GameObject original4 = (GameObject)RCAssets.Load(array7[j]);
                                    selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
                                    if (list4.Contains(array7[j]))
                                    {
                                        selectedObj.name = "customb," + array7[j];
                                    }
                                    else
                                    {
                                        selectedObj.name = "custom," + array7[j];
                                    }
                                }
                            }
                            GUI.EndScrollView();
                        }
                        break;
                    case 105:
                        {
                            GUI.Label(new Rect(num11 + 95f, 85f, 130f, 20f), "Custom Spawners:", "Label");
                            GUI.DrawTexture(new Rect(num11 + 7.8f, 110f, 64f, 64f), LoadTextureRC("ptitan"));
                            GUI.DrawTexture(new Rect(num11 + 79.6f, 110f, 64f, 64f), LoadTextureRC("pabnormal"));
                            GUI.DrawTexture(new Rect(num11 + 151.4f, 110f, 64f, 64f), LoadTextureRC("pjumper"));
                            GUI.DrawTexture(new Rect(num11 + 223.2f, 110f, 64f, 64f), LoadTextureRC("pcrawler"));
                            GUI.DrawTexture(new Rect(num11 + 7.8f, 224f, 64f, 64f), LoadTextureRC("ppunk"));
                            GUI.DrawTexture(new Rect(num11 + 79.6f, 224f, 64f, 64f), LoadTextureRC("pannie"));
                            float result2;
                            if (GUI.Button(new Rect(num11 + 7.8f, 179f, 64f, 30f), "Titan"))
                            {
                                if (!float.TryParse((string)Settings[83], out result2))
                                {
                                    Settings[83] = "30";
                                }
                                flag = true;
                                flag2 = true;
                                GameObject original3 = (GameObject)RCAssets.Load("spawnTitan");
                                selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
                                selectedObj.name = "photon,spawnTitan," + (string)Settings[83] + "," + ((int)Settings[84]).ToString();
                            }
                            else if (GUI.Button(new Rect(num11 + 79.6f, 179f, 64f, 30f), "Aberrant"))
                            {
                                if (!float.TryParse((string)Settings[83], out result2))
                                {
                                    Settings[83] = "30";
                                }
                                flag = true;
                                flag2 = true;
                                GameObject original3 = (GameObject)RCAssets.Load("spawnAbnormal");
                                selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
                                selectedObj.name = "photon,spawnAbnormal," + (string)Settings[83] + "," + ((int)Settings[84]).ToString();
                            }
                            else if (GUI.Button(new Rect(num11 + 151.4f, 179f, 64f, 30f), "Jumper"))
                            {
                                if (!float.TryParse((string)Settings[83], out result2))
                                {
                                    Settings[83] = "30";
                                }
                                flag = true;
                                flag2 = true;
                                GameObject original3 = (GameObject)RCAssets.Load("spawnJumper");
                                selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
                                selectedObj.name = "photon,spawnJumper," + (string)Settings[83] + "," + ((int)Settings[84]).ToString();
                            }
                            else if (GUI.Button(new Rect(num11 + 223.2f, 179f, 64f, 30f), "Crawler"))
                            {
                                if (!float.TryParse((string)Settings[83], out result2))
                                {
                                    Settings[83] = "30";
                                }
                                flag = true;
                                flag2 = true;
                                GameObject original3 = (GameObject)RCAssets.Load("spawnCrawler");
                                selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
                                selectedObj.name = "photon,spawnCrawler," + (string)Settings[83] + "," + ((int)Settings[84]).ToString();
                            }
                            else if (GUI.Button(new Rect(num11 + 7.8f, 293f, 64f, 30f), "Punk"))
                            {
                                if (!float.TryParse((string)Settings[83], out result2))
                                {
                                    Settings[83] = "30";
                                }
                                flag = true;
                                flag2 = true;
                                GameObject original3 = (GameObject)RCAssets.Load("spawnPunk");
                                selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
                                selectedObj.name = "photon,spawnPunk," + (string)Settings[83] + "," + ((int)Settings[84]).ToString();
                            }
                            else if (GUI.Button(new Rect(num11 + 79.6f, 293f, 64f, 30f), "Annie"))
                            {
                                if (!float.TryParse((string)Settings[83], out result2))
                                {
                                    Settings[83] = "30";
                                }
                                flag = true;
                                flag2 = true;
                                GameObject original3 = (GameObject)RCAssets.Load("spawnAnnie");
                                selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
                                selectedObj.name = "photon,spawnAnnie," + (string)Settings[83] + "," + ((int)Settings[84]).ToString();
                            }
                            GUI.Label(new Rect(num11 + 7f, 379f, 140f, 22f), "Spawn Timer:", "Label");
                            Settings[83] = GUI.TextField(new Rect(num11 + 100f, 379f, 50f, 20f), (string)Settings[83]);
                            GUI.Label(new Rect(num11 + 7f, 356f, 140f, 22f), "Endless spawn:", "Label");
                            GUI.Label(new Rect(num11 + 7f, 405f, 290f, 80f), "* The above settings apply only to the next placed spawner. You can have unique spawn times and settings for each individual titan spawner.", "Label");
                            bool flag8 = false;
                            if ((int)Settings[84] == 1)
                            {
                                flag8 = true;
                            }
                            bool flag9 = GUI.Toggle(new Rect(num11 + 100f, 356f, 40f, 20f), flag8, "On");
                            if (flag8 != flag9)
                            {
                                if (flag9)
                                {
                                    Settings[84] = 1;
                                }
                                else
                                {
                                    Settings[84] = 0;
                                }
                            }
                        }
                        break;
                    case 106:
                        GUI.Label(new Rect(num11 + 10f, 80f, 200f, 22f), "- Tree 2 designed by Ken P.", "Label");
                        GUI.Label(new Rect(num11 + 10f, 105f, 250f, 22f), "- Tower 2, House 5 designed by Matthew Santos", "Label");
                        GUI.Label(new Rect(num11 + 10f, 130f, 200f, 22f), "- Cannon retextured by Mika", "Label");
                        GUI.Label(new Rect(num11 + 10f, 155f, 200f, 22f), "- Arena 1,2,3 & 4 created by Gun", "Label");
                        GUI.Label(new Rect(num11 + 10f, 180f, 250f, 22f), "- Cannon Wall/Ground textured by Bellfox", "Label");
                        GUI.Label(new Rect(num11 + 10f, 205f, 250f, 120f), "- House 7 - 14, Statue1, Statue2, Wagon1, Wall 1, Wall 2, Wall 3, Wall 4, CannonWall, CannonGround, Tower5, Bridge1, Dummy1, Spike1 created by meecube", "Label");
                        break;
                    case 107:
                        {
                            GUI.DrawTexture(new Rect(num11 + 30f, 90f, 64f, 64f), LoadTextureRC("pbarrier"));
                            GUI.DrawTexture(new Rect(num11 + 30f, 199f, 64f, 64f), LoadTextureRC("pregion"));
                            GUI.Label(new Rect(num11 + 110f, 243f, 200f, 22f), "Region Name:", "Label");
                            GUI.Label(new Rect(num11 + 110f, 179f, 200f, 22f), "Disable Map Bounds:", "Label");
                            bool flag6 = false;
                            if ((int)Settings[186] == 1)
                            {
                                flag6 = true;
                                if (!LinkHash[3].ContainsKey("mapbounds"))
                                {
                                    LinkHash[3].Add("mapbounds", "map,disablebounds");
                                }
                            }
                            else if (LinkHash[3].ContainsKey("mapbounds"))
                            {
                                LinkHash[3].Remove("mapbounds");
                            }
                            if (GUI.Button(new Rect(num11 + 30f, 159f, 64f, 30f), "Barrier"))
                            {
                                flag = true;
                                flag2 = true;
                                GameObject original2 = (GameObject)RCAssets.Load("barrierEditor");
                                selectedObj = (GameObject)UnityEngine.Object.Instantiate(original2);
                                selectedObj.name = "misc,barrier";
                            }
                            else if (GUI.Button(new Rect(num11 + 30f, 268f, 64f, 30f), "Region"))
                            {
                                if ((string)Settings[191] == string.Empty)
                                {
                                    Settings[191] = "Region" + UnityEngine.Random.Range(10000, 99999).ToString();
                                }
                                flag = true;
                                flag2 = true;
                                GameObject original2 = (GameObject)RCAssets.Load("regionEditor");
                                selectedObj = (GameObject)UnityEngine.Object.Instantiate(original2);
                                GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
                                gameObject3.name = "RegionLabel";
                                if (!float.TryParse((string)Settings[71], out result))
                                {
                                    Settings[71] = "1";
                                }
                                if (!float.TryParse((string)Settings[70], out result))
                                {
                                    Settings[70] = "1";
                                }
                                if (!float.TryParse((string)Settings[72], out result))
                                {
                                    Settings[72] = "1";
                                }
                                gameObject3.transform.parent = selectedObj.transform;
                                float y = 1f;
                                if (Convert.ToSingle((string)Settings[71]) > 100f)
                                {
                                    y = 0.8f;
                                }
                                else if (Convert.ToSingle((string)Settings[71]) > 1000f)
                                {
                                    y = 0.5f;
                                }
                                gameObject3.transform.localPosition = new Vector3(0f, y, 0f);
                                gameObject3.transform.localScale = new Vector3(5f / Convert.ToSingle((string)Settings[70]), 5f / Convert.ToSingle((string)Settings[71]), 5f / Convert.ToSingle((string)Settings[72]));
                                gameObject3.GetComponent<UILabel>().text = (string)Settings[191];
                                selectedObj.AddComponent<RCRegionLabel>();
                                selectedObj.GetComponent<RCRegionLabel>().myLabel = gameObject3;
                                selectedObj.name = "misc,region," + (string)Settings[191];
                            }
                            Settings[191] = GUI.TextField(new Rect(num11 + 200f, 243f, 75f, 20f), (string)Settings[191]);
                            bool flag7 = GUI.Toggle(new Rect(num11 + 240f, 179f, 40f, 20f), flag6, "On");
                            if (flag7 != flag6)
                            {
                                if (flag7)
                                {
                                    Settings[186] = 1;
                                }
                                else
                                {
                                    Settings[186] = 0;
                                }
                            }
                        }
                        break;
                    case 108:
                        {
                            string[] array8 = new string[12]
                            {
                                "Cuboid",
                                "Plane",
                                "Sphere",
                                "Cylinder",
                                "Capsule",
                                "Pyramid",
                                "Cone",
                                "Prism",
                                "Arc90",
                                "Arc180",
                                "Torus",
                                "Tube"
                            };
                            string[] array7 = new string[12];
                            for (int j = 0; j < array7.Length; j++)
                            {
                                array7[j] = "start" + array8[j];
                            }
                            float val = 110f + 114f * (float)((array7.Length - 1) / 4);
                            val *= 4f;
                            val += 200f;
                            scroll = GUI.BeginScrollView(new Rect(num11, 90f, 303f, 350f), scroll, new Rect(num11, 90f, 300f, val));
                            GUI.Label(new Rect(num11 + 90f, 90f, 200f, 22f), "Racing Start Barrier");
                            int num22 = 125;
                            for (int j = 0; j < array7.Length; j++)
                            {
                                int num21 = j % 4;
                                int num20 = j / 4;
                                GUI.DrawTexture(new Rect(num11 + 7.8f + 71.8f * (float)num21, (float)num22 + 114f * (float)num20, 64f, 64f), LoadTextureRC("p" + array7[j]));
                                if (GUI.Button(new Rect(num11 + 7.8f + 71.8f * (float)num21, (float)num22 + 69f + 114f * (float)num20, 64f, 30f), array8[j]))
                                {
                                    flag = true;
                                    flag2 = true;
                                    GameObject original4 = (GameObject)RCAssets.Load(array7[j]);
                                    selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
                                    selectedObj.name = "racing," + array7[j];
                                }
                            }
                            num22 += 114 * (array7.Length / 4) + 10;
                            GUI.Label(new Rect(num11 + 93f, num22, 200f, 22f), "Racing End Trigger");
                            num22 += 35;
                            for (int j = 0; j < array7.Length; j++)
                            {
                                array7[j] = "end" + array8[j];
                            }
                            for (int j = 0; j < array7.Length; j++)
                            {
                                int num21 = j % 4;
                                int num20 = j / 4;
                                GUI.DrawTexture(new Rect(num11 + 7.8f + 71.8f * (float)num21, (float)num22 + 114f * (float)num20, 64f, 64f), LoadTextureRC("p" + array7[j]));
                                if (GUI.Button(new Rect(num11 + 7.8f + 71.8f * (float)num21, (float)num22 + 69f + 114f * (float)num20, 64f, 30f), array8[j]))
                                {
                                    flag = true;
                                    flag2 = true;
                                    GameObject original4 = (GameObject)RCAssets.Load(array7[j]);
                                    selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
                                    selectedObj.name = "racing," + array7[j];
                                }
                            }
                            num22 += 114 * (array7.Length / 4) + 10;
                            GUI.Label(new Rect(num11 + 113f, num22, 200f, 22f), "Kill Trigger");
                            num22 += 35;
                            for (int j = 0; j < array7.Length; j++)
                            {
                                array7[j] = "kill" + array8[j];
                            }
                            for (int j = 0; j < array7.Length; j++)
                            {
                                int num21 = j % 4;
                                int num20 = j / 4;
                                GUI.DrawTexture(new Rect(num11 + 7.8f + 71.8f * (float)num21, (float)num22 + 114f * (float)num20, 64f, 64f), LoadTextureRC("p" + array7[j]));
                                if (GUI.Button(new Rect(num11 + 7.8f + 71.8f * (float)num21, (float)num22 + 69f + 114f * (float)num20, 64f, 30f), array8[j]))
                                {
                                    flag = true;
                                    flag2 = true;
                                    GameObject original4 = (GameObject)RCAssets.Load(array7[j]);
                                    selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
                                    selectedObj.name = "racing," + array7[j];
                                }
                            }
                            num22 += 114 * (array7.Length / 4) + 10;
                            GUI.Label(new Rect(num11 + 95f, num22, 200f, 22f), "Checkpoint Trigger");
                            num22 += 35;
                            for (int j = 0; j < array7.Length; j++)
                            {
                                array7[j] = "checkpoint" + array8[j];
                            }
                            for (int j = 0; j < array7.Length; j++)
                            {
                                int num21 = j % 4;
                                int num20 = j / 4;
                                GUI.DrawTexture(new Rect(num11 + 7.8f + 71.8f * (float)num21, (float)num22 + 114f * (float)num20, 64f, 64f), LoadTextureRC("p" + array7[j]));
                                if (GUI.Button(new Rect(num11 + 7.8f + 71.8f * (float)num21, (float)num22 + 69f + 114f * (float)num20, 64f, 30f), array8[j]))
                                {
                                    flag = true;
                                    flag2 = true;
                                    GameObject original4 = (GameObject)RCAssets.Load(array7[j]);
                                    selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
                                    selectedObj.name = "racing," + array7[j];
                                }
                            }
                            GUI.EndScrollView();
                        }
                        break;
                }
                if (!flag || !(selectedObj != null))
                {
                    return;
                }
                if (!float.TryParse((string)Settings[70], out result))
                {
                    Settings[70] = "1";
                }
                if (!float.TryParse((string)Settings[71], out result))
                {
                    Settings[71] = "1";
                }
                if (!float.TryParse((string)Settings[72], out result))
                {
                    Settings[72] = "1";
                }
                if (!float.TryParse((string)Settings[79], out result))
                {
                    Settings[79] = "1";
                }
                if (!float.TryParse((string)Settings[80], out result))
                {
                    Settings[80] = "1";
                }
                if (!flag2)
                {
                    float a = 1f;
                    if ((string)Settings[69] != "default")
                    {
                        if (((string)Settings[69]).StartsWith("transparent"))
                        {
                            if (float.TryParse(((string)Settings[69]).Substring(11), out float result3))
                            {
                                a = result3;
                            }
                            Renderer[] componentsInChildren = selectedObj.GetComponentsInChildren<Renderer>();
                            foreach (Renderer renderer2 in componentsInChildren)
                            {
                                renderer2.material = (Material)RCAssets.Load("transparent");
                                renderer2.material.mainTextureScale = new Vector2(renderer2.material.mainTextureScale.x * Convert.ToSingle((string)Settings[79]), renderer2.material.mainTextureScale.y * Convert.ToSingle((string)Settings[80]));
                            }
                        }
                        else
                        {
                            Renderer[] componentsInChildren = selectedObj.GetComponentsInChildren<Renderer>();
                            foreach (Renderer renderer2 in componentsInChildren)
                            {
                                if (!renderer2.name.Contains("Particle System") || !selectedObj.name.Contains("aot_supply"))
                                {
                                    renderer2.material = (Material)RCAssets.Load((string)Settings[69]);
                                    renderer2.material.mainTextureScale = new Vector2(renderer2.material.mainTextureScale.x * Convert.ToSingle((string)Settings[79]), renderer2.material.mainTextureScale.y * Convert.ToSingle((string)Settings[80]));
                                }
                            }
                        }
                    }
                    float num23 = 1f;
                    MeshFilter[] componentsInChildren2 = selectedObj.GetComponentsInChildren<MeshFilter>();
                    foreach (MeshFilter meshFilter in componentsInChildren2)
                    {
                        if (selectedObj.name.StartsWith("customb"))
                        {
                            if (num23 < meshFilter.mesh.bounds.size.y)
                            {
                                num23 = meshFilter.mesh.bounds.size.y;
                            }
                        }
                        else if (num23 < meshFilter.mesh.bounds.size.z)
                        {
                            num23 = meshFilter.mesh.bounds.size.z;
                        }
                    }
                    float num24 = selectedObj.transform.localScale.x * Convert.ToSingle((string)Settings[70]);
                    num24 -= 0.001f;
                    float y3 = selectedObj.transform.localScale.y * Convert.ToSingle((string)Settings[71]);
                    float z2 = selectedObj.transform.localScale.z * Convert.ToSingle((string)Settings[72]);
                    selectedObj.transform.localScale = new Vector3(num24, y3, z2);
                    if ((int)Settings[76] == 1)
                    {
                        Color color = new Color((float)Settings[73], (float)Settings[74], (float)Settings[75], a);
                        componentsInChildren2 = selectedObj.GetComponentsInChildren<MeshFilter>();
                        foreach (MeshFilter meshFilter in componentsInChildren2)
                        {
                            Mesh mesh = meshFilter.mesh;
                            Color[] array4 = new Color[mesh.vertexCount];
                            for (int k = 0; k < mesh.vertexCount; k++)
                            {
                                array4[k] = color;
                            }
                            mesh.colors = array4;
                        }
                    }
                    float num25 = selectedObj.transform.localScale.z;
                    if (selectedObj.name.Contains("boulder2") || selectedObj.name.Contains("boulder3") || selectedObj.name.Contains("field2"))
                    {
                        num25 *= 0.01f;
                    }
                    float num26 = 10f + num25 * num23 * 1.2f / 2f;
                    selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * num26, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * num26);
                    selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
                    string text4 = selectedObj.name;
                    selectedObj.name = text4 + "," + (string)Settings[69] + "," + (string)Settings[70] + "," + (string)Settings[71] + "," + (string)Settings[72] + "," + Settings[76].ToString() + "," + ((float)Settings[73]).ToString() + "," + ((float)Settings[74]).ToString() + "," + ((float)Settings[75]).ToString() + "," + (string)Settings[79] + "," + (string)Settings[80];
                    UnloadAssetsEditor();
                }
                else if (selectedObj.name.StartsWith("misc"))
                {
                    if (selectedObj.name.Contains("barrier") || selectedObj.name.Contains("region") || selectedObj.name.Contains("racing"))
                    {
                        float num23 = 1f;
                        float num24 = selectedObj.transform.localScale.x * Convert.ToSingle((string)Settings[70]);
                        num24 -= 0.001f;
                        float y3 = selectedObj.transform.localScale.y * Convert.ToSingle((string)Settings[71]);
                        float z2 = selectedObj.transform.localScale.z * Convert.ToSingle((string)Settings[72]);
                        selectedObj.transform.localScale = new Vector3(num24, y3, z2);
                        float num25 = selectedObj.transform.localScale.z;
                        float num26 = 10f + num25 * num23 * 1.2f / 2f;
                        selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * num26, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * num26);
                        if (!selectedObj.name.Contains("region"))
                        {
                            selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
                        }
                        string text4 = selectedObj.name;
                        selectedObj.name = text4 + "," + (string)Settings[70] + "," + (string)Settings[71] + "," + (string)Settings[72];
                    }
                }
                else if (selectedObj.name.StartsWith("racing"))
                {
                    float num23 = 1f;
                    float num24 = selectedObj.transform.localScale.x * Convert.ToSingle((string)Settings[70]);
                    num24 -= 0.001f;
                    float y3 = selectedObj.transform.localScale.y * Convert.ToSingle((string)Settings[71]);
                    float z2 = selectedObj.transform.localScale.z * Convert.ToSingle((string)Settings[72]);
                    selectedObj.transform.localScale = new Vector3(num24, y3, z2);
                    float num25 = selectedObj.transform.localScale.z;
                    float num26 = 10f + num25 * num23 * 1.2f / 2f;
                    selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * num26, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * num26);
                    selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
                    string text4 = selectedObj.name;
                    selectedObj.name = text4 + "," + (string)Settings[70] + "," + (string)Settings[71] + "," + (string)Settings[72];
                }
                else
                {
                    selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * 10f, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * 10f);
                    selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
                }
                Screen.lockCursor = true;
                GUI.FocusControl(null);
            }
            else if (inputManager != null && inputManager.menuOn)
            {
                Screen.showCursor = true;
                Screen.lockCursor = false;
                if ((int)Settings[64] == 6)
                {
                    return;
                }
                float halfMenuWidth = (float)Screen.width / 2f - 350f;
                float halfMenuHeight = (float)Screen.height / 2f - 250f;
                GUI.Box(new Rect(halfMenuWidth, halfMenuHeight, 700f, 500f), string.Empty);
                if (GUI.Button(new Rect(halfMenuWidth + 7f, halfMenuHeight + 7f, 59f, 25f), "General"))
                {
                    Settings[64] = 0;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 71f, halfMenuHeight + 7f, 60f, 25f), "Rebinds"))
                {
                    Settings[64] = 1;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 136f, halfMenuHeight + 7f, 85f, 25f), "Human Skins"))
                {
                    Settings[64] = 2;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 226f, halfMenuHeight + 7f, 85f, 25f), "Titan Skins"))
                {
                    Settings[64] = 3;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 316f, halfMenuHeight + 7f, 85f, 25f), "Level Skins"))
                {
                    Settings[64] = 7;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 406f, halfMenuHeight + 7f, 85f, 25f), "Custom Map"))
                {
                    Settings[64] = 8;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 496f, halfMenuHeight + 7f, 93f, 25f), "Custom Logic"))
                {
                    Settings[64] = 9;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 594f, halfMenuHeight + 7f, 99f, 25f), "Game Settings"))
                {
                    Settings[64] = 10;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 7f, halfMenuHeight + 37f, 70f, 25f), "Abilities"))
                {
                    Settings[64] = 11;
                }

                switch ((int)Settings[64])
                {
                    case 0:
                        {
                            GUI.Label(new Rect(halfMenuWidth + 150f, halfMenuHeight + 51f, 185f, 22f), "Graphics", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 81f, 185f, 22f), "Disable custom gas textures:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 106f, 185f, 22f), "Disable weapon trail:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 131f, 185f, 22f), "Disable wind effect:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 156f, 185f, 22f), "Enable vSync:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 184f, 227f, 20f), "FPS Cap (0 for disabled):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 212f, 150f, 22f), "Texture Quality:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 242f, 150f, 22f), "Overall Quality:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 272f, 185f, 22f), "Disable Mipmapping:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 297f, 185f, 65f), "*Disabling mipmapping will increase custom texture quality at the cost of performance.", "Label");
                            qualitySlider = GUI.HorizontalSlider(new Rect(halfMenuWidth + 199f, halfMenuHeight + 247f, 115f, 20f), qualitySlider, 0f, 1f);
                            PlayerPrefs.SetFloat("GameQuality", qualitySlider);
                            if (qualitySlider < 0.167f)
                            {
                                QualitySettings.SetQualityLevel(0, applyExpensiveChanges: true);
                            }
                            else if (qualitySlider < 0.33f)
                            {
                                QualitySettings.SetQualityLevel(1, applyExpensiveChanges: true);
                            }
                            else if (qualitySlider < 0.5f)
                            {
                                QualitySettings.SetQualityLevel(2, applyExpensiveChanges: true);
                            }
                            else if (qualitySlider < 0.67f)
                            {
                                QualitySettings.SetQualityLevel(3, applyExpensiveChanges: true);
                            }
                            else if (qualitySlider < 0.83f)
                            {
                                QualitySettings.SetQualityLevel(4, applyExpensiveChanges: true);
                            }
                            else if (qualitySlider <= 1f)
                            {
                                QualitySettings.SetQualityLevel(5, applyExpensiveChanges: true);
                            }
                            if (qualitySlider >= 0.9f && !Level.Name.StartsWith("Custom"))
                            {
                                Camera.main.GetComponent<TiltShift>().enabled = true;
                            }
                            else
                            {
                                Camera.main.GetComponent<TiltShift>().enabled = false;
                            }
                            bool flag13 = false;
                            bool flag14 = false;
                            bool showSpeedLines = false;
                            bool flag16 = false;
                            bool vsyncEnabled = false;
                            if ((int)Settings[15] == 1)
                            {
                                flag13 = true;
                            }
                            if ((int)Settings[92] == 1)
                            {
                                flag14 = true;
                            }
                            if ((int)Settings[93] == 1)
                            {
                                showSpeedLines = true;
                            }
                            if ((int)Settings[63] == 1)
                            {
                                flag16 = true;
                            }
                            if ((int)Settings[183] == 1)
                            {
                                vsyncEnabled = true;
                            }
                            bool flag18 = GUI.Toggle(new Rect(halfMenuWidth + 274f, halfMenuHeight + 81f, 40f, 20f), flag13, "On");
                            if (flag18 != flag13)
                            {
                                if (flag18)
                                {
                                    Settings[15] = 1;
                                }
                                else
                                {
                                    Settings[15] = 0;
                                }
                            }
                            bool flag9 = GUI.Toggle(new Rect(halfMenuWidth + 274f, halfMenuHeight + 106f, 40f, 20f), flag14, "On");
                            if (flag9 != flag14)
                            {
                                if (flag9)
                                {
                                    Settings[92] = 1;
                                }
                                else
                                {
                                    Settings[92] = 0;
                                }
                            }
                            bool toggleSpeedLines = GUI.Toggle(new Rect(halfMenuWidth + 274f, halfMenuHeight + 131f, 40f, 20f), showSpeedLines, "On");
                            if (toggleSpeedLines != showSpeedLines)
                            {
                                if (toggleSpeedLines)
                                {
                                    Settings[93] = 1;
                                }
                                else
                                {
                                    Settings[93] = 0;
                                }
                            }
                            bool toggleVsync = GUI.Toggle(new Rect(halfMenuWidth + 274f, halfMenuHeight + 156f, 40f, 20f), vsyncEnabled, "On");
                            if (toggleVsync != vsyncEnabled)
                            {
                                if (toggleVsync)
                                {
                                    Settings[183] = 1;
                                    QualitySettings.vSyncCount = 1;
                                }
                                else
                                {
                                    Settings[183] = 0;
                                    QualitySettings.vSyncCount = 0;
                                }

                                Minimap.WaitAndTryRecaptureInstance(0.5f);
                            }
                            bool flag21 = GUI.Toggle(new Rect(halfMenuWidth + 274f, halfMenuHeight + 272f, 40f, 20f), flag16, "On");
                            if (flag21 != flag16)
                            {
                                if (flag21)
                                {
                                    Settings[63] = 1;
                                }
                                else
                                {
                                    Settings[63] = 0;
                                }
                                LinkHash[0].Clear();
                                LinkHash[1].Clear();
                                LinkHash[2].Clear();
                            }
                            if (GUI.Button(new Rect(halfMenuWidth + 199f, halfMenuHeight + 212f, 115f, 20f), MasterTextureType(QualitySettings.masterTextureLimit)))
                            {
                                if (QualitySettings.masterTextureLimit <= 0)
                                {
                                    QualitySettings.masterTextureLimit = 8;
                                }
                                else
                                {
                                    QualitySettings.masterTextureLimit--;
                                }
                                LinkHash[0].Clear();
                                LinkHash[1].Clear();
                                LinkHash[2].Clear();
                            }
                            Settings[184] = GUI.TextField(new Rect(halfMenuWidth + 234f, halfMenuHeight + 184f, 80f, 20f), (string)Settings[184]);
                            Application.targetFrameRate = -1;
                            if (int.TryParse((string)Settings[184], out int result4) && result4 > 0)
                            {
                                Application.targetFrameRate = result4;
                            }
                            GUI.Label(new Rect(halfMenuWidth + 470f, halfMenuHeight + 51f, 185f, 22f), "Snapshots", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 81f, 185f, 22f), "Enable Snapshots:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 106f, 185f, 22f), "Show In Game:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 131f, 227f, 22f), "Snapshot Minimum Damage:", "Label");
                            Settings[95] = GUI.TextField(new Rect(halfMenuWidth + 563f, halfMenuHeight + 131f, 65f, 20f), (string)Settings[95]);
                            bool enableSnapshots = false;
                            bool snowSnapshotsInGame = false;
                            if (PlayerPrefs.GetInt("EnableSS", 0) == 1)
                            {
                                enableSnapshots = true;
                            }
                            if (PlayerPrefs.GetInt("showSSInGame", 0) == 1)
                            {
                                snowSnapshotsInGame = true;
                            }
                            bool toggleSnapshots = GUI.Toggle(new Rect(halfMenuWidth + 588f, halfMenuHeight + 81f, 40f, 20f), enableSnapshots, "On");
                            if (toggleSnapshots != enableSnapshots)
                            {
                                if (toggleSnapshots)
                                {
                                    PlayerPrefs.SetInt("EnableSS", 1);
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("EnableSS", 0);
                                }
                            }
                            bool toggleSnapshotsInGame = GUI.Toggle(new Rect(halfMenuWidth + 588f, halfMenuHeight + 106f, 40f, 20f), snowSnapshotsInGame, "On");
                            if (snowSnapshotsInGame != toggleSnapshotsInGame)
                            {
                                if (toggleSnapshotsInGame)
                                {
                                    PlayerPrefs.SetInt("showSSInGame", 1);
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("showSSInGame", 0);
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 485f, halfMenuHeight + 161f, 185f, 22f), "Other", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 186f, 80f, 20f), "Volume:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 211f, 95f, 20f), "Mouse Speed:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 236f, 95f, 20f), "Camera Dist:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 261f, 80f, 20f), "Camera Tilt:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 283f, 80f, 20f), "Invert Mouse:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 305f, 80f, 20f), "Speedometer:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 375f, 80f, 20f), "Minimap:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 386f, halfMenuHeight + 397f, 100f, 20f), "Game Feed:", "Label");
                            string[] texts = new string[3]
                            {
                                "Off",
                                "Speed",
                                "Damage"
                            };
                            Settings[189] = GUI.SelectionGrid(new Rect(halfMenuWidth + 480f, halfMenuHeight + 305f, 140f, 60f), (int)Settings[189], texts, 1, GUI.skin.toggle);
                            AudioListener.volume = GUI.HorizontalSlider(new Rect(halfMenuWidth + 478f, halfMenuHeight + 191f, 150f, 20f), AudioListener.volume, 0f, 1f);
                            mouseSlider = GUI.HorizontalSlider(new Rect(halfMenuWidth + 478f, halfMenuHeight + 216f, 150f, 20f), mouseSlider, 0.1f, 1f);
                            PlayerPrefs.SetFloat("MouseSensitivity", mouseSlider);
                            IN_GAME_MAIN_CAMERA.SensitivityMulti = PlayerPrefs.GetFloat("MouseSensitivity");
                            distanceSlider = GUI.HorizontalSlider(new Rect(halfMenuWidth + 478f, halfMenuHeight + 241f, 150f, 20f), distanceSlider, 0f, 1f);
                            PlayerPrefs.SetFloat("cameraDistance", distanceSlider);
                            IN_GAME_MAIN_CAMERA.CameraDistance = 0.3f + distanceSlider;
                            bool cameraTilt = false;
                            bool invertMouse = false;
                            bool flag28 = false;
                            bool flag29 = false;
                            if ((int)Settings[231] == 1)
                            {
                                flag28 = true;
                            }
                            if ((int)Settings[244] == 1)
                            {
                                flag29 = true;
                            }
                            if (PlayerPrefs.HasKey("cameraTilt"))
                            {
                                if (PlayerPrefs.GetInt("cameraTilt") == 1)
                                {
                                    cameraTilt = true;
                                }
                            }
                            else
                            {
                                PlayerPrefs.SetInt("cameraTilt", 1);
                            }
                            if (PlayerPrefs.HasKey("invertMouseY"))
                            {
                                if (PlayerPrefs.GetInt("invertMouseY") == -1)
                                {
                                    invertMouse = true;
                                }
                            }
                            else
                            {
                                PlayerPrefs.SetInt("invertMouseY", 1);
                            }
                            bool toggleCameraTilt = GUI.Toggle(new Rect(halfMenuWidth + 480f, halfMenuHeight + 261f, 40f, 20f), cameraTilt, "On");
                            if (cameraTilt != toggleCameraTilt)
                            {
                                if (toggleCameraTilt)
                                {
                                    PlayerPrefs.SetInt("cameraTilt", 1);
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("cameraTilt", 0);
                                }
                            }
                            bool toggleInvertMouse = GUI.Toggle(new Rect(halfMenuWidth + 480f, halfMenuHeight + 283f, 40f, 20f), invertMouse, "On");
                            if (toggleInvertMouse != invertMouse)
                            {
                                if (toggleInvertMouse)
                                {
                                    PlayerPrefs.SetInt("invertMouseY", -1);
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("invertMouseY", 1);
                                }
                            }
                            bool flag32 = GUI.Toggle(new Rect(halfMenuWidth + 480f, halfMenuHeight + 375f, 40f, 20f), flag28, "On");
                            if (flag28 != flag32)
                            {
                                if (flag32)
                                {
                                    Settings[231] = 1;
                                }
                                else
                                {
                                    Settings[231] = 0;
                                }
                            }
                            bool flag33 = GUI.Toggle(new Rect(halfMenuWidth + 480f, halfMenuHeight + 397f, 40f, 20f), flag29, "On");
                            if (flag29 != flag33)
                            {
                                if (flag33)
                                {
                                    Settings[244] = 1;
                                }
                                else
                                {
                                    Settings[244] = 0;
                                }
                            }
                            IN_GAME_MAIN_CAMERA.CameraTilt = PlayerPrefs.GetInt("cameraTilt");
                            IN_GAME_MAIN_CAMERA.InvertY = PlayerPrefs.GetInt("invertMouseY");
                        }
                        break;
                    case 1:
                        {
                            if (GUI.Button(new Rect(halfMenuWidth + 233f, halfMenuHeight + 51f, 55f, 25f), "Human"))
                            {
                                Settings[190] = 0;
                            }
                            else if (GUI.Button(new Rect(halfMenuWidth + 293f, halfMenuHeight + 51f, 52f, 25f), "Titan"))
                            {
                                Settings[190] = 1;
                            }
                            else if (GUI.Button(new Rect(halfMenuWidth + 350f, halfMenuHeight + 51f, 53f, 25f), "Horse"))
                            {
                                Settings[190] = 2;
                            }
                            else if (GUI.Button(new Rect(halfMenuWidth + 408f, halfMenuHeight + 51f, 59f, 25f), "Cannon"))
                            {
                                Settings[190] = 3;
                            }
                            if ((int)Settings[190] == 0)
                            {
                                List<string> list6 = new List<string>();
                                list6.Add("Forward:");
                                list6.Add("Backward:");
                                list6.Add("Left:");
                                list6.Add("Right:");
                                list6.Add("Jump:");
                                list6.Add("Dodge:");
                                list6.Add("Left Hook:");
                                list6.Add("Right Hook:");
                                list6.Add("Both Hooks:");
                                list6.Add("Lock:");
                                list6.Add("Attack:");
                                list6.Add("Special:");
                                list6.Add("Salute:");
                                list6.Add("Change Camera:");
                                list6.Add("Reset:");
                                list6.Add("Pause:");
                                list6.Add("Show/Hide Cursor:");
                                list6.Add("Fullscreen:");
                                list6.Add("Change Blade:");
                                list6.Add("Flare Green:");
                                list6.Add("Flare Red:");
                                list6.Add("Flare Black:");
                                list6.Add("Reel in:");
                                list6.Add("Reel out:");
                                list6.Add("Gas Burst:");
                                list6.Add("Minimap Max:");
                                list6.Add("Minimap Toggle:");
                                list6.Add("Minimap Reset:");
                                list6.Add("Open Chat:");
                                list6.Add("Live Spectate");
                                for (int j = 0; j < list6.Count; j++)
                                {
                                    int k = j;
                                    float num31 = 80f;
                                    if (k > 14)
                                    {
                                        num31 = 390f;
                                        k -= 15;
                                    }
                                    GUI.Label(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 145f, 22f), list6[j], "Label");
                                }
                                bool flag36 = false;
                                if ((int)Settings[97] == 1)
                                {
                                    flag36 = true;
                                }
                                bool flag37 = false;
                                if ((int)Settings[116] == 1)
                                {
                                    flag37 = true;
                                }
                                bool flag38 = false;
                                if ((int)Settings[181] == 1)
                                {
                                    flag38 = true;
                                }
                                bool flag39 = GUI.Toggle(new Rect(halfMenuWidth + 457f, halfMenuHeight + 261f, 40f, 20f), flag36, "On");
                                if (flag36 != flag39)
                                {
                                    if (flag39)
                                    {
                                        Settings[97] = 1;
                                    }
                                    else
                                    {
                                        Settings[97] = 0;
                                    }
                                }
                                bool flag40 = GUI.Toggle(new Rect(halfMenuWidth + 457f, halfMenuHeight + 286f, 40f, 20f), flag37, "On");
                                if (flag37 != flag40)
                                {
                                    if (flag40)
                                    {
                                        Settings[116] = 1;
                                    }
                                    else
                                    {
                                        Settings[116] = 0;
                                    }
                                }
                                bool flag41 = GUI.Toggle(new Rect(halfMenuWidth + 457f, halfMenuHeight + 311f, 40f, 20f), flag38, "On");
                                if (flag38 != flag41)
                                {
                                    if (flag41)
                                    {
                                        Settings[181] = 1;
                                    }
                                    else
                                    {
                                        Settings[181] = 0;
                                    }
                                }
                                for (int j = 0; j < 22; j++)
                                {
                                    int k = j;
                                    float num31 = 190f;
                                    if (k > 14)
                                    {
                                        num31 = 500f;
                                        k -= 15;
                                    }
                                    if (GUI.Button(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 120f, 20f), inputManager.getKeyRC(j)))
                                    {
                                        Settings[100] = j + 1;
                                        inputManager.setNameRC(j, "waiting...");
                                    }
                                }
                                if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 261f, 120f, 20f), (string)Settings[98]))
                                {
                                    Settings[98] = "waiting...";
                                    Settings[100] = 98;
                                }
                                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 286f, 120f, 20f), (string)Settings[99]))
                                {
                                    Settings[99] = "waiting...";
                                    Settings[100] = 99;
                                }
                                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 311f, 120f, 20f), (string)Settings[182]))
                                {
                                    Settings[182] = "waiting...";
                                    Settings[100] = 182;
                                }
                                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 336f, 120f, 20f), (string)Settings[232]))
                                {
                                    Settings[232] = "waiting...";
                                    Settings[100] = 232;
                                }
                                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 361f, 120f, 20f), (string)Settings[233]))
                                {
                                    Settings[233] = "waiting...";
                                    Settings[100] = 233;
                                }
                                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 386f, 120f, 20f), (string)Settings[234]))
                                {
                                    Settings[234] = "waiting...";
                                    Settings[100] = 234;
                                }
                                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 411f, 120f, 20f), (string)Settings[236]))
                                {
                                    Settings[236] = "waiting...";
                                    Settings[100] = 236;
                                }
                                else if (GUI.Button(new Rect(halfMenuWidth + 500f, halfMenuHeight + 436f, 120f, 20f), (string)Settings[262]))
                                {
                                    Settings[262] = "waiting...";
                                    Settings[100] = 262;
                                }
                                if ((int)Settings[100] != 0)
                                {
                                    Event current = Event.current;
                                    bool flag3 = false;
                                    string text3 = "waiting...";
                                    if (current.type == EventType.KeyDown && current.keyCode != 0)
                                    {
                                        flag3 = true;
                                        text3 = current.keyCode.ToString();
                                    }
                                    else if (Input.GetKey(KeyCode.LeftShift))
                                    {
                                        flag3 = true;
                                        text3 = KeyCode.LeftShift.ToString();
                                    }
                                    else if (Input.GetKey(KeyCode.RightShift))
                                    {
                                        flag3 = true;
                                        text3 = KeyCode.RightShift.ToString();
                                    }
                                    else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                                    {
                                        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                                        {
                                            flag3 = true;
                                            text3 = "Scroll Up";
                                        }
                                        else
                                        {
                                            flag3 = true;
                                            text3 = "Scroll Down";
                                        }
                                    }
                                    else
                                    {
                                        for (int j = 0; j < 7; j++)
                                        {
                                            if (Input.GetKeyDown((KeyCode)(323 + j)))
                                            {
                                                flag3 = true;
                                                text3 = "Mouse" + Convert.ToString(j);
                                            }
                                        }
                                    }
                                    if (flag3)
                                    {
                                        if ((int)Settings[100] == 98)
                                        {
                                            Settings[98] = text3;
                                            Settings[100] = 0;
                                            InputRC.setInputHuman(InputCodeRC.ReelIn, text3);
                                        }
                                        else if ((int)Settings[100] == 99)
                                        {
                                            Settings[99] = text3;
                                            Settings[100] = 0;
                                            InputRC.setInputHuman(InputCodeRC.ReelOut, text3);
                                        }
                                        else if ((int)Settings[100] == 182)
                                        {
                                            Settings[182] = text3;
                                            Settings[100] = 0;
                                            InputRC.setInputHuman(InputCodeRC.Dash, text3);
                                        }
                                        else if ((int)Settings[100] == 232)
                                        {
                                            Settings[232] = text3;
                                            Settings[100] = 0;
                                            InputRC.setInputHuman(InputCodeRC.MapMaximize, text3);
                                        }
                                        else if ((int)Settings[100] == 233)
                                        {
                                            Settings[233] = text3;
                                            Settings[100] = 0;
                                            InputRC.setInputHuman(InputCodeRC.MapToggle, text3);
                                        }
                                        else if ((int)Settings[100] == 234)
                                        {
                                            Settings[234] = text3;
                                            Settings[100] = 0;
                                            InputRC.setInputHuman(InputCodeRC.MapReset, text3);
                                        }
                                        else if ((int)Settings[100] == 236)
                                        {
                                            Settings[236] = text3;
                                            Settings[100] = 0;
                                            InputRC.setInputHuman(InputCodeRC.Chat, text3);
                                        }
                                        else if ((int)Settings[100] == 262)
                                        {
                                            Settings[262] = text3;
                                            Settings[100] = 0;
                                            InputRC.setInputHuman(InputCodeRC.LiveCamera, text3);
                                        }
                                        else
                                        {
                                            for (int j = 0; j < 22; j++)
                                            {
                                                int num16 = j + 1;
                                                if ((int)Settings[100] == num16)
                                                {
                                                    inputManager.setKeyRC(j, text3);
                                                    Settings[100] = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if ((int)Settings[190] == 1)
                            {
                                List<string> list8 = new List<string>();
                                list8.Add("Forward:");
                                list8.Add("Back:");
                                list8.Add("Left:");
                                list8.Add("Right:");
                                list8.Add("Walk:");
                                list8.Add("Jump:");
                                list8.Add("Punch:");
                                list8.Add("Slam:");
                                list8.Add("Grab (front):");
                                list8.Add("Grab (back):");
                                list8.Add("Grab (nape):");
                                list8.Add("Slap:");
                                list8.Add("Bite:");
                                list8.Add("Cover Nape:");
                                List<string> list7 = list8;
                                for (int j = 0; j < list7.Count; j++)
                                {
                                    int k = j;
                                    float num31 = 80f;
                                    if (k > 6)
                                    {
                                        num31 = 390f;
                                        k -= 7;
                                    }
                                    GUI.Label(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 145f, 22f), list7[j], "Label");
                                }
                                for (int j = 0; j < 14; j++)
                                {
                                    int num16 = 101 + j;
                                    int k = j;
                                    float num31 = 190f;
                                    if (k > 6)
                                    {
                                        num31 = 500f;
                                        k -= 7;
                                    }
                                    if (GUI.Button(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 120f, 20f), (string)Settings[num16]))
                                    {
                                        Settings[num16] = "waiting...";
                                        Settings[100] = num16;
                                    }
                                }
                                if ((int)Settings[100] != 0)
                                {
                                    Event current = Event.current;
                                    bool flag3 = false;
                                    string text3 = "waiting...";
                                    if (current.type == EventType.KeyDown && current.keyCode != 0)
                                    {
                                        flag3 = true;
                                        text3 = current.keyCode.ToString();
                                    }
                                    else if (Input.GetKey(KeyCode.LeftShift))
                                    {
                                        flag3 = true;
                                        text3 = KeyCode.LeftShift.ToString();
                                    }
                                    else if (Input.GetKey(KeyCode.RightShift))
                                    {
                                        flag3 = true;
                                        text3 = KeyCode.RightShift.ToString();
                                    }
                                    else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                                    {
                                        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                                        {
                                            flag3 = true;
                                            text3 = "Scroll Up";
                                        }
                                        else
                                        {
                                            flag3 = true;
                                            text3 = "Scroll Down";
                                        }
                                    }
                                    else
                                    {
                                        for (int j = 0; j < 7; j++)
                                        {
                                            if (Input.GetKeyDown((KeyCode)(323 + j)))
                                            {
                                                flag3 = true;
                                                text3 = "Mouse" + Convert.ToString(j);
                                            }
                                        }
                                    }
                                    if (flag3)
                                    {
                                        for (int j = 0; j < 14; j++)
                                        {
                                            int num16 = 101 + j;
                                            if ((int)Settings[100] == num16)
                                            {
                                                Settings[num16] = text3;
                                                Settings[100] = 0;
                                                InputRC.setInputTitan(j, text3);
                                            }
                                        }
                                    }
                                }
                            }
                            else if ((int)Settings[190] == 2)
                            {
                                List<string> list9 = new List<string>();
                                list9.Add("Forward:");
                                list9.Add("Back:");
                                list9.Add("Left:");
                                list9.Add("Right:");
                                list9.Add("Walk:");
                                list9.Add("Jump:");
                                list9.Add("Mount:");
                                List<string> list7 = list9;
                                for (int j = 0; j < list7.Count; j++)
                                {
                                    int k = j;
                                    float num31 = 80f;
                                    if (k > 3)
                                    {
                                        num31 = 390f;
                                        k -= 4;
                                    }
                                    GUI.Label(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 145f, 22f), list7[j], "Label");
                                }
                                for (int j = 0; j < 7; j++)
                                {
                                    int num16 = 237 + j;
                                    int k = j;
                                    float num31 = 190f;
                                    if (k > 3)
                                    {
                                        num31 = 500f;
                                        k -= 4;
                                    }
                                    if (GUI.Button(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 120f, 20f), (string)Settings[num16]))
                                    {
                                        Settings[num16] = "waiting...";
                                        Settings[100] = num16;
                                    }
                                }
                                if ((int)Settings[100] != 0)
                                {
                                    Event current = Event.current;
                                    bool flag3 = false;
                                    string text3 = "waiting...";
                                    if (current.type == EventType.KeyDown && current.keyCode != 0)
                                    {
                                        flag3 = true;
                                        text3 = current.keyCode.ToString();
                                    }
                                    else if (Input.GetKey(KeyCode.LeftShift))
                                    {
                                        flag3 = true;
                                        text3 = KeyCode.LeftShift.ToString();
                                    }
                                    else if (Input.GetKey(KeyCode.RightShift))
                                    {
                                        flag3 = true;
                                        text3 = KeyCode.RightShift.ToString();
                                    }
                                    else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                                    {
                                        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                                        {
                                            flag3 = true;
                                            text3 = "Scroll Up";
                                        }
                                        else
                                        {
                                            flag3 = true;
                                            text3 = "Scroll Down";
                                        }
                                    }
                                    else
                                    {
                                        for (int j = 0; j < 7; j++)
                                        {
                                            if (Input.GetKeyDown((KeyCode)(323 + j)))
                                            {
                                                flag3 = true;
                                                text3 = "Mouse" + Convert.ToString(j);
                                            }
                                        }
                                    }
                                    if (flag3)
                                    {
                                        for (int j = 0; j < 7; j++)
                                        {
                                            int num16 = 237 + j;
                                            if ((int)Settings[100] == num16)
                                            {
                                                Settings[num16] = text3;
                                                Settings[100] = 0;
                                                InputRC.setInputHorse(j, text3);
                                            }
                                        }
                                    }
                                }
                            }
                            else if ((int)Settings[190] == 3)
                            {
                                List<string> list10 = new List<string>();
                                list10.Add("Rotate Up:");
                                list10.Add("Rotate Down:");
                                list10.Add("Rotate Left:");
                                list10.Add("Rotate Right:");
                                list10.Add("Fire:");
                                list10.Add("Mount:");
                                list10.Add("Slow Rotate:");
                                List<string> list7 = list10;
                                for (int j = 0; j < list7.Count; j++)
                                {
                                    int k = j;
                                    float num31 = 80f;
                                    if (k > 3)
                                    {
                                        num31 = 390f;
                                        k -= 4;
                                    }
                                    GUI.Label(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 145f, 22f), list7[j], "Label");
                                }
                                for (int j = 0; j < 7; j++)
                                {
                                    int num16 = 254 + j;
                                    int k = j;
                                    float num31 = 190f;
                                    if (k > 3)
                                    {
                                        num31 = 500f;
                                        k -= 4;
                                    }
                                    if (GUI.Button(new Rect(halfMenuWidth + num31, halfMenuHeight + 86f + (float)k * 25f, 120f, 20f), (string)Settings[num16]))
                                    {
                                        Settings[num16] = "waiting...";
                                        Settings[100] = num16;
                                    }
                                }
                                if ((int)Settings[100] != 0)
                                {
                                    Event current = Event.current;
                                    bool flag3 = false;
                                    string text3 = "waiting...";
                                    if (current.type == EventType.KeyDown && current.keyCode != 0)
                                    {
                                        flag3 = true;
                                        text3 = current.keyCode.ToString();
                                    }
                                    else if (Input.GetKey(KeyCode.LeftShift))
                                    {
                                        flag3 = true;
                                        text3 = KeyCode.LeftShift.ToString();
                                    }
                                    else if (Input.GetKey(KeyCode.RightShift))
                                    {
                                        flag3 = true;
                                        text3 = KeyCode.RightShift.ToString();
                                    }
                                    else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                                    {
                                        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                                        {
                                            flag3 = true;
                                            text3 = "Scroll Up";
                                        }
                                        else
                                        {
                                            flag3 = true;
                                            text3 = "Scroll Down";
                                        }
                                    }
                                    else
                                    {
                                        for (int j = 0; j < 6; j++)
                                        {
                                            if (Input.GetKeyDown((KeyCode)(323 + j)))
                                            {
                                                flag3 = true;
                                                text3 = "Mouse" + Convert.ToString(j);
                                            }
                                        }
                                    }
                                    if (flag3)
                                    {
                                        for (int j = 0; j < 7; j++)
                                        {
                                            int num16 = 254 + j;
                                            if ((int)Settings[100] == num16)
                                            {
                                                Settings[num16] = text3;
                                                Settings[100] = 0;
                                                InputRC.setInputCannon(j, text3);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case 2:
                        {
                            GUI.Label(new Rect(halfMenuWidth + 205f, halfMenuHeight + 52f, 120f, 30f), "Human Skin Mode:", "Label");
                            bool flag = false;
                            if ((int)Settings[0] == 1)
                            {
                                flag = true;
                            }
                            bool flag4 = GUI.Toggle(new Rect(halfMenuWidth + 325f, halfMenuHeight + 52f, 40f, 20f), flag, "On");
                            if (flag != flag4)
                            {
                                if (flag4)
                                {
                                    Settings[0] = 1;
                                }
                                else
                                {
                                    Settings[0] = 0;
                                }
                            }
                            float num28 = 44f;
                            if ((int)Settings[133] == 0)
                            {
                                if (GUI.Button(new Rect(halfMenuWidth + 375f, halfMenuHeight + 51f, 120f, 22f), "Human Set 1"))
                                {
                                    Settings[133] = 1;
                                }
                                Settings[3] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)Settings[3]);
                                Settings[4] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)Settings[4]);
                                Settings[5] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)Settings[5]);
                                Settings[6] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)Settings[6]);
                                Settings[7] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)Settings[7]);
                                Settings[8] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)Settings[8]);
                                Settings[14] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 6f, 230f, 20f), (string)Settings[14]);
                                Settings[9] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)Settings[9]);
                                Settings[10] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)Settings[10]);
                                Settings[11] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)Settings[11]);
                                Settings[12] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)Settings[12]);
                                Settings[13] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)Settings[13]);
                                Settings[94] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)Settings[94]);
                            }
                            else if ((int)Settings[133] == 1)
                            {
                                if (GUI.Button(new Rect(halfMenuWidth + 375f, halfMenuHeight + 51f, 120f, 22f), "Human Set 2"))
                                {
                                    Settings[133] = 2;
                                }
                                Settings[134] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)Settings[134]);
                                Settings[135] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)Settings[135]);
                                Settings[136] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)Settings[136]);
                                Settings[137] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)Settings[137]);
                                Settings[138] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)Settings[138]);
                                Settings[139] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)Settings[139]);
                                Settings[145] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 6f, 230f, 20f), (string)Settings[145]);
                                Settings[140] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)Settings[140]);
                                Settings[141] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)Settings[141]);
                                Settings[142] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)Settings[142]);
                                Settings[143] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)Settings[143]);
                                Settings[144] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)Settings[144]);
                                Settings[146] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)Settings[146]);
                            }
                            else if ((int)Settings[133] == 2)
                            {
                                if (GUI.Button(new Rect(halfMenuWidth + 375f, halfMenuHeight + 51f, 120f, 22f), "Human Set 3"))
                                {
                                    Settings[133] = 0;
                                }
                                Settings[147] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)Settings[147]);
                                Settings[148] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)Settings[148]);
                                Settings[149] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)Settings[149]);
                                Settings[150] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)Settings[150]);
                                Settings[151] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)Settings[151]);
                                Settings[152] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)Settings[152]);
                                Settings[158] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 114f + num28 * 6f, 230f, 20f), (string)Settings[158]);
                                Settings[153] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 0f, 230f, 20f), (string)Settings[153]);
                                Settings[154] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 1f, 230f, 20f), (string)Settings[154]);
                                Settings[155] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 2f, 230f, 20f), (string)Settings[155]);
                                Settings[156] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 3f, 230f, 20f), (string)Settings[156]);
                                Settings[157] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 4f, 230f, 20f), (string)Settings[157]);
                                Settings[159] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 114f + num28 * 5f, 230f, 20f), (string)Settings[159]);
                            }
                            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 0f, 150f, 20f), "Horse:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 1f, 227f, 20f), "Hair (model dependent):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 2f, 150f, 20f), "Eyes:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 3f, 240f, 20f), "Glass (must have a glass enabled):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 4f, 150f, 20f), "Face:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 5f, 150f, 20f), "Skin:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 6f, 240f, 20f), "Hoodie (costume dependent):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 0f, 240f, 20f), "Costume (model dependent):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 1f, 150f, 20f), "Logo & Cape:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 2f, 240f, 20f), "3DMG Center & 3DMG/Blade/Gun(left):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 3f, 227f, 20f), "3DMG/Blade/Gun(right):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 4f, 150f, 20f), "Gas:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 5f, 150f, 20f), "Weapon Trail:", "Label");
                        }
                        break;
                    case 3:
                        {
                            GUI.Label(new Rect(halfMenuWidth + 270f, halfMenuHeight + 52f, 120f, 30f), "Titan Skin Mode:", "Label");
                            bool flag5 = false;
                            if ((int)Settings[1] == 1)
                            {
                                flag5 = true;
                            }
                            bool flag10 = GUI.Toggle(new Rect(halfMenuWidth + 390f, halfMenuHeight + 52f, 40f, 20f), flag5, "On");
                            if (flag5 != flag10)
                            {
                                if (flag10)
                                {
                                    Settings[1] = 1;
                                }
                                else
                                {
                                    Settings[1] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 270f, halfMenuHeight + 77f, 120f, 30f), "Randomized Pairs:", "Label");
                            flag5 = false;
                            if ((int)Settings[32] == 1)
                            {
                                flag5 = true;
                            }
                            flag10 = GUI.Toggle(new Rect(halfMenuWidth + 390f, halfMenuHeight + 77f, 40f, 20f), flag5, "On");
                            if (flag5 != flag10)
                            {
                                if (flag10)
                                {
                                    Settings[32] = 1;
                                }
                                else
                                {
                                    Settings[32] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 158f, halfMenuHeight + 112f, 150f, 20f), "Titan Hair:", "Label");
                            Settings[21] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 134f, 165f, 20f), (string)Settings[21]);
                            Settings[22] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 156f, 165f, 20f), (string)Settings[22]);
                            Settings[23] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 178f, 165f, 20f), (string)Settings[23]);
                            Settings[24] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 200f, 165f, 20f), (string)Settings[24]);
                            Settings[25] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 222f, 165f, 20f), (string)Settings[25]);
                            if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 134f, 60f, 20f), HairType((int)Settings[16])))
                            {
                                int num29 = 16;
                                int num30 = (int)Settings[num29];
                                num30 = ((num30 < 9) ? (num30 + 1) : (-1));
                                Settings[num29] = num30;
                            }
                            else if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 156f, 60f, 20f), HairType((int)Settings[17])))
                            {
                                int num29 = 17;
                                int num30 = (int)Settings[num29];
                                num30 = ((num30 < 9) ? (num30 + 1) : (-1));
                                Settings[num29] = num30;
                            }
                            else if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 178f, 60f, 20f), HairType((int)Settings[18])))
                            {
                                int num29 = 18;
                                int num30 = (int)Settings[num29];
                                num30 = ((num30 < 9) ? (num30 + 1) : (-1));
                                Settings[num29] = num30;
                            }
                            else if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 200f, 60f, 20f), HairType((int)Settings[19])))
                            {
                                int num29 = 19;
                                int num30 = (int)Settings[num29];
                                num30 = ((num30 < 9) ? (num30 + 1) : (-1));
                                Settings[num29] = num30;
                            }
                            else if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 222f, 60f, 20f), HairType((int)Settings[20])))
                            {
                                int num29 = 20;
                                int num30 = (int)Settings[num29];
                                num30 = ((num30 < 9) ? (num30 + 1) : (-1));
                                Settings[num29] = num30;
                            }
                            GUI.Label(new Rect(halfMenuWidth + 158f, halfMenuHeight + 252f, 150f, 20f), "Titan Eye:", "Label");
                            Settings[26] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 274f, 230f, 20f), (string)Settings[26]);
                            Settings[27] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 296f, 230f, 20f), (string)Settings[27]);
                            Settings[28] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 318f, 230f, 20f), (string)Settings[28]);
                            Settings[29] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 340f, 230f, 20f), (string)Settings[29]);
                            Settings[30] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 362f, 230f, 20f), (string)Settings[30]);
                            GUI.Label(new Rect(halfMenuWidth + 455f, halfMenuHeight + 112f, 150f, 20f), "Titan Body:", "Label");
                            Settings[86] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 134f, 230f, 20f), (string)Settings[86]);
                            Settings[87] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 156f, 230f, 20f), (string)Settings[87]);
                            Settings[88] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 178f, 230f, 20f), (string)Settings[88]);
                            Settings[89] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 200f, 230f, 20f), (string)Settings[89]);
                            Settings[90] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 222f, 230f, 20f), (string)Settings[90]);
                            GUI.Label(new Rect(halfMenuWidth + 472f, halfMenuHeight + 252f, 150f, 20f), "Eren:", "Label");
                            Settings[65] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 274f, 230f, 20f), (string)Settings[65]);
                            GUI.Label(new Rect(halfMenuWidth + 470f, halfMenuHeight + 296f, 150f, 20f), "Annie:", "Label");
                            Settings[66] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 318f, 230f, 20f), (string)Settings[66]);
                            GUI.Label(new Rect(halfMenuWidth + 465f, halfMenuHeight + 340f, 150f, 20f), "Colossal:", "Label");
                            Settings[67] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 362f, 230f, 20f), (string)Settings[67]);
                        }
                        break;
                    case 4:
                        GUI.TextArea(new Rect(halfMenuWidth + 80f, halfMenuHeight + 57f, 270f, 30f), "Settings saved to PlayerPrefs!", 100, "Label");
                        break;
                    case 5:
                        GUI.TextArea(new Rect(halfMenuWidth + 80f, halfMenuHeight + 57f, 270f, 30f), "Settings reloaded from PlayerPrefs!", 100, "Label");
                        break;
                    case 6:
                        // ?
                        break;
                    case 7:
                        {
                            float num28 = 22f;
                            GUI.Label(new Rect(halfMenuWidth + 205f, halfMenuHeight + 52f, 145f, 30f), "Level Skin Mode:", "Label");
                            bool flag11 = false;
                            if ((int)Settings[2] == 1)
                            {
                                flag11 = true;
                            }
                            bool flag12 = GUI.Toggle(new Rect(halfMenuWidth + 325f, halfMenuHeight + 52f, 40f, 20f), flag11, "On");
                            if (flag11 != flag12)
                            {
                                if (flag12)
                                {
                                    Settings[2] = 1;
                                }
                                else
                                {
                                    Settings[2] = 0;
                                }
                            }
                            if ((int)Settings[188] == 0)
                            {
                                if (GUI.Button(new Rect(halfMenuWidth + 375f, halfMenuHeight + 51f, 120f, 22f), "Forest Skins"))
                                {
                                    Settings[188] = 1;
                                }
                                GUI.Label(new Rect(halfMenuWidth + 205f, halfMenuHeight + 77f, 145f, 30f), "Randomized Pairs:", "Label");
                                flag11 = false;
                                if ((int)Settings[50] == 1)
                                {
                                    flag11 = true;
                                }
                                flag12 = GUI.Toggle(new Rect(halfMenuWidth + 325f, halfMenuHeight + 77f, 40f, 20f), flag11, "On");
                                if (flag11 != flag12)
                                {
                                    if (flag12)
                                    {
                                        Settings[50] = 1;
                                    }
                                    else
                                    {
                                        Settings[50] = 0;
                                    }
                                }
                                scroll = GUI.BeginScrollView(new Rect(halfMenuWidth, halfMenuHeight + 115f, 700f, 340f), scroll, new Rect(halfMenuWidth, halfMenuHeight + 115f, 700f, 475f));
                                GUI.Label(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 0f, 150f, 20f), "Ground:", "Label");
                                Settings[49] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 1f, 227f, 20f), (string)Settings[49]);
                                GUI.Label(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 2f, 150f, 20f), "Forest Trunks", "Label");
                                Settings[33] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 3f, 227f, 20f), (string)Settings[33]);
                                Settings[34] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 4f, 227f, 20f), (string)Settings[34]);
                                Settings[35] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 5f, 227f, 20f), (string)Settings[35]);
                                Settings[36] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 6f, 227f, 20f), (string)Settings[36]);
                                Settings[37] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 7f, 227f, 20f), (string)Settings[37]);
                                Settings[38] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 8f, 227f, 20f), (string)Settings[38]);
                                Settings[39] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 9f, 227f, 20f), (string)Settings[39]);
                                Settings[40] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 10f, 227f, 20f), (string)Settings[40]);
                                GUI.Label(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 11f, 150f, 20f), "Forest Leaves:", "Label");
                                Settings[41] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 12f, 227f, 20f), (string)Settings[41]);
                                Settings[42] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 13f, 227f, 20f), (string)Settings[42]);
                                Settings[43] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 14f, 227f, 20f), (string)Settings[43]);
                                Settings[44] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 15f, 227f, 20f), (string)Settings[44]);
                                Settings[45] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 16f, 227f, 20f), (string)Settings[45]);
                                Settings[46] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 17f, 227f, 20f), (string)Settings[46]);
                                Settings[47] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 18f, 227f, 20f), (string)Settings[47]);
                                Settings[48] = GUI.TextField(new Rect(halfMenuWidth + 79f, halfMenuHeight + 117f + num28 * 19f, 227f, 20f), (string)Settings[48]);
                                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 0f, 150f, 20f), "Skybox Front:", "Label");
                                Settings[163] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 1f, 227f, 20f), (string)Settings[163]);
                                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 2f, 150f, 20f), "Skybox Back:", "Label");
                                Settings[164] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 3f, 227f, 20f), (string)Settings[164]);
                                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 4f, 150f, 20f), "Skybox Left:", "Label");
                                Settings[165] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 5f, 227f, 20f), (string)Settings[165]);
                                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 6f, 150f, 20f), "Skybox Right:", "Label");
                                Settings[166] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 7f, 227f, 20f), (string)Settings[166]);
                                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 8f, 150f, 20f), "Skybox Up:", "Label");
                                Settings[167] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 9f, 227f, 20f), (string)Settings[167]);
                                GUI.Label(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 10f, 150f, 20f), "Skybox Down:", "Label");
                                Settings[168] = GUI.TextField(new Rect(halfMenuWidth + 379f, halfMenuHeight + 117f + num28 * 11f, 227f, 20f), (string)Settings[168]);
                                GUI.EndScrollView();
                            }
                            else if ((int)Settings[188] == 1)
                            {
                                if (GUI.Button(new Rect(halfMenuWidth + 375f, halfMenuHeight + 51f, 120f, 22f), "City Skins"))
                                {
                                    Settings[188] = 0;
                                }
                                GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 0f, 150f, 20f), "Ground:", "Label");
                                Settings[59] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 1f, 230f, 20f), (string)Settings[59]);
                                GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 2f, 150f, 20f), "Wall:", "Label");
                                Settings[60] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 3f, 230f, 20f), (string)Settings[60]);
                                GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 4f, 150f, 20f), "Gate:", "Label");
                                Settings[61] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 5f, 230f, 20f), (string)Settings[61]);
                                GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 6f, 150f, 20f), "Houses:", "Label");
                                Settings[51] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 7f, 230f, 20f), (string)Settings[51]);
                                Settings[52] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 8f, 230f, 20f), (string)Settings[52]);
                                Settings[53] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 9f, 230f, 20f), (string)Settings[53]);
                                Settings[54] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 10f, 230f, 20f), (string)Settings[54]);
                                Settings[55] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 11f, 230f, 20f), (string)Settings[55]);
                                Settings[56] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 12f, 230f, 20f), (string)Settings[56]);
                                Settings[57] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 13f, 230f, 20f), (string)Settings[57]);
                                Settings[58] = GUI.TextField(new Rect(halfMenuWidth + 80f, halfMenuHeight + 92f + num28 * 14f, 230f, 20f), (string)Settings[58]);
                                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 0f, 150f, 20f), "Skybox Front:", "Label");
                                Settings[169] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 1f, 230f, 20f), (string)Settings[169]);
                                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 2f, 150f, 20f), "Skybox Back:", "Label");
                                Settings[170] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 3f, 230f, 20f), (string)Settings[170]);
                                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 4f, 150f, 20f), "Skybox Left:", "Label");
                                Settings[171] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 5f, 230f, 20f), (string)Settings[171]);
                                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 6f, 150f, 20f), "Skybox Right:", "Label");
                                Settings[172] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 7f, 230f, 20f), (string)Settings[172]);
                                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 8f, 150f, 20f), "Skybox Up:", "Label");
                                Settings[173] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 9f, 230f, 20f), (string)Settings[173]);
                                GUI.Label(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 10f, 150f, 20f), "Skybox Down:", "Label");
                                Settings[174] = GUI.TextField(new Rect(halfMenuWidth + 390f, halfMenuHeight + 92f + num28 * 11f, 230f, 20f), (string)Settings[174]);
                            }
                        }
                        break;
                    case 8:
                        {
                            GUI.Label(new Rect(halfMenuWidth + 150f, halfMenuHeight + 51f, 120f, 22f), "Map Settings", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 50f, halfMenuHeight + 81f, 140f, 20f), "Titan Spawn Cap:", "Label");
                            Settings[85] = GUI.TextField(new Rect(halfMenuWidth + 155f, halfMenuHeight + 81f, 30f, 20f), (string)Settings[85]);
                            string[] texts = new string[5]
                            {
                                "1 Round",
                                "Waves",
                                "PVP",
                                "Racing",
                                "Custom"
                            };
                            RCSettings.GameType = GUI.SelectionGrid(new Rect(halfMenuWidth + 190f, halfMenuHeight + 80f, 140f, 60f), RCSettings.GameType, texts, 2, GUI.skin.toggle);
                            GUI.Label(new Rect(halfMenuWidth + 150f, halfMenuHeight + 155f, 150f, 20f), "Level Script:", "Label");
                            CurrentScript = GUI.TextField(new Rect(halfMenuWidth + 50f, halfMenuHeight + 180f, 275f, 220f), CurrentScript);
                            if (GUI.Button(new Rect(halfMenuWidth + 100f, halfMenuHeight + 410f, 50f, 25f), "Copy"))
                            {
                                TextEditor textEditor = new TextEditor();
                                textEditor.content = new GUIContent(CurrentScript);
                                textEditor.SelectAll();
                                textEditor.Copy();
                            }
                            else if (GUI.Button(new Rect(halfMenuWidth + 225f, halfMenuHeight + 410f, 50f, 25f), "Clear"))
                            {
                                CurrentScript = string.Empty;
                            }
                            GUI.Label(new Rect(halfMenuWidth + 455f, halfMenuHeight + 51f, 180f, 20f), "Custom Textures", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 81f, 180f, 20f), "Ground Skin:", "Label");
                            Settings[162] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 103f, 275f, 20f), (string)Settings[162]);
                            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 125f, 150f, 20f), "Skybox Front:", "Label");
                            Settings[175] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 147f, 275f, 20f), (string)Settings[175]);
                            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 169f, 150f, 20f), "Skybox Back:", "Label");
                            Settings[176] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 191f, 275f, 20f), (string)Settings[176]);
                            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 213f, 150f, 20f), "Skybox Left:", "Label");
                            Settings[177] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 235f, 275f, 20f), (string)Settings[177]);
                            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 257f, 150f, 20f), "Skybox Right:", "Label");
                            Settings[178] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 279f, 275f, 20f), (string)Settings[178]);
                            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 301f, 150f, 20f), "Skybox Up:", "Label");
                            Settings[179] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 323f, 275f, 20f), (string)Settings[179]);
                            GUI.Label(new Rect(halfMenuWidth + 375f, halfMenuHeight + 345f, 150f, 20f), "Skybox Down:", "Label");
                            Settings[180] = GUI.TextField(new Rect(halfMenuWidth + 375f, halfMenuHeight + 367f, 275f, 20f), (string)Settings[180]);
                        }
                        break;
                    case 9:
                        CurrentScriptLogic = GUI.TextField(new Rect(halfMenuWidth + 50f, halfMenuHeight + 82f, 600f, 270f), CurrentScriptLogic);
                        if (GUI.Button(new Rect(halfMenuWidth + 250f, halfMenuHeight + 365f, 50f, 20f), "Copy"))
                        {
                            TextEditor textEditor = new TextEditor();
                            textEditor.content = new GUIContent(CurrentScriptLogic);
                            textEditor.SelectAll();
                            textEditor.Copy();
                        }
                        else if (GUI.Button(new Rect(halfMenuWidth + 400f, halfMenuHeight + 365f, 50f, 20f), "Clear"))
                        {
                            CurrentScriptLogic = string.Empty;
                        }
                        break;
                    case 10:
                        GUI.Label(new Rect(halfMenuWidth + 200f, halfMenuHeight + 382f, 400f, 22f), "Master Client only. Changes will take effect upon restart.");
                        if (GUI.Button(new Rect(halfMenuWidth + 267.5f, halfMenuHeight + 50f, 60f, 25f), "Titans"))
                        {
                            Settings[230] = 0;
                        }
                        else if (GUI.Button(new Rect(halfMenuWidth + 332.5f, halfMenuHeight + 50f, 40f, 25f), "PVP"))
                        {
                            Settings[230] = 1;
                        }
                        else if (GUI.Button(new Rect(halfMenuWidth + 377.5f, halfMenuHeight + 50f, 50f, 25f), "Misc"))
                        {
                            Settings[230] = 2;
                        }
                        else if (GUI.Button(new Rect(halfMenuWidth + 320f, halfMenuHeight + 415f, 60f, 30f), "Reset"))
                        {
                            Settings[192] = 0;
                            Settings[193] = 0;
                            Settings[194] = 0;
                            Settings[195] = 0;
                            Settings[196] = "30";
                            Settings[197] = 0;
                            Settings[198] = "100";
                            Settings[199] = "200";
                            Settings[200] = 0;
                            Settings[201] = "1";
                            Settings[202] = 0;
                            Settings[203] = 0;
                            Settings[204] = "1";
                            Settings[205] = 0;
                            Settings[206] = "1000";
                            Settings[207] = 0;
                            Settings[208] = "1.0";
                            Settings[209] = "3.0";
                            Settings[210] = 0;
                            Settings[211] = "20.0";
                            Settings[212] = "20.0";
                            Settings[213] = "20.0";
                            Settings[214] = "20.0";
                            Settings[215] = "20.0";
                            Settings[216] = 0;
                            Settings[217] = 0;
                            Settings[218] = "1";
                            Settings[219] = 0;
                            Settings[220] = 0;
                            Settings[221] = 0;
                            Settings[222] = "20";
                            Settings[223] = 0;
                            Settings[224] = "10";
                            Settings[225] = string.Empty;
                            Settings[226] = 0;
                            Settings[227] = "50";
                            Settings[228] = 0;
                            Settings[229] = 0;
                            Settings[235] = 0;
                        }
                        if ((int)Settings[230] == 0)
                        {
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 90f, 160f, 22f), "Custom Titan Number:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 112f, 200f, 22f), "Amount (Integer):", "Label");
                            Settings[204] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 112f, 50f, 22f), (string)Settings[204]);
                            bool flag34 = false;
                            if ((int)Settings[203] == 1)
                            {
                                flag34 = true;
                            }
                            bool flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 90f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[203] = 1;
                                }
                                else
                                {
                                    Settings[203] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 152f, 160f, 22f), "Custom Titan Spawns:", "Label");
                            flag34 = false;
                            if ((int)Settings[210] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 152f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[210] = 1;
                                }
                                else
                                {
                                    Settings[210] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 174f, 150f, 22f), "Normal (Decimal):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 196f, 150f, 22f), "Aberrant (Decimal):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 218f, 150f, 22f), "Jumper (Decimal):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 240f, 150f, 22f), "Crawler (Decimal):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 262f, 150f, 22f), "Punk (Decimal):", "Label");
                            Settings[211] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 174f, 50f, 22f), (string)Settings[211]);
                            Settings[212] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 196f, 50f, 22f), (string)Settings[212]);
                            Settings[213] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 218f, 50f, 22f), (string)Settings[213]);
                            Settings[214] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 240f, 50f, 22f), (string)Settings[214]);
                            Settings[215] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 262f, 50f, 22f), (string)Settings[215]);
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 302f, 160f, 22f), "Titan Size Mode:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 324f, 150f, 22f), "Minimum (Decimal):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 346f, 150f, 22f), "Maximum (Decimal):", "Label");
                            Settings[208] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 324f, 50f, 22f), (string)Settings[208]);
                            Settings[209] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 346f, 50f, 22f), (string)Settings[209]);
                            flag34 = false;
                            if ((int)Settings[207] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 302f, 40f, 20f), flag34, "On");
                            if (flag35 != flag34)
                            {
                                if (flag35)
                                {
                                    Settings[207] = 1;
                                }
                                else
                                {
                                    Settings[207] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 90f, 160f, 22f), "Titan Health Mode:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 161f, 150f, 22f), "Minimum (Integer):", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 183f, 150f, 22f), "Maximum (Integer):", "Label");
                            Settings[198] = GUI.TextField(new Rect(halfMenuWidth + 550f, halfMenuHeight + 161f, 50f, 22f), (string)Settings[198]);
                            Settings[199] = GUI.TextField(new Rect(halfMenuWidth + 550f, halfMenuHeight + 183f, 50f, 22f), (string)Settings[199]);
                            string[] texts = new string[3]
                            {
                            "Off",
                            "Fixed",
                            "Scaled"
                            };
                            Settings[197] = GUI.SelectionGrid(new Rect(halfMenuWidth + 550f, halfMenuHeight + 90f, 100f, 66f), (int)Settings[197], texts, 1, GUI.skin.toggle);
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 223f, 160f, 22f), "Titan Damage Mode:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 245f, 150f, 22f), "Damage (Integer):", "Label");
                            Settings[206] = GUI.TextField(new Rect(halfMenuWidth + 550f, halfMenuHeight + 245f, 50f, 22f), (string)Settings[206]);
                            flag34 = false;
                            if ((int)Settings[205] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 223f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[205] = 1;
                                }
                                else
                                {
                                    Settings[205] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 285f, 160f, 22f), "Titan Explode Mode:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 307f, 160f, 22f), "Radius (Integer):", "Label");
                            Settings[196] = GUI.TextField(new Rect(halfMenuWidth + 550f, halfMenuHeight + 307f, 50f, 22f), (string)Settings[196]);
                            flag34 = false;
                            if ((int)Settings[195] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 285f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[195] = 1;
                                }
                                else
                                {
                                    Settings[195] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 347f, 160f, 22f), "Disable Rock Throwing:", "Label");
                            flag34 = false;
                            if ((int)Settings[194] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 347f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[194] = 1;
                                }
                                else
                                {
                                    Settings[194] = 0;
                                }
                            }
                        }
                        else if ((int)Settings[230] == 1)
                        {
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 90f, 160f, 22f), "Point Mode:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 112f, 160f, 22f), "Max Points (Integer):", "Label");
                            Settings[227] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 112f, 50f, 22f), (string)Settings[227]);
                            bool flag34 = false;
                            if ((int)Settings[226] == 1)
                            {
                                flag34 = true;
                            }
                            bool flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 90f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[226] = 1;
                                }
                                else
                                {
                                    Settings[226] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 152f, 160f, 22f), "PVP Bomb Mode:", "Label");
                            flag34 = false;
                            if ((int)Settings[192] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 152f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[192] = 1;
                                }
                                else
                                {
                                    Settings[192] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 182f, 100f, 66f), "Team Mode:", "Label");
                            string[] texts = new string[4]
                            {
                            "Off",
                            "No Sort",
                            "Size-Lock",
                            "Skill-Lock"
                            };
                            Settings[193] = GUI.SelectionGrid(new Rect(halfMenuWidth + 250f, halfMenuHeight + 182f, 120f, 88f), (int)Settings[193], texts, 1, GUI.skin.toggle);
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 278f, 160f, 22f), "Infection Mode:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 300f, 160f, 22f), "Starting Titans (Integer):", "Label");
                            Settings[201] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 300f, 50f, 22f), (string)Settings[201]);
                            flag34 = false;
                            if ((int)Settings[200] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 278f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[200] = 1;
                                }
                                else
                                {
                                    Settings[200] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 330f, 160f, 22f), "Friendly Mode:", "Label");
                            flag34 = false;
                            if ((int)Settings[219] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 330f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[219] = 1;
                                }
                                else
                                {
                                    Settings[219] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 90f, 160f, 22f), "Sword/AHSS PVP:", "Label");
                            texts = new string[3]
                            {
                            "Off",
                            "Teams",
                            "FFA"
                            };
                            Settings[220] = GUI.SelectionGrid(new Rect(halfMenuWidth + 550f, halfMenuHeight + 90f, 100f, 66f), (int)Settings[220], texts, 1, GUI.skin.toggle);
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 164f, 160f, 22f), "No AHSS Air-Reloading:", "Label");
                            flag34 = false;
                            if ((int)Settings[228] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 164f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[228] = 1;
                                }
                                else
                                {
                                    Settings[228] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 194f, 160f, 22f), "Cannons kill humans:", "Label");
                            flag34 = false;
                            if ((int)Settings[261] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 194f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[261] = 1;
                                }
                                else
                                {
                                    Settings[261] = 0;
                                }
                            }
                        }
                        else if ((int)Settings[230] == 2)
                        {
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 90f, 160f, 22f), "Custom Titans/Wave:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 112f, 160f, 22f), "Amount (Integer):", "Label");
                            Settings[218] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 112f, 50f, 22f), (string)Settings[218]);
                            bool flag34 = false;
                            if ((int)Settings[217] == 1)
                            {
                                flag34 = true;
                            }
                            bool flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 90f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[217] = 1;
                                }
                                else
                                {
                                    Settings[217] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 152f, 160f, 22f), "Maximum Waves:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 174f, 160f, 22f), "Amount (Integer):", "Label");
                            Settings[222] = GUI.TextField(new Rect(halfMenuWidth + 250f, halfMenuHeight + 174f, 50f, 22f), (string)Settings[222]);
                            flag34 = false;
                            if ((int)Settings[221] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 152f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[221] = 1;
                                }
                                else
                                {
                                    Settings[221] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 214f, 160f, 22f), "Punks every 5 waves:", "Label");
                            flag34 = false;
                            if ((int)Settings[229] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 214f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[229] = 1;
                                }
                                else
                                {
                                    Settings[229] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 100f, halfMenuHeight + 244f, 160f, 22f), "Global Minimap Disable:", "Label");
                            flag34 = false;
                            if ((int)Settings[235] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 250f, halfMenuHeight + 244f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[235] = 1;
                                }
                                else
                                {
                                    Settings[235] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 90f, 160f, 22f), "Endless Respawn:", "Label");
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 112f, 160f, 22f), "Respawn Time (Integer):", "Label");
                            Settings[224] = GUI.TextField(new Rect(halfMenuWidth + 550f, halfMenuHeight + 112f, 50f, 22f), (string)Settings[224]);
                            flag34 = false;
                            if ((int)Settings[223] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 90f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[223] = 1;
                                }
                                else
                                {
                                    Settings[223] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 152f, 160f, 22f), "Kick Eren Titan:", "Label");
                            flag34 = false;
                            if ((int)Settings[202] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 152f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[202] = 1;
                                }
                                else
                                {
                                    Settings[202] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 182f, 160f, 22f), "Allow Horses:", "Label");
                            flag34 = false;
                            if ((int)Settings[216] == 1)
                            {
                                flag34 = true;
                            }
                            flag35 = GUI.Toggle(new Rect(halfMenuWidth + 550f, halfMenuHeight + 182f, 40f, 20f), flag34, "On");
                            if (flag34 != flag35)
                            {
                                if (flag35)
                                {
                                    Settings[216] = 1;
                                }
                                else
                                {
                                    Settings[216] = 0;
                                }
                            }
                            GUI.Label(new Rect(halfMenuWidth + 400f, halfMenuHeight + 212f, 180f, 22f), "Message of the Day:", "Label");
                            Settings[225] = GUI.TextArea(new Rect(halfMenuWidth + 400f, halfMenuHeight + 234f, 200f, 100f), (string)Settings[225]);
                        }
                        break;
                    case 11:
                        GUI.Label(new Rect(halfMenuWidth + 150f, halfMenuHeight + 80f, 185f, 22f), "Bomb Mode", "Label");
                        GUI.Label(new Rect(halfMenuWidth + 80f, halfMenuHeight + 110f, 80f, 22f), "Color:", "Label");
                        Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
                        texture2D.SetPixel(0, 0, new Color((float)Settings[246], (float)Settings[247], (float)Settings[248], (float)Settings[249]));
                        texture2D.Apply();
                        GUI.DrawTexture(new Rect(halfMenuWidth + 120f, halfMenuHeight + 113f, 40f, 15f), texture2D, ScaleMode.StretchToFill);
                        UnityEngine.Object.Destroy(texture2D);
                        GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 135f, 20f, 22f), "R:", "Label");
                        GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 160f, 20f, 22f), "G:", "Label");
                        GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 185f, 20f, 22f), "B:", "Label");
                        GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 210f, 20f, 22f), "A:", "Label");
                        Settings[246] = GUI.HorizontalSlider(new Rect(halfMenuWidth + 92f, halfMenuHeight + 138f, 100f, 20f), (float)Settings[246], 0f, 1f);
                        Settings[247] = GUI.HorizontalSlider(new Rect(halfMenuWidth + 92f, halfMenuHeight + 163f, 100f, 20f), (float)Settings[247], 0f, 1f);
                        Settings[248] = GUI.HorizontalSlider(new Rect(halfMenuWidth + 92f, halfMenuHeight + 188f, 100f, 20f), (float)Settings[248], 0f, 1f);
                        Settings[249] = GUI.HorizontalSlider(new Rect(halfMenuWidth + 92f, halfMenuHeight + 213f, 100f, 20f), (float)Settings[249], 0.5f, 1f);
                        GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 235f, 95f, 22f), "Bomb Radius:", "Label");
                        GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 260f, 95f, 22f), "Bomb Range:", "Label");
                        GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 285f, 95f, 22f), "Bomb Speed:", "Label");
                        GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 310f, 95f, 22f), "Bomb CD:", "Label");
                        GUI.Label(new Rect(halfMenuWidth + 72f, halfMenuHeight + 335f, 95f, 22f), "Unused Points:", "Label");
                        GUI.Label(new Rect(halfMenuWidth + 168f, halfMenuHeight + 235f, 20f, 22f), ((float)Settings[250]).ToString(), "Label");
                        GUI.Label(new Rect(halfMenuWidth + 168f, halfMenuHeight + 260f, 20f, 22f), ((float)Settings[251]).ToString(), "Label");
                        GUI.Label(new Rect(halfMenuWidth + 168f, halfMenuHeight + 285f, 20f, 22f), ((float)Settings[252]).ToString(), "Label");
                        GUI.Label(new Rect(halfMenuWidth + 168f, halfMenuHeight + 310f, 20f, 22f), ((float)Settings[253]).ToString(), "Label");
                        float unusedBombPoints = 20 - (float)Settings[250] - (float)Settings[251] - (float)Settings[252] - (float)Settings[253];
                        GUI.Label(new Rect(halfMenuWidth + 168f, halfMenuHeight + 335f, 20f, 22f), unusedBombPoints.ToString(), "Label");
                        if (GUI.Button(new Rect(halfMenuWidth + 190f, halfMenuHeight + 235f, 20f, 20f), "-"))
                        {
                            if ((float)Settings[250] > 0)
                            {
                                Settings[250] = (float)Settings[250] - 0.5f;
                            }
                        }
                        else if (GUI.Button(new Rect(halfMenuWidth + 215f, halfMenuHeight + 235f, 20f, 20f), "+") && (float)Settings[250] < 10 && unusedBombPoints > 0)
                        {
                            Settings[250] = (float)Settings[250] + 0.5f;
                        }
                        if (GUI.Button(new Rect(halfMenuWidth + 190f, halfMenuHeight + 260f, 20f, 20f), "-"))
                        {
                            if ((float)Settings[251] > 0)
                            {
                                Settings[251] = (float)Settings[251] - 0.5f;
                            }
                        }
                        else if (GUI.Button(new Rect(halfMenuWidth + 215f, halfMenuHeight + 260f, 20f, 20f), "+") && (float)Settings[251] < 10 && unusedBombPoints > 0)
                        {
                            Settings[251] = (float)Settings[251] + 0.5f;
                        }
                        if (GUI.Button(new Rect(halfMenuWidth + 190f, halfMenuHeight + 285f, 20f, 20f), "-"))
                        {
                            if ((float)Settings[252] > 0)
                            {
                                Settings[252] = (float)Settings[252] - 0.5f;
                            }
                        }
                        else if (GUI.Button(new Rect(halfMenuWidth + 215f, halfMenuHeight + 285f, 20f, 20f), "+") && (float)Settings[252] < 10 && unusedBombPoints > 0)
                        {
                            Settings[252] = (float)Settings[252] + 0.5f;
                        }
                        if (GUI.Button(new Rect(halfMenuWidth + 190f, halfMenuHeight + 310f, 20f, 20f), "-"))
                        {
                            if ((float)Settings[253] > 0)
                            {
                                Settings[253] = (float)Settings[253] - 0.5f;
                            }
                        }
                        else if (GUI.Button(new Rect(halfMenuWidth + 215f, halfMenuHeight + 310f, 20f, 20f), "+") && (float)Settings[253] < 10 && unusedBombPoints > 0)
                        {
                            Settings[253] = (float)Settings[253] + 0.5f;
                        }
                        break;
                }
                if (GUI.Button(new Rect(halfMenuWidth + 416f, halfMenuHeight + 468f, 42f, 25f), "Save"))
                {
                    PlayerPrefs.SetInt("human", (int)Settings[0]);
                    PlayerPrefs.SetInt("titan", (int)Settings[1]);
                    PlayerPrefs.SetInt("level", (int)Settings[2]);
                    PlayerPrefs.SetString("horse", (string)Settings[3]);
                    PlayerPrefs.SetString("hair", (string)Settings[4]);
                    PlayerPrefs.SetString("eye", (string)Settings[5]);
                    PlayerPrefs.SetString("glass", (string)Settings[6]);
                    PlayerPrefs.SetString("face", (string)Settings[7]);
                    PlayerPrefs.SetString("skin", (string)Settings[8]);
                    PlayerPrefs.SetString("costume", (string)Settings[9]);
                    PlayerPrefs.SetString("logo", (string)Settings[10]);
                    PlayerPrefs.SetString("bladel", (string)Settings[11]);
                    PlayerPrefs.SetString("blader", (string)Settings[12]);
                    PlayerPrefs.SetString("gas", (string)Settings[13]);
                    PlayerPrefs.SetString("haircolor", (string)Settings[14]);
                    PlayerPrefs.SetInt("gasenable", (int)Settings[15]);
                    PlayerPrefs.SetInt("titantype1", (int)Settings[16]);
                    PlayerPrefs.SetInt("titantype2", (int)Settings[17]);
                    PlayerPrefs.SetInt("titantype3", (int)Settings[18]);
                    PlayerPrefs.SetInt("titantype4", (int)Settings[19]);
                    PlayerPrefs.SetInt("titantype5", (int)Settings[20]);
                    PlayerPrefs.SetString("titanhair1", (string)Settings[21]);
                    PlayerPrefs.SetString("titanhair2", (string)Settings[22]);
                    PlayerPrefs.SetString("titanhair3", (string)Settings[23]);
                    PlayerPrefs.SetString("titanhair4", (string)Settings[24]);
                    PlayerPrefs.SetString("titanhair5", (string)Settings[25]);
                    PlayerPrefs.SetString("titaneye1", (string)Settings[26]);
                    PlayerPrefs.SetString("titaneye2", (string)Settings[27]);
                    PlayerPrefs.SetString("titaneye3", (string)Settings[28]);
                    PlayerPrefs.SetString("titaneye4", (string)Settings[29]);
                    PlayerPrefs.SetString("titaneye5", (string)Settings[30]);
                    PlayerPrefs.SetInt("titanR", (int)Settings[32]);
                    PlayerPrefs.SetString("tree1", (string)Settings[33]);
                    PlayerPrefs.SetString("tree2", (string)Settings[34]);
                    PlayerPrefs.SetString("tree3", (string)Settings[35]);
                    PlayerPrefs.SetString("tree4", (string)Settings[36]);
                    PlayerPrefs.SetString("tree5", (string)Settings[37]);
                    PlayerPrefs.SetString("tree6", (string)Settings[38]);
                    PlayerPrefs.SetString("tree7", (string)Settings[39]);
                    PlayerPrefs.SetString("tree8", (string)Settings[40]);
                    PlayerPrefs.SetString("leaf1", (string)Settings[41]);
                    PlayerPrefs.SetString("leaf2", (string)Settings[42]);
                    PlayerPrefs.SetString("leaf3", (string)Settings[43]);
                    PlayerPrefs.SetString("leaf4", (string)Settings[44]);
                    PlayerPrefs.SetString("leaf5", (string)Settings[45]);
                    PlayerPrefs.SetString("leaf6", (string)Settings[46]);
                    PlayerPrefs.SetString("leaf7", (string)Settings[47]);
                    PlayerPrefs.SetString("leaf8", (string)Settings[48]);
                    PlayerPrefs.SetString("forestG", (string)Settings[49]);
                    PlayerPrefs.SetInt("forestR", (int)Settings[50]);
                    PlayerPrefs.SetString("house1", (string)Settings[51]);
                    PlayerPrefs.SetString("house2", (string)Settings[52]);
                    PlayerPrefs.SetString("house3", (string)Settings[53]);
                    PlayerPrefs.SetString("house4", (string)Settings[54]);
                    PlayerPrefs.SetString("house5", (string)Settings[55]);
                    PlayerPrefs.SetString("house6", (string)Settings[56]);
                    PlayerPrefs.SetString("house7", (string)Settings[57]);
                    PlayerPrefs.SetString("house8", (string)Settings[58]);
                    PlayerPrefs.SetString("cityG", (string)Settings[59]);
                    PlayerPrefs.SetString("cityW", (string)Settings[60]);
                    PlayerPrefs.SetString("cityH", (string)Settings[61]);
                    PlayerPrefs.SetInt("skinQ", QualitySettings.masterTextureLimit);
                    PlayerPrefs.SetInt("skinQL", (int)Settings[63]);
                    PlayerPrefs.SetString("eren", (string)Settings[65]);
                    PlayerPrefs.SetString("annie", (string)Settings[66]);
                    PlayerPrefs.SetString("colossal", (string)Settings[67]);
                    PlayerPrefs.SetString("hoodie", (string)Settings[14]);
                    PlayerPrefs.SetString("cnumber", (string)Settings[82]);
                    PlayerPrefs.SetString("cmax", (string)Settings[85]);
                    PlayerPrefs.SetString("titanbody1", (string)Settings[86]);
                    PlayerPrefs.SetString("titanbody2", (string)Settings[87]);
                    PlayerPrefs.SetString("titanbody3", (string)Settings[88]);
                    PlayerPrefs.SetString("titanbody4", (string)Settings[89]);
                    PlayerPrefs.SetString("titanbody5", (string)Settings[90]);
                    PlayerPrefs.SetInt("customlevel", (int)Settings[91]);
                    PlayerPrefs.SetInt("traildisable", (int)Settings[92]);
                    PlayerPrefs.SetInt("wind", (int)Settings[93]);
                    PlayerPrefs.SetString("trailskin", (string)Settings[94]);
                    PlayerPrefs.SetString("snapshot", (string)Settings[95]);
                    PlayerPrefs.SetString("trailskin2", (string)Settings[96]);
                    PlayerPrefs.SetInt("reel", (int)Settings[97]);
                    PlayerPrefs.SetString("reelin", (string)Settings[98]);
                    PlayerPrefs.SetString("reelout", (string)Settings[99]);
                    PlayerPrefs.SetFloat("vol", AudioListener.volume);
                    PlayerPrefs.SetString("tforward", (string)Settings[101]);
                    PlayerPrefs.SetString("tback", (string)Settings[102]);
                    PlayerPrefs.SetString("tleft", (string)Settings[103]);
                    PlayerPrefs.SetString("tright", (string)Settings[104]);
                    PlayerPrefs.SetString("twalk", (string)Settings[105]);
                    PlayerPrefs.SetString("tjump", (string)Settings[106]);
                    PlayerPrefs.SetString("tpunch", (string)Settings[107]);
                    PlayerPrefs.SetString("tslam", (string)Settings[108]);
                    PlayerPrefs.SetString("tgrabfront", (string)Settings[109]);
                    PlayerPrefs.SetString("tgrabback", (string)Settings[110]);
                    PlayerPrefs.SetString("tgrabnape", (string)Settings[111]);
                    PlayerPrefs.SetString("tantiae", (string)Settings[112]);
                    PlayerPrefs.SetString("tbite", (string)Settings[113]);
                    PlayerPrefs.SetString("tcover", (string)Settings[114]);
                    PlayerPrefs.SetString("tsit", (string)Settings[115]);
                    PlayerPrefs.SetInt("reel2", (int)Settings[116]);
                    PlayerPrefs.SetInt("humangui", (int)Settings[133]);
                    PlayerPrefs.SetString("horse2", (string)Settings[134]);
                    PlayerPrefs.SetString("hair2", (string)Settings[135]);
                    PlayerPrefs.SetString("eye2", (string)Settings[136]);
                    PlayerPrefs.SetString("glass2", (string)Settings[137]);
                    PlayerPrefs.SetString("face2", (string)Settings[138]);
                    PlayerPrefs.SetString("skin2", (string)Settings[139]);
                    PlayerPrefs.SetString("costume2", (string)Settings[140]);
                    PlayerPrefs.SetString("logo2", (string)Settings[141]);
                    PlayerPrefs.SetString("bladel2", (string)Settings[142]);
                    PlayerPrefs.SetString("blader2", (string)Settings[143]);
                    PlayerPrefs.SetString("gas2", (string)Settings[144]);
                    PlayerPrefs.SetString("hoodie2", (string)Settings[145]);
                    PlayerPrefs.SetString("trail2", (string)Settings[146]);
                    PlayerPrefs.SetString("horse3", (string)Settings[147]);
                    PlayerPrefs.SetString("hair3", (string)Settings[148]);
                    PlayerPrefs.SetString("eye3", (string)Settings[149]);
                    PlayerPrefs.SetString("glass3", (string)Settings[150]);
                    PlayerPrefs.SetString("face3", (string)Settings[151]);
                    PlayerPrefs.SetString("skin3", (string)Settings[152]);
                    PlayerPrefs.SetString("costume3", (string)Settings[153]);
                    PlayerPrefs.SetString("logo3", (string)Settings[154]);
                    PlayerPrefs.SetString("bladel3", (string)Settings[155]);
                    PlayerPrefs.SetString("blader3", (string)Settings[156]);
                    PlayerPrefs.SetString("gas3", (string)Settings[157]);
                    PlayerPrefs.SetString("hoodie3", (string)Settings[158]);
                    PlayerPrefs.SetString("trail3", (string)Settings[159]);
                    PlayerPrefs.SetString("customGround", (string)Settings[162]);
                    PlayerPrefs.SetString("forestskyfront", (string)Settings[163]);
                    PlayerPrefs.SetString("forestskyback", (string)Settings[164]);
                    PlayerPrefs.SetString("forestskyleft", (string)Settings[165]);
                    PlayerPrefs.SetString("forestskyright", (string)Settings[166]);
                    PlayerPrefs.SetString("forestskyup", (string)Settings[167]);
                    PlayerPrefs.SetString("forestskydown", (string)Settings[168]);
                    PlayerPrefs.SetString("cityskyfront", (string)Settings[169]);
                    PlayerPrefs.SetString("cityskyback", (string)Settings[170]);
                    PlayerPrefs.SetString("cityskyleft", (string)Settings[171]);
                    PlayerPrefs.SetString("cityskyright", (string)Settings[172]);
                    PlayerPrefs.SetString("cityskyup", (string)Settings[173]);
                    PlayerPrefs.SetString("cityskydown", (string)Settings[174]);
                    PlayerPrefs.SetString("customskyfront", (string)Settings[175]);
                    PlayerPrefs.SetString("customskyback", (string)Settings[176]);
                    PlayerPrefs.SetString("customskyleft", (string)Settings[177]);
                    PlayerPrefs.SetString("customskyright", (string)Settings[178]);
                    PlayerPrefs.SetString("customskyup", (string)Settings[179]);
                    PlayerPrefs.SetString("customskydown", (string)Settings[180]);
                    PlayerPrefs.SetInt("dashenable", (int)Settings[181]);
                    PlayerPrefs.SetString("dashkey", (string)Settings[182]);
                    PlayerPrefs.SetInt("vsync", (int)Settings[183]);
                    PlayerPrefs.SetString("fpscap", (string)Settings[184]);
                    PlayerPrefs.SetInt("speedometer", (int)Settings[189]);
                    PlayerPrefs.SetInt("bombMode", (int)Settings[192]);
                    PlayerPrefs.SetInt("teamMode", (int)Settings[193]);
                    PlayerPrefs.SetInt("rockThrow", (int)Settings[194]);
                    PlayerPrefs.SetInt("explodeModeOn", (int)Settings[195]);
                    PlayerPrefs.SetString("explodeModeNum", (string)Settings[196]);
                    PlayerPrefs.SetInt("healthMode", (int)Settings[197]);
                    PlayerPrefs.SetString("healthLower", (string)Settings[198]);
                    PlayerPrefs.SetString("healthUpper", (string)Settings[199]);
                    PlayerPrefs.SetInt("infectionModeOn", (int)Settings[200]);
                    PlayerPrefs.SetString("infectionModeNum", (string)Settings[201]);
                    PlayerPrefs.SetInt("banEren", (int)Settings[202]);
                    PlayerPrefs.SetInt("moreTitanOn", (int)Settings[203]);
                    PlayerPrefs.SetString("moreTitanNum", (string)Settings[204]);
                    PlayerPrefs.SetInt("damageModeOn", (int)Settings[205]);
                    PlayerPrefs.SetString("damageModeNum", (string)Settings[206]);
                    PlayerPrefs.SetInt("sizeMode", (int)Settings[207]);
                    PlayerPrefs.SetString("sizeLower", (string)Settings[208]);
                    PlayerPrefs.SetString("sizeUpper", (string)Settings[209]);
                    PlayerPrefs.SetInt("spawnModeOn", (int)Settings[210]);
                    PlayerPrefs.SetString("nRate", (string)Settings[211]);
                    PlayerPrefs.SetString("aRate", (string)Settings[212]);
                    PlayerPrefs.SetString("jRate", (string)Settings[213]);
                    PlayerPrefs.SetString("cRate", (string)Settings[214]);
                    PlayerPrefs.SetString("pRate", (string)Settings[215]);
                    PlayerPrefs.SetInt("horseMode", (int)Settings[216]);
                    PlayerPrefs.SetInt("waveModeOn", (int)Settings[217]);
                    PlayerPrefs.SetString("waveModeNum", (string)Settings[218]);
                    PlayerPrefs.SetInt("friendlyMode", (int)Settings[219]);
                    PlayerPrefs.SetInt("pvpMode", (int)Settings[220]);
                    PlayerPrefs.SetInt("maxWaveOn", (int)Settings[221]);
                    PlayerPrefs.SetString("maxWaveNum", (string)Settings[222]);
                    PlayerPrefs.SetInt("endlessModeOn", (int)Settings[223]);
                    PlayerPrefs.SetString("endlessModeNum", (string)Settings[224]);
                    PlayerPrefs.SetString("motd", (string)Settings[225]);
                    PlayerPrefs.SetInt("pointModeOn", (int)Settings[226]);
                    PlayerPrefs.SetString("pointModeNum", (string)Settings[227]);
                    PlayerPrefs.SetInt("ahssReload", (int)Settings[228]);
                    PlayerPrefs.SetInt("punkWaves", (int)Settings[229]);
                    PlayerPrefs.SetInt("mapOn", (int)Settings[231]);
                    PlayerPrefs.SetString("mapMaximize", (string)Settings[232]);
                    PlayerPrefs.SetString("mapToggle", (string)Settings[233]);
                    PlayerPrefs.SetString("mapReset", (string)Settings[234]);
                    PlayerPrefs.SetInt("globalDisableMinimap", (int)Settings[235]);
                    PlayerPrefs.SetString("chatRebind", (string)Settings[236]);
                    PlayerPrefs.SetString("hforward", (string)Settings[237]);
                    PlayerPrefs.SetString("hback", (string)Settings[238]);
                    PlayerPrefs.SetString("hleft", (string)Settings[239]);
                    PlayerPrefs.SetString("hright", (string)Settings[240]);
                    PlayerPrefs.SetString("hwalk", (string)Settings[241]);
                    PlayerPrefs.SetString("hjump", (string)Settings[242]);
                    PlayerPrefs.SetString("hmount", (string)Settings[243]);
                    PlayerPrefs.SetInt("chatfeed", (int)Settings[244]);
                    PlayerPrefs.SetFloat("bombR", (float)Settings[246]);
                    PlayerPrefs.SetFloat("bombG", (float)Settings[247]);
                    PlayerPrefs.SetFloat("bombB", (float)Settings[248]);
                    PlayerPrefs.SetFloat("bombA", (float)Settings[249]);
                    PlayerPrefs.SetFloat("bombRadius", (float)Settings[250]);
                    PlayerPrefs.SetFloat("bombRange", (float)Settings[251]);
                    PlayerPrefs.SetFloat("bombSpeed", (float)Settings[252]);
                    PlayerPrefs.SetFloat("bombCD", (float)Settings[253]);
                    PlayerPrefs.SetString("cannonUp", (string)Settings[254]);
                    PlayerPrefs.SetString("cannonDown", (string)Settings[255]);
                    PlayerPrefs.SetString("cannonLeft", (string)Settings[256]);
                    PlayerPrefs.SetString("cannonRight", (string)Settings[257]);
                    PlayerPrefs.SetString("cannonFire", (string)Settings[258]);
                    PlayerPrefs.SetString("cannonMount", (string)Settings[259]);
                    PlayerPrefs.SetString("cannonSlow", (string)Settings[260]);
                    PlayerPrefs.SetInt("deadlyCannon", (int)Settings[261]);
                    PlayerPrefs.SetString("liveCam", (string)Settings[262]);
                    Settings[64] = 4;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 463f, halfMenuHeight + 468f, 40f, 25f), "Load"))
                {
                    LoadConfig();
                    Settings[64] = 5;
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 508f, halfMenuHeight + 468f, 60f, 25f), "Default"))
                {
                    GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().setToDefault();
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 573f, halfMenuHeight + 468f, 75f, 25f), "Continue"))
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                    {
                        Time.timeScale = 1f;
                    }
                    if (!mainCamera.enabled)
                    {
                        Screen.showCursor = true;
                        Screen.lockCursor = true;
                        GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
                        Camera.main.GetComponent<SpectatorMovement>().disable = false;
                        Camera.main.GetComponent<MouseLook>().disable = false;
                        return;
                    }
                    IN_GAME_MAIN_CAMERA.IsPausing = false;
                    if (IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS)
                    {
                        Screen.showCursor = false;
                        Screen.lockCursor = true;
                    }
                    else
                    {
                        Screen.showCursor = false;
                        Screen.lockCursor = false;
                    }
                    GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
                    GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().justUPDATEME();
                }
                else if (GUI.Button(new Rect(halfMenuWidth + 653f, halfMenuHeight + 468f, 40f, 25f), "Quit"))
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                    {
                        Time.timeScale = 1f;
                    }
                    else
                    {
                        PhotonNetwork.Disconnect();
                    }
                    Screen.lockCursor = false;
                    Screen.showCursor = true;
                    IN_GAME_MAIN_CAMERA.Gametype = GameType.Stop;
                    gameStart = false;
                    GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
                    DestroyAllExistingCloths();
                    UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
                    Application.LoadLevel("menu");
                }
            }
            else
            {
                if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Multiplayer)
                {
                    return;
                }

                float num7 = (float)Screen.width / 2f;
                float num8 = (float)Screen.height / 2f;
                if (Time.timeScale <= 0.1f)
                {
                    GUI.Box(new Rect(num7 - 100f, num8 - 50f, 200f, 100f), string.Empty);
                    if (this.pauseWaitTime > 3f)
                    {
                        GUI.Label(new Rect(num7 - 43f, num8 - 10f, 200f, 22f), "Game Paused.");
                    }
                    else
                    {
                        GUI.Label(new Rect(num7 - 43f, num8 - 15f, 200f, 22f), "Unpausing in:");
                        GUI.Label(new Rect(num7 - 8f, num8 + 5f, 200f, 22f), pauseWaitTime.ToString("F1"));
                    }
                }
                else if (!LogicLoaded || !CustomLevelLoaded)
                {
                    GUI.Box(new Rect(num7 - 100f, num8 - 50f, 200f, 150f), string.Empty);
                    int ourLength = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.CurrentLevel]).Length;
                    int masterLength = GExtensions.AsString(PhotonNetwork.masterClient.customProperties[PhotonPlayerProperty.CurrentLevel]).Length;
                    GUI.Label(new Rect(num7 - 60f, num8 - 30f, 200f, 22f), "Loading Level (" + ourLength + "/" + masterLength + ")");
                    retryTime += Time.deltaTime;
                    Screen.lockCursor = false;
                    Screen.showCursor = true;
                    if (GUI.Button(new Rect(num7 - 20f, num8 + 50f, 40f, 30f), "Quit"))
                    {
                        PhotonNetwork.Disconnect();
                        Screen.lockCursor = false;
                        Screen.showCursor = true;
                        IN_GAME_MAIN_CAMERA.Gametype = GameType.Stop;
                        this.gameStart = false;
                        GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
                        DestroyAllExistingCloths();
                        UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
                        Application.LoadLevel("menu");
                    }
                }
            }
        }
    }

    public void SpawnTitanCustom(string place, int abnormal, int rate, bool punk)
    {
        int titansToSpawn = rate;

        if (Level.Name.StartsWith("Custom"))
        {
            titansToSpawn = 5;
            if (RCSettings.GameType == 1)
            {
                titansToSpawn = 3;
            }
            else if (RCSettings.GameType == 2 || RCSettings.GameType == 3)
            {
                titansToSpawn = 0;
            }
        }

        if (RCSettings.MoreTitans > 0 || (RCSettings.MoreTitans == 0 && Level.Name.StartsWith("Custom") && RCSettings.GameType >= 2))
        {
            titansToSpawn = RCSettings.MoreTitans;
        }

        if (Level.Mode == GameMode.Survival)
        {
            if (punk)
            {
                titansToSpawn = rate;
            }
            else if (RCSettings.MoreTitans == 0)
            {
                int scale = 1;
                if (RCSettings.WaveModeOn == 1)
                {
                    scale = RCSettings.WaveModeNum;
                }
                titansToSpawn += (wave - 1) * (scale - 1);
            }
            else if (RCSettings.MoreTitans > 0)
            {
                int scale = 1;
                if (RCSettings.WaveModeOn == 1)
                {
                    scale = RCSettings.WaveModeNum;
                }
                titansToSpawn += (wave - 1) * scale;
            }
        }

        titansToSpawn = Math.Min(50, titansToSpawn);
        if (RCSettings.SpawnMode == 1)
        {
            float normalRate = RCSettings.NormalRate;
            float abbyRate = RCSettings.AberrantRate;
            float jumperRate = RCSettings.JumperRate;
            float crawlerRate = RCSettings.CrawlerRate;
            float punkRate = RCSettings.PunkRate;
            if (punk && RCSettings.PunkWaves == 1)
            {
                normalRate = 0f;
                abbyRate = 0f;
                jumperRate = 0f;
                crawlerRate = 0f;
                punkRate = 100f;
                titansToSpawn = rate;
            }
            for (int i = 0; i < titansToSpawn; i++)
            {
                Vector3 position = new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f));
                Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
                if (titanSpawns.Count > 0)
                {
                    position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
                }
                else
                {
                    GameObject[] array = GameObject.FindGameObjectsWithTag(place);
                    if (array.Length > 0)
                    {
                        int num8 = UnityEngine.Random.Range(0, array.Length);
                        GameObject gameObject = array[num8];
                        while (array[num8] == null)
                        {
                            num8 = UnityEngine.Random.Range(0, array.Length);
                            gameObject = array[num8];
                        }
                        array[num8] = null;
                        position = gameObject.transform.position;
                        rotation = gameObject.transform.rotation;
                    }
                }
                float rngRate = UnityEngine.Random.Range(0f, 100f);
                if (rngRate <= normalRate + abbyRate + jumperRate + crawlerRate + punkRate)
                {
                    GameObject titan = SpawnTitanRaw(position, rotation);
                    if (rngRate < normalRate)
                    {
                        titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Normal, forceCrawler: false);
                    }
                    else if (rngRate >= normalRate && rngRate < normalRate + abbyRate)
                    {
                        titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Aberrant, forceCrawler: false);
                    }
                    else if (rngRate >= normalRate + abbyRate && rngRate < normalRate + abbyRate + jumperRate)
                    {
                        titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Jumper, forceCrawler: false);
                    }
                    else if (rngRate >= normalRate + abbyRate + jumperRate && rngRate < normalRate + abbyRate + jumperRate + crawlerRate)
                    {
                        titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Crawler, forceCrawler: true);
                    }
                    else if (rngRate >= normalRate + abbyRate + jumperRate + crawlerRate && rngRate < normalRate + abbyRate + jumperRate + crawlerRate + punkRate)
                    {
                        titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Punk, forceCrawler: false);
                    }
                    else
                    {
                        titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Normal, forceCrawler: false);
                    }
                }
                else
                {
                    SpawnTitan(abnormal, position, rotation, punk);
                }
            }
        }
        else if (Level.Name.StartsWith("Custom"))
        {
            for (int i = 0; i < titansToSpawn; i++)
            {
                Vector3 position = new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f));
                Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
                if (titanSpawns.Count > 0)
                {
                    position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
                }
                else
                {
                    GameObject[] array = GameObject.FindGameObjectsWithTag(place);
                    if (array.Length > 0)
                    {
                        int num8 = UnityEngine.Random.Range(0, array.Length);
                        GameObject gameObject = array[num8];
                        while (array[num8] == null)
                        {
                            num8 = UnityEngine.Random.Range(0, array.Length);
                            gameObject = array[num8];
                        }
                        array[num8] = null;
                        position = gameObject.transform.position;
                        rotation = gameObject.transform.rotation;
                    }
                }
                SpawnTitan(abnormal, position, rotation, punk);
            }
        }
        else
        {
            for (int i = 0; i < titansToSpawn; i++)
            {
                SpawnTitanRandom("titanRespawn", abnormal, punk);
            }
        }
    }

    public void SpawnTitanAction(int type, float size, int health, int number)
    {
        Vector3 position = new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f));
        Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
        if (titanSpawns.Count > 0)
        {
            position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
        }
        else
        {
            GameObject[] array = GameObject.FindGameObjectsWithTag("titanRespawn");
            if (array.Length > 0)
            {
                int num = UnityEngine.Random.Range(0, array.Length);
                GameObject gameObject = array[num];
                while (array[num] == null)
                {
                    num = UnityEngine.Random.Range(0, array.Length);
                    gameObject = array[num];
                }
                array[num] = null;
                position = gameObject.transform.position;
                rotation = gameObject.transform.rotation;
            }
        }
        for (int i = 0; i < number; i++)
        {
            GameObject titan = SpawnTitanRaw(position, rotation);
            titan.GetComponent<TITAN>().SetLevel(size);
            titan.GetComponent<TITAN>().hasSetLevel = true;
            if ((float)health > 0f)
            {
                titan.GetComponent<TITAN>().currentHealth = health;
                titan.GetComponent<TITAN>().maxHealth = health;
            }
            switch (type)
            {
                case 0:
                    titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Normal, forceCrawler: false);
                    break;
                case 1:
                    titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Aberrant, forceCrawler: false);
                    break;
                case 2:
                    titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Jumper, forceCrawler: false);
                    break;
                case 3:
                    titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Crawler, forceCrawler: true);
                    break;
                case 4:
                    titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Punk, forceCrawler: false);
                    break;
            }
        }
    }

    public void SpawnTitanAtAction(int type, float size, int health, int number, float posX, float posY, float posZ)
    {
        Vector3 position = new Vector3(posX, posY, posZ);
        Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
        for (int i = 0; i < number; i++)
        {
            GameObject titan = SpawnTitanRaw(position, rotation);
            titan.GetComponent<TITAN>().SetLevel(size);
            titan.GetComponent<TITAN>().hasSetLevel = true;
            if ((float)health > 0f)
            {
                titan.GetComponent<TITAN>().currentHealth = health;
                titan.GetComponent<TITAN>().maxHealth = health;
            }
            switch (type)
            {
                case 0:
                    titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Normal, forceCrawler: false);
                    break;
                case 1:
                    titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Aberrant, forceCrawler: false);
                    break;
                case 2:
                    titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Jumper, forceCrawler: false);
                    break;
                case 3:
                    titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Crawler, forceCrawler: true);
                    break;
                case 4:
                    titan.GetComponent<TITAN>().setAbnormalType2(TitanClass.Punk, forceCrawler: false);
                    break;
            }
        }
    }

    [RPC]
    private void spawnTitanRPC(PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            foreach (TITAN titan in titans)
            {
                if (titan.photonView.isMine && (!PhotonNetwork.isMasterClient || titan.nonAI))
                {
                    PhotonNetwork.Destroy(titan.gameObject);
                }
            }

            SpawnNonAITitan2(myLastHero);
        }
    }

    [RPC]
    private void setTeamRPC(int newTeam, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient || info.sender.isLocal)
        {
            SetTeam(newTeam);
        }
    }

    private void SetTeam(int newTeam)
    {
        switch (newTeam)
        {
            case 0: // Individual
                {
                    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.RCTeam, 0);
                    hashtable.Add(PhotonPlayerProperty.Name, LoginFengKAI.Player.Name);
                    PhotonNetwork.player.SetCustomProperties(hashtable);
                    break;
                }
            case 1: // Cyan
                {
                    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.RCTeam, 1);
                    hashtable.Add(PhotonPlayerProperty.Name, "[00FFFF]" + LoginFengKAI.Player.Name.StripNGUI());
                    PhotonNetwork.player.SetCustomProperties(hashtable);
                    break;
                }
            case 2: // Magenta
                {
                    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.RCTeam, 2);
                    hashtable.Add(PhotonPlayerProperty.Name, "[FF00FF]" + LoginFengKAI.Player.Name.StripNGUI());
                    PhotonNetwork.player.SetCustomProperties(hashtable);
                    break;
                }
            case 3: // Auto
                {
                    int cyanCount = 0;
                    int magentaCount = 0;
                    int team = 1;
                    foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
                    {
                        switch (GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.RCTeam]))
                        {
                            case 1:
                                cyanCount++;
                                break;
                            case 2:
                                magentaCount++;
                                break;
                        }
                    }
                    if (cyanCount > magentaCount)
                    {
                        team = 2;
                    }
                    SetTeam(team);
                    break;
                }
        }

        foreach (HERO hero in GameObject.FindObjectsOfType<HERO>())
        {
            if (hero.photonView.isMine)
            {
                base.photonView.RPC("labelRPC", PhotonTargets.All, hero.photonView.viewID);
            }
        }
    }

    [RPC]
    private void settingRPC(ExitGames.Client.Photon.Hashtable settings, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            SetGameSettings(settings);
        }
    }

    private void SetGameSettings(ExitGames.Client.Photon.Hashtable settings)
    {
        restartingEren = false;
        restartingBomb = false;
        restartingHorse = false;
        restartingTitan = false;

        // Bomb PVP
        if (settings.ContainsKey("bomb"))
        {
            if (RCSettings.BombMode != (int)settings["bomb"])
            {
                RCSettings.BombMode = (int)settings["bomb"];
                InRoomChat.Instance.AddLine("PVP Bomb Mode enabled.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.BombMode != 0)
        {
            RCSettings.BombMode = 0;
            InRoomChat.Instance.AddLine("PVP Bomb Mode disabled.".AsColor("FFCC00"));
            if (PhotonNetwork.isMasterClient)
            {
                restartingBomb = true;
            }
        }

        // Global Minimap Disable
        if (settings.ContainsKey("globalDisableMinimap"))
        {
            if (RCSettings.GlobalDisableMinimap != (int)settings["globalDisableMinimap"])
            {
                RCSettings.GlobalDisableMinimap = (int)settings["globalDisableMinimap"];
                InRoomChat.Instance.AddLine("Minimaps are not allowed.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.GlobalDisableMinimap != 0)
        {
            RCSettings.GlobalDisableMinimap = 0;
            InRoomChat.Instance.AddLine("Minimaps are allowed.".AsColor("FFCC00"));
        }

        // Horses
        if (settings.ContainsKey("horse"))
        {
            if (RCSettings.HorseMode != (int)settings["horse"])
            {
                RCSettings.HorseMode = (int)settings["horse"];
                InRoomChat.Instance.AddLine("Horses enabled.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.HorseMode != 0)
        {
            RCSettings.HorseMode = 0;
            InRoomChat.Instance.AddLine("Horses disabled.".AsColor("FFCC00"));
            if (PhotonNetwork.isMasterClient)
            {
                restartingHorse = true;
            }
        }

        // Punk Waves
        if (settings.ContainsKey("punkWaves"))
        {
            if (RCSettings.PunkWaves != (int)settings["punkWaves"])
            {
                RCSettings.PunkWaves = (int)settings["punkWaves"];
                InRoomChat.Instance.AddLine("Punk override every 5 waves enabled.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.PunkWaves != 0)
        {
            RCSettings.PunkWaves = 0;
            InRoomChat.Instance.AddLine("Punk override every 5 waves disabled.".AsColor("FFCC00"));
        }

        // AHSS Air-Reload
        if (settings.ContainsKey("ahssReload"))
        {
            if (RCSettings.AhssReload != (int)settings["ahssReload"])
            {
                RCSettings.AhssReload = (int)settings["ahssReload"];
                InRoomChat.Instance.AddLine("AHSS Air-Reload is not allowed.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.AhssReload != 0)
        {
            RCSettings.AhssReload = 0;
            InRoomChat.Instance.AddLine("AHSS Air-Reload is allowed.".AsColor("FFCC00"));
        }

        // Team Sorting
        if (settings.ContainsKey("team"))
        {
            if (RCSettings.TeamMode != (int)settings["team"])
            {
                RCSettings.TeamMode = (int)settings["team"];
                string str = string.Empty;
                switch (RCSettings.TeamMode)
                {
                    case 1:
                        str = "No sort";
                        break;
                    case 2:
                        str = "Locked by Size";
                        break;
                    case 3:
                        str = "Locked by Skill";
                        break;
                }
                InRoomChat.Instance.AddLine("Team Mode enabled</color> (".AsColor("FFCC00") + str + ").".AsColor("FFCC00"));
                if (GExtensions.AsInt(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCTeam]) == 0)
                {
                    SetTeam(3);
                }
            }
        }
        else if (RCSettings.TeamMode != 0)
        {
            RCSettings.TeamMode = 0;
            SetTeam(0);
            InRoomChat.Instance.AddLine("Team Mode disabled.".AsColor("FFCC00"));
        }

        // Point limit
        if (settings.ContainsKey("point"))
        {
            if (RCSettings.PointMode != (int)settings["point"])
            {
                RCSettings.PointMode = (int)settings["point"];
                InRoomChat.Instance.AddLine("Point Limit enabled (".AsColor("FFCC00") + RCSettings.PointMode + ").".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.PointMode != 0)
        {
            RCSettings.PointMode = 0;
            InRoomChat.Instance.AddLine("Point limit disabled.".AsColor("FFCC00"));
        }

        // Punk Rocks
        if (settings.ContainsKey("rock"))
        {
            if (RCSettings.DisableRock != (int)settings["rock"])
            {
                RCSettings.DisableRock = (int)settings["rock"];
                InRoomChat.Instance.AddLine("Punk rock throwing disabled.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.DisableRock != 0)
        {
            RCSettings.DisableRock = 0;
            InRoomChat.Instance.AddLine("Punk rock throwing enabled.".AsColor("FFCC00"));
        }

        // Titan Explode
        if (settings.ContainsKey("explode"))
        {
            if (RCSettings.ExplodeMode != (int)settings["explode"])
            {
                RCSettings.ExplodeMode = (int)settings["explode"];
                InRoomChat.Instance.AddLine("Titan Explode Mode enabled (Radius ".AsColor("FFCC00") + RCSettings.ExplodeMode + ").".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.ExplodeMode != 0)
        {
            RCSettings.ExplodeMode = 0;
            InRoomChat.Instance.AddLine("Titan Explode Mode disabled.".AsColor("FFCC00"));
        }

        // Titan Health
        if (settings.ContainsKey("healthMode") && settings.ContainsKey("healthLower") && settings.ContainsKey("healthUpper"))
        {
            if (RCSettings.HealthMode != (int)settings["healthMode"] || RCSettings.HealthLower != (int)settings["healthLower"] || RCSettings.HealthUpper != (int)settings["healthUpper"])
            {
                RCSettings.HealthMode = (int)settings["healthMode"];
                RCSettings.HealthLower = (int)settings["healthLower"];
                RCSettings.HealthUpper = (int)settings["healthUpper"];
                string str = "Static";
                if (RCSettings.HealthMode == 2)
                {
                    str = "Scaled";
                }
                InRoomChat.Instance.AddLine("Titan Health (".AsColor("FFCC00") + str + ", ".AsColor("FFCC00")
                    + RCSettings.HealthLower + " to ".AsColor("FFCC00")
                    + RCSettings.HealthUpper + ") enabled.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.HealthMode != 0 || RCSettings.HealthLower != 0 || RCSettings.HealthUpper != 0)
        {
            RCSettings.HealthMode = 0;
            RCSettings.HealthLower = 0;
            RCSettings.HealthUpper = 0;
            InRoomChat.Instance.AddLine("Titan Health disabled.".AsColor("FFCC00"));
        }

        // Infection
        if (settings.ContainsKey("infection"))
        {
            if (RCSettings.InfectionMode != (int)settings["infection"])
            {
                RCSettings.InfectionMode = (int)settings["infection"];
                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable.Add(PhotonPlayerProperty.RCTeam, 0);
                PhotonNetwork.player.SetCustomProperties(hashtable);
                InRoomChat.Instance.AddLine("Infection mode (".AsColor("FFCC00") + RCSettings.InfectionMode + ") enabled. Make sure your first character is human.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.InfectionMode != 0)
        {
            RCSettings.InfectionMode = 0;
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.IsTitan, 1);
            PhotonNetwork.player.SetCustomProperties(hashtable);
            InRoomChat.Instance.AddLine("Infection Mode disabled.".AsColor("FFCC00"));
            if (PhotonNetwork.isMasterClient)
            {
                restartingTitan = true;
            }
        }

        // Anti-Eren
        if (settings.ContainsKey("eren"))
        {
            if (RCSettings.BanEren != (int)settings["eren"])
            {
                RCSettings.BanEren = (int)settings["eren"];
                InRoomChat.Instance.AddLine("Anti-Eren enabled. Using Titan Eren will get you kicked.".AsColor("FFCC00"));
                if (PhotonNetwork.isMasterClient)
                {
                    restartingEren = true;
                }
            }
        }
        else if (RCSettings.BanEren != 0)
        {
            RCSettings.BanEren = 0;
            InRoomChat.Instance.AddLine("Anti-Eren disabled. Titan Eren is allowed.".AsColor("FFCC00"));
        }

        // Custom Titan Count
        if (settings.ContainsKey("titanc"))
        {
            if (RCSettings.MoreTitans != (int)settings["titanc"])
            {
                RCSettings.MoreTitans = (int)settings["titanc"];
                InRoomChat.Instance.AddLine(RCSettings.MoreTitans + " Titans will spawn each round.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.MoreTitans != 0)
        {
            RCSettings.MoreTitans = 0;
            InRoomChat.Instance.AddLine("Default titan amount will spawn each round.".AsColor("FFCC00"));
        }

        // Minimum Damage
        if (settings.ContainsKey("damage"))
        {
            if (RCSettings.MinimumDamage != (int)settings["damage"])
            {
                RCSettings.MinimumDamage = (int)settings["damage"];
                InRoomChat.Instance.AddLine("Minimum nape damage (".AsColor("FFCC00") + RCSettings.MinimumDamage + ") enabled.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.MinimumDamage != 0)
        {
            RCSettings.MinimumDamage = 0;
            InRoomChat.Instance.AddLine("Minimum nape damage disabled.".AsColor("FFCC00"));
        }

        // Custom Titan Sizes
        if (settings.ContainsKey("sizeMode") && settings.ContainsKey("sizeLower") && settings.ContainsKey("sizeUpper"))
        {
            // Temporary? fix for RiceCake not properly re-implementing all legacy settings
            if (settings["sizeMode"] is bool)
            {
                settings["sizeMode"] = (bool)settings["sizeMode"] ? 1 : 0;

                // Logging it for funsies, lol
                Guardian.Mod.Logger.Debug("RC2020 'sizeMode' as <b>bool</b> detected, replacing with <b>int</b> equivalent.");
            }

            if (RCSettings.SizeMode != (int)settings["sizeMode"] || RCSettings.SizeLower != (float)settings["sizeLower"] || RCSettings.SizeUpper != (float)settings["sizeUpper"])
            {
                RCSettings.SizeMode = (int)settings["sizeMode"];
                RCSettings.SizeLower = (float)settings["sizeLower"];
                RCSettings.SizeUpper = (float)settings["sizeUpper"];
                InRoomChat.Instance.AddLine("Custom titan size (".AsColor("FFCC00")
                    + RCSettings.SizeLower.ToString("F2") + ", ".AsColor("FFCC00")
                    + RCSettings.SizeUpper.ToString("F2") + ") enabled.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.SizeMode != 0 || RCSettings.SizeLower != 0f || RCSettings.SizeUpper != 0f)
        {
            RCSettings.SizeMode = 0;
            RCSettings.SizeLower = 0f;
            RCSettings.SizeUpper = 0f;
            InRoomChat.Instance.AddLine("Custom titan size disabled.".AsColor("FFCC00"));
        }

        // Custom Spawn Rates
        if (settings.ContainsKey("spawnMode") && settings.ContainsKey("nRate") && settings.ContainsKey("aRate") && settings.ContainsKey("jRate") && settings.ContainsKey("cRate") && settings.ContainsKey("pRate"))
        {
            if (RCSettings.SpawnMode != (int)settings["spawnMode"] || RCSettings.NormalRate != (float)settings["nRate"] || RCSettings.AberrantRate != (float)settings["aRate"] || RCSettings.JumperRate != (float)settings["jRate"] || RCSettings.CrawlerRate != (float)settings["cRate"] || RCSettings.PunkRate != (float)settings["pRate"])
            {
                RCSettings.SpawnMode = (int)settings["spawnMode"];
                RCSettings.NormalRate = (float)settings["nRate"];
                RCSettings.AberrantRate = (float)settings["aRate"];
                RCSettings.JumperRate = (float)settings["jRate"];
                RCSettings.CrawlerRate = (float)settings["cRate"];
                RCSettings.PunkRate = (float)settings["pRate"];
                InRoomChat.Instance.AddLine("Custom spawn rate enabled (".AsColor("FFCC00") + RCSettings.NormalRate.ToString("F2") + "% Normal, ".AsColor("FFCC00")
                    + RCSettings.AberrantRate.ToString("F2") + "% Abnormal, ".AsColor("FFCC00")
                    + RCSettings.JumperRate.ToString("F2") + "% Jumper, ".AsColor("FFCC00")
                    + RCSettings.CrawlerRate.ToString("F2") + "% Crawler, ".AsColor("FFCC00")
                    + RCSettings.PunkRate.ToString("F2") + "% Punk)".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.SpawnMode != 0 || RCSettings.NormalRate != 0f || RCSettings.AberrantRate != 0f || RCSettings.JumperRate != 0f || RCSettings.CrawlerRate != 0f || RCSettings.PunkRate != 0f)
        {
            RCSettings.SpawnMode = 0;
            RCSettings.NormalRate = 0f;
            RCSettings.AberrantRate = 0f;
            RCSettings.JumperRate = 0f;
            RCSettings.CrawlerRate = 0f;
            RCSettings.PunkRate = 0f;
            InRoomChat.Instance.AddLine("Custom spawn rate disabled.".AsColor("FFCC00"));
        }

        // Wave Mode (Titan count multiplier?)
        if (settings.ContainsKey("waveModeOn") && settings.ContainsKey("waveModeNum"))
        {
            if (RCSettings.WaveModeOn != (int)settings["waveModeOn"] || RCSettings.WaveModeNum != (int)settings["waveModeNum"])
            {
                RCSettings.WaveModeOn = (int)settings["waveModeOn"];
                RCSettings.WaveModeNum = (int)settings["waveModeNum"];
                InRoomChat.Instance.AddLine("Custom Wave Mode (".AsColor("FFCC00") + RCSettings.WaveModeNum.ToString() + ") enabled.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.WaveModeOn != 0 || RCSettings.WaveModeNum != 0)
        {
            RCSettings.WaveModeOn = 0;
            RCSettings.WaveModeNum = 0;
            InRoomChat.Instance.AddLine("Custom Wave Mode disabled.".AsColor("FFCC00"));
        }

        // Friendly Fire
        if (settings.ContainsKey("friendly"))
        {
            if (RCSettings.FriendlyMode != (int)settings["friendly"])
            {
                RCSettings.FriendlyMode = (int)settings["friendly"];
                InRoomChat.Instance.AddLine("Friendly Fire disabled, PVP is not allowed.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.FriendlyMode != 0)
        {
            RCSettings.FriendlyMode = 0;
            InRoomChat.Instance.AddLine("Friendly Fire enabled, PVP is allowed.".AsColor("FFCC00"));
        }

        // PVP Mode
        if (settings.ContainsKey("pvp"))
        {
            if (RCSettings.PvPMode != (int)settings["pvp"])
            {
                RCSettings.PvPMode = (int)settings["pvp"];
                string str = string.Empty;
                if (RCSettings.PvPMode == 1)
                {
                    str = "Team-Based";
                }
                else if (RCSettings.PvPMode == 2)
                {
                    str = "FFA";
                }
                InRoomChat.Instance.AddLine("Blade/AHSS PVP enabled (".AsColor("FFCC00") + str + ").".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.PvPMode != 0)
        {
            RCSettings.PvPMode = 0;
            InRoomChat.Instance.AddLine("Blade/AHSS PVP disabled.".AsColor("FFCC00"));
        }

        // Max Wave
        if (settings.ContainsKey("maxwave"))
        {
            if (RCSettings.MaxWave != (int)settings["maxwave"])
            {
                RCSettings.MaxWave = (int)settings["maxwave"];
                InRoomChat.Instance.AddLine("Max Wave is ".AsColor("FFCC00") + RCSettings.MaxWave + ".".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.MaxWave != 0)
        {
            RCSettings.MaxWave = 0;
            InRoomChat.Instance.AddLine("Max Wave set to default (20)".AsColor("FFCC00"));
        }

        // Endless Respawn
        if (settings.ContainsKey("endless"))
        {
            if (RCSettings.EndlessMode != (int)settings["endless"])
            {
                RCSettings.EndlessMode = (int)settings["endless"];
                InRoomChat.Instance.AddLine("Endless Respawn enabled (".AsColor("FFCC00") + RCSettings.EndlessMode + "s).".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.EndlessMode != 0)
        {
            RCSettings.EndlessMode = 0;
            InRoomChat.Instance.AddLine("Endless Respawn disabled.".AsColor("FFCC00"));
        }

        // Deadly Cannons
        if (settings.ContainsKey("deadlycannons"))
        {
            if (RCSettings.DeadlyCannons != (int)settings["deadlycannons"])
            {
                RCSettings.DeadlyCannons = (int)settings["deadlycannons"];
                InRoomChat.Instance.AddLine("Cannons will now kill humans.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.DeadlyCannons != 0)
        {
            RCSettings.DeadlyCannons = 0;
            InRoomChat.Instance.AddLine("Cannons will no longer kill humans.".AsColor("FFCC00"));
        }

        // Aso Racing
        if (settings.ContainsKey("asoracing"))
        {
            if (RCSettings.RacingStatic != (int)settings["asoracing"])
            {
                RCSettings.RacingStatic = (int)settings["asoracing"];
                InRoomChat.Instance.AddLine("Racing will not restart on win.".AsColor("FFCC00"));
            }
        }
        else if (RCSettings.RacingStatic != 0)
        {
            RCSettings.RacingStatic = 0;
            InRoomChat.Instance.AddLine("Racing will restart on win.".AsColor("FFCC00"));
        }

        // MOTD
        if (settings.ContainsKey("motd"))
        {
            if (RCSettings.Motd != (string)settings["motd"])
            {
                RCSettings.Motd = (string)settings["motd"];
                InRoomChat.Instance.AddLine("MOTD: ".AsColor("FFCC00") + RCSettings.Motd);
            }
        }
        else if (RCSettings.Motd.Length > 0)
        {
            RCSettings.Motd = string.Empty;
        }
    }

    [RPC]
    private void labelRPC(int viewId, PhotonMessageInfo info)
    {
        // Anarchy detection
        // https://github.com/aelariane/Anarchy/blob/master/Anarchy/Assembly/AoTTG/FengRPCs.cs
        if (((info.timeInt - 1000000) * -1) == info.sender.Id)
        {
            info.sender.IsAnarchy = true;
        }

        PhotonView pv = PhotonView.Find(viewId);
        if (pv == null || pv.owner != info.sender || pv.gameObject == null)
        {
            return;
        }
        PhotonPlayer owner = pv.owner;
        string newGuild = GExtensions.AsString(owner.customProperties[PhotonPlayerProperty.Guild]);
        string newName = GExtensions.AsString(owner.customProperties[PhotonPlayerProperty.Name]);
        GameObject gameObject = pv.gameObject;

        HERO component = gameObject.GetComponent<HERO>();
        if (component != null)
        {
            UILabel label = component.myNetWorkName.GetComponent<UILabel>();
            label.text = newName;

            if (newGuild.Length > 0)
            {
                label.text = "[FFFF00]" + newGuild + "\n[FFFFFF]" + label.text;
            }
        }
    }

    [RPC]
    private void setMasterRC(PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            MasterRC = true;
        }
    }

    private string HairType(int type)
    {
        if (type < 0)
        {
            return "Random";
        }
        return "Male " + type;
    }

    private string MasterTextureType(int type)
    {
        switch (type)
        {
            case 0:
                return "Highest";
            case 1:
                return "Medium";
            case 2:
                return "Low";
            case 3:
                return "Lower";
            case 4:
                return "Lowest";
            case 5:
                return "Ultra-Low";
            case 6:
                return "Ultra-Low..er?";
            case 7:
                return "We don't like 7.";
            case 8:
                return "Why Why Why";
        }
        return type + ", what???";
    }

    public void RestartRC()
    {
        IntVariables.Clear();
        BoolVariables.Clear();
        StringVariables.Clear();
        FloatVariables.Clear();
        PlayerVariables.Clear();
        TitanVariables.Clear();

        if (RCSettings.InfectionMode > 0)
        {
            EndGameInfection();
        }
        else
        {
            EndGameRC();
        }
    }
}