using UnityEngine;

public class FengMath
{
	public static float GetHorizontalAngle(Vector3 from, Vector3 to)
	{
		Vector3 vector = to - from;
		return (0f - Mathf.Atan2(vector.z, vector.x)) * (180f / Mathf.PI);
	}
}
