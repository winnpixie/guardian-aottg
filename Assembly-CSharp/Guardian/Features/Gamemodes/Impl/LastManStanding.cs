using Guardian.Utilities;
using Guardian.Features.Properties;

namespace Guardian.Features.Gamemodes.Impl
{
    class LastManStanding : Gamemode
    {
        private Property<int> _killInterval = new Property<int>("Gamemodes_LastManStanding:KillInterval", new string[0], 30);
        private long _nextKill;
        private long _lastUpdate;

        public LastManStanding() : base("LastManStanding", new string[] { "lms" })
        {
            Mod.Properties.Add(_killInterval);
        }

        public override void OnReset()
        {
            _nextKill = GameHelper.CurrentTimeMillis() + (_killInterval.Value * 1000);
            GameHelper.Broadcast($"Last Man Standing! Whoever has the least number of kills after every {_killInterval.Value} second period will DIE!");

            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.Kills, 0 },
                { PhotonPlayerProperty.Deaths, 0 },
                { PhotonPlayerProperty.MaxDamage, 0 },
                { PhotonPlayerProperty.TotalDamage, 0 },
            };

            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                player.SetCustomProperties(props);
            }
        }

        public override void OnUpdate()
        {
            if (GameHelper.CurrentTimeMillis() - _lastUpdate >= 1000)
            {
                _lastUpdate = GameHelper.CurrentTimeMillis();

                if (GameHelper.CurrentTimeMillis() >= _nextKill)
                {
                    _nextKill = GameHelper.CurrentTimeMillis() + (_killInterval.Value * 1000);

                    if (FengGameManagerMKII.Instance.heroes.Count > 1)
                    {
                        int playersAlive = 0;

                        int highestKills = int.MinValue;
                        HERO bestPlayer = null;

                        int lowestKills = int.MaxValue;
                        HERO worstPlayer = null;

                        foreach (PhotonPlayer player in PhotonNetwork.playerList)
                        {
                            HERO hero = GameHelper.GetHero(player);
                            if (hero != null)
                            {
                                playersAlive++;

                                int kills = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Kills]);

                                if (kills < lowestKills)
                                {
                                    lowestKills = kills;
                                    worstPlayer = hero;
                                }

                                if (kills > highestKills)
                                {
                                    highestKills = kills;
                                    bestPlayer = hero;
                                }
                            }
                        }

                        if (worstPlayer != null && playersAlive > 1)
                        {
                            PhotonNetwork.Instantiate("FX/Thunder", worstPlayer.transform.position, worstPlayer.transform.rotation, 0);
                            worstPlayer.MarkDead();
                            worstPlayer.photonView.RPC("netDie2", worstPlayer.photonView.owner, -1, "Lowest Kill Count");

                            GameHelper.Broadcast($"{GExtensions.AsString(worstPlayer.photonView.owner.customProperties[PhotonPlayerProperty.Name]).ColorParsed().AsColor("FFFFFF")} didn't make it!"
                                        .AsColor("FF0000"));
                        }

                        if (playersAlive < 3 && bestPlayer != null)
                        {
                            GameHelper.Broadcast($"{GExtensions.AsString(bestPlayer.photonView.owner.customProperties[PhotonPlayerProperty.Name]).ColorParsed().AsColor("FFFFFF")} wins!"
                                                .AsColor("AAFF00"));
                            FengGameManagerMKII.Instance.WinGame();

                            return;
                        }
                        else
                        {
                            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
                            {
                                { PhotonPlayerProperty.Kills, 0 },
                                { PhotonPlayerProperty.Deaths, 0 },
                                { PhotonPlayerProperty.MaxDamage, 0 },
                                { PhotonPlayerProperty.TotalDamage, 0 },
                            };

                            foreach (PhotonPlayer player in PhotonNetwork.playerList)
                            {
                                player.SetCustomProperties(props);
                            }
                        }
                    }

                    GameHelper.Broadcast($"A new {_killInterval.Value} second period has begun!");
                }
                else if (_nextKill - GameHelper.CurrentTimeMillis() <= 5000)
                {
                    int timeLeft = MathHelper.Floor((_nextKill - GameHelper.CurrentTimeMillis()) / 1000f) + 1;

                    GameHelper.Broadcast($"{timeLeft}...".AsColor("FF0000"));
                }
            }
        }

        public override void OnPlayerJoin(PhotonPlayer player)
        {
            FengGameManagerMKII.Instance.photonView.RPC("Chat", player,
                $"Last Man Standing! Whoever has the least number of kills after every {_killInterval.Value} second period will DIE!", string.Empty);
        }
    }
}
