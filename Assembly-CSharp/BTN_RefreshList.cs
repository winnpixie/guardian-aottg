using UnityEngine;

public class BTN_RefreshList : MonoBehaviour
{
    private UIMainReferences ui;

    public void Start()
    {
        ui = GameObject.Find("UIRefer").GetComponent<UIMainReferences>();
    }

    private void OnClick()
    {
        ui.panelMultiROOM.GetComponent<PanelMultiJoin>().refresh();
    }
}
