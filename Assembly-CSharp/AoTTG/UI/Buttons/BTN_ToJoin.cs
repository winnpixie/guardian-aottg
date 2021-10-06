using UnityEngine;

public class BTN_ToJoin : MonoBehaviour
{
    private void OnClick()
    {
        NGUITools.SetActive(base.transform.parent.gameObject, state: false);
        NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiJoinPrivate, state: true);
        Transform transform = GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiJoinPrivate.transform;

        Transform joinBtn = transform.Find("ButtonJOIN");
        if (joinBtn.GetComponent<BTN_Join_LAN>() == null)
        {
            joinBtn.gameObject.AddComponent<BTN_Join_LAN>();
        }

        Transform ipTxt = transform.Find("InputIP");
        string lastIp = PlayerPrefs.GetString("lastIP", "127.0.0.1");
        ipTxt.GetComponent<UIInput>().text = lastIp;
        ipTxt.GetComponent<UIInput>().label.text = lastIp;

        Transform portTxt = transform.Find("InputPort");
        string lastPort = PlayerPrefs.GetString("lastPort", "5055");
        portTxt.GetComponent<UIInput>().text = lastPort;
        portTxt.GetComponent<UIInput>().label.text = lastPort;

        ipTxt.GetComponent<UIInput>().label.shrinkToFit = true;
        portTxt.GetComponent<UIInput>().label.shrinkToFit = true;

        Transform passwdLbl = transform.Find("LabelAuthPass");
        Transform passwdTxt = transform.Find("InputAuthPass");
        if (passwdTxt == null)
        {
            uint width = (uint)ipTxt.transform.Find("Background").localScale.x;
            Vector3 position = joinBtn.localPosition + new Vector3(0f, 61f, 0f);
            passwdTxt = CreateInput(transform.gameObject, ipTxt.gameObject, position, joinBtn.rotation, "InputAuthPass", string.Empty, width).transform;
            passwdTxt.GetComponent<UIInput>().label.shrinkToFit = true;
        }
        if (passwdLbl == null)
        {
            Vector3 position = passwdTxt.localPosition + new Vector3(0f, 35f, 0f);
            GameObject gameObject = transform.Find("LabelIP").gameObject;
            passwdLbl = CreateLabel(transform.gameObject, gameObject, position, passwdTxt.rotation, "LabelAuthPass", "Admin Password (Optional)", gameObject.GetComponent<UILabel>().font.dynamicFontSize, gameObject.GetComponent<UILabel>().lineWidth).transform;
            passwdLbl.localScale = gameObject.transform.localScale;
            passwdLbl.GetComponent<UILabel>().color = gameObject.GetComponent<UILabel>().color;
        }

        string lastPasswd = PlayerPrefs.GetString("lastAuthPass", string.Empty);
        passwdTxt.GetComponent<UIInput>().text = lastPasswd;
        passwdTxt.GetComponent<UIInput>().label.text = lastPasswd;
    }

    public static GameObject CreateInput(GameObject parent, GameObject toClone, Vector3 position, Quaternion rotation, string name, string hint, uint width = 100u, int maxChars = 100, bool isPassword = false)
    {
        GameObject prefab = (GameObject)Object.Instantiate(toClone);
        GameObject gameObject = NGUITools.AddChild(parent, prefab);
        gameObject.name = name;
        gameObject.transform.localPosition = position;
        gameObject.transform.localRotation = rotation;
        gameObject.transform.Find("Label").gameObject.GetComponent<UILabel>().text = hint;
        gameObject.GetComponent<UIInput>().isPassword = isPassword;
        gameObject.GetComponent<UIInput>().maxChars = maxChars;
        Vector3 vector = gameObject.GetComponent<BoxCollider>().size;
        float x = vector.x;
        vector.x = (float)(double)width;
        gameObject.GetComponent<BoxCollider>().size = vector;
        gameObject.GetComponent<UIInput>().label.lineWidth = (int)width;
        vector = gameObject.transform.Find("Background").localScale;
        vector.x *= (float)(double)width / x;
        gameObject.transform.Find("Background").localScale = vector;
        gameObject.transform.Find("Background").position = gameObject.GetComponent<UIInput>().label.transform.position;
        return gameObject;
    }

    public static GameObject CreateLabel(GameObject parent, GameObject toClone, Vector3 position, Quaternion rotation, string name, string text, int fontsize, int lineWidth = 130)
    {
        GameObject prefab = (GameObject)Object.Instantiate(toClone);
        GameObject gameObject = NGUITools.AddChild(parent, prefab);
        gameObject.name = name;
        gameObject.transform.localPosition = position;
        gameObject.transform.localRotation = rotation;
        gameObject.GetComponent<UILabel>().text = text;
        gameObject.GetComponent<UILabel>().font.dynamicFontSize = fontsize;
        gameObject.GetComponent<UILabel>().lineWidth = lineWidth;
        return gameObject;
    }
}
