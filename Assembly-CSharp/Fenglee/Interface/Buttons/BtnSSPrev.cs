using UnityEngine;

public class BtnSSPrev : MonoBehaviour
{
	private void OnClick()
	{
		if ((bool)base.gameObject.transform.parent.gameObject.GetComponent<CharacterCreationComponent>())
		{
			base.gameObject.transform.parent.gameObject.GetComponent<CharacterCreationComponent>().prevOption();
		}
		else
		{
			base.gameObject.transform.parent.gameObject.GetComponent<CharacterStatComponent>().prevOption();
		}
	}
}
