using Photon;
using UnityEngine;

public class RCGeneralEffect : Photon.MonoBehaviour
{
	private void Awake()
	{
		Object.Destroy(base.gameObject, 1.5f);
	}
}
