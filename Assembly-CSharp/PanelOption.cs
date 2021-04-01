using UnityEngine;

public class PanelOption : MonoBehaviour
{
    public GameObject label_KEY_LEFT;
    public GameObject label_KEY_RIGHT;
    public GameObject label_mouse_sensitivity;
    public GameObject label_change_quality;
    public GameObject label_camera_tilt;
    public GameObject label_invert_mouse_y;
    public GameObject label_default;
    public GameObject label_back;
    public GameObject label_continue;
    public GameObject label_quit;
    private int lang = -1;

    private void Update()
    {
        if (lang != Language.type)
        {
            lang = Language.type;
            label_KEY_LEFT.GetComponent<UILabel>().text = Language.key_set_info_1[Language.type].Replace("\\n", "\n");
            label_KEY_RIGHT.GetComponent<UILabel>().text = Language.key_set_info_2[Language.type].Replace("\\n", "\n");
            label_mouse_sensitivity.GetComponent<UILabel>().text = Language.mouse_sensitivity[Language.type];
            label_change_quality.GetComponent<UILabel>().text = Language.change_quality[Language.type];
            label_camera_tilt.GetComponent<UILabel>().text = Language.camera_tilt[Language.type];
            label_default.GetComponent<UILabel>().text = Language.btn_default[Language.type];
            label_invert_mouse_y.GetComponent<UILabel>().text = Language.invert_mouse[Language.type];
            if (label_back != null)
            {
                label_back.GetComponent<UILabel>().text = Language.btn_back[Language.type];
            }
            if (label_continue != null)
            {
                label_continue.GetComponent<UILabel>().text = Language.btn_continue[Language.type];
            }
            if (label_quit != null)
            {
                label_quit.GetComponent<UILabel>().text = Language.btn_quit[Language.type];
            }
        }
    }
}
