using UnityEngine;

public class Btn_TO_CC : MonoBehaviour
{
	private void OnClick()
	{
		Application.LoadLevel("characterCreation");

		Guardian.Mod.UI.OpenScreen(new Guardian.UI.Impl.UICustomChar());
	}
}
