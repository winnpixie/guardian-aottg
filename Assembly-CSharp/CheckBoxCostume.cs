using UnityEngine;

public class CheckBoxCostume : MonoBehaviour
{
    public int set = 1;
    public static int costumeSet;

    private void OnActivate(bool yes)
    {
        if (yes)
        {
            costumeSet = set;
        }
    }
}
