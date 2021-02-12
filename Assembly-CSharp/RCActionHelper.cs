using System;

public class RCActionHelper
{
    public enum helperClasses
    {
        primitive,
        variable,
        player,
        titan,
        region,
        convert
    }

    public enum mathTypes
    {
        add,
        subtract,
        multiply,
        divide,
        modulo,
        power
    }

    public enum variableTypes
    {
        typeInt,
        typeBool,
        typeString,
        typeFloat,
        typePlayer,
        typeTitan
    }

    public enum playerTypes
    {
        playerType,
        playerTeam,
        playerAlive,
        playerTitan,
        playerKills,
        playerDeaths,
        playerMaxDamage,
        playerTotalDamage,
        playerCustomInt,
        playerCustomBool,
        playerCustomString,
        playerCustomFloat,
        playerName,
        playerGuildName,
        playerPosX,
        playerPosY,
        playerPosZ,
        playerSpeed
    }

    public enum titanTypes
    {
        titanType,
        titanSize,
        titanHealth,
        positionX,
        positionY,
        positionZ
    }

    public enum other
    {
        regionX,
        regionY,
        regionZ
    }

    public int helperClass;
    public int helperType;
    private object parameters;
    private RCActionHelper nextHelper;

    public RCActionHelper(int sentClass, int sentType, object options)
    {
        helperClass = sentClass;
        helperType = sentType;
        parameters = options;
    }

    public void SetNextHelper(RCActionHelper sentHelper)
    {
        nextHelper = sentHelper;
    }

    public void CallException(string str)
    {
        InRoomChat.Instance.AddLine(str);
    }

    public int ReturnInt(object sentObject)
    {
        object obj = sentObject;
        if (parameters != null)
        {
            obj = parameters;
        }
        switch (helperClass)
        {
            case 0:
                return (int)obj;
            case 5:
                switch (helperType)
                {
                    case 0:
                        return (int)obj;
                    case 1:
                        {
                            bool value = (bool)obj;
                            return Convert.ToInt32(value);
                        }
                    case 3:
                        {
                            float value2 = (float)obj;
                            return Convert.ToInt32(value2);
                        }
                    case 2:
                        {
                            string text = (string)obj;
                            if (int.TryParse((string)obj, out int result))
                            {
                                return result;
                            }
                            return 0;
                        }
                    default:
                        return (int)obj;
                }
            case 1:
                {
                    RCActionHelper rCActionHelper2 = (RCActionHelper)obj;
                    switch (helperType)
                    {
                        case 0:
                            return (int)FengGameManagerMKII.IntVariables[rCActionHelper2.returnString(null)];
                        case 1:
                            return nextHelper.ReturnInt(FengGameManagerMKII.BoolVariables[rCActionHelper2.returnString(null)]);
                        case 2:
                            return nextHelper.ReturnInt(FengGameManagerMKII.StringVariables[rCActionHelper2.returnString(null)]);
                        case 3:
                            return nextHelper.ReturnInt(FengGameManagerMKII.FloatVariables[rCActionHelper2.returnString(null)]);
                        case 4:
                            return nextHelper.ReturnInt(FengGameManagerMKII.PlayerVariables[rCActionHelper2.returnString(null)]);
                        case 5:
                            return nextHelper.ReturnInt(FengGameManagerMKII.TitanVariables[rCActionHelper2.returnString(null)]);
                        default:
                            return 0;
                    }
                }
            case 2:
                {
                    PhotonPlayer photonPlayer = (PhotonPlayer)obj;
                    if (photonPlayer != null)
                    {
                        switch (helperType)
                        {
                            case 0:
                                return (int)photonPlayer.customProperties[PhotonPlayerProperty.Team];
                            case 1:
                                return (int)photonPlayer.customProperties[PhotonPlayerProperty.RCTeam];
                            case 2:
                                return nextHelper.ReturnInt(photonPlayer.customProperties[PhotonPlayerProperty.Dead]);
                            case 3:
                                return (int)photonPlayer.customProperties[PhotonPlayerProperty.IsTitan];
                            case 4:
                                return (int)photonPlayer.customProperties[PhotonPlayerProperty.Kills];
                            case 5:
                                return (int)photonPlayer.customProperties[PhotonPlayerProperty.Deaths];
                            case 6:
                                return (int)photonPlayer.customProperties[PhotonPlayerProperty.MaxDamage];
                            case 7:
                                return (int)photonPlayer.customProperties[PhotonPlayerProperty.TotalDamage];
                            case 8:
                                return (int)photonPlayer.customProperties[PhotonPlayerProperty.CustomInt];
                            case 9:
                                return nextHelper.ReturnInt(photonPlayer.customProperties[PhotonPlayerProperty.CustomBool]);
                            case 10:
                                return nextHelper.ReturnInt(photonPlayer.customProperties[PhotonPlayerProperty.CustomString]);
                            case 11:
                                return nextHelper.ReturnInt(photonPlayer.customProperties[PhotonPlayerProperty.CustomFloat]);
                            case 14:
                                {
                                    int iD4 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD4))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD4];
                                        return nextHelper.ReturnInt(hERO.transform.position.x);
                                    }
                                    return 0;
                                }
                            case 15:
                                {
                                    int iD3 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD3))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD3];
                                        return nextHelper.ReturnInt(hERO.transform.position.y);
                                    }
                                    return 0;
                                }
                            case 16:
                                {
                                    int iD2 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD2))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD2];
                                        return nextHelper.ReturnInt(hERO.transform.position.z);
                                    }
                                    return 0;
                                }
                            case 12:
                                return nextHelper.ReturnInt(photonPlayer.customProperties[PhotonPlayerProperty.Name]);
                            case 13:
                                return nextHelper.ReturnInt(photonPlayer.customProperties[PhotonPlayerProperty.Guild]);
                            case 17:
                                {
                                    int iD = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD];
                                        return nextHelper.ReturnInt(hERO.rigidbody.velocity.magnitude);
                                    }
                                    return 0;
                                }
                        }
                    }
                    return 0;
                }
            case 3:
                {
                    TITAN tITAN = (TITAN)obj;
                    if (tITAN != null)
                    {
                        switch (helperType)
                        {
                            case 0:
                                return (int)tITAN.abnormalType;
                            case 1:
                                return nextHelper.ReturnInt(tITAN.myLevel);
                            case 2:
                                return tITAN.currentHealth;
                            case 3:
                                return nextHelper.ReturnInt(tITAN.transform.position.x);
                            case 4:
                                return nextHelper.ReturnInt(tITAN.transform.position.y);
                            case 5:
                                return nextHelper.ReturnInt(tITAN.transform.position.z);
                        }
                    }
                    return 0;
                }
            case 4:
                {
                    RCActionHelper rCActionHelper = (RCActionHelper)obj;
                    RCRegion rCRegion = (RCRegion)FengGameManagerMKII.RCRegions[rCActionHelper.returnString(null)];
                    switch (helperType)
                    {
                        case 0:
                            return nextHelper.ReturnInt(rCRegion.GetRandomX());
                        case 1:
                            return nextHelper.ReturnInt(rCRegion.GetRandomY());
                        case 2:
                            return nextHelper.ReturnInt(rCRegion.GetRandomZ());
                        default:
                            return 0;
                    }
                }
            default:
                return 0;
        }
    }

    public bool returnBool(object sentObject)
    {
        object obj = sentObject;
        if (parameters != null)
        {
            obj = parameters;
        }
        switch (helperClass)
        {
            case 0:
                return (bool)obj;
            case 5:
                switch (helperType)
                {
                    case 1:
                        return (bool)obj;
                    case 0:
                        {
                            int value3 = (int)obj;
                            return Convert.ToBoolean(value3);
                        }
                    case 2:
                        {
                            string value2 = (string)obj;
                            return Convert.ToBoolean(value2);
                        }
                    case 3:
                        {
                            float value = (float)obj;
                            return Convert.ToBoolean(value);
                        }
                    default:
                        return false;
                }
            case 1:
                {
                    RCActionHelper rCActionHelper2 = (RCActionHelper)obj;
                    switch (helperType)
                    {
                        case 1:
                            return (bool)FengGameManagerMKII.BoolVariables[rCActionHelper2.returnString(null)];
                        case 0:
                            return nextHelper.returnBool(FengGameManagerMKII.IntVariables[rCActionHelper2.returnString(null)]);
                        case 2:
                            return nextHelper.returnBool(FengGameManagerMKII.StringVariables[rCActionHelper2.returnString(null)]);
                        case 3:
                            return nextHelper.returnBool(FengGameManagerMKII.FloatVariables[rCActionHelper2.returnString(null)]);
                        case 4:
                            return nextHelper.returnBool(FengGameManagerMKII.PlayerVariables[rCActionHelper2.returnString(null)]);
                        case 5:
                            return nextHelper.returnBool(FengGameManagerMKII.TitanVariables[rCActionHelper2.returnString(null)]);
                        default:
                            return false;
                    }
                }
            case 2:
                {
                    PhotonPlayer photonPlayer = (PhotonPlayer)obj;
                    if (photonPlayer != null)
                    {
                        switch (helperType)
                        {
                            case 0:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.Team]);
                            case 1:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.RCTeam]);
                            case 2:
                                return !(bool)photonPlayer.customProperties[PhotonPlayerProperty.Dead];
                            case 3:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.IsTitan]);
                            case 4:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.Kills]);
                            case 5:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.Deaths]);
                            case 6:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.MaxDamage]);
                            case 7:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.TotalDamage]);
                            case 8:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.CustomInt]);
                            case 9:
                                return (bool)photonPlayer.customProperties[PhotonPlayerProperty.CustomBool];
                            case 10:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.CustomString]);
                            case 11:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.CustomFloat]);
                            case 14:
                                {
                                    int iD4 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD4))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD4];
                                        return nextHelper.returnBool(hERO.transform.position.x);
                                    }
                                    return false;
                                }
                            case 15:
                                {
                                    int iD3 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD3))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD3];
                                        return nextHelper.returnBool(hERO.transform.position.y);
                                    }
                                    return false;
                                }
                            case 16:
                                {
                                    int iD2 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD2))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD2];
                                        return nextHelper.returnBool(hERO.transform.position.z);
                                    }
                                    return false;
                                }
                            case 12:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.Name]);
                            case 13:
                                return nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.Guild]);
                            case 17:
                                {
                                    int iD = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD];
                                        return nextHelper.returnBool(hERO.rigidbody.velocity.magnitude);
                                    }
                                    return false;
                                }
                        }
                    }
                    return false;
                }
            case 3:
                {
                    TITAN tITAN = (TITAN)obj;
                    if (tITAN != null)
                    {
                        switch (helperType)
                        {
                            case 0:
                                return nextHelper.returnBool(tITAN.abnormalType);
                            case 1:
                                return nextHelper.returnBool(tITAN.myLevel);
                            case 2:
                                return nextHelper.returnBool(tITAN.currentHealth);
                            case 3:
                                return nextHelper.returnBool(tITAN.transform.position.x);
                            case 4:
                                return nextHelper.returnBool(tITAN.transform.position.y);
                            case 5:
                                return nextHelper.returnBool(tITAN.transform.position.z);
                        }
                    }
                    return false;
                }
            case 4:
                {
                    RCActionHelper rCActionHelper = (RCActionHelper)obj;
                    RCRegion rCRegion = (RCRegion)FengGameManagerMKII.RCRegions[rCActionHelper.returnString(null)];
                    switch (helperType)
                    {
                        case 0:
                            return nextHelper.returnBool(rCRegion.GetRandomX());
                        case 1:
                            return nextHelper.returnBool(rCRegion.GetRandomY());
                        case 2:
                            return nextHelper.returnBool(rCRegion.GetRandomZ());
                        default:
                            return false;
                    }
                }
            default:
                return false;
        }
    }

    public string returnString(object sentObject)
    {
        object obj = sentObject;
        if (parameters != null)
        {
            obj = parameters;
        }
        switch (helperClass)
        {
            case 0:
                return (string)obj;
            case 5:
                switch (helperType)
                {
                    case 0:
                        return ((int)obj).ToString();
                    case 1:
                        return ((bool)obj).ToString();
                    case 3:
                        return ((float)obj).ToString();
                    case 2:
                        return (string)obj;
                    default:
                        return string.Empty;
                }
            case 1:
                {
                    RCActionHelper rCActionHelper2 = (RCActionHelper)obj;
                    switch (helperType)
                    {
                        case 0:
                            return nextHelper.returnString(FengGameManagerMKII.IntVariables[rCActionHelper2.returnString(null)]);
                        case 1:
                            return nextHelper.returnString(FengGameManagerMKII.BoolVariables[rCActionHelper2.returnString(null)]);
                        case 2:
                            return (string)FengGameManagerMKII.StringVariables[rCActionHelper2.returnString(null)];
                        case 3:
                            return nextHelper.returnString(FengGameManagerMKII.FloatVariables[rCActionHelper2.returnString(null)]);
                        case 4:
                            return nextHelper.returnString(FengGameManagerMKII.PlayerVariables[rCActionHelper2.returnString(null)]);
                        case 5:
                            return nextHelper.returnString(FengGameManagerMKII.TitanVariables[rCActionHelper2.returnString(null)]);
                        default:
                            return string.Empty;
                    }
                }
            case 2:
                {
                    PhotonPlayer photonPlayer = (PhotonPlayer)obj;
                    if (photonPlayer != null)
                    {
                        switch (helperType)
                        {
                            case 0:
                                return nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.Team]);
                            case 1:
                                return nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.RCTeam]);
                            case 2:
                                return nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.Dead]);
                            case 3:
                                return nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.IsTitan]);
                            case 4:
                                return nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.Kills]);
                            case 5:
                                return nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.Deaths]);
                            case 6:
                                return nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.MaxDamage]);
                            case 7:
                                return nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.TotalDamage]);
                            case 8:
                                return nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.CustomInt]);
                            case 9:
                                return nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.CustomBool]);
                            case 10:
                                return (string)photonPlayer.customProperties[PhotonPlayerProperty.CustomString];
                            case 11:
                                return nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.CustomFloat]);
                            case 14:
                                {
                                    int iD4 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD4))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD4];
                                        return nextHelper.returnString(hERO.transform.position.x);
                                    }
                                    return string.Empty;
                                }
                            case 15:
                                {
                                    int iD3 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD3))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD3];
                                        return nextHelper.returnString(hERO.transform.position.y);
                                    }
                                    return string.Empty;
                                }
                            case 16:
                                {
                                    int iD2 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD2))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD2];
                                        return nextHelper.returnString(hERO.transform.position.z);
                                    }
                                    return string.Empty;
                                }
                            case 12:
                                return (string)photonPlayer.customProperties[PhotonPlayerProperty.Name];
                            case 13:
                                return (string)photonPlayer.customProperties[PhotonPlayerProperty.Guild];
                            case 17:
                                {
                                    int iD = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD];
                                        return nextHelper.returnString(hERO.rigidbody.velocity.magnitude);
                                    }
                                    return string.Empty;
                                }
                        }
                    }
                    return string.Empty;
                }
            case 3:
                {
                    TITAN tITAN = (TITAN)obj;
                    if (tITAN != null)
                    {
                        switch (helperType)
                        {
                            case 0:
                                return nextHelper.returnString(tITAN.abnormalType);
                            case 1:
                                return nextHelper.returnString(tITAN.myLevel);
                            case 2:
                                return nextHelper.returnString(tITAN.currentHealth);
                            case 3:
                                return nextHelper.returnString(tITAN.transform.position.x);
                            case 4:
                                return nextHelper.returnString(tITAN.transform.position.y);
                            case 5:
                                return nextHelper.returnString(tITAN.transform.position.z);
                        }
                    }
                    return string.Empty;
                }
            case 4:
                {
                    RCActionHelper rCActionHelper = (RCActionHelper)obj;
                    RCRegion rCRegion = (RCRegion)FengGameManagerMKII.RCRegions[rCActionHelper.returnString(null)];
                    switch (helperType)
                    {
                        case 0:
                            return nextHelper.returnString(rCRegion.GetRandomX());
                        case 1:
                            return nextHelper.returnString(rCRegion.GetRandomY());
                        case 2:
                            return nextHelper.returnString(rCRegion.GetRandomZ());
                        default:
                            return string.Empty;
                    }
                }
            default:
                return string.Empty;
        }
    }

    public float returnFloat(object sentObject)
    {
        object obj = sentObject;
        if (parameters != null)
        {
            obj = parameters;
        }
        switch (helperClass)
        {
            case 0:
                return (float)obj;
            case 5:
                switch (helperType)
                {
                    case 0:
                        {
                            int value2 = (int)obj;
                            return Convert.ToSingle(value2);
                        }
                    case 1:
                        {
                            bool value = (bool)obj;
                            return Convert.ToSingle(value);
                        }
                    case 3:
                        return (float)obj;
                    case 2:
                        {
                            string text = (string)obj;
                            if (float.TryParse((string)obj, out float result))
                            {
                                return result;
                            }
                            return 0f;
                        }
                    default:
                        return (float)obj;
                }
            case 1:
                {
                    RCActionHelper rCActionHelper2 = (RCActionHelper)obj;
                    switch (helperType)
                    {
                        case 0:
                            return nextHelper.returnFloat(FengGameManagerMKII.IntVariables[rCActionHelper2.returnString(null)]);
                        case 1:
                            return nextHelper.returnFloat(FengGameManagerMKII.BoolVariables[rCActionHelper2.returnString(null)]);
                        case 2:
                            return nextHelper.returnFloat(FengGameManagerMKII.StringVariables[rCActionHelper2.returnString(null)]);
                        case 3:
                            return (float)FengGameManagerMKII.FloatVariables[rCActionHelper2.returnString(null)];
                        case 4:
                            return nextHelper.returnFloat(FengGameManagerMKII.PlayerVariables[rCActionHelper2.returnString(null)]);
                        case 5:
                            return nextHelper.returnFloat(FengGameManagerMKII.TitanVariables[rCActionHelper2.returnString(null)]);
                        default:
                            return 0f;
                    }
                }
            case 2:
                {
                    PhotonPlayer photonPlayer = (PhotonPlayer)obj;
                    if (photonPlayer != null)
                    {
                        switch (helperType)
                        {
                            case 0:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.Team]);
                            case 1:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.RCTeam]);
                            case 2:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.Dead]);
                            case 3:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.IsTitan]);
                            case 4:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.Kills]);
                            case 5:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.Deaths]);
                            case 6:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.MaxDamage]);
                            case 7:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.TotalDamage]);
                            case 8:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.CustomInt]);
                            case 9:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.CustomBool]);
                            case 10:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.CustomString]);
                            case 11:
                                return (float)photonPlayer.customProperties[PhotonPlayerProperty.CustomFloat];
                            case 14:
                                {
                                    int iD4 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD4))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD4];
                                        return hERO.transform.position.x;
                                    }
                                    return 0f;
                                }
                            case 15:
                                {
                                    int iD3 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD3))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD3];
                                        return hERO.transform.position.y;
                                    }
                                    return 0f;
                                }
                            case 16:
                                {
                                    int iD2 = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD2))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD2];
                                        return hERO.transform.position.z;
                                    }
                                    return 0f;
                                }
                            case 12:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.Name]);
                            case 13:
                                return nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.Guild]);
                            case 17:
                                {
                                    int iD = photonPlayer.Id;
                                    if (FengGameManagerMKII.HeroHash.ContainsKey(iD))
                                    {
                                        HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD];
                                        return hERO.rigidbody.velocity.magnitude;
                                    }
                                    return 0f;
                                }
                        }
                    }
                    return 0f;
                }
            case 3:
                {
                    TITAN tITAN = (TITAN)obj;
                    if (tITAN != null)
                    {
                        switch (helperType)
                        {
                            case 0:
                                return nextHelper.returnFloat(tITAN.abnormalType);
                            case 1:
                                return tITAN.myLevel;
                            case 2:
                                return nextHelper.returnFloat(tITAN.currentHealth);
                            case 3:
                                return tITAN.transform.position.x;
                            case 4:
                                return tITAN.transform.position.y;
                            case 5:
                                return tITAN.transform.position.z;
                        }
                    }
                    return 0f;
                }
            case 4:
                {
                    RCActionHelper rCActionHelper = (RCActionHelper)obj;
                    RCRegion rCRegion = (RCRegion)FengGameManagerMKII.RCRegions[rCActionHelper.returnString(null)];
                    switch (helperType)
                    {
                        case 0:
                            return rCRegion.GetRandomX();
                        case 1:
                            return rCRegion.GetRandomY();
                        case 2:
                            return rCRegion.GetRandomZ();
                        default:
                            return 0f;
                    }
                }
            default:
                return 0f;
        }
    }

    public PhotonPlayer returnPlayer(object objParameter)
    {
        object obj = objParameter;
        if (parameters != null)
        {
            obj = parameters;
        }
        switch (helperClass)
        {
            case 1:
                {
                    RCActionHelper rCActionHelper = (RCActionHelper)obj;
                    return (PhotonPlayer)FengGameManagerMKII.PlayerVariables[rCActionHelper.returnString(null)];
                }
            case 2:
                return (PhotonPlayer)obj;
            default:
                return (PhotonPlayer)obj;
        }
    }

    public TITAN returnTitan(object objParameter)
    {
        object obj = objParameter;
        if (parameters != null)
        {
            obj = parameters;
        }
        switch (helperClass)
        {
            case 1:
                {
                    RCActionHelper rCActionHelper = (RCActionHelper)obj;
                    return (TITAN)FengGameManagerMKII.TitanVariables[rCActionHelper.returnString(null)];
                }
            case 3:
                return (TITAN)obj;
            default:
                return (TITAN)obj;
        }
    }
}
