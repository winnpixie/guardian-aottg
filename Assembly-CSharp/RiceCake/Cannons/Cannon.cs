using System;
using UnityEngine;

public class Cannon : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;
    private Quaternion correctBarrelRot = Quaternion.identity;
    public float SmoothingDelay = 5f;
    public Transform firingPoint;
    public Transform ballPoint;
    public Transform barrel;
    public GameObject myCannonBall;
    public LineRenderer myCannonLine;
    public float currentRot = 0f;
    public HERO myHero;
    public string settings;
    public bool isCannonGround;

    public void Awake()
    {
        if (base.photonView == null)
        {
            return;
        }
        base.photonView.observed = this;
        barrel = base.transform.Find("Barrel");
        correctPlayerPos = base.transform.position;
        correctPlayerRot = base.transform.rotation;
        correctBarrelRot = barrel.rotation;
        if (base.photonView.isMine)
        {
            firingPoint = barrel.Find("FiringPoint");
            ballPoint = barrel.Find("BallPoint");
            myCannonLine = ballPoint.GetComponent<LineRenderer>();
            if (base.gameObject.name.Contains("CannonGround"))
            {
                isCannonGround = true;
            }
        }
        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }
        PhotonPlayer owner = base.photonView.owner;
        if (FengGameManagerMKII.Instance.allowedToCannon.ContainsKey(owner.Id))
        {
            settings = FengGameManagerMKII.Instance.allowedToCannon[owner.Id].Settings;
            base.photonView.RPC("SetSize", PhotonTargets.All, settings);
            int viewID = FengGameManagerMKII.Instance.allowedToCannon[owner.Id].ViewId;
            FengGameManagerMKII.Instance.allowedToCannon.Remove(owner.Id);
            CannonPropRegion component = PhotonView.Find(viewID).gameObject.GetComponent<CannonPropRegion>();
            if (component != null)
            {
                component.disabled = true;
                component.destroyed = true;
                PhotonNetwork.Destroy(component.gameObject);
            }
        }
        else if (!owner.isLocal && !FengGameManagerMKII.Instance.restartingMC)
        {
            FengGameManagerMKII.Instance.KickPlayer(owner, ban: false, "spawning cannon without request.");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(base.transform.position);
            stream.SendNext(base.transform.rotation);
            stream.SendNext(barrel.rotation);
        }
        else
        {
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
            correctBarrelRot = (Quaternion)stream.ReceiveNext();
        }
    }

    [Guardian.Networking.RPC]
    public void SetSize(string settings, PhotonMessageInfo info)
    {
        if (!info.sender.isMasterClient)
        {
            return;
        }
        string[] array = settings.Split(',');
        if (array.Length <= 15)
        {
            return;
        }
        float a = 1f;
        if (array[2] != "default")
        {
            if (array[2].StartsWith("transparent"))
            {
                if (float.TryParse(array[2].Substring(11), out float result))
                {
                    a = result;
                }
                Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in componentsInChildren)
                {
                    renderer.material = (Material)FengGameManagerMKII.RCAssets.Load("transparent");
                    if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
                    {
                        renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(array[11]));
                    }
                }
            }
            else
            {
                Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in componentsInChildren)
                {
                    if (!renderer.name.Contains("Line Renderer"))
                    {
                        renderer.material = (Material)FengGameManagerMKII.RCAssets.Load(array[2]);
                        if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
                        {
                            renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(array[11]));
                        }
                    }
                }
            }
        }
        float num = gameObject.transform.localScale.x * Convert.ToSingle(array[3]);
        num -= 0.001f;
        float y = gameObject.transform.localScale.y * Convert.ToSingle(array[4]);
        float z = gameObject.transform.localScale.z * Convert.ToSingle(array[5]);
        gameObject.transform.localScale = new Vector3(num, y, z);
        if (!(array[6] != "0"))
        {
            return;
        }
        Color color = new Color(Convert.ToSingle(array[7]), Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), a);
        MeshFilter[] componentsInChildren2 = gameObject.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter meshFilter in componentsInChildren2)
        {
            Mesh mesh = meshFilter.mesh;
            Color[] array2 = new Color[mesh.vertexCount];
            for (int j = 0; j < mesh.vertexCount; j++)
            {
                array2[j] = color;
            }
            mesh.colors = array2;
        }
    }

    public void Update()
    {
        if (!base.photonView.isMine)
        {
            base.transform.position = Vector3.Lerp(base.transform.position, correctPlayerPos, Time.deltaTime * SmoothingDelay);
            base.transform.rotation = Quaternion.Lerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * SmoothingDelay);
            barrel.rotation = Quaternion.Lerp(barrel.rotation, correctBarrelRot, Time.deltaTime * SmoothingDelay);
            return;
        }
        Vector3 a = new Vector3(0f, -30f, 0f);
        Vector3 position = ballPoint.position;
        Vector3 a2 = ballPoint.forward * 300f;
        float d = 40f / a2.magnitude;
        myCannonLine.SetWidth(0.5f, 40f);
        myCannonLine.SetVertexCount(100);
        for (int i = 0; i < 100; i++)
        {
            myCannonLine.SetPosition(i, position);
            position += a2 * d + 0.5f * a * d * d;
            a2 += a * d;
        }
        float num = 30f;
        if (FengGameManagerMKII.InputRC.IsInputCannon(InputCodeRC.CannonSlow))
        {
            num = 5f;
        }
        if (isCannonGround)
        {
            if (FengGameManagerMKII.InputRC.IsInputCannon(InputCodeRC.CannonForward))
            {
                if (currentRot <= 32f)
                {
                    currentRot += Time.deltaTime * num;
                    barrel.Rotate(new Vector3(0f, 0f, Time.deltaTime * num));
                }
            }
            else if (FengGameManagerMKII.InputRC.IsInputCannon(InputCodeRC.CannonBack) && currentRot >= -18f)
            {
                currentRot += Time.deltaTime * (0f - num);
                barrel.Rotate(new Vector3(0f, 0f, Time.deltaTime * (0f - num)));
            }
            if (FengGameManagerMKII.InputRC.IsInputCannon(InputCodeRC.CannonLeft))
            {
                base.transform.Rotate(new Vector3(0f, Time.deltaTime * (0f - num), 0f));
            }
            else if (FengGameManagerMKII.InputRC.IsInputCannon(InputCodeRC.CannonRight))
            {
                base.transform.Rotate(new Vector3(0f, Time.deltaTime * num, 0f));
            }
        }
        else
        {
            if (FengGameManagerMKII.InputRC.IsInputCannon(InputCodeRC.CannonForward))
            {
                if (currentRot >= -50f)
                {
                    currentRot += Time.deltaTime * (0f - num);
                    barrel.Rotate(new Vector3(Time.deltaTime * (0f - num), 0f, 0f));
                }
            }
            else if (FengGameManagerMKII.InputRC.IsInputCannon(InputCodeRC.CannonBack) && currentRot <= 40f)
            {
                currentRot += Time.deltaTime * num;
                barrel.Rotate(new Vector3(Time.deltaTime * num, 0f, 0f));
            }
            if (FengGameManagerMKII.InputRC.IsInputCannon(InputCodeRC.CannonLeft))
            {
                base.transform.Rotate(new Vector3(0f, Time.deltaTime * (0f - num), 0f));
            }
            else if (FengGameManagerMKII.InputRC.IsInputCannon(InputCodeRC.CannonRight))
            {
                base.transform.Rotate(new Vector3(0f, Time.deltaTime * num, 0f));
            }
        }
        if (FengGameManagerMKII.InputRC.IsInputCannon(InputCodeRC.CannonFire))
        {
            Fire();
        }
        else if (FengGameManagerMKII.InputRC.IsInputCannonDown(InputCodeRC.CannonMount))
        {
            if (myHero != null)
            {
                myHero.isCannon = false;
                myHero.myCannonRegion = null;
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(myHero.gameObject);
                myHero.baseRigidBody.velocity = Vector3.zero;
                myHero.photonView.RPC("ReturnFromCannon", PhotonTargets.Others);
                myHero.skillCDLast = myHero.skillCDLastCannon;
                myHero.skillCDDuration = myHero.skillCDLast;
            }
            PhotonNetwork.Destroy(base.gameObject);
        }
    }

    public void Fire()
    {
        if (myHero.skillCDDuration <= 0f)
        {
            GameObject gameObject = PhotonNetwork.Instantiate("FX/boom2", firingPoint.position, firingPoint.rotation, 0);
            EnemyCheckCollider[] componentsInChildren = gameObject.GetComponentsInChildren<EnemyCheckCollider>();
            foreach (EnemyCheckCollider enemyCheckCollider in componentsInChildren)
            {
                enemyCheckCollider.dmg = 0;
            }
            myCannonBall = PhotonNetwork.Instantiate("RCAsset/CannonBallObject", ballPoint.position, firingPoint.rotation, 0);
            myCannonBall.rigidbody.velocity = firingPoint.forward * 300f;
            myCannonBall.GetComponent<CannonBall>().myHero = myHero;
            myHero.skillCDDuration = 3.5f;
        }
    }

    public void OnDestroy()
    {
        if (!PhotonNetwork.isMasterClient || FengGameManagerMKII.Instance.isRestarting)
        {
            return;
        }
        string[] array = settings.Split(',');
        if (array[0] == "photon")
        {
            if (array.Length > 15)
            {
                GameObject gameObject = PhotonNetwork.Instantiate("RCAsset/" + array[1] + "Prop", new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])), 0);
                gameObject.GetComponent<CannonPropRegion>().settings = settings;
                gameObject.GetPhotonView().RPC("SetSize", PhotonTargets.AllBuffered, settings);
            }
            else
            {
                GameObject gameObject = PhotonNetwork.Instantiate("RCAsset/" + array[1] + "Prop", new Vector3(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]), Convert.ToSingle(array[4])), new Quaternion(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8])), 0);
                gameObject.GetComponent<CannonPropRegion>().settings = settings;
            }
        }
    }
}
