using SimpleJSON;
using UnityEngine;

internal class Test_CSharp : MonoBehaviour
{
	private string m_InGameLog = string.Empty;

	private Vector2 m_Position = Vector2.zero;

	private void P(string aText)
	{
		m_InGameLog = m_InGameLog + aText + "\n";
	}

	private void Test()
	{
		JSONNode jSONNode = JSONNode.Parse("{\"name\":\"test\", \"array\":[1,{\"data\":\"value\"}]}");
		jSONNode["array"][1]["Foo"] = "Bar";
		P("'nice formatted' string representation of the JSON tree:");
		P(jSONNode.ToString(string.Empty));
		P(string.Empty);
		P("'normal' string representation of the JSON tree:");
		P(jSONNode.ToString());
		P(string.Empty);
		P("content of member 'name':");
		P(jSONNode["name"]);
		P(string.Empty);
		P("content of member 'array':");
		P(jSONNode["array"].ToString(string.Empty));
		P(string.Empty);
		P("first element of member 'array': " + jSONNode["array"][0]);
		P(string.Empty);
		jSONNode["array"][0].AsInt = 10;
		P("value of the first element set to: " + jSONNode["array"][0]);
		P("The value of the first element as integer: " + jSONNode["array"][0].AsInt);
		P(string.Empty);
		P("N[\"array\"][1][\"data\"] == " + jSONNode["array"][1]["data"]);
		P(string.Empty);
		string text = jSONNode.SaveToBase64();
		string aText = jSONNode.SaveToCompressedBase64();
		jSONNode = null;
		P("Serialized to Base64 string:");
		P(text);
		P("Serialized to Base64 string (compressed):");
		P(aText);
		P(string.Empty);
		jSONNode = JSONNode.LoadFromBase64(text);
		P("Deserialized from Base64 string:");
		P(jSONNode.ToString());
		P(string.Empty);
		JSONClass jSONClass = new JSONClass();
		jSONClass["version"].AsInt = 5;
		jSONClass["author"]["name"] = "Bunny83";
		jSONClass["author"]["phone"] = "0123456789";
		jSONClass["data"][-1] = "First item\twith tab";
		jSONClass["data"][-1] = "Second item";
		jSONClass["data"][-1]["value"] = "class item";
		jSONClass["data"].Add("Forth item");
		jSONClass["data"][1] = string.Concat(jSONClass["data"][1], " 'addition to the second item'");
		jSONClass.Add("version", "1.0");
		P("Second example:");
		P(jSONClass.ToString());
		P(string.Empty);
		P("I[\"data\"][0]            : " + jSONClass["data"][0]);
		P("I[\"data\"][0].ToString() : " + jSONClass["data"][0].ToString());
		P("I[\"data\"][0].Value      : " + jSONClass["data"][0].Value);
		P(jSONClass.ToString());
	}

	private void Start()
	{
		Test();
		Debug.Log("Test results:\n" + m_InGameLog);
	}

	private void OnGUI()
	{
		m_Position = GUILayout.BeginScrollView(m_Position);
		GUILayout.Label(m_InGameLog);
		GUILayout.EndScrollView();
	}
}
