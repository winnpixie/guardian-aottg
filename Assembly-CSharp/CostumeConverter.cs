using ExitGames.Client.Photon;
using UnityEngine;

public class CostumeConverter
{
    public static void HeroCostumeToLocalData(HeroCostume costume, string slot)
    {
        slot = slot.ToUpper();
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.Sex, SexToInt(costume.sex));
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
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.Division, DivisionToInt(costume.division));
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.StatSpd, costume.stat.SPD);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.StatGas, costume.stat.GAS);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.StatBla, costume.stat.BLA);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.StatAcl, costume.stat.ACL);
        PlayerPrefs.SetString(slot + PhotonPlayerProperty.StatSkill, costume.stat.skillId);
    }

    public static HeroCostume LocalDataToHeroCostume(string slot)
    {
        slot = slot.ToUpper();
        if (!PlayerPrefs.HasKey(slot + PhotonPlayerProperty.Sex))
        {
            return HeroCostume.costume[0];
        }
        // Costume Info
        HeroCostume costume = new HeroCostume();
        costume.sex = IntToSex(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.Sex));
        costume.id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.HeroCostumeId);
        costume.costumeId = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.CostumeId);
        costume.cape = ((PlayerPrefs.GetInt(slot + PhotonPlayerProperty.Cape) == 1) ? true : false);
        costume.hairInfo = ((costume.sex != 0) ? CostumeHair.hairsF[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.HairInfo)] : CostumeHair.hairsM[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.HairInfo)]);
        costume.eye_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.EyeTextureId);
        costume.beard_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.BeardTextureId);
        costume.glass_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.GlassTextureId);
        costume.skin_color = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.SkinColor);
        costume.hair_color = new Color(PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.HairColor1), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.HairColor2), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.HairColor3));
        costume.division = IntToDivision(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.Division));

        // Stats
        costume.stat = new HeroStat();
        costume.stat.SPD = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.StatSpd);
        costume.stat.GAS = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.StatGas);
        costume.stat.BLA = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.StatBla);
        costume.stat.ACL = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.StatAcl);
        costume.stat.skillId = PlayerPrefs.GetString(slot + PhotonPlayerProperty.StatSkill);
        costume.setBodyByCostumeId();
        costume.setMesh2();
        costume.setTexture();
        return costume;
    }

    private static DIVISION IntToDivision(int id)
    {
        switch (id)
        {
            case 0:
                return DIVISION.TheGarrison;
            case 1:
                return DIVISION.TheMilitaryPolice;
            case 2:
                return DIVISION.TheSurveryCorps;
            case 3:
                return DIVISION.TraineesSquad;
            default:
                return DIVISION.TheSurveryCorps;
        }
    }

    private static int DivisionToInt(DIVISION id)
    {
        switch (id)
        {
            case DIVISION.TheGarrison:
                return 0;
            case DIVISION.TheMilitaryPolice:
                return 1;
            case DIVISION.TheSurveryCorps:
                return 2;
            case DIVISION.TraineesSquad:
                return 3;
            default:
                return 2;
        }
    }

    private static SEX IntToSex(int id)
    {
        switch (id)
        {
            case 0:
                return SEX.FEMALE;
            case 1:
                return SEX.MALE;
            default:
                return SEX.MALE;
        }
    }

    private static int SexToInt(SEX id)
    {
        switch (id)
        {
            case SEX.FEMALE:
                return 0;
            case SEX.MALE:
                return 1;
            default:
                return 1;
        }
    }

    public static void HeroCostumeToPhotonData2(HeroCostume costume, PhotonPlayer player)
    {
        Hashtable properties = new Hashtable();
        properties.Add(PhotonPlayerProperty.Sex, SexToInt(costume.sex));
        int num = costume.costumeId;
        if (num == 26)
        {
            num = 25;
        }
        properties.Add(PhotonPlayerProperty.CostumeId, num);
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
        properties.Add(PhotonPlayerProperty.Division, DivisionToInt(costume.division));
        properties.Add(PhotonPlayerProperty.StatSpd, costume.stat.SPD);
        properties.Add(PhotonPlayerProperty.StatGas, costume.stat.GAS);
        properties.Add(PhotonPlayerProperty.StatBla, costume.stat.BLA);
        properties.Add(PhotonPlayerProperty.StatAcl, costume.stat.ACL);
        properties.Add(PhotonPlayerProperty.StatSkill, costume.stat.skillId);
        player.SetCustomProperties(properties);
    }

    public static HeroCostume PhotonDataToHeroCostume2(PhotonPlayer player)
    {
        SEX sex = IntToSex((int)player.customProperties[PhotonPlayerProperty.Sex]);
        HeroCostume heroCostume = new HeroCostume();
        heroCostume.sex = sex;
        heroCostume.costumeId = (int)player.customProperties[PhotonPlayerProperty.CostumeId];
        heroCostume.id = (int)player.customProperties[PhotonPlayerProperty.HeroCostumeId];
        heroCostume.cape = (bool)player.customProperties[PhotonPlayerProperty.Cape];
        heroCostume.hairInfo = ((sex != 0) ? CostumeHair.hairsF[(int)player.customProperties[PhotonPlayerProperty.HairInfo]] : CostumeHair.hairsM[(int)player.customProperties[PhotonPlayerProperty.HairInfo]]);
        heroCostume.eye_texture_id = (int)player.customProperties[PhotonPlayerProperty.EyeTextureId];
        heroCostume.beard_texture_id = (int)player.customProperties[PhotonPlayerProperty.BeardTextureId];
        heroCostume.glass_texture_id = (int)player.customProperties[PhotonPlayerProperty.GlassTextureId];
        heroCostume.skin_color = (int)player.customProperties[PhotonPlayerProperty.SkinColor];
        heroCostume.hair_color = new Color((float)player.customProperties[PhotonPlayerProperty.HairColor1], (float)player.customProperties[PhotonPlayerProperty.HairColor2], (float)player.customProperties[PhotonPlayerProperty.HairColor3]);
        heroCostume.division = IntToDivision((int)player.customProperties[PhotonPlayerProperty.Division]);
        heroCostume.stat = new HeroStat();
        heroCostume.stat.SPD = (int)player.customProperties[PhotonPlayerProperty.StatSpd];
        heroCostume.stat.GAS = (int)player.customProperties[PhotonPlayerProperty.StatGas];
        heroCostume.stat.BLA = (int)player.customProperties[PhotonPlayerProperty.StatBla];
        heroCostume.stat.ACL = (int)player.customProperties[PhotonPlayerProperty.StatAcl];
        heroCostume.stat.skillId = (string)player.customProperties[PhotonPlayerProperty.StatSkill];
        if (heroCostume.costumeId == 25 && heroCostume.sex == SEX.FEMALE)
        {
            heroCostume.costumeId = 26;
        }
        heroCostume.setBodyByCostumeId();
        heroCostume.setMesh2();
        heroCostume.setTexture();
        return heroCostume;
    }
}
