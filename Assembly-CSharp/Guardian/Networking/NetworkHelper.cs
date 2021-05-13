using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Guardian.Networking
{
    class NetworkHelper
    {
        public static PhotonApplication App = PhotonApplication.Custom;
        public static PhotonConnection Connection = PhotonConnection.TCP;

        public static string GetMasterAddress(CloudRegionCode code)
        {
            switch (code)
            {
                case CloudRegionCode.us:
                    return "app-us.exitgamescloud.com";
                case CloudRegionCode.eu:
                    return "app-eu.exitgamescloud.com";
                case CloudRegionCode.jp:
                    return "app-jp.exitgamescloud.com";
                case CloudRegionCode.asia:
                    return "app-asia.exitgamescloud.com";
            }

            return string.Empty;
        }

        public static long GetResponseTime(string server, int port)
        {
            IPHostEntry entry = Dns.GetHostEntry(server);
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
            string[] regions = new string[] { "us", "eu", "asia", "jp" };
            string bestRegion = string.Empty;
            long lowestPing = long.MaxValue;

            foreach (string code in regions)
            {
                string address = $"app-{code}.exitgamescloud.com";
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
