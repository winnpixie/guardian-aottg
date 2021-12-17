using Guardian.Utilities;
using Guardian.Features.Properties;

namespace Guardian.Features.Gamemodes.Impl
{
    class LastManStanding : Gamemode
    {
        private Property<int> KillInterval = new Property<int>("Gamemodes_LastManStanding:KillInterval", new string[0], 30);
        private long NextKillTime;
        private long LastPollTime;

        public LastManStanding() : base("LastManStanding", new string[] { "lms" })
        {
            Mod.Properties.Add(KillInterval);
        }

        public override void OnReset()
        {
            NextKillTime = GameHelper.CurrentTimeMillis() + (KillInterval.Value * 1000);
            GameHelper.Broadcast($"Last Man Standing! Whoever has the least number of kills after every {KillInterval.Value} second period will DIE!");

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
            if (GameHelper.CurrentTimeMillis() - LastPollTime < 1000) return;

            LastPollTime = GameHelper.CurrentTimeMillis();

            if (GameHelper.CurrentTimeMillis() >= NextKillTime)
            {
                NextKillTime = GameHelper.CurrentTimeMillis() + (KillInterval.Value * 1000);

                if (FengGameManagerMKII.Instance.Heroes.Count > 1)
                {
                    int playersAlive = 0;

                    int highestKills = int.MinValue;
                    HERO bestPlayer = null;

                    int lowestKills = int.MaxValue;
                    HERO worstPlayer = null;

                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        HERO hero = player.GetHero();
                        if (hero == null) continue;

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

                    if (worstPlayer != null && playersAlive > 1)
                    {
                        PhotonNetwork.Instantiate("FX/Thunder", worstPlayer.transform.position, worstPlayer.transform.rotation, 0);
                        worstPlayer.MarkDead();
                        worstPlayer.photonView.RPC("netDie2", worstPlayer.photonView.owner, -1, "Lowest Kill Count");

                        GameHelper.Broadcast($"{GExtensions.AsString(worstPlayer.photonView.owner.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity().AsColor("FFFFFF")} didn't make it!".AsColor("FF0000"));
                    }

                    if (playersAlive < 3 && bestPlayer != null)
                    {
                        GameHelper.Broadcast($"{GExtensions.AsString(bestPlayer.photonView.owner.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity().AsColor("FFFFFF")} wins!".AsColor("AAFF00"));
                        FengGameManagerMKII.Instance.FinishGame();

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

                GameHelper.Broadcast($"A new {KillInterval.Value} second period has begun!");
            }
            else if (NextKillTime - GameHelper.CurrentTimeMillis() > 5000) return;

            int timeLeft = MathHelper.Floor((NextKillTime - GameHelper.CurrentTimeMillis()) / 1000f) + 1;
            GameHelper.Broadcast($"{timeLeft}...".AsColor("FF0000"));
        }

        public override void OnPlayerJoin(PhotonPlayer player)
        {
            FengGameManagerMKII.Instance.photonView.RPC("Chat", player,
                $"Last Man Standing! Whoever has the least number of kills after every {KillInterval.Value} second period will DIE!", string.Empty);
        }
    }
}
