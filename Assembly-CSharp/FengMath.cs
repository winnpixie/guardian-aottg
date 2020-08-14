using UnityEngine;

public class FengMath
{
	public static Quaternion getHorizontalRotation(Vector3 from, Vector3 to)
	{
		Vector3 vector = from - to;
		float y = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
		return Quaternion.Euler(0f, y, 0f);
	}

	public static float getHorizontalAngle(Vector3 from, Vector3 to)
	{
		Vector3 vector = to - from;
		return (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
	}
}
