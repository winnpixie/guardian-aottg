using UnityEngine;

public class HeroCostume
{
    public int id;
    public string hair_mesh = string.Empty;
    public string hair_1_mesh = string.Empty;
    public Color hair_color = new Color(0.5f, 0.1f, 0f);
    public string arm_l_mesh = string.Empty;
    public string arm_r_mesh = string.Empty;
    public string hand_l_mesh = string.Empty;
    public string hand_r_mesh = string.Empty;
    public string mesh_3dmg = string.Empty;
    public string mesh_3dmg_gas_l = string.Empty;
    public string mesh_3dmg_gas_r = string.Empty;
    public string mesh_3dmg_belt = string.Empty;
    public string weapon_l_mesh = string.Empty;
    public string weapon_r_mesh = string.Empty;
    public bool cape;
    public string cape_mesh = string.Empty;
    public string cape_texture = string.Empty;
    public string name = string.Empty;
    public string part_chest_skinned_cloth_mesh = string.Empty;
    public string part_chest_skinned_cloth_texture = string.Empty;
    public string part_chest_object_mesh = string.Empty;
    public string part_chest_object_texture = string.Empty;
    public string part_chest_1_object_mesh = string.Empty;
    public string part_chest_1_object_texture = string.Empty;
    public string body_mesh = string.Empty;
    public string body_texture = string.Empty;
    public string brand1_mesh = string.Empty;
    public string brand2_mesh = string.Empty;
    public string brand3_mesh = string.Empty;
    public string brand4_mesh = string.Empty;
    public string brand_texture = string.Empty;
    public string _3dmg_texture = string.Empty;
    public string face_texture = string.Empty;
    public string eye_mesh = string.Empty;
    public string beard_mesh = string.Empty;
    public string glass_mesh = string.Empty;
    public int eye_texture_id = -1;
    public int beard_texture_id = -1;
    public int glass_texture_id = -1;
    public int skin_color = 1;
    public string skin_texture = string.Empty;
    public UNIFORM_TYPE uniform_type = UNIFORM_TYPE.CasualA;
    public SEX sex;
    public CostumeHair hairInfo;
    public DIVISION division;
    public HeroStat stat;
    public static HeroCostume[] costume;
    public static HeroCostume[] costumeOption;
    public int costumeId;
    public static string[] body_uniform_ma_texture;
    public static string[] body_uniform_fa_texture;
    public static string[] body_uniform_mb_texture;
    public static string[] body_uniform_fb_texture;
    public static string[] body_casual_ma_texture;
    public static string[] body_casual_fa_texture;
    public static string[] body_casual_mb_texture;
    public static string[] body_casual_fb_texture;
    private static bool inited;

    public void setMesh()
    {
        brand1_mesh = string.Empty;
        brand2_mesh = string.Empty;
        brand3_mesh = string.Empty;
        brand4_mesh = string.Empty;
        hand_l_mesh = "character_hand_l";
        hand_r_mesh = "character_hand_r";
        mesh_3dmg = "character_3dmg";
        mesh_3dmg_belt = "character_3dmg_belt";
        mesh_3dmg_gas_l = "character_3dmg_gas_l";
        mesh_3dmg_gas_r = "character_3dmg_gas_r";
        weapon_l_mesh = "character_blade_l";
        weapon_r_mesh = "character_blade_r";
        if (uniform_type == UNIFORM_TYPE.CasualAHSS)
        {
            hand_l_mesh = "character_hand_l_ah";
            hand_r_mesh = "character_hand_r_ah";
            arm_l_mesh = "character_arm_casual_l_ah";
            arm_r_mesh = "character_arm_casual_r_ah";
            body_mesh = "character_body_casual_MA";
            mesh_3dmg = "character_3dmg_2";
            mesh_3dmg_belt = string.Empty;
            mesh_3dmg_gas_l = "character_gun_mag_l";
            mesh_3dmg_gas_r = "character_gun_mag_r";
            weapon_l_mesh = "character_gun_l";
            weapon_r_mesh = "character_gun_r";
        }
        else if (uniform_type == UNIFORM_TYPE.UniformA)
        {
            arm_l_mesh = "character_arm_uniform_l";
            arm_r_mesh = "character_arm_uniform_r";
            brand1_mesh = "character_brand_arm_l";
            brand2_mesh = "character_brand_arm_r";
            if (sex == SEX.FEMALE)
            {
                body_mesh = "character_body_uniform_FA";
                brand3_mesh = "character_brand_chest_f";
                brand4_mesh = "character_brand_back_f";
            }
            else
            {
                body_mesh = "character_body_uniform_MA";
                brand3_mesh = "character_brand_chest_m";
                brand4_mesh = "character_brand_back_m";
            }
        }
        else if (uniform_type == UNIFORM_TYPE.UniformB)
        {
            arm_l_mesh = "character_arm_uniform_l";
            arm_r_mesh = "character_arm_uniform_r";
            brand1_mesh = "character_brand_arm_l";
            brand2_mesh = "character_brand_arm_r";
            if (sex == SEX.FEMALE)
            {
                body_mesh = "character_body_uniform_FB";
                brand3_mesh = "character_brand_chest_f";
                brand4_mesh = "character_brand_back_f";
            }
            else
            {
                body_mesh = "character_body_uniform_MB";
                brand3_mesh = "character_brand_chest_m";
                brand4_mesh = "character_brand_back_m";
            }
        }
        else if (uniform_type == UNIFORM_TYPE.CasualA)
        {
            arm_l_mesh = "character_arm_casual_l";
            arm_r_mesh = "character_arm_casual_r";
            if (sex == SEX.FEMALE)
            {
                body_mesh = "character_body_casual_FA";
            }
            else
            {
                body_mesh = "character_body_casual_MA";
            }
        }
        else if (uniform_type == UNIFORM_TYPE.CasualB)
        {
            arm_l_mesh = "character_arm_casual_l";
            arm_r_mesh = "character_arm_casual_r";
            if (sex == SEX.FEMALE)
            {
                body_mesh = "character_body_casual_FB";
            }
            else
            {
                body_mesh = "character_body_casual_MB";
            }
        }
        if (hairInfo.hair.Length > 0)
        {
            hair_mesh = hairInfo.hair;
        }
        if (hairInfo.hasCloth)
        {
            hair_1_mesh = hairInfo.hair_1;
        }
        if (eye_texture_id >= 0)
        {
            eye_mesh = "character_eye";
        }
        if (beard_texture_id >= 0)
        {
            beard_mesh = "character_face";
        }
        else
        {
            beard_mesh = string.Empty;
        }
        if (glass_texture_id >= 0)
        {
            glass_mesh = "glass";
        }
        else
        {
            glass_mesh = string.Empty;
        }
        setCape();
    }

    public void setCape()
    {
        if (cape)
        {
            cape_mesh = "character_cape";
        }
        else
        {
            cape_mesh = string.Empty;
        }
    }

    public void setBodyByCostumeId(int id = -1)
    {
        if (id == -1)
        {
            id = costumeId;
        }
        costumeId = id;
        arm_l_mesh = costumeOption[id].arm_l_mesh;
        arm_r_mesh = costumeOption[id].arm_r_mesh;
        body_mesh = costumeOption[id].body_mesh;
        body_texture = costumeOption[id].body_texture;
        uniform_type = costumeOption[id].uniform_type;
        part_chest_1_object_mesh = costumeOption[id].part_chest_1_object_mesh;
        part_chest_1_object_texture = costumeOption[id].part_chest_1_object_texture;
        part_chest_object_mesh = costumeOption[id].part_chest_object_mesh;
        part_chest_object_texture = costumeOption[id].part_chest_object_texture;
        part_chest_skinned_cloth_mesh = costumeOption[id].part_chest_skinned_cloth_mesh;
        part_chest_skinned_cloth_texture = costumeOption[id].part_chest_skinned_cloth_texture;
    }

    public void setTexture()
    {
        if (uniform_type == UNIFORM_TYPE.CasualAHSS)
        {
            _3dmg_texture = "aottg_hero_AHSS_3dmg";
        }
        else
        {
            _3dmg_texture = "AOTTG_HERO_3DMG";
        }
        face_texture = "aottg_hero_eyes";
        if (division == DIVISION.TheMilitaryPolice)
        {
            brand_texture = "aottg_hero_brand_mp";
        }
        if (division == DIVISION.TheGarrison)
        {
            brand_texture = "aottg_hero_brand_g";
        }
        if (division == DIVISION.TheSurveryCorps)
        {
            brand_texture = "aottg_hero_brand_sc";
        }
        if (division == DIVISION.TraineesSquad)
        {
            brand_texture = "aottg_hero_brand_ts";
        }
        if (skin_color == 1)
        {
            skin_texture = "aottg_hero_skin_1";
        }
        else if (skin_color == 2)
        {
            skin_texture = "aottg_hero_skin_2";
        }
        else if (skin_color == 3)
        {
            skin_texture = "aottg_hero_skin_3";
        }
    }

    public void checkstat()
    {
        int num = 0;
        num += stat.SPD;
        num += stat.GAS;
        num += stat.BLA;
        num += stat.ACL;
        if (num > 400)
        {
            stat.SPD = (stat.GAS = (stat.BLA = (stat.ACL = 100)));
        }
    }

    public static void init2()
    {
        if (!inited)
        {
            inited = true;
            CostumeHair.init();
            body_uniform_ma_texture = new string[3]
            {
                "aottg_hero_uniform_ma_1",
                "aottg_hero_uniform_ma_2",
                "aottg_hero_uniform_ma_3"
            };
            body_uniform_fa_texture = new string[3]
            {
                "aottg_hero_uniform_fa_1",
                "aottg_hero_uniform_fa_2",
                "aottg_hero_uniform_fa_3"
            };
            body_uniform_mb_texture = new string[4]
            {
                "aottg_hero_uniform_mb_1",
                "aottg_hero_uniform_mb_2",
                "aottg_hero_uniform_mb_3",
                "aottg_hero_uniform_mb_4"
            };
            body_uniform_fb_texture = new string[2]
            {
                "aottg_hero_uniform_fb_1",
                "aottg_hero_uniform_fb_2"
            };
            body_casual_ma_texture = new string[3]
            {
                "aottg_hero_casual_ma_1",
                "aottg_hero_casual_ma_2",
                "aottg_hero_casual_ma_3"
            };
            body_casual_fa_texture = new string[3]
            {
                "aottg_hero_casual_fa_1",
                "aottg_hero_casual_fa_2",
                "aottg_hero_casual_fa_3"
            };
            body_casual_mb_texture = new string[4]
            {
                "aottg_hero_casual_mb_1",
                "aottg_hero_casual_mb_2",
                "aottg_hero_casual_mb_3",
                "aottg_hero_casual_mb_4"
            };
            body_casual_fb_texture = new string[2]
            {
                "aottg_hero_casual_fb_1",
                "aottg_hero_casual_fb_2"
            };
            costume = new HeroCostume[39];
            costume[0] = new HeroCostume();
            costume[0].name = "annie";
            costume[0].sex = SEX.FEMALE;
            costume[0].uniform_type = UNIFORM_TYPE.UniformB;
            costume[0].part_chest_object_mesh = "character_cap_uniform";
            costume[0].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
            costume[0].cape = true;
            costume[0].body_texture = body_uniform_fb_texture[0];
            costume[0].hairInfo = CostumeHair.hairsF[5];
            costume[0].eye_texture_id = 0;
            costume[0].beard_texture_id = 33;
            costume[0].glass_texture_id = -1;
            costume[0].skin_color = 1;
            costume[0].hair_color = new Color(1f, 0.9f, 0.5f);
            costume[0].division = DIVISION.TheMilitaryPolice;
            costume[0].costumeId = 0;
            costume[1] = new HeroCostume();
            costume[1].name = "annie";
            costume[1].sex = SEX.FEMALE;
            costume[1].uniform_type = UNIFORM_TYPE.UniformB;
            costume[1].part_chest_object_mesh = "character_cap_uniform";
            costume[1].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
            costume[1].body_texture = body_uniform_fb_texture[0];
            costume[1].cape = false;
            costume[1].hairInfo = CostumeHair.hairsF[5];
            costume[1].eye_texture_id = 0;
            costume[1].beard_texture_id = 33;
            costume[1].glass_texture_id = -1;
            costume[1].skin_color = 1;
            costume[1].hair_color = new Color(1f, 0.9f, 0.5f);
            costume[1].division = DIVISION.TraineesSquad;
            costume[1].costumeId = 0;
            costume[2] = new HeroCostume();
            costume[2].name = "annie";
            costume[2].sex = SEX.FEMALE;
            costume[2].uniform_type = UNIFORM_TYPE.CasualB;
            costume[2].part_chest_object_mesh = "character_cap_casual";
            costume[2].part_chest_object_texture = "aottg_hero_annie_cap_causal";
            costume[2].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[2].part_chest_1_object_texture = body_casual_fb_texture[0];
            costume[2].body_texture = body_casual_fb_texture[0];
            costume[2].cape = false;
            costume[2].hairInfo = CostumeHair.hairsF[5];
            costume[2].eye_texture_id = 0;
            costume[2].beard_texture_id = 33;
            costume[2].glass_texture_id = -1;
            costume[2].skin_color = 1;
            costume[2].hair_color = new Color(1f, 0.9f, 0.5f);
            costume[2].costumeId = 1;
            costume[3] = new HeroCostume();
            costume[3].name = "mikasa";
            costume[3].sex = SEX.FEMALE;
            costume[3].uniform_type = UNIFORM_TYPE.UniformB;
            costume[3].body_texture = body_uniform_fb_texture[1];
            costume[3].cape = true;
            costume[3].hairInfo = CostumeHair.hairsF[7];
            costume[3].eye_texture_id = 2;
            costume[3].beard_texture_id = 33;
            costume[3].glass_texture_id = -1;
            costume[3].skin_color = 1;
            costume[3].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[3].division = DIVISION.TheSurveryCorps;
            costume[3].costumeId = 2;
            costume[4] = new HeroCostume();
            costume[4].name = "mikasa";
            costume[4].sex = SEX.FEMALE;
            costume[4].uniform_type = UNIFORM_TYPE.UniformB;
            costume[4].part_chest_skinned_cloth_mesh = "mikasa_asset_uni";
            costume[4].part_chest_skinned_cloth_texture = body_uniform_fb_texture[1];
            costume[4].body_texture = body_uniform_fb_texture[1];
            costume[4].cape = false;
            costume[4].hairInfo = CostumeHair.hairsF[7];
            costume[4].eye_texture_id = 2;
            costume[4].beard_texture_id = 33;
            costume[4].glass_texture_id = -1;
            costume[4].skin_color = 1;
            costume[4].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[4].division = DIVISION.TraineesSquad;
            costume[4].costumeId = 3;
            costume[5] = new HeroCostume();
            costume[5].name = "mikasa";
            costume[5].sex = SEX.FEMALE;
            costume[5].uniform_type = UNIFORM_TYPE.CasualB;
            costume[5].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
            costume[5].part_chest_skinned_cloth_texture = body_casual_fb_texture[1];
            costume[5].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[5].part_chest_1_object_texture = body_casual_fb_texture[1];
            costume[5].body_texture = body_casual_fb_texture[1];
            costume[5].cape = false;
            costume[5].hairInfo = CostumeHair.hairsF[7];
            costume[5].eye_texture_id = 2;
            costume[5].beard_texture_id = 33;
            costume[5].glass_texture_id = -1;
            costume[5].skin_color = 1;
            costume[5].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[5].costumeId = 4;
            costume[6] = new HeroCostume();
            costume[6].name = "levi";
            costume[6].sex = SEX.MALE;
            costume[6].uniform_type = UNIFORM_TYPE.UniformB;
            costume[6].body_texture = body_uniform_mb_texture[1];
            costume[6].cape = true;
            costume[6].hairInfo = CostumeHair.hairsM[7];
            costume[6].eye_texture_id = 1;
            costume[6].beard_texture_id = -1;
            costume[6].glass_texture_id = -1;
            costume[6].skin_color = 1;
            costume[6].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[6].division = DIVISION.TheSurveryCorps;
            costume[6].costumeId = 11;
            costume[7] = new HeroCostume();
            costume[7].name = "levi";
            costume[7].sex = SEX.MALE;
            costume[7].uniform_type = UNIFORM_TYPE.CasualB;
            costume[7].body_texture = body_casual_mb_texture[1];
            costume[7].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[7].part_chest_1_object_texture = body_casual_mb_texture[1];
            costume[7].cape = false;
            costume[7].hairInfo = CostumeHair.hairsM[7];
            costume[7].eye_texture_id = 1;
            costume[7].beard_texture_id = -1;
            costume[7].glass_texture_id = -1;
            costume[7].skin_color = 1;
            costume[7].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[7].costumeId = 12;
            costume[8] = new HeroCostume();
            costume[8].name = "eren";
            costume[8].sex = SEX.MALE;
            costume[8].uniform_type = UNIFORM_TYPE.UniformB;
            costume[8].body_texture = body_uniform_mb_texture[0];
            costume[8].cape = true;
            costume[8].hairInfo = CostumeHair.hairsM[4];
            costume[8].eye_texture_id = 3;
            costume[8].beard_texture_id = -1;
            costume[8].glass_texture_id = -1;
            costume[8].skin_color = 1;
            costume[8].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[8].division = DIVISION.TheSurveryCorps;
            costume[8].costumeId = 13;
            costume[9] = new HeroCostume();
            costume[9].name = "eren";
            costume[9].sex = SEX.MALE;
            costume[9].uniform_type = UNIFORM_TYPE.UniformB;
            costume[9].body_texture = body_uniform_mb_texture[0];
            costume[9].cape = false;
            costume[9].hairInfo = CostumeHair.hairsM[4];
            costume[9].eye_texture_id = 3;
            costume[9].beard_texture_id = -1;
            costume[9].glass_texture_id = -1;
            costume[9].skin_color = 1;
            costume[9].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[9].division = DIVISION.TraineesSquad;
            costume[9].costumeId = 13;
            costume[10] = new HeroCostume();
            costume[10].name = "eren";
            costume[10].sex = SEX.MALE;
            costume[10].uniform_type = UNIFORM_TYPE.CasualB;
            costume[10].body_texture = body_casual_mb_texture[0];
            costume[10].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[10].part_chest_1_object_texture = body_casual_mb_texture[0];
            costume[10].cape = false;
            costume[10].hairInfo = CostumeHair.hairsM[4];
            costume[10].eye_texture_id = 3;
            costume[10].beard_texture_id = -1;
            costume[10].glass_texture_id = -1;
            costume[10].skin_color = 1;
            costume[10].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[10].costumeId = 14;
            costume[11] = new HeroCostume();
            costume[11].name = "sasha";
            costume[11].sex = SEX.FEMALE;
            costume[11].uniform_type = UNIFORM_TYPE.UniformA;
            costume[11].body_texture = body_uniform_fa_texture[1];
            costume[11].cape = true;
            costume[11].hairInfo = CostumeHair.hairsF[10];
            costume[11].eye_texture_id = 4;
            costume[11].beard_texture_id = 33;
            costume[11].glass_texture_id = -1;
            costume[11].skin_color = 1;
            costume[11].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[11].division = DIVISION.TheSurveryCorps;
            costume[11].costumeId = 5;
            costume[12] = new HeroCostume();
            costume[12].name = "sasha";
            costume[12].sex = SEX.FEMALE;
            costume[12].uniform_type = UNIFORM_TYPE.UniformA;
            costume[12].body_texture = body_uniform_fa_texture[1];
            costume[12].cape = false;
            costume[12].hairInfo = CostumeHair.hairsF[10];
            costume[12].eye_texture_id = 4;
            costume[12].beard_texture_id = 33;
            costume[12].glass_texture_id = -1;
            costume[12].skin_color = 1;
            costume[12].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[12].division = DIVISION.TraineesSquad;
            costume[12].costumeId = 5;
            costume[13] = new HeroCostume();
            costume[13].name = "sasha";
            costume[13].sex = SEX.FEMALE;
            costume[13].uniform_type = UNIFORM_TYPE.CasualA;
            costume[13].body_texture = body_casual_fa_texture[1];
            costume[13].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[13].part_chest_1_object_texture = body_casual_fa_texture[1];
            costume[13].cape = false;
            costume[13].hairInfo = CostumeHair.hairsF[10];
            costume[13].eye_texture_id = 4;
            costume[13].beard_texture_id = 33;
            costume[13].glass_texture_id = -1;
            costume[13].skin_color = 1;
            costume[13].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[13].costumeId = 6;
            costume[14] = new HeroCostume();
            costume[14].name = "hanji";
            costume[14].sex = SEX.FEMALE;
            costume[14].uniform_type = UNIFORM_TYPE.UniformA;
            costume[14].body_texture = body_uniform_fa_texture[2];
            costume[14].cape = true;
            costume[14].hairInfo = CostumeHair.hairsF[6];
            costume[14].eye_texture_id = 5;
            costume[14].beard_texture_id = 33;
            costume[14].glass_texture_id = 49;
            costume[14].skin_color = 1;
            costume[14].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[14].division = DIVISION.TheSurveryCorps;
            costume[14].costumeId = 7;
            costume[15] = new HeroCostume();
            costume[15].name = "hanji";
            costume[15].sex = SEX.FEMALE;
            costume[15].uniform_type = UNIFORM_TYPE.CasualA;
            costume[15].body_texture = body_casual_fa_texture[2];
            costume[15].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[15].part_chest_1_object_texture = body_casual_fa_texture[2];
            costume[15].cape = false;
            costume[15].hairInfo = CostumeHair.hairsF[6];
            costume[15].eye_texture_id = 5;
            costume[15].beard_texture_id = 33;
            costume[15].glass_texture_id = 49;
            costume[15].skin_color = 1;
            costume[15].hair_color = new Color(0.295f, 0.23f, 0.17f);
            costume[15].costumeId = 8;
            costume[16] = new HeroCostume();
            costume[16].name = "rico";
            costume[16].sex = SEX.FEMALE;
            costume[16].uniform_type = UNIFORM_TYPE.UniformA;
            costume[16].body_texture = body_uniform_fa_texture[0];
            costume[16].cape = true;
            costume[16].hairInfo = CostumeHair.hairsF[9];
            costume[16].eye_texture_id = 6;
            costume[16].beard_texture_id = 33;
            costume[16].glass_texture_id = 48;
            costume[16].skin_color = 1;
            costume[16].hair_color = new Color(1f, 1f, 1f);
            costume[16].division = DIVISION.TheGarrison;
            costume[16].costumeId = 9;
            costume[17] = new HeroCostume();
            costume[17].name = "rico";
            costume[17].sex = SEX.FEMALE;
            costume[17].uniform_type = UNIFORM_TYPE.CasualA;
            costume[17].body_texture = body_casual_fa_texture[0];
            costume[17].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[17].part_chest_1_object_texture = body_casual_fa_texture[0];
            costume[17].cape = false;
            costume[17].hairInfo = CostumeHair.hairsF[9];
            costume[17].eye_texture_id = 6;
            costume[17].beard_texture_id = 33;
            costume[17].glass_texture_id = 48;
            costume[17].skin_color = 1;
            costume[17].hair_color = new Color(1f, 1f, 1f);
            costume[17].costumeId = 10;
            costume[18] = new HeroCostume();
            costume[18].name = "jean";
            costume[18].sex = SEX.MALE;
            costume[18].uniform_type = UNIFORM_TYPE.UniformA;
            costume[18].body_texture = body_uniform_ma_texture[1];
            costume[18].cape = true;
            costume[18].hairInfo = CostumeHair.hairsM[6];
            costume[18].eye_texture_id = 7;
            costume[18].beard_texture_id = -1;
            costume[18].glass_texture_id = -1;
            costume[18].skin_color = 1;
            costume[18].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[18].division = DIVISION.TheSurveryCorps;
            costume[18].costumeId = 15;
            costume[19] = new HeroCostume();
            costume[19].name = "jean";
            costume[19].sex = SEX.MALE;
            costume[19].uniform_type = UNIFORM_TYPE.UniformA;
            costume[19].body_texture = body_uniform_ma_texture[1];
            costume[19].cape = false;
            costume[19].hairInfo = CostumeHair.hairsM[6];
            costume[19].eye_texture_id = 7;
            costume[19].beard_texture_id = -1;
            costume[19].glass_texture_id = -1;
            costume[19].skin_color = 1;
            costume[19].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[19].division = DIVISION.TraineesSquad;
            costume[19].costumeId = 15;
            costume[20] = new HeroCostume();
            costume[20].name = "jean";
            costume[20].sex = SEX.MALE;
            costume[20].uniform_type = UNIFORM_TYPE.CasualA;
            costume[20].body_texture = body_casual_ma_texture[1];
            costume[20].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[20].part_chest_1_object_texture = body_casual_ma_texture[1];
            costume[20].cape = false;
            costume[20].hairInfo = CostumeHair.hairsM[6];
            costume[20].eye_texture_id = 7;
            costume[20].beard_texture_id = -1;
            costume[20].glass_texture_id = -1;
            costume[20].skin_color = 1;
            costume[20].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[20].costumeId = 16;
            costume[21] = new HeroCostume();
            costume[21].name = "marco";
            costume[21].sex = SEX.MALE;
            costume[21].uniform_type = UNIFORM_TYPE.UniformA;
            costume[21].body_texture = body_uniform_ma_texture[2];
            costume[21].cape = false;
            costume[21].hairInfo = CostumeHair.hairsM[8];
            costume[21].eye_texture_id = 8;
            costume[21].beard_texture_id = -1;
            costume[21].glass_texture_id = -1;
            costume[21].skin_color = 1;
            costume[21].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[21].division = DIVISION.TraineesSquad;
            costume[21].costumeId = 17;
            costume[22] = new HeroCostume();
            costume[22].name = "marco";
            costume[22].sex = SEX.MALE;
            costume[22].uniform_type = UNIFORM_TYPE.CasualA;
            costume[22].body_texture = body_casual_ma_texture[2];
            costume[22].cape = false;
            costume[22].hairInfo = CostumeHair.hairsM[8];
            costume[22].eye_texture_id = 8;
            costume[22].beard_texture_id = -1;
            costume[22].glass_texture_id = -1;
            costume[22].skin_color = 1;
            costume[22].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[22].costumeId = 18;
            costume[23] = new HeroCostume();
            costume[23].name = "mike";
            costume[23].sex = SEX.MALE;
            costume[23].uniform_type = UNIFORM_TYPE.UniformB;
            costume[23].body_texture = body_uniform_mb_texture[3];
            costume[23].cape = true;
            costume[23].hairInfo = CostumeHair.hairsM[9];
            costume[23].eye_texture_id = 9;
            costume[23].beard_texture_id = 32;
            costume[23].glass_texture_id = -1;
            costume[23].skin_color = 1;
            costume[23].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[23].division = DIVISION.TheSurveryCorps;
            costume[23].costumeId = 19;
            costume[24] = new HeroCostume();
            costume[24].name = "mike";
            costume[24].sex = SEX.MALE;
            costume[24].uniform_type = UNIFORM_TYPE.CasualB;
            costume[24].body_texture = body_casual_mb_texture[3];
            costume[24].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[24].part_chest_1_object_texture = body_casual_mb_texture[3];
            costume[24].cape = false;
            costume[24].hairInfo = CostumeHair.hairsM[9];
            costume[24].eye_texture_id = 9;
            costume[24].beard_texture_id = 32;
            costume[24].glass_texture_id = -1;
            costume[24].skin_color = 1;
            costume[24].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[24].division = DIVISION.TheSurveryCorps;
            costume[24].costumeId = 20;
            costume[25] = new HeroCostume();
            costume[25].name = "connie";
            costume[25].sex = SEX.MALE;
            costume[25].uniform_type = UNIFORM_TYPE.UniformB;
            costume[25].body_texture = body_uniform_mb_texture[2];
            costume[25].cape = true;
            costume[25].hairInfo = CostumeHair.hairsM[10];
            costume[25].eye_texture_id = 10;
            costume[25].beard_texture_id = -1;
            costume[25].glass_texture_id = -1;
            costume[25].skin_color = 1;
            costume[25].division = DIVISION.TheSurveryCorps;
            costume[25].costumeId = 21;
            costume[26] = new HeroCostume();
            costume[26].name = "connie";
            costume[26].sex = SEX.MALE;
            costume[26].uniform_type = UNIFORM_TYPE.UniformB;
            costume[26].body_texture = body_uniform_mb_texture[2];
            costume[26].cape = false;
            costume[26].hairInfo = CostumeHair.hairsM[10];
            costume[26].eye_texture_id = 10;
            costume[26].beard_texture_id = -1;
            costume[26].glass_texture_id = -1;
            costume[26].skin_color = 1;
            costume[26].division = DIVISION.TraineesSquad;
            costume[26].costumeId = 21;
            costume[27] = new HeroCostume();
            costume[27].name = "connie";
            costume[27].sex = SEX.MALE;
            costume[27].uniform_type = UNIFORM_TYPE.CasualB;
            costume[27].body_texture = body_casual_mb_texture[2];
            costume[27].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[27].part_chest_1_object_texture = body_casual_mb_texture[2];
            costume[27].cape = false;
            costume[27].hairInfo = CostumeHair.hairsM[10];
            costume[27].eye_texture_id = 10;
            costume[27].beard_texture_id = -1;
            costume[27].glass_texture_id = -1;
            costume[27].skin_color = 1;
            costume[27].costumeId = 22;
            costume[28] = new HeroCostume();
            costume[28].name = "armin";
            costume[28].sex = SEX.MALE;
            costume[28].uniform_type = UNIFORM_TYPE.UniformA;
            costume[28].body_texture = body_uniform_ma_texture[0];
            costume[28].cape = true;
            costume[28].hairInfo = CostumeHair.hairsM[5];
            costume[28].eye_texture_id = 11;
            costume[28].beard_texture_id = -1;
            costume[28].glass_texture_id = -1;
            costume[28].skin_color = 1;
            costume[28].hair_color = new Color(0.95f, 0.8f, 0.5f);
            costume[28].division = DIVISION.TheSurveryCorps;
            costume[28].costumeId = 23;
            costume[29] = new HeroCostume();
            costume[29].name = "armin";
            costume[29].sex = SEX.MALE;
            costume[29].uniform_type = UNIFORM_TYPE.UniformA;
            costume[29].body_texture = body_uniform_ma_texture[0];
            costume[29].cape = false;
            costume[29].hairInfo = CostumeHair.hairsM[5];
            costume[29].eye_texture_id = 11;
            costume[29].beard_texture_id = -1;
            costume[29].glass_texture_id = -1;
            costume[29].skin_color = 1;
            costume[29].hair_color = new Color(0.95f, 0.8f, 0.5f);
            costume[29].division = DIVISION.TraineesSquad;
            costume[29].costumeId = 23;
            costume[30] = new HeroCostume();
            costume[30].name = "armin";
            costume[30].sex = SEX.MALE;
            costume[30].uniform_type = UNIFORM_TYPE.CasualA;
            costume[30].body_texture = body_casual_ma_texture[0];
            costume[30].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[30].part_chest_1_object_texture = body_casual_ma_texture[0];
            costume[30].cape = false;
            costume[30].hairInfo = CostumeHair.hairsM[5];
            costume[30].eye_texture_id = 11;
            costume[30].beard_texture_id = -1;
            costume[30].glass_texture_id = -1;
            costume[30].skin_color = 1;
            costume[30].hair_color = new Color(0.95f, 0.8f, 0.5f);
            costume[30].costumeId = 24;
            costume[31] = new HeroCostume();
            costume[31].name = "petra";
            costume[31].sex = SEX.FEMALE;
            costume[31].uniform_type = UNIFORM_TYPE.UniformA;
            costume[31].body_texture = body_uniform_fa_texture[0];
            costume[31].cape = true;
            costume[31].hairInfo = CostumeHair.hairsF[8];
            costume[31].eye_texture_id = 27;
            costume[31].beard_texture_id = -1;
            costume[31].glass_texture_id = -1;
            costume[31].skin_color = 1;
            costume[31].hair_color = new Color(1f, 0.725f, 0.376f);
            costume[31].division = DIVISION.TheSurveryCorps;
            costume[31].costumeId = 9;
            costume[32] = new HeroCostume();
            costume[32].name = "petra";
            costume[32].sex = SEX.FEMALE;
            costume[32].uniform_type = UNIFORM_TYPE.CasualA;
            costume[32].body_texture = body_casual_fa_texture[0];
            costume[32].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[32].part_chest_1_object_texture = body_casual_fa_texture[0];
            costume[32].cape = false;
            costume[32].hairInfo = CostumeHair.hairsF[8];
            costume[32].eye_texture_id = 27;
            costume[32].beard_texture_id = -1;
            costume[32].glass_texture_id = -1;
            costume[32].skin_color = 1;
            costume[32].hair_color = new Color(1f, 0.725f, 0.376f);
            costume[32].division = DIVISION.TheSurveryCorps;
            costume[32].costumeId = 10;
            costume[33] = new HeroCostume();
            costume[33].name = "custom";
            costume[33].sex = SEX.FEMALE;
            costume[33].uniform_type = UNIFORM_TYPE.CasualB;
            costume[33].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
            costume[33].part_chest_skinned_cloth_texture = body_casual_fb_texture[1];
            costume[33].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[33].part_chest_1_object_texture = body_casual_fb_texture[1];
            costume[33].body_texture = body_casual_fb_texture[1];
            costume[33].cape = false;
            costume[33].hairInfo = CostumeHair.hairsF[2];
            costume[33].eye_texture_id = 12;
            costume[33].beard_texture_id = 33;
            costume[33].glass_texture_id = -1;
            costume[33].skin_color = 1;
            costume[33].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[33].costumeId = 4;
            costume[34] = new HeroCostume();
            costume[34].name = "custom";
            costume[34].sex = SEX.MALE;
            costume[34].uniform_type = UNIFORM_TYPE.CasualA;
            costume[34].body_texture = body_casual_ma_texture[0];
            costume[34].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[34].part_chest_1_object_texture = body_casual_ma_texture[0];
            costume[34].cape = false;
            costume[34].hairInfo = CostumeHair.hairsM[3];
            costume[34].eye_texture_id = 26;
            costume[34].beard_texture_id = 44;
            costume[34].glass_texture_id = -1;
            costume[34].skin_color = 1;
            costume[34].hair_color = new Color(0.41f, 1f, 0f);
            costume[34].costumeId = 24;
            costume[35] = new HeroCostume();
            costume[35].name = "custom";
            costume[35].sex = SEX.FEMALE;
            costume[35].uniform_type = UNIFORM_TYPE.UniformA;
            costume[35].body_texture = body_uniform_fa_texture[1];
            costume[35].cape = false;
            costume[35].hairInfo = CostumeHair.hairsF[4];
            costume[35].eye_texture_id = 22;
            costume[35].beard_texture_id = 33;
            costume[35].glass_texture_id = 56;
            costume[35].skin_color = 1;
            costume[35].hair_color = new Color(0f, 1f, 0.874f);
            costume[35].costumeId = 5;
            costume[36] = new HeroCostume();
            costume[36].name = "feng";
            costume[36].sex = SEX.MALE;
            costume[36].uniform_type = UNIFORM_TYPE.CasualB;
            costume[36].body_texture = body_casual_mb_texture[3];
            costume[36].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[36].part_chest_1_object_texture = body_casual_mb_texture[3];
            costume[36].cape = true;
            costume[36].hairInfo = CostumeHair.hairsM[10];
            costume[36].eye_texture_id = 25;
            costume[36].beard_texture_id = 39;
            costume[36].glass_texture_id = 53;
            costume[36].skin_color = 1;
            costume[36].division = DIVISION.TheSurveryCorps;
            costume[36].costumeId = 20;
            costume[37] = new HeroCostume();
            costume[37].name = "AHSS";
            costume[37].sex = SEX.MALE;
            costume[37].uniform_type = UNIFORM_TYPE.CasualAHSS;
            costume[37].body_texture = body_casual_ma_texture[0] + "_ahss";
            costume[37].cape = false;
            costume[37].hairInfo = CostumeHair.hairsM[6];
            costume[37].eye_texture_id = 25;
            costume[37].beard_texture_id = 39;
            costume[37].glass_texture_id = 53;
            costume[37].skin_color = 3;
            costume[37].division = DIVISION.TheMilitaryPolice;
            costume[37].costumeId = 25;
            costume[38] = new HeroCostume();
            costume[38].name = "AHSS (F)";
            costume[38].sex = SEX.FEMALE;
            costume[38].uniform_type = UNIFORM_TYPE.CasualAHSS;
            costume[38].body_texture = body_casual_fa_texture[0];
            costume[38].cape = false;
            costume[38].hairInfo = CostumeHair.hairsF[6];
            costume[38].eye_texture_id = 2;
            costume[38].beard_texture_id = 33;
            costume[38].glass_texture_id = -1;
            costume[38].skin_color = 3;
            costume[38].division = DIVISION.TheMilitaryPolice;
            costume[38].costumeId = 26;
            for (int i = 0; i < costume.Length; i++)
            {
                costume[i].stat = HeroStat.getInfo("CUSTOM_DEFAULT");
                costume[i].id = i;
                costume[i].setMesh2();
                costume[i].setTexture();
            }
            costumeOption = new HeroCostume[27]
            {
                costume[0],
                costume[2],
                costume[3],
                costume[4],
                costume[5],
                costume[11],
                costume[13],
                costume[14],
                costume[15],
                costume[16],
                costume[17],
                costume[6],
                costume[7],
                costume[8],
                costume[10],
                costume[18],
                costume[19],
                costume[21],
                costume[22],
                costume[23],
                costume[24],
                costume[25],
                costume[27],
                costume[28],
                costume[30],
                costume[37],
                costume[38]
            };
        }
    }

    public void setMesh2()
    {
        brand1_mesh = string.Empty;
        brand2_mesh = string.Empty;
        brand3_mesh = string.Empty;
        brand4_mesh = string.Empty;
        hand_l_mesh = "character_hand_l";
        hand_r_mesh = "character_hand_r";
        mesh_3dmg = "character_3dmg";
        mesh_3dmg_belt = "character_3dmg_belt";
        mesh_3dmg_gas_l = "character_3dmg_gas_l";
        mesh_3dmg_gas_r = "character_3dmg_gas_r";
        weapon_l_mesh = "character_blade_l";
        weapon_r_mesh = "character_blade_r";
        if (uniform_type == UNIFORM_TYPE.CasualAHSS)
        {
            hand_l_mesh = "character_hand_l_ah";
            hand_r_mesh = "character_hand_r_ah";
            arm_l_mesh = "character_arm_casual_l_ah";
            arm_r_mesh = "character_arm_casual_r_ah";
            if (sex == SEX.FEMALE)
            {
                body_mesh = "character_body_casual_FA";
            }
            else
            {
                body_mesh = "character_body_casual_MA";
            }
            mesh_3dmg = "character_3dmg_2";
            mesh_3dmg_belt = string.Empty;
            mesh_3dmg_gas_l = "character_gun_mag_l";
            mesh_3dmg_gas_r = "character_gun_mag_r";
            weapon_l_mesh = "character_gun_l";
            weapon_r_mesh = "character_gun_r";
        }
        else if (uniform_type == UNIFORM_TYPE.UniformA)
        {
            arm_l_mesh = "character_arm_uniform_l";
            arm_r_mesh = "character_arm_uniform_r";
            brand1_mesh = "character_brand_arm_l";
            brand2_mesh = "character_brand_arm_r";
            if (sex == SEX.FEMALE)
            {
                body_mesh = "character_body_uniform_FA";
                brand3_mesh = "character_brand_chest_f";
                brand4_mesh = "character_brand_back_f";
            }
            else
            {
                body_mesh = "character_body_uniform_MA";
                brand3_mesh = "character_brand_chest_m";
                brand4_mesh = "character_brand_back_m";
            }
        }
        else if (uniform_type == UNIFORM_TYPE.UniformB)
        {
            arm_l_mesh = "character_arm_uniform_l";
            arm_r_mesh = "character_arm_uniform_r";
            brand1_mesh = "character_brand_arm_l";
            brand2_mesh = "character_brand_arm_r";
            if (sex == SEX.FEMALE)
            {
                body_mesh = "character_body_uniform_FB";
                brand3_mesh = "character_brand_chest_f";
                brand4_mesh = "character_brand_back_f";
            }
            else
            {
                body_mesh = "character_body_uniform_MB";
                brand3_mesh = "character_brand_chest_m";
                brand4_mesh = "character_brand_back_m";
            }
        }
        else if (uniform_type == UNIFORM_TYPE.CasualA)
        {
            arm_l_mesh = "character_arm_casual_l";
            arm_r_mesh = "character_arm_casual_r";
            if (sex == SEX.FEMALE)
            {
                body_mesh = "character_body_casual_FA";
            }
            else
            {
                body_mesh = "character_body_casual_MA";
            }
        }
        else if (uniform_type == UNIFORM_TYPE.CasualB)
        {
            arm_l_mesh = "character_arm_casual_l";
            arm_r_mesh = "character_arm_casual_r";
            if (sex == SEX.FEMALE)
            {
                body_mesh = "character_body_casual_FB";
            }
            else
            {
                body_mesh = "character_body_casual_MB";
            }
        }
        if (hairInfo.hair.Length > 0)
        {
            hair_mesh = hairInfo.hair;
        }
        if (hairInfo.hasCloth)
        {
            hair_1_mesh = hairInfo.hair_1;
        }
        if (eye_texture_id >= 0)
        {
            eye_mesh = "character_eye";
        }
        if (beard_texture_id >= 0)
        {
            beard_mesh = "character_face";
        }
        else
        {
            beard_mesh = string.Empty;
        }
        if (glass_texture_id >= 0)
        {
            glass_mesh = "glass";
        }
        else
        {
            glass_mesh = string.Empty;
        }
        setCape();
    }
}
