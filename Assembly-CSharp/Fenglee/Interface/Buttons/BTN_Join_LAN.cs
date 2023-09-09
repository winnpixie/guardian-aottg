using UnityEngine;
using System.Text.RegularExpressions;

public class BTN_Join_LAN : MonoBehaviour
{
    private static readonly Regex PhotonCloud = new Regex("app-\\w+\\.exitgames(cloud)?\\.com", RegexOptions.IgnoreCase);

    private void OnClick()
    {
        PhotonNetwork.Disconnect();

        string ipStr = base.transform.parent.Find("InputIP").GetComponent<UIInput>().text;
        string portStr = base.transform.parent.Find("InputPort").GetComponent<UIInput>().text;
        string passwdStr = base.transform.parent.Find("InputAuthPass").GetComponent<UIInput>().text;

        if (PhotonCloud.IsMatch(ipStr))
        {
            Guardian.GuardianClient.Logger.Info("Joining a Photon Cloud server.");
        }
        else
        {
            Guardian.GuardianClient.Logger.Info("Joining a non-Photon Cloud server.");
        }

        FengGameManagerMKII.OnPrivateServer = !PhotonCloud.IsMatch(ipStr);
        Guardian.Networking.NetworkHelper.IsCloud = !FengGameManagerMKII.OnPrivateServer;

        if (int.TryParse(portStr, out int port) && PhotonNetwork.ConnectToMaster(ipStr, port, Guardian.Networking.NetworkHelper.App.Id, UIMainReferences.Version))
        {
            PlayerPrefs.SetString("lastIP", ipStr);
            PlayerPrefs.SetString("lastPort", portStr);
            PlayerPrefs.SetString("lastAuthPass", passwdStr);
            FengGameManagerMKII.PrivateServerAuthPass = passwdStr;
        }
    }
}
