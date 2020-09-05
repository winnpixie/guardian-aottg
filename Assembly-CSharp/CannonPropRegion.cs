using Photon;
using System;
using System.Collections;
using UnityEngine;

public class CannonPropRegion : Photon.MonoBehaviour
{
    public bool disabled;
    public bool destroyed;
    public string settings;
    public HERO storedHero;

    public void Start()
    {
        if ((int)FengGameManagerMKII.Settings[64] >= 100)
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        GameObject gameObject = collider.transform.root.gameObject;
        if (gameObject.layer != 8 || !gameObject.GetPhotonView().isMine)
        {
            return;
        }
        HERO component = gameObject.GetComponent<HERO>();
        if (component != null && !component.isCannon)
        {
            if (component.myCannonRegion != null)
            {
                component.myCannonRegion.storedHero = null;
            }
            component.myCannonRegion = this;
            storedHero = component;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        GameObject gameObject = collider.transform.root.gameObject;
        if (gameObject.layer == 8 && gameObject.GetPhotonView().isMine)
        {
            HERO component = gameObject.GetComponent<HERO>();
            if (component != null && storedHero != null && component == storedHero)
            {
                component.myCannonRegion = null;
                component.ClearPopup();
                storedHero = null;
            }
        }
    }

    [RPC]
    public void RequestControlRPC(int viewID, PhotonMessageInfo info)
    {
        if (base.photonView.isMine && PhotonNetwork.isMasterClient && !disabled)
        {
            HERO component = PhotonView.Find(viewID).gameObject.GetComponent<HERO>();
            if (component != null && component.photonView.owner == info.sender && !FengGameManagerMKII.Instance.allowedToCannon.ContainsKey(info.sender.Id))
            {
                disabled = true;
                StartCoroutine(WaitAndEnable());
                FengGameManagerMKII.Instance.allowedToCannon.Add(info.sender.Id, new CannonValues(base.photonView.viewID, settings));
                component.photonView.RPC("SpawnCannonRPC", info.sender, settings);
            }
        }
    }

    [RPC]
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
                    renderer.material = (Material)FengGameManagerMKII.RCAssets.Load(array[2]);
                    if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
                    {
                        renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(array[11]));
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

    public void OnDestroy()
    {
        if (storedHero != null)
        {
            storedHero.myCannonRegion = null;
            storedHero.ClearPopup();
        }
    }

    public IEnumerator WaitAndEnable()
    {
        yield return new WaitForSeconds(5f);
        if (!destroyed)
        {
            disabled = false;
        }
    }
}
