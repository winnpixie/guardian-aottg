using UnityEngine;

public class BTN_SIGNOUT : MonoBehaviour
{
	public GameObject loginPanel;

	public GameObject logincomponent;

	private void OnClick()
	{
		NGUITools.SetActive(base.transform.parent.gameObject, state: false);
		NGUITools.SetActive(loginPanel, state: true);
		logincomponent.GetComponent<LoginFengKAI>().Logout();
	}
}
