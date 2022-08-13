using UnityEngine;

public class BTN_ToOption : MonoBehaviour
{
	private void OnClick()
	{
		NGUITools.SetActive(base.transform.parent.gameObject, state: false);
		NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelOption, state: true);
		GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().ShowKeyMap();
		GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = true;
	}
}
