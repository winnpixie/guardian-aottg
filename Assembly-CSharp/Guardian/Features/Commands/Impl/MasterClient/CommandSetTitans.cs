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
                TitanClass? type = args[0].ToLower() switch
                {
                    "normal" => TitanClass.Normal,
                    "aberrant" => TitanClass.Aberrant,
                    "jumper" => TitanClass.Jumper,
                    "crawler" => TitanClass.Crawler,
                    "punk" => TitanClass.Punk,
                    _ => null
                };

                if (type != null)
                {
                    GameHelper.Broadcast($"All non-player titans are now of type {type.Value}!");

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
