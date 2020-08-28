using UnityEngine;

public class MapNameChange : MonoBehaviour
{
    private void OnSelectionChange()
    {
        UIPopupList list = GetComponent<UIPopupList>();
        LevelInfo info = LevelInfo.getInfo(list.selection);
        if (info != null)
        {
            GameObject.Find("LabelLevelInfo").GetComponent<UILabel>().text = info.desc;
        }

        if (!list.items.Contains("The City II"))
        {
            list.items.Insert(1, "The City II");
            list.textScale *= 0.7f;
        }
        if (list.items.Contains("The City III"))
        {
            list.items.Remove("The City III");
            list.items.Insert(2, "The City III");
        }
        if (!list.items.Contains("Multi-Map"))
        {
            list.items.Add("Multi-Map");
        }
        if (!list.items.Contains("Custom"))
        {
            list.items.Add("Custom");
        }
        if (!list.items.Contains("Custom (No PT)"))
        {
            list.items.Add("Custom (No PT)");
        }
    }
}
