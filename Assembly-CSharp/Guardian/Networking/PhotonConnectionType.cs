using ExitGames.Client.Photon;

namespace Guardian.Networking
{
    class PhotonConnectionType
    {
        public static PhotonConnectionType TCP = new PhotonConnectionType("TCP", 4530, ConnectionProtocol.Tcp);
        public static PhotonConnectionType UDP = new PhotonConnectionType("UDP", 5055, ConnectionProtocol.Udp);
        public static PhotonConnectionType RHttp = new PhotonConnectionType("RHttp", 6063, ConnectionProtocol.RHttp);

        public string Name;
        public int Port;
        public ConnectionProtocol Protocol;

        public PhotonConnectionType(string name, int port, ConnectionProtocol protocol)
        {
            this.Name = name;
            this.Port = port;
            this.Protocol = protocol;
        }
    }
}
