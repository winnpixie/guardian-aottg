using Guardian.Utilities;
using Guardian.Features.Properties;
using UnityEngine;

namespace Guardian.Features.Gamemodes.Impl
{
    class CageFight : Gamemode
    {
        private Property<int> _groundLevel = new Property<int>("Gamemodes_CageFight:GroundLevel", new string[0], 0);

        // Left Bounds
        private Property<int> _minXLeft = new Property<int>("Gamemodes_CageFight:MinX-Left", new string[0], -400);
        private Property<int> _maxXLeft = new Property<int>("Gamemodes_CageFight:MaxX-Left", new string[0], -50);

        private Property<int> _minZLeft = new Property<int>("Gamemodes_CageFight:MinZ-Left", new string[0], -400);
        private Property<int> _maxZLeft = new Property<int>("Gamemodes_CageFight:MaxZ-Left", new string[0], 400);

        // Right Bounds
        private Property<int> _minXRight = new Property<int>("Gamemodes_CageFight:MinX-Right", new string[0], 50);
        private Property<int> _maxXRight = new Property<int>("Gamemodes_CageFight:MaxX-Right", new string[0], 400);

        private Property<int> _minZRight = new Property<int>("Gamemodes_CageFight:MinZ-Right", new string[0], -400);
        private Property<int> _maxZRight = new Property<int>("Gamemodes_CageFight:MaxZ-Right", new string[0], 400);

        private long roundStartTime;
        private PhotonPlayer playerOne; // Left side
        private PhotonPlayer playerTwo; // Right side
        private bool isRoundOver;

        public CageFight() : base("CageFight", new string[] { "cf", "standoff", "sf" })
        {
            Mod.Properties.Add(_groundLevel);

            // Left
            Mod.Properties.Add(_minXLeft);
            Mod.Properties.Add(_maxXLeft);
            Mod.Properties.Add(_minZLeft);
            Mod.Properties.Add(_maxZLeft);

            // Right
            Mod.Properties.Add(_minXRight);
            Mod.Properties.Add(_maxXRight);
            Mod.Properties.Add(_minZRight);
            Mod.Properties.Add(_maxZRight);
        }

        public override void OnReset()
        {
            isRoundOver = false;
            roundStartTime = -1;

            playerOne = null;
            playerTwo = null;

            if (!FengGameManagerMKII.Level.Name.StartsWith("Custom"))
            {
                InRoomChat.Instance.AddLine("Cage Fight requires either Custom or Custom (No PT) to work!".AsColor("FF0000"));
            }
            else if (PhotonNetwork.room.playerCount < 2)
            {
                InRoomChat.Instance.AddLine("Cage Fight requires two players to work!".AsColor("FF0000"));
            }
            else
            {
                roundStartTime = GameHelper.CurrentTimeMillis();

                GameHelper.Broadcast("Cage Fight! Each kill puts another titan in the opponents ring, whoever dies first loses!");
                GameHelper.Broadcast("Starting in 5 seconds...");
            }
        }

        public override void OnUpdate()
        {
            if (roundStartTime != -1 && GameHelper.CurrentTimeMillis() - roundStartTime >= 5000)
            {
                roundStartTime = -1;

                playerOne = PhotonNetwork.playerList[MathHelper.RandomInt(0, PhotonNetwork.playerList.Length)];

                do
                {
                    playerTwo = PhotonNetwork.playerList[MathHelper.RandomInt(0, PhotonNetwork.playerList.Length)];
                } while (playerOne == playerTwo);

                HERO playerOneHero = GameHelper.GetHero(playerOne);
                HERO playerTwoHero = GameHelper.GetHero(playerTwo);

                if (playerOneHero != null && playerTwoHero != null)
                {
                    float spawnXL = (_minXLeft.Value + _maxXLeft.Value) / 2f;
                    float spawnZL = (_minZLeft.Value + _maxZLeft.Value) / 2f;
                    playerOneHero.photonView.RPC("moveToRPC", playerOne, spawnXL, (float)_groundLevel.Value, spawnZL);

                    float spawnXR = (_minXRight.Value + _maxXRight.Value) / 2f;
                    float spawnZR = (_minZRight.Value + _maxZRight.Value) / 2f;
                    playerTwoHero.photonView.RPC("moveToRPC", playerTwo, spawnXR, (float)_groundLevel.Value, spawnZR);
                }
                else
                {
                    InRoomChat.Instance.AddLine("One or more players are not spawned in! Trying again in 5 seconds...");
                    roundStartTime = GameHelper.CurrentTimeMillis();
                }

                foreach (TITAN titan in FengGameManagerMKII.Instance.Titans)
                {
                    PhotonNetwork.Destroy(titan.gameObject);
                }

                SpawnTitan(0);
                SpawnTitan(1);
            }
        }

        public override void OnPlayerLeave(PhotonPlayer player)
        {
            if (player == playerOne || player == playerTwo)
            {
                isRoundOver = true;

                GameHelper.Broadcast($"{GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).ColorParsed().AsColor("FFFFFF")} forfeit!".AsColor("FF0000"));

                FengGameManagerMKII.Instance.WinGame();
            }
        }

        public override void OnPlayerKilled(HERO hero, int killerId, bool wasKilledByTitan)
        {
            if (!isRoundOver)
            {
                if (hero.photonView.owner == playerOne || hero.photonView.owner == playerTwo)
                {
                    isRoundOver = true;

                    if (hero.photonView.owner == playerOne)
                    {
                        GameHelper.Broadcast($"{GExtensions.AsString(playerTwo.customProperties[PhotonPlayerProperty.Name]).ColorParsed().AsColor("FFFFFF")} wins!".AsColor("AAFF00"));
                    }
                    else if (hero.photonView.owner == playerTwo)
                    {
                        GameHelper.Broadcast($"{GExtensions.AsString(playerOne.customProperties[PhotonPlayerProperty.Name]).ColorParsed().AsColor("FFFFFF")} wins!".AsColor("AAFF00"));
                    }

                    FengGameManagerMKII.Instance.WinGame();
                }
            }
        }

        public override void OnTitanKilled(TITAN titan, PhotonPlayer killer, int damage)
        {
            if (killer == playerOne)
            {
                SpawnTitan(1, titan);
            }
            else if (killer == playerTwo)
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
                    position = new Vector3(MathHelper.RandomInt(_minXLeft.Value, _maxXLeft.Value), _groundLevel.Value, MathHelper.RandomInt(_minZLeft.Value, _maxZLeft.Value));
                    break;
                case 1:
                    position = new Vector3(MathHelper.RandomInt(_minXRight.Value, _maxXRight.Value), _groundLevel.Value, MathHelper.RandomInt(_minZRight.Value, _maxZRight.Value));
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
                go.GetComponent<TITAN>().setAbnormalType2(type, type.Equals(TitanClass.Crawler));
            }
            else
            {
                go.GetComponent<TITAN>().setAbnormalType2(TitanClass.Normal, false);
            }

            return go.GetComponent<TITAN>();
        }
    }
}
