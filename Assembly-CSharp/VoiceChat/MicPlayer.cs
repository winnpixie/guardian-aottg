// Created By: Elite Future, Discord: Elite Future#1043 for questions, suggestions, or optimizations

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MicPlayer
{
    private float micModifier = 1f; // Used to adjust audio wait times(Look in the IEnum)
    public bool Processing = false; // To know if the IEnum is running
    private Queue<AudioClip> ClipQueue = new Queue<AudioClip>(); // Queue of clips itself
    private int Id = -1;
    private float Volume = 1.5f;
    public string Name;
    private bool IsMuted;
    public bool MutedYou;
    public bool changingVolume = false;

    // Maybe an Icon as well if wanted

    // Add a way to remove yourself from the list of receivers, like sending a float[] of { 0.173 } or something

    // Sets up the mic player, not much else
    public MicPlayer(int id)
    {
        if (Camera.main.gameObject.GetComponent<AudioSource>() == null)
        {
            Camera.main.gameObject.AddComponent<AudioSource>();
        }
        this.Id = id;
        PhotonPlayer player = PhotonPlayer.Find(id);
        if (player.customProperties.ContainsKey("name") && player.customProperties["name"] is string)
        {
            Name = ((string)player.customProperties["name"]).ColorParsed();
        }
        MutedYou = false;
    }

    public bool isMuted
    {
        get
        {
            return IsMuted;
        }
    }

    // Potential use for future identification
    public int ID
    {
        get
        {
            return Id;
        }
    }

    // Sets and gets the volume of the specific person [Make a GUI later]
    public float volume
    {
        get
        {
            return Volume;
        }
        set
        {
            Volume = value;
        }
    }

    // Adds an audioclip to the queue
    public void AddClip(AudioClip clip)
    {
        ClipQueue.Enqueue(clip);
        if (!Processing)
        {
            FengGameManagerMKII.Instance.StartCoroutine(PlayClipQueue());
        }
    }

    // Processes and plays the queue of clips for a smooth voice effect
    public IEnumerator PlayClipQueue()
    {
        if (!Processing)
        {
            Processing = true;
        }
        if (ClipQueue.Count > 0)
        {
            AudioClip clip = ClipQueue.Dequeue();
            Camera.main.gameObject.GetComponent<AudioSource>().PlayOneShot(clip, Volume * MicEF.VolumeMultiplier);

            // This makes it so that the queue doesn't get too long, the stiched audios also sounds a bit better at 0.98, but otherwise unnoticeable
            if (micModifier == 1f && ClipQueue.Count >= 4)
            {
                micModifier = 0.98f;
            }
            else if (micModifier == 0.98f && ClipQueue.Count <= 2)
            {
                micModifier = 1f;
            }

            // Waits for the audio to be finished
            yield return new WaitForSeconds(clip.length * micModifier);

            // Repeats the IEnum for the potential next audio clip
            FengGameManagerMKII.Instance.StartCoroutine(PlayClipQueue());
        }
        else
        {
            Processing = false;
        }
    }

    // Refreshes so the audio will work correctly
    public void RefreshInformation()
    {
        Processing = false;
        ClipQueue = new Queue<AudioClip>();
    }

    // Mutes player
    public void Mute(bool enabled)
    {
        if (!MutedYou)
        {
            IsMuted = enabled;
            if (enabled)
            {
                PhotonNetwork.RaiseEvent((byte)173, new byte[] { (byte)254 }, true, new RaiseEventOptions
                {
                    TargetActors = new int[] { Id }
                });
                MicEF.MuteList.Add(Id);
                if (MicEF.AdjustableList.Contains(Id))
                {
                    MicEF.AdjustableList.Remove(Id);
                    MicEF.RecompileSendList();
                }
            }
            else if (MicEF.MuteList.Contains(Id))
            {
                MicEF.MuteList.Remove(Id);
                PhotonNetwork.RaiseEvent((byte)173, new byte[] { (byte)255 }, true, new RaiseEventOptions
                {
                    TargetActors = new int[] { Id }
                });
                if (!MicEF.AdjustableList.Contains(Id))
                {
                    MicEF.AdjustableList.Add(Id);
                    MicEF.RecompileSendList();
                }
            }
        }
    }
}