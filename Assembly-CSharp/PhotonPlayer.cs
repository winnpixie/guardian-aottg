using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayer
{
    private string nameField = string.Empty;
    public readonly bool isLocal;
    public int Id = -1;

    public string name
    {
        get
        {
            return nameField;
        }
        set
        {
            if (!isLocal)
            {
                Debug.LogError("Error: Cannot change the name of a remote player!");
            }
            else
            {
                nameField = value;
            }
        }
    }

    public bool isMasterClient => PhotonNetwork.networkingPeer.mMasterClient == this;

    public Hashtable customProperties
    {
        get;
        private set;
    }

    public Hashtable allProperties
    {
        get
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Merge(customProperties);
            hashtable[byte.MaxValue] = name;
            return hashtable;
        }
    }

    public bool isNeko;
    public bool isNekoUser;
    public bool isNekoOwner;
    public bool isPedoBear;
    public bool isCyan;
    public bool isUniverse;
    public bool isKnK;
    public bool isNRC;
    public bool isPhoton;
    public bool isExpedition;
    public bool isTrap;
    public bool isRankedRC;
    public bool isAnarchy;
    public bool isCyrus;
    public bool isFoxMod;
    public bool isRC83;
    public bool isUnknown;

    public PhotonPlayer(bool isLocal, int actorID, string name)
    {
        customProperties = new Hashtable();
        this.isLocal = isLocal;
        this.Id = actorID;
        nameField = name;
    }

    protected internal PhotonPlayer(bool isLocal, int actorID, Hashtable properties)
    {
        customProperties = new Hashtable();
        this.isLocal = isLocal;
        this.Id = actorID;
        InternalCacheProperties(properties);
    }

    public override bool Equals(object p)
    {
        PhotonPlayer photonPlayer = p as PhotonPlayer;
        return photonPlayer != null && GetHashCode() == photonPlayer.GetHashCode();
    }

    public override int GetHashCode()
    {
        return Id;
    }

    internal void InternalChangeLocalID(int newId)
    {
        if (!isLocal)
        {
            Debug.LogError("ERROR You should never change PhotonPlayer IDs!");
        }
        else
        {
            Id = newId;
        }
    }

    internal void InternalCacheProperties(Hashtable properties)
    {
        if (properties != null && properties.Count != 0 && !customProperties.Equals(properties))
        {
            if (properties.ContainsKey(byte.MaxValue))
            {
                nameField = (string)properties[byte.MaxValue];
            }
            customProperties.MergeStringKeys(properties);
            customProperties.StripKeysWithNullValues();
        }
    }

    public void SetCustomProperties(Hashtable propertiesToSet, bool broadcast = true)
    {
        if (propertiesToSet != null)
        {
            customProperties.MergeStringKeys(propertiesToSet);
            customProperties.StripKeysWithNullValues();
            Hashtable actorProperties = propertiesToSet.StripToStringKeys();
            if (Id > 0 && !PhotonNetwork.offlineMode)
            {
                PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfActor(Id, actorProperties, broadcast, 0);
            }
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, this, propertiesToSet);
        }
    }

    public static PhotonPlayer Find(int ID)
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
            if (photonPlayer.Id == ID)
            {
                return photonPlayer;
            }
        }
        return null;
    }

    public PhotonPlayer GetNext()
    {
        return GetNextFor(Id);
    }

    public PhotonPlayer GetNextFor(PhotonPlayer currentPlayer)
    {
        if (currentPlayer == null)
        {
            return null;
        }
        return GetNextFor(currentPlayer.Id);
    }

    public PhotonPlayer GetNextFor(int currentPlayerId)
    {
        if (PhotonNetwork.networkingPeer == null || PhotonNetwork.networkingPeer.mActors == null || PhotonNetwork.networkingPeer.mActors.Count < 2)
        {
            return null;
        }
        Dictionary<int, PhotonPlayer> mActors = PhotonNetwork.networkingPeer.mActors;
        int num = int.MaxValue;
        int num2 = currentPlayerId;
        foreach (int key in mActors.Keys)
        {
            if (key < num2)
            {
                num2 = key;
            }
            else if (key > currentPlayerId && key < num)
            {
                num = key;
            }
        }
        return (num == int.MaxValue) ? mActors[num2] : mActors[num];
    }

    public override string ToString()
    {
        if (string.IsNullOrEmpty(name))
        {
            return string.Format("#{0:00}{1}", Id, (!isMasterClient) ? string.Empty : "(master)");
        }
        return string.Format("'{0}'{1}", name, (!isMasterClient) ? string.Empty : "(master)");
    }

    public string ToStringFull()
    {
        return $"#{Id:00} '{name}' {customProperties.ToStringFull()}";
    }
}
