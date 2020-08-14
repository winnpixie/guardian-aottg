using UnityEngine;

public class BTN_ToLAN : MonoBehaviour
{
	private void OnClick()
	{
		NGUITools.SetActive(base.transform.parent.gameObject, state: false);
		NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiStart, state: true);
	}
}
