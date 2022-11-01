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
                return masterAddress.Substr(masterAddress.IndexOf('-') + 1, masterAddress.IndexOf('.') - 1).ToUpper();
            }

            return masterAddress switch
            {
                "142.44.242.29" => "NA",
                "172.107.193.233" => "SA",
                "135.125.239.180" => "EU",
                "51.79.164.137" => "SG",
                _ => "??"
            };
        }
    }
}
