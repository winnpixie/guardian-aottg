using Photon;
using UnityEngine;

public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
	public bool AutoConnect = true;

	private bool ConnectInUpdate = true;

	public virtual void Start()
	{
		PhotonNetwork.autoJoinLobby = false;
	}

	public virtual void Update()
	{
		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings("2." + Application.loadedLevel);
		}
	}

	public virtual void OnConnectedToMaster()
	{
		if (PhotonNetwork.networkingPeer.AvailableRegions != null)
		{
			Debug.LogWarning(string.Concat("List of available regions counts ", PhotonNetwork.networkingPeer.AvailableRegions.Count, ". First: ", PhotonNetwork.networkingPeer.AvailableRegions[0], " \t Current Region: ", PhotonNetwork.networkingPeer.CloudRegion));
		}
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}

	public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		PhotonNetwork.CreateRoom(null, new RoomOptions
		{
			maxPlayers = 4
		}, null);
	}

	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}

	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
	}

	public virtual void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby(). Use a GUI to show existing rooms available in PhotonNetwork.GetRoomList().");
	}
}
