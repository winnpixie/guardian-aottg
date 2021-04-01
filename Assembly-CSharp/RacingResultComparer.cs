using System;
using System.Collections;

public class RacingResultComparer : IComparer
{
    public int Compare(object x, object y)
    {
        float xTime = ((RacingResult)x).time;
        float yTime = ((RacingResult)y).time;

        if (xTime == yTime || Math.Abs(xTime - yTime) < float.Epsilon)
        {
            return 0;
        }

        if (xTime < yTime)
        {
            return -1;
        }

        return 1;
    }
}
