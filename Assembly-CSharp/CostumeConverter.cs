using ExitGames.Client.Photon;
using UnityEngine;

public class CostumeConverter
{
    public static void ToLocalData(HeroCostume costume, string slot)
    {
        slot = slot.ToUpper();
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.Sex, (int)costume.sex);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.CostumeId, costume.costumeId);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.HeroCostumeId, costume.id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.Cape, costume.cape ? 1 : 0);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.HairInfo, costume.hairInfo.id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.EyeTextureId, costume.eye_texture_id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.BeardTextureId, costume.beard_texture_id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.GlassTextureId, costume.glass_texture_id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.SkinColor, costume.skin_color);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.HairColor1, costume.hair_color.r);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.HairColor2, costume.hair_color.g);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.HairColor3, costume.hair_color.b);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.Division, (int)costume.division);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.StatSpd, costume.stat.SPD);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.StatGas, costume.stat.GAS);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.StatBla, costume.stat.BLA);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.StatAcl, costume.stat.ACL);
        PlayerPrefs.SetString(slot + PhotonPlayerProperty.StatSkill, costume.stat.skillId);
    }

    public static HeroCostume FromLocalData(string slot)
    {
        slot = slot.ToUpper();
        if (!PlayerPrefs.HasKey(slot + PhotonPlayerProperty.Sex))
        {
            return HeroCostume.Costumes[0];
        }
        // Costume Info
        HeroCostume costume = new HeroCostume();
        costume.sex = (Sex)PlayerPrefs.GetInt(slot + PhotonPlayerProperty.Sex);
        costume.id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.HeroCostumeId);
        costume.costumeId = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.CostumeId);
        costume.cape = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.Cape) == 1;
        costume.hairInfo = ((costume.sex != Sex.MALE) ? CostumeHair.hairsF[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.HairInfo)] : CostumeHair.hairsM[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.HairInfo)]);
        costume.eye_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.EyeTextureId);
        costume.beard_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.BeardTextureId);
        costume.glass_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.GlassTextureId);
        costume.skin_color = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.SkinColor);
        costume.hair_color = new Color(PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.HairColor1), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.HairColor2), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.HairColor3));
        costume.division = (Division)PlayerPrefs.GetInt(slot + PhotonPlayerProperty.Division);

        // Stats
        costume.stat = new HeroStat();
        costume.stat.SPD = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.StatSpd);
        costume.stat.GAS = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.StatGas);
        costume.stat.BLA = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.StatBla);
        costume.stat.ACL = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.StatAcl);
        costume.stat.skillId = PlayerPrefs.GetString(slot + PhotonPlayerProperty.StatSkill);
        costume.setBodyByCostumeId();
        costume.SetMesh();
        costume.setTexture();
        return costume;
    }

    public static void ToPhotonData(HeroCostume costume, PhotonPlayer player)
    {
        Hashtable properties = new Hashtable();
        properties.Add(PhotonPlayerProperty.Sex, (int)costume.sex);
        properties.Add(PhotonPlayerProperty.CostumeId, costume.costumeId);
        properties.Add(PhotonPlayerProperty.HeroCostumeId, costume.id);
        properties.Add(PhotonPlayerProperty.Cape, costume.cape);
        properties.Add(PhotonPlayerProperty.HairInfo, costume.hairInfo.id);
        properties.Add(PhotonPlayerProperty.EyeTextureId, costume.eye_texture_id);
        properties.Add(PhotonPlayerProperty.BeardTextureId, costume.beard_texture_id);
        properties.Add(PhotonPlayerProperty.GlassTextureId, costume.glass_texture_id);
        properties.Add(PhotonPlayerProperty.SkinColor, costume.skin_color);
        properties.Add(PhotonPlayerProperty.HairColor1, costume.hair_color.r);
        properties.Add(PhotonPlayerProperty.HairColor2, costume.hair_color.g);
        properties.Add(PhotonPlayerProperty.HairColor3, costume.hair_color.b);
        properties.Add(PhotonPlayerProperty.Division, (int)costume.division);
        properties.Add(PhotonPlayerProperty.StatSpd, costume.stat.SPD);
        properties.Add(PhotonPlayerProperty.StatGas, costume.stat.GAS);
        properties.Add(PhotonPlayerProperty.StatBla, costume.stat.BLA);
        properties.Add(PhotonPlayerProperty.StatAcl, costume.stat.ACL);
        properties.Add(PhotonPlayerProperty.StatSkill, costume.stat.skillId);
        player.SetCustomProperties(properties);
    }

    public static HeroCostume FromPhotonData(PhotonPlayer player)
    {
        Sex sex = (Sex)(int)player.customProperties[PhotonPlayerProperty.Sex];

        HeroCostume heroCostume = new HeroCostume();
        heroCostume.sex = sex;
        heroCostume.costumeId = (int)player.customProperties[PhotonPlayerProperty.CostumeId];
        heroCostume.id = (int)player.customProperties[PhotonPlayerProperty.HeroCostumeId];
        heroCostume.cape = (bool)player.customProperties[PhotonPlayerProperty.Cape];
        heroCostume.hairInfo = ((sex != Sex.MALE) ? CostumeHair.hairsF[(int)player.customProperties[PhotonPlayerProperty.HairInfo]] : CostumeHair.hairsM[(int)player.customProperties[PhotonPlayerProperty.HairInfo]]);
        heroCostume.eye_texture_id = (int)player.customProperties[PhotonPlayerProperty.EyeTextureId];
        heroCostume.beard_texture_id = (int)player.customProperties[PhotonPlayerProperty.BeardTextureId];
        heroCostume.glass_texture_id = (int)player.customProperties[PhotonPlayerProperty.GlassTextureId];
        heroCostume.skin_color = (int)player.customProperties[PhotonPlayerProperty.SkinColor];
        heroCostume.hair_color = new Color((float)player.customProperties[PhotonPlayerProperty.HairColor1], (float)player.customProperties[PhotonPlayerProperty.HairColor2], (float)player.customProperties[PhotonPlayerProperty.HairColor3]);
        heroCostume.division = (Division)(int)player.customProperties[PhotonPlayerProperty.Division];
        heroCostume.stat = new HeroStat();
        heroCostume.stat.SPD = (int)player.customProperties[PhotonPlayerProperty.StatSpd];
        heroCostume.stat.GAS = (int)player.customProperties[PhotonPlayerProperty.StatGas];
        heroCostume.stat.BLA = (int)player.customProperties[PhotonPlayerProperty.StatBla];
        heroCostume.stat.ACL = (int)player.customProperties[PhotonPlayerProperty.StatAcl];
        heroCostume.stat.skillId = (string)player.customProperties[PhotonPlayerProperty.StatSkill];
        heroCostume.setBodyByCostumeId();
        heroCostume.SetMesh();
        heroCostume.setTexture();
        return heroCostume;
    }
}
