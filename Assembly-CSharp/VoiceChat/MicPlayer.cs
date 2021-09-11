// Created By: Elite Future, Discord: Elite Future#1043 for questions, suggestions, or optimizations

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MicPlayer
{
    private float micModifier = 1f; // Used to adjust audio wait times(Look in the IEnum)
    public bool clipProcess = false; // To know if the IEnum is running
    private Queue<AudioClip> clipQueue = new Queue<AudioClip>(); // Queue of clips itself
    private int id = -1;
    private float micVolume = 1.5f;
    public string name;
    private bool muted;
    public bool mutedYou;
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
        this.id = id;
        PhotonPlayer player = PhotonPlayer.Find(id);
        if (player.customProperties.ContainsKey("name") && player.customProperties["name"] is string)
        {
            name = ((string)player.customProperties["name"]).ColorParsed();
        }
        mutedYou = false;
    }

    public bool isMuted
    {
        get
        {
            return muted;
        }
    }

    // Potential use for future identification
    public int ID
    {
        get
        {
            return id;
        }
    }

    // Sets and gets the volume of the specific person [Make a GUI later]
    public float volume
    {
        get
        {
            return micVolume;
        }
        set
        {
            micVolume = value;
        }
    }

    // Adds an audioclip to the queue
    public void AddClip(AudioClip clip)
    {
        clipQueue.Enqueue(clip);
        if (!clipProcess)
        {
            FengGameManagerMKII.Instance.StartCoroutine(PlayClipQueue());
        }
    }

    // Processes and plays the queue of clips for a smooth voice effect
    public IEnumerator PlayClipQueue()
    {
        if (!clipProcess)
        {
            clipProcess = true;
        }
        if (clipQueue.Count > 0)
        {
            AudioClip clip = clipQueue.Dequeue();
            Camera.main.gameObject.GetComponent<AudioSource>().PlayOneShot(clip, micVolume * MicEF.VolumeMultiplier);

            // This makes it so that the queue doesn't get too long, the stiched audios also sounds a bit better at 0.98, but otherwise unnoticeable
            if (micModifier == 1f && clipQueue.Count >= 4)
            {
                micModifier = 0.98f;
            }
            else if (micModifier == 0.98f && clipQueue.Count <= 2)
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
            clipProcess = false;
        }
    }

    // Refreshes so the audio will work correctly
    public void RefreshInformation()
    {
        clipProcess = false;
        clipQueue = new Queue<AudioClip>();
    }

    // Mutes player
    public void Mute(bool enabled)
    {
        if (!mutedYou)
        {
            muted = enabled;
            if (enabled)
            {
                PhotonNetwork.RaiseEvent((byte)173, new byte[] { (byte)254 }, true, new RaiseEventOptions
                {
                    TargetActors = new int[] { id }
                });
                MicEF.MuteList.Add(id);
                if (MicEF.AdjustableList.Contains(id))
                {
                    MicEF.AdjustableList.Remove(id);
                    MicEF.RecompileSendList();
                }
            }
            else if (MicEF.MuteList.Contains(id))
            {
                MicEF.MuteList.Remove(id);
                PhotonNetwork.RaiseEvent((byte)173, new byte[] { (byte)255 }, true, new RaiseEventOptions
                {
                    TargetActors = new int[] { id }
                });
                if (!MicEF.AdjustableList.Contains(id))
                {
                    MicEF.AdjustableList.Add(id);
                    MicEF.RecompileSendList();
                }
            }
        }
    }
}