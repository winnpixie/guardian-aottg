using UnityEngine;

public class BTN_SET_GUILD_NAME : MonoBehaviour
{
	public GameObject guildname;

	public GameObject logincomponent;

	public GameObject output;

	private void OnClick()
	{
		if (guildname.GetComponent<UIInput>().text.Length < 3)
		{
			output.GetComponent<UILabel>().text = "Guild name too short.";
			return;
		}
		output.GetComponent<UILabel>().text = "please wait...";
		logincomponent.GetComponent<LoginFengKAI>().ChangeGuild(guildname.GetComponent<UIInput>().text);
	}
}
