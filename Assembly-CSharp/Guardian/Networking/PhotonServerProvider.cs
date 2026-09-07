using System.Collections.Generic;

namespace Guardian.Networking
{
    public class PhotonServerProvider
    {
        public static readonly PhotonServerProvider ExitGames = new PhotonServerProvider("ExitGames [NS]",
            new Dictionary<CloudRegionCode, string>()
            {
                { CloudRegionCode.us, "app-us.exitgames.com" },
                { CloudRegionCode.eu, "app-eu.exitgames.com" },
                { CloudRegionCode.asia, "app-asia.exitgames.com" },
                { CloudRegionCode.jp, "app-jp.exitgames.com" },
                { CloudRegionCode.sa, "app-sa.exitgames.com" },
            }, true, 0);

        public static readonly PhotonServerProvider ExitGamesCloud = new PhotonServerProvider("ExitGames Cloud",
            new Dictionary<CloudRegionCode, string>()
            {
                { CloudRegionCode.us, "app-us.exitgamescloud.com" },
                { CloudRegionCode.eu, "app-eu.exitgamescloud.com" },
                { CloudRegionCode.asia, "app-asia.exitgamescloud.com" },
                { CloudRegionCode.jp, "app-jp.exitgamescloud.com" },
                { CloudRegionCode.sa, "app-sa.exitgamescloud.com" },
            }, true, 1);

        public static readonly PhotonServerProvider AoTTG2 = new PhotonServerProvider("AoTTG-2",
            new Dictionary<CloudRegionCode, string>()
            {
                { CloudRegionCode.us, "us.aottg2.com" },
                { CloudRegionCode.eu, "eu.aottg2.com" },
                { CloudRegionCode.asia, "cn.aottg2.com" },
                { CloudRegionCode.jp, "asia.aottg2.com" },
                { CloudRegionCode.sa, "sa.aottg2.com" },
            }, false, 2);

        public readonly string Name;
        public readonly Dictionary<CloudRegionCode, string> Regions;
        public readonly bool IsCloud;

        private readonly int _index;

        public PhotonServerProvider(string name, Dictionary<CloudRegionCode, string> regions, bool isCloud, int index)
        {
            this.Name = name;
            this.Regions = regions;
            this.IsCloud = isCloud;
            this._index = index;
        }

        public static PhotonServerProvider GetNext(PhotonServerProvider provider)
        {
            return provider._index switch
            {
                0 => ExitGamesCloud,
                1 => AoTTG2,
                2 => ExitGames,
                _ => ExitGames
            };
        }
    }
}