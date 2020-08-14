using UnityEngine;

public class BTN_LOGIN : MonoBehaviour
{
    public GameObject username;
    public GameObject password;
    public GameObject output;
    public GameObject logincomponent;

    private void OnClick()
    {
        logincomponent.GetComponent<LoginFengKAI>().Login(username.GetComponent<UIInput>().text, password.GetComponent<UIInput>().text);
        output.GetComponent<UILabel>().text = "please wait...";
    }
}
