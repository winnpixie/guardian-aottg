using System.Collections;
using UnityEngine;

public class LoginFengKAI : MonoBehaviour
{
    private static string PlayerName = string.Empty;
    private static string PlayerGuild = string.Empty;
    private static string PlayerPassword = string.Empty;
    public string formText = string.Empty;
    private string CheckUserURL = "http://fenglee.com/game/aog/login_check.php";
    private string RegisterURL = "http://fenglee.com/game/aog/signup_check.php";
    private string ForgetPasswordURL = "http://fenglee.com/game/aog/forget_password.php";
    private string GetInfoURL = "http://fenglee.com/game/aog/require_user_info.php";
    private string ChangePasswordURL = "http://fenglee.com/game/aog/change_password.php";
    private string ChangeGuildURL = "http://fenglee.com/game/aog/change_guild_name.php";
    public GameObject output;
    public GameObject output2;
    public PanelLoginGroupManager loginGroup;
    public GameObject panelLogin;
    public GameObject panelForget;
    public GameObject panelRegister;
    public GameObject panelStatus;
    public GameObject panelChangePassword;
    public GameObject panelChangeGUILDNAME;
    public static PlayerInfoPHOTON Player;

    private void Start()
    {
        if (Player == null)
        {
            Player = new PlayerInfoPHOTON();
            Player.InitAsGuest();
        }
        if (PlayerName != string.Empty)
        {
            NGUITools.SetActive(panelLogin, state: false);
            NGUITools.SetActive(panelStatus, state: true);
            StartCoroutine(GetInfo());
        }
        else
        {
            output.GetComponent<UILabel>().text = "Welcome, " + Player.name;
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
        WWW w = new WWW(CheckUserURL, form);
        yield return w;
        ClearCookies();
        if (w.error != null)
        {
            print(w.error);
            yield break;
        }
        output.GetComponent<UILabel>().text = w.text;
        formText = w.text;
        w.Dispose();
        if (formText.Contains("Welcome back") && formText.Contains("(^o^)/~"))
        {
            NGUITools.SetActive(panelLogin, state: false);
            NGUITools.SetActive(panelStatus, state: true);
            PlayerName = name;
            PlayerPassword = password;
            StartCoroutine(GetInfo());
        }
    }

    private IEnumerator GetInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", PlayerName);
        form.AddField("password", PlayerPassword);
        WWW w = new WWW(GetInfoURL, form);
        yield return w;
        if (w.error != null)
        {
            print(w.error);
            yield break;
        }
        if (w.text.Contains("Error,please sign in again."))
        {
            NGUITools.SetActive(panelLogin, state: true);
            NGUITools.SetActive(panelStatus, state: false);
            output.GetComponent<UILabel>().text = w.text;
            PlayerName = string.Empty;
            PlayerPassword = string.Empty;
        }
        else
        {
            string[] result = w.text.Split('|');
            PlayerGuild = result[0];
            output2.GetComponent<UILabel>().text = result[1];
            Player.name = PlayerName;
            Player.guildname = PlayerGuild;
        }
        w.Dispose();
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
        WWW w = new WWW(RegisterURL, form);
        yield return w;
        if (w.error != null)
        {
            print(w.error);
        }
        else
        {
            output.GetComponent<UILabel>().text = w.text;
            if (w.text.Contains("Final step,to activate your account, please click the link in the activation email"))
            {
                NGUITools.SetActive(panelRegister, state: false);
                NGUITools.SetActive(panelLogin, state: true);
            }
            w.Dispose();
        }
        ClearCookies();
    }

    public void ChangePassword(string oldpassword, string password, string password2)
    {
        if (PlayerName == string.Empty)
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
        form.AddField("userid", PlayerName);
        form.AddField("old_password", oldpassword);
        form.AddField("password", password);
        form.AddField("password2", password2);
        WWW www = new WWW(ChangePasswordURL, form);
        yield return www;
        if (www.error != null)
        {
            print(www.error);
            yield break;
        }
        output.GetComponent<UILabel>().text = www.text;
        if (www.text.Contains("Thanks, your password changed successfully"))
        {
            NGUITools.SetActive(panelChangePassword, state: false);
            NGUITools.SetActive(panelLogin, state: true);
        }
        www.Dispose();
    }

    public void ChangeGuild(string name)
    {
        if (PlayerName == string.Empty)
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
        form.AddField("name", PlayerName);
        form.AddField("guildname", name);
        WWW w = new WWW(ChangeGuildURL, form);
        yield return w;
        if (w.error != null)
        {
            print(w.error);
            yield break;
        }
        output.GetComponent<UILabel>().text = w.text;
        if (w.text.Contains("Guild name set."))
        {
            NGUITools.SetActive(panelChangeGUILDNAME, state: false);
            NGUITools.SetActive(panelStatus, state: true);
            StartCoroutine(GetInfo());
        }
        w.Dispose();
    }

    public void ResetPassword(string email)
    {
        StartCoroutine(ForgetPassword(email));
    }

    private IEnumerator ForgetPassword(string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        WWW w = new WWW(ForgetPasswordURL, form);
        yield return w;
        if (w.error != null)
        {
            print(w.error);
        }
        else
        {
            output.GetComponent<UILabel>().text = w.text;
            w.Dispose();
            NGUITools.SetActive(panelForget, state: false);
            NGUITools.SetActive(panelLogin, state: true);
        }
        ClearCookies();
    }

    private void ClearCookies()
    {
        PlayerName = string.Empty;
        PlayerPassword = string.Empty;
    }

    public void Logout()
    {
        ClearCookies();
        Player = new PlayerInfoPHOTON();
        Player.InitAsGuest();
        output.GetComponent<UILabel>().text = "Welcome, " + Player.name;
    }
}
