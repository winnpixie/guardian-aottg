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

    public void SetEyeTexture(GameObject eyeGo, int eyeIndex)
    {
        if (eyeIndex >= 0)
        {
            float num = 0.25f;
            float num2 = 0.125f;
            float x = num2 * (float)(int)((float)eyeIndex / 8f);
            float y = (0f - num) * (float)(eyeIndex % 4);
            eyeGo.renderer.material.mainTextureOffset = new Vector2(x, y);
        }
    }

    public void SetPunkHair2()
    {
        if ((int)FengGameManagerMKII.Settings[1] == 1 && (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine))
        {
            int hairIndex = Random.Range(0, 9);
            if (hairIndex == 3)
            {
                hairIndex = 9;
            }
            int num2 = skin - 70;
            if ((int)FengGameManagerMKII.Settings[32] == 1)
            {
                num2 = Random.Range(16, 20);
            }
            if ((int)FengGameManagerMKII.Settings[num2] >= 0)
            {
                hairIndex = (int)FengGameManagerMKII.Settings[num2];
            }
            string text = (string)FengGameManagerMKII.Settings[num2 + 5];
            int eyeIndex = Random.Range(1, 8);
            if (haseye)
            {
                eyeIndex = 0;
            }
            bool isSkinned = false;
            if (text.EndsWith(".jpg") || text.EndsWith(".png") || text.EndsWith(".jpeg"))
            {
                isSkinned = true;
            }
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
            {
                if (isSkinned)
                {
                    base.photonView.RPC("setHairRPC2", PhotonTargets.AllBuffered, hairIndex, eyeIndex, text);
                }
                else
                {
                    Color hair_color = HeroCostume.Costumes[Random.Range(0, HeroCostume.Costumes.Length - 5)].hair_color;
                    base.photonView.RPC("setHairPRC", PhotonTargets.AllBuffered, hairIndex, eyeIndex, hair_color.r, hair_color.g, hair_color.b);
                }
            }
            else if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                if (isSkinned)
                {
                    StartCoroutine(loadskinE(hairIndex, eyeIndex, text));
                    return;
                }
                Color hair_color = HeroCostume.Costumes[Random.Range(0, HeroCostume.Costumes.Length - 5)].hair_color;
                setHairPRC(hairIndex, eyeIndex, hair_color.r, hair_color.g, hair_color.b);
            }
        }
        else
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
            int hairColorIndex = Random.Range(1, 4);
            switch(hairColorIndex)
            {
                case 1:
                    gameObject.renderer.material.color = FengColor.PunkHair1;
                    break;
                case 2:
                    gameObject.renderer.material.color = FengColor.PunkHair2;
                    break;
                case 3:
                    gameObject.renderer.material.color = FengColor.PunkHair3;
                    break;
            }
            part_hair = gameObject;
            SetEyeTexture(eye, 0);
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
            {
                Color color = part_hair.renderer.material.color;
                base.photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, hairType, 0, color.r, color.g, color.b);
            }
        }
    }

    [RPC]
    private void setHairPRC(int hairIndex, int eyeIndex, float hairR, float hairG, float hairB)
    {
        Object.Destroy(part_hair);
        hair = CostumeHair.MaleHairs[hairIndex];
        this.hairType = hairIndex;
        if (hair.hair.Length > 0)
        {
            GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Character/" + hair.hair));
            gameObject.transform.parent = hair_go_ref.transform.parent;
            gameObject.transform.position = hair_go_ref.transform.position;
            gameObject.transform.rotation = hair_go_ref.transform.rotation;
            gameObject.transform.localScale = hair_go_ref.transform.localScale;
            gameObject.renderer.material = CharacterMaterials.materials[hair.texture];
            gameObject.renderer.material.color = new Color(hairR, hairG, hairB);
            part_hair = gameObject;
        }
        SetEyeTexture(eye, eyeIndex);
    }

    public void SetHair2()
    {
        if ((int)FengGameManagerMKII.Settings[1] == 1 && (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer || base.photonView.isMine))
        {
            int hairIndex = Random.Range(0, 9);
            if (hairIndex == 3)
            {
                hairIndex = 9;
            }
            int num2 = skin - 70;
            if ((int)FengGameManagerMKII.Settings[32] == 1)
            {
                num2 = Random.Range(16, 20);
            }
            if ((int)FengGameManagerMKII.Settings[num2] >= 0)
            {
                hairIndex = (int)FengGameManagerMKII.Settings[num2];
            }
            string text = (string)FengGameManagerMKII.Settings[num2 + 5];
            int eyeIndex = Random.Range(1, 8);
            if (haseye)
            {
                eyeIndex = 0;
            }
            bool isSkinned = false;
            if (text.EndsWith(".jpg") || text.EndsWith(".png") || text.EndsWith(".jpeg"))
            {
                isSkinned = true;
            }
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
            {
                if (isSkinned)
                {
                    base.photonView.RPC("setHairRPC2", PhotonTargets.AllBuffered, hairIndex, eyeIndex, text);
                }
                else
                {
                    Color hair_color = HeroCostume.Costumes[Random.Range(0, HeroCostume.Costumes.Length - 5)].hair_color;
                    base.photonView.RPC("setHairPRC", PhotonTargets.AllBuffered, hairIndex, eyeIndex, hair_color.r, hair_color.g, hair_color.b);
                }
            }
            else if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
            {
                if (isSkinned)
                {
                    StartCoroutine(loadskinE(hairIndex, eyeIndex, text));
                    return;
                }
                Color hair_color = HeroCostume.Costumes[Random.Range(0, HeroCostume.Costumes.Length - 5)].hair_color;
                setHairPRC(hairIndex, eyeIndex, hair_color.r, hair_color.g, hair_color.b);
            }
        }
        else
        {
            int hairIndex = Random.Range(0, CostumeHair.MaleHairs.Length);
            if (hairIndex == 3)
            {
                hairIndex = 9;
            }
            Object.Destroy(part_hair);
            this.hairType = hairIndex;
            hair = CostumeHair.MaleHairs[hairIndex];
            if (hair.hair == string.Empty)
            {
                hair = CostumeHair.MaleHairs[9];
                this.hairType = 9;
            }
            part_hair = (GameObject)Object.Instantiate(Resources.Load("Character/" + hair.hair));
            part_hair.transform.parent = hair_go_ref.transform.parent;
            part_hair.transform.position = hair_go_ref.transform.position;
            part_hair.transform.rotation = hair_go_ref.transform.rotation;
            part_hair.transform.localScale = hair_go_ref.transform.localScale;
            part_hair.renderer.material = CharacterMaterials.materials[hair.texture];
            part_hair.renderer.material.color = HeroCostume.Costumes[Random.Range(0, HeroCostume.Costumes.Length - 5)].hair_color;
            int eyeIndex = Random.Range(1, 8);
            SetEyeTexture(eye, eyeIndex);
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer && base.photonView.isMine)
            {
                object[] parameters = new object[5]
                {
                    this.hairType,
                    eyeIndex,
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

    public IEnumerator loadskinE(int hairIndex, int eyeIndex, string hairLink)
    {
        bool unload = false;
        Object.Destroy(part_hair);
        this.hair = CostumeHair.MaleHairs[hairIndex];
        hairType = hairIndex;
        if (this.hair.hair.Length > 0)
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
            if (hairLink.EndsWith(".jpg") || hairLink.EndsWith(".png") || hairLink.EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.LinkHash[0].ContainsKey(hairLink))
                {
                    WWW link = Guardian.Utilities.GameHelper.CreateWWW(hairLink);
                    if (link != null)
                    {
                        yield return link;

                        // TODO: Old limit: 200KB
                        Texture2D tex = RCextensions.LoadImage(link, flag, 300000);
                        link.Dispose();
                        if (!FengGameManagerMKII.LinkHash[0].ContainsKey(hairLink))
                        {
                            unload = true;
                            obj2.renderer.material.mainTexture = tex;
                            FengGameManagerMKII.LinkHash[0].Add(hairLink, obj2.renderer.material);
                        }
                        obj2.renderer.material = (Material)FengGameManagerMKII.LinkHash[0][hairLink];
                    }
                }
                else
                {
                    obj2.renderer.material = (Material)FengGameManagerMKII.LinkHash[0][hairLink];
                }
            }
            else if (hairLink.ToLower() == "transparent")
            {
                obj2.renderer.enabled = false;
            }
            part_hair = obj2;
        }
        if (eyeIndex >= 0)
        {
            SetEyeTexture(this.eye, eyeIndex);
        }
        if (unload)
        {
            FengGameManagerMKII.Instance.UnloadAssets();
        }
    }

    public void SetVar(int skin, bool hasEyes)
    {
        this.skin = skin;
        this.haseye = hasEyes;
    }
}
