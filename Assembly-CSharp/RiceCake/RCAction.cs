using ExitGames.Client.Photon;
using System;

public class RCAction
{
    public enum actionClasses
    {
        typeVoid,
        typeVariableInt,
        typeVariableBool,
        typeVariableString,
        typeVariableFloat,
        typeVariablePlayer,
        typeVariableTitan,
        typePlayer,
        typeTitan,
        typeGame
    }

    public enum varTypes
    {
        set,
        add,
        subtract,
        multiply,
        divide,
        modulo,
        power,
        concat,
        append,
        remove,
        replace,
        toOpposite,
        setRandom
    }

    public enum playerTypes
    {
        killPlayer,
        spawnPlayer,
        spawnPlayerAt,
        movePlayer,
        setKills,
        setDeaths,
        setMaxDmg,
        setTotalDmg,
        setName,
        setGuildName,
        setTeam,
        setCustomInt,
        setCustomBool,
        setCustomString,
        setCustomFloat
    }

    public enum titanTypes
    {
        killTitan,
        spawnTitan,
        spawnTitanAt,
        setHealth,
        moveTitan
    }

    public enum gameTypes
    {
        printMessage,
        winGame,
        loseGame,
        restartGame
    }

    private int actionClass;
    private int actionType;
    private RCEvent nextEvent;
    private RCActionHelper[] parameters;

    public RCAction(int category, int type, RCEvent next, RCActionHelper[] helpers)
    {
        actionClass = category;
        actionType = type;
        nextEvent = next;
        parameters = helpers;
    }

    public void CallException(string str)
    {
        InRoomChat.Instance.AddLine(str);
    }

    public void DoAction()
    {
        switch (actionClass)
        {
            case 0:
                nextEvent.CheckEvent();
                break;
            case 1:
                {
                    string text5 = parameters[0].ReturnString(null);
                    int num3 = parameters[1].ReturnInt(null);
                    switch (actionType)
                    {
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            break;
                        case 0:
                            if (!FengGameManagerMKII.IntVariables.ContainsKey(text5))
                            {
                                FengGameManagerMKII.IntVariables.Add(text5, num3);
                            }
                            else
                            {
                                FengGameManagerMKII.IntVariables[text5] = num3;
                            }
                            break;
                        case 1:
                            if (!FengGameManagerMKII.IntVariables.ContainsKey(text5))
                            {
                                CallException("Variable not found: " + text5);
                            }
                            else
                            {
                                FengGameManagerMKII.IntVariables[text5] = (int)FengGameManagerMKII.IntVariables[text5] + num3;
                            }
                            break;
                        case 2:
                            if (!FengGameManagerMKII.IntVariables.ContainsKey(text5))
                            {
                                CallException("Variable not found: " + text5);
                            }
                            else
                            {
                                FengGameManagerMKII.IntVariables[text5] = (int)FengGameManagerMKII.IntVariables[text5] - num3;
                            }
                            break;
                        case 3:
                            if (!FengGameManagerMKII.IntVariables.ContainsKey(text5))
                            {
                                CallException("Variable not found: " + text5);
                            }
                            else
                            {
                                FengGameManagerMKII.IntVariables[text5] = (int)FengGameManagerMKII.IntVariables[text5] * num3;
                            }
                            break;
                        case 4:
                            if (!FengGameManagerMKII.IntVariables.ContainsKey(text5))
                            {
                                CallException("Variable not found: " + text5);
                            }
                            else
                            {
                                FengGameManagerMKII.IntVariables[text5] = (int)FengGameManagerMKII.IntVariables[text5] / num3;
                            }
                            break;
                        case 5:
                            if (!FengGameManagerMKII.IntVariables.ContainsKey(text5))
                            {
                                CallException("Variable not found: " + text5);
                            }
                            else
                            {
                                FengGameManagerMKII.IntVariables[text5] = (int)FengGameManagerMKII.IntVariables[text5] % num3;
                            }
                            break;
                        case 6:
                            if (!FengGameManagerMKII.IntVariables.ContainsKey(text5))
                            {
                                CallException("Variable not found: " + text5);
                            }
                            else
                            {
                                FengGameManagerMKII.IntVariables[text5] = (int)Math.Pow((int)FengGameManagerMKII.IntVariables[text5], num3);
                            }
                            break;
                        case 12:
                            if (!FengGameManagerMKII.IntVariables.ContainsKey(text5))
                            {
                                FengGameManagerMKII.IntVariables.Add(text5, UnityEngine.Random.Range(num3, parameters[2].ReturnInt(null)));
                            }
                            else
                            {
                                FengGameManagerMKII.IntVariables[text5] = UnityEngine.Random.Range(num3, parameters[2].ReturnInt(null));
                            }
                            break;
                    }
                    break;
                }
            case 2:
                {
                    string text4 = parameters[0].ReturnString(null);
                    bool flag = parameters[1].ReturnBool(null);
                    switch (actionType)
                    {
                        case 0:
                            if (!FengGameManagerMKII.BoolVariables.ContainsKey(text4))
                            {
                                FengGameManagerMKII.BoolVariables.Add(text4, flag);
                            }
                            else
                            {
                                FengGameManagerMKII.BoolVariables[text4] = flag;
                            }
                            break;
                        case 11:
                            if (!FengGameManagerMKII.BoolVariables.ContainsKey(text4))
                            {
                                CallException("Variable not found: " + text4);
                            }
                            else
                            {
                                FengGameManagerMKII.BoolVariables[text4] = !(bool)FengGameManagerMKII.BoolVariables[text4];
                            }
                            break;
                        case 12:
                            if (!FengGameManagerMKII.BoolVariables.ContainsKey(text4))
                            {
                                FengGameManagerMKII.BoolVariables.Add(text4, Convert.ToBoolean(UnityEngine.Random.Range(0, 2)));
                            }
                            else
                            {
                                FengGameManagerMKII.BoolVariables[text4] = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
                            }
                            break;
                    }
                    break;
                }
            case 3:
                {
                    string key3 = parameters[0].ReturnString(null);
                    switch (actionType)
                    {
                        case 0:
                            {
                                string value2 = parameters[1].ReturnString(null);
                                if (!FengGameManagerMKII.StringVariables.ContainsKey(key3))
                                {
                                    FengGameManagerMKII.StringVariables.Add(key3, value2);
                                }
                                else
                                {
                                    FengGameManagerMKII.StringVariables[key3] = value2;
                                }
                                break;
                            }
                        case 7:
                            {
                                string text3 = string.Empty;
                                for (int i = 1; i < parameters.Length; i++)
                                {
                                    text3 += parameters[i].ReturnString(null);
                                }
                                if (!FengGameManagerMKII.StringVariables.ContainsKey(key3))
                                {
                                    FengGameManagerMKII.StringVariables.Add(key3, text3);
                                }
                                else
                                {
                                    FengGameManagerMKII.StringVariables[key3] = text3;
                                }
                                break;
                            }
                        case 8:
                            {
                                string str = parameters[1].ReturnString(null);
                                if (!FengGameManagerMKII.StringVariables.ContainsKey(key3))
                                {
                                    CallException("No Variable");
                                }
                                else
                                {
                                    FengGameManagerMKII.StringVariables[key3] = (string)FengGameManagerMKII.StringVariables[key3] + str;
                                }
                                break;
                            }
                        case 9:
                            {
                                string text = parameters[1].ReturnString(null);
                                if (!FengGameManagerMKII.StringVariables.ContainsKey(key3))
                                {
                                    CallException("No Variable");
                                    break;
                                }
                                string text2 = (string)FengGameManagerMKII.StringVariables[key3];
                                FengGameManagerMKII.StringVariables[key3] = text2.Replace(parameters[1].ReturnString(null), parameters[2].ReturnString(null));
                                break;
                            }
                    }
                    break;
                }
            case 4:
                {
                    string key2 = parameters[0].ReturnString(null);
                    float num2 = parameters[1].ReturnFloat(null);
                    switch (actionType)
                    {
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            break;
                        case 0:
                            if (!FengGameManagerMKII.FloatVariables.ContainsKey(key2))
                            {
                                FengGameManagerMKII.FloatVariables.Add(key2, num2);
                            }
                            else
                            {
                                FengGameManagerMKII.FloatVariables[key2] = num2;
                            }
                            break;
                        case 1:
                            if (!FengGameManagerMKII.FloatVariables.ContainsKey(key2))
                            {
                                CallException("No Variable");
                            }
                            else
                            {
                                FengGameManagerMKII.FloatVariables[key2] = (float)FengGameManagerMKII.FloatVariables[key2] + num2;
                            }
                            break;
                        case 2:
                            if (!FengGameManagerMKII.FloatVariables.ContainsKey(key2))
                            {
                                CallException("No Variable");
                            }
                            else
                            {
                                FengGameManagerMKII.FloatVariables[key2] = (float)FengGameManagerMKII.FloatVariables[key2] - num2;
                            }
                            break;
                        case 3:
                            if (!FengGameManagerMKII.FloatVariables.ContainsKey(key2))
                            {
                                CallException("No Variable");
                            }
                            else
                            {
                                FengGameManagerMKII.FloatVariables[key2] = (float)FengGameManagerMKII.FloatVariables[key2] * num2;
                            }
                            break;
                        case 4:
                            if (!FengGameManagerMKII.FloatVariables.ContainsKey(key2))
                            {
                                CallException("No Variable");
                            }
                            else
                            {
                                FengGameManagerMKII.FloatVariables[key2] = (float)FengGameManagerMKII.FloatVariables[key2] / num2;
                            }
                            break;
                        case 5:
                            if (!FengGameManagerMKII.FloatVariables.ContainsKey(key2))
                            {
                                CallException("No Variable");
                            }
                            else
                            {
                                FengGameManagerMKII.FloatVariables[key2] = (float)FengGameManagerMKII.FloatVariables[key2] % num2;
                            }
                            break;
                        case 6:
                            if (!FengGameManagerMKII.FloatVariables.ContainsKey(key2))
                            {
                                CallException("No Variable");
                            }
                            else
                            {
                                FengGameManagerMKII.FloatVariables[key2] = (float)Math.Pow((int)FengGameManagerMKII.FloatVariables[key2], num2);
                            }
                            break;
                        case 12:
                            if (!FengGameManagerMKII.FloatVariables.ContainsKey(key2))
                            {
                                FengGameManagerMKII.FloatVariables.Add(key2, UnityEngine.Random.Range(num2, parameters[2].ReturnFloat(null)));
                            }
                            else
                            {
                                FengGameManagerMKII.FloatVariables[key2] = UnityEngine.Random.Range(num2, parameters[2].ReturnFloat(null));
                            }
                            break;
                    }
                    break;
                }
            case 5:
                {
                    string key4 = parameters[0].ReturnString(null);
                    PhotonPlayer value3 = parameters[1].ReturnPlayer(null);
                    if (actionType == 0)
                    {
                        if (!FengGameManagerMKII.PlayerVariables.ContainsKey(key4))
                        {
                            FengGameManagerMKII.PlayerVariables.Add(key4, value3);
                        }
                        else
                        {
                            FengGameManagerMKII.PlayerVariables[key4] = value3;
                        }
                    }
                    break;
                }
            case 6:
                {
                    string key = parameters[0].ReturnString(null);
                    TITAN value = parameters[1].ReturnTitan(null);
                    if (actionType == 0)
                    {
                        if (!FengGameManagerMKII.TitanVariables.ContainsKey(key))
                        {
                            FengGameManagerMKII.TitanVariables.Add(key, value);
                        }
                        else
                        {
                            FengGameManagerMKII.TitanVariables[key] = value;
                        }
                    }
                    break;
                }
            case 7:
                {
                    PhotonPlayer photonPlayer = parameters[0].ReturnPlayer(null);
                    switch (actionType)
                    {
                        case 0:
                            {
                                if (FengGameManagerMKII.HeroHash.ContainsKey(photonPlayer.Id))
                                {
                                    HERO hERO2 = (HERO)FengGameManagerMKII.HeroHash[photonPlayer.Id];
                                    hERO2.MarkDead();
                                    hERO2.photonView.RPC("netDie2", PhotonTargets.All, -1, parameters[1].ReturnString(null) + " ");
                                }
                                else
                                {
                                    CallException("Player Not Alive");
                                }
                                break;
                            }
                        case 1:
                            FengGameManagerMKII.Instance.photonView.RPC("respawnHeroInNewRound", photonPlayer);
                            break;
                        case 2:
                            FengGameManagerMKII.Instance.photonView.RPC("spawnPlayerAtRPC", photonPlayer, parameters[1].ReturnFloat(null), parameters[2].ReturnFloat(null), parameters[3].ReturnFloat(null));
                            break;
                        case 3:
                            {
                                int iD = photonPlayer.Id;
                                if (FengGameManagerMKII.HeroHash.ContainsKey(iD))
                                {
                                    HERO hERO = (HERO)FengGameManagerMKII.HeroHash[iD];
                                    hERO.photonView.RPC("moveToRPC", photonPlayer, parameters[1].ReturnFloat(null), parameters[2].ReturnFloat(null), parameters[3].ReturnFloat(null));
                                }
                                else
                                {
                                    CallException("Player Not Alive");
                                }
                                break;
                            }
                        case 4:
                            {
                                Hashtable hashtable11 = new Hashtable();
                                hashtable11.Add(PhotonPlayerProperty.Kills, parameters[1].ReturnInt(null));
                                photonPlayer.SetCustomProperties(hashtable11);
                                break;
                            }
                        case 5:
                            {
                                Hashtable hashtable10 = new Hashtable();
                                hashtable10.Add(PhotonPlayerProperty.Deaths, parameters[1].ReturnInt(null));
                                photonPlayer.SetCustomProperties(hashtable10);
                                break;
                            }
                        case 6:
                            {
                                Hashtable hashtable9 = new Hashtable();
                                hashtable9.Add(PhotonPlayerProperty.MaxDamage, parameters[1].ReturnInt(null));
                                photonPlayer.SetCustomProperties(hashtable9);
                                break;
                            }
                        case 7:
                            {
                                Hashtable hashtable8 = new Hashtable();
                                hashtable8.Add(PhotonPlayerProperty.TotalDamage, parameters[1].ReturnInt(null));
                                photonPlayer.SetCustomProperties(hashtable8);
                                break;
                            }
                        case 8:
                            {
                                Hashtable hashtable7 = new Hashtable();
                                hashtable7.Add(PhotonPlayerProperty.Name, parameters[1].ReturnString(null));
                                photonPlayer.SetCustomProperties(hashtable7);
                                break;
                            }
                        case 9:
                            {
                                Hashtable hashtable6 = new Hashtable();
                                hashtable6.Add(PhotonPlayerProperty.Guild, parameters[1].ReturnString(null));
                                photonPlayer.SetCustomProperties(hashtable6);
                                break;
                            }
                        case 10:
                            {
                                Hashtable hashtable5 = new Hashtable();
                                hashtable5.Add(PhotonPlayerProperty.RCTeam, parameters[1].ReturnInt(null));
                                photonPlayer.SetCustomProperties(hashtable5);
                                break;
                            }
                        case 11:
                            {
                                Hashtable hashtable4 = new Hashtable();
                                hashtable4.Add(PhotonPlayerProperty.CustomInt, parameters[1].ReturnInt(null));
                                photonPlayer.SetCustomProperties(hashtable4);
                                break;
                            }
                        case 12:
                            {
                                Hashtable hashtable3 = new Hashtable();
                                hashtable3.Add(PhotonPlayerProperty.CustomBool, parameters[1].ReturnBool(null));
                                photonPlayer.SetCustomProperties(hashtable3);
                                break;
                            }
                        case 13:
                            {
                                Hashtable hashtable2 = new Hashtable();
                                hashtable2.Add(PhotonPlayerProperty.CustomString, parameters[1].ReturnString(null));
                                photonPlayer.SetCustomProperties(hashtable2);
                                break;
                            }
                        case 14:
                            {
                                Hashtable hashtable = new Hashtable();
                                hashtable.Add(PhotonPlayerProperty.RCTeam, parameters[1].ReturnFloat(null));
                                photonPlayer.SetCustomProperties(hashtable);
                                break;
                            }
                    }
                    break;
                }
            case 8:
                switch (actionType)
                {
                    case 0:
                        {
                            TITAN titanObj = parameters[0].ReturnTitan(null);
                            object[] array = new object[2]
                            {
                                parameters[1].ReturnPlayer(null).Id,
                                parameters[2].ReturnInt(null)
                            };
                            titanObj.photonView.RPC("titanGetHit", titanObj.photonView.owner, array);
                            break;
                        }
                    case 1:
                        FengGameManagerMKII.Instance.SpawnTitanAction(parameters[0].ReturnInt(null), parameters[1].ReturnFloat(null), parameters[2].ReturnInt(null), parameters[3].ReturnInt(null));
                        break;
                    case 2:
                        FengGameManagerMKII.Instance.SpawnTitanAtAction(parameters[0].ReturnInt(null), parameters[1].ReturnFloat(null), parameters[2].ReturnInt(null), parameters[3].ReturnInt(null), parameters[4].ReturnFloat(null), parameters[5].ReturnFloat(null), parameters[6].ReturnFloat(null));
                        break;
                    case 3:
                        {
                            TITAN titanObj = parameters[0].ReturnTitan(null);
                            int num = titanObj.currentHealth = parameters[1].ReturnInt(null);
                            if (titanObj.maxHealth == 0)
                            {
                                titanObj.maxHealth = titanObj.currentHealth;
                            }
                            titanObj.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, titanObj.currentHealth, titanObj.maxHealth);
                            break;
                        }
                    case 4:
                        {
                            TITAN titanObj = parameters[0].ReturnTitan(null);
                            if (titanObj.photonView.isMine)
                            {
                                titanObj.MoveTo(parameters[1].ReturnFloat(null), parameters[2].ReturnFloat(null), parameters[3].ReturnFloat(null));
                            }
                            else
                            {
                                titanObj.photonView.RPC("moveToRPC", titanObj.photonView.owner, parameters[1].ReturnFloat(null), parameters[2].ReturnFloat(null), parameters[3].ReturnFloat(null));
                            }
                            break;
                        }
                }
                break;
            case 9:
                switch (actionType)
                {
                    case 0:
                        FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, parameters[0].ReturnString(null), string.Empty);
                        break;
                    case 2:
                        FengGameManagerMKII.Instance.FinishGame(true);
                        if (parameters[0].ReturnBool(null))
                        {
                            FengGameManagerMKII.IntVariables.Clear();
                            FengGameManagerMKII.BoolVariables.Clear();
                            FengGameManagerMKII.StringVariables.Clear();
                            FengGameManagerMKII.FloatVariables.Clear();
                            FengGameManagerMKII.PlayerVariables.Clear();
                            FengGameManagerMKII.TitanVariables.Clear();
                        }
                        break;
                    case 1:
                        FengGameManagerMKII.Instance.FinishGame();
                        if (parameters[0].ReturnBool(null))
                        {
                            FengGameManagerMKII.IntVariables.Clear();
                            FengGameManagerMKII.BoolVariables.Clear();
                            FengGameManagerMKII.StringVariables.Clear();
                            FengGameManagerMKII.FloatVariables.Clear();
                            FengGameManagerMKII.PlayerVariables.Clear();
                            FengGameManagerMKII.TitanVariables.Clear();
                        }
                        break;
                    case 3:
                        if (parameters[0].ReturnBool(null))
                        {
                            FengGameManagerMKII.IntVariables.Clear();
                            FengGameManagerMKII.BoolVariables.Clear();
                            FengGameManagerMKII.StringVariables.Clear();
                            FengGameManagerMKII.FloatVariables.Clear();
                            FengGameManagerMKII.PlayerVariables.Clear();
                            FengGameManagerMKII.TitanVariables.Clear();
                        }
                        FengGameManagerMKII.Instance.RestartGame();
                        break;
                }
                break;
        }
    }
}
