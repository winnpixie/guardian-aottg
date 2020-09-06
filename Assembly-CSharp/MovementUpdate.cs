using UnityEngine;

public class MovementUpdate : MonoBehaviour
{
	private Vector3 lastPosition;

	private Vector3 lastVelocity;

	private Quaternion lastRotation;

	public bool disabled;

	private Vector3 targetPosition;

	private void Start()
	{
		if (IN_GAME_MAIN_CAMERA.Gametype == GAMETYPE.SINGLE)
		{
			disabled = true;
			base.enabled = false;
		}
		else if (base.networkView.isMine)
		{
			base.networkView.RPC("updateMovement", RPCMode.OthersBuffered, base.transform.position, base.transform.rotation, base.transform.localScale, Vector3.zero);
		}
		else
		{
			targetPosition = base.transform.position;
		}
	}

	private void Update()
	{
		if (disabled || Network.peerType == NetworkPeerType.Disconnected || Network.peerType == NetworkPeerType.Connecting)
		{
			return;
		}
		if (base.networkView.isMine)
		{
			if (Vector3.Distance(base.transform.position, lastPosition) >= 0.5f)
			{
				lastPosition = base.transform.position;
				base.networkView.RPC("updateMovement", RPCMode.Others, base.transform.position, base.transform.rotation, base.transform.localScale, base.rigidbody.velocity);
			}
			else if (Vector3.Distance(base.transform.rigidbody.velocity, lastVelocity) >= 0.1f)
			{
				lastVelocity = base.transform.rigidbody.velocity;
				base.networkView.RPC("updateMovement", RPCMode.Others, base.transform.position, base.transform.rotation, base.transform.localScale, base.rigidbody.velocity);
			}
			else if (Quaternion.Angle(base.transform.rotation, lastRotation) >= 1f)
			{
				lastRotation = base.transform.rotation;
				base.networkView.RPC("updateMovement", RPCMode.Others, base.transform.position, base.transform.rotation, base.transform.localScale, base.rigidbody.velocity);
			}
		}
		else
		{
			base.transform.position = Vector3.Slerp(base.transform.position, targetPosition, Time.deltaTime * 2f);
		}
	}

	[RPC]
	private void updateMovement(Vector3 newPosition, Quaternion newRotation, Vector3 newScale, Vector3 veloctiy)
	{
		targetPosition = newPosition;
		base.transform.rotation = newRotation;
		base.transform.localScale = newScale;
		base.rigidbody.velocity = veloctiy;
	}
}
