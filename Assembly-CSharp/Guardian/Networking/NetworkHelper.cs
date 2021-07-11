using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Guardian.Networking
{
    class NetworkHelper
    {
        public static PhotonApplication App = PhotonApplication.AoTTG2;
        public static PhotonConnection Connection = PhotonConnection.UDP;
        public static bool IsCloud = false;

        public static long GetResponseTime(string addr, int port)
        {
            IPHostEntry entry = Dns.GetHostEntry(addr);
            Stopwatch sw = new Stopwatch();

            foreach (IPAddress address in entry.AddressList)
            {
                sw.Start();

                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(ipe);

                if (socket.Connected)
                {
                    return sw.ElapsedMilliseconds;
                }

                sw.Reset();
            }

            return -1;
        }

        public static string GetBestRegion()
        {
            string[] regions = new string[] { "us", "eu", "asia", "jp", "sa" };
            string bestRegion = string.Empty;
            long lowestPing = long.MaxValue;

            foreach (string code in regions)
            {
                string address = $"app-{code}.exitgames.com";
                long ping = GetResponseTime(address, 4530);

                if (ping != -1 && ping < lowestPing)
                {
                    lowestPing = ping;
                    bestRegion = address;
                }
            }

            return bestRegion;
        }

        public static string GetRegionCode()
        {
            string masterAddress = PhotonNetwork.networkingPeer.MasterServerAddress.ToLower();

            if (masterAddress.StartsWith("app-"))
            {
                return masterAddress.Substr(masterAddress.IndexOf('-') + 1, masterAddress.IndexOf('.') - 1);
            }

            return "unknown";
        }
    }
}
