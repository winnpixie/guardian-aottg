using UnityEngine;

public class MapNameChange : MonoBehaviour
{
    private UIPopupList mapList;

    void Awake()
    {
        mapList = GetComponent<UIPopupList>();
    }

    private void OnSelectionChange()
    {
        LevelInfo info = LevelInfo.GetInfo(mapList.selection);
        if (info == null) return;

        GameObject.Find("LabelLevelInfo").GetComponent<UILabel>().text = info.Description;
    }
}
