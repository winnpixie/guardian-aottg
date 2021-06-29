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
    private GameObject latinAmerBtn;
    private int lang = -1;

    private void OnEnable()
    {
        Guardian.Mod.Menus.OpenScreen(new Guardian.UI.Impl.UIMultiplayer());

        if (latinAmerBtn == null)
        {
            GameObject japanBtn = GameObject.Find("ButtonServer4");
            latinAmerBtn = NGUITools.AddChild(base.gameObject, japanBtn);
            latinAmerBtn.name = "ButtonServerSA";
            latinAmerBtn.transform.localPosition = new Vector3(-110f, -85f, 0f);
            latinAmerBtn.transform.Find("Label").GetComponent<UILabel>().text = "SA";

            Object.Destroy(latinAmerBtn.GetComponent<BTN_Server_JPN>());
            latinAmerBtn.AddComponent<BTN_Server_SA>();
        }
    }

    private void OnDisable()
    {
        if (Guardian.Mod.Menus.CurrentScreen is Guardian.UI.Impl.UIMultiplayer)
        {
            Guardian.Mod.Menus.OpenScreen(null);
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
            label_server_JAPAN.GetComponent<UILabel>().text = Language.btn_server_JAPAN[Language.type];
            label_QUICK_MATCH.GetComponent<UILabel>().text = Language.btn_QUICK_MATCH[Language.type];
            label_server.GetComponent<UILabel>().text = Language.choose_region_server[Language.type];
        }
    }
}
