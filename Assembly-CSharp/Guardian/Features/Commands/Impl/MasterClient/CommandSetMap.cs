using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandSetMap : Command
    {
        public CommandSetMap() : base("setmap", new string[] { "map" }, "<name>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                LevelInfo levelInfo = LevelInfo.GetInfo(string.Join(" ", args));
                if (levelInfo != null)
                {
                    PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                    {
                        { "Map", levelInfo.Name }
                    });
                    FengGameManagerMKII.Instance.RestartGame();

                    GameHelper.Broadcast($"The map in play is now {levelInfo.Name}!");
                }
            }
        }
    }
}
