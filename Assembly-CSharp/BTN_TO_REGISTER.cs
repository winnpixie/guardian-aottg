using UnityEngine;

public class BTN_TO_REGISTER : MonoBehaviour
{
	public GameObject registerPanel;

	private void OnClick()
	{
		NGUITools.SetActive(base.transform.parent.gameObject, state: false);
		NGUITools.SetActive(registerPanel, state: true);
	}
}
