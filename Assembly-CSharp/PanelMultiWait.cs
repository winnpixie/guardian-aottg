using UnityEngine;

public class PanelMultiWait : MonoBehaviour
{
    public GameObject label_START;
    public GameObject label_BACK;
    public GameObject label_READY;
    public GameObject label_camera;
    public GameObject label_original;
    public GameObject label_wow;
    public GameObject label_tps;
    public GameObject label_character;
    private int lang = -1;

    private void Update()
    {
        if (lang != Language.type)
        {
            lang = Language.type;
            label_START.GetComponent<UILabel>().text = Language.btn_start[Language.type];
            label_BACK.GetComponent<UILabel>().text = Language.btn_back[Language.type];
            label_READY.GetComponent<UILabel>().text = Language.btn_ready[Language.type];
            label_camera.GetComponent<UILabel>().text = Language.camera_type[Language.type];
            label_original.GetComponent<UILabel>().text = Language.camera_original[Language.type];
            label_wow.GetComponent<UILabel>().text = Language.camera_wow[Language.type];
            label_tps.GetComponent<UILabel>().text = Language.camera_tps[Language.type];
            label_character.GetComponent<UILabel>().text = Language.choose_character[Language.type];
        }
    }
}
