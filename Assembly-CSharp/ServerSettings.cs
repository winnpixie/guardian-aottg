using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ServerSettings : ScriptableObject
{
    public enum HostingOption
    {
        NotSet,
        PhotonCloud,
        SelfHosted,
        OfflineMode,
        BestRegion
    }

    public HostingOption HostType;
    public ConnectionProtocol Protocol;
    public string ServerAddress = string.Empty;
    public int ServerPort = 5055;
    public CloudRegionCode PreferredRegion;
    public string AppID = string.Empty;
    public bool PingCloudServersOnAwake;
    public List<string> RpcList = new List<string>();
    [HideInInspector]
    public bool DisableAutoOpenWizard;

    public void UseCloudBestResion(string cloudAppid)
    {
        HostType = HostingOption.BestRegion;
        AppID = cloudAppid;
    }

    public void UseCloud(string cloudAppid)
    {
        HostType = HostingOption.PhotonCloud;
        AppID = cloudAppid;
    }

    public void UseCloud(string cloudAppid, CloudRegionCode code)
    {
        HostType = HostingOption.PhotonCloud;
        AppID = cloudAppid;
        PreferredRegion = code;
    }

    public void UseMyServer(string serverAddress, int serverPort, string application)
    {
        HostType = HostingOption.SelfHosted;
        AppID = ((application == null) ? "master" : application);
        ServerAddress = serverAddress;
        ServerPort = serverPort;
    }

    public override string ToString()
    {
        return "ServerSettings: " + HostType + " " + ServerAddress;
    }
}
