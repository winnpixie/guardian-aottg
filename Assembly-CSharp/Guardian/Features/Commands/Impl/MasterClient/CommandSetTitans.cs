using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandSetTitans : Command
    {
        public CommandSetTitans() : base("settitans", new string[0], "<normal/aberrant/jumper/crawler/punk>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                TitanClass? type = null;

                switch (args[0].ToLower())
                {
                    case "normal":
                        type = TitanClass.Normal;
                        GameHelper.Broadcast("All non-player titans are now of type NORMAL!");
                        break;
                    case "aberrant":
                        type = TitanClass.Aberrant;
                        GameHelper.Broadcast("All non-player titans are now of type ABERRANT!");
                        break;
                    case "jumper":
                        type = TitanClass.Jumper;
                        GameHelper.Broadcast("All non-player titans are now of type JUMPER!");
                        break;
                    case "crawler":
                        type = TitanClass.Crawler;
                        GameHelper.Broadcast("All non-player titans are now of type CRAWLER!");
                        break;
                    case "punk":
                        type = TitanClass.Punk;
                        GameHelper.Broadcast("All non-player titans are now of type PUNK!");
                        break;
                }

                if (type != null)
                {
                    foreach (TITAN titan in FengGameManagerMKII.Instance.titans)
                    {
                        if (titan.photonView.isMine && type != titan.abnormalType)
                        {
                            titan.setAbnormalType2(type ?? titan.abnormalType, type == TitanClass.Crawler);
                        }
                    }
                }
            }
        }
    }
}
