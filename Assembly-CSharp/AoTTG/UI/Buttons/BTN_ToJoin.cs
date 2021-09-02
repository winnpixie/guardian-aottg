using UnityEngine;

public class BTN_ToJoin : MonoBehaviour
{
    private void OnClick()
    {
        NGUITools.SetActive(base.transform.parent.gameObject, state: false);
        NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiJoinPrivate, state: true);
        Transform transform = GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiJoinPrivate.transform;
        Transform transform2 = transform.Find("ButtonJOIN");
        if (transform2.GetComponent<BTN_Join_LAN>() == null)
        {
            transform2.gameObject.AddComponent<BTN_Join_LAN>();
        }
        Transform transform3 = transform.Find("InputIP");
        Transform transform4 = transform.Find("InputPort");
        string @string = PlayerPrefs.GetString("lastIP", "127.0.0.1");
        string string2 = PlayerPrefs.GetString("lastPort", "5055");
        transform3.GetComponent<UIInput>().text = @string;
        transform3.GetComponent<UIInput>().label.text = @string;
        transform4.GetComponent<UIInput>().text = string2;
        transform4.GetComponent<UIInput>().label.text = string2;
        transform3.GetComponent<UIInput>().label.shrinkToFit = true;
        transform4.GetComponent<UIInput>().label.shrinkToFit = true;
        Transform x = transform.Find("LabelAuthPass");
        Transform transform5 = transform.Find("InputAuthPass");
        if (transform5 == null)
        {
            uint width = (uint)transform3.transform.Find("Background").localScale.x;
            Vector3 position = transform2.localPosition + new Vector3(0f, 61f, 0f);
            transform5 = CreateInput(transform.gameObject, transform3.gameObject, position, transform2.rotation, "InputAuthPass", string.Empty, width).transform;
            transform5.GetComponent<UIInput>().label.shrinkToFit = true;
        }
        if (x == null)
        {
            Vector3 position = transform5.localPosition + new Vector3(0f, 35f, 0f);
            GameObject gameObject = transform.Find("LabelIP").gameObject;
            x = CreateLabel(transform.gameObject, gameObject, position, transform5.rotation, "LabelAuthPass", "Admin Password (Optional)", gameObject.GetComponent<UILabel>().font.dynamicFontSize, gameObject.GetComponent<UILabel>().lineWidth).transform;
            x.localScale = gameObject.transform.localScale;
            x.GetComponent<UILabel>().color = gameObject.GetComponent<UILabel>().color;
        }
        string string3 = PlayerPrefs.GetString("lastAuthPass", string.Empty);
        transform5.GetComponent<UIInput>().text = string3;
        transform5.GetComponent<UIInput>().label.text = string3;
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
