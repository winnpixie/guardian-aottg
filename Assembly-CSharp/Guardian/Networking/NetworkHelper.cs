namespace Guardian.Networking
{
    public static class NetworkHelper
    {
        public static PhotonServerProvider Provider = PhotonServerProvider.AoTTG2;
        public static PhotonApplication App = PhotonApplication.AoTTG2;
        public static PhotonConnectionType Connection = PhotonConnectionType.UDP;
        public static bool IsCloud = false;

        public static bool ConnectToRegion(CloudRegionCode regionCode)
        {
            IsCloud = Provider.IsCloud;
            FengGameManagerMKII.OnPrivateServer = false;

            if (Provider == PhotonServerProvider.ExitGames)
            {
                PhotonNetwork.networkingPeer.SetApp(App.Id, UIMainReferences.Version);
                return PhotonNetwork.networkingPeer.ConnectToRegionMaster(regionCode);
            }

            return PhotonNetwork.ConnectToMaster(Provider.Regions[regionCode], Connection.Port, App.Id,
                UIMainReferences.Version);
        }

        public static string GetRegionCode()
        {
            string masterAddress = PhotonNetwork.networkingPeer.MasterServerAddress.ToUpper();

            if (masterAddress.StartsWith("APP-") || masterAddress.StartsWith("MP-"))
            {
                return masterAddress.Substr(masterAddress.IndexOf('-') + 1, masterAddress.IndexOf('.') - 1);
            }

            return "??";
        }
    }
}