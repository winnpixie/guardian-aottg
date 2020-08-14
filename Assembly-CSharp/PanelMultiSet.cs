using UnityEngine;

public class PanelMultiSet : MonoBehaviour
{
    public GameObject label_START;
    public GameObject label_BACK;
    public GameObject label_choose_map;
    public GameObject label_server_name;
    public GameObject label_max_player;
    public GameObject label_max_time;
    public GameObject label_game_time;
    public GameObject label_difficulty;
    public GameObject label_normal;
    public GameObject label_hard;
    public GameObject label_ab;
    private int lang = -1;

    private void OnEnable()
    {
        GameObject.Find("InputServerName").GetComponent<UIInput>().label.text = "FoodForAngels";
        GameObject.Find("InputMaxTime").GetComponent<UIInput>().text = "999999";
    }

    private void Update()
    {
        if (lang != Language.type)
        {
            lang = Language.type;
            label_START.GetComponent<UILabel>().text = Language.btn_start[Language.type];
            label_BACK.GetComponent<UILabel>().text = Language.btn_back[Language.type];
            label_choose_map.GetComponent<UILabel>().text = Language.choose_map[Language.type];
            label_server_name.GetComponent<UILabel>().text = Language.server_name[Language.type];
            label_max_player.GetComponent<UILabel>().text = Language.max_player[Language.type];
            label_max_time.GetComponent<UILabel>().text = Language.max_Time[Language.type];
            label_game_time.GetComponent<UILabel>().text = Language.game_time[Language.type];
            label_difficulty.GetComponent<UILabel>().text = Language.difficulty[Language.type];
            label_normal.GetComponent<UILabel>().text = Language.normal[Language.type];
            label_hard.GetComponent<UILabel>().text = Language.hard[Language.type];
            label_ab.GetComponent<UILabel>().text = Language.abnormal[Language.type];
        }
    }
}
