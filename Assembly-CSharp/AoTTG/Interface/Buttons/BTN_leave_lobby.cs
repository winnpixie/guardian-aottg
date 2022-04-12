using UnityEngine;

public class BTN_leave_lobby : MonoBehaviour
{
	private void OnClick()
	{
		NGUITools.SetActive(base.transform.parent.gameObject, state: false);
		NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiStart, state: true);
		PhotonNetwork.Disconnect();
	}
}
