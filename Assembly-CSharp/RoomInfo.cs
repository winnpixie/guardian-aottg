using ExitGames.Client.Photon;

public class RoomInfo
{
    private Hashtable customPropertiesField = new Hashtable();
    protected byte maxPlayersField;
    protected bool openField = true;
    protected bool visibleField = true;
    protected bool autoCleanUpField = PhotonNetwork.autoCleanUpPlayerObjects;
    protected string nameField;
    private int masterClientIdField;
    private string[] expectedUsersField;
    protected int emptyRoomTtlField;
    protected int playerTtlField;

    public bool removedFromList
    {
        get;
        internal set;
    }

    public Hashtable customProperties => customPropertiesField;
    public string name => nameField;

    public int playerCount
    {
        get;
        private set;
    }

    public bool isLocalClientInside
    {
        get;
        set;
    }

    public byte maxPlayers => maxPlayersField;
    public bool open => openField;
    public bool visible => visibleField;
    public int masterClientId => masterClientIdField;
    public string[] expectedUsers => expectedUsersField;
    public int emptyRoomTtl => emptyRoomTtlField;
    public int playerTtl => playerTtlField;

    protected internal RoomInfo(string roomName, Hashtable properties)
    {
        CacheProperties(properties);
        nameField = roomName;
    }

    public override bool Equals(object p)
    {
        Room room = p as Room;
        return room != null && nameField.Equals(room.nameField);
    }

    public override int GetHashCode()
    {
        return nameField.GetHashCode();
    }

    public override string ToString()
    {
        return string.Format("Room: '{0}' {1},{2} {4}/{3} players.", nameField, (!visibleField) ? "hidden" : "visible", (!openField) ? "closed" : "open", maxPlayersField, playerCount);
    }

    public string ToStringFull()
    {
        return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", nameField, (!visibleField) ? "hidden" : "visible", (!openField) ? "closed" : "open", maxPlayersField, playerCount, customPropertiesField.ToStringFull());
    }

    protected internal void CacheProperties(Hashtable propertiesToCache)
    {
        if (propertiesToCache == null || propertiesToCache.Count == 0 || customPropertiesField.Equals(propertiesToCache))
        {
            return;
        }
        if (propertiesToCache.ContainsKey((byte)251))
        {
            removedFromList = (bool)propertiesToCache[(byte)251];
            if (removedFromList)
            {
                return;
            }
        }
        if (propertiesToCache.ContainsKey(byte.MaxValue))
        {
            maxPlayersField = (byte)propertiesToCache[byte.MaxValue];
        }
        if (propertiesToCache.ContainsKey((byte)253))
        {
            openField = (bool)propertiesToCache[(byte)253];
        }
        if (propertiesToCache.ContainsKey((byte)254))
        {
            visibleField = (bool)propertiesToCache[(byte)254];
        }
        if (propertiesToCache.ContainsKey((byte)252))
        {
            playerCount = (byte)propertiesToCache[(byte)252];
        }
        if (propertiesToCache.ContainsKey((byte)249))
        {
            autoCleanUpField = (bool)propertiesToCache[(byte)249];
        }
        if (propertiesToCache.ContainsKey((byte)248))
        {
            masterClientIdField = (int)propertiesToCache[(byte)248];
        }
        if (propertiesToCache.ContainsKey((byte)247))
        {
            expectedUsersField = (string[])propertiesToCache[(byte)247];
        }
        if (propertiesToCache.ContainsKey((byte)246))
        {
            playerTtlField = (int)propertiesToCache[(byte)246];
        }
        if (propertiesToCache.ContainsKey((byte)245))
        {
            emptyRoomTtlField = (int)propertiesToCache[(byte)245];
        }
        customPropertiesField.MergeStringKeys(propertiesToCache);
    }
}
