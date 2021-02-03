using System.Collections;
using UnityEngine;

public class TITAN_SETUP : Photon.MonoBehaviour
{
    public GameObject eye;
    private GameObject part_hair;
    private CostumeHair hair;
    private int hairType;
    private GameObject hair_go_ref;
    public int skin;
    public bool haseye;

    private void Awake()
    {
        CostumeHair.Init();
        CharacterMaterials.InitData();
        HeroCostume.Init();
        hair_go_ref = new GameObject();
        eye.transform.parent = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").transform;
        hair_go_ref.transform.position = eye.transform.position + Vector3.up * 3.5f + base.transform.forward * 5.2f;
        hair_go_ref.transform.rotation = eye.transform.rotation;
        hair_go_ref.transform.RotateAround(eye.transform.position, base.transform.right, -20f);
        hair_go_ref.transform.localScale = new Vector3(210f, 210f, 210f);
        hair_go_ref.transform.parent = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").transform;
    }

    public void setFacialTexture(GameObject go, int id)
    {
        if (id >= 0)
        {
            float num = 0.25f;
            float num2 = 0.125f;
            float x = num2 * (float)(int)((float)id / 8f);
            float y = (0f - num) * (float)(id % 4);
            go.renderer.material.mainTextureOffset = new Vector2(x, y);
        }
    }

    public void setPunkHair()
    {
        Object.Destroy(part_hair);
        hair = CostumeHair.MaleHairs[3];
        hairType = 3;
        GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Character/" + hair.hair));
        gameObject.transform.parent = hair_go_ref.transform.parent;
        gameObject.transform.position = hair_go_ref.transform.position;
        gameObject.transform.rotation = hair_go_ref.transform.rotation;
        gameObject.transform.localScale = hair_go_ref.transform.localScale;
        gameObject.renderer.material = CharacterMaterials.materials[hair.texture];
        int num = Random.Range(1, 4);
        if (num == 1)
        {
            gameObject.renderer.material.color = FengColor.PunkHair1;
        }
        if (num == 2)
        {
            gameObject.renderer.material.color = FengColor.PunkHair2;
        }
        if (num == 3)
        {
            gameObject.renderer.material.color = FengColor.PunkHair3;
        }
        part_hair = gameObject;
        setFacialTexture(eye, 0);
        if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
        {
            PhotonView photonView = base.photonView;
            object[] obj = new object[5]
            {
                hairType,
                0,
                null,
                null,
                null
            };
            Color color = part_hair.renderer.material.color;
            obj[2] = color.r;
            obj[3] = color.g;
            obj[4] = color.b;
            photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, obj);
        }
    }

    [RPC]
    private void setHairPRC(int type, int eye_type, float c1, float c2, float c3)
    {
        Object.Destroy(part_hair);
        hair = CostumeHair.MaleHairs[type];
        hairType = type;
        if (hair.hair != string.Empty)
        {
            GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Character/" + hair.hair));
            gameObject.transform.parent = hair_go_ref.transform.parent;
            gameObject.transform.position = hair_go_ref.transform.position;
            gameObject.transform.rotation = hair_go_ref.transform.rotation;
            gameObject.transform.localScale = hair_go_ref.transform.localScale;
            gameObject.renderer.material = CharacterMaterials.materials[hair.texture];
            gameObject.renderer.material.color = new Color(c1, c2, c3);
            part_hair = gameObject;
        }
        setFacialTexture(eye, eye_type);
    }

    public void setHair2()
    {
        if ((int)FengGameManagerMKII.Settings[1] == 1 && (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine))
        {
            int num = Random.Range(0, 9);
            if (num == 3)
            {
                num = 9;
            }
            int num2 = skin - 70;
            if ((int)FengGameManagerMKII.Settings[32] == 1)
            {
                num2 = Random.Range(16, 20);
            }
            if ((int)FengGameManagerMKII.Settings[num2] >= 0)
            {
                num = (int)FengGameManagerMKII.Settings[num2];
            }
            string text = (string)FengGameManagerMKII.Settings[num2 + 5];
            int num3 = Random.Range(1, 8);
            if (haseye)
            {
                num3 = 0;
            }
            bool flag = false;
            if (text.EndsWith(".jpg") || text.EndsWith(".png") || text.EndsWith(".jpeg"))
            {
                flag = true;
            }
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
            {
                if (flag)
                {
                    object[] parameters = new object[3]
                    {
                        num,
                        num3,
                        text
                    };
                    base.photonView.RPC("setHairRPC2", PhotonTargets.AllBuffered, parameters);
                }
                else
                {
                    Color hair_color = HeroCostume.Costumes[Random.Range(0, HeroCostume.Costumes.Length - 5)].hair_color;
                    object[] parameters = new object[5]
                    {
                        num,
                        num3,
                        hair_color.r,
                        hair_color.g,
                        hair_color.b
                    };
                    base.photonView.RPC("setHairPRC", PhotonTargets.AllBuffered, parameters);
                }
            }
            else if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                if (flag)
                {
                    StartCoroutine(loadskinE(num, num3, text));
                    return;
                }
                Color hair_color = HeroCostume.Costumes[Random.Range(0, HeroCostume.Costumes.Length - 5)].hair_color;
                setHairPRC(num, num3, hair_color.r, hair_color.g, hair_color.b);
            }
        }
        else
        {
            int num = Random.Range(0, CostumeHair.MaleHairs.Length);
            if (num == 3)
            {
                num = 9;
            }
            Object.Destroy(part_hair);
            hairType = num;
            hair = CostumeHair.MaleHairs[num];
            if (hair.hair == string.Empty)
            {
                hair = CostumeHair.MaleHairs[9];
                hairType = 9;
            }
            part_hair = (GameObject)Object.Instantiate(Resources.Load("Character/" + hair.hair));
            part_hair.transform.parent = hair_go_ref.transform.parent;
            part_hair.transform.position = hair_go_ref.transform.position;
            part_hair.transform.rotation = hair_go_ref.transform.rotation;
            part_hair.transform.localScale = hair_go_ref.transform.localScale;
            part_hair.renderer.material = CharacterMaterials.materials[hair.texture];
            part_hair.renderer.material.color = HeroCostume.Costumes[Random.Range(0, HeroCostume.Costumes.Length - 5)].hair_color;
            int num4 = Random.Range(1, 8);
            setFacialTexture(eye, num4);
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
            {
                object[] parameters = new object[5]
                {
                    hairType,
                    num4,
                    part_hair.renderer.material.color.r,
                    part_hair.renderer.material.color.g,
                    part_hair.renderer.material.color.b
                };
                base.photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, parameters);
            }
        }
    }

    [RPC]
    public void setHairRPC2(int hair, int eye, string hairlink)
    {
        if ((int)FengGameManagerMKII.Settings[1] == 1)
        {
            StartCoroutine(loadskinE(hair, eye, hairlink));
        }
    }

    [RPC]
    public IEnumerator loadskinE(int hair, int eye, string hairlink)
    {
        bool unload = false;
        Object.Destroy(part_hair);
        this.hair = CostumeHair.MaleHairs[hair];
        hairType = hair;
        if (this.hair.hair != string.Empty)
        {
            GameObject obj2 = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
            obj2.transform.parent = hair_go_ref.transform.parent;
            obj2.transform.position = hair_go_ref.transform.position;
            obj2.transform.rotation = hair_go_ref.transform.rotation;
            obj2.transform.localScale = hair_go_ref.transform.localScale;
            obj2.renderer.material = CharacterMaterials.materials[this.hair.texture];
            bool flag = true;
            if ((int)FengGameManagerMKII.Settings[63] == 1)
            {
                flag = false;
            }
            if (hairlink.EndsWith(".jpg") || hairlink.EndsWith(".png") || hairlink.EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(hairlink))
                {
                    WWW link = Guardian.Utilities.GameHelper.CreateWWW(hairlink);
                    if (link != null)
                    {
                        yield return link;
                        Texture2D tex = RCextensions.LoadImage(link, flag, 200000);
                        link.Dispose();
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(hairlink))
                        {
                            unload = true;
                            obj2.renderer.material.mainTexture = tex;
                            FengGameManagerMKII.LinkHash[0].Add(hairlink, obj2.renderer.material);
                            obj2.renderer.material = (Material)FengGameManagerMKII.LinkHash[0][hairlink];
                        }
                        else
                        {
                            obj2.renderer.material = (Material)FengGameManagerMKII.LinkHash[0][hairlink];
                        }
                    }
                }
                else
                {
                    obj2.renderer.material = (Material)FengGameManagerMKII.LinkHash[0][hairlink];
                }
            }
            else if (hairlink.ToLower() == "transparent")
            {
                obj2.renderer.enabled = false;
            }
            part_hair = obj2;
        }
        if (eye >= 0)
        {
            setFacialTexture(this.eye, eye);
        }
        if (unload)
        {
            FengGameManagerMKII.Instance.UnloadAssets();
        }
    }

    public void setVar(int skin, bool haseye)
    {
        this.skin = skin;
        this.haseye = haseye;
    }
}
