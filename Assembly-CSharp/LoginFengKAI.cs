using System.Collections;
using UnityEngine;

public class LoginFengKAI : MonoBehaviour
{
    public static string Name = string.Empty;
    public static string Guild = string.Empty;
    public static string Password = string.Empty;
    public static FengPlayer Player;
    public static LoginState LoginState;

    public string CheckUserURL = "http://fenglee.com/game/aog/login_check.php";
    public string RegisterURL = "http://fenglee.com/game/aog/signup_check.php";
    public string ForgetPasswordURL = "http://fenglee.com/game/aog/forget_password.php";
    public string GetInfoURL = "http://fenglee.com/game/aog/require_user_info.php";
    public string ChangePasswordURL = "http://fenglee.com/game/aog/change_password.php";
    public string ChangeGuildURL = "http://fenglee.com/game/aog/change_guild_name.php";
    public string formText = string.Empty;
    public GameObject output;
    public GameObject output2;
    public PanelLoginGroupManager loginGroup;
    public GameObject panelLogin;
    public GameObject panelForget;
    public GameObject panelRegister;
    public GameObject panelStatus;
    public GameObject panelChangePassword;
    public GameObject panelChangeGUILDNAME;

    private void Start()
    {
        if (Player == null)
        {
            Player = new FengPlayer();
            Player.InitAsGuest();
        }

        if (Name != string.Empty)
        {
            NGUITools.SetActive(panelLogin, state: false);
            NGUITools.SetActive(panelStatus, state: true);
            StartCoroutine(GetInfo());
        }
        else
        {
            output.GetComponent<UILabel>().text = "Welcome, " + Player.Name;
        }
    }

    public void Login(string name, string password)
    {
        StartCoroutine(LoginE(name, password));
    }

    private IEnumerator LoginE(string name, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", name);
        form.AddField("password", password);
        form.AddField("version", UIMainReferences.Version);

        using (WWW www = new WWW(CheckUserURL, form))
        {
            yield return www;
            ClearCookies();
            if (www.error != null)
            {
                print(www.error);
            }
            output.GetComponent<UILabel>().text = www.text;
            formText = www.text;
        }

        if (formText.Contains("Welcome back") && formText.Contains("(^o^)/~"))
        {
            NGUITools.SetActive(panelLogin, state: false);
            NGUITools.SetActive(panelStatus, state: true);
            Name = name;
            Password = password;
            StartCoroutine(GetInfo());
        }
    }

    private IEnumerator GetInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", Name);
        form.AddField("password", Password);

        using (WWW www = new WWW(GetInfoURL, form))
        {
            yield return www;
            if (www.error != null)
            {
                print(www.error);
            }
            if (www.text.Contains("Error,please sign in again."))
            {
                NGUITools.SetActive(panelLogin, state: true);
                NGUITools.SetActive(panelStatus, state: false);
                output.GetComponent<UILabel>().text = www.text;
                Name = string.Empty;
                Password = string.Empty;
            }
            else
            {
                string[] result = www.text.Split('|');
                Guild = result[0];
                output2.GetComponent<UILabel>().text = result[1];
                Player.Name = Name;
                Player.Guild = Guild;
            }
        }
    }

    public void Register(string name, string password, string password2, string email)
    {
        StartCoroutine(RegisterE(name, password, password2, email));
    }

    private IEnumerator RegisterE(string name, string password, string password2, string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", name);
        form.AddField("password", password);
        form.AddField("password2", password2);
        form.AddField("email", email);

        using (WWW www = new WWW(RegisterURL, form))
        {
            yield return www;
            if (www.error != null)
            {
                print(www.error);
            }
            else
            {
                output.GetComponent<UILabel>().text = www.text;
                if (www.text.Contains("Final step,to activate your account, please click the link in the activation email"))
                {
                    NGUITools.SetActive(panelRegister, state: false);
                    NGUITools.SetActive(panelLogin, state: true);
                }
            }
        }

        ClearCookies();
    }

    public void ChangePassword(string oldpassword, string password, string password2)
    {
        if (Name == string.Empty)
        {
            Logout();
            NGUITools.SetActive(panelChangePassword, state: false);
            NGUITools.SetActive(panelLogin, state: true);
            output.GetComponent<UILabel>().text = "Please sign in.";
        }
        else
        {
            StartCoroutine(ChangePasswordE(oldpassword, password, password2));
        }
    }

    private IEnumerator ChangePasswordE(string oldpassword, string password, string password2)
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", Name);
        form.AddField("old_password", oldpassword);
        form.AddField("password", password);
        form.AddField("password2", password2);

        using (WWW www = new WWW(ChangePasswordURL, form))
        {
            yield return www;
            if (www.error != null)
            {
                print(www.error);
            }
            output.GetComponent<UILabel>().text = www.text;
            if (www.text.Contains("Thanks, your password changed successfully"))
            {
                NGUITools.SetActive(panelChangePassword, state: false);
                NGUITools.SetActive(panelLogin, state: true);
            }
        }
    }

    public void ChangeGuild(string name)
    {
        if (Name == string.Empty)
        {
            Logout();
            NGUITools.SetActive(panelChangeGUILDNAME, state: false);
            NGUITools.SetActive(panelLogin, state: true);
            output.GetComponent<UILabel>().text = "Please sign in.";
        }
        else
        {
            StartCoroutine(ChangeGuildE(name));
        }
    }

    private IEnumerator ChangeGuildE(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", Name);
        form.AddField("guildname", name);

        using (WWW www = new WWW(ChangeGuildURL, form))
        {
            yield return www;
            if (www.error != null)
            {
                print(www.error);
            }
            output.GetComponent<UILabel>().text = www.text;
            if (www.text.Contains("Guild name set."))
            {
                NGUITools.SetActive(panelChangeGUILDNAME, state: false);
                NGUITools.SetActive(panelStatus, state: true);
                StartCoroutine(GetInfo());
            }
        }
    }

    public void ResetPassword(string email)
    {
        StartCoroutine(ForgetPassword(email));
    }

    private IEnumerator ForgetPassword(string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);

        using (WWW www = new WWW(ForgetPasswordURL, form))
        {
            yield return www;
            if (www.error != null)
            {
                print(www.error);
            }
            else
            {
                output.GetComponent<UILabel>().text = www.text;
                NGUITools.SetActive(panelForget, state: false);
                NGUITools.SetActive(panelLogin, state: true);
            }
        }
        ClearCookies();
    }

    private void ClearCookies()
    {
        Name = string.Empty;
        Password = string.Empty;
        LoginState = LoginState.LoggedOut;
    }

    public void Logout()
    {
        ClearCookies();
        Player = new FengPlayer();
        Player.InitAsGuest();
        output.GetComponent<UILabel>().text = "Welcome, " + Player.Name;
    }
}
