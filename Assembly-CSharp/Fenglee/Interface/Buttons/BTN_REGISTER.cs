using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

public class BTN_REGISTER : MonoBehaviour
{
	public GameObject nameGO;

	public GameObject password;

	public GameObject password2;

	public GameObject email;

	public GameObject logincomponent;

	public GameObject output;

	private bool invalid;

	private void OnClick()
	{
		if (nameGO.GetComponent<UIInput>().text.Length < 3)
		{
			output.GetComponent<UILabel>().text = "User name too short.";
			return;
		}
		if (password.GetComponent<UIInput>().text.Length < 3)
		{
			output.GetComponent<UILabel>().text = "Password too short.";
			return;
		}
		if (password.GetComponent<UIInput>().text != password2.GetComponent<UIInput>().text)
		{
			output.GetComponent<UILabel>().text = "Password does not match the confirm password.";
			return;
		}
		if (!IsValidEmail(email.GetComponent<UIInput>().text))
		{
			output.GetComponent<UILabel>().text = "This e-mail address is not valid.";
			return;
		}
		logincomponent.GetComponent<LoginFengKAI>().Register(nameGO.GetComponent<UIInput>().text, password.GetComponent<UIInput>().text, password2.GetComponent<UIInput>().text, email.GetComponent<UIInput>().text);
		output.GetComponent<UILabel>().text = "please wait...";
	}

	public bool IsValidEmail(string strIn)
	{
		invalid = false;
		if (string.IsNullOrEmpty(strIn))
		{
			return false;
		}
		strIn = Regex.Replace(strIn, "(@)(.+)$", DomainMapper);
		if (invalid)
		{
			return false;
		}
		return Regex.IsMatch(strIn, "^(?(\")(\"[^\"]+?\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9]{2,17}))$", RegexOptions.IgnoreCase);
	}

	private string DomainMapper(Match match)
	{
		IdnMapping idnMapping = new IdnMapping();
		string text = match.Groups[2].Value;
		try
		{
			text = idnMapping.GetAscii(text);
		}
		catch (ArgumentException)
		{
			invalid = true;
		}
		return match.Groups[1].Value + text;
	}
}
