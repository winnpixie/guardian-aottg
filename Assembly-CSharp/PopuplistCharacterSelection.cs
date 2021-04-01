using UnityEngine;

public class PopuplistCharacterSelection : MonoBehaviour
{
    public GameObject SPD;
    public GameObject GAS;
    public GameObject BLA;
    public GameObject ACL;

    private void onCharacterChange()
    {
        string selection = GetComponent<UIPopupList>().selection;
        HeroStat heroStat;
        switch (selection)
        {
            case "Set 1":
            case "Set 2":
            case "Set 3":
                {
                    HeroCostume heroCostume = CostumeConverter.FromLocalData(selection.ToUpper());
                    heroStat = ((heroCostume != null) ? heroCostume.stat : new HeroStat());
                    break;
                }
            default:
                heroStat = HeroStat.GetInfo(GetComponent<UIPopupList>().selection);
                break;
        }
        SPD.transform.localScale = new Vector3(heroStat.Speed, 20f, 0f);
        GAS.transform.localScale = new Vector3(heroStat.Gas, 20f, 0f);
        BLA.transform.localScale = new Vector3(heroStat.Blade, 20f, 0f);
        ACL.transform.localScale = new Vector3(heroStat.Accel, 20f, 0f);
    }
}
