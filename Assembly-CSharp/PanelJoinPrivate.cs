using UnityEngine;

public class PanelJoinPrivate : MonoBehaviour
{
    public GameObject label_ip;
    public GameObject label_port;
    public GameObject label_join;
    public GameObject label_back;
    private int lang = -1;

    private void Update()
    {
        if (lang != Language.type)
        {
            lang = Language.type;
            label_ip.GetComponent<UILabel>().text = Language.server_ip[Language.type];
            label_port.GetComponent<UILabel>().text = Language.port[Language.type];
            label_join.GetComponent<UILabel>().text = Language.btn_join[Language.type];
            label_back.GetComponent<UILabel>().text = Language.btn_back[Language.type];
        }
    }
}
