using System.Collections.Generic;
using UnityEngine;

public class CharacterMaterials
{
    public static Dictionary<string, Material> materials;
    private static bool Init;

    public static void InitData()
    {
        if (!Init)
        {
            Init = true;
            materials = new Dictionary<string, Material>();
            InstantiateMaterial("AOTTG_HERO_3DMG");
            InstantiateMaterial("aottg_hero_AHSS_3dmg");
            InstantiateMaterial("aottg_hero_annie_cap_causal");
            InstantiateMaterial("aottg_hero_annie_cap_uniform");
            InstantiateMaterial("aottg_hero_brand_sc");
            InstantiateMaterial("aottg_hero_brand_mp");
            InstantiateMaterial("aottg_hero_brand_g");
            InstantiateMaterial("aottg_hero_brand_ts");
            InstantiateMaterial("aottg_hero_skin_1");
            InstantiateMaterial("aottg_hero_skin_2");
            InstantiateMaterial("aottg_hero_skin_3");
            InstantiateMaterial("aottg_hero_casual_fa_1");
            InstantiateMaterial("aottg_hero_casual_fa_2");
            InstantiateMaterial("aottg_hero_casual_fa_3");
            InstantiateMaterial("aottg_hero_casual_fb_1");
            InstantiateMaterial("aottg_hero_casual_fb_2");
            InstantiateMaterial("aottg_hero_casual_ma_1");
            InstantiateMaterial("aottg_hero_casual_ma_1_ahss");
            InstantiateMaterial("aottg_hero_casual_ma_2");
            InstantiateMaterial("aottg_hero_casual_ma_3");
            InstantiateMaterial("aottg_hero_casual_mb_1");
            InstantiateMaterial("aottg_hero_casual_mb_2");
            InstantiateMaterial("aottg_hero_casual_mb_3");
            InstantiateMaterial("aottg_hero_casual_mb_4");
            InstantiateMaterial("aottg_hero_uniform_fa_1");
            InstantiateMaterial("aottg_hero_uniform_fa_2");
            InstantiateMaterial("aottg_hero_uniform_fa_3");
            InstantiateMaterial("aottg_hero_uniform_fb_1");
            InstantiateMaterial("aottg_hero_uniform_fb_2");
            InstantiateMaterial("aottg_hero_uniform_ma_1");
            InstantiateMaterial("aottg_hero_uniform_ma_2");
            InstantiateMaterial("aottg_hero_uniform_ma_3");
            InstantiateMaterial("aottg_hero_uniform_mb_1");
            InstantiateMaterial("aottg_hero_uniform_mb_2");
            InstantiateMaterial("aottg_hero_uniform_mb_3");
            InstantiateMaterial("aottg_hero_uniform_mb_4");
            InstantiateMaterial("hair_annie");
            InstantiateMaterial("hair_armin");
            InstantiateMaterial("hair_boy1");
            InstantiateMaterial("hair_boy2");
            InstantiateMaterial("hair_boy3");
            InstantiateMaterial("hair_boy4");
            InstantiateMaterial("hair_eren");
            InstantiateMaterial("hair_girl1");
            InstantiateMaterial("hair_girl2");
            InstantiateMaterial("hair_girl3");
            InstantiateMaterial("hair_girl4");
            InstantiateMaterial("hair_girl5");
            InstantiateMaterial("hair_hanji");
            InstantiateMaterial("hair_jean");
            InstantiateMaterial("hair_levi");
            InstantiateMaterial("hair_marco");
            InstantiateMaterial("hair_mike");
            InstantiateMaterial("hair_petra");
            InstantiateMaterial("hair_rico");
            InstantiateMaterial("hair_sasha");
            InstantiateMaterial("hair_mikasa");

            Texture mainTexture = (Texture)Object.Instantiate(Resources.Load("NewTexture/aottg_hero_eyes"));
            Material material = (Material)Object.Instantiate(Resources.Load("NewTexture/MaterialGLASS"));
            material.mainTexture = mainTexture;
            materials.Add("aottg_hero_eyes", material);
        }
    }

    private static void InstantiateMaterial(string pref)
    {
        Texture mainTexture = (Texture)Object.Instantiate(Resources.Load("NewTexture/" + pref));
        Material material = (Material)Object.Instantiate(Resources.Load("NewTexture/MaterialCharacter"));
        material.mainTexture = mainTexture;
        materials.Add(pref, material);
    }
}
