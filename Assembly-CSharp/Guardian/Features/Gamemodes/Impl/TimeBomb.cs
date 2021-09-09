using Guardian.Utilities;
using Guardian.Features.Properties;
using System.Collections.Generic;

namespace Guardian.Features.Gamemodes.Impl
{
    class TimeBomb : Gamemode
    {
        private Dictionary<int, int> _lifeTimes;
        private Property<int> _startTime = new Property<int>("Gamemodes_TimeBomb:StartTime", new string[0], 90);
        private Property<float> _scoreMultiplier = new Property<float>("Gamemodes_TimeBomb:ScoreMultiplier", new string[0], 1f);
        private long _lastUpdate;

        public TimeBomb() : base("TimeBomb", new string[] { "tb" })
        {
            Mod.Properties.Add(_startTime);
            Mod.Properties.Add(_scoreMultiplier);
        }

        public override void CleanUp()
        {
            _lifeTimes.Clear();

            if (PhotonNetwork.inRoom)
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

        public override void OnReset()
        {
            _lifeTimes = new Dictionary<int, int>();
            _lastUpdate = GameHelper.CurrentTimeMillis() + 1000;

            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                _lifeTimes.Add(player.Id, _startTime.Value);
            }

            GameHelper.Broadcast("Tick-Tock! Time-Bomb mode is enabled, kill titans to stay alive!");
            GameHelper.Broadcast($"Everyone has been given a {_startTime.Value} second starting time.");
        }

        public override void OnUpdate()
        {
            if (GameHelper.CurrentTimeMillis() - _lastUpdate >= 1000)
            {
                Dictionary<int, int> newTimes = new Dictionary<int, int>(_lifeTimes);
                _lastUpdate = GameHelper.CurrentTimeMillis();

                foreach (KeyValuePair<int, int> entry in _lifeTimes)
                {
                    PhotonPlayer player = PhotonPlayer.Find(entry.Key);
                    if (player != null)
                    {
                        HERO hero = GameHelper.GetHero(player);

                        if (hero != null)
                        {
                            int timeLeft = entry.Value;
                            if (entry.Value >= 0)
                            {
                                timeLeft--;

                                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                                {
                                    { PhotonPlayerProperty.TotalDamage, timeLeft }
                                });
                            }

                            if (timeLeft <= 0)
                            {
                                PhotonNetwork.Instantiate("FX/Thunder", hero.transform.position, hero.transform.rotation, 0);
                                hero.MarkDead();
                                hero.photonView.RPC("netDie2", player, -1, "[FF0000]Time's Up!");

                                GameHelper.Broadcast($"{GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).Colored().WithColor("FFFFFF")} ran out of time!"
                                    .WithColor("FF0000"));

                                _lifeTimes[hero.photonView.owner.Id] = _startTime.Value;
                            }
                            else if (timeLeft == 15)
                            {
                                FengGameManagerMKII.Instance.photonView.RPC("Chat", player, $"15 seconds left...".WithColor("FF0000"), string.Empty);
                            }
                            else if (timeLeft < 6)
                            {
                                FengGameManagerMKII.Instance.photonView.RPC("Chat", player, $"{timeLeft}...".WithColor("FF0000"), string.Empty);
                            }

                            newTimes[entry.Key] = timeLeft;
                        }
                    }
                }

                _lifeTimes = newTimes;
            }
        }

        public override void OnPlayerJoin(PhotonPlayer player)
        {
            FengGameManagerMKII.Instance.photonView.RPC("Chat", player, "Tick-Tock! Time-Bomb mode is enabled, kill titans to stay alive!", string.Empty);
            _lifeTimes.Add(player.Id, _startTime.Value);
        }

        public override void OnPlayerLeave(PhotonPlayer player)
        {
            _lifeTimes.Remove(player.Id);
        }

        public override void OnPlayerKilled(HERO hero, int killerId, bool wasKilledByTitan)
        {
            _lifeTimes[hero.photonView.owner.Id] = _startTime.Value;
        }

        public override void OnTitanKilled(TITAN titan, PhotonPlayer killer, int damage)
        {
            int timeBonus = MathHelper.Floor((damage / 100f) * _scoreMultiplier.Value);
            _lifeTimes[killer.Id] += timeBonus;

            FengGameManagerMKII.Instance.photonView.RPC("Chat", killer, ("+" + timeBonus + (timeBonus == 1 ? " second" : " seconds") + "!").WithColor("00FF00"), string.Empty);
        }
    }
}
