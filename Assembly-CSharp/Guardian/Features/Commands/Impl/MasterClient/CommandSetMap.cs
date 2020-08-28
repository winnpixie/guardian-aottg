using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandSetMap : Command
    {
        public CommandSetMap() : base("setmap", new string[] { "map" }, "<mapName>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                LevelInfo levelInfo = LevelInfo.getInfo(string.Join(" ", args));
                if (levelInfo != null)
                {
                    FengGameManagerMKII.Instance.photonView.RPC("SetCurrentMap", PhotonTargets.All, levelInfo.name);
                    FengGameManagerMKII.Instance.RestartGame();
                    GameHelper.Broadcast($"The current map in play is now {levelInfo.name}!");
                }
            }
        }
    }
}
