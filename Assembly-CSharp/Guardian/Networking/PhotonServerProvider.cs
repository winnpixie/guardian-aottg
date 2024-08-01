using System.Collections.Generic;

namespace Guardian.Networking
{
    class PhotonServerProvider
    {
        public static readonly PhotonServerProvider ExitGames = new PhotonServerProvider("ExitGames [NS]", new Dictionary<CloudRegionCode, string>() {
            { CloudRegionCode.us, "app-us.exitgames.com" },
            { CloudRegionCode.eu, "app-eu.exitgames.com" },
            { CloudRegionCode.asia, "app-asia.exitgames.com" },
            { CloudRegionCode.jp, "app-jp.exitgames.com" },
            { CloudRegionCode.sa, "app-sa.exitgames.com" },
        }, true, 0);

        public static readonly PhotonServerProvider ExitGamesCloud = new PhotonServerProvider("ExitGames Cloud", new Dictionary<CloudRegionCode, string>() {
            { CloudRegionCode.us, "app-us.exitgamescloud.com" },
            { CloudRegionCode.eu, "app-eu.exitgamescloud.com" },
            { CloudRegionCode.asia, "app-asia.exitgamescloud.com" },
            { CloudRegionCode.jp, "app-jp.exitgamescloud.com" },
            { CloudRegionCode.sa, "app-sa.exitgamescloud.com" },
        }, true, 1);

        public static readonly PhotonServerProvider AoTTG2 = new PhotonServerProvider("AoTTG-2", new Dictionary<CloudRegionCode, string>() {
            { CloudRegionCode.us, "142.44.242.29" },
            { CloudRegionCode.eu, "135.125.239.180" } ,
            { CloudRegionCode.asia, "51.79.164.137" },
            { CloudRegionCode.jp, "app-jp.exitgamescloud.com" },
            { CloudRegionCode.sa, "108.181.69.221" },
        }, false, 2);

        public readonly string Name;
        public readonly Dictionary<CloudRegionCode, string> Regions;
        public readonly bool IsCloud;

        private readonly int Index;

        public PhotonServerProvider(string name, Dictionary<CloudRegionCode, string> regions, bool isCloud, int index)
        {
            this.Name = name;
            this.Regions = regions;
            this.IsCloud = isCloud;
            this.Index = index;
        }

        public static PhotonServerProvider GetNext(PhotonServerProvider provider)
        {
            return provider.Index switch
            {
                0 => ExitGamesCloud,
                1 => AoTTG2,
                2 => ExitGames,
                _ => ExitGames
            };
        }
    }
}
