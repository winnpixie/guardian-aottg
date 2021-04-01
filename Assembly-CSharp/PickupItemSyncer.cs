using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PickupItemSyncer : Photon.MonoBehaviour
{
	private const float TimeDeltaToIgnore = 0.2f;
	public bool IsWaitingForPickupInit;

	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		if (PhotonNetwork.isMasterClient)
		{
			SendPickedUpItems(newPlayer);
		}
	}

	public void OnJoinedRoom()
	{
		Debug.Log("Joined Room. isMasterClient: " + PhotonNetwork.isMasterClient + " id: " + PhotonNetwork.player.Id);
		IsWaitingForPickupInit = !PhotonNetwork.isMasterClient;
		if (PhotonNetwork.playerList.Length >= 2)
		{
			Invoke("AskForPickupItemSpawnTimes", 2f);
		}
	}

	public void AskForPickupItemSpawnTimes()
	{
		if (!IsWaitingForPickupInit)
		{
			return;
		}
		if (PhotonNetwork.playerList.Length < 2)
		{
			Debug.Log("Cant ask anyone else for PickupItem spawn times.");
			IsWaitingForPickupInit = false;
			return;
		}
		PhotonPlayer next = PhotonNetwork.masterClient.GetNext();
		if (next == null || next.Equals(PhotonNetwork.player))
		{
			next = PhotonNetwork.player.GetNext();
		}
		if (next != null && !next.Equals(PhotonNetwork.player))
		{
			base.photonView.RPC("RequestForPickupTimes", next);
			return;
		}
		Debug.Log("No player left to ask");
		IsWaitingForPickupInit = false;
	}

	[RPC]
	public void RequestForPickupTimes(PhotonMessageInfo msgInfo)
	{
		if (msgInfo.sender == null)
		{
			Debug.LogError("Unknown player asked for PickupItems");
		}
		else
		{
			SendPickedUpItems(msgInfo.sender);
		}
	}

	private void SendPickedUpItems(PhotonPlayer targtePlayer)
	{
		if (targtePlayer == null)
		{
			Debug.LogWarning("Cant send PickupItem spawn times to unknown targetPlayer.");
			return;
		}
		double time = PhotonNetwork.time;
		double num = time + 0.20000000298023224;
		PickupItem[] array = new PickupItem[PickupItem.DisabledPickupItems.Count];
		PickupItem.DisabledPickupItems.CopyTo(array);
		List<float> list = new List<float>(array.Length * 2);
		foreach (PickupItem pickupItem in array)
		{
			if (pickupItem.SecondsBeforeRespawn <= 0f)
			{
				list.Add(pickupItem.ViewID);
				list.Add(0f);
				continue;
			}
			double num2 = pickupItem.TimeOfRespawn - PhotonNetwork.time;
			if (pickupItem.TimeOfRespawn > num)
			{
				Debug.Log(pickupItem.ViewID + " respawn: " + pickupItem.TimeOfRespawn + " timeUntilRespawn: " + num2 + " (now: " + PhotonNetwork.time + ")");
				list.Add(pickupItem.ViewID);
				list.Add((float)num2);
			}
		}
		Debug.Log("Sent count: " + list.Count + " now: " + time);
		base.photonView.RPC("PickupItemInit", targtePlayer, PhotonNetwork.time, list.ToArray());
	}

	[RPC]
	public void PickupItemInit(double timeBase, float[] inactivePickupsAndTimes)
	{
		IsWaitingForPickupInit = false;
		for (int i = 0; i < inactivePickupsAndTimes.Length / 2; i++)
		{
			int num = i * 2;
			int viewID = (int)inactivePickupsAndTimes[num];
			float num2 = inactivePickupsAndTimes[num + 1];
			PhotonView photonView = PhotonView.Find(viewID);
			PickupItem component = photonView.GetComponent<PickupItem>();
			if (num2 <= 0f)
			{
				component.PickedUp(0f);
				continue;
			}
			double num3 = (double)num2 + timeBase;
			Debug.Log(photonView.viewID + " respawn: " + num3 + " timeUntilRespawnBasedOnTimeBase:" + num2 + " SecondsBeforeRespawn: " + component.SecondsBeforeRespawn);
			double num4 = num3 - PhotonNetwork.time;
			if (num2 <= 0f)
			{
				num4 = 0.0;
			}
			component.PickedUp((float)num4);
		}
	}
}
