using UnityEngine;

public class Btn_TO_CC : MonoBehaviour
{
	private void OnClick()
	{
		Application.LoadLevel("characterCreation");

		Guardian.UI.ModUI.CurrentScreen = new Guardian.UI.Impl.UICustomChar();
	}
}
