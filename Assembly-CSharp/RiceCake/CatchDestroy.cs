using UnityEngine;

public class CatchDestroy : MonoBehaviour
{
	public GameObject target;

	private void OnDestroy()
	{
		if (target != null)
		{
			Object.Destroy(target);
		}
	}
}
