using System;
using System.Collections;

public class IComparerRacingResult : IComparer
{
    int IComparer.Compare(object x, object y)
    {
        float time = ((RacingResult)x).time;
        float time2 = ((RacingResult)y).time;
        if (time == time2 || Math.Abs(time - time2) < float.Epsilon)
        {
            return 0;
        }
        if (time < time2)
        {
            return -1;
        }
        return 1;
    }
}
