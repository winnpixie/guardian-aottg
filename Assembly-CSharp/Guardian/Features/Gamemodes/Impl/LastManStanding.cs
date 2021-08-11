using Guardian.Utilities;
using Guardian.Features.Properties;
using System;

namespace Guardian.Features.Gamemodes.Impl
{
    class LastManStanding : Gamemode
    {
        private Property<int> _killInterval = new Property<int>("Gamemodes_LastManStanding:KillInterval", new string[0], 30);
        private long _lastKill;
        private Comparison<PhotonPlayer> _playerSorter = new Comparison<PhotonPlayer>((p1, p2) =>
        {
            return GExtensions.AsInt(p1.customProperties[PhotonPlayerProperty.Kills]) - GExtensions.AsInt(p2.customProperties[PhotonPlayerProperty.Kills]);
        });

        public LastManStanding() : base("LastManStanding", new string[] { "lms" })
        {
            Mod.Properties.Add(_killInterval);
        }

        public override void OnReset()
        {
            _lastKill = GameHelper.CurrentTimeMillis() + _killInterval.Value;
            GameHelper.Broadcast($"Last Man Standing! Whoever has the least number of kills after every {_killInterval.Value} second period will DIE!");
        }

        public override void OnUpdate()
        {
            if (GameHelper.CurrentTimeMillis() - _lastKill >= (_killInterval.Value * 1000))
            {
                _lastKill = GameHelper.CurrentTimeMillis();

                if (FengGameManagerMKII.Instance.heroes.Count > 1)
                {
                    foreach (PhotonPlayer player in PhotonNetwork.playerList.Sorted(_playerSorter))
                    {
                        HERO hero = GameHelper.GetHero(player);
                        if (hero != null)
                        {
                            PhotonNetwork.Instantiate("FX/Thunder", hero.transform.position, hero.transform.rotation, 0);
                            hero.MarkDead();
                            hero.photonView.RPC("netDie2", player, -1, "Lowest Kill Count");

                            GameHelper.Broadcast($"{GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).Colored().WithColor("FFFFFF")} didn't make it!"
                                        .WithColor("FF0000"));
                            break;
                        }
                    }
                }

                if (FengGameManagerMKII.Instance.heroes.Count == 1)
                {
                    HERO winner = FengGameManagerMKII.Instance.heroes[0] as HERO;
                    GameHelper.Broadcast($"{GExtensions.AsString(winner.photonView.owner.customProperties[PhotonPlayerProperty.Name]).Colored().WithColor("FFFFFF")} wins!"
                                        .WithColor("AAFF00"));
                    FengGameManagerMKII.Instance.WinGame();
                }
            }
        }
    }
}
