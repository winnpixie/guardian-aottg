// Created By: Elite Future, Discord: Elite Future#1043 for questions, suggestions, or optimizations
// Compression/Decompression By: Sadico

using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Core;

public class MicEF : MonoBehaviour
{
    private int lastPos;
    private AudioClip clip;
    public static float Frequency = 10000f;
    public static int Decrease = 125;
    public static int ThreadId;
    public static Dictionary<int, MicPlayer> Users;
    public static KeyCode PushToTalk = KeyCode.V;
    public static List<int> MuteList;
    private static int[] SendList;
    public static List<int> AdjustableList;
    public static float VolumeMultiplier = 1f;
    public static bool Disconnected;
    public static bool AutoConnect = true;
    public static string DeviceName = string.Empty;
    public static bool AutoMute = false;
    public static bool ToggleMic = false;
    private bool micToggled = false;

    public void Start()
    {
        if (PlayerPrefs.HasKey("pushToTalk"))
        {
            PushToTalk = (KeyCode)PlayerPrefs.GetInt("pushToTalk");
        }
        if (PlayerPrefs.HasKey("voiceAutoConnect"))
        {
            string str = PlayerPrefs.GetString("voiceAutoConnect");
            if (str.ToLower().StartsWith("f"))
            {
                AutoConnect = false;
            }
        }
        if (PlayerPrefs.HasKey("voiceAutoMute"))
        {
            string str = PlayerPrefs.GetString("voiceAutoMute");
            if (str.ToLower().StartsWith("t"))
            {
                AutoMute = true;
            }
        }
        if (PlayerPrefs.HasKey("voiceToggleMic"))
        {
            string str = PlayerPrefs.GetString("voiceToggleMic");
            if (str.ToLower().StartsWith("t"))
            {
                ToggleMic = true;
            }
        }
        if (PlayerPrefs.HasKey("volumeMultiplier"))
        {
            VolumeMultiplier = PlayerPrefs.GetFloat("volumeMultiplier");
        }
        if (PlayerPrefs.HasKey("micDevice"))
        {
            DeviceName = PlayerPrefs.GetString("micDevice");
        }

        Disconnected = !AutoConnect;
        SendList = new int[0];
        AdjustableList = new List<int>();
        MuteList = new List<int>();
        Users = new Dictionary<int, MicPlayer>(); // int for ID
        Decrease = (int)(Frequency / 80f); // This is to slightly slow down the audio, not noticeable sound-wise, but removes some gaps
        ThreadId = -1;
        base.gameObject.AddComponent<MicGUI>();
    }

    // Resets when joining a room
    public void OnJoinedRoom()
    {
        ThreadId = -1;
        Disconnected = !AutoConnect;
        SendList = new int[0];
        AdjustableList = new List<int>();
        MuteList = new List<int>();
        Users = new Dictionary<int, MicPlayer>();
        if (base.gameObject.GetComponent<MicGUI>() == null)
        {
            base.gameObject.AddComponent<MicGUI>();
        }
    }

    // Resets when leaving room
    public void OnLeftRoom()
    {
        ThreadId = -1;
        Disconnected = true;
        SendList = new int[0];
        AdjustableList = new List<int>();
        MuteList = new List<int>();
        Users = new Dictionary<int, MicPlayer>();
        if (base.gameObject.GetComponent<MicGUI>() == null)
        {
            base.gameObject.AddComponent<MicGUI>();
        }
    }

    // Resets the player stuff on restarts
    private void OnLevelWasLoaded(int level)
    {
        if (Camera.main.gameObject.GetComponent<AudioSource>() == null)
        {
            Camera.main.gameObject.AddComponent<AudioSource>();
        }
        foreach (KeyValuePair<int, MicPlayer> entry in MicEF.Users)
        {
            entry.Value.RefreshInformation();
        }
    }

    // Adds a person to the voice sending list
    public static void AddSpeaker(int id)
    {
        if (!Users.ContainsKey(id))
        {
            Users.Add(id, new MicPlayer(id));
            if (!AdjustableList.Contains(id))
            {
                AdjustableList.Add(id);
                RecompileSendList();
            }

            PhotonNetwork.RaiseEvent((byte)173, new byte[0], true, new RaiseEventOptions
            {
                TargetActors = new int[] { id }
            });

            if (AutoMute)
            {
                Users[id].Mute(true);
            }
        }
    }

    // Recompiles list to an array so I don't need to do it whever it sends voice
    public static void RecompileSendList()
    {
        SendList = AdjustableList.ToArray();
    }

    public void Update()
    {
        if (!Disconnected)
        {
            if (ThreadId != -1 && ((!ToggleMic && (Input.GetKeyUp(PushToTalk) || !Input.GetKey(PushToTalk))) || (ToggleMic && micToggled && Input.GetKeyDown(PushToTalk))))
            {
                ThreadId = -1;
                micToggled = false;
            }
            else if (Input.GetKeyDown(PushToTalk) && ThreadId == -1)
            {
                if (ToggleMic)
                {
                    micToggled = true;
                }

                // Too lazy to actually put this onto onjoin, so ez pz send that you have mic to everyone every time you use your mic
                PhotonNetwork.RaiseEvent((byte)173, new byte[0], true, new RaiseEventOptions
                {
                    Receivers = ExitGames.Client.Photon.Lite.ReceiverGroup.Others
                });

                clip = Microphone.Start(DeviceName, true, 100, (int)Frequency);

                ThreadId = UnityEngine.Random.Range(0, int.MaxValue);

                GThreadPool.Enqueue(new Thread(() =>
                {
                    try
                    {
                        int tId = ThreadId;
                        Thread.CurrentThread.IsBackground = true;
                        while (tId == ThreadId && !Disconnected) // Just in case 2 instances of the thread is up, it'll stop the old one
                        {
                            // Delay to make larger packets and less gaps(so less choppiness)
                            Thread.Sleep(300); // You can adjust it as you like to see what the best combo is for low latency + low choppiness

                            // Send after delay
                            SendMicData();
                            if (tId != ThreadId)
                            {
                                lastPos = 0;
                                Microphone.End(DeviceName);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        print(e);
                    }
                }));
            }
        }
    }

    // Removes player so that it doesn't send stuff unnecessarily
    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (Users.ContainsKey(player.Id))
        {
            Users.Remove(player.Id);
            if (AdjustableList.Contains(player.Id))
            {
                AdjustableList.Remove(player.Id);
                RecompileSendList();
            }
        }
    }

    // Used to show their name while they talk
    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
        ExitGames.Client.Photon.Hashtable properties = playerAndUpdatedProps[1] as ExitGames.Client.Photon.Hashtable;

        if (properties.ContainsKey("name") && properties["name"] is string && Users.ContainsKey(player.Id))
        {
            Users[player.Id].name = ((string)properties["name"]).Colored();
        }
    }

    // Sends Mic data
    private void SendMicData()
    {
        if (AdjustableList.Count > 0)
        {
            int pos = Microphone.GetPosition(DeviceName);
            if (pos < lastPos) // If the microphone loops, the last sample needs to loop too
            {
                lastPos = 0;
            }

            int diff = pos - lastPos;
            if (diff > 0) // So it doesn't send an empty float[]
            {
                float[] samples = new float[diff];
                clip.GetData(samples, lastPos);
                byte[] bytes = GzipCompress(samples);
                if (bytes.Length < 12000)
                {
                    PhotonNetwork.RaiseEvent((byte)173, bytes, false, new RaiseEventOptions
                    {
                        TargetActors = SendList
                    });
                }
            }
            lastPos = pos;
        }
    }

    // Sadico's Gzip Compression (but Kevin's comments) (but I made it slightly simpler)
    public static byte[] GzipCompress(float[] data)
    {
        if (data == null)
            return null;

        using (MemoryStream outStream = new MemoryStream())
        {
            using (GZipOutputStream gzos = new GZipOutputStream(outStream))
            {
                byte[] floatSerialization = new byte[data.Length * 4]; // Float uses 4 bytes (it's 32 bit)
                Buffer.BlockCopy(data, 0, floatSerialization, 0, floatSerialization.Length); // Serializes the float[] to bytes

                gzos.Write(floatSerialization, 0, floatSerialization.Length);
                gzos.Finish(); // Finishes the stream

                return outStream.ToArray();
            }
        }
    }

    // Sadico's Gzip Decompression (but Kevin's comments) (but I made it slightly simpler)
    public static float[] GzipDecompress(byte[] bytes)
    {
        if (bytes == null)
            return null;

        using (MemoryStream inStream = new MemoryStream(bytes))
        {
            using (GZipInputStream gzis = new GZipInputStream(inStream))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    byte[] buffer = new byte[4096];
                    StreamUtils.Copy(gzis, outStream, buffer);

                    byte[] decompressed = outStream.ToArray(); // Converts the stream to byte array
                    float[] data = new float[decompressed.Length / 4]; // float uses 4 bytes (32 bits)
                    Buffer.BlockCopy(decompressed, 0, data, 0, decompressed.Length); // Converts the decompressed bytes into the float[]

                    return data;
                }
            }
        }
    }
}