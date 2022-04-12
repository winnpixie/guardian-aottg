using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayer
{
    private string nameField = string.Empty;
    public readonly bool isLocal;
    public int Id = -1;
    public bool IsInactive;

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

    public bool Muted = false;
    public int Ping = -1;

    public bool IsNewRC;
    public bool IsNeko;
    public bool IsNekoUser;
    public bool IsNekoOwner;
    public bool IsPedoBear;
    public bool IsCyan;
    public bool IsUniverse;
    public bool IsKNK;
    public bool IsNRC;
    public bool IsExp;
    public bool IsTRAP;
    public bool IsRRC;
    public bool IsPhotonMod;
    public bool IsAnarchy;
    public bool IsAnarchyExp;
    public bool IsCyrus;
    public bool IsFoxMod;
    public bool IsRC83;
    public bool IsUnknown;

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

    // BEGIN Guardian
    public HERO GetHero()
    {
        foreach (HERO hero in FengGameManagerMKII.Instance.Heroes)
        {
            if (hero.photonView.ownerId == Id)
            {
                return hero;
            }
        }

        return null;
    }

    public TITAN GetTitan()
    {
        foreach (TITAN titan in FengGameManagerMKII.Instance.Titans)
        {
            if (titan.photonView.ownerId == Id)
            {
                return titan;
            }
        }

        return null;
    }

    public string Username
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.Name)
                    && customProperties[PhotonPlayerProperty.Name] is string username)
            {
                return username;
            }

            return string.Empty;
        }
    }

    public string Guild
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.Guild)
                && customProperties[PhotonPlayerProperty.Guild] is string guild)
            {
                return guild;
            }

            return string.Empty;
        }
    }

    public bool IsDead
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.IsDead)
                && customProperties[PhotonPlayerProperty.IsDead] is bool state)
            {
                return state;
            }

            return false;
        }
    }

    public int Team
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.Team)
                && customProperties[PhotonPlayerProperty.Team] is int team)
            {
                return team;
            }

            return 0;
        }
    }

    public bool IsAhss
    {
        get { return Team == 2; }
    }

    public bool IsTitan
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.IsTitan)
                && customProperties[PhotonPlayerProperty.IsTitan] is int state)
            {
                return state == 2;
            }

            return false;
        }
    }

    public int Kills
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.Kills)
                            && customProperties[PhotonPlayerProperty.Kills] is int kills)
            {
                return kills;
            }

            return 0;
        }
    }

    public int Deaths
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.Deaths)
                            && customProperties[PhotonPlayerProperty.Deaths] is int deaths)
            {
                return deaths;
            }

            return 0;
        }
    }

    public int MaxDamage
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.MaxDamage)
                            && customProperties[PhotonPlayerProperty.MaxDamage] is int maxDamage)
            {
                return maxDamage;
            }

            return 0;
        }
    }

    public int TotalDamage
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.TotalDamage)
                            && customProperties[PhotonPlayerProperty.TotalDamage] is int totalDamage)
            {
                return totalDamage;
            }

            return 0;
        }
    }

    public int SpeedStat
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.StatSpeed)
                            && customProperties[PhotonPlayerProperty.StatSpeed] is int speed)
            {
                return speed;
            }

            return 0;
        }
    }

    public int BladeStat
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.StatBlade)
                            && customProperties[PhotonPlayerProperty.StatBlade] is int blade)
            {
                return blade;
            }

            return 0;
        }
    }

    public int GasStat
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.StatGas)
                            && customProperties[PhotonPlayerProperty.StatGas] is int gas)
            {
                return gas;
            }

            return 0;
        }
    }

    public int AccelStat
    {
        get
        {
            if (customProperties.ContainsKey(PhotonPlayerProperty.StatAccel)
                            && customProperties[PhotonPlayerProperty.StatAccel] is int accel)
            {
                return accel;
            }

            return 0;
        }
    }
    // END Guardian

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
