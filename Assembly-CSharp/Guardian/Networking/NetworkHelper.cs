namespace Guardian.Networking
{
    class NetworkHelper
    {
        public static PhotonApplication App = PhotonApplication.AoTTG2;
        public static PhotonConnection Connection = PhotonConnection.UDP;
        public static bool IsCloud = false;

        public static string GetRegionCode()
        {
            string masterAddress = PhotonNetwork.networkingPeer.MasterServerAddress.ToLower();

            if (masterAddress.StartsWith("app-"))
            {
                return masterAddress.Substr(masterAddress.IndexOf('-') + 1, masterAddress.IndexOf('.') - 1);
            }

            // TODO: Add region detection for AoTTG-2 IPs

            return "??";
        }
    }
}
