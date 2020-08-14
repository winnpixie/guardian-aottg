using Guardian;
using Photon;
using System;
using System.Collections;
using UnityEngine;

public class Bullet : Photon.MonoBehaviour
{
    private GameObject master;
    private Vector3 velocity = Vector3.zero;
    private Vector3 velocity2 = Vector3.zero;
    public GameObject rope;
    private LineRenderer lineRenderer;
    private ArrayList nodes = new ArrayList();
    private ArrayList spiralNodes;
    private int phase;
    private float killTime;
    private float killTime2;
    private bool left = true;
    public bool leviMode;
    public float leviShootTime;
    private GameObject myRef;
    private int spiralcount;
    private bool isdestroying;
    public TITAN myTitan;

    public void launch(Vector3 v, Vector3 v2, string launcher_ref, bool isLeft, GameObject hero, bool leviMode = false)
    {
        if (phase != 2)
        {
            master = hero;
            velocity = v;
            float f = Mathf.Acos(Vector3.Dot(v.normalized, v2.normalized)) * 57.29578f;
            if (Mathf.Abs(f) > 90f)
            {
                velocity2 = Vector3.zero;
            }
            else
            {
                velocity2 = Vector3.Project(v2, v);
            }
            if (launcher_ref == "hookRefL1")
            {
                myRef = hero.GetComponent<HERO>().hookRefL1;
            }
            if (launcher_ref == "hookRefL2")
            {
                myRef = hero.GetComponent<HERO>().hookRefL2;
            }
            if (launcher_ref == "hookRefR1")
            {
                myRef = hero.GetComponent<HERO>().hookRefR1;
            }
            if (launcher_ref == "hookRefR2")
            {
                myRef = hero.GetComponent<HERO>().hookRefR2;
            }
            nodes = new ArrayList();
            nodes.Add(myRef.transform.position);
            phase = 0;
            this.leviMode = leviMode;
            left = isLeft;
            if (IN_GAME_MAIN_CAMERA.Gametype != 0 && base.photonView.isMine)
            {
                base.photonView.RPC("myMasterIs", PhotonTargets.Others, hero.GetComponent<HERO>().photonView.viewID, launcher_ref);
                base.photonView.RPC("setVelocityAndLeft", PhotonTargets.Others, v, velocity2, left);
            }
            base.transform.position = myRef.transform.position;
            base.transform.rotation = Quaternion.LookRotation(v.normalized);
        }
    }

    [RPC]
    private void myMasterIs(int viewId, string launcherRef, PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.HookPatches.IsHookMasterSetValid(this, viewId, info))
        {
            master = PhotonView.Find(viewId).gameObject;
            if (launcherRef == "hookRefL1")
            {
                myRef = master.GetComponent<HERO>().hookRefL1;
            }
            if (launcherRef == "hookRefL2")
            {
                myRef = master.GetComponent<HERO>().hookRefL2;
            }
            if (launcherRef == "hookRefR1")
            {
                myRef = master.GetComponent<HERO>().hookRefR1;
            }
            if (launcherRef == "hookRefR2")
            {
                myRef = master.GetComponent<HERO>().hookRefR2;
            }
        }
    }

    [RPC]
    private void netLaunch(Vector3 newPosition)
    {
        nodes = new ArrayList();
        nodes.Add(newPosition);
    }

    [RPC]
    private void netUpdatePhase1(Vector3 newPosition, Vector3 masterPosition)
    {
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, newPosition);
        lineRenderer.SetPosition(1, masterPosition);
        base.transform.position = newPosition;
    }

    [RPC]
    private void netUpdateLeviSpiral(Vector3 newPosition, Vector3 masterPosition, Vector3 masterrotation)
    {
        phase = 2;
        leviMode = true;
        getSpiral(masterPosition, masterrotation);
        Vector3 b = masterPosition - (Vector3)spiralNodes[0];
        lineRenderer.SetVertexCount((int)((float)spiralNodes.Count - (float)spiralcount * 0.5f));
        for (int i = 0; (float)i <= (float)(spiralNodes.Count - 1) - (float)spiralcount * 0.5f; i++)
        {
            if (spiralcount < 5)
            {
                Vector3 vector = (Vector3)spiralNodes[i] + b;
                float num = (float)(spiralNodes.Count - 1) - (float)spiralcount * 0.5f;
                vector = new Vector3(vector.x, vector.y * ((num - (float)i) / num) + newPosition.y * ((float)i / num), vector.z);
                lineRenderer.SetPosition(i, vector);
            }
            else
            {
                lineRenderer.SetPosition(i, (Vector3)spiralNodes[i] + b);
            }
        }
    }

    public bool isHooked()
    {
        return phase == 1;
    }

    private void getSpiral(Vector3 masterposition, Vector3 masterrotation)
    {
        float num = 1.2f;
        float num2 = 30f;
        float num3 = 2f;
        float num4 = 0.5f;
        num = 30f;
        num3 = 0.05f + (float)spiralcount * 0.03f;
        if (spiralcount < 5)
        {
            Vector2 a = new Vector2(masterposition.x, masterposition.z);
            Vector3 position = base.gameObject.transform.position;
            float x = position.x;
            Vector3 position2 = base.gameObject.transform.position;
            float num5 = Vector2.Distance(a, new Vector2(x, position2.z));
            num = num5;
        }
        else
        {
            num = 1.2f + (float)(60 - spiralcount) * 0.1f;
        }
        num4 -= (float)spiralcount * 0.06f;
        float num6 = num / num2;
        float num7 = num3 / num2;
        float num8 = num7 * 2f * (float)Math.PI;
        num4 *= (float)Math.PI * 2f;
        spiralNodes = new ArrayList();
        for (int i = 1; (float)i <= num2; i++)
        {
            float num9 = (float)i * num6 * (1f + 0.05f * (float)i);
            float f = (float)i * num8 + num4 + (float)Math.PI * 2f / 5f + masterrotation.y * 0.0173f;
            float x2 = Mathf.Cos(f) * num9;
            float z = (0f - Mathf.Sin(f)) * num9;
            spiralNodes.Add(new Vector3(x2, 0f, z));
        }
    }

    private void setLinePhase0()
    {
        if (master == null)
        {
            UnityEngine.Object.Destroy(rope);
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else if (nodes.Count > 0)
        {
            Vector3 a = myRef.transform.position - (Vector3)nodes[0];
            lineRenderer.SetVertexCount(nodes.Count);
            for (int i = 0; i <= nodes.Count - 1; i++)
            {
                lineRenderer.SetPosition(i, (Vector3)nodes[i] + a * Mathf.Pow(0.75f, i));
            }
            if (nodes.Count > 1)
            {
                lineRenderer.SetPosition(1, myRef.transform.position);
            }
        }
    }

    [RPC]
    private void setPhase(int value)
    {
        phase = value;
    }

    [RPC]
    private void setVelocityAndLeft(Vector3 value, Vector3 v2, bool l)
    {
        velocity = value;
        velocity2 = v2;
        left = l;
        base.transform.rotation = Quaternion.LookRotation(value.normalized);
    }

    [RPC]
    private void tieMeTo(Vector3 p)
    {
        base.transform.position = p;
    }

    [RPC]
    private void tieMeToOBJ(int id, PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.HookPatches.IsHookTieValid(this, id, info))
        {
            base.transform.parent = PhotonView.Find(id).gameObject.transform;
        }
    }

    public void update()
    {
        if (master == null)
        {
            removeMe();
        }
        else
        {
            if (isdestroying)
            {
                return;
            }
            if (leviMode)
            {
                leviShootTime += Time.deltaTime;
                if (leviShootTime > 0.4f)
                {
                    phase = 2;
                    base.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            if (phase == 0)
            {
                setLinePhase0();
            }
            else if (phase == 1)
            {
                Vector3 a = base.transform.position - myRef.transform.position;
                Vector3 vector = base.transform.position + myRef.transform.position;
                Vector3 a2 = master.rigidbody.velocity;
                float magnitude = a2.magnitude;
                float magnitude2 = a.magnitude;
                int value = (int)((magnitude2 + magnitude) / 5f);
                value = Mathf.Clamp(value, 2, 6);
                lineRenderer.SetVertexCount(value);
                lineRenderer.SetPosition(0, myRef.transform.position);
                int i = 1;
                float num = Mathf.Pow(magnitude2, 0.3f);
                for (; i < value; i++)
                {
                    int num2 = value / 2;
                    float num3 = Mathf.Abs(i - num2);
                    float f = ((float)num2 - num3) / (float)num2;
                    f = Mathf.Pow(f, 0.5f);
                    float num4 = (num + magnitude) * 0.0015f * f;
                    lineRenderer.SetPosition(i, new Vector3(UnityEngine.Random.Range(0f - num4, num4), UnityEngine.Random.Range(0f - num4, num4), UnityEngine.Random.Range(0f - num4, num4)) + myRef.transform.position + a * ((float)i / (float)value) - Vector3.up * num * 0.05f * f - a2 * 0.001f * f * num);
                }
                lineRenderer.SetPosition(value - 1, base.transform.position);
            }
            else if (phase == 2)
            {
                if (leviMode)
                {
                    getSpiral(master.transform.position, master.transform.rotation.eulerAngles);
                    Vector3 b = myRef.transform.position - (Vector3)spiralNodes[0];
                    lineRenderer.SetVertexCount((int)((float)spiralNodes.Count - (float)spiralcount * 0.5f));
                    for (int j = 0; (float)j <= (float)(spiralNodes.Count - 1) - (float)spiralcount * 0.5f; j++)
                    {
                        if (spiralcount < 5)
                        {
                            Vector3 position = (Vector3)spiralNodes[j] + b;
                            float num5 = (float)(spiralNodes.Count - 1) - (float)spiralcount * 0.5f;
                            float x = position.x;
                            float num6 = position.y * ((num5 - (float)j) / num5);
                            Vector3 position2 = base.gameObject.transform.position;
                            position = new Vector3(x, num6 + position2.y * ((float)j / num5), position.z);
                            lineRenderer.SetPosition(j, position);
                        }
                        else
                        {
                            lineRenderer.SetPosition(j, (Vector3)spiralNodes[j] + b);
                        }
                    }
                }
                else
                {
                    lineRenderer.SetVertexCount(2);
                    lineRenderer.SetPosition(0, base.transform.position);
                    lineRenderer.SetPosition(1, myRef.transform.position);
                    killTime += Time.deltaTime * 0.2f;
                    lineRenderer.SetWidth(0.1f - killTime, 0.1f - killTime);
                    if (killTime > 0.1f)
                    {
                        removeMe();
                    }
                }
            }
            else
            {
                if (phase != 4)
                {
                    return;
                }
                base.gameObject.transform.position += velocity + velocity2 * Time.deltaTime;
                ArrayList arrayList = nodes;
                Vector3 position3 = base.gameObject.transform.position;
                float x2 = position3.x;
                Vector3 position4 = base.gameObject.transform.position;
                float y = position4.y;
                Vector3 position5 = base.gameObject.transform.position;
                arrayList.Add(new Vector3(x2, y, position5.z));
                Vector3 a3 = myRef.transform.position - (Vector3)nodes[0];
                for (int k = 0; k <= nodes.Count - 1; k++)
                {
                    lineRenderer.SetVertexCount(nodes.Count);
                    lineRenderer.SetPosition(k, (Vector3)nodes[k] + a3 * Mathf.Pow(0.5f, k));
                }
                killTime2 += Time.deltaTime;
                if (killTime2 > 0.8f)
                {
                    killTime += Time.deltaTime * 0.2f;
                    lineRenderer.SetWidth(0.1f - killTime, 0.1f - killTime);
                    if (killTime > 0.1f)
                    {
                        removeMe();
                    }
                }
            }
        }
    }

    public void disable()
    {
        phase = 2;
        killTime = 0f;
        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER)
        {
            base.photonView.RPC("setPhase", PhotonTargets.Others, 2);
        }
    }

    public void removeMe()
    {
        isdestroying = true;
        if (IN_GAME_MAIN_CAMERA.Gametype != 0 && base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.photonView);
            PhotonNetwork.RemoveRPCs(base.photonView);
        }
        else if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
        {
            UnityEngine.Object.Destroy(rope);
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    [RPC]
    private void killObject(PhotonMessageInfo info)
    {
        if (Guardian.AntiAbuse.HookPatches.IsKillObjectValid(info))
        {
            UnityEngine.Object.Destroy(rope);
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (FengGameManagerMKII.Instance != null)
        {
            FengGameManagerMKII.Instance.RemoveHook(this);
        }
        if (myTitan != null)
        {
            myTitan.isHooked = false;
        }
        UnityEngine.Object.Destroy(rope);
    }

    private void FixedUpdate()
    {
        if ((phase == 2 || phase == 1) && leviMode)
        {
            spiralcount++;
            if (spiralcount >= 60)
            {
                isdestroying = true;
                removeMe();
                return;
            }
        }
        if (IN_GAME_MAIN_CAMERA.Gametype != 0 && !base.photonView.isMine)
        {
            if (phase == 0)
            {
                base.gameObject.transform.position += velocity * Time.deltaTime * 50f + velocity2 * Time.deltaTime;
                nodes.Add(new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z));
            }
        }
        else
        {
            if (phase != 0)
            {
                return;
            }
            checkTitan();
            base.gameObject.transform.position += velocity * Time.deltaTime * 50f + velocity2 * Time.deltaTime;
            LayerMask mask = 1 << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask2 = 1 << LayerMask.NameToLayer("Ground");
            LayerMask mask3 = 1 << LayerMask.NameToLayer("NetworkObject");
            LayerMask layerMask = (int)mask | (int)mask2 | (int)mask3;
            bool flag = false;
            if ((nodes.Count <= 1) ? Physics.Linecast((Vector3)nodes[nodes.Count - 1], base.gameObject.transform.position, out RaycastHit hitInfo, layerMask.value) : Physics.Linecast((Vector3)nodes[nodes.Count - 2], base.gameObject.transform.position, out hitInfo, layerMask.value))
            {
                bool flag3 = true;
                if (hitInfo.collider.transform.gameObject.layer == LayerMask.NameToLayer("EnemyBox"))
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER)
                    {
                        object[] parameters = new object[1]
                        {
                            hitInfo.collider.transform.root.gameObject.GetPhotonView().viewID
                        };
                        base.photonView.RPC("tieMeToOBJ", PhotonTargets.Others, parameters);
                    }
                    master.GetComponent<HERO>().lastHook = hitInfo.collider.transform.root;
                    base.transform.parent = hitInfo.collider.transform;
                }
                else if (hitInfo.collider.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    master.GetComponent<HERO>().lastHook = null;
                }
                else if (hitInfo.collider.transform.gameObject.layer == LayerMask.NameToLayer("NetworkObject") && hitInfo.collider.transform.gameObject.tag == "Player" && !leviMode)
                {
                    if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER)
                    {
                        object[] parameters2 = new object[1]
                        {
                            hitInfo.collider.transform.root.gameObject.GetPhotonView().viewID
                        };
                        base.photonView.RPC("tieMeToOBJ", PhotonTargets.Others, parameters2);
                    }
                    master.GetComponent<HERO>().hookToHuman(hitInfo.collider.transform.root.gameObject, base.transform.position);
                    base.transform.parent = hitInfo.collider.transform;
                    master.GetComponent<HERO>().lastHook = null;
                }
                else
                {
                    flag3 = false;
                }
                if (phase == 2)
                {
                    flag3 = false;
                }
                if (flag3)
                {
                    master.GetComponent<HERO>().launch(hitInfo.point, left, leviMode);
                    base.transform.position = hitInfo.point;
                    if (phase != 2)
                    {
                        phase = 1;
                        if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER)
                        {
                            object[] parameters3 = new object[1]
                            {
                                1
                            };
                            base.photonView.RPC("setPhase", PhotonTargets.Others, parameters3);
                            object[] parameters4 = new object[1]
                            {
                                base.transform.position
                            };
                            base.photonView.RPC("tieMeTo", PhotonTargets.Others, parameters4);
                        }
                        if (leviMode)
                        {
                            getSpiral(master.transform.position, master.transform.rotation.eulerAngles);
                        }
                        flag = true;
                    }
                }
            }
            nodes.Add(new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z));
            if (flag)
            {
                return;
            }
            killTime2 += Time.deltaTime;
            if (killTime2 > 0.8f)
            {
                phase = 4;
                if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.MULTIPLAYER)
                {
                    object[] parameters5 = new object[1]
                    {
                        4
                    };
                    base.photonView.RPC("setPhase", PhotonTargets.Others, parameters5);
                }
            }
        }
    }

    public void checkTitan()
    {
        GameObject main_object = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
        if (!(main_object != null) || !(master != null) || !(master == main_object) || !Physics.Raycast(layerMask: ((LayerMask)(1 << LayerMask.NameToLayer("PlayerAttackBox"))).value, origin: base.transform.position, direction: velocity, hitInfo: out RaycastHit hitInfo, distance: 10f))
        {
            return;
        }
        Collider collider = hitInfo.collider;
        if (!collider.name.Contains("PlayerDetectorRC"))
        {
            return;
        }
        TITAN component = collider.transform.root.gameObject.GetComponent<TITAN>();
        if (component != null)
        {
            if (myTitan == null)
            {
                myTitan = component;
                myTitan.isHooked = true;
            }
            else if (myTitan != component)
            {
                myTitan.isHooked = false;
                myTitan = component;
                myTitan.isHooked = true;
            }
        }
    }

    private void Start()
    {
        rope = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("rope"));
        lineRenderer = rope.GetComponent<LineRenderer>();
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().AddHook(this);
    }
}
