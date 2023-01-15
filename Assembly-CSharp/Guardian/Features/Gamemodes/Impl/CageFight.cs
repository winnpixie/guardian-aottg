using Guardian.Utilities;
using Guardian.Features.Properties;
using UnityEngine;

namespace Guardian.Features.Gamemodes.Impl
{
    class CageFight : Gamemode
    {
        private Property<int> GroundLevel = new Property<int>("Gamemodes_CageFight:GroundLevel", new string[0], 0);

        // Left Bounds
        private Property<int> LeftMinX = new Property<int>("Gamemodes_CageFight:MinX-Left", new string[0], -400);
        private Property<int> LeftMaxX = new Property<int>("Gamemodes_CageFight:MaxX-Left", new string[0], -50);

        private Property<int> LeftMinZ = new Property<int>("Gamemodes_CageFight:MinZ-Left", new string[0], -400);
        private Property<int> LeftMaxZ = new Property<int>("Gamemodes_CageFight:MaxZ-Left", new string[0], 400);

        // Right Bounds
        private Property<int> RightMinX = new Property<int>("Gamemodes_CageFight:MinX-Right", new string[0], 50);
        private Property<int> RightMaxX = new Property<int>("Gamemodes_CageFight:MaxX-Right", new string[0], 400);

        private Property<int> RightMinZ = new Property<int>("Gamemodes_CageFight:MinZ-Right", new string[0], -400);
        private Property<int> RightMaxZ = new Property<int>("Gamemodes_CageFight:MaxZ-Right", new string[0], 400);

        private long RoundStartTime;
        private PhotonPlayer PlayerOne; // Left side
        private PhotonPlayer PlayerTwo; // Right side
        private bool GameOver;

        public CageFight() : base("CageFight", new string[] { "cf", "standoff", "sf" })
        {
            GuardianClient.Properties.Add(GroundLevel);

            // Left
            GuardianClient.Properties.Add(LeftMinX);
            GuardianClient.Properties.Add(LeftMaxX);
            GuardianClient.Properties.Add(LeftMinZ);
            GuardianClient.Properties.Add(LeftMaxZ);

            // Right
            GuardianClient.Properties.Add(RightMinX);
            GuardianClient.Properties.Add(RightMaxX);
            GuardianClient.Properties.Add(RightMinZ);
            GuardianClient.Properties.Add(RightMaxZ);
        }

        public override void OnReset()
        {
            GameOver = false;
            RoundStartTime = -1;

            PlayerOne = null;
            PlayerTwo = null;

            if (!FengGameManagerMKII.Level.DisplayName.StartsWith("Custom"))
            {
                InRoomChat.Instance.AddLine("Cage Fight requires either Custom or Custom (No PT) to work!".AsColor("FF0000"));
            }
            else if (PhotonNetwork.room.playerCount < 2)
            {
                InRoomChat.Instance.AddLine("Cage Fight requires two players to work!".AsColor("FF0000"));
            }
            else
            {
                RoundStartTime = GameHelper.CurrentTimeMillis();

                GameHelper.Broadcast("Cage Fight! Each kill puts another titan in the opponents ring, whoever dies first loses!");
                GameHelper.Broadcast("Starting in 5 seconds...");
            }
        }

        public override void OnUpdate()
        {
            if (RoundStartTime == -1 || GameHelper.CurrentTimeMillis() - RoundStartTime < 5000) return;

            RoundStartTime = -1;

            PlayerOne = PhotonNetwork.playerList[MathHelper.RandInt(0, PhotonNetwork.playerList.Length)];
            do
            {
                PlayerTwo = PhotonNetwork.playerList[MathHelper.RandInt(0, PhotonNetwork.playerList.Length)];
            } while (PlayerOne == PlayerTwo);

            HERO playerOneHero = PlayerOne.GetHero();
            HERO playerTwoHero = PlayerTwo.GetHero();

            if (playerOneHero != null && playerTwoHero != null)
            {
                float spawnXL = (LeftMinX.Value + LeftMaxX.Value) / 2f;
                float spawnZL = (LeftMinZ.Value + LeftMaxZ.Value) / 2f;
                playerOneHero.photonView.RPC("moveToRPC", PlayerOne, spawnXL, (float)GroundLevel.Value, spawnZL);

                float spawnXR = (RightMinX.Value + RightMaxX.Value) / 2f;
                float spawnZR = (RightMinZ.Value + RightMaxZ.Value) / 2f;
                playerTwoHero.photonView.RPC("moveToRPC", PlayerTwo, spawnXR, (float)GroundLevel.Value, spawnZR);
            }
            else
            {
                InRoomChat.Instance.AddLine("One or more players are not spawned in! Trying again in 5 seconds...");
                RoundStartTime = GameHelper.CurrentTimeMillis();
            }

            foreach (TITAN titan in FengGameManagerMKII.Instance.Titans)
            {
                PhotonNetwork.Destroy(titan.gameObject);
            }

            SpawnTitan(0);
            SpawnTitan(1);
        }

        public override void OnPlayerLeave(PhotonPlayer player)
        {
            if (GameOver || (player != PlayerOne && player != PlayerTwo)) return;
            GameOver = true;

            GameHelper.Broadcast($"{GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity().AsColor("FFFFFF")} forfeit!".AsColor("FF0000"));

            FengGameManagerMKII.Instance.FinishGame();
        }

        public override void OnPlayerKilled(HERO hero, int killerId, bool wasKilledByTitan)
        {
            if (GameOver || (hero.photonView.owner != PlayerOne && hero.photonView.owner != PlayerTwo)) return;

            GameOver = true;

            if (hero.photonView.owner == PlayerOne)
            {
                GameHelper.Broadcast($"{GExtensions.AsString(PlayerTwo.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity().AsColor("FFFFFF")} wins!".AsColor("AAFF00"));
            }
            else if (hero.photonView.owner == PlayerTwo)
            {
                GameHelper.Broadcast($"{GExtensions.AsString(PlayerOne.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity().AsColor("FFFFFF")} wins!".AsColor("AAFF00"));
            }

            FengGameManagerMKII.Instance.FinishGame();
        }

        public override void OnTitanKilled(TITAN titan, PhotonPlayer killer, int damage)
        {
            if (killer == PlayerOne)
            {
                SpawnTitan(1, titan);
            }
            else if (killer == PlayerTwo)
            {
                SpawnTitan(0, titan);
            }
            else
            {
                SpawnTitan(titan.transform.position, titan);
            }
        }

        // 0 = left, 1 = right
        private TITAN SpawnTitan(byte side, TITAN originalTitan = null)
        {
            // -X = left side of map, +X = right side
            Vector3 position = default;

            switch (side)
            {
                case 0:
                    position = new Vector3(MathHelper.RandInt(LeftMinX.Value, LeftMaxX.Value), GroundLevel.Value, MathHelper.RandInt(LeftMinZ.Value, LeftMaxZ.Value));
                    break;
                case 1:
                    position = new Vector3(MathHelper.RandInt(RightMinX.Value, RightMaxX.Value), GroundLevel.Value, MathHelper.RandInt(RightMinZ.Value, RightMaxZ.Value));
                    break;
            }

            return SpawnTitan(position, originalTitan);
        }

        private TITAN SpawnTitan(Vector3 position, TITAN originalTitan = null)
        {
            GameObject go = FengGameManagerMKII.Instance.SpawnTitanRaw(position, Quaternion.identity);

            if (originalTitan != null)
            {
                TitanClass type = originalTitan.abnormalType;
                go.GetComponent<TITAN>().SetAbnormalType2(type, type.Equals(TitanClass.Crawler));
            }
            else
            {
                go.GetComponent<TITAN>().SetAbnormalType2(TitanClass.Normal, false);
            }

            return go.GetComponent<TITAN>();
        }
    }
}
