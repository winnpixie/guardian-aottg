using UnityEngine;

public class BTN_SSR_Down : MonoBehaviour
{
	public GameObject panel;

	private void OnClick()
	{
		panel.GetComponent<SnapShotReview>().ShowNextIMG();
	}
}
