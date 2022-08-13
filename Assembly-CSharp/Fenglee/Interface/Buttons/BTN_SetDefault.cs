using UnityEngine;

public class BTN_SetDefault : MonoBehaviour
{
	private void OnClick()
	{
		GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().SetToDefault();
		GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().ShowKeyMap();
	}
}
