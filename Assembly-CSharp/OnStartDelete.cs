using UnityEngine;

public class OnStartDelete : MonoBehaviour
{
	private void Start()
	{
		Object.DestroyObject(base.gameObject);
	}
}
