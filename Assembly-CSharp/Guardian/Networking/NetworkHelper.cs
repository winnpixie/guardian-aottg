namespace Guardian.Networking
{
    class NetworkHelper
    {
        public static PhotonApplication App = PhotonApplication.AoTTG2;
        public static PhotonConnectionType Connection = PhotonConnectionType.UDP;
        public static bool IsCloud = false;

        public static string GetRegionCode()
        {
            string masterAddress = PhotonNetwork.networkingPeer.MasterServerAddress.ToUpper();

            if (masterAddress.StartsWith("APP-"))
            {
                return masterAddress.Substr(masterAddress.IndexOf('-') + 1, masterAddress.IndexOf('.') - 1);
            }

            return masterAddress switch
            {
                "142.44.242.29" => "NA",
                "135.125.239.180" => "EU",
                "51.79.164.137" => "SG",
                "108.181.69.221" => "SA",
                _ => "LAN"
            };
        }
    }
}
