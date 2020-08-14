using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using UnityEngine;

public class PhotonPingManager
{
    public bool UseNative;
    public static int Attempts = 5;
    public static bool IgnoreInitialAttempt = true;
    public static int MaxMilliseconsPerPing = 800;
    private int PingsRunning;

    public Region BestRegion
    {
        get
        {
            Region result = null;
            int num = int.MaxValue;
            foreach (Region availableRegion in PhotonNetwork.networkingPeer.AvailableRegions)
            {
                UnityEngine.Debug.Log("BestRegion checks region: " + availableRegion);
                if (availableRegion.Ping != 0 && availableRegion.Ping < num)
                {
                    num = availableRegion.Ping;
                    result = availableRegion;
                }
            }
            return result;
        }
    }

    public bool Done => PingsRunning == 0;

    public IEnumerator PingSocket(Region region)
    {
        region.Ping = Attempts * MaxMilliseconsPerPing;
        PingsRunning++;
        PhotonPing ping;
        if (PhotonHandler.PingImplementation == typeof(PingNativeDynamic))
        {
            UnityEngine.Debug.Log("Using constructor for new PingNativeDynamic()");
            ping = new PingNativeDynamic();
        }
        else
        {
            ping = (PhotonPing)Activator.CreateInstance(PhotonHandler.PingImplementation);
        }
        float rttSum = 0f;
        int replyCount = 0;
        string cleanIpOfRegion = region.HostAndPort;
        int indexOfColon = cleanIpOfRegion.LastIndexOf(':');
        if (indexOfColon > 1)
        {
            cleanIpOfRegion = cleanIpOfRegion.Substring(0, indexOfColon);
        }
        cleanIpOfRegion = ResolveHost(cleanIpOfRegion);
        for (int i = 0; i < Attempts; i++)
        {
            bool overtime = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                ping.StartPing(cleanIpOfRegion);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("catched: " + e);
                PingsRunning--;
                break;
            }
            while (!ping.Done())
            {
                if (sw.ElapsedMilliseconds >= MaxMilliseconsPerPing)
                {
                    overtime = true;
                    break;
                }
                yield return 0;
            }
            int rtt = (int)sw.ElapsedMilliseconds;
            if ((!IgnoreInitialAttempt || i != 0) && ping.Successful && !overtime)
            {
                rttSum += (float)rtt;
                replyCount++;
                region.Ping = (int)(rttSum / (float)replyCount);
            }
            yield return new WaitForSeconds(0.1f);
        }
        PingsRunning--;
        yield return null;
    }

    public static string ResolveHost(string hostName)
    {
        try
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
            if (hostAddresses.Length == 1)
            {
                return hostAddresses[0].ToString();
            }
            foreach (IPAddress iPAddress in hostAddresses)
            {
                if (iPAddress != null)
                {
                    string text = iPAddress.ToString();
                    if (text.IndexOf('.') >= 0)
                    {
                        return text;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log("Exception caught! " + ex.Source + " Message: " + ex.Message);
        }
        return string.Empty;
    }
}
