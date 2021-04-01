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
    }
}
