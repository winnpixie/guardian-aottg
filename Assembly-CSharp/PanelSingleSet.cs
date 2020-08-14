using UnityEngine;

public class PanelSingleSet : MonoBehaviour
{
    public GameObject label_START;
    public GameObject label_BACK;
    public GameObject label_camera;
    public GameObject label_original;
    public GameObject label_wow;
    public GameObject label_tps;
    public GameObject label_difficulty;
    public GameObject label_normal;
    public GameObject label_hard;
    public GameObject label_ab;
    public GameObject label_choose_map;
    public GameObject label_choose_character;
    private int lang = -1;

    private void OnEnable()
    {
        UIPopupList list = GameObject.Find("PopupListMap").GetComponent<UIPopupList>();
        if(!list.items.Contains("Custom"))
        {
            list.items.Add("Custom");
        }
        if (!list.items.Contains("Custom (No PT)"))
        {
            list.items.Add("Custom (No PT)");
        }
    }

    private void Update()
    {
        if (lang != Language.type)
        {
            lang = Language.type;
            label_START.GetComponent<UILabel>().text = Language.btn_start[Language.type];
            label_BACK.GetComponent<UILabel>().text = Language.btn_back[Language.type];
            label_camera.GetComponent<UILabel>().text = Language.camera_type[Language.type];
            label_original.GetComponent<UILabel>().text = Language.camera_original[Language.type];
            label_wow.GetComponent<UILabel>().text = Language.camera_wow[Language.type];
            label_tps.GetComponent<UILabel>().text = Language.camera_tps[Language.type];
            label_choose_character.GetComponent<UILabel>().text = Language.choose_character[Language.type];
            label_difficulty.GetComponent<UILabel>().text = Language.difficulty[Language.type];
            label_choose_map.GetComponent<UILabel>().text = Language.choose_map[Language.type];
            label_normal.GetComponent<UILabel>().text = Language.normal[Language.type];
            label_hard.GetComponent<UILabel>().text = Language.hard[Language.type];
            label_ab.GetComponent<UILabel>().text = Language.abnormal[Language.type];
        }
    }
}
