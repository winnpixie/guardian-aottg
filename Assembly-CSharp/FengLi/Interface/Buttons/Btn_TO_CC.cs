using UnityEngine;

public class Btn_TO_CC : MonoBehaviour
{
	private void OnClick()
	{
		Application.LoadLevel("characterCreation");

		Guardian.Mod.GuiController.OpenScreen(new Guardian.UI.Impl.GuiCustomCharacter());
	}
}
