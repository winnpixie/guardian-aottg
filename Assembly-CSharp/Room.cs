using ExitGames.Client.Photon;
using UnityEngine;

public class Room : RoomInfo
{
    public bool expectedJoinability;
    public bool expectedVisibility;
    public int expectedMaxPlayers;

    public new int playerCount
    {
        get
        {
            if (PhotonNetwork.playerList != null)
            {
                return PhotonNetwork.playerList.Length;
            }
            return 0;
        }
    }

    public new string name
    {
        get
        {
            return nameField;
        }
        internal set
        {
            nameField = value;
        }
    }

    public new int maxPlayers
    {
        get
        {
            return maxPlayersField;
        }
        set
        {
            if (!Equals(PhotonNetwork.room))
            {
                Debug.LogWarning("Can't set maxPlayers when not in that room.");
            }
            if (value > 255)
            {
                value = 255;
            }
            if (!PhotonNetwork.offlineMode)
            {
                PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable
                {
                    { GameProperties.MaxPlayers, (byte)value }
                }, true, 0);
            }
            maxPlayersField = (byte)value;
        }
    }

    public new bool open
    {
        get
        {
            return openField;
        }
        set
        {
            if (!Equals(PhotonNetwork.room))
            {
                Debug.LogWarning("Can't set open when not in that room.");
            }
            if (!PhotonNetwork.offlineMode)
            {
                PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable
                {
                    { GameProperties.IsOpen, value }
                }, true, 0);
            }
            openField = value;
        }
    }

    public new bool visible
    {
        get
        {
            return visibleField;
        }
        set
        {
            if (!Equals(PhotonNetwork.room))
            {
                Debug.LogWarning("Can't set visible when not in that room.");
            }
            if (!PhotonNetwork.offlineMode)
            {
                PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable
                {
                    { GameProperties.IsVisible, value }
                }, true, 0);
            }
            visibleField = value;
        }
    }

    public new int playerTtl
    {
        get
        {
            return playerTtlField;
        }
        set
        {
            if (!Equals(PhotonNetwork.room))
            {
                Debug.LogWarning("Can't set playerTtl when not in that room.");
            }
            if (!PhotonNetwork.offlineMode)
            {
                PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable
                {
                    { GameProperties.PlayerTtl, value }
                }, true, 0);
            }
            playerTtlField = value;
        }
    }

    public new int emptyRoomTtl
    {
        get
        {
            return emptyRoomTtlField;
        }
        set
        {
            if (!Equals(PhotonNetwork.room))
            {
                Debug.LogWarning("Can't set playerTtl when not in that room.");
            }
            if (!PhotonNetwork.offlineMode)
            {
                PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable
                {
                    { GameProperties.RoomTtl, value }
                }, true, 0);
            }
            emptyRoomTtlField = value;
        }
    }

    public string[] propertiesListedInLobby
    {
        get;
        private set;
    }

    public bool autoCleanUp => autoCleanUpField;

    internal Room(string roomName, RoomOptions options) : base(roomName, null)
    {
        if (options == null)
        {
            options = new RoomOptions();
        }
        visibleField = options.isVisible;
        openField = options.isOpen;
        maxPlayersField = (byte)options.maxPlayers;
        autoCleanUpField = false;
        CacheProperties(options.customRoomProperties);
        propertiesListedInLobby = options.customRoomPropertiesForLobby;
    }

    public void SetCustomProperties(Hashtable propertiesToSet, bool broadcast = true)
    {
        if (propertiesToSet != null)
        {
            base.customProperties.MergeStringKeys(propertiesToSet);
            base.customProperties.StripKeysWithNullValues();
            Hashtable gameProperties = propertiesToSet.StripToStringKeys();
            if (!PhotonNetwork.offlineMode)
            {
                PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfRoom(gameProperties, broadcast, 0);
            }
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, propertiesToSet);
        }
    }

    public void SetPropertiesListedInLobby(string[] propsListedInLobby)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[(byte)250] = propsListedInLobby;
        PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, false, 0);
        propertiesListedInLobby = propsListedInLobby;
    }

    public override string ToString()
    {
        return string.Format("Room: '{0}' {1},{2} {4}/{3} players.", nameField, (!visibleField) ? "hidden" : "visible", (!openField) ? "closed" : "open", maxPlayersField, playerCount);
    }

    public new string ToStringFull()
    {
        return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", nameField, (!visibleField) ? "hidden" : "visible", (!openField) ? "closed" : "open", maxPlayersField, playerCount, base.customProperties.ToStringFull());
    }
}
