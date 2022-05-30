using Guardian.Utilities;
using Guardian.Features.Properties;
using System.Collections.Generic;

namespace Guardian.Features.Gamemodes.Impl
{
    class TimeBomb : Gamemode
    {
        private Dictionary<int, int> LifeTimes;
        private Property<int> StartTime = new Property<int>("Gamemodes_TimeBomb:StartTime", new string[0], 90);
        private Property<float> ScoreMultiplier = new Property<float>("Gamemodes_TimeBomb:ScoreMultiplier", new string[0], 1f);
        private long LastPollTime;

        public TimeBomb() : base("TimeBomb", new string[] { "tb" })
        {
            GuardianClient.Properties.Add(StartTime);
            GuardianClient.Properties.Add(ScoreMultiplier);
        }

        public override void CleanUp()
        {
            LifeTimes.Clear();

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
            LifeTimes = new Dictionary<int, int>();
            LastPollTime = GameHelper.CurrentTimeMillis() + 1000;

            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                LifeTimes.Add(player.Id, StartTime.Value);
            }

            GameHelper.Broadcast("Tick-Tock! Time-Bomb mode is enabled, kill titans to stay alive!");
            GameHelper.Broadcast($"Everyone has been given a {StartTime.Value} second starting time.");
        }

        public override void OnUpdate()
        {
            if (GameHelper.CurrentTimeMillis() - LastPollTime < 1000) return;

            Dictionary<int, int> newTimes = new Dictionary<int, int>(LifeTimes);
            LastPollTime = GameHelper.CurrentTimeMillis();

            foreach (KeyValuePair<int, int> entry in LifeTimes)
            {
                PhotonPlayer player = PhotonPlayer.Find(entry.Key);
                if (player == null) continue;

                HERO hero = player.GetHero();
                if (hero == null) continue;

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
                    hero.photonView.RPC("netDie2", player, -1, "[FF4444]Time's Up!");

                    GameHelper.Broadcast($"{GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity().AsColor("FFFFFF")} ran out of time!"
                        .AsColor("FF0000"));

                    LifeTimes[hero.photonView.owner.Id] = StartTime.Value;
                }
                else if (timeLeft == 15)
                {
                    FengGameManagerMKII.Instance.photonView.RPC("Chat", player, $"15 seconds left...".AsColor("FF0000"), string.Empty);
                }
                else if (timeLeft < 6)
                {
                    FengGameManagerMKII.Instance.photonView.RPC("Chat", player, $"{timeLeft}...".AsColor("FF0000"), string.Empty);
                }

                newTimes[entry.Key] = timeLeft;
            }

            LifeTimes = newTimes;
        }

        public override void OnPlayerJoin(PhotonPlayer player)
        {
            FengGameManagerMKII.Instance.photonView.RPC("Chat", player, "Tick-Tock! Time-Bomb mode is enabled, kill titans to stay alive!", string.Empty);
            LifeTimes.Add(player.Id, StartTime.Value);
        }

        public override void OnPlayerLeave(PhotonPlayer player)
        {
            LifeTimes.Remove(player.Id);
        }

        public override void OnPlayerKilled(HERO hero, int killerId, bool wasKilledByTitan)
        {
            LifeTimes[hero.photonView.owner.Id] = StartTime.Value;
        }

        public override void OnTitanKilled(TITAN titan, PhotonPlayer killer, int damage)
        {
            int timeBonus = MathHelper.Floor((damage / 100f) * ScoreMultiplier.Value);
            LifeTimes[killer.Id] += timeBonus;

            FengGameManagerMKII.Instance.photonView.RPC("Chat", killer, ("+" + timeBonus + "second(s)!").AsColor("00FF00"), string.Empty);
        }
    }
}
