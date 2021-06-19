using UnityEngine;

public class PanelMultiStart : MonoBehaviour
{
    public GameObject label_BACK;
    public GameObject label_LAN;
    public GameObject label_server_US;
    public GameObject label_server_EU;
    public GameObject label_server_ASIA;
    public GameObject label_server_JAPAN;
    public GameObject label_QUICK_MATCH;
    public GameObject label_server;
    private int lang = -1;

    private void OnEnable()
    {
        Guardian.Mod.UI.OpenScreen(new Guardian.UI.Impl.UIMultiplayer());

        GameObject.Find("ButtonServer4").gameObject.transform.localPosition = new Vector3(-110f, -85f, 0f);
    }

    private void OnDisable()
    {
        if (Guardian.Mod.UI.CurrentScreen is Guardian.UI.Impl.UIMultiplayer)
        {
            Guardian.Mod.UI.OpenScreen(null);
        }
    }

    private void Update()
    {
        if (lang != Language.type)
        {
            lang = Language.type;
            label_BACK.GetComponent<UILabel>().text = Language.btn_back[Language.type];
            label_LAN.GetComponent<UILabel>().text = Language.btn_LAN[Language.type];
            label_server_US.GetComponent<UILabel>().text = Language.btn_server_US[Language.type];
            label_server_EU.GetComponent<UILabel>().text = Language.btn_server_EU[Language.type];
            label_server_ASIA.GetComponent<UILabel>().text = Language.btn_server_ASIA[Language.type];
            label_server_JAPAN.GetComponent<UILabel>().text = "SA"; // Language.btn_server_JAPAN[Language.type];
            label_QUICK_MATCH.GetComponent<UILabel>().text = Language.btn_QUICK_MATCH[Language.type];
            label_server.GetComponent<UILabel>().text = Language.choose_region_server[Language.type];
        }
    }
}
