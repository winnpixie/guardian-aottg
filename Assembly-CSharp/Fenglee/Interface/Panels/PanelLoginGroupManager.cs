using UnityEngine;

public class PanelLoginGroupManager : MonoBehaviour
{
    public GameObject[] panels;
    public PanelGroupManager pgm;
    public LoginFengKAI logincomponent;
    private string _loginName = string.Empty;
    private string _loginPassword = string.Empty;

    public string loginName
    {
        set
        {
            _loginName = value;
        }
    }

    public string loginPassword
    {
        set
        {
            _loginPassword = value;
        }
    }

    private void Start()
    {
        pgm = new PanelGroupManager()
        {
            panelGroup = panels
        };
    }

    public void toLoginPanel()
    {
        pgm.ActivePanel(0);
    }

    public void toSignUpPanel()
    {
        pgm.ActivePanel(1);
    }

    public void toNewPasswordPanel()
    {
        pgm.ActivePanel(2);
    }

    public void toForgetPasswordPanel()
    {
        pgm.ActivePanel(3);
    }

    public void toChangeGuildNamePanel()
    {
        pgm.ActivePanel(4);
    }

    public void toStatusPanel()
    {
        pgm.ActivePanel(5);
    }

    public void SignIn()
    {
        logincomponent.Login(_loginName, _loginPassword);
    }
}
