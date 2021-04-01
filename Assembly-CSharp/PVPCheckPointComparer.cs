using System;
using System.Collections;

public class PVPCheckPointComparer : IComparer
{
    public int Compare(object x, object y)
    {
        float xId = ((PVPcheckPoint)x).id;
        float yId = ((PVPcheckPoint)y).id;

        if (xId == yId || Math.Abs(xId - yId) < float.Epsilon)
        {
            return 0;
        }

        if (xId < yId)
        {
            return -1;
        }

        return 1;
    }
}
