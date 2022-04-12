using UnityEngine;

public class BTN_LOAD_CC : MonoBehaviour
{
	public GameObject manager;

	private void OnClick()
	{
		manager.GetComponent<CustomCharacterManager>().LoadData();
	}
}
