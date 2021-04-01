using UnityEngine;

public class CheckBoxCostume : MonoBehaviour
{
    public static int CostumeSet;

    public int set = 1;

    private void OnActivate(bool yes)
    {
        if (yes)
        {
            CostumeSet = set;
        }
    }
}
