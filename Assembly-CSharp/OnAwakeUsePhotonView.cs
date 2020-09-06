using Photon;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class OnAwakeUsePhotonView : Photon.MonoBehaviour
{
	private void Awake()
	{
		if (base.photonView.isMine)
		{
			base.photonView.RPC("OnAwakeRPC", PhotonTargets.All);
		}
	}

	private void Start()
	{
		if (base.photonView.isMine)
		{
			base.photonView.RPC("OnAwakeRPC", PhotonTargets.All, (byte)1);
		}
	}

	[RPC]
	public void OnAwakeRPC()
	{
		Debug.Log("RPC: 'OnAwakeRPC' PhotonView: " + base.photonView);
	}

	[RPC]
	public void OnAwakeRPC(byte myParameter)
	{
		Debug.Log("RPC: 'OnAwakeRPC' Parameter: " + myParameter + " PhotonView: " + base.photonView);
	}
}
