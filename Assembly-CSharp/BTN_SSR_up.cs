using UnityEngine;

public class BTN_SSR_up : MonoBehaviour
{
	public GameObject panel;

	private void OnClick()
	{
		panel.GetComponent<SnapShotReview>().ShowPrevIMG();
	}
}
