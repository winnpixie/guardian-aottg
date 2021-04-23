using ExitGames.Client.Photon;

namespace Guardian.Networking
{
    class PhotonConnection
    {
        public static PhotonConnection TCP = new PhotonConnection("TCP", 4530, ConnectionProtocol.Tcp);
        public static PhotonConnection UDP = new PhotonConnection("UDP", 5055, ConnectionProtocol.Udp);

        public string Name;
        public int Port;
        public ConnectionProtocol Protocol;

        public PhotonConnection(string name, int port, ConnectionProtocol protocol)
        {
            this.Name = name;
            this.Port = port;
            this.Protocol = protocol;
        }
    }
}
