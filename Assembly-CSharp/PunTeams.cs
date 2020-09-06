using System;
using System.Collections.Generic;
using UnityEngine;

public class PunTeams : MonoBehaviour
{
	public enum Team : byte
	{
		none,
		red,
		blue
	}

	public const string TeamPlayerProp = "team";

	public static Dictionary<Team, List<PhotonPlayer>> PlayersPerTeam;

	public void Start()
	{
		PlayersPerTeam = new Dictionary<Team, List<PhotonPlayer>>();
		Array values = Enum.GetValues(typeof(Team));
		foreach (object item in values)
		{
			PlayersPerTeam[(Team)(byte)item] = new List<PhotonPlayer>();
		}
	}

	public void OnJoinedRoom()
	{
		UpdateTeams();
	}

	public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		UpdateTeams();
	}

	public void UpdateTeams()
	{
		Array values = Enum.GetValues(typeof(Team));
		foreach (object item in values)
		{
			PlayersPerTeam[(Team)(byte)item].Clear();
		}
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
			Team team = photonPlayer.GetTeam();
			PlayersPerTeam[team].Add(photonPlayer);
		}
	}
}
