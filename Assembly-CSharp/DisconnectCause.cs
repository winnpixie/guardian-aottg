using System;

public enum DisconnectCause
{
    ExceptionOnConnect = 0x3FF,
    SecurityExceptionOnConnect = 1022,
    [Obsolete("Replaced by clearer: DisconnectByClientTimeout")]
    TimeoutDisconnect = 1040,
    DisconnectByClientTimeout = 1040,
    InternalReceiveException = 1039,
    [Obsolete("Replaced by clearer: DisconnectByServerTimeout")]
    DisconnectByServer = 1041,
    DisconnectByServerTimeout = 1041,
    DisconnectByServerLogic = 1043,
    DisconnectByServerUserLimit = 1042,
    Exception = 1026,
    InvalidRegion = 32756,
    MaxCcuReached = 32757,
    InvalidAuthentication = 0x7FFF,
    AuthenticationTicketExpired = 32753
}
