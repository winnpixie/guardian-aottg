using UnityEngine;

public class BTN_SAVE_CC : MonoBehaviour
{
	public GameObject manager;

	private void OnClick()
	{
		manager.GetComponent<CustomCharacterManager>().SaveData();
	}
}
