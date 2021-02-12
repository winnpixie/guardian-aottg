public class RCCondition
{
    public enum castTypes
    {
        typeInt,
        typeBool,
        typeString,
        typeFloat,
        typePlayer,
        typeTitan
    }

    public enum operands
    {
        lt,
        lte,
        e,
        gte,
        gt,
        ne
    }

    public enum stringOperands
    {
        equals,
        notEquals,
        contains,
        notContains,
        startsWith,
        notStartsWith,
        endsWith,
        notEndsWith
    }

    private int operand;
    private int type;
    private RCActionHelper parameter1;
    private RCActionHelper parameter2;

    public RCCondition(int sentOperand, int sentType, RCActionHelper sentParam1, RCActionHelper sentParam2)
    {
        operand = sentOperand;
        type = sentType;
        parameter1 = sentParam1;
        parameter2 = sentParam2;
    }

    public bool CheckCondition()
    {
        switch (type)
        {
            case 0:
                return IntCompare(parameter1.ReturnInt(null), parameter2.ReturnInt(null));
            case 1:
                return BoolCompare(parameter1.returnBool(null), parameter2.returnBool(null));
            case 2:
                return StringCompare(parameter1.returnString(null), parameter2.returnString(null));
            case 3:
                return FloatCompare(parameter1.returnFloat(null), parameter2.returnFloat(null));
            case 4:
                return PlayerCompare(parameter1.returnPlayer(null), parameter2.returnPlayer(null));
            case 5:
                return TitanCompare(parameter1.returnTitan(null), parameter2.returnTitan(null));
            default:
                return false;
        }
    }

    private bool PlayerCompare(PhotonPlayer basePlayer, PhotonPlayer comparePlayer)
    {
        switch (operand)
        {
            case 2:
                if (basePlayer == comparePlayer)
                {
                    return true;
                }
                return false;
            case 5:
                if (basePlayer != comparePlayer)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    private bool TitanCompare(TITAN baseTitan, TITAN compareTitan)
    {
        switch (operand)
        {
            case 2:
                if (baseTitan == compareTitan)
                {
                    return true;
                }
                return false;
            case 5:
                if (baseTitan != compareTitan)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    private bool BoolCompare(bool baseBool, bool compareBool)
    {
        switch (operand)
        {
            case 2:
                if (baseBool == compareBool)
                {
                    return true;
                }
                return false;
            case 5:
                if (baseBool != compareBool)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    private bool StringCompare(string baseString, string compareString)
    {
        switch (operand)
        {
            case 0:
                if (baseString == compareString)
                {
                    return true;
                }
                return false;
            case 1:
                if (baseString != compareString)
                {
                    return true;
                }
                return false;
            case 2:
                if (baseString.Contains(compareString))
                {
                    return true;
                }
                return false;
            case 3:
                if (!baseString.Contains(compareString))
                {
                    return true;
                }
                return false;
            case 4:
                if (baseString.StartsWith(compareString))
                {
                    return true;
                }
                return false;
            case 5:
                if (!baseString.StartsWith(compareString))
                {
                    return true;
                }
                return false;
            case 6:
                if (baseString.EndsWith(compareString))
                {
                    return true;
                }
                return false;
            case 7:
                if (!baseString.EndsWith(compareString))
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    private bool IntCompare(int baseInt, int compareInt)
    {
        switch (operand)
        {
            case 2:
                if (baseInt == compareInt)
                {
                    return true;
                }
                return false;
            case 5:
                if (baseInt != compareInt)
                {
                    return true;
                }
                return false;
            case 0:
                if (baseInt < compareInt)
                {
                    return true;
                }
                return false;
            case 1:
                if (baseInt <= compareInt)
                {
                    return true;
                }
                return false;
            case 3:
                if (baseInt >= compareInt)
                {
                    return true;
                }
                return false;
            case 4:
                if (baseInt > compareInt)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    private bool FloatCompare(float baseFloat, float compareFloat)
    {
        switch (operand)
        {
            case 0:
                if (baseFloat < compareFloat)
                {
                    return true;
                }
                return false;
            case 1:
                if (baseFloat <= compareFloat)
                {
                    return true;
                }
                return false;
            case 2:
                if (baseFloat == compareFloat)
                {
                    return true;
                }
                return false;
            case 3:
                if (baseFloat >= compareFloat)
                {
                    return true;
                }
                return false;
            case 4:
                if (baseFloat > compareFloat)
                {
                    return true;
                }
                return false;
            case 5:
                if (baseFloat != compareFloat)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }
}
