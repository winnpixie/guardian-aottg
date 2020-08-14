using System.Collections;
using UnityEngine;

public class PVPcheckPoint : Photon.MonoBehaviour
{
    private FengGameManagerMKII fengGame;
    public int id;
    public GameObject humanCyc;
    public GameObject titanCyc;
    public CheckPointState state;
    private bool titanOn;
    private bool playerOn;
    public float humanPt;
    public float titanPt;
    public float humanPtMax = 40f;
    public float titanPtMax = 40f;
    public int normalTitanRate = 70;
    private bool annie;
    private float spawnTitanTimer;
    public float titanInterval = 30f;
    public float size = 1f;
    private float hitTestR = 15f;
    private GameObject supply;
    private float syncTimer;
    private float syncInterval = 0.6f;
    private float getPtsTimer;
    private float getPtsInterval = 20f;
    public GameObject[] chkPtNextArr;
    public GameObject[] chkPtPreviousArr;
    public static ArrayList chkPts;
    public bool hasAnnie;
    public bool isBase;

    public GameObject chkPtNext
    {
        get
        {
            if (chkPtNextArr.Length <= 0)
            {
                return null;
            }
            return chkPtNextArr[Random.Range(0, chkPtNextArr.Length)];
        }
    }

    public GameObject chkPtPrevious
    {
        get
        {
            if (chkPtPreviousArr.Length <= 0)
            {
                return null;
            }
            return chkPtPreviousArr[Random.Range(0, chkPtPreviousArr.Length)];
        }
    }

    private void Start()
    {
        fengGame = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        if (IN_GAME_MAIN_CAMERA.Gamemode != GAMEMODE.PVP_CAPTURE)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        chkPts.Add(this);
        IComparer comparer = new IComparerPVPchkPtID();
        chkPts.Sort(comparer);
        if (humanPt == humanPtMax)
        {
            state = CheckPointState.Human;
            if (base.photonView.isMine && LevelInfo.getInfo(FengGameManagerMKII.level).mapName != "The City I")
            {
                Vector3 position = base.transform.position;
                Vector3 up = Vector3.up;
                Vector3 position2 = base.transform.position;
                supply = PhotonNetwork.Instantiate("aot_supply", position - up * (position2.y - getHeight(base.transform.position)), base.transform.rotation, 0);
            }
        }
        else if (base.photonView.isMine && !hasAnnie)
        {
            if (Random.Range(0, 100) < 50)
            {
                int num = Random.Range(1, 2);
                for (int i = 0; i < num; i++)
                {
                    newTitan();
                }
            }
            if (isBase)
            {
                newTitan();
            }
        }
        if (titanPt == titanPtMax)
        {
            state = CheckPointState.Titan;
        }
        hitTestR = 15f * size;
        base.transform.localScale = new Vector3(size, size, size);
    }

    private void newTitan()
    {
        int rate = normalTitanRate;
        Vector3 position = base.transform.position;
        Vector3 up = Vector3.up;
        Vector3 position2 = base.transform.position;
        GameObject gameObject = fengGame.SpawnTitan(rate, position - up * (position2.y - getHeight(base.transform.position)), base.transform.rotation);
        if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName == "The City I")
        {
            gameObject.GetComponent<TITAN>().chaseDistance = 120f;
        }
        else
        {
            gameObject.GetComponent<TITAN>().chaseDistance = 200f;
        }
        gameObject.GetComponent<TITAN>().PVPfromCheckPt = this;
    }

    private float getHeight(Vector3 pt)
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask layerMask2 = layerMask;
        if (Physics.Raycast(pt, -Vector3.up, out RaycastHit hitInfo, 1000f, layerMask2.value))
        {
            Vector3 point = hitInfo.point;
            return point.y;
        }
        return 0f;
    }

    private void Update()
    {
        float num = humanPt / humanPtMax;
        float num2 = titanPt / titanPtMax;
        if (!base.photonView.isMine)
        {
            humanCyc.transform.localScale = new Vector3(num, num, 1f);
            titanCyc.transform.localScale = new Vector3(num2, num2, 1f);
            syncTimer += Time.deltaTime;
            if (syncTimer > syncInterval)
            {
                syncTimer = 0f;
                checkIfBeingCapture();
            }
            return;
        }
        switch (state)
        {
            case CheckPointState.Non:
                if (playerOn && !titanOn)
                {
                    humanGetsPoint();
                    titanLosePoint();
                }
                else if (titanOn && !playerOn)
                {
                    titanGetsPoint();
                    humanLosePoint();
                }
                else
                {
                    humanLosePoint();
                    titanLosePoint();
                }
                break;
            case CheckPointState.Human:
                if (titanOn && !playerOn)
                {
                    titanGetsPoint();
                }
                else
                {
                    titanLosePoint();
                }
                getPtsTimer += Time.deltaTime;
                if (getPtsTimer > getPtsInterval)
                {
                    getPtsTimer = 0f;
                    if (!isBase)
                    {
                        fengGame.PVPhumanScore++;
                    }
                    fengGame.CheckPvPPoints();
                }
                break;
            case CheckPointState.Titan:
                if (playerOn && !titanOn)
                {
                    humanGetsPoint();
                }
                else
                {
                    humanLosePoint();
                }
                getPtsTimer += Time.deltaTime;
                if (getPtsTimer > getPtsInterval)
                {
                    getPtsTimer = 0f;
                    if (!isBase)
                    {
                        fengGame.PVPtitanScore++;
                    }
                    fengGame.CheckPvPPoints();
                }
                spawnTitanTimer += Time.deltaTime;
                if (spawnTitanTimer > titanInterval)
                {
                    spawnTitanTimer = 0f;
                    if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName == "The City I")
                    {
                        if (GameObject.FindGameObjectsWithTag("titan").Length < 12)
                        {
                            newTitan();
                        }
                    }
                    else if (GameObject.FindGameObjectsWithTag("titan").Length < 20)
                    {
                        newTitan();
                    }
                }
                break;
        }
        syncTimer += Time.deltaTime;
        if (syncTimer > syncInterval)
        {
            syncTimer = 0f;
            checkIfBeingCapture();
            syncPts();
        }
        num = humanPt / humanPtMax;
        num2 = titanPt / titanPtMax;
        humanCyc.transform.localScale = new Vector3(num, num, 1f);
        titanCyc.transform.localScale = new Vector3(num2, num2, 1f);
    }

    private void checkIfBeingCapture()
    {
        playerOn = false;
        titanOn = false;
        GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] array2 = GameObject.FindGameObjectsWithTag("titan");
        int i;
        for (i = 0; i < array.Length; i++)
        {
            if (!(Vector3.Distance(array[i].transform.position, base.transform.position) < hitTestR))
            {
                continue;
            }
            playerOn = true;
            if (state == CheckPointState.Human && array[i].GetPhotonView().isMine)
            {
                if (fengGame.checkpoint != base.gameObject)
                {
                    fengGame.checkpoint = base.gameObject;
                    InRoomChat.Instance.AddLine("<color=#A8FF24>Respawn point changed to point" + id + "</color>");
                }
                break;
            }
        }
        i = 0;
        while (true)
        {
            if (i >= array2.Length)
            {
                return;
            }
            if (Vector3.Distance(array2[i].transform.position, base.transform.position) < hitTestR + 5f && (!array2[i].GetComponent<TITAN>() || !array2[i].GetComponent<TITAN>().hasDie))
            {
                titanOn = true;
                if (state == CheckPointState.Titan && array2[i].GetPhotonView().isMine && (bool)array2[i].GetComponent<TITAN>() && array2[i].GetComponent<TITAN>().nonAI)
                {
                    break;
                }
            }
            i++;
        }
        if (fengGame.checkpoint != base.gameObject)
        {
            fengGame.checkpoint = base.gameObject;
            InRoomChat.Instance.AddLine("<color=#A8FF24>Respawn point changed to point" + id + "</color>");
        }
    }

    private void humanGetsPoint()
    {
        if (humanPt >= humanPtMax)
        {
            humanPt = humanPtMax;
            titanPt = 0f;
            syncPts();
            state = CheckPointState.Human;
            base.photonView.RPC("changeState", PhotonTargets.All, 1);
            if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName != "The City I")
            {
                Vector3 position = base.transform.position;
                Vector3 up = Vector3.up;
                Vector3 position2 = base.transform.position;
                supply = PhotonNetwork.Instantiate("aot_supply", position - up * (position2.y - getHeight(base.transform.position)), base.transform.rotation, 0);
            }
            fengGame.PVPhumanScore += 2;
            fengGame.CheckPvPPoints();
            if (checkIfHumanWins())
            {
                fengGame.gameWin2();
            }
        }
        else
        {
            humanPt += Time.deltaTime;
        }
    }

    private void titanGetsPoint()
    {
        if (titanPt >= titanPtMax)
        {
            titanPt = titanPtMax;
            humanPt = 0f;
            syncPts();
            if (state == CheckPointState.Human && supply != null)
            {
                PhotonNetwork.Destroy(supply);
            }
            state = CheckPointState.Titan;
            base.photonView.RPC("changeState", PhotonTargets.All, 2);
            fengGame.PVPtitanScore += 2;
            fengGame.CheckPvPPoints();
            if (checkIfTitanWins())
            {
                fengGame.gameLose2();
            }
            if (hasAnnie)
            {
                if (!annie)
                {
                    annie = true;
                    Vector3 position = base.transform.position;
                    Vector3 up = Vector3.up;
                    Vector3 position2 = base.transform.position;
                    PhotonNetwork.Instantiate("FEMALE_TITAN", position - up * (position2.y - getHeight(base.transform.position)), base.transform.rotation, 0);
                }
                else
                {
                    newTitan();
                }
            }
            else
            {
                newTitan();
            }
        }
        else
        {
            titanPt += Time.deltaTime;
        }
    }

    private void titanLosePoint()
    {
        if (!(titanPt > 0f))
        {
            return;
        }
        titanPt -= Time.deltaTime * 3f;
        if (titanPt <= 0f)
        {
            titanPt = 0f;
            syncPts();
            if (state != CheckPointState.Human)
            {
                state = CheckPointState.Non;
                base.photonView.RPC("changeState", PhotonTargets.All, 0);
            }
        }
    }

    private void humanLosePoint()
    {
        if (!(humanPt > 0f))
        {
            return;
        }
        humanPt -= Time.deltaTime * 3f;
        if (humanPt <= 0f)
        {
            humanPt = 0f;
            syncPts();
            if (state != CheckPointState.Titan)
            {
                state = CheckPointState.Non;
                base.photonView.RPC("changeState", PhotonTargets.Others, 0);
            }
        }
    }

    private void syncPts()
    {
        base.photonView.RPC("changeTitanPt", PhotonTargets.Others, titanPt);
        base.photonView.RPC("changeHumanPt", PhotonTargets.Others, humanPt);
    }

    public string getStateString()
    {
        switch (state)
        {
            case CheckPointState.Human:
                return "[" + ColorSet.Human + "]H[-]";
            case CheckPointState.Titan:
                return "[" + ColorSet.TitanPlayer + "]T[-]";
            default:
                return "[" + ColorSet.D + "]_[-]";
        }
    }

    private bool checkIfHumanWins()
    {
        for (int i = 0; i < chkPts.Count; i++)
        {
            if ((chkPts[i] as PVPcheckPoint).state != CheckPointState.Human)
            {
                return false;
            }
        }
        return true;
    }

    private bool checkIfTitanWins()
    {
        for (int i = 0; i < chkPts.Count; i++)
        {
            if ((chkPts[i] as PVPcheckPoint).state != CheckPointState.Titan)
            {
                return false;
            }
        }
        return true;
    }

    [RPC]
    private void changeHumanPt(float pt)
    {
        humanPt = pt;
    }

    [RPC]
    private void changeTitanPt(float pt)
    {
        titanPt = pt;
    }

    [RPC]
    private void changeState(int num)
    {
        switch (num)
        {
            case 0:
                state = CheckPointState.Non;
                break;
            case 1:
                state = CheckPointState.Human;
                break;
            case 2:
                state = CheckPointState.Titan;
                break;
        }
    }
}
