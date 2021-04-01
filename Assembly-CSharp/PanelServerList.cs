using UnityEngine;

public class PanelServerList : MonoBehaviour
{
    public GameObject label_name;
    public GameObject label_refresh;
    public GameObject label_back;
    public GameObject label_create;
    private int lang = -1;

    private void Update()
    {
        if (lang != Language.type)
        {
            lang = Language.type;
            label_name.GetComponent<UILabel>().text = Language.server_name[Language.type];
            label_refresh.GetComponent<UILabel>().text = Language.btn_refresh[Language.type];
            label_back.GetComponent<UILabel>().text = Language.btn_back[Language.type];
            label_create.GetComponent<UILabel>().text = Language.btn_create_game[Language.type];
        }
    }
}
