using UnityEngine;

public class TestStyledComboBox : MonoBehaviour
{
	public StyledComboBox comboBox;

	private void Start()
	{
		comboBox.AddItems("English", "简体中文", "繁體中文", "繁體中文", "繁體中文", "繁體中文", "繁體中文", "繁體中文", "繁體中文", "繁體中文", "繁體中文");
	}
}
