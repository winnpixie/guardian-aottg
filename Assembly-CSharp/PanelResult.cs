using UnityEngine;

public class PanelResult : MonoBehaviour
{
    public GameObject label_quit;
    private int lang = -1;

    private void Update()
    {
        if (lang != Language.type)
        {
            lang = Language.type;
            label_quit.GetComponent<UILabel>().text = Language.btn_quit[Language.type];
        }
    }
}
