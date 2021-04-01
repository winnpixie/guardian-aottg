using UnityEngine;
using Xft;

public class HERO_SETUP : MonoBehaviour
{
    public GameObject part_3dmg;
    public GameObject part_3dmg_belt;
    public GameObject part_3dmg_gas_l;
    public GameObject part_3dmg_gas_r;
    public GameObject part_blade_l;
    public GameObject part_blade_r;
    public GameObject part_chest;
    public GameObject part_hand_l;
    public GameObject part_hand_r;
    public GameObject part_head;
    public GameObject part_leg;
    public GameObject part_upper_body;
    public GameObject part_arm_l;
    public GameObject part_arm_r;
    public GameObject part_face;
    public GameObject part_hair;
    public GameObject part_hair_1;
    public GameObject part_hair_2;
    public GameObject part_eye;
    public GameObject part_glass;
    public GameObject part_cape;
    public GameObject part_asset_1;
    public GameObject part_asset_2;
    public GameObject part_brand_1;
    public GameObject part_brand_2;
    public GameObject part_brand_3;
    public GameObject part_brand_4;
    public GameObject part_chest_1;
    public GameObject part_chest_2;
    public GameObject part_chest_3;
    public GameObject chest_info;
    public GameObject reference;
    public HeroCostume myCostume;
    private GameObject mount_3dmg;
    private GameObject mount_3dmg_gas_l;
    private GameObject mount_3dmg_gas_r;
    private GameObject mount_3dmg_gun_mag_l;
    private GameObject mount_3dmg_gun_mag_r;
    private GameObject mount_weapon_l;
    private GameObject mount_weapon_r;
    public bool isDeadBody;

    private void Awake()
    {
        part_head.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
        mount_3dmg = new GameObject();
        mount_3dmg_gas_l = new GameObject();
        mount_3dmg_gas_r = new GameObject();
        mount_3dmg_gun_mag_l = new GameObject();
        mount_3dmg_gun_mag_r = new GameObject();
        mount_weapon_l = new GameObject();
        mount_weapon_r = new GameObject();
        mount_3dmg.transform.position = base.transform.position;
        Transform transform = mount_3dmg.transform;
        Vector3 eulerAngles = base.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(270f, eulerAngles.y, 0f);
        mount_3dmg.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
        mount_3dmg_gas_l.transform.position = base.transform.position;
        Transform transform2 = mount_3dmg_gas_l.transform;
        Vector3 eulerAngles2 = base.transform.rotation.eulerAngles;
        transform2.rotation = Quaternion.Euler(270f, eulerAngles2.y, 0f);
        mount_3dmg_gas_l.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine").transform;
        mount_3dmg_gas_r.transform.position = base.transform.position;
        Transform transform3 = mount_3dmg_gas_r.transform;
        Vector3 eulerAngles3 = base.transform.rotation.eulerAngles;
        transform3.rotation = Quaternion.Euler(270f, eulerAngles3.y, 0f);
        mount_3dmg_gas_r.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine").transform;
        mount_3dmg_gun_mag_l.transform.position = base.transform.position;
        Transform transform4 = mount_3dmg_gun_mag_l.transform;
        Vector3 eulerAngles4 = base.transform.rotation.eulerAngles;
        transform4.rotation = Quaternion.Euler(270f, eulerAngles4.y, 0f);
        mount_3dmg_gun_mag_l.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/thigh_L").transform;
        mount_3dmg_gun_mag_r.transform.position = base.transform.position;
        Transform transform5 = mount_3dmg_gun_mag_r.transform;
        Vector3 eulerAngles5 = base.transform.rotation.eulerAngles;
        transform5.rotation = Quaternion.Euler(270f, eulerAngles5.y, 0f);
        mount_3dmg_gun_mag_r.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/thigh_R").transform;
        mount_weapon_l.transform.position = base.transform.position;
        Transform transform6 = mount_weapon_l.transform;
        Vector3 eulerAngles6 = base.transform.rotation.eulerAngles;
        transform6.rotation = Quaternion.Euler(270f, eulerAngles6.y, 0f);
        mount_weapon_l.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L").transform;
        mount_weapon_r.transform.position = base.transform.position;
        Transform transform7 = mount_weapon_r.transform;
        Vector3 eulerAngles7 = base.transform.rotation.eulerAngles;
        transform7.rotation = Quaternion.Euler(270f, eulerAngles7.y, 0f);
        mount_weapon_r.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R").transform;
    }

    public void Init()
    {
        CharacterMaterials.InitData();
    }

    public void Create3DMG()
    {
        Object.Destroy(part_3dmg);
        Object.Destroy(part_3dmg_belt);
        Object.Destroy(part_3dmg_gas_l);
        Object.Destroy(part_3dmg_gas_r);
        Object.Destroy(part_blade_l);
        Object.Destroy(part_blade_r);
        if (myCostume.mesh_3dmg.Length > 0)
        {
            part_3dmg = (GameObject)Object.Instantiate(Resources.Load("Character/" + myCostume.mesh_3dmg));
            part_3dmg.transform.position = mount_3dmg.transform.position;
            part_3dmg.transform.rotation = mount_3dmg.transform.rotation;
            part_3dmg.transform.parent = mount_3dmg.transform.parent;
            part_3dmg.renderer.material = CharacterMaterials.materials[myCostume._3dmg_texture];
        }
        if (myCostume.mesh_3dmg_belt.Length > 0)
        {
            part_3dmg_belt = GenerateCloth(reference, "Character/" + myCostume.mesh_3dmg_belt);
            part_3dmg_belt.renderer.material = CharacterMaterials.materials[myCostume._3dmg_texture];
        }
        if (myCostume.mesh_3dmg_gas_l.Length > 0)
        {
            part_3dmg_gas_l = (GameObject)Object.Instantiate(Resources.Load("Character/" + myCostume.mesh_3dmg_gas_l));
            if (myCostume.uniform_type != UNIFORM_TYPE.CasualAHSS)
            {
                part_3dmg_gas_l.transform.position = mount_3dmg_gas_l.transform.position;
                part_3dmg_gas_l.transform.rotation = mount_3dmg_gas_l.transform.rotation;
                part_3dmg_gas_l.transform.parent = mount_3dmg_gas_l.transform.parent;
            }
            else
            {
                part_3dmg_gas_l.transform.position = mount_3dmg_gun_mag_l.transform.position;
                part_3dmg_gas_l.transform.rotation = mount_3dmg_gun_mag_l.transform.rotation;
                part_3dmg_gas_l.transform.parent = mount_3dmg_gun_mag_l.transform.parent;
            }
            part_3dmg_gas_l.renderer.material = CharacterMaterials.materials[myCostume._3dmg_texture];
        }
        if (myCostume.mesh_3dmg_gas_r.Length > 0)
        {
            part_3dmg_gas_r = (GameObject)Object.Instantiate(Resources.Load("Character/" + myCostume.mesh_3dmg_gas_r));
            if (myCostume.uniform_type != UNIFORM_TYPE.CasualAHSS)
            {
                part_3dmg_gas_r.transform.position = mount_3dmg_gas_r.transform.position;
                part_3dmg_gas_r.transform.rotation = mount_3dmg_gas_r.transform.rotation;
                part_3dmg_gas_r.transform.parent = mount_3dmg_gas_r.transform.parent;
            }
            else
            {
                part_3dmg_gas_r.transform.position = mount_3dmg_gun_mag_r.transform.position;
                part_3dmg_gas_r.transform.rotation = mount_3dmg_gun_mag_r.transform.rotation;
                part_3dmg_gas_r.transform.parent = mount_3dmg_gun_mag_r.transform.parent;
            }
            part_3dmg_gas_r.renderer.material = CharacterMaterials.materials[myCostume._3dmg_texture];
        }
        if (myCostume.weapon_l_mesh.Length > 0)
        {
            part_blade_l = (GameObject)Object.Instantiate(Resources.Load("Character/" + myCostume.weapon_l_mesh));
            part_blade_l.transform.position = mount_weapon_l.transform.position;
            part_blade_l.transform.rotation = mount_weapon_l.transform.rotation;
            part_blade_l.transform.parent = mount_weapon_l.transform.parent;
            part_blade_l.renderer.material = CharacterMaterials.materials[myCostume._3dmg_texture];
            if ((bool)part_blade_l.transform.Find("X-WeaponTrailA"))
            {
                part_blade_l.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>().Deactivate();
                part_blade_l.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>().Deactivate();
                if ((bool)base.gameObject.GetComponent<HERO>())
                {
                    base.gameObject.GetComponent<HERO>().leftbladetrail = part_blade_l.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>();
                    base.gameObject.GetComponent<HERO>().leftbladetrail2 = part_blade_l.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>();
                }
            }
        }
        if (myCostume.weapon_r_mesh.Length <= 0)
        {
            return;
        }
        part_blade_r = (GameObject)Object.Instantiate(Resources.Load("Character/" + myCostume.weapon_r_mesh));
        part_blade_r.transform.position = mount_weapon_r.transform.position;
        part_blade_r.transform.rotation = mount_weapon_r.transform.rotation;
        part_blade_r.transform.parent = mount_weapon_r.transform.parent;
        part_blade_r.renderer.material = CharacterMaterials.materials[myCostume._3dmg_texture];
        if ((bool)part_blade_r.transform.Find("X-WeaponTrailA"))
        {
            part_blade_r.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>().Deactivate();
            part_blade_r.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>().Deactivate();
            if ((bool)base.gameObject.GetComponent<HERO>())
            {
                base.gameObject.GetComponent<HERO>().rightbladetrail = part_blade_r.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>();
                base.gameObject.GetComponent<HERO>().rightbladetrail2 = part_blade_r.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>();
            }
        }
    }

    public void CreateCharacterComponent()
    {
        CreateHead();
        CreateUpperBody();
        CreateLeftArm();
        CreateRightArm();
        CreateLowerBody();
        Create3DMG();
    }

    public void CreateLowerBody()
    {
        part_leg.renderer.material = CharacterMaterials.materials[myCostume.body_texture];
    }

    public void CreateLeftArm()
    {
        Object.Destroy(part_arm_l);
        if (myCostume.arm_l_mesh.Length > 0)
        {
            part_arm_l = GenerateCloth(reference, "Character/" + myCostume.arm_l_mesh);
            part_arm_l.renderer.material = CharacterMaterials.materials[myCostume.body_texture];
        }
        Object.Destroy(part_hand_l);
        if (myCostume.hand_l_mesh.Length > 0)
        {
            part_hand_l = GenerateCloth(reference, "Character/" + myCostume.hand_l_mesh);
            part_hand_l.renderer.material = CharacterMaterials.materials[myCostume.skin_texture];
        }
    }

    public void CreateRightArm()
    {
        Object.Destroy(part_arm_r);
        if (myCostume.arm_r_mesh.Length > 0)
        {
            part_arm_r = GenerateCloth(reference, "Character/" + myCostume.arm_r_mesh);
            part_arm_r.renderer.material = CharacterMaterials.materials[myCostume.body_texture];
        }
        Object.Destroy(part_hand_r);
        if (myCostume.hand_r_mesh.Length > 0)
        {
            part_hand_r = GenerateCloth(reference, "Character/" + myCostume.hand_r_mesh);
            part_hand_r.renderer.material = CharacterMaterials.materials[myCostume.skin_texture];
        }
    }

    public void CreateFace()
    {
        part_face = (GameObject)Object.Instantiate(Resources.Load("Character/character_face"));
        part_face.transform.position = part_head.transform.position;
        part_face.transform.rotation = part_head.transform.rotation;
        part_face.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
    }

    public void CreateGlasses()
    {
        part_glass = (GameObject)Object.Instantiate(Resources.Load("Character/glass"));
        part_glass.transform.position = part_head.transform.position;
        part_glass.transform.rotation = part_head.transform.rotation;
        part_glass.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
    }

    public void SetSkin()
    {
        part_head.renderer.material = CharacterMaterials.materials[myCostume.skin_texture];
        part_chest.renderer.material = CharacterMaterials.materials[myCostume.skin_texture];
        part_hand_l.renderer.material = CharacterMaterials.materials[myCostume.skin_texture];
        part_hand_r.renderer.material = CharacterMaterials.materials[myCostume.skin_texture];
    }

    public void SetFaceTexture(GameObject go, int id)
    {
        if (id >= 0)
        {
            go.renderer.material = CharacterMaterials.materials[myCostume.face_texture];
            float num = 0.125f;
            float x = num * (float)(int)((float)id / 8f);
            float y = (0f - num) * (float)(id % 8);
            go.renderer.material.mainTextureOffset = new Vector2(x, y);
        }
    }

    private GameObject GenerateCloth(GameObject go, string res)
    {
        if (!go.GetComponent<SkinnedMeshRenderer>())
        {
            go.AddComponent<SkinnedMeshRenderer>();
        }
        SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
        Transform[] bones = component.bones;
        SkinnedMeshRenderer component2 = ((GameObject)Object.Instantiate(Resources.Load(res))).GetComponent<SkinnedMeshRenderer>();
        component2.gameObject.transform.parent = component.gameObject.transform.parent;
        component2.transform.localPosition = Vector3.zero;
        component2.transform.localScale = Vector3.one;
        component2.bones = bones;
        component2.quality = SkinQuality.Bone4;
        return component2.gameObject;
    }

    public void CreateCape()
    {
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_cape);
            if (myCostume.cape_mesh.Length > 0)
            {
                part_cape = ClothFactory.GetCape(reference, "Character/" + myCostume.cape_mesh, CharacterMaterials.materials[myCostume.brand_texture]);
            }
        }
    }

    public void CreateHair()
    {
        Object.Destroy(part_hair);
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_hair_1);
        }
        if (myCostume.hair_mesh != string.Empty)
        {
            part_hair = (GameObject)Object.Instantiate(Resources.Load("Character/" + myCostume.hair_mesh));
            part_hair.transform.position = part_head.transform.position;
            part_hair.transform.rotation = part_head.transform.rotation;
            part_hair.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
            part_hair.renderer.material = CharacterMaterials.materials[myCostume.hairInfo.texture];
            part_hair.renderer.material.color = myCostume.hair_color;
        }
        if (myCostume.hair_1_mesh.Length > 0 && !isDeadBody)
        {
            string name = "Character/" + myCostume.hair_1_mesh;
            Material material = CharacterMaterials.materials[myCostume.hairInfo.texture];
            part_hair_1 = ClothFactory.GetHair(reference, name, material, myCostume.hair_color);
        }
    }

    public void CreateHead()
    {
        Object.Destroy(part_eye);
        Object.Destroy(part_face);
        Object.Destroy(part_glass);
        Object.Destroy(part_hair);
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_hair_1);
        }
        CreateHair();
        if (myCostume.eye_mesh.Length > 0)
        {
            part_eye = (GameObject)Object.Instantiate(Resources.Load("Character/" + myCostume.eye_mesh));
            part_eye.transform.position = part_head.transform.position;
            part_eye.transform.rotation = part_head.transform.rotation;
            part_eye.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
            SetFaceTexture(part_eye, myCostume.eye_texture_id);
        }
        if (myCostume.beard_texture_id >= 0)
        {
            CreateFace();
            SetFaceTexture(part_face, myCostume.beard_texture_id);
        }
        if (myCostume.glass_texture_id >= 0)
        {
            CreateGlasses();
            SetFaceTexture(part_glass, myCostume.glass_texture_id);
        }
        part_head.renderer.material = CharacterMaterials.materials[myCostume.skin_texture];
        part_chest.renderer.material = CharacterMaterials.materials[myCostume.skin_texture];
    }

    public void DeleteCharacterComponent()
    {
        Object.Destroy(part_eye);
        Object.Destroy(part_face);
        Object.Destroy(part_glass);
        Object.Destroy(part_hair);
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_hair_1);
        }
        Object.Destroy(part_upper_body);
        Object.Destroy(part_arm_l);
        Object.Destroy(part_arm_r);
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_hair_2);
            ClothFactory.DisposeObject(part_cape);
        }
        Object.Destroy(part_brand_1);
        Object.Destroy(part_brand_2);
        Object.Destroy(part_brand_3);
        Object.Destroy(part_brand_4);
        Object.Destroy(part_chest_1);
        Object.Destroy(part_chest_2);
        Object.Destroy(part_chest_3);
        Object.Destroy(part_3dmg);
        Object.Destroy(part_3dmg_belt);
        Object.Destroy(part_3dmg_gas_l);
        Object.Destroy(part_3dmg_gas_r);
        Object.Destroy(part_blade_l);
        Object.Destroy(part_blade_r);
    }

    public void CreateUpperBody()
    {
        Object.Destroy(part_upper_body);
        Object.Destroy(part_brand_1);
        Object.Destroy(part_brand_2);
        Object.Destroy(part_brand_3);
        Object.Destroy(part_brand_4);
        Object.Destroy(part_chest_1);
        Object.Destroy(part_chest_2);
        if (!isDeadBody)
        {
            ClothFactory.DisposeObject(part_chest_3);
        }
        CreateCape();
        if (myCostume.part_chest_object_mesh.Length > 0)
        {
            part_chest_1 = (GameObject)Object.Instantiate(Resources.Load("Character/" + myCostume.part_chest_object_mesh));
            part_chest_1.transform.position = chest_info.transform.position;
            part_chest_1.transform.rotation = chest_info.transform.rotation;
            part_chest_1.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
            part_chest_1.renderer.material = CharacterMaterials.materials[myCostume.part_chest_object_texture];
        }
        if (myCostume.part_chest_1_object_mesh.Length > 0)
        {
            part_chest_2 = (GameObject)Object.Instantiate(Resources.Load("Character/" + myCostume.part_chest_1_object_mesh));
            part_chest_2.transform.position = chest_info.transform.position;
            part_chest_2.transform.rotation = chest_info.transform.rotation;
            part_chest_2.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
            part_chest_2.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
            part_chest_2.renderer.material = CharacterMaterials.materials[myCostume.part_chest_1_object_texture];
        }
        if (myCostume.part_chest_skinned_cloth_mesh.Length > 0 && !isDeadBody)
        {
            part_chest_3 = ClothFactory.GetCape(reference, "Character/" + myCostume.part_chest_skinned_cloth_mesh, CharacterMaterials.materials[myCostume.part_chest_skinned_cloth_texture]);
        }
        if (myCostume.body_mesh.Length > 0)
        {
            part_upper_body = GenerateCloth(reference, "Character/" + myCostume.body_mesh);
            part_upper_body.renderer.material = CharacterMaterials.materials[myCostume.body_texture];
        }
        if (myCostume.brand1_mesh.Length > 0)
        {
            part_brand_1 = GenerateCloth(reference, "Character/" + myCostume.brand1_mesh);
            part_brand_1.renderer.material = CharacterMaterials.materials[myCostume.brand_texture];
        }
        if (myCostume.brand2_mesh.Length > 0)
        {
            part_brand_2 = GenerateCloth(reference, "Character/" + myCostume.brand2_mesh);
            part_brand_2.renderer.material = CharacterMaterials.materials[myCostume.brand_texture];
        }
        if (myCostume.brand3_mesh.Length > 0)
        {
            part_brand_3 = GenerateCloth(reference, "Character/" + myCostume.brand3_mesh);
            part_brand_3.renderer.material = CharacterMaterials.materials[myCostume.brand_texture];
        }
        if (myCostume.brand4_mesh.Length > 0)
        {
            part_brand_4 = GenerateCloth(reference, "Character/" + myCostume.brand4_mesh);
            part_brand_4.renderer.material = CharacterMaterials.materials[myCostume.brand_texture];
        }
        part_head.renderer.material = CharacterMaterials.materials[myCostume.skin_texture];
        part_chest.renderer.material = CharacterMaterials.materials[myCostume.skin_texture];
    }
}
