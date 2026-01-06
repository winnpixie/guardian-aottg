using ExitGames.Client.Photon;

namespace Guardian.Networking
{
    public class PhotonConnectionType
    {
        public static readonly PhotonConnectionType TCP =
            new PhotonConnectionType("TCP", 4530, ConnectionProtocol.Tcp, 0);

        public static readonly PhotonConnectionType UDP =
            new PhotonConnectionType("UDP", 5055, ConnectionProtocol.Udp, 1);

        public static readonly PhotonConnectionType RHttp =
            new PhotonConnectionType("RHttp [DEAD]", 6063, ConnectionProtocol.RHttp, 2);

        public readonly string Name;
        public readonly int Port;
        public readonly ConnectionProtocol Protocol;

        private readonly int _index;

        public PhotonConnectionType(string name, int port, ConnectionProtocol protocol, int index)
        {
            this.Name = name;
            this.Port = port;
            this.Protocol = protocol;
            this._index = index;
        }

        public static PhotonConnectionType GetNext(PhotonConnectionType connection)
        {
            return connection._index switch
            {
                0 => UDP,
                1 => RHttp,
                2 => TCP,
                _ => UDP
            };
        }
    }
}