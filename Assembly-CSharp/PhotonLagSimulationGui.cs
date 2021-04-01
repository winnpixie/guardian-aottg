using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonLagSimulationGui : MonoBehaviour
{
	public Rect WindowRect = new Rect(0f, 100f, 120f, 100f);

	public int WindowId = 101;

	public bool Visible = true;

	public PhotonPeer Peer
	{
		get;
		set;
	}

	public void Start()
	{
		Peer = PhotonNetwork.networkingPeer;
	}

	public void OnGUI()
	{
		if (Visible)
		{
			if (Peer == null)
			{
				WindowRect = GUILayout.Window(WindowId, WindowRect, NetSimHasNoPeerWindow, "Netw. Sim.");
			}
			else
			{
				WindowRect = GUILayout.Window(WindowId, WindowRect, NetSimWindow, "Netw. Sim.");
			}
		}
	}

	private void NetSimHasNoPeerWindow(int windowId)
	{
		GUILayout.Label("No peer to communicate with. ");
	}

	private void NetSimWindow(int windowId)
	{
		GUILayout.Label(string.Format("Rtt:{0,4} +/-{1,3}", Peer.RoundTripTime, Peer.RoundTripTimeVariance));
		bool isSimulationEnabled = Peer.IsSimulationEnabled;
		bool flag = GUILayout.Toggle(isSimulationEnabled, "Simulate");
		if (flag != isSimulationEnabled)
		{
			Peer.IsSimulationEnabled = flag;
		}
		float num = Peer.NetworkSimulationSettings.IncomingLag;
		GUILayout.Label("Lag " + num);
		num = GUILayout.HorizontalSlider(num, 0f, 500f);
		Peer.NetworkSimulationSettings.IncomingLag = (int)num;
		Peer.NetworkSimulationSettings.OutgoingLag = (int)num;
		float num2 = Peer.NetworkSimulationSettings.IncomingJitter;
		GUILayout.Label("Jit " + num2);
		num2 = GUILayout.HorizontalSlider(num2, 0f, 100f);
		Peer.NetworkSimulationSettings.IncomingJitter = (int)num2;
		Peer.NetworkSimulationSettings.OutgoingJitter = (int)num2;
		float num3 = Peer.NetworkSimulationSettings.IncomingLossPercentage;
		GUILayout.Label("Loss " + num3);
		num3 = GUILayout.HorizontalSlider(num3, 0f, 10f);
		Peer.NetworkSimulationSettings.IncomingLossPercentage = (int)num3;
		Peer.NetworkSimulationSettings.OutgoingLossPercentage = (int)num3;
		if (GUI.changed)
		{
			WindowRect.height = 100f;
		}
		GUI.DragWindow();
	}
}
