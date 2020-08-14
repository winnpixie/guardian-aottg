using UnityEngine;

public class BillboardScript : MonoBehaviour
{
	public void Update()
	{
		base.transform.LookAt(Camera.main.transform.position);
		base.transform.Rotate(Vector3.left * -90f);
	}
}
