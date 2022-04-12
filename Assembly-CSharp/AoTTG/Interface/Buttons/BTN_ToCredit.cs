using UnityEngine;

public class BTN_ToCredit : MonoBehaviour
{
	private void OnClick()
	{
		NGUITools.SetActive(base.transform.parent.gameObject, state: false);
		NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelCredits, state: true);
	}
}
