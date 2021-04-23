using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Lite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

internal class NetworkingPeer : LoadbalancingPeer, IPhotonPeerListener
{
    protected internal const string CurrentSceneProperty = "curScn";
    protected internal string mAppVersion;
    protected internal string mAppId;
    private string playername = string.Empty;
    private IPhotonPeerListener externalListener;
    private JoinType mLastJoinType;
    private bool mPlayernameHasToBeUpdated;
    public Dictionary<int, PhotonPlayer> mActors = new Dictionary<int, PhotonPlayer>();
    public PhotonPlayer[] mOtherPlayerListCopy = new PhotonPlayer[0];
    public PhotonPlayer[] mPlayerListCopy = new PhotonPlayer[0];
    public PhotonPlayer mMasterClient;
    public bool hasSwitchedMC;
    public bool requestSecurity = true;
    private Dictionary<Type, List<MethodInfo>> monoRPCMethodsCache = new Dictionary<Type, List<MethodInfo>>();
    private Dictionary<PhotonView, List<MonoBehaviour>> monoRPCBehavioursCache = new Dictionary<PhotonView, List<MonoBehaviour>>();
    public static bool UsePrefabCache = true;
    public static Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();
    public Dictionary<string, RoomInfo> mGameList = new Dictionary<string, RoomInfo>();
    public RoomInfo[] mGameListCopy = new RoomInfo[0];
    public bool insideLobby;
    public Dictionary<int, GameObject> instantiatedObjects = new Dictionary<int, GameObject>();
    private HashSet<int> allowedReceivingGroups = new HashSet<int>();
    private HashSet<int> blockSendingGroups = new HashSet<int>();
    protected internal Dictionary<int, PhotonView> photonViewList = new Dictionary<int, PhotonView>();
    protected internal short currentLevelPrefix;
    private readonly Dictionary<string, int> rpcShortcuts;
    public bool IsInitialConnect;
    public string NameServerAddress
    {
        get
        {
            if (this.UsedProtocol == ConnectionProtocol.RHttp)
            {
                return "http://ns.exitgamescloud.com:80/photon/n";
            }

            return "ns.exitgamescloud.com";
        }
    }

    private static readonly Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort = new Dictionary<ConnectionProtocol, int>
    {
        { ConnectionProtocol.Udp, 5058 },
        { ConnectionProtocol.Tcp, 4533 },
        { ConnectionProtocol.RHttp, 6063 }
    };

    private bool didAuthenticate;
    private string[] friendListRequested;
    private int friendListTimestamp;
    private bool isFetchingFriends;
    private Dictionary<int, object[]> tempInstantiationData = new Dictionary<int, object[]>();
    protected internal bool loadingLevelAndPausedNetwork;
    private string authSecretCache;

    protected internal string mAppVersionPun => string.Format("{0}_{1}", mAppVersion, "1.28");

    public AuthenticationValues CustomAuthenticationValues
    {
        get;
        set;
    }

    public string MasterServerAddress
    {
        get;
        protected internal set;
    }

    public string PlayerName
    {
        get
        {
            return playername;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && !value.Equals(playername))
            {
                if (mLocalActor != null)
                {
                    mLocalActor.name = value;
                }
                playername = value;
                if (mCurrentGame != null)
                {
                    SendPlayerName();
                }
            }
        }
    }

    public PeerState State
    {
        get;
        internal set;
    }

    public Room mCurrentGame
    {
        get
        {
            if (mRoomToGetInto != null && mRoomToGetInto.isLocalClientInside)
            {
                return mRoomToGetInto;
            }
            return null;
        }
    }

    internal Room mRoomToGetInto
    {
        get;
        set;
    }

    internal RoomOptions mRoomOptionsForCreate
    {
        get;
        set;
    }

    internal TypedLobby mRoomToEnterLobby
    {
        get;
        set;
    }

    public PhotonPlayer mLocalActor
    {
        get;
        internal set;
    }

    public string mGameserver
    {
        get;
        internal set;
    }

    public int mQueuePosition
    {
        get;
        internal set;
    }

    public TypedLobby lobby
    {
        get;
        set;
    }

    public int mPlayersOnMasterCount
    {
        get;
        internal set;
    }

    public int mGameCount
    {
        get;
        internal set;
    }

    public int mPlayersInRoomsCount
    {
        get;
        internal set;
    }

    protected internal ServerConnection server
    {
        get;
        private set;
    }

    public bool IsUsingNameServer
    {
        get;
        protected internal set;
    }

    public List<Region> AvailableRegions
    {
        get;
        protected internal set;
    }

    public CloudRegionCode CloudRegion
    {
        get;
        protected internal set;
    }

    public bool IsAuthorizeSecretAvailable => false;

    protected internal int FriendsListAge => (!isFetchingFriends && friendListTimestamp != 0) ? (Environment.TickCount - friendListTimestamp) : 0;

    public NetworkingPeer(IPhotonPeerListener listener, string playername, ConnectionProtocol connectionProtocol) : base(listener, connectionProtocol)
    {
        if (PhotonHandler.PingImplementation == null)
        {
            PhotonHandler.PingImplementation = typeof(PingMono);
        }
        base.Listener = this;
        lobby = TypedLobby.Default;
        base.LimitOfUnreliableCommands = 40;
        externalListener = listener;
        PlayerName = playername;
        mLocalActor = new PhotonPlayer(isLocal: true, -1, this.playername);
        AddNewPlayer(mLocalActor.Id, mLocalActor);
        rpcShortcuts = new Dictionary<string, int>(PhotonNetwork.PhotonServerSettings.RpcList.Count);
        for (int i = 0; i < PhotonNetwork.PhotonServerSettings.RpcList.Count; i++)
        {
            string key = PhotonNetwork.PhotonServerSettings.RpcList[i];
            rpcShortcuts[key] = i;
        }
        State = global::PeerState.PeerCreated;
    }

    public override bool Connect(string serverAddress, string applicationName)
    {
        Debug.LogError("Avoid using this directly. Thanks.");
        return false;
    }

    public bool ReconnectToMaster()
    {
        if (this.CustomAuthenticationValues == null)
        {
            Debug.LogWarning("ReconnectToMaster() with AuthValues == null is not correct!");
            this.CustomAuthenticationValues = new AuthenticationValues();
        }
        this.CustomAuthenticationValues.Secret = this.authSecretCache;

        return this.Connect(this.MasterServerAddress, ServerConnection.MasterServer);
    }

    public bool ReconnectAndRejoin()
    {
        if (this.CustomAuthenticationValues == null)
        {
            Debug.LogWarning("ReconnectAndRejoin() with AuthValues == null is not correct!");
            this.CustomAuthenticationValues = new AuthenticationValues();
        }
        this.CustomAuthenticationValues.Secret = this.authSecretCache;

        if (!string.IsNullOrEmpty(this.mGameserver))
        {
            this.mLastJoinType = JoinType.JoinGame;
            return this.Connect(this.mGameserver, ServerConnection.GameServer);
        }

        return false;
    }

    public bool Connect(string serverAddress, ServerConnection type)
    {
        if (PhotonHandler.AppQuits)
        {
            Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
            return false;
        }
        if (PhotonNetwork.connectionStateDetailed == global::PeerState.Disconnecting)
        {
            Debug.LogError("Connect() failed. Can't connect while disconnecting (still). Current state: " + PhotonNetwork.connectionStateDetailed);
            return false;
        }
        bool flag = base.Connect(serverAddress, string.Empty);
        if (flag)
        {
            switch (type)
            {
                case ServerConnection.NameServer:
                    State = global::PeerState.ConnectingToNameServer;
                    break;
                case ServerConnection.MasterServer:
                    State = global::PeerState.ConnectingToMasterserver;
                    break;
                case ServerConnection.GameServer:
                    State = global::PeerState.ConnectingToGameserver;
                    break;
            }
        }
        return flag;
    }

    public bool ConnectToNameServer()
    {
        if (PhotonHandler.AppQuits)
        {
            Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
            return false;
        }
        IsUsingNameServer = true;
        CloudRegion = CloudRegionCode.none;
        if (State == global::PeerState.ConnectedToNameServer)
        {
            return true;
        }
        string text = NameServerAddress;
        if (!text.Contains(":"))
        {
            int value = 0;
            ProtocolToNameServerPort.TryGetValue(base.UsedProtocol, out value);
            text = $"{text}:{value}";
            Debug.Log("Server to connect to: " + text + " settings protocol: " + PhotonNetwork.PhotonServerSettings.Protocol);
        }
        if (!base.Connect(text, "ns"))
        {
            return false;
        }
        State = global::PeerState.ConnectingToNameServer;
        return true;
    }

    public bool ConnectToRegionMaster(CloudRegionCode region)
    {
        if (PhotonHandler.AppQuits)
        {
            Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
            return false;
        }
        IsUsingNameServer = true;
        CloudRegion = region;
        if (State == global::PeerState.ConnectedToNameServer)
        {
            return OpAuthenticate(mAppId, mAppVersionPun, PlayerName, CustomAuthenticationValues, region.ToString());
        }
        string text = NameServerAddress;
        if (!text.Contains(":"))
        {
            int value = 0;
            ProtocolToNameServerPort.TryGetValue(base.UsedProtocol, out value);
            text = $"{text}:{value}";
        }
        if (!base.Connect(text, "ns"))
        {
            return false;
        }
        State = global::PeerState.ConnectingToNameServer;
        return true;
    }

    public bool GetRegions()
    {
        if (server != ServerConnection.NameServer)
        {
            return false;
        }
        bool flag = OpGetRegions(mAppId);
        if (flag)
        {
            AvailableRegions = null;
        }
        return flag;
    }

    public override void Disconnect()
    {
        if (base.PeerState == PeerStateValue.Disconnected)
        {
            if (!PhotonHandler.AppQuits)
            {
                Debug.LogWarning($"Can't execute Disconnect() while not connected. Nothing changed. State: {State}");
            }
        }
        else
        {
            State = global::PeerState.Disconnecting;
            base.Disconnect();
        }
    }

    private void DisconnectToReconnect()
    {
        switch (server)
        {
            case ServerConnection.NameServer:
                State = global::PeerState.DisconnectingFromNameServer;
                base.Disconnect();
                break;
            case ServerConnection.MasterServer:
                State = global::PeerState.DisconnectingFromMasterserver;
                base.Disconnect();
                break;
            case ServerConnection.GameServer:
                State = global::PeerState.DisconnectingFromGameserver;
                base.Disconnect();
                break;
        }
    }

    private void LeftLobbyCleanup()
    {
        mGameList = new Dictionary<string, RoomInfo>();
        mGameListCopy = new RoomInfo[0];
        if (insideLobby)
        {
            insideLobby = false;
            SendMonoMessage(PhotonNetworkingMessage.OnLeftLobby);
        }
    }

    private void LeftRoomCleanup()
    {
        bool flag = mRoomToGetInto != null;
        bool flag2 = (mRoomToGetInto == null) ? PhotonNetwork.autoCleanUpPlayerObjects : mRoomToGetInto.autoCleanUp;
        hasSwitchedMC = false;
        mRoomToGetInto = null;
        mActors = new Dictionary<int, PhotonPlayer>();
        mPlayerListCopy = new PhotonPlayer[0];
        mOtherPlayerListCopy = new PhotonPlayer[0];
        mMasterClient = null;
        allowedReceivingGroups = new HashSet<int>();
        blockSendingGroups = new HashSet<int>();
        mGameList = new Dictionary<string, RoomInfo>();
        mGameListCopy = new RoomInfo[0];
        isFetchingFriends = false;
        ChangeLocalID(-1);
        if (flag2)
        {
            LocalCleanupAnythingInstantiated(destroyInstantiatedGameObjects: true);
            PhotonNetwork.manuallyAllocatedViewIds = new List<int>();
        }
        if (flag)
        {
            SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom);
        }
    }

    protected internal void LocalCleanupAnythingInstantiated(bool destroyInstantiatedGameObjects)
    {
        if (tempInstantiationData.Count > 0)
        {
            Debug.LogWarning("It seems some instantiation is not completed, as instantiation data is used. You should make sure instantiations are paused when calling this method. Cleaning now, despite this.");
        }
        if (destroyInstantiatedGameObjects)
        {
            HashSet<GameObject> hashSet = new HashSet<GameObject>(instantiatedObjects.Values);
            foreach (GameObject item in hashSet)
            {
                RemoveInstantiatedGO(item, localOnly: true);
            }
        }
        tempInstantiationData.Clear();
        instantiatedObjects = new Dictionary<int, GameObject>();
        PhotonNetwork.lastUsedViewSubId = 0;
        PhotonNetwork.lastUsedViewSubIdStatic = 0;
    }

    private void ReadoutProperties(ExitGames.Client.Photon.Hashtable gameProperties, ExitGames.Client.Photon.Hashtable pActorProperties, int targetActorNr)
    {
        if (mCurrentGame != null && gameProperties != null)
        {
            mCurrentGame.CacheProperties(gameProperties);
            SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, gameProperties);
            if (PhotonNetwork.automaticallySyncScene)
            {
                LoadLevelIfSynced();
            }
        }
        if (pActorProperties == null || pActorProperties.Count <= 0)
        {
            return;
        }
        if (targetActorNr > 0)
        {
            PhotonPlayer playerWithID = GetPlayerWithID(targetActorNr);
            if (playerWithID != null)
            {
                ExitGames.Client.Photon.Hashtable actorPropertiesForActorNr = GetActorPropertiesForActorNr(pActorProperties, targetActorNr);
                playerWithID.InternalCacheProperties(actorPropertiesForActorNr);
                SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, playerWithID, actorPropertiesForActorNr);
            }
        }
        else
        {
            foreach (object key in pActorProperties.Keys)
            {
                int num = (int)key;
                ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)pActorProperties[key];
                string name = (string)hashtable[byte.MaxValue];
                PhotonPlayer photonPlayer = GetPlayerWithID(num);
                if (photonPlayer == null)
                {
                    photonPlayer = new PhotonPlayer(isLocal: false, num, name);
                    AddNewPlayer(num, photonPlayer);
                }
                photonPlayer.InternalCacheProperties(hashtable);
                SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, photonPlayer, hashtable);
            }
        }
    }

    private void AddNewPlayer(int ID, PhotonPlayer player)
    {
        if (!mActors.ContainsKey(ID))
        {
            mActors[ID] = player;
            RebuildPlayerListCopies();
        }
        else
        {
            Debug.LogError("Adding player twice: " + ID);
        }
    }

    private void RemovePlayer(int ID, PhotonPlayer player)
    {
        mActors.Remove(ID);
        if (!player.isLocal)
        {
            RebuildPlayerListCopies();
        }
    }

    private void RebuildPlayerListCopies()
    {
        mPlayerListCopy = new PhotonPlayer[mActors.Count];
        mActors.Values.CopyTo(mPlayerListCopy, 0);
        List<PhotonPlayer> list = new List<PhotonPlayer>();
        PhotonPlayer[] array = mPlayerListCopy;
        foreach (PhotonPlayer photonPlayer in array)
        {
            if (!photonPlayer.isLocal)
            {
                list.Add(photonPlayer);
            }
        }
        mOtherPlayerListCopy = list.ToArray();
    }

    private void ResetPhotonViewsOnSerialize()
    {
        foreach (PhotonView value in photonViewList.Values)
        {
            value.lastOnSerializeDataSent = null;
        }
    }

    private void HandleEventLeave(int actorID, EventData eventData)
    {
        if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
        {
            Debug.Log("HandleEventLeave for player ID: " + actorID);
        }
        if (actorID < 0 || !mActors.ContainsKey(actorID))
        {
            Debug.LogError($"Received event Leave for unknown player ID: {actorID}");
            return;
        }

        PhotonPlayer player = GetPlayerWithID(actorID);
        if (player == null)
        {
            Debug.LogError("HandleEventLeave for player ID: " + actorID + " has no PhotonPlayer!");
        }

        bool _isAlreadyInactive = player.IsInactive;

        if (eventData.Parameters.ContainsKey(ParameterCode.IsInactive))
        {
            // player becomes inactive (but might return / is not gone for good)
            player.IsInactive = (bool)eventData.Parameters[ParameterCode.IsInactive];
            Guardian.Mod.Logger.Info($"{player.Id} inactive: {player.IsInactive}");

            if (player.IsInactive && _isAlreadyInactive)
            {
                Debug.LogWarning("HandleEventLeave for player ID: " + actorID + " isInactive: " + player.IsInactive + ". Stopping handling if inactive.");
                return;
            }
        }

        CheckMasterClient(actorID);

        if (player.IsInactive && !_isAlreadyInactive)
        {
            return;
        }

        if (mCurrentGame != null && mCurrentGame.autoCleanUp)
        {
            DestroyPlayerObjects(actorID, localOnly: true);
        }
        RemovePlayer(actorID, player);
        SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerDisconnected, player);
    }

    private void CheckMasterClient(int leavingPlayerId)
    {
        bool flag = mMasterClient != null && mMasterClient.Id == leavingPlayerId;
        bool flag2 = leavingPlayerId > 0;
        if (!flag2 || flag)
        {
            if (mActors.Count <= 1)
            {
                mMasterClient = mLocalActor;
            }
            else
            {
                int num = int.MaxValue;
                foreach (int key in mActors.Keys)
                {
                    if (key < num && key != leavingPlayerId)
                    {
                        num = key;
                    }
                }
                mMasterClient = mActors[num];
            }
            if (flag2)
            {
                SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, mMasterClient);
            }
        }
    }

    protected internal bool SetMasterClient(int playerId, bool sync)
    {
        if (mMasterClient == null || mMasterClient.Id == -1 || !mActors.ContainsKey(playerId))
        {
            return false;
        }
        if (sync && !OpRaiseEvent(208, new ExitGames.Client.Photon.Hashtable
        {
            {
                (byte)1,
                playerId
            }
        }, sendReliable: true, null))
        {
            return false;
        }
        hasSwitchedMC = true;
        mMasterClient = mActors[playerId];
        SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, mMasterClient);
        return true;
    }

    private ExitGames.Client.Photon.Hashtable GetActorPropertiesForActorNr(ExitGames.Client.Photon.Hashtable actorProperties, int actorNr)
    {
        if (actorProperties.ContainsKey(actorNr))
        {
            return (ExitGames.Client.Photon.Hashtable)actorProperties[actorNr];
        }
        return actorProperties;
    }

    private PhotonPlayer GetPlayerWithID(int number)
    {
        if (mActors != null && mActors.ContainsKey(number))
        {
            return mActors[number];
        }
        return null;
    }

    private void SendPlayerName()
    {
        if (State == global::PeerState.Joining)
        {
            mPlayernameHasToBeUpdated = true;
        }
        else if (mLocalActor != null)
        {
            mLocalActor.name = PlayerName;
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable[byte.MaxValue] = PlayerName;
            if (mLocalActor.Id > 0)
            {
                OpSetPropertiesOfActor(mLocalActor.Id, hashtable, broadcast: true, 0);
                mPlayernameHasToBeUpdated = false;
            }
        }
    }

    private void GameEnteredOnGameServer(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode != 0)
        {
            switch (operationResponse.OperationCode)
            {
                case 227:
                    if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                    {
                        Debug.Log("Create failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
                    }
                    SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed, operationResponse.ReturnCode, operationResponse.DebugMessage);
                    break;
                case 226:
                    if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                    {
                        Debug.Log("Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
                        if (operationResponse.ReturnCode == 32758)
                        {
                            Debug.Log("Most likely the game became empty during the switch to GameServer.");
                        }
                    }
                    SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed, operationResponse.ReturnCode, operationResponse.DebugMessage);
                    break;
                case 225:
                    if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                    {
                        Debug.Log("Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
                        if (operationResponse.ReturnCode == 32758)
                        {
                            Debug.Log("Most likely the game became empty during the switch to GameServer.");
                        }
                    }
                    SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed, operationResponse.ReturnCode, operationResponse.DebugMessage);
                    break;
            }
            DisconnectToReconnect2();
        }
        else
        {
            State = global::PeerState.Joined;
            mRoomToGetInto.isLocalClientInside = true;
            ExitGames.Client.Photon.Hashtable pActorProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
            ExitGames.Client.Photon.Hashtable gameProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
            ReadoutProperties(gameProperties, pActorProperties, 0);
            int newID = (int)operationResponse[254];
            ChangeLocalID(newID);
            CheckMasterClient(-1);
            if (mPlayernameHasToBeUpdated)
            {
                SendPlayerName();
            }
            switch (operationResponse.OperationCode)
            {
                case 225:
                case 226:
                    break;
                case 227:
                    SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
                    break;
            }
        }
    }

    private ExitGames.Client.Photon.Hashtable GetLocalActorProperties()
    {
        if (PhotonNetwork.player != null)
        {
            return PhotonNetwork.player.allProperties;
        }
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable[byte.MaxValue] = PlayerName;
        return hashtable;
    }

    public void ChangeLocalID(int newID)
    {
        if (mLocalActor == null)
        {
            Debug.LogWarning($"Local actor is null or not in mActors! mLocalActor: {mLocalActor} mActors==null: {mActors == null} newID: {newID}");
        }
        if (mActors.ContainsKey(mLocalActor.Id))
        {
            mActors.Remove(mLocalActor.Id);
        }
        mLocalActor.InternalChangeLocalID(newID);
        mActors[mLocalActor.Id] = mLocalActor;
        RebuildPlayerListCopies();
    }

    public bool OpCreateGame(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
    {
        bool flag = server == ServerConnection.GameServer;
        if (!flag)
        {
            mRoomOptionsForCreate = roomOptions;
            mRoomToGetInto = new Room(roomName, roomOptions);
            mRoomToEnterLobby = (typedLobby ?? ((!insideLobby) ? null : lobby));
        }
        mLastJoinType = JoinType.CreateGame;
        return base.OpCreateRoom(roomName, roomOptions, mRoomToEnterLobby, GetLocalActorProperties(), flag);
    }

    public bool OpJoinRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, bool createIfNotExists, bool rejoin = false)
    {
        bool flag = server == ServerConnection.GameServer;
        if (!flag)
        {
            mRoomOptionsForCreate = roomOptions;
            mRoomToGetInto = new Room(roomName, roomOptions);
            mRoomToEnterLobby = null;
            if (createIfNotExists)
            {
                mRoomToEnterLobby = (typedLobby ?? ((!insideLobby) ? null : lobby));
            }
        }
        mLastJoinType = ((!createIfNotExists) ? JoinType.JoinGame : JoinType.JoinOrCreateOnDemand);
        return base.OpJoinRoom(roomName, roomOptions, mRoomToEnterLobby, createIfNotExists, GetLocalActorProperties(), flag, rejoin);
    }

    public override bool OpJoinRandomRoom(ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, ExitGames.Client.Photon.Hashtable playerProperties, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter)
    {
        mRoomToGetInto = new Room(null, null);
        mRoomToEnterLobby = null;
        mLastJoinType = JoinType.JoinRandomGame;
        return base.OpJoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, playerProperties, matchingType, typedLobby, sqlLobbyFilter);
    }

    public virtual bool OpLeave(bool becomeInactive)
    {
        if (State != global::PeerState.Joined)
        {
            Debug.LogWarning("Not sending leave operation. State is not 'Joined': " + State);
            return false;
        }
        Dictionary<byte, object> parameters = null;
        if (becomeInactive)
        {
            parameters = new Dictionary<byte, object>
            {
                { (byte)233, becomeInactive }
            };
        }
        return OpCustom(OperationCode.Leave, parameters, sendReliable: true, 0);
    }

    public override bool OpRaiseEvent(byte eventCode, object customEventContent, bool sendReliable, RaiseEventOptions raiseEventOptions)
    {
        if (PhotonNetwork.offlineMode)
        {
            return false;
        }

        return base.OpRaiseEvent(eventCode, customEventContent, sendReliable, raiseEventOptions);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        externalListener.DebugReturn(level, message);
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        if (PhotonNetwork.networkingPeer.State == global::PeerState.Disconnecting)
        {
            if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
            {
                Debug.Log("OperationResponse ignored while disconnecting. Code: " + operationResponse.OperationCode);
            }
            return;
        }
        if (operationResponse.ReturnCode == 0)
        {
            if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
            {
                Debug.Log(operationResponse.ToString());
            }
        }
        else if (operationResponse.ReturnCode == -3)
        {
            Debug.LogError("Operation " + operationResponse.OperationCode + " could not be executed (yet). Wait for state JoinedLobby or ConnectedToMaster and their callbacks before calling operations. WebRPCs need a server-side configuration. Enum OperationCode helps identify the operation.");
        }
        else if (operationResponse.ReturnCode == 32752)
        {
            Debug.LogError("Operation " + operationResponse.OperationCode + " failed in a server-side plugin. Check the configuration in the Dashboard. Message from server-plugin: " + operationResponse.DebugMessage);
        }
        else if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
        {
            Debug.LogError("Operation failed: " + operationResponse.ToStringFull() + " Server: " + server);
        }
        if (operationResponse.Parameters.ContainsKey(221))
        {
            if (CustomAuthenticationValues == null)
            {
                CustomAuthenticationValues = new AuthenticationValues();
            }
            CustomAuthenticationValues.Secret = (operationResponse[221] as string);
            authSecretCache = CustomAuthenticationValues.Secret;
        }
        switch (operationResponse.OperationCode)
        {
            case OperationCode.Authenticate:
                if (operationResponse.ReturnCode != 0)
                {
                    if (operationResponse.ReturnCode == -2)
                    {
                        Debug.LogError(string.Format("If you host Photon yourself, make sure to start the 'Instance LoadBalancing' " + base.ServerAddress));
                    }
                    else if (operationResponse.ReturnCode == short.MaxValue)
                    {
                        Debug.LogError($"The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account.");
                        SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, DisconnectCause.InvalidAuthentication);
                    }
                    else if (operationResponse.ReturnCode == 32755)
                    {
                        Debug.LogError($"Custom Authentication failed (either due to user-input or configuration or AuthParameter string format). Calling: OnCustomAuthenticationFailed()");
                        SendMonoMessage(PhotonNetworkingMessage.OnCustomAuthenticationFailed, operationResponse.DebugMessage);
                    }
                    else
                    {
                        Debug.LogError($"Authentication failed: '{operationResponse.DebugMessage}' Code: {operationResponse.ReturnCode}");
                    }
                    State = global::PeerState.Disconnecting;
                    Disconnect();
                    if (operationResponse.ReturnCode == 32757)
                    {
                        if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                        {
                            Debug.LogWarning($"Currently, the limit of users is reached for this title. Try again later. Disconnecting");
                        }
                        SendMonoMessage(PhotonNetworkingMessage.OnPhotonMaxCccuReached);
                        SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, DisconnectCause.MaxCcuReached);
                    }
                    else if (operationResponse.ReturnCode == 32756)
                    {
                        if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                        {
                            Debug.LogError($"The used master server address is not available with the subscription currently used. Got to Photon Cloud Dashboard or change URL. Disconnecting.");
                        }
                        SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, DisconnectCause.InvalidRegion);
                    }
                    else if (operationResponse.ReturnCode == 32753)
                    {
                        if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                        {
                            Debug.LogError($"The authentication ticket expired. You need to connect (and authenticate) again. Disconnecting.");
                        }
                        SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, DisconnectCause.AuthenticationTicketExpired);
                    }
                }
                else if (server == ServerConnection.NameServer)
                {
                    MasterServerAddress = (operationResponse[230] as string);
                    DisconnectToReconnect2();
                }
                else if (server == ServerConnection.MasterServer)
                {
                    if (PhotonNetwork.autoJoinLobby)
                    {
                        State = global::PeerState.Authenticated;
                        OpJoinLobby(lobby);
                    }
                    else
                    {
                        State = global::PeerState.ConnectedToMaster;
                        SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
                    }
                }
                else if (server == ServerConnection.GameServer)
                {
                    State = global::PeerState.Joining;
                    if (mLastJoinType == JoinType.JoinGame || mLastJoinType == JoinType.JoinRandomGame || mLastJoinType == JoinType.JoinOrCreateOnDemand)
                    {
                        OpJoinRoom(mRoomToGetInto.name, mRoomOptionsForCreate, mRoomToEnterLobby, mLastJoinType == JoinType.JoinOrCreateOnDemand);
                    }
                    else if (mLastJoinType == JoinType.CreateGame)
                    {
                        OpCreateGame(mRoomToGetInto.name, mRoomOptionsForCreate, mRoomToEnterLobby);
                    }
                }

                if (server == ServerConnection.NameServer || server == ServerConnection.GameServer)
                {
                    if (operationResponse.Parameters.ContainsKey(ParameterCode.UserId))
                    {
                        string incomingId = (string)operationResponse.Parameters[ParameterCode.UserId];
                        if (!string.IsNullOrEmpty(incomingId))
                        {
                            if (this.CustomAuthenticationValues == null)
                            {
                                this.CustomAuthenticationValues = new AuthenticationValues();
                            }
                            this.CustomAuthenticationValues.UserId = incomingId;
                            PhotonNetwork.player.UserId = incomingId;

                            if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                            {
                                this.DebugReturn(DebugLevel.INFO, string.Format("Received your UserID from server. Updating local value to: {0}", incomingId));
                            }
                        }
                    }
                }
                break;
            case OperationCode.GetRegions:
                {
                    if (operationResponse.ReturnCode == short.MaxValue)
                    {
                        Debug.LogError($"The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account.");
                        SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, DisconnectCause.InvalidAuthentication);
                        State = global::PeerState.Disconnecting;
                        Disconnect();
                        return;
                    }
                    string[] array3 = operationResponse[210] as string[];
                    string[] array4 = operationResponse[230] as string[];
                    if (array3 == null || array4 == null || array3.Length != array4.Length)
                    {
                        Debug.LogError("The region arrays from Name Server are not ok. Must be non-null and same length.");
                        break;
                    }
                    AvailableRegions = new List<Region>(array3.Length);
                    for (int j = 0; j < array3.Length; j++)
                    {
                        string text = array3[j];
                        if (!string.IsNullOrEmpty(text))
                        {
                            text = text.ToLower();
                            CloudRegionCode code = Region.Parse(text);
                            AvailableRegions.Add(new Region
                            {
                                Code = code,
                                HostAndPort = array4[j]
                            });
                        }
                    }
                    if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion)
                    {
                        PhotonHandler.PingAvailableRegionsAndConnectToBest();
                    }
                    break;
                }
            case OperationCode.CreateGame:
                {
                    if (server == ServerConnection.GameServer)
                    {
                        GameEnteredOnGameServer(operationResponse);
                        break;
                    }
                    if (operationResponse.ReturnCode != 0)
                    {
                        if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                        {
                            Debug.LogWarning($"CreateRoom failed, client stays on masterserver: {operationResponse.ToStringFull()}.");
                        }
                        SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed);
                        break;
                    }
                    string text2 = (string)operationResponse[byte.MaxValue];
                    if (!string.IsNullOrEmpty(text2))
                    {
                        mRoomToGetInto.name = text2;
                    }
                    mGameserver = (string)operationResponse[230];
                    DisconnectToReconnect2();
                    break;
                }
            case OperationCode.JoinGame:
                if (server != ServerConnection.GameServer)
                {
                    if (operationResponse.ReturnCode != 0)
                    {
                        if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                        {
                            Debug.Log($"JoinRoom failed (room maybe closed by now). Client stays on masterserver: {operationResponse.ToStringFull()}. State: {State}");
                        }
                        SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed);
                    }
                    else
                    {
                        mGameserver = (string)operationResponse[230];
                        DisconnectToReconnect2();
                    }
                }
                else
                {
                    GameEnteredOnGameServer(operationResponse);
                }
                break;
            case OperationCode.JoinRandomGame:
                if (operationResponse.ReturnCode != 0)
                {
                    if (operationResponse.ReturnCode == 32760)
                    {
                        if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
                        {
                            Debug.Log("JoinRandom failed: No open game. Calling: OnPhotonRandomJoinFailed() and staying on master server.");
                        }
                    }
                    else if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                    {
                        Debug.LogWarning($"JoinRandom failed: {operationResponse.ToStringFull()}.");
                    }
                    SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed);
                }
                else
                {
                    string name = (string)operationResponse[byte.MaxValue];
                    mRoomToGetInto.name = name;
                    mGameserver = (string)operationResponse[230];
                    DisconnectToReconnect2();
                }
                break;
            case OperationCode.JoinLobby:
                State = global::PeerState.JoinedLobby;
                insideLobby = true;
                SendMonoMessage(PhotonNetworkingMessage.OnJoinedLobby);
                break;
            case OperationCode.LeaveLobby:
                State = global::PeerState.Authenticated;
                LeftLobbyCleanup();
                break;
            case OperationCode.Leave:
                DisconnectToReconnect2();
                break;
            case OperationCode.GetProperties:
                {
                    ExitGames.Client.Photon.Hashtable pActorProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
                    ExitGames.Client.Photon.Hashtable gameProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
                    ReadoutProperties(gameProperties, pActorProperties, 0);
                    break;
                }
            case OperationCode.FindFriends:
                {
                    bool[] array = operationResponse[1] as bool[];
                    string[] array2 = operationResponse[2] as string[];
                    if (array != null && array2 != null && friendListRequested != null && array.Length == friendListRequested.Length)
                    {
                        List<FriendInfo> list = new List<FriendInfo>(friendListRequested.Length);
                        for (int i = 0; i < friendListRequested.Length; i++)
                        {
                            FriendInfo friendInfo = new FriendInfo();
                            friendInfo.Name = friendListRequested[i];
                            friendInfo.Room = array2[i];
                            friendInfo.IsOnline = array[i];
                            list.Insert(i, friendInfo);
                        }
                        PhotonNetwork.Friends = list;
                    }
                    else
                    {
                        Debug.LogError("FindFriends failed to apply the result, as a required value wasn't provided or the friend list length differed from result.");
                    }
                    friendListRequested = null;
                    isFetchingFriends = false;
                    friendListTimestamp = Environment.TickCount;
                    if (friendListTimestamp == 0)
                    {
                        friendListTimestamp = 1;
                    }
                    SendMonoMessage(PhotonNetworkingMessage.OnUpdatedFriendList);
                    break;
                }
            case OperationCode.WebRpc:
                SendMonoMessage(PhotonNetworkingMessage.OnWebRpcResponse, operationResponse);
                break;
            default:
                Debug.LogWarning($"OperationResponse unhandled: {operationResponse}");
                break;
            case OperationCode.SetProperties:
            case OperationCode.RaiseEvent:
                break;
        }
        externalListener.OnOperationResponse(operationResponse);
    }

    public override bool OpFindFriends(string[] friendsToFind)
    {
        if (isFetchingFriends)
        {
            return false;
        }
        friendListRequested = friendsToFind;
        isFetchingFriends = true;
        return base.OpFindFriends(friendsToFind);
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
        {
            Debug.Log($"OnStatusChanged: {statusCode}");
        }
        switch (statusCode)
        {
            case StatusCode.Connect:
                if (State == global::PeerState.ConnectingToNameServer)
                {
                    if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
                    {
                        Debug.Log("Connected to NameServer.");
                    }
                    server = ServerConnection.NameServer;
                    if (CustomAuthenticationValues != null)
                    {
                        CustomAuthenticationValues.Secret = null;
                    }
                }
                if (State == global::PeerState.ConnectingToGameserver)
                {
                    if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
                    {
                        Debug.Log("Connected to gameserver.");
                    }
                    server = ServerConnection.GameServer;
                    State = global::PeerState.ConnectedToGameserver;
                }
                if (State == global::PeerState.ConnectingToMasterserver)
                {
                    if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
                    {
                        Debug.Log("Connected to masterserver.");
                    }
                    server = ServerConnection.MasterServer;
                    State = global::PeerState.ConnectedToMaster;
                    if (IsInitialConnect)
                    {
                        IsInitialConnect = false;
                        SendMonoMessage(PhotonNetworkingMessage.OnConnectedToPhoton);
                    }
                }
                EstablishEncryption();
                if (IsAuthorizeSecretAvailable)
                {
                    didAuthenticate = OpAuthenticate(mAppId, mAppVersionPun, PlayerName, CustomAuthenticationValues, CloudRegion.ToString());
                    if (didAuthenticate)
                    {
                        State = global::PeerState.Authenticating;
                    }
                }
                break;
            case StatusCode.EncryptionEstablished:
                if (server == ServerConnection.NameServer)
                {
                    State = global::PeerState.ConnectedToNameServer;
                    if (!didAuthenticate && CloudRegion == CloudRegionCode.none)
                    {
                        OpGetRegions(mAppId);
                    }
                }
                if (!didAuthenticate && (!IsUsingNameServer || CloudRegion != CloudRegionCode.none))
                {
                    // TODO: Mod
                    didAuthenticate = OpAuthenticate(mAppId, mAppVersionPun, PlayerName, CustomAuthenticationValues, CloudRegion.ToString());
                    if (didAuthenticate)
                    {
                        State = global::PeerState.Authenticating;
                    }
                }
                break;
            case StatusCode.EncryptionFailedToEstablish:
                Debug.LogError("Encryption wasn't established: " + statusCode + ". Going to authenticate anyways.");
                AuthenticationValues authV = this.CustomAuthenticationValues ?? new AuthenticationValues() { UserId = this.PlayerName };
                OpAuthenticate(mAppId, mAppVersionPun, PlayerName, authV, CloudRegion.ToString());
                break;
            case StatusCode.Disconnect:
                didAuthenticate = false;
                isFetchingFriends = false;
                if (server == ServerConnection.GameServer)
                {
                    LeftRoomCleanup();
                }
                if (server == ServerConnection.MasterServer)
                {
                    LeftLobbyCleanup();
                }
                if (State == global::PeerState.DisconnectingFromMasterserver)
                {
                    if (Connect(mGameserver, ServerConnection.GameServer))
                    {
                        State = global::PeerState.ConnectingToGameserver;
                    }
                    break;
                }
                if (State == global::PeerState.DisconnectingFromGameserver || State == global::PeerState.DisconnectingFromNameServer)
                {
                    if (Connect(MasterServerAddress, ServerConnection.MasterServer))
                    {
                        State = global::PeerState.ConnectingToMasterserver;
                    }
                    break;
                }
                if (CustomAuthenticationValues != null)
                {
                    CustomAuthenticationValues.Secret = null;
                }
                State = global::PeerState.PeerCreated;
                SendMonoMessage(PhotonNetworkingMessage.OnDisconnectedFromPhoton);
                break;
            case StatusCode.SecurityExceptionOnConnect:
            case StatusCode.ExceptionOnConnect:
                {
                    State = global::PeerState.PeerCreated;
                    if (CustomAuthenticationValues != null)
                    {
                        CustomAuthenticationValues.Secret = null;
                    }
                    DisconnectCause disconnectCause = (DisconnectCause)statusCode;
                    SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, disconnectCause);
                    break;
                }
            case StatusCode.Exception:
                if (IsInitialConnect)
                {
                    Debug.LogError("Exception while connecting to: " + base.ServerAddress + ". Check if the server is available.");
                    if (base.ServerAddress == null || base.ServerAddress.StartsWith("127.0.0.1"))
                    {
                        Debug.LogWarning("The server address is 127.0.0.1 (localhost): Make sure the server is running on this machine. Android and iOS emulators have their own localhost.");
                        if (base.ServerAddress == mGameserver)
                        {
                            Debug.LogWarning("This might be a misconfiguration in the game server config. You need to edit it to a (public) address.");
                        }
                    }
                    State = global::PeerState.PeerCreated;
                    DisconnectCause disconnectCause = (DisconnectCause)statusCode;
                    SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, disconnectCause);
                }
                else
                {
                    State = global::PeerState.PeerCreated;
                    DisconnectCause disconnectCause = (DisconnectCause)statusCode;
                    SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, disconnectCause);
                }
                Disconnect();
                break;
            case StatusCode.ExceptionOnReceive:
            case StatusCode.TimeoutDisconnect:
            case StatusCode.DisconnectByServer:
            case StatusCode.DisconnectByServerUserLimit:
            case StatusCode.DisconnectByServerLogic:
                if (IsInitialConnect)
                {
                    Debug.LogWarning(statusCode + " while connecting to: " + base.ServerAddress + ". Check if the server is available.");
                    DisconnectCause disconnectCause = (DisconnectCause)statusCode;
                    SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, disconnectCause);
                }
                else
                {
                    DisconnectCause disconnectCause = (DisconnectCause)statusCode;
                    SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, disconnectCause);
                }
                if (CustomAuthenticationValues != null)
                {
                    CustomAuthenticationValues.Secret = null;
                }
                Disconnect();
                break;
            case StatusCode.QueueIncomingReliableWarning:
            case StatusCode.QueueIncomingUnreliableWarning:
                Debug.Log(statusCode + ". This client buffers many incoming messages. This is OK temporarily. With lots of these warnings, check if you send too much or execute messages too slow. " + ((!PhotonNetwork.isMessageQueueRunning) ? "Your isMessageQueueRunning is false. This can cause the issue temporarily." : string.Empty));
                break;
            default:
                Debug.LogError("Received unknown status code: " + statusCode);
                break;
            case StatusCode.QueueOutgoingReliableWarning:
            case StatusCode.QueueOutgoingUnreliableWarning:
            case StatusCode.SendError:
            case StatusCode.QueueOutgoingAcksWarning:
            case StatusCode.QueueSentWarning:
                break;
        }
        externalListener.OnStatusChanged(statusCode);
    }

    public static void SendMonoMessage(PhotonNetworkingMessage methodString, params object[] parameters)
    {
        HashSet<GameObject> hashSet;
        if (PhotonNetwork.SendMonoMessageTargets != null)
        {
            hashSet = PhotonNetwork.SendMonoMessageTargets;
        }
        else
        {
            hashSet = new HashSet<GameObject>();
            Component[] array = (Component[])UnityEngine.Object.FindObjectsOfType(typeof(MonoBehaviour));
            for (int i = 0; i < array.Length; i++)
            {
                hashSet.Add(array[i].gameObject);
            }
        }
        string methodName = methodString.ToString();
        foreach (GameObject item in hashSet)
        {
            if (parameters != null && parameters.Length == 1)
            {
                item.SendMessage(methodName, parameters[0], SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                item.SendMessage(methodName, parameters, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    /*
     * rpcData[(byte)0] = int - viewID
     * rpcData[(byte)1] = short - PhotonView prefix
     * rpcData[(byte)2] = int - Time
     * rpcData[(byte)3] = string - RPC name
     * rpcData[(byte)4] = object[] - RPC parameters
     * rpcData[(byte)5] = byte - RPC index
     */
    public void ExecuteRPC(ExitGames.Client.Photon.Hashtable rpcData, PhotonPlayer sender)
    {
        string rpcName = string.Empty;
        int viewId = (int)rpcData[(byte)0];
        int viewPrefix = rpcData.ContainsKey((byte)1) ? (short)rpcData[(byte)1] : 0;
        int timestamp = rpcData.ContainsKey((byte)2) ? (int)rpcData[(byte)2] : ServerTimeInMilliSeconds;

        if (rpcData.ContainsKey((byte)5))
        {
            int rpcIndex = (byte)rpcData[(byte)5];
            if (rpcIndex < PhotonNetwork.PhotonServerSettings.RpcList.Count)
            {
                rpcName = PhotonNetwork.PhotonServerSettings.RpcList[rpcIndex];
            }
        }
        else
        {
            rpcName = (string)rpcData[(byte)3];
        }

        object[] parameters = new object[0];
        if (rpcData.ContainsKey((byte)4))
        {
            parameters = (object[])rpcData[(byte)4] ?? new object[0];
        }

        System.Type[] callParameterTypes = new System.Type[parameters.Length];
        for (int i = 0; i < callParameterTypes.Length; i++)
        {
            object obj = parameters[i];
            callParameterTypes[i] = obj?.GetType();
        }

        string argsAsString = string.Empty;
        for (int i = 0; i < callParameterTypes.Length; i++)
        {
            System.Type type = callParameterTypes[i];
            if (argsAsString.Length > 0)
            {
                argsAsString += ", ";
            }
            argsAsString += type == null ? "null" : type.Name;
        }
        bool isKnown = false;

        PhotonView photonView = this.GetPhotonView(viewId);
        if (photonView == null)
        {
            int ownerId = viewId / PhotonNetwork.MAX_VIEW_IDS;
            bool isOurs = ownerId == mLocalActor.Id;
            bool isSenders = ownerId == sender.Id;
            if (isOurs)
            {
                Debug.LogWarning("Received RPC \"" + rpcName + "\" for viewID " + viewId + " but this PhotonView does not exist! View was/is ours." + (!isSenders ? " Remote called." : " Owner called."));
            }
            else
            {
                Debug.LogError("Received RPC \"" + rpcName + "\" for viewID " + viewId + " but this PhotonView does not exist! Was remote PV." + (!isSenders ? " Remote called." : " Owner called."));
            }
            return;
        }
        if (photonView.prefix != viewPrefix)
        {
            Debug.LogError("Received RPC \"" + rpcName + "\" on viewID " + viewId + " with a prefix of " + viewPrefix + ", our prefix is " + photonView.prefix + ". The RPC has been ignored.");
            return;
        }

        if (photonView != null && rpcName.Length != 0 && photonView.prefix == viewPrefix)
        {
            if (photonView.group == 0 || this.allowedReceivingGroups.Contains(photonView.group))
            {
                List<MonoBehaviour> behaviours = null;
                if (this.monoRPCBehavioursCache.ContainsKey(photonView))
                {
                    behaviours = this.monoRPCBehavioursCache[photonView];
                }
                if (behaviours == null)
                {
                    behaviours = this.monoRPCBehavioursCache[photonView] = photonView.GetComponents<MonoBehaviour>().Where(b => b != null).ToList();
                }
                foreach (MonoBehaviour behaviour in behaviours)
                {
                    System.Type keyType = behaviour.GetType();
                    List<MethodInfo> methods = null;
                    if (this.monoRPCMethodsCache.ContainsKey(keyType))
                    {
                        methods = this.monoRPCMethodsCache[keyType];
                    }

                    if (methods == null)
                    {
                        methods = this.monoRPCMethodsCache[keyType] = SupportClass.GetMethods(keyType, typeof(RPC));
                    }

                    if (methods != null)
                    {
                        foreach (MethodInfo method in methods)
                        {
                            if (method.Name.Equals(rpcName))
                            {
                                isKnown = true;
                                ParameterInfo[] methodParameters = method.GetParameters();
                                if (this.CheckTypeMatch(methodParameters, callParameterTypes))
                                {
                                    object[] _params = parameters;
                                    if (methodParameters.Length > 0 && methodParameters[methodParameters.Length - 1].ParameterType == typeof(PhotonMessageInfo))
                                    {
                                        if (callParameterTypes.Length == methodParameters.Length)
                                        {
                                            Guardian.Mod.Logger.Error($"Spoofed '{rpcName}({argsAsString})' RPC from #{(sender == null ? "?" : sender.Id.ToString())}.");
                                            if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.Id))
                                            {
                                                FengGameManagerMKII.IgnoreList.Add(sender.Id);
                                            }
                                            break;
                                        }
                                        object[] tmp = new object[_params.Length + 1];
                                        _params.CopyTo(tmp, 0);
                                        tmp[tmp.Length - 1] = new PhotonMessageInfo(sender, timestamp, photonView);
                                        _params = tmp;
                                    }
                                    object returnVal = method.Invoke(behaviour, _params);
                                    if (method.ReturnType == typeof(IEnumerator))
                                    {
                                        behaviour.StartCoroutine((IEnumerator)returnVal);
                                    }
                                }
                                else if (methodParameters.Length == 1 && methodParameters[0].ParameterType.IsArray)
                                {
                                    object returnVal = method.Invoke(behaviour, new object[] { parameters });
                                    if (method.ReturnType == typeof(IEnumerator))
                                    {
                                        behaviour.StartCoroutine((IEnumerator)returnVal);
                                    }
                                }
                                else
                                {
                                    Guardian.Mod.Logger.Warn($"Invalid '{rpcName}' RPC from #{(sender == null ? "?" : sender.Id.ToString())}, parameters: {argsAsString}.");
                                }
                            }
                        }
                    }
                }
            }
        }
        if (sender != null && photonView != null)
        {
            if (!isKnown)
            {
                switch (rpcName)
                {
                    case "SetupThunderSpearsRPC": // Updated RC
                    case "SetThunderSpearsRPC":
                    case "IsUpdatedRPC":
                    case "DisableRPC":
                        sender.IsNewRC = true;
                        break;
                    case "pedoModUser": // PedoBear
                    case "NetThrowBlade":
                    case "FireSingleTS":
                    case "dropObj":
                    case "GravityChange":
                    case "dropPicked":
                        sender.IsPedoBear = true;
                        break;
                    case "whoIsMyReinerTitan": // Universe
                    case "whoIsMyAnnieTitan":
                    case "backToAnnieHumanRPC":
                    case "whoIsMyColossalTitan":
                    case "SetAnimationSpeed":
                    case "GoBerserk":
                    case "SetBerserkTexture":
                    case "CrownRPC":
                        sender.IsUniverse = true;
                        break;
                    case "Cyan_modRPC": // Cyan Mod
                    case "LoadObjects":
                    case "newObject":
                        sender.IsCyan = true;
                        break;
                    case "RPC_Ball":
                        sender.IsKNK = true;
                        break;
                    case "NRCRPC":
                        sender.IsNRC = true;
                        break;
                    case "RecompilePlayerRPC": // I'll never know
                    case "NekoEarsRPC":
                    case "FoxTailRPC":
                    case "WingsRPC":
                    case "HornsRPC":
                    case "NekoRPC":
                        break;
                    case "receiveSatanPlayers": // RC83
                        sender.IsRC83 = true;
                        break;
                    case "AddMeToCEList": // Cyrus Essentials
                        sender.IsCyrus = true;
                        break;
                    case "ResetRPCMgr": // ExpMod
                    case "HookDMRPC":
                    case "GetDownRPC":
                    case "pairRPC": // ExpMod?
                    case "flareColorRPC":
                    case "EMCustomMapRPC":
                    case "AniSpeed":
                        sender.IsEXP = true;
                        break;
                    case "TrapJoin": // TRAP
                        sender.IsTRAP = true;
                        break;
                    case "team_winner_popup": // Ranked RC
                        sender.IsRRC = true;
                        break;
                    default:
                        Guardian.Mod.Logger.Warn($"No '{rpcName}({argsAsString})' from #{sender.Id} in PV {viewId}.");
                        break;
                }
            }
        }
    }

    private bool CheckTypeMatch(ParameterInfo[] methodParameters, Type[] callParameterTypes)
    {
        if (methodParameters.Length < callParameterTypes.Length)
        {
            return false;
        }
        for (int i = 0; i < callParameterTypes.Length; i++)
        {
            Type parameterType = methodParameters[i].ParameterType;
            if (callParameterTypes[i] != null && !parameterType.Equals(callParameterTypes[i]))
            {
                return false;
            }
        }
        return true;
    }

    internal ExitGames.Client.Photon.Hashtable SendInstantiate(string prefabName, Vector3 position, Quaternion rotation, int group, int[] viewIDs, object[] data, bool isGlobalObject)
    {
        int num = viewIDs[0];
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable[(byte)0] = prefabName;
        if (position != Vector3.zero)
        {
            hashtable[(byte)1] = position;
        }
        if (rotation != Quaternion.identity)
        {
            hashtable[(byte)2] = rotation;
        }
        if (group != 0)
        {
            hashtable[(byte)3] = group;
        }
        if (viewIDs.Length > 1)
        {
            hashtable[(byte)4] = viewIDs;
        }
        if (data != null)
        {
            hashtable[(byte)5] = data;
        }
        if (currentLevelPrefix > 0)
        {
            hashtable[(byte)8] = currentLevelPrefix;
        }
        hashtable[(byte)6] = base.ServerTimeInMilliSeconds;
        hashtable[(byte)7] = num;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.CachingOption = ((!isGlobalObject) ? EventCaching.AddToRoomCache : EventCaching.AddToRoomCacheGlobal);
        OpRaiseEvent(202, hashtable, sendReliable: true, raiseEventOptions);
        return hashtable;
    }

    private void StoreInstantiationData(int instantiationId, object[] instantiationData)
    {
        tempInstantiationData[instantiationId] = instantiationData;
    }

    public object[] FetchInstantiationData(int instantiationId)
    {
        if (instantiationId == 0)
        {
            return null;
        }
        tempInstantiationData.TryGetValue(instantiationId, out object[] value);
        return value;
    }

    private void RemoveInstantiationData(int instantiationId)
    {
        tempInstantiationData.Remove(instantiationId);
    }

    public void RemoveAllInstantiatedObjects()
    {
        GameObject[] array = new GameObject[instantiatedObjects.Count];
        instantiatedObjects.Values.CopyTo(array, 0);
        foreach (GameObject gameObject in array)
        {
            if (!(gameObject == null))
            {
                RemoveInstantiatedGO(gameObject, localOnly: false);
            }
        }
        if (instantiatedObjects.Count > 0)
        {
            Debug.LogError("RemoveAllInstantiatedObjects() this.instantiatedObjects.Count should be 0 by now.");
        }
        instantiatedObjects = new Dictionary<int, GameObject>();
    }

    public void DestroyPlayerObjects(int playerId, bool localOnly)
    {
        if (playerId <= 0)
        {
            Debug.LogError("Failed to Destroy objects of playerId: " + playerId);
            return;
        }
        if (!localOnly)
        {
            OpRemoveFromServerInstantiationsOfPlayer(playerId);
            OpCleanRpcBuffer(playerId);
            SendDestroyOfPlayer(playerId);
        }
        Queue<GameObject> queue = new Queue<GameObject>();
        int num = playerId * PhotonNetwork.MAX_VIEW_IDS;
        int num2 = num + PhotonNetwork.MAX_VIEW_IDS;
        foreach (KeyValuePair<int, GameObject> instantiatedObject in instantiatedObjects)
        {
            if (instantiatedObject.Key > num && instantiatedObject.Key < num2)
            {
                queue.Enqueue(instantiatedObject.Value);
            }
        }
        foreach (GameObject item in queue)
        {
            RemoveInstantiatedGO(item, localOnly: true);
        }
    }

    public void DestroyAll(bool localOnly)
    {
        if (!localOnly)
        {
            OpRemoveCompleteCache();
            SendDestroyOfAll();
        }
        LocalCleanupAnythingInstantiated(destroyInstantiatedGameObjects: true);
    }

    public void RemoveInstantiatedGO(GameObject go, bool localOnly)
    {
        if (go == null)
        {
            Debug.LogError("Failed to 'network-remove' GameObject because it's null.");
            return;
        }
        PhotonView[] componentsInChildren = go.GetComponentsInChildren<PhotonView>();
        if (componentsInChildren == null || componentsInChildren.Length <= 0)
        {
            Debug.LogError("Failed to 'network-remove' GameObject because has no PhotonView components: " + go);
            return;
        }
        PhotonView photonView = componentsInChildren[0];
        int ownerActorNr = photonView.OwnerActorNr;
        int instantiationId = photonView.instantiationId;
        if (!localOnly)
        {
            if (!photonView.isMine && (!mLocalActor.isMasterClient || mActors.ContainsKey(ownerActorNr)))
            {
                Debug.LogError("Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left: " + photonView);
                return;
            }
            if (instantiationId < 1)
            {
                Debug.LogError("Failed to 'network-remove' GameObject because it is missing a valid InstantiationId on view: " + photonView + ". Not Destroying GameObject or PhotonViews!");
                return;
            }
        }
        if (!localOnly)
        {
            ServerCleanInstantiateAndDestroy(instantiationId, ownerActorNr);
        }
        instantiatedObjects.Remove(instantiationId);
        for (int num = componentsInChildren.Length - 1; num >= 0; num--)
        {
            PhotonView photonView2 = componentsInChildren[num];
            if (!(photonView2 == null))
            {
                if (photonView2.instantiationId >= 1)
                {
                    LocalCleanPhotonView(photonView2);
                }
                if (!localOnly)
                {
                    OpCleanRpcBuffer(photonView2);
                }
            }
        }
        if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
        {
            Debug.Log("Network destroy Instantiated GO: " + go.name);
        }
        UnityEngine.Object.Destroy(go);
    }

    public int GetInstantiatedObjectsId(GameObject go)
    {
        int result = -1;
        if (go == null)
        {
            Debug.LogError("GetInstantiatedObjectsId() for GO == null.");
            return result;
        }
        PhotonView[] photonViewsInChildren = go.GetPhotonViewsInChildren();
        if (photonViewsInChildren != null && photonViewsInChildren.Length > 0 && photonViewsInChildren[0] != null)
        {
            return photonViewsInChildren[0].instantiationId;
        }
        if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
        {
            Debug.Log("GetInstantiatedObjectsId failed for GO: " + go);
        }
        return result;
    }

    private void ServerCleanInstantiateAndDestroy(int instantiateId, int actorNr)
    {
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable[(byte)7] = instantiateId;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
        raiseEventOptions.TargetActors = new int[1]
        {
            actorNr
        };
        RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
        OpRaiseEvent(202, hashtable, sendReliable: true, raiseEventOptions2);
        ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
        hashtable2[(byte)0] = instantiateId;
        OpRaiseEvent(204, hashtable2, sendReliable: true, null);
    }

    private void SendDestroyOfPlayer(int actorNr)
    {
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable[(byte)0] = actorNr;
        OpRaiseEvent(207, hashtable, sendReliable: true, null);
    }

    private void SendDestroyOfAll()
    {
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable[(byte)0] = -1;
        OpRaiseEvent(207, hashtable, sendReliable: true, null);
    }

    private void OpRemoveFromServerInstantiationsOfPlayer(int actorNr)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
        raiseEventOptions.TargetActors = new int[1]
        {
            actorNr
        };
        RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
        OpRaiseEvent(202, null, sendReliable: true, raiseEventOptions2);
    }

    public void LocalCleanPhotonView(PhotonView view)
    {
        view.destroyedByPhotonNetworkOrQuit = true;
        photonViewList.Remove(view.viewID);

        // TODO: Mod
        monoRPCBehavioursCache.Remove(view);
    }

    public PhotonView GetPhotonView(int viewID)
    {
        photonViewList.TryGetValue(viewID, out PhotonView value);
        if (value == null)
        {
            PhotonView[] array = UnityEngine.Object.FindObjectsOfType(typeof(PhotonView)) as PhotonView[];
            PhotonView[] array2 = array;
            foreach (PhotonView photonView in array2)
            {
                if (photonView.viewID == viewID)
                {
                    if (photonView.didAwake)
                    {
                        Debug.LogWarning("Had to lookup view that wasn't in dict: " + photonView);
                    }
                    return photonView;
                }
            }
        }
        return value;
    }

    public void RegisterPhotonView(PhotonView netView)
    {
        if (!Application.isPlaying)
        {
            photonViewList = new Dictionary<int, PhotonView>();
        }
        else
        {
            if (netView.subId == 0)
            {
                return;
            }
            if (photonViewList.TryGetValue(netView.viewID, out PhotonView listedView))
            {
                if (netView != listedView)
                {
                    Debug.LogError($"PhotonView ID duplicate found: {netView.viewID}. New: {netView} old: {listedView}. Maybe one wasn't destroyed on scene load?! Check for 'DontDestroyOnLoad'. Destroying old entry, adding new.");
                    RemoveInstantiatedGO(listedView.gameObject, localOnly: true);
                }
            }
            photonViewList.Add(netView.viewID, netView);
            if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
            {
                Debug.Log("Registered PhotonView: " + netView.viewID);
            }
        }
    }

    public void OpCleanRpcBuffer(int actorNumber)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
        raiseEventOptions.TargetActors = new int[1]
        {
            actorNumber
        };
        RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
        OpRaiseEvent(200, null, sendReliable: true, raiseEventOptions2);
    }

    public void OpRemoveCompleteCacheOfPlayer(int actorNumber)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
        raiseEventOptions.TargetActors = new int[1]
        {
            actorNumber
        };
        RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
        OpRaiseEvent(0, null, sendReliable: true, raiseEventOptions2);
    }

    public void OpRemoveCompleteCache()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
        raiseEventOptions.Receivers = ReceiverGroup.MasterClient;
        RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
        OpRaiseEvent(0, null, sendReliable: true, raiseEventOptions2);
    }

    private void RemoveCacheOfLeftPlayers()
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary[244] = (byte)0;
        dictionary[247] = (byte)7;
        OpCustom(253, dictionary, sendReliable: true, 0);
    }

    public void CleanRpcBufferIfMine(PhotonView view)
    {
        if (view.ownerId != mLocalActor.Id && !mLocalActor.isMasterClient)
        {
            Debug.LogError("Cannot remove cached RPCs on a PhotonView thats not ours! " + view.owner + " scene: " + view.isSceneView);
        }
        else
        {
            OpCleanRpcBuffer(view);
        }
    }

    public void OpCleanRpcBuffer(PhotonView view)
    {
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable[(byte)0] = view.viewID;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
        RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
        OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions2);
    }

    public void RemoveRPCsInGroup(int group)
    {
        foreach (KeyValuePair<int, PhotonView> photonView in photonViewList)
        {
            PhotonView value = photonView.Value;
            if (value.group == group)
            {
                CleanRpcBufferIfMine(value);
            }
        }
    }

    public void SetLevelPrefix(short prefix)
    {
        currentLevelPrefix = prefix;
    }

    internal void RPC(PhotonView view, string methodName, PhotonPlayer player, params object[] parameters)
    {
        if (!blockSendingGroups.Contains(view.group))
        {
            if (view.viewID < 1)
            {
                Debug.LogError("Illegal view ID:" + view.viewID + " method: " + methodName + " GO:" + view.gameObject.name);
            }
            if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
            {
                Debug.Log("Sending RPC \"" + methodName + "\" to player[" + player + "]");
            }
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable[(byte)0] = view.viewID;
            if (view.prefix > 0)
            {
                hashtable[(byte)1] = (short)view.prefix;
            }
            hashtable[(byte)2] = base.ServerTimeInMilliSeconds;
            if (rpcShortcuts.TryGetValue(methodName, out int value))
            {
                hashtable[(byte)5] = (byte)value;
            }
            else
            {
                hashtable[(byte)3] = methodName;
            }
            if (parameters != null && parameters.Length > 0)
            {
                hashtable[(byte)4] = parameters;
            }
            if (mLocalActor == player)
            {
                ExecuteRPC(hashtable, player);
                return;
            }
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
            raiseEventOptions.TargetActors = new int[1]
            {
                player.Id
            };
            RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
            OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions2);
        }
    }

    internal void RPC(PhotonView view, string methodName, PhotonTargets target, params object[] parameters)
    {
        if (blockSendingGroups.Contains(view.group))
        {
            return;
        }
        if (view.viewID < 1)
        {
            Debug.LogError("Illegal view ID:" + view.viewID + " method: " + methodName + " GO:" + view.gameObject.name);
        }
        if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
        {
            Debug.Log("Sending RPC \"" + methodName + "\" to " + target);
        }
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable[(byte)0] = view.viewID;
        if (view.prefix > 0)
        {
            hashtable[(byte)1] = (short)view.prefix;
        }
        hashtable[(byte)2] = base.ServerTimeInMilliSeconds;
        if (rpcShortcuts.TryGetValue(methodName, out int value))
        {
            hashtable[(byte)5] = (byte)value;
        }
        else
        {
            hashtable[(byte)3] = methodName;
        }
        if (parameters != null && parameters.Length > 0)
        {
            hashtable[(byte)4] = parameters;
        }
        switch (target)
        {
            case PhotonTargets.All:
                {
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                    raiseEventOptions.InterestGroup = (byte)view.group;
                    RaiseEventOptions raiseEventOptions6 = raiseEventOptions;
                    OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions6);
                    ExecuteRPC(hashtable, mLocalActor);
                    break;
                }
            case PhotonTargets.Others:
                {
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                    raiseEventOptions.InterestGroup = (byte)view.group;
                    RaiseEventOptions raiseEventOptions5 = raiseEventOptions;
                    OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions5);
                    break;
                }
            case PhotonTargets.AllBuffered:
                {
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                    raiseEventOptions.CachingOption = EventCaching.AddToRoomCache;
                    RaiseEventOptions raiseEventOptions8 = raiseEventOptions;
                    OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions8);
                    ExecuteRPC(hashtable, mLocalActor);
                    break;
                }
            case PhotonTargets.OthersBuffered:
                {
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                    raiseEventOptions.CachingOption = EventCaching.AddToRoomCache;
                    RaiseEventOptions raiseEventOptions7 = raiseEventOptions;
                    OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions7);
                    break;
                }
            case PhotonTargets.MasterClient:
                {
                    if (mMasterClient == mLocalActor)
                    {
                        ExecuteRPC(hashtable, mLocalActor);
                        break;
                    }
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                    raiseEventOptions.Receivers = ReceiverGroup.MasterClient;
                    RaiseEventOptions raiseEventOptions4 = raiseEventOptions;
                    OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions4);
                    break;
                }
            case PhotonTargets.AllViaServer:
                {
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                    raiseEventOptions.InterestGroup = (byte)view.group;
                    raiseEventOptions.Receivers = ReceiverGroup.All;
                    RaiseEventOptions raiseEventOptions3 = raiseEventOptions;
                    OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions3);
                    break;
                }
            case PhotonTargets.AllBufferedViaServer:
                {
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                    raiseEventOptions.InterestGroup = (byte)view.group;
                    raiseEventOptions.Receivers = ReceiverGroup.All;
                    raiseEventOptions.CachingOption = EventCaching.AddToRoomCache;
                    RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
                    OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions2);
                    break;
                }
            default:
                Debug.LogError("Unsupported target enum: " + target);
                break;
        }
    }

    public void SetReceivingEnabled(int group, bool enabled)
    {
        if (group <= 0)
        {
            Debug.LogError("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: " + group + ". The group number should be at least 1.");
        }
        else if (enabled)
        {
            if (!allowedReceivingGroups.Contains(group))
            {
                allowedReceivingGroups.Add(group);
                byte[] groupsToAdd = new byte[1]
                {
                    (byte)group
                };
                OpChangeGroups(null, groupsToAdd);
            }
        }
        else if (allowedReceivingGroups.Contains(group))
        {
            allowedReceivingGroups.Remove(group);
            byte[] groupsToRemove = new byte[1]
            {
                (byte)group
            };
            OpChangeGroups(groupsToRemove, null);
        }
    }

    public void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
    {
        List<byte> list = new List<byte>();
        List<byte> list2 = new List<byte>();
        if (enableGroups != null)
        {
            foreach (int num in enableGroups)
            {
                if (num <= 0)
                {
                    Debug.LogError("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: " + num + ". The group number should be at least 1.");
                }
                else if (!allowedReceivingGroups.Contains(num))
                {
                    allowedReceivingGroups.Add(num);
                    list.Add((byte)num);
                }
            }
        }
        if (disableGroups != null)
        {
            foreach (int num2 in disableGroups)
            {
                if (num2 <= 0)
                {
                    Debug.LogError("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: " + num2 + ". The group number should be at least 1.");
                }
                else if (list.Contains((byte)num2))
                {
                    Debug.LogError("Error: PhotonNetwork.SetReceivingEnabled disableGroups contains a group that is also in the enableGroups: " + num2 + ".");
                }
                else if (allowedReceivingGroups.Contains(num2))
                {
                    allowedReceivingGroups.Remove(num2);
                    list2.Add((byte)num2);
                }
            }
        }
        OpChangeGroups((list2.Count <= 0) ? null : list2.ToArray(), (list.Count <= 0) ? null : list.ToArray());
    }

    public void SetSendingEnabled(int group, bool enabled)
    {
        if (!enabled)
        {
            blockSendingGroups.Add(group);
        }
        else
        {
            blockSendingGroups.Remove(group);
        }
    }

    public void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
    {
        if (enableGroups != null)
        {
            foreach (int item in enableGroups)
            {
                if (blockSendingGroups.Contains(item))
                {
                    blockSendingGroups.Remove(item);
                }
            }
        }
        if (disableGroups == null)
        {
            return;
        }
        foreach (int item2 in disableGroups)
        {
            if (!blockSendingGroups.Contains(item2))
            {
                blockSendingGroups.Add(item2);
            }
        }
    }

    public void NewSceneLoaded()
    {
        if (loadingLevelAndPausedNetwork)
        {
            loadingLevelAndPausedNetwork = false;
            PhotonNetwork.isMessageQueueRunning = true;
        }
        List<int> list = new List<int>();
        foreach (KeyValuePair<int, PhotonView> photonView in photonViewList)
        {
            PhotonView value = photonView.Value;
            if (value == null)
            {
                list.Add(photonView.Key);

                // TODO: Mod
                monoRPCBehavioursCache.Remove(value);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            int key = list[i];
            photonViewList.Remove(key);
        }
        if (list.Count > 0 && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
        {
            Debug.Log("New level loaded. Removed " + list.Count + " scene view IDs from last level.");
        }
    }

    public void RunViewUpdate()
    {
        if (PhotonNetwork.connected && !PhotonNetwork.offlineMode && mActors != null && mActors.Count > 1)
        {
            Dictionary<int, ExitGames.Client.Photon.Hashtable> dictionary = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();
            Dictionary<int, ExitGames.Client.Photon.Hashtable> dictionary2 = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();
            foreach (KeyValuePair<int, PhotonView> photonView in photonViewList)
            {
                PhotonView value = photonView.Value;
                if (value.observed != null && value.synchronization != 0 && (value.ownerId == mLocalActor.Id || (value.isSceneView && mMasterClient == mLocalActor)) && value.gameObject.activeInHierarchy && !blockSendingGroups.Contains(value.group))
                {
                    ExitGames.Client.Photon.Hashtable hashtable = OnSerializeWrite(value);
                    if (hashtable != null)
                    {
                        if (value.synchronization == ViewSynchronization.ReliableDeltaCompressed || value.mixedModeIsReliable)
                        {
                            if (hashtable.ContainsKey((byte)1) || hashtable.ContainsKey((byte)2))
                            {
                                if (!dictionary.ContainsKey(value.group))
                                {
                                    dictionary[value.group] = new ExitGames.Client.Photon.Hashtable();
                                    dictionary[value.group][(byte)0] = base.ServerTimeInMilliSeconds;
                                    if (currentLevelPrefix >= 0)
                                    {
                                        dictionary[value.group][(byte)1] = currentLevelPrefix;
                                    }
                                }
                                ExitGames.Client.Photon.Hashtable hashtable2 = dictionary[value.group];
                                hashtable2.Add((short)hashtable2.Count, hashtable);
                            }
                        }
                        else
                        {
                            if (!dictionary2.ContainsKey(value.group))
                            {
                                dictionary2[value.group] = new ExitGames.Client.Photon.Hashtable();
                                dictionary2[value.group][(byte)0] = base.ServerTimeInMilliSeconds;
                                if (currentLevelPrefix >= 0)
                                {
                                    dictionary2[value.group][(byte)1] = currentLevelPrefix;
                                }
                            }
                            ExitGames.Client.Photon.Hashtable hashtable3 = dictionary2[value.group];
                            hashtable3.Add((short)hashtable3.Count, hashtable);
                        }
                    }
                }
            }
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
            foreach (KeyValuePair<int, ExitGames.Client.Photon.Hashtable> item in dictionary)
            {
                raiseEventOptions.InterestGroup = (byte)item.Key;
                OpRaiseEvent(206, item.Value, sendReliable: true, raiseEventOptions);
            }
            foreach (KeyValuePair<int, ExitGames.Client.Photon.Hashtable> item2 in dictionary2)
            {
                raiseEventOptions.InterestGroup = (byte)item2.Key;
                OpRaiseEvent(201, item2.Value, sendReliable: false, raiseEventOptions);
            }
        }
    }

    private ExitGames.Client.Photon.Hashtable OnSerializeWrite(PhotonView view)
    {
        List<object> list = new List<object>();
        if (view.observed is MonoBehaviour)
        {
            PhotonStream photonStream = new PhotonStream(write: true, null);
            PhotonMessageInfo info = new PhotonMessageInfo(mLocalActor, base.ServerTimeInMilliSeconds, view);
            view.ExecuteOnSerialize(photonStream, info);
            if (photonStream.Count == 0)
            {
                return null;
            }
            list = photonStream.data;
        }
        else if (view.observed is Transform)
        {
            Transform transform = (Transform)view.observed;
            if (view.onSerializeTransformOption == OnSerializeTransform.OnlyPosition || view.onSerializeTransformOption == OnSerializeTransform.PositionAndRotation || view.onSerializeTransformOption == OnSerializeTransform.All)
            {
                list.Add(transform.localPosition);
            }
            else
            {
                list.Add(null);
            }
            if (view.onSerializeTransformOption == OnSerializeTransform.OnlyRotation || view.onSerializeTransformOption == OnSerializeTransform.PositionAndRotation || view.onSerializeTransformOption == OnSerializeTransform.All)
            {
                list.Add(transform.localRotation);
            }
            else
            {
                list.Add(null);
            }
            if (view.onSerializeTransformOption == OnSerializeTransform.OnlyScale || view.onSerializeTransformOption == OnSerializeTransform.All)
            {
                list.Add(transform.localScale);
            }
        }
        else
        {
            if (!(view.observed is Rigidbody))
            {
                Debug.LogError("Observed type is not serializable: " + view.observed.GetType());
                return null;
            }
            Rigidbody rigidbody = (Rigidbody)view.observed;
            if (view.onSerializeRigidBodyOption != OnSerializeRigidBody.OnlyAngularVelocity)
            {
                list.Add(rigidbody.velocity);
            }
            else
            {
                list.Add(null);
            }
            if (view.onSerializeRigidBodyOption != 0)
            {
                list.Add(rigidbody.angularVelocity);
            }
        }
        object[] array = list.ToArray();
        if (view.synchronization == ViewSynchronization.UnreliableOnChange)
        {
            if (AlmostEquals(array, view.lastOnSerializeDataSent))
            {
                if (view.mixedModeIsReliable)
                {
                    return null;
                }
                view.mixedModeIsReliable = true;
                view.lastOnSerializeDataSent = array;
            }
            else
            {
                view.mixedModeIsReliable = false;
                view.lastOnSerializeDataSent = array;
            }
        }
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable[(byte)0] = view.viewID;
        hashtable[(byte)1] = array;
        if (view.synchronization == ViewSynchronization.ReliableDeltaCompressed)
        {
            bool flag = DeltaCompressionWrite(view, hashtable);
            view.lastOnSerializeDataSent = array;
            if (!flag)
            {
                return null;
            }
        }
        return hashtable;
    }

    private void OnSerializeRead(ExitGames.Client.Photon.Hashtable data, PhotonPlayer sender, int networkTime, short correctPrefix)
    {
        int num = (int)data[(byte)0];
        PhotonView photonView = GetPhotonView(num);
        if (photonView == null)
        {
            Debug.LogWarning("Received OnSerialization for view ID " + num + ". We have no such PhotonView! Ignored this if you're leaving a room. State: " + State);
        }
        else if (photonView.prefix > 0 && correctPrefix != photonView.prefix)
        {
            Debug.LogError("Received OnSerialization for view ID " + num + " with prefix " + correctPrefix + ". Our prefix is " + photonView.prefix);
        }
        else
        {
            if (photonView.group != 0 && !allowedReceivingGroups.Contains(photonView.group))
            {
                return;
            }
            if (photonView.synchronization == ViewSynchronization.ReliableDeltaCompressed)
            {
                if (!DeltaCompressionRead(photonView, data))
                {
                    if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
                    {
                        Debug.Log("Skipping packet for " + photonView.name + " [" + photonView.viewID + "] as we haven't received a full packet for delta compression yet. This is OK if it happens for the first few frames after joining a game.");
                    }
                    return;
                }
                photonView.lastOnSerializeDataReceived = (data[(byte)1] as object[]);
            }
            if (photonView.observed is MonoBehaviour)
            {
                object[] incomingData = data[(byte)1] as object[];
                PhotonStream pStream = new PhotonStream(write: false, incomingData);
                PhotonMessageInfo info = new PhotonMessageInfo(sender, networkTime, photonView);
                photonView.ExecuteOnSerialize(pStream, info);
            }
            else if (photonView.observed is Transform)
            {
                object[] array = data[(byte)1] as object[];
                Transform transform = (Transform)photonView.observed;
                if (array.Length >= 1 && array[0] != null)
                {
                    transform.localPosition = (Vector3)array[0];
                }
                if (array.Length >= 2 && array[1] != null)
                {
                    transform.localRotation = (Quaternion)array[1];
                }
                if (array.Length >= 3 && array[2] != null)
                {
                    transform.localScale = (Vector3)array[2];
                }
            }
            else if (photonView.observed is Rigidbody)
            {
                object[] array2 = data[(byte)1] as object[];
                Rigidbody rigidbody = (Rigidbody)photonView.observed;
                if (array2.Length >= 1 && array2[0] != null)
                {
                    rigidbody.velocity = (Vector3)array2[0];
                }
                if (array2.Length >= 2 && array2[1] != null)
                {
                    rigidbody.angularVelocity = (Vector3)array2[1];
                }
            }
            else
            {
                Debug.LogError("Type of observed is unknown when receiving.");
            }
        }
    }

    private bool AlmostEquals(object[] lastData, object[] currentContent)
    {
        if (lastData == null && currentContent == null)
        {
            return true;
        }
        if (lastData == null || currentContent == null || lastData.Length != currentContent.Length)
        {
            return false;
        }
        for (int i = 0; i < currentContent.Length; i++)
        {
            object one = currentContent[i];
            object two = lastData[i];
            if (!ObjectIsSameWithInprecision(one, two))
            {
                return false;
            }
        }
        return true;
    }

    private bool DeltaCompressionWrite(PhotonView view, ExitGames.Client.Photon.Hashtable data)
    {
        if (view.lastOnSerializeDataSent == null)
        {
            return true;
        }
        object[] lastOnSerializeDataSent = view.lastOnSerializeDataSent;
        object[] array = data[(byte)1] as object[];
        if (array == null)
        {
            return false;
        }
        if (lastOnSerializeDataSent.Length != array.Length)
        {
            return true;
        }
        object[] array2 = new object[array.Length];
        int num = 0;
        List<int> list = new List<int>();
        for (int i = 0; i < array2.Length; i++)
        {
            object obj = array[i];
            object two = lastOnSerializeDataSent[i];
            if (ObjectIsSameWithInprecision(obj, two))
            {
                num++;
                continue;
            }
            array2[i] = array[i];
            if (obj == null)
            {
                list.Add(i);
            }
        }
        if (num > 0)
        {
            data.Remove((byte)1);
            if (num == array.Length)
            {
                return false;
            }
            data[(byte)2] = array2;
            if (list.Count > 0)
            {
                data[(byte)3] = list.ToArray();
            }
        }
        return true;
    }

    private bool DeltaCompressionRead(PhotonView view, ExitGames.Client.Photon.Hashtable data)
    {
        if (data.ContainsKey((byte)1))
        {
            return true;
        }
        if (view.lastOnSerializeDataReceived == null)
        {
            return false;
        }
        object[] array = data[(byte)2] as object[];
        if (array == null)
        {
            return false;
        }
        int[] array2 = data[(byte)3] as int[];
        if (array2 == null)
        {
            array2 = new int[0];
        }
        object[] lastOnSerializeDataReceived = view.lastOnSerializeDataReceived;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == null && !array2.Contains(i))
            {
                array[i] = lastOnSerializeDataReceived[i];
            }
        }
        data[(byte)1] = array;
        return true;
    }

    private bool ObjectIsSameWithInprecision(object one, object two)
    {
        if (one == null || two == null)
        {
            return one == null && two == null;
        }
        if (!one.Equals(two))
        {
            if (one is Vector3)
            {
                Vector3 target = (Vector3)one;
                Vector3 second = (Vector3)two;
                if (target.AlmostEquals(second, PhotonNetwork.precisionForVectorSynchronization))
                {
                    return true;
                }
            }
            else if (one is Vector2)
            {
                Vector2 target2 = (Vector2)one;
                Vector2 second2 = (Vector2)two;
                if (target2.AlmostEquals(second2, PhotonNetwork.precisionForVectorSynchronization))
                {
                    return true;
                }
            }
            else if (one is Quaternion)
            {
                Quaternion target3 = (Quaternion)one;
                Quaternion second3 = (Quaternion)two;
                if (target3.AlmostEquals(second3, PhotonNetwork.precisionForQuaternionSynchronization))
                {
                    return true;
                }
            }
            else if (one is float)
            {
                float target4 = (float)one;
                float second4 = (float)two;
                if (target4.AlmostEquals(second4, PhotonNetwork.precisionForFloatSynchronization))
                {
                    return true;
                }
            }
            return false;
        }
        return true;
    }

    protected internal static bool GetMethod(MonoBehaviour monob, string methodType, out MethodInfo mi)
    {
        mi = null;
        if (monob == null || string.IsNullOrEmpty(methodType))
        {
            return false;
        }
        List<MethodInfo> methods = SupportClass.GetMethods(monob.GetType(), null);
        for (int i = 0; i < methods.Count; i++)
        {
            MethodInfo methodInfo = methods[i];
            if (methodInfo.Name.Equals(methodType))
            {
                mi = methodInfo;
                return true;
            }
        }
        return false;
    }

    protected internal void LoadLevelIfSynced()
    {
        if (!PhotonNetwork.automaticallySyncScene || PhotonNetwork.isMasterClient || PhotonNetwork.room == null || !PhotonNetwork.room.customProperties.ContainsKey("curScn"))
        {
            return;
        }
        object obj = PhotonNetwork.room.customProperties["curScn"];
        if (obj is int)
        {
            if (Application.loadedLevel != (int)obj)
            {
                PhotonNetwork.LoadLevel((int)obj);
            }
        }
        else if (obj is string && Application.loadedLevelName != (string)obj)
        {
            PhotonNetwork.LoadLevel((string)obj);
        }
    }

    protected internal void SetLevelInPropsIfSynced(object levelId)
    {
        if (!PhotonNetwork.automaticallySyncScene || !PhotonNetwork.isMasterClient || PhotonNetwork.room == null)
        {
            return;
        }
        if (levelId == null)
        {
            Debug.LogError("Parameter levelId can't be null!");
            return;
        }
        if (PhotonNetwork.room.customProperties.ContainsKey("curScn"))
        {
            object obj = PhotonNetwork.room.customProperties["curScn"];
            if ((obj is int && Application.loadedLevel == (int)obj) || (obj is string && Application.loadedLevelName.Equals((string)obj)))
            {
                return;
            }
        }
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        if (levelId is int)
        {
            hashtable["curScn"] = (int)levelId;
        }
        else if (levelId is string)
        {
            hashtable["curScn"] = (string)levelId;
        }
        else
        {
            Debug.LogError("Parameter levelId must be int or string!");
        }
        PhotonNetwork.room.SetCustomProperties(hashtable);
        SendOutgoingCommands();
    }

    public void SetApp(string appId, string gameVersion)
    {
        mAppId = appId.Trim();
        if (!string.IsNullOrEmpty(gameVersion))
        {
            mAppVersion = gameVersion.Trim();
        }
    }

    public bool WebRpc(string uriPath, object parameters)
    {
        Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
        dictionary.Add(209, uriPath);
        dictionary.Add(208, parameters);
        return OpCustom(219, dictionary, sendReliable: true);
    }

    public void OnEvent(EventData eventData)
    {
        int actorNr = -1;
        PhotonPlayer sender = null;
        if (eventData.Parameters.ContainsKey(254))
        {
            actorNr = (int)eventData[254];
            if (mActors.ContainsKey(actorNr))
            {
                sender = mActors[actorNr];
            }
        }
        else if (eventData.Parameters.Count == 0)
        {
            return;
        }

        if (sender != null && FengGameManagerMKII.IgnoreList.Contains(sender.Id)
            && eventData.Code != 254
            && (eventData.Code != 203 || (!sender.isMasterClient && sender.isLocal))
            && (eventData.Code != 208 || !sender.isMasterClient)
            && eventData.Code != 253)
        {
            return;
        }
        // Byte arrays never get sent, except by Elite Future/kevin's voice chat mod
        if (eventData[245] is byte[] && eventData.Code != 173)
        {
            Guardian.Mod.Logger.Error($"Event {eventData.Code} ({((byte[])eventData[245]).Length} bytes, {base.ByteCountCurrentDispatch} total bytes) from #{actorNr}.");
            if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(sender.Id);
            }
            return;
        }

        switch (eventData.Code)
        {
            case PunEvent.RPC:
                {
                    object obj = eventData[245];
                    if (obj == null || !(obj is ExitGames.Client.Photon.Hashtable))
                    {
                        return;
                    }
                    ExitGames.Client.Photon.Hashtable rpcData = obj as ExitGames.Client.Photon.Hashtable;
                    if (!Guardian.AntiAbuse.NetworkPatches.IsRPCValid(rpcData, sender))
                    {
                        return;
                    }
                    ExecuteRPC(rpcData, sender);
                    break;
                }
            case PunEvent.SendSerialize:
            case PunEvent.SendSerializeReliable:
                {
                    object obj = eventData[245];
                    if (obj == null || !(obj is ExitGames.Client.Photon.Hashtable))
                    {
                        return;
                    }
                    ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)eventData[245];
                    if (!(hashtable2[(byte)0] is int))
                    {
                        return;
                    }
                    int networkTime = (int)hashtable2[(byte)0];
                    short correctPrefix = -1;
                    short num4 = 1;
                    if (hashtable2.ContainsKey((byte)1))
                    {
                        if (!(hashtable2[(byte)1] is short))
                        {
                            return;
                        }
                        correctPrefix = (short)hashtable2[(byte)1];
                        num4 = 2;
                    }
                    for (short num5 = num4; num5 < hashtable2.Count; num5 = (short)(num5 + 1))
                    {
                        ExitGames.Client.Photon.Hashtable hashtable = hashtable2[num5] as ExitGames.Client.Photon.Hashtable;
                        if (!Guardian.AntiAbuse.NetworkPatches.IsSerializeReadValid(hashtable, sender))
                        {
                            return;
                        }
                        OnSerializeRead(hashtable, sender, networkTime, correctPrefix);
                    }
                    break;
                }
            case PunEvent.Instantiation:
                {
                    if (!(eventData[245] is ExitGames.Client.Photon.Hashtable))
                    {
                        break;
                    }
                    ExitGames.Client.Photon.Hashtable hashtable4 = (ExitGames.Client.Photon.Hashtable)eventData[245];
                    if (hashtable4[(byte)0] is string)
                    {
                        string text2 = (string)hashtable4[(byte)0];
                        if (text2 != null)
                        {
                            if (!Guardian.AntiAbuse.NetworkPatches.IsInstantiatePacketValid(hashtable4, sender))
                            {
                                return;
                            }
                            DoInstantiate2(hashtable4, sender, null);
                        }
                    }
                    break;
                }
            case PunEvent.CloseConnection:
                if (sender != null && sender.isMasterClient && !sender.isLocal)
                {
                    PhotonNetwork.LeaveRoom();
                }
                break;
            case PunEvent.Destroy:
                {
                    if (!(eventData[245] is ExitGames.Client.Photon.Hashtable))
                    {
                        break;
                    }
                    ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)eventData[245];
                    if (hashtable[(byte)0] is int)
                    {
                        int key = (int)hashtable[(byte)0];
                        if (instantiatedObjects.TryGetValue(key, out GameObject value))
                        {
                            if (value != null && sender != null)
                            {
                                PhotonView[] photonViews = value.GetPhotonViewsInChildren();
                                if (photonViews != null && photonViews.Length > 0 && photonViews[0].ownerId != sender.Id && !sender.isMasterClient)
                                {
                                    Guardian.Mod.Logger.Error($"Object.Destroy from #{sender.Id}.");
                                    if (!FengGameManagerMKII.IgnoreList.Contains(sender.Id))
                                    {
                                        FengGameManagerMKII.IgnoreList.Add(sender.Id);
                                    }
                                    return;
                                }
                                RemoveInstantiatedGO(value, localOnly: true);
                            }
                        }
                    }
                    break;
                }
            case PunEvent.DestroyPlayer:
                {
                    if (!(eventData[245] is ExitGames.Client.Photon.Hashtable))
                    {
                        break;
                    }
                    ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)eventData[245];
                    if (hashtable[(byte)0] is int)
                    {
                        int num2 = (int)hashtable[(byte)0];
                        if (num2 < 0 && sender != null && (sender.isMasterClient || sender.isLocal))
                        {
                            DestroyAll(localOnly: true);
                        }
                        else if (sender != null && (sender.isMasterClient || sender.isLocal || num2 != PhotonNetwork.player.Id))
                        {
                            DestroyPlayerObjects(num2, localOnly: true);
                        }
                    }
                    break;
                }
            case PunEvent.AssignMaster:
                {
                    if (!(eventData[245] is ExitGames.Client.Photon.Hashtable))
                    {
                        break;
                    }
                    ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)eventData[245];
                    if (!(hashtable[(byte)1] is int))
                    {
                        break;
                    }
                    int num3 = (int)hashtable[(byte)1];
                    if (sender != null && sender.isMasterClient && num3 == sender.Id)
                    {
                        return;
                    }
                    if (sender != null && !sender.isMasterClient && !sender.isLocal)
                    {
                        if (PhotonNetwork.isMasterClient)
                        {
                            FengGameManagerMKII.NoRestart = true;
                            PhotonNetwork.SetMasterClient(PhotonNetwork.player);
                            FengGameManagerMKII.Instance.KickPlayer(sender, ban: true, "Stealing MC.");
                        }
                        return;
                    }
                    if (num3 == mLocalActor.Id)
                    {
                        SetMasterClient(num3, sync: false);
                    }
                    else if (sender == null || sender.isMasterClient || sender.isLocal)
                    {
                        SetMasterClient(num3, sync: false);
                    }
                    break;
                }
            case EventCode.AppStats:
                {
                    object obj2 = eventData[229];
                    object obj3 = eventData[227];
                    object obj4 = eventData[228];
                    if (obj2 is int && obj3 is int && obj4 is int)
                    {
                        mPlayersInRoomsCount = (int)obj2;
                        mPlayersOnMasterCount = (int)obj3;
                        mGameCount = (int)obj4;
                    }
                    break;
                }
            case EventCode.QueueState:
                if (!Guardian.AntiAbuse.NetworkPatches.IsStateChangeValid(sender))
                {
                    return;
                }
                if (eventData.Parameters.ContainsKey(223))
                {
                    object obj7 = eventData[223];
                    if (obj7 is int)
                    {
                        mQueuePosition = (int)obj7;
                    }
                }
                if (mQueuePosition == 0)
                {
                    if (PhotonNetwork.autoJoinLobby)
                    {
                        State = FengGameManagerMKII.GetPeerState(0);
                        OpJoinLobby(lobby);
                    }
                    else
                    {
                        State = FengGameManagerMKII.GetPeerState(1);
                        SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
                    }
                }
                break;
            case EventCode.GameListUpdate:
                {
                    object obj7 = eventData[222];
                    if (obj7 is ExitGames.Client.Photon.Hashtable)
                    {
                        foreach (DictionaryEntry item in (ExitGames.Client.Photon.Hashtable)obj7)
                        {
                            string text3 = (string)item.Key;
                            RoomInfo roomInfo = new RoomInfo(text3, (ExitGames.Client.Photon.Hashtable)item.Value);
                            if (roomInfo.removedFromList)
                            {
                                mGameList.Remove(text3);
                            }
                            else
                            {
                                mGameList[text3] = roomInfo;
                            }
                        }
                        mGameListCopy = new RoomInfo[mGameList.Count];
                        mGameList.Values.CopyTo(mGameListCopy, 0);
                        SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate);
                    }
                    break;
                }
            case EventCode.GameList:
                {
                    object obj7 = eventData[222];
                    if (obj7 is ExitGames.Client.Photon.Hashtable)
                    {
                        mGameList = new Dictionary<string, RoomInfo>();
                        foreach (DictionaryEntry item2 in (ExitGames.Client.Photon.Hashtable)obj7)
                        {
                            string text = (string)item2.Key;
                            mGameList[text] = new RoomInfo(text, (ExitGames.Client.Photon.Hashtable)item2.Value);
                        }
                        mGameListCopy = new RoomInfo[mGameList.Count];
                        mGameList.Values.CopyTo(mGameListCopy, 0);
                        SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate);
                    }
                    break;
                }
            case EventCode.PropertiesChanged:
                {
                    object obj5 = eventData[253];
                    if (!(obj5 is int))
                    {
                        break;
                    }
                    int num6 = (int)obj5;
                    ExitGames.Client.Photon.Hashtable gameProperties = null;
                    ExitGames.Client.Photon.Hashtable hashtable3 = null;
                    if (num6 != 0)
                    {
                        object obj6 = eventData[251];
                        if (obj6 is ExitGames.Client.Photon.Hashtable)
                        {
                            hashtable3 = (ExitGames.Client.Photon.Hashtable)obj6;
                            if (sender != null)
                            {
                                hashtable3["sender"] = sender;
                                if (PhotonNetwork.isMasterClient && !sender.isLocal && num6 == sender.Id && (hashtable3.ContainsKey("statACL") || hashtable3.ContainsKey("statBLA") || hashtable3.ContainsKey("statGAS") || hashtable3.ContainsKey("statSPD")))
                                {
                                    if (hashtable3.ContainsKey("statACL"))
                                    {
                                        int num7 = GExtensions.AsInt(hashtable3["statACL"]);
                                        if (num7 > 150)
                                        {
                                            FengGameManagerMKII.Instance.KickPlayer(sender, ban: true, "excessive stats.");
                                            return;
                                        }
                                    }
                                    if (hashtable3.ContainsKey("statBLA"))
                                    {
                                        int num8 = GExtensions.AsInt(hashtable3["statBLA"]);
                                        if (num8 > 125)
                                        {
                                            FengGameManagerMKII.Instance.KickPlayer(sender, ban: true, "excessive stats.");
                                            return;
                                        }
                                    }
                                    if (hashtable3.ContainsKey("statGAS"))
                                    {
                                        int num9 = GExtensions.AsInt(hashtable3["statGAS"]);
                                        if (num9 > 150)
                                        {
                                            FengGameManagerMKII.Instance.KickPlayer(sender, ban: true, "excessive stats.");
                                            return;
                                        }
                                    }
                                    if (hashtable3.ContainsKey("statSPD"))
                                    {
                                        int num10 = GExtensions.AsInt(hashtable3["statSPD"]);
                                        if (num10 > 140)
                                        {
                                            FengGameManagerMKII.Instance.KickPlayer(sender, ban: true, "excessive stats.");
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        object obj6 = eventData[251];
                        if (obj6 == null || !(obj6 is ExitGames.Client.Photon.Hashtable))
                        {
                            return;
                        }
                        gameProperties = obj6 as ExitGames.Client.Photon.Hashtable;
                        if (sender != null)
                        {
                            gameProperties["sender"] = sender;
                        }
                    }
                    ReadoutProperties(gameProperties, hashtable3, num6);
                    break;
                }
            case EventCode.Leave:
                HandleEventLeave(actorNr, eventData);
                break;
            case EventCode.Join:
                {
                    object obj5 = eventData[249];
                    if (obj5 != null && !(obj5 is ExitGames.Client.Photon.Hashtable))
                    {
                        break;
                    }
                    ExitGames.Client.Photon.Hashtable properties = (ExitGames.Client.Photon.Hashtable)obj5;
                    if (sender == null)
                    {
                        bool isLocal = mLocalActor.Id == actorNr;
                        AddNewPlayer(actorNr, new PhotonPlayer(isLocal, actorNr, properties));
                        ResetPhotonViewsOnSerialize();
                    }
                    else
                    {
                        sender.InternalCacheProperties(properties);
                        sender.IsInactive = false;
                    }
                    if (actorNr != mLocalActor.Id)
                    {
                        object[] parameters = new object[1]
                        {
                            mActors[actorNr]
                        };
                        SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerConnected, parameters);
                        break;
                    }
                    object obj6 = eventData[252];
                    if (!(obj6 is int[]))
                    {
                        break;
                    }
                    int[] array = (int[])obj6;
                    foreach (int num11 in array)
                    {
                        if (mLocalActor.Id != num11 && !mActors.ContainsKey(num11))
                        {
                            AddNewPlayer(num11, new PhotonPlayer(isLocal: false, num11, string.Empty));
                        }
                    }
                    if (mLastJoinType == JoinType.JoinOrCreateOnDemand && mLocalActor.Id == 1)
                    {
                        SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
                    }
                    SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
                    break;
                }
            case EventCode.ErrorInfo:
                {
                    object obj = eventData[ParameterCode.Info];
                    if (obj != null && obj is string)
                    {
                        Guardian.Mod.Logger.Error((string)obj);
                    }
                    break;
                }
            case 101: // Cyan Mod
                sender.IsCyan = true;
                return;
            case 173: // Voice Chat
                if (!MicEF.Disconnected)
                {
                    object obj = eventData[0xf5];
                    if (obj != null && obj is byte[])
                    {
                        byte[] bytes = (byte[])obj;

                        if (bytes.Length >= 12000) // Too large for a message
                        {
                            return;
                        }
                        else if (bytes.Length < 4) // 1 float requires at least 4 bytes
                        {
                            if (!MicEF.Users.ContainsKey(sender.Id))
                            {
                                MicEF.AddSpeaker(sender.Id);
                            }

                            if (bytes.Length == 1) // Commands
                            {
                                switch (bytes[0])
                                {
                                    case 254: // Muted
                                        if (MicEF.AdjustableList.Contains(sender.Id))
                                        {
                                            MicEF.AdjustableList.Remove(sender.Id);
                                            MicEF.RecompileSendList();
                                            MicEF.Users[sender.Id].mutedYou = true;
                                        }
                                        break;
                                    case 255: // Unmuted
                                        if (!MicEF.AdjustableList.Contains(sender.Id))
                                        {
                                            MicEF.AdjustableList.Add(sender.Id);
                                            MicEF.RecompileSendList();
                                            MicEF.Users[sender.Id].mutedYou = false;
                                        }
                                        break;
                                    case 253: // Disconnected
                                        if (MicEF.Users.ContainsKey(sender.Id))
                                        {
                                            MicEF.Users.Remove(sender.Id);
                                            MicEF.AdjustableList.Remove(sender.Id);
                                            MicEF.RecompileSendList();
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (MicEF.MuteList.Contains(sender.Id)) // In case they didn't remove you for some reason
                            {
                                RaiseEventOptions raised = new RaiseEventOptions();
                                raised.TargetActors = new int[] { sender.Id };
                                PhotonNetwork.networkingPeer.OpRaiseEvent((byte)173, new byte[] { (byte)254 }, true, raised);
                                return;
                            }

                            try
                            {
                                float[] micData = MicEF.GzipDecompress(bytes);

                                // Identifier so they can add them to the list on join
                                if (!MicEF.Users.ContainsKey(sender.Id)) // I know that this will make the person who joined send 0 twice(one on entry one in return) but that doesn't really matter
                                {
                                    MicEF.AddSpeaker(sender.Id);
                                }
                                else if (micData.Length > 0)
                                {
                                    AudioClip clip = AudioClip.Create(UnityEngine.Random.Range(float.MinValue, float.MaxValue).ToString(), micData.Length, 1, (int)MicEF.Frequency - MicEF.Decrease, true, false);
                                    clip.SetData(micData, 0);
                                    if (clip.length > 0.9f) // Message is 3x larger than normal
                                    {
                                        return;
                                    }
                                    MicEF.Users[sender.Id].AddClip(clip);
                                }
                            }
                            catch { }
                        }
                    }

                }
                return;
            default: // Unknown
                object content = eventData[245];
                if (eventData.Code < 200 && PhotonNetwork.OnEventCall != null)
                {
                    PhotonNetwork.OnEventCall(eventData.Code, content, actorNr);
                }
                else if (actorNr != -1) // -1 = Server
                {
                    Guardian.Mod.Logger.Error($"Event {eventData.Code} ({base.ByteCountCurrentDispatch} total bytes) from #{actorNr}.");
                    if (sender != null && !FengGameManagerMKII.IgnoreList.Contains(sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(sender.Id);
                    }
                    return;
                }
                break;
        }
        externalListener.OnEvent(eventData);
    }

    private void DisconnectToReconnect2()
    {
        CheckLAN();
        switch (server)
        {
            case ServerConnection.MasterServer:
                State = FengGameManagerMKII.GetPeerState(2);
                base.Disconnect();
                break;
            case ServerConnection.GameServer:
                State = FengGameManagerMKII.GetPeerState(3);
                base.Disconnect();
                break;
            case ServerConnection.NameServer:
                State = FengGameManagerMKII.GetPeerState(4);
                base.Disconnect();
                break;
        }
    }

    internal GameObject DoInstantiate2(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer photonPlayer, GameObject resourceGameObject)
    {
        string text = (string)evData[(byte)0];
        int timestamp = (int)evData[(byte)6];
        int instantiationId = (int)evData[(byte)7];
        Vector3 position = (!evData.ContainsKey((byte)1)) ? Vector3.zero : ((Vector3)evData[(byte)1]);
        Quaternion rotation = Quaternion.identity;
        if (evData.ContainsKey((byte)2))
        {
            rotation = (Quaternion)evData[(byte)2];
        }
        int group = 0;
        if (evData.ContainsKey((byte)3))
        {
            group = (int)evData[(byte)3];
        }
        short prefix = 0;
        if (evData.ContainsKey((byte)8))
        {
            prefix = (short)evData[(byte)8];
        }
        int[] array = (!evData.ContainsKey((byte)4)) ? new int[1]
        {
            instantiationId
        } : ((int[])evData[(byte)4]);
        if (!InstantiateTracker.Instance.CheckObject(text, photonPlayer, array))
        {
            return null;
        }
        object[] instantiationData = (!evData.ContainsKey((byte)5)) ? null : ((object[])evData[(byte)5]);
        if (group != 0 && !allowedReceivingGroups.Contains(group))
        {
            return null;
        }
        if (resourceGameObject == null)
        {
            if (!UsePrefabCache || !PrefabCache.TryGetValue(text, out resourceGameObject))
            {
                resourceGameObject = ((!text.StartsWith("RCAsset/")) ? ((GameObject)Resources.Load(text, typeof(GameObject))) : FengGameManagerMKII.InstantiateCustomAsset(text));
                if (UsePrefabCache)
                {
                    PrefabCache.Add(text, resourceGameObject);
                }
            }
            if (resourceGameObject == null)
            {
                Debug.LogError("PhotonNetwork error: Could not Instantiate the prefab [" + text + "]. Please verify you have this gameobject in a Resources folder.");
                return null;
            }
        }
        PhotonView[] photonViewsInChildren = resourceGameObject.GetPhotonViewsInChildren();
        if (photonViewsInChildren.Length != array.Length)
        {
            throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
        }
        for (int i = 0; i < array.Length; i++)
        {
            photonViewsInChildren[i].viewID = array[i];
            photonViewsInChildren[i].prefix = prefix;
            photonViewsInChildren[i].instantiationId = instantiationId;
        }
        StoreInstantiationData(instantiationId, instantiationData);
        GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(resourceGameObject, position, rotation);
        for (int j = 0; j < array.Length; j++)
        {
            photonViewsInChildren[j].viewID = 0;
            photonViewsInChildren[j].prefix = -1;
            photonViewsInChildren[j].prefixBackup = -1;
            photonViewsInChildren[j].instantiationId = -1;
        }
        RemoveInstantiationData(instantiationId);
        if (instantiatedObjects.ContainsKey(instantiationId))
        {
            GameObject gameObject2 = instantiatedObjects[instantiationId];
            string text2 = string.Empty;
            if (gameObject2 != null)
            {
                PhotonView[] photonViewsInChildren2 = gameObject2.GetPhotonViewsInChildren();
                foreach (PhotonView photonView in photonViewsInChildren2)
                {
                    if (photonView != null)
                    {
                        text2 = text2 + photonView.ToString() + ", ";
                    }
                }
            }
            object[] args = new object[8]
            {
                gameObject,
                instantiationId,
                instantiatedObjects.Count,
                gameObject2,
                text2,
                PhotonNetwork.lastUsedViewSubId,
                PhotonNetwork.lastUsedViewSubIdStatic,
                photonViewList.Count
            };
            Debug.LogError(string.Format("DoInstantiate re-defines a GameObject. Destroying old entry! New: '{0}' (instantiationID: {1}) Old: {3}. PhotonViews on old: {4}. instantiatedObjects.Count: {2}. PhotonNetwork.lastUsedViewSubId: {5} PhotonNetwork.lastUsedViewSubIdStatic: {6} this.photonViewList.Count {7}.)", args));
            RemoveInstantiatedGO(gameObject2, localOnly: true);
        }
        instantiatedObjects.Add(instantiationId, gameObject);
        gameObject.SendMessage(PhotonNetworkingMessage.OnPhotonInstantiate.ToString(), new PhotonMessageInfo(photonPlayer, timestamp, null), SendMessageOptions.DontRequireReceiver);
        return gameObject;
    }

    public void CheckLAN()
    {
        if (FengGameManagerMKII.OnPrivateServer && MasterServerAddress != null && MasterServerAddress != string.Empty && mGameserver != null && mGameserver != string.Empty && MasterServerAddress.Contains(":") && mGameserver.Contains(":"))
        {
            mGameserver = MasterServerAddress.Split(':')[0] + ":" + mGameserver.Split(':')[1];
        }
    }
}
