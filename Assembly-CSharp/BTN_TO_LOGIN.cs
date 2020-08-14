using UnityEngine;

public class BTN_TO_LOGIN : MonoBehaviour
{
	public GameObject loginPanel;

	private void OnClick()
	{
		NGUITools.SetActive(base.transform.parent.gameObject, state: false);
		NGUITools.SetActive(loginPanel, state: true);
	}
}
