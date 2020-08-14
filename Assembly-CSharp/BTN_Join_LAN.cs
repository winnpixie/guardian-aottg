using UnityEngine;

public class BTN_Join_LAN : MonoBehaviour
{
	private void OnClick()
	{
		string ip = base.transform.parent.Find("InputIP").GetComponent<UIInput>().text;
		string port = base.transform.parent.Find("InputPort").GetComponent<UIInput>().text;
		string passwd = base.transform.parent.Find("InputAuthPass").GetComponent<UIInput>().text;
		PhotonNetwork.Disconnect();

		if (int.TryParse(port, out int result) && PhotonNetwork.ConnectToMaster(ip, result, FengGameManagerMKII.ApplicationId, UIMainReferences.version))
		{
			PlayerPrefs.SetString("lastIP", ip);
			PlayerPrefs.SetString("lastPort", port);
			PlayerPrefs.SetString("lastAuthPass", passwd);
			FengGameManagerMKII.OnPrivateServer = true;
			FengGameManagerMKII.PrivateServerAuthPass = passwd;
		}
	}
}
