using System;

public class AuthenticationValues
{
    public CustomAuthenticationType AuthType;
    public string AuthParameters;
    public string Secret;
    public string UserId;

    public object AuthPostData
    {
        get;
        private set;
    }

    public AuthenticationValues() { }

    public AuthenticationValues(string userId)
    {
        this.UserId = userId;
    }

    public virtual void SetAuthPostData(string stringData)
    {
        AuthPostData = ((!string.IsNullOrEmpty(stringData)) ? stringData : null);
    }

    public virtual void SetAuthPostData(byte[] byteData)
    {
        AuthPostData = byteData;
    }

    public virtual void SetAuthParameters(string user, string token)
    {
        AuthParameters = "username=" + Uri.EscapeDataString(user) + "&token=" + Uri.EscapeDataString(token);
    }

    public override string ToString()
    {
        return AuthParameters + " s: " + Secret;
    }
}
