using UnityEngine;

public class BTN_Enter_PWD : MonoBehaviour
{
    private void OnClick()
    {
        string pass = GameObject.Find("InputEnterPWD").GetComponent<UIInput>().label.text;
        SimpleAES aes = new SimpleAES();
        if (pass.Equals(aes.Decrypt(PanelMultiJoinPWD.Password)))
        {
            PhotonNetwork.JoinRoom(PanelMultiJoinPWD.RoomName);
            return;
        }

        UIMainReferences ui = GameObject.Find("UIRefer").GetComponent<UIMainReferences>();
        NGUITools.SetActive(ui.PanelMultiPWD, state: false);
        NGUITools.SetActive(ui.panelMultiROOM, state: true);
        GameObject.Find("PanelMultiROOM").GetComponent<PanelMultiJoin>().Refresh();
    }
}
