using UnityEngine;
using System.Text.RegularExpressions;

public class BTN_Join_LAN : MonoBehaviour
{
    private static readonly Regex PhotonCloud = new Regex("app-[\\w]+\\.exitgames(cloud)?\\.com", RegexOptions.IgnoreCase);

    private void OnClick()
    {
        string ipStr = base.transform.parent.Find("InputIP").GetComponent<UIInput>().text;
        string portStr = base.transform.parent.Find("InputPort").GetComponent<UIInput>().text;
        string passwdStr = base.transform.parent.Find("InputAuthPass").GetComponent<UIInput>().text;

        PhotonNetwork.Disconnect();
        if (int.TryParse(portStr, out int port) && PhotonNetwork.ConnectToMaster(ipStr, port, FengGameManagerMKII.ApplicationId, UIMainReferences.Version))
        {
            PlayerPrefs.SetString("lastIP", ipStr);
            PlayerPrefs.SetString("lastPort", portStr);
            PlayerPrefs.SetString("lastAuthPass", passwdStr);

            if (PhotonCloud.IsMatch(ipStr))
            {
                Guardian.Mod.Logger.Info("Joining a Photon Cloud server.");
            }
            else
            {
                Guardian.Mod.Logger.Info("Joining a non-Photon Cloud server.");
            }

            FengGameManagerMKII.OnPrivateServer = !PhotonCloud.IsMatch(ipStr);
            Guardian.Networking.NetworkHelper.IsCloud = !FengGameManagerMKII.OnPrivateServer;
            FengGameManagerMKII.PrivateServerAuthPass = passwdStr;
        }
    }
}
