using UnityEngine;

public class SupportLogger : MonoBehaviour
{
	public bool LogTrafficStats = true;

	public void Start()
	{
		GameObject x = GameObject.Find("PunSupportLogger");
		if (x == null)
		{
			x = new GameObject("PunSupportLogger");
			Object.DontDestroyOnLoad(x);
			SupportLogging supportLogging = x.AddComponent<SupportLogging>();
			supportLogging.LogTrafficStats = LogTrafficStats;
		}
	}
}
