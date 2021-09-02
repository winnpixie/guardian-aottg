using UnityEngine;

public class PanelMain : MonoBehaviour
{
    public GameObject label_single;
    public GameObject label_multi;
    public GameObject label_option;
    public GameObject label_credits;
    private int lang = -1;

    private void Update()
    {
        if (lang != Language.type)
        {
            lang = Language.type;
            label_single.GetComponent<UILabel>().text = Language.btn_single[Language.type];
            label_multi.GetComponent<UILabel>().text = Language.btn_multiplayer[Language.type];
            label_option.GetComponent<UILabel>().text = Language.btn_option[Language.type];
            label_credits.GetComponent<UILabel>().text = Language.btn_credits[Language.type];
        }
    }
}
