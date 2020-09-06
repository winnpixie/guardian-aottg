using Photon;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PickupItemSimple : Photon.MonoBehaviour
{
	public float SecondsBeforeRespawn = 2f;

	public bool PickupOnCollide;

	public bool SentPickup;

	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (PickupOnCollide && component != null && component.isMine)
		{
			Pickup();
		}
	}

	public void Pickup()
	{
		if (!SentPickup)
		{
			SentPickup = true;
			base.photonView.RPC("PunPickupSimple", PhotonTargets.AllViaServer);
		}
	}

	[RPC]
	public void PunPickupSimple(PhotonMessageInfo msgInfo)
	{
		if (!SentPickup || !msgInfo.sender.isLocal || base.gameObject.GetActive())
		{
		}
		SentPickup = false;
		if (!base.gameObject.GetActive())
		{
			Debug.Log("Ignored PU RPC, cause item is inactive. " + base.gameObject);
			return;
		}
		double num = PhotonNetwork.time - msgInfo.timestamp;
		float num2 = SecondsBeforeRespawn - (float)num;
		if (num2 > 0f)
		{
			base.gameObject.SetActive(value: false);
			Invoke("RespawnAfter", num2);
		}
	}

	public void RespawnAfter()
	{
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(value: true);
		}
	}
}
