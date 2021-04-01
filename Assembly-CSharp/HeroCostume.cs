using UnityEngine;

public class HeroCostume
{
    public static HeroCostume[] Costumes;
    public static HeroCostume[] CostumeOptions;
    public static string[] body_uniform_ma_texture;
    public static string[] body_uniform_fa_texture;
    public static string[] body_uniform_mb_texture;
    public static string[] body_uniform_fb_texture;
    public static string[] body_casual_ma_texture;
    public static string[] body_casual_fa_texture;
    public static string[] body_casual_mb_texture;
    public static string[] body_casual_fb_texture;
    private static bool Initialized;

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
    public Sex sex;
    public CostumeHair hairInfo;
    public Division division;
    public HeroStat stat;
    public int costumeId;

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
        arm_l_mesh = CostumeOptions[id].arm_l_mesh;
        arm_r_mesh = CostumeOptions[id].arm_r_mesh;
        body_mesh = CostumeOptions[id].body_mesh;
        body_texture = CostumeOptions[id].body_texture;
        uniform_type = CostumeOptions[id].uniform_type;
        part_chest_1_object_mesh = CostumeOptions[id].part_chest_1_object_mesh;
        part_chest_1_object_texture = CostumeOptions[id].part_chest_1_object_texture;
        part_chest_object_mesh = CostumeOptions[id].part_chest_object_mesh;
        part_chest_object_texture = CostumeOptions[id].part_chest_object_texture;
        part_chest_skinned_cloth_mesh = CostumeOptions[id].part_chest_skinned_cloth_mesh;
        part_chest_skinned_cloth_texture = CostumeOptions[id].part_chest_skinned_cloth_texture;
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
        if (division == Division.TheMilitaryPolice)
        {
            brand_texture = "aottg_hero_brand_mp";
        }
        if (division == Division.TheGarrison)
        {
            brand_texture = "aottg_hero_brand_g";
        }
        if (division == Division.TheSurveryCorps)
        {
            brand_texture = "aottg_hero_brand_sc";
        }
        if (division == Division.TraineesSquad)
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

    public void CheckStats()
    {
        int num = 0;
        num += stat.Speed;
        num += stat.Gas;
        num += stat.Blade;
        num += stat.Accel;
        if (num > 400)
        {
            stat.Speed = (stat.Gas = (stat.Blade = (stat.Accel = 100)));
        }
    }

    public static void Init()
    {
        if (!Initialized)
        {
            Initialized = true;
            CostumeHair.Init();

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

            Costumes = new HeroCostume[39];

            Costumes[0] = new HeroCostume();
            Costumes[0].name = "annie";
            Costumes[0].sex = Sex.Female;
            Costumes[0].uniform_type = UNIFORM_TYPE.UniformB;
            Costumes[0].part_chest_object_mesh = "character_cap_uniform";
            Costumes[0].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
            Costumes[0].cape = true;
            Costumes[0].body_texture = body_uniform_fb_texture[0];
            Costumes[0].hairInfo = CostumeHair.FemaleHairs[5];
            Costumes[0].eye_texture_id = 0;
            Costumes[0].beard_texture_id = 33;
            Costumes[0].glass_texture_id = -1;
            Costumes[0].skin_color = 1;
            Costumes[0].hair_color = new Color(1f, 0.9f, 0.5f);
            Costumes[0].division = Division.TheMilitaryPolice;
            Costumes[0].costumeId = 0;

            Costumes[1] = new HeroCostume();
            Costumes[1].name = "annie";
            Costumes[1].sex = Sex.Female;
            Costumes[1].uniform_type = UNIFORM_TYPE.UniformB;
            Costumes[1].part_chest_object_mesh = "character_cap_uniform";
            Costumes[1].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
            Costumes[1].body_texture = body_uniform_fb_texture[0];
            Costumes[1].cape = false;
            Costumes[1].hairInfo = CostumeHair.FemaleHairs[5];
            Costumes[1].eye_texture_id = 0;
            Costumes[1].beard_texture_id = 33;
            Costumes[1].glass_texture_id = -1;
            Costumes[1].skin_color = 1;
            Costumes[1].hair_color = new Color(1f, 0.9f, 0.5f);
            Costumes[1].division = Division.TraineesSquad;
            Costumes[1].costumeId = 0;

            Costumes[2] = new HeroCostume();
            Costumes[2].name = "annie";
            Costumes[2].sex = Sex.Female;
            Costumes[2].uniform_type = UNIFORM_TYPE.CasualB;
            Costumes[2].part_chest_object_mesh = "character_cap_casual";
            Costumes[2].part_chest_object_texture = "aottg_hero_annie_cap_causal";
            Costumes[2].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            Costumes[2].part_chest_1_object_texture = body_casual_fb_texture[0];
            Costumes[2].body_texture = body_casual_fb_texture[0];
            Costumes[2].cape = false;
            Costumes[2].hairInfo = CostumeHair.FemaleHairs[5];
            Costumes[2].eye_texture_id = 0;
            Costumes[2].beard_texture_id = 33;
            Costumes[2].glass_texture_id = -1;
            Costumes[2].skin_color = 1;
            Costumes[2].hair_color = new Color(1f, 0.9f, 0.5f);
            Costumes[2].costumeId = 1;

            Costumes[3] = new HeroCostume();
            Costumes[3].name = "mikasa";
            Costumes[3].sex = Sex.Female;
            Costumes[3].uniform_type = UNIFORM_TYPE.UniformB;
            Costumes[3].body_texture = body_uniform_fb_texture[1];
            Costumes[3].cape = true;
            Costumes[3].hairInfo = CostumeHair.FemaleHairs[7];
            Costumes[3].eye_texture_id = 2;
            Costumes[3].beard_texture_id = 33;
            Costumes[3].glass_texture_id = -1;
            Costumes[3].skin_color = 1;
            Costumes[3].hair_color = new Color(0.15f, 0.15f, 0.145f);
            Costumes[3].division = Division.TheSurveryCorps;
            Costumes[3].costumeId = 2;

            Costumes[4] = new HeroCostume();
            Costumes[4].name = "mikasa";
            Costumes[4].sex = Sex.Female;
            Costumes[4].uniform_type = UNIFORM_TYPE.UniformB;
            Costumes[4].part_chest_skinned_cloth_mesh = "mikasa_asset_uni";
            Costumes[4].part_chest_skinned_cloth_texture = body_uniform_fb_texture[1];
            Costumes[4].body_texture = body_uniform_fb_texture[1];
            Costumes[4].cape = false;
            Costumes[4].hairInfo = CostumeHair.FemaleHairs[7];
            Costumes[4].eye_texture_id = 2;
            Costumes[4].beard_texture_id = 33;
            Costumes[4].glass_texture_id = -1;
            Costumes[4].skin_color = 1;
            Costumes[4].hair_color = new Color(0.15f, 0.15f, 0.145f);
            Costumes[4].division = Division.TraineesSquad;
            Costumes[4].costumeId = 3;

            Costumes[5] = new HeroCostume();
            Costumes[5].name = "mikasa";
            Costumes[5].sex = Sex.Female;
            Costumes[5].uniform_type = UNIFORM_TYPE.CasualB;
            Costumes[5].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
            Costumes[5].part_chest_skinned_cloth_texture = body_casual_fb_texture[1];
            Costumes[5].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            Costumes[5].part_chest_1_object_texture = body_casual_fb_texture[1];
            Costumes[5].body_texture = body_casual_fb_texture[1];
            Costumes[5].cape = false;
            Costumes[5].hairInfo = CostumeHair.FemaleHairs[7];
            Costumes[5].eye_texture_id = 2;
            Costumes[5].beard_texture_id = 33;
            Costumes[5].glass_texture_id = -1;
            Costumes[5].skin_color = 1;
            Costumes[5].hair_color = new Color(0.15f, 0.15f, 0.145f);
            Costumes[5].costumeId = 4;

            Costumes[6] = new HeroCostume();
            Costumes[6].name = "levi";
            Costumes[6].sex = Sex.Male;
            Costumes[6].uniform_type = UNIFORM_TYPE.UniformB;
            Costumes[6].body_texture = body_uniform_mb_texture[1];
            Costumes[6].cape = true;
            Costumes[6].hairInfo = CostumeHair.MaleHairs[7];
            Costumes[6].eye_texture_id = 1;
            Costumes[6].beard_texture_id = -1;
            Costumes[6].glass_texture_id = -1;
            Costumes[6].skin_color = 1;
            Costumes[6].hair_color = new Color(0.295f, 0.295f, 0.275f);
            Costumes[6].division = Division.TheSurveryCorps;
            Costumes[6].costumeId = 11;

            Costumes[7] = new HeroCostume();
            Costumes[7].name = "levi";
            Costumes[7].sex = Sex.Male;
            Costumes[7].uniform_type = UNIFORM_TYPE.CasualB;
            Costumes[7].body_texture = body_casual_mb_texture[1];
            Costumes[7].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            Costumes[7].part_chest_1_object_texture = body_casual_mb_texture[1];
            Costumes[7].cape = false;
            Costumes[7].hairInfo = CostumeHair.MaleHairs[7];
            Costumes[7].eye_texture_id = 1;
            Costumes[7].beard_texture_id = -1;
            Costumes[7].glass_texture_id = -1;
            Costumes[7].skin_color = 1;
            Costumes[7].hair_color = new Color(0.295f, 0.295f, 0.275f);
            Costumes[7].costumeId = 12;

            Costumes[8] = new HeroCostume();
            Costumes[8].name = "eren";
            Costumes[8].sex = Sex.Male;
            Costumes[8].uniform_type = UNIFORM_TYPE.UniformB;
            Costumes[8].body_texture = body_uniform_mb_texture[0];
            Costumes[8].cape = true;
            Costumes[8].hairInfo = CostumeHair.MaleHairs[4];
            Costumes[8].eye_texture_id = 3;
            Costumes[8].beard_texture_id = -1;
            Costumes[8].glass_texture_id = -1;
            Costumes[8].skin_color = 1;
            Costumes[8].hair_color = new Color(0.295f, 0.295f, 0.275f);
            Costumes[8].division = Division.TheSurveryCorps;
            Costumes[8].costumeId = 13;

            Costumes[9] = new HeroCostume();
            Costumes[9].name = "eren";
            Costumes[9].sex = Sex.Male;
            Costumes[9].uniform_type = UNIFORM_TYPE.UniformB;
            Costumes[9].body_texture = body_uniform_mb_texture[0];
            Costumes[9].cape = false;
            Costumes[9].hairInfo = CostumeHair.MaleHairs[4];
            Costumes[9].eye_texture_id = 3;
            Costumes[9].beard_texture_id = -1;
            Costumes[9].glass_texture_id = -1;
            Costumes[9].skin_color = 1;
            Costumes[9].hair_color = new Color(0.295f, 0.295f, 0.275f);
            Costumes[9].division = Division.TraineesSquad;
            Costumes[9].costumeId = 13;

            Costumes[10] = new HeroCostume();
            Costumes[10].name = "eren";
            Costumes[10].sex = Sex.Male;
            Costumes[10].uniform_type = UNIFORM_TYPE.CasualB;
            Costumes[10].body_texture = body_casual_mb_texture[0];
            Costumes[10].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            Costumes[10].part_chest_1_object_texture = body_casual_mb_texture[0];
            Costumes[10].cape = false;
            Costumes[10].hairInfo = CostumeHair.MaleHairs[4];
            Costumes[10].eye_texture_id = 3;
            Costumes[10].beard_texture_id = -1;
            Costumes[10].glass_texture_id = -1;
            Costumes[10].skin_color = 1;
            Costumes[10].hair_color = new Color(0.295f, 0.295f, 0.275f);
            Costumes[10].costumeId = 14;

            Costumes[11] = new HeroCostume();
            Costumes[11].name = "sasha";
            Costumes[11].sex = Sex.Female;
            Costumes[11].uniform_type = UNIFORM_TYPE.UniformA;
            Costumes[11].body_texture = body_uniform_fa_texture[1];
            Costumes[11].cape = true;
            Costumes[11].hairInfo = CostumeHair.FemaleHairs[10];
            Costumes[11].eye_texture_id = 4;
            Costumes[11].beard_texture_id = 33;
            Costumes[11].glass_texture_id = -1;
            Costumes[11].skin_color = 1;
            Costumes[11].hair_color = new Color(0.45f, 0.33f, 0.255f);
            Costumes[11].division = Division.TheSurveryCorps;
            Costumes[11].costumeId = 5;

            Costumes[12] = new HeroCostume();
            Costumes[12].name = "sasha";
            Costumes[12].sex = Sex.Female;
            Costumes[12].uniform_type = UNIFORM_TYPE.UniformA;
            Costumes[12].body_texture = body_uniform_fa_texture[1];
            Costumes[12].cape = false;
            Costumes[12].hairInfo = CostumeHair.FemaleHairs[10];
            Costumes[12].eye_texture_id = 4;
            Costumes[12].beard_texture_id = 33;
            Costumes[12].glass_texture_id = -1;
            Costumes[12].skin_color = 1;
            Costumes[12].hair_color = new Color(0.45f, 0.33f, 0.255f);
            Costumes[12].division = Division.TraineesSquad;
            Costumes[12].costumeId = 5;

            Costumes[13] = new HeroCostume();
            Costumes[13].name = "sasha";
            Costumes[13].sex = Sex.Female;
            Costumes[13].uniform_type = UNIFORM_TYPE.CasualA;
            Costumes[13].body_texture = body_casual_fa_texture[1];
            Costumes[13].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            Costumes[13].part_chest_1_object_texture = body_casual_fa_texture[1];
            Costumes[13].cape = false;
            Costumes[13].hairInfo = CostumeHair.FemaleHairs[10];
            Costumes[13].eye_texture_id = 4;
            Costumes[13].beard_texture_id = 33;
            Costumes[13].glass_texture_id = -1;
            Costumes[13].skin_color = 1;
            Costumes[13].hair_color = new Color(0.45f, 0.33f, 0.255f);
            Costumes[13].costumeId = 6;

            Costumes[14] = new HeroCostume();
            Costumes[14].name = "hanji";
            Costumes[14].sex = Sex.Female;
            Costumes[14].uniform_type = UNIFORM_TYPE.UniformA;
            Costumes[14].body_texture = body_uniform_fa_texture[2];
            Costumes[14].cape = true;
            Costumes[14].hairInfo = CostumeHair.FemaleHairs[6];
            Costumes[14].eye_texture_id = 5;
            Costumes[14].beard_texture_id = 33;
            Costumes[14].glass_texture_id = 49;
            Costumes[14].skin_color = 1;
            Costumes[14].hair_color = new Color(0.45f, 0.33f, 0.255f);
            Costumes[14].division = Division.TheSurveryCorps;
            Costumes[14].costumeId = 7;

            Costumes[15] = new HeroCostume();
            Costumes[15].name = "hanji";
            Costumes[15].sex = Sex.Female;
            Costumes[15].uniform_type = UNIFORM_TYPE.CasualA;
            Costumes[15].body_texture = body_casual_fa_texture[2];
            Costumes[15].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            Costumes[15].part_chest_1_object_texture = body_casual_fa_texture[2];
            Costumes[15].cape = false;
            Costumes[15].hairInfo = CostumeHair.FemaleHairs[6];
            Costumes[15].eye_texture_id = 5;
            Costumes[15].beard_texture_id = 33;
            Costumes[15].glass_texture_id = 49;
            Costumes[15].skin_color = 1;
            Costumes[15].hair_color = new Color(0.295f, 0.23f, 0.17f);
            Costumes[15].costumeId = 8;

            Costumes[16] = new HeroCostume();
            Costumes[16].name = "rico";
            Costumes[16].sex = Sex.Female;
            Costumes[16].uniform_type = UNIFORM_TYPE.UniformA;
            Costumes[16].body_texture = body_uniform_fa_texture[0];
            Costumes[16].cape = true;
            Costumes[16].hairInfo = CostumeHair.FemaleHairs[9];
            Costumes[16].eye_texture_id = 6;
            Costumes[16].beard_texture_id = 33;
            Costumes[16].glass_texture_id = 48;
            Costumes[16].skin_color = 1;
            Costumes[16].hair_color = new Color(1f, 1f, 1f);
            Costumes[16].division = Division.TheGarrison;
            Costumes[16].costumeId = 9;

            Costumes[17] = new HeroCostume();
            Costumes[17].name = "rico";
            Costumes[17].sex = Sex.Female;
            Costumes[17].uniform_type = UNIFORM_TYPE.CasualA;
            Costumes[17].body_texture = body_casual_fa_texture[0];
            Costumes[17].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            Costumes[17].part_chest_1_object_texture = body_casual_fa_texture[0];
            Costumes[17].cape = false;
            Costumes[17].hairInfo = CostumeHair.FemaleHairs[9];
            Costumes[17].eye_texture_id = 6;
            Costumes[17].beard_texture_id = 33;
            Costumes[17].glass_texture_id = 48;
            Costumes[17].skin_color = 1;
            Costumes[17].hair_color = new Color(1f, 1f, 1f);
            Costumes[17].costumeId = 10;

            Costumes[18] = new HeroCostume();
            Costumes[18].name = "jean";
            Costumes[18].sex = Sex.Male;
            Costumes[18].uniform_type = UNIFORM_TYPE.UniformA;
            Costumes[18].body_texture = body_uniform_ma_texture[1];
            Costumes[18].cape = true;
            Costumes[18].hairInfo = CostumeHair.MaleHairs[6];
            Costumes[18].eye_texture_id = 7;
            Costumes[18].beard_texture_id = -1;
            Costumes[18].glass_texture_id = -1;
            Costumes[18].skin_color = 1;
            Costumes[18].hair_color = new Color(0.94f, 0.84f, 0.6f);
            Costumes[18].division = Division.TheSurveryCorps;
            Costumes[18].costumeId = 15;

            Costumes[19] = new HeroCostume();
            Costumes[19].name = "jean";
            Costumes[19].sex = Sex.Male;
            Costumes[19].uniform_type = UNIFORM_TYPE.UniformA;
            Costumes[19].body_texture = body_uniform_ma_texture[1];
            Costumes[19].cape = false;
            Costumes[19].hairInfo = CostumeHair.MaleHairs[6];
            Costumes[19].eye_texture_id = 7;
            Costumes[19].beard_texture_id = -1;
            Costumes[19].glass_texture_id = -1;
            Costumes[19].skin_color = 1;
            Costumes[19].hair_color = new Color(0.94f, 0.84f, 0.6f);
            Costumes[19].division = Division.TraineesSquad;
            Costumes[19].costumeId = 15;

            Costumes[20] = new HeroCostume();
            Costumes[20].name = "jean";
            Costumes[20].sex = Sex.Male;
            Costumes[20].uniform_type = UNIFORM_TYPE.CasualA;
            Costumes[20].body_texture = body_casual_ma_texture[1];
            Costumes[20].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            Costumes[20].part_chest_1_object_texture = body_casual_ma_texture[1];
            Costumes[20].cape = false;
            Costumes[20].hairInfo = CostumeHair.MaleHairs[6];
            Costumes[20].eye_texture_id = 7;
            Costumes[20].beard_texture_id = -1;
            Costumes[20].glass_texture_id = -1;
            Costumes[20].skin_color = 1;
            Costumes[20].hair_color = new Color(0.94f, 0.84f, 0.6f);
            Costumes[20].costumeId = 16;

            Costumes[21] = new HeroCostume();
            Costumes[21].name = "marco";
            Costumes[21].sex = Sex.Male;
            Costumes[21].uniform_type = UNIFORM_TYPE.UniformA;
            Costumes[21].body_texture = body_uniform_ma_texture[2];
            Costumes[21].cape = false;
            Costumes[21].hairInfo = CostumeHair.MaleHairs[8];
            Costumes[21].eye_texture_id = 8;
            Costumes[21].beard_texture_id = -1;
            Costumes[21].glass_texture_id = -1;
            Costumes[21].skin_color = 1;
            Costumes[21].hair_color = new Color(0.295f, 0.295f, 0.275f);
            Costumes[21].division = Division.TraineesSquad;
            Costumes[21].costumeId = 17;

            Costumes[22] = new HeroCostume();
            Costumes[22].name = "marco";
            Costumes[22].sex = Sex.Male;
            Costumes[22].uniform_type = UNIFORM_TYPE.CasualA;
            Costumes[22].body_texture = body_casual_ma_texture[2];
            Costumes[22].cape = false;
            Costumes[22].hairInfo = CostumeHair.MaleHairs[8];
            Costumes[22].eye_texture_id = 8;
            Costumes[22].beard_texture_id = -1;
            Costumes[22].glass_texture_id = -1;
            Costumes[22].skin_color = 1;
            Costumes[22].hair_color = new Color(0.295f, 0.295f, 0.275f);
            Costumes[22].costumeId = 18;

            Costumes[23] = new HeroCostume();
            Costumes[23].name = "mike";
            Costumes[23].sex = Sex.Male;
            Costumes[23].uniform_type = UNIFORM_TYPE.UniformB;
            Costumes[23].body_texture = body_uniform_mb_texture[3];
            Costumes[23].cape = true;
            Costumes[23].hairInfo = CostumeHair.MaleHairs[9];
            Costumes[23].eye_texture_id = 9;
            Costumes[23].beard_texture_id = 32;
            Costumes[23].glass_texture_id = -1;
            Costumes[23].skin_color = 1;
            Costumes[23].hair_color = new Color(0.94f, 0.84f, 0.6f);
            Costumes[23].division = Division.TheSurveryCorps;
            Costumes[23].costumeId = 19;

            Costumes[24] = new HeroCostume();
            Costumes[24].name = "mike";
            Costumes[24].sex = Sex.Male;
            Costumes[24].uniform_type = UNIFORM_TYPE.CasualB;
            Costumes[24].body_texture = body_casual_mb_texture[3];
            Costumes[24].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            Costumes[24].part_chest_1_object_texture = body_casual_mb_texture[3];
            Costumes[24].cape = false;
            Costumes[24].hairInfo = CostumeHair.MaleHairs[9];
            Costumes[24].eye_texture_id = 9;
            Costumes[24].beard_texture_id = 32;
            Costumes[24].glass_texture_id = -1;
            Costumes[24].skin_color = 1;
            Costumes[24].hair_color = new Color(0.94f, 0.84f, 0.6f);
            Costumes[24].division = Division.TheSurveryCorps;
            Costumes[24].costumeId = 20;

            Costumes[25] = new HeroCostume();
            Costumes[25].name = "connie";
            Costumes[25].sex = Sex.Male;
            Costumes[25].uniform_type = UNIFORM_TYPE.UniformB;
            Costumes[25].body_texture = body_uniform_mb_texture[2];
            Costumes[25].cape = true;
            Costumes[25].hairInfo = CostumeHair.MaleHairs[10];
            Costumes[25].eye_texture_id = 10;
            Costumes[25].beard_texture_id = -1;
            Costumes[25].glass_texture_id = -1;
            Costumes[25].skin_color = 1;
            Costumes[25].division = Division.TheSurveryCorps;
            Costumes[25].costumeId = 21;

            Costumes[26] = new HeroCostume();
            Costumes[26].name = "connie";
            Costumes[26].sex = Sex.Male;
            Costumes[26].uniform_type = UNIFORM_TYPE.UniformB;
            Costumes[26].body_texture = body_uniform_mb_texture[2];
            Costumes[26].cape = false;
            Costumes[26].hairInfo = CostumeHair.MaleHairs[10];
            Costumes[26].eye_texture_id = 10;
            Costumes[26].beard_texture_id = -1;
            Costumes[26].glass_texture_id = -1;
            Costumes[26].skin_color = 1;
            Costumes[26].division = Division.TraineesSquad;
            Costumes[26].costumeId = 21;

            Costumes[27] = new HeroCostume();
            Costumes[27].name = "connie";
            Costumes[27].sex = Sex.Male;
            Costumes[27].uniform_type = UNIFORM_TYPE.CasualB;
            Costumes[27].body_texture = body_casual_mb_texture[2];
            Costumes[27].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            Costumes[27].part_chest_1_object_texture = body_casual_mb_texture[2];
            Costumes[27].cape = false;
            Costumes[27].hairInfo = CostumeHair.MaleHairs[10];
            Costumes[27].eye_texture_id = 10;
            Costumes[27].beard_texture_id = -1;
            Costumes[27].glass_texture_id = -1;
            Costumes[27].skin_color = 1;
            Costumes[27].costumeId = 22;

            Costumes[28] = new HeroCostume();
            Costumes[28].name = "armin";
            Costumes[28].sex = Sex.Male;
            Costumes[28].uniform_type = UNIFORM_TYPE.UniformA;
            Costumes[28].body_texture = body_uniform_ma_texture[0];
            Costumes[28].cape = true;
            Costumes[28].hairInfo = CostumeHair.MaleHairs[5];
            Costumes[28].eye_texture_id = 11;
            Costumes[28].beard_texture_id = -1;
            Costumes[28].glass_texture_id = -1;
            Costumes[28].skin_color = 1;
            Costumes[28].hair_color = new Color(0.95f, 0.8f, 0.5f);
            Costumes[28].division = Division.TheSurveryCorps;
            Costumes[28].costumeId = 23;

            Costumes[29] = new HeroCostume();
            Costumes[29].name = "armin";
            Costumes[29].sex = Sex.Male;
            Costumes[29].uniform_type = UNIFORM_TYPE.UniformA;
            Costumes[29].body_texture = body_uniform_ma_texture[0];
            Costumes[29].cape = false;
            Costumes[29].hairInfo = CostumeHair.MaleHairs[5];
            Costumes[29].eye_texture_id = 11;
            Costumes[29].beard_texture_id = -1;
            Costumes[29].glass_texture_id = -1;
            Costumes[29].skin_color = 1;
            Costumes[29].hair_color = new Color(0.95f, 0.8f, 0.5f);
            Costumes[29].division = Division.TraineesSquad;
            Costumes[29].costumeId = 23;

            Costumes[30] = new HeroCostume();
            Costumes[30].name = "armin";
            Costumes[30].sex = Sex.Male;
            Costumes[30].uniform_type = UNIFORM_TYPE.CasualA;
            Costumes[30].body_texture = body_casual_ma_texture[0];
            Costumes[30].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            Costumes[30].part_chest_1_object_texture = body_casual_ma_texture[0];
            Costumes[30].cape = false;
            Costumes[30].hairInfo = CostumeHair.MaleHairs[5];
            Costumes[30].eye_texture_id = 11;
            Costumes[30].beard_texture_id = -1;
            Costumes[30].glass_texture_id = -1;
            Costumes[30].skin_color = 1;
            Costumes[30].hair_color = new Color(0.95f, 0.8f, 0.5f);
            Costumes[30].costumeId = 24;

            Costumes[31] = new HeroCostume();
            Costumes[31].name = "petra";
            Costumes[31].sex = Sex.Female;
            Costumes[31].uniform_type = UNIFORM_TYPE.UniformA;
            Costumes[31].body_texture = body_uniform_fa_texture[0];
            Costumes[31].cape = true;
            Costumes[31].hairInfo = CostumeHair.FemaleHairs[8];
            Costumes[31].eye_texture_id = 27;
            Costumes[31].beard_texture_id = -1;
            Costumes[31].glass_texture_id = -1;
            Costumes[31].skin_color = 1;
            Costumes[31].hair_color = new Color(1f, 0.725f, 0.376f);
            Costumes[31].division = Division.TheSurveryCorps;
            Costumes[31].costumeId = 9;

            Costumes[32] = new HeroCostume();
            Costumes[32].name = "petra";
            Costumes[32].sex = Sex.Female;
            Costumes[32].uniform_type = UNIFORM_TYPE.CasualA;
            Costumes[32].body_texture = body_casual_fa_texture[0];
            Costumes[32].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            Costumes[32].part_chest_1_object_texture = body_casual_fa_texture[0];
            Costumes[32].cape = false;
            Costumes[32].hairInfo = CostumeHair.FemaleHairs[8];
            Costumes[32].eye_texture_id = 27;
            Costumes[32].beard_texture_id = -1;
            Costumes[32].glass_texture_id = -1;
            Costumes[32].skin_color = 1;
            Costumes[32].hair_color = new Color(1f, 0.725f, 0.376f);
            Costumes[32].division = Division.TheSurveryCorps;
            Costumes[32].costumeId = 10;

            Costumes[33] = new HeroCostume();
            Costumes[33].name = "custom";
            Costumes[33].sex = Sex.Female;
            Costumes[33].uniform_type = UNIFORM_TYPE.CasualB;
            Costumes[33].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
            Costumes[33].part_chest_skinned_cloth_texture = body_casual_fb_texture[1];
            Costumes[33].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            Costumes[33].part_chest_1_object_texture = body_casual_fb_texture[1];
            Costumes[33].body_texture = body_casual_fb_texture[1];
            Costumes[33].cape = false;
            Costumes[33].hairInfo = CostumeHair.FemaleHairs[2];
            Costumes[33].eye_texture_id = 12;
            Costumes[33].beard_texture_id = 33;
            Costumes[33].glass_texture_id = -1;
            Costumes[33].skin_color = 1;
            Costumes[33].hair_color = new Color(0.15f, 0.15f, 0.145f);
            Costumes[33].costumeId = 4;

            Costumes[34] = new HeroCostume();
            Costumes[34].name = "custom";
            Costumes[34].sex = Sex.Male;
            Costumes[34].uniform_type = UNIFORM_TYPE.CasualA;
            Costumes[34].body_texture = body_casual_ma_texture[0];
            Costumes[34].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            Costumes[34].part_chest_1_object_texture = body_casual_ma_texture[0];
            Costumes[34].cape = false;
            Costumes[34].hairInfo = CostumeHair.MaleHairs[3];
            Costumes[34].eye_texture_id = 26;
            Costumes[34].beard_texture_id = 44;
            Costumes[34].glass_texture_id = -1;
            Costumes[34].skin_color = 1;
            Costumes[34].hair_color = new Color(0.41f, 1f, 0f);
            Costumes[34].costumeId = 24;

            Costumes[35] = new HeroCostume();
            Costumes[35].name = "custom";
            Costumes[35].sex = Sex.Female;
            Costumes[35].uniform_type = UNIFORM_TYPE.UniformA;
            Costumes[35].body_texture = body_uniform_fa_texture[1];
            Costumes[35].cape = false;
            Costumes[35].hairInfo = CostumeHair.FemaleHairs[4];
            Costumes[35].eye_texture_id = 22;
            Costumes[35].beard_texture_id = 33;
            Costumes[35].glass_texture_id = 56;
            Costumes[35].skin_color = 1;
            Costumes[35].hair_color = new Color(0f, 1f, 0.874f);
            Costumes[35].costumeId = 5;

            Costumes[36] = new HeroCostume();
            Costumes[36].name = "feng";
            Costumes[36].sex = Sex.Male;
            Costumes[36].uniform_type = UNIFORM_TYPE.CasualB;
            Costumes[36].body_texture = body_casual_mb_texture[3];
            Costumes[36].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            Costumes[36].part_chest_1_object_texture = body_casual_mb_texture[3];
            Costumes[36].cape = true;
            Costumes[36].hairInfo = CostumeHair.MaleHairs[10];
            Costumes[36].eye_texture_id = 25;
            Costumes[36].beard_texture_id = 39;
            Costumes[36].glass_texture_id = 53;
            Costumes[36].skin_color = 1;
            Costumes[36].division = Division.TheSurveryCorps;
            Costumes[36].costumeId = 20;

            Costumes[37] = new HeroCostume();
            Costumes[37].name = "AHSS";
            Costumes[37].sex = Sex.Male;
            Costumes[37].uniform_type = UNIFORM_TYPE.CasualAHSS;
            Costumes[37].body_texture = body_casual_ma_texture[0] + "_ahss";
            Costumes[37].cape = false;
            Costumes[37].hairInfo = CostumeHair.MaleHairs[6];
            Costumes[37].eye_texture_id = 25;
            Costumes[37].beard_texture_id = 39;
            Costumes[37].glass_texture_id = 53;
            Costumes[37].skin_color = 3;
            Costumes[37].division = Division.TheMilitaryPolice;
            Costumes[37].costumeId = 25;

            Costumes[38] = new HeroCostume();
            Costumes[38].name = "AHSS (F)";
            Costumes[38].sex = Sex.Female;
            Costumes[38].uniform_type = UNIFORM_TYPE.CasualAHSS;
            Costumes[38].body_texture = body_casual_ma_texture[0] + "_ahss";
            Costumes[38].cape = false;
            Costumes[38].hairInfo = CostumeHair.FemaleHairs[6];
            Costumes[38].eye_texture_id = 2;
            Costumes[38].beard_texture_id = 33;
            Costumes[38].glass_texture_id = -1;
            Costumes[38].skin_color = 3;
            Costumes[38].division = Division.TheMilitaryPolice;
            Costumes[38].costumeId = 25;

            for (int i = 0; i < Costumes.Length; i++)
            {
                Costumes[i].stat = HeroStat.GetInfo("CUSTOM_DEFAULT");
                Costumes[i].id = i;
                Costumes[i].SetMesh();
                Costumes[i].setTexture();
            }
            CostumeOptions = new HeroCostume[27]
            {
                Costumes[0],
                Costumes[2],
                Costumes[3],
                Costumes[4],
                Costumes[5],
                Costumes[11],
                Costumes[13],
                Costumes[14],
                Costumes[15],
                Costumes[16],
                Costumes[17],
                Costumes[6],
                Costumes[7],
                Costumes[8],
                Costumes[10],
                Costumes[18],
                Costumes[19],
                Costumes[21],
                Costumes[22],
                Costumes[23],
                Costumes[24],
                Costumes[25],
                Costumes[27],
                Costumes[28],
                Costumes[30],
                Costumes[37],
                Costumes[38]
            };
        }
    }

    public void SetMesh()
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
            if (sex == Sex.Female)
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
            if (sex == Sex.Female)
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
            if (sex == Sex.Female)
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
            if (sex == Sex.Female)
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
            if (sex == Sex.Female)
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
