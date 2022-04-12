using UnityEngine;

public class PanelCredits : MonoBehaviour
{
    public GameObject label_title;
    public GameObject label_back;
    private int lang = -1;

    private void Update()
    {
        if (lang != Language.type)
        {
            lang = Language.type;
            label_title.GetComponent<UILabel>().text = Language.btn_credits[Language.type];
            label_back.GetComponent<UILabel>().text = Language.btn_back[Language.type];
        }
    }
}
