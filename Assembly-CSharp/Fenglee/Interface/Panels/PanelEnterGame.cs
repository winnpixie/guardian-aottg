using UnityEngine;

public class PanelEnterGame : MonoBehaviour
{
    public GameObject label_human;
    public GameObject label_titan;
    public GameObject label_select_character;
    public GameObject label_select_titan;
    public GameObject label_camera_type;
    public GameObject label_camera_info;
    private int lang = -1;

    private void Update()
    {
        if (lang != Language.type)
        {
            lang = Language.type;
            label_human.GetComponent<UILabel>().text = Language.soldier[Language.type];
            label_titan.GetComponent<UILabel>().text = Language.titan[Language.type];
            label_select_character.GetComponent<UILabel>().text = Language.choose_character[Language.type];
            label_select_titan.GetComponent<UILabel>().text = Language.select_titan[Language.type];
            label_camera_type.GetComponent<UILabel>().text = Language.camera_type[Language.type];
            label_camera_info.GetComponent<UILabel>().text = Language.camera_info[Language.type];
            if (IN_GAME_MAIN_CAMERA.Gamemode == GameMode.TeamDeathmatch)
            {
                label_select_titan.GetComponent<UILabel>().text = "Play As AHSS";
                label_titan.GetComponent<UILabel>().text = "AHSS";
            }
        }
    }
}
