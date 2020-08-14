using ExitGames.Client.Photon;
using Photon;
using System;
using System.Collections;
using UnityEngine;

internal class PhotonHandler : Photon.MonoBehaviour, IPhotonPeerListener
{
    public static PhotonHandler SP;
    public int updateInterval;
    public int updateIntervalOnSerialize;
    private int nextSendTickCount;
    private int nextSendTickCountOnSerialize;
    private static bool sendThreadShouldRun;
    public static bool AppQuits;
    public static Type PingImplementation;
    internal static CloudRegionCode BestRegionCodeCurrently = CloudRegionCode.none;

    internal static CloudRegionCode BestRegionCodeInPreferences
    {
        get
        {
            string key = PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty);
            if (!string.IsNullOrEmpty(key))
            {
                return Region.Parse(key);
            }
            return CloudRegionCode.none;
        }
        set
        {
            if (value == CloudRegionCode.none)
            {
                PlayerPrefs.DeleteKey("PUNCloudBestRegion");
            }
            else
            {
                PlayerPrefs.SetString("PUNCloudBestRegion", value.ToString());
            }
        }
    }

    protected void Awake()
    {
        if (SP != null && SP != this && SP.gameObject != null)
        {
            UnityEngine.Object.DestroyImmediate(SP.gameObject);
        }
        SP = this;
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        updateInterval = 1000 / PhotonNetwork.sendRate;
        updateIntervalOnSerialize = 1000 / PhotonNetwork.sendRateOnSerialize;
        StartFallbackSendAckThread();
    }

    protected void OnApplicationQuit()
    {
        AppQuits = true;
        StopFallbackSendAckThread();
        PhotonNetwork.Disconnect();
    }

    protected void Update()
    {
        if (PhotonNetwork.networkingPeer == null)
        {
            Debug.LogError("NetworkPeer broke!");
        }
        else
        {
            if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerState.Disconnected || PhotonNetwork.offlineMode || !PhotonNetwork.isMessageQueueRunning)
            {
                return;
            }
            bool flag = true;
            while (PhotonNetwork.isMessageQueueRunning && flag)
            {
                flag = PhotonNetwork.networkingPeer.DispatchIncomingCommands();
            }
            int num = (int)(Time.realtimeSinceStartup * 1000f);
            if (PhotonNetwork.isMessageQueueRunning && num > nextSendTickCountOnSerialize)
            {
                PhotonNetwork.networkingPeer.RunViewUpdate();
                nextSendTickCountOnSerialize = num + updateIntervalOnSerialize;
                nextSendTickCount = 0;
            }
            num = (int)(Time.realtimeSinceStartup * 1000f);
            if (num > nextSendTickCount)
            {
                bool flag2 = true;
                while (PhotonNetwork.isMessageQueueRunning && flag2)
                {
                    flag2 = PhotonNetwork.networkingPeer.SendOutgoingCommands();
                }
                nextSendTickCount = num + updateInterval;
            }
        }
    }

    protected void OnLevelWasLoaded(int level)
    {
        PhotonNetwork.networkingPeer.NewSceneLoaded();
        PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(Application.loadedLevelName);
    }

    protected void OnJoinedRoom()
    {
        PhotonNetwork.networkingPeer.LoadLevelIfSynced();
    }

    protected void OnCreatedRoom()
    {
        PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(Application.loadedLevelName);
    }

    public static void StartFallbackSendAckThread()
    {
        if (!sendThreadShouldRun)
        {
            sendThreadShouldRun = true;
            SupportClass.CallInBackground(FallbackSendAckThread);
        }
    }

    public static void StopFallbackSendAckThread()
    {
        sendThreadShouldRun = false;
    }

    public static bool FallbackSendAckThread()
    {
        if (sendThreadShouldRun && PhotonNetwork.networkingPeer != null)
        {
            PhotonNetwork.networkingPeer.SendAcksOnly();
        }
        return sendThreadShouldRun;
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        switch (level)
        {
            case DebugLevel.ERROR:
                Debug.LogError(message);
                return;
            case DebugLevel.WARNING:
                Debug.LogWarning(message);
                return;
            case DebugLevel.INFO:
                if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                {
                    Debug.Log(message);
                    return;
                }
                break;
        }
        if (level == DebugLevel.ALL && PhotonNetwork.logLevel == PhotonLogLevel.Full)
        {
            Debug.Log(message);
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
    }

    public void OnEvent(EventData photonEvent)
    {
    }

    protected internal static void PingAvailableRegionsAndConnectToBest()
    {
        SP.StartCoroutine(SP.PingAvailableRegionsCoroutine(connectToBest: true));
    }

    internal IEnumerator PingAvailableRegionsCoroutine(bool connectToBest)
    {
        BestRegionCodeCurrently = CloudRegionCode.none;
        while (PhotonNetwork.networkingPeer.AvailableRegions == null)
        {
            if (PhotonNetwork.connectionStateDetailed != PeerState.ConnectingToNameServer && PhotonNetwork.connectionStateDetailed != PeerState.ConnectedToNameServer)
            {
                Debug.LogError("Call ConnectToNameServer to ping available regions.");
                yield break;
            }
            Debug.Log("Waiting for AvailableRegions. State: " + PhotonNetwork.connectionStateDetailed + " Server: " + PhotonNetwork.Server + " PhotonNetwork.networkingPeer.AvailableRegions " + (PhotonNetwork.networkingPeer.AvailableRegions != null));
            yield return new WaitForSeconds(0.25f);
        }
        if (PhotonNetwork.networkingPeer.AvailableRegions == null || PhotonNetwork.networkingPeer.AvailableRegions.Count == 0)
        {
            Debug.LogError("No regions available. Are you sure your appid is valid and setup?");
            yield break;
        }
        PhotonPingManager pingManager = new PhotonPingManager();
        foreach (Region region in PhotonNetwork.networkingPeer.AvailableRegions)
        {
            SP.StartCoroutine(pingManager.PingSocket(region));
        }
        while (!pingManager.Done)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Region best = pingManager.BestRegion;
        BestRegionCodeCurrently = best.Code;
        BestRegionCodeInPreferences = best.Code;
        Debug.Log("Found best region: " + best.Code + " ping: " + best.Ping + ". Calling ConnectToRegionMaster() is: " + connectToBest);
        if (connectToBest)
        {
            PhotonNetwork.networkingPeer.ConnectToRegionMaster(best.Code);
        }
    }
}
