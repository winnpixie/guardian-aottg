using UnityEngine;

public class MapNameChange : MonoBehaviour
{
    private void OnSelectionChange()
    {
        UIPopupList list = GetComponent<UIPopupList>();
        LevelInfo info = LevelInfo.GetInfo(list.selection);
        if (info != null)
        {
            GameObject.Find("LabelLevelInfo").GetComponent<UILabel>().text = info.Description;
        }
        LevelInfo.InitData();

        bool shouldResize = false;
        foreach (LevelInfo levelInfo in LevelInfo.Levels)
        {
            if (!list.items.Contains(levelInfo.Name) && !levelInfo.Name.StartsWith("[S]") && !levelInfo.Name.Equals("Cage Fighting"))
            {
                list.items.Add(levelInfo.Name);
                shouldResize = true;
            }
        }
        if (shouldResize)
        {
            list.textScale *= 0.55f;
        }
    }
}
