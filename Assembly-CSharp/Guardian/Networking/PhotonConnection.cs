using ExitGames.Client.Photon;

namespace Guardian.Networking
{
    class PhotonConnection
    {
        public string Name;
        public int Port;
        public ConnectionProtocol Protocol;

        public PhotonConnection(string name, int port, ConnectionProtocol protocol)
        {
            this.Name = name;
            this.Port = port;
            this.Protocol = protocol;
        }

        public class TCP : PhotonConnection
        {
            public TCP() : base("TCP", 4530, ConnectionProtocol.Tcp) { }
        }

        public class UDP : PhotonConnection
        {
            public UDP() : base("UDP", 5055, ConnectionProtocol.Udp) { }
        }

        public class RHttp : PhotonConnection
        {
            public RHttp() : base("RHttp", 6063, ConnectionProtocol.RHttp) { }
        }
    }
}
