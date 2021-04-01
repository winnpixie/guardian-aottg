using System.Collections.Generic;

public class RCEvent
{
    public enum loopType
    {
        noLoop,
        ifLoop,
        foreachLoop,
        whileLoop
    }

    public enum foreachType
    {
        titan,
        player
    }

    private RCCondition condition;
    public List<RCAction> trueActions;
    public string foreachVariableName;
    private RCAction elseAction;
    private int eventClass;
    private int eventType;

    public RCEvent(RCCondition sentCondition, List<RCAction> sentTrueActions, int sentClass, int sentType)
    {
        condition = sentCondition;
        trueActions = sentTrueActions;
        eventClass = sentClass;
        eventType = sentType;
    }

    public void SetElse(RCAction sentElse)
    {
        elseAction = sentElse;
    }

    public void CheckEvent()
    {
        switch (eventClass)
        {
            default:
                return;
            case 0:
                {
                    for (int j = 0; j < trueActions.Count; j++)
                    {
                        trueActions[j].DoAction();
                    }
                    return;
                }
            case 1:
                if (condition.CheckCondition())
                {
                    for (int j = 0; j < trueActions.Count; j++)
                    {
                        trueActions[j].DoAction();
                    }
                }
                else if (elseAction != null)
                {
                    elseAction.DoAction();
                }
                return;
            case 2:
                switch (eventType)
                {
                    case 0:
                        foreach (TITAN titan in FengGameManagerMKII.Instance.titans)
                        {
                            if (FengGameManagerMKII.TitanVariables.ContainsKey(foreachVariableName))
                            {
                                FengGameManagerMKII.TitanVariables[foreachVariableName] = titan;
                            }
                            else
                            {
                                FengGameManagerMKII.TitanVariables.Add(foreachVariableName, titan);
                            }
                            foreach (RCAction trueAction in trueActions)
                            {
                                trueAction.DoAction();
                            }
                        }
                        break;
                    case 1:
                        {
                            PhotonPlayer[] playerList = PhotonNetwork.playerList;
                            foreach (PhotonPlayer value in playerList)
                            {
                                if (FengGameManagerMKII.PlayerVariables.ContainsKey(foreachVariableName))
                                {
                                    FengGameManagerMKII.PlayerVariables[foreachVariableName] = value;
                                }
                                else
                                {
                                    FengGameManagerMKII.TitanVariables.Add(foreachVariableName, value);
                                }
                                foreach (RCAction trueAction2 in trueActions)
                                {
                                    trueAction2.DoAction();
                                }
                            }
                            break;
                        }
                }
                return;
            case 3:
                break;
        }
        while (condition.CheckCondition())
        {
            foreach (RCAction trueAction3 in trueActions)
            {
                trueAction3.DoAction();
            }
        }
    }
}
