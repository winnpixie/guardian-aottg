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
            string regionAddress = PhotonNetwork.networkingPeer.MasterServerAddress.ToUpper();

            if (regionAddress.StartsWith("APP-") || regionAddress.StartsWith("MP-"))
            {
                int hyphenIdx = regionAddress.IndexOf('-');
                int dotIdx = regionAddress.IndexOf('.');
                return regionAddress.Substring(hyphenIdx, dotIdx - hyphenIdx - 1);
            }

            return "??";
        }
    }
}