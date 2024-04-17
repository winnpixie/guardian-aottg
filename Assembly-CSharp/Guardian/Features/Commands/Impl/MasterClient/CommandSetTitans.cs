using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandSetTitans : Command
    {
        public CommandSetTitans() : base("settitans", new string[0], "<type>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1) return;

            TitanClass? newTitanType = args[0].ToLower() switch
            {
                "normal" => TitanClass.Normal,
                "aberrant" => TitanClass.Aberrant,
                "jumper" => TitanClass.Jumper,
                "crawler" => TitanClass.Crawler,
                "punk" => TitanClass.Punk,
                _ => null
            };

            if (newTitanType == null) return;

            foreach (TITAN titan in FengGameManagerMKII.Instance.Titans)
            {
                if (!titan.photonView.isMine || newTitanType.Equals(titan.abnormalType)) continue;

                titan.SetAbnormalType2(newTitanType ?? titan.abnormalType, newTitanType == TitanClass.Crawler);
            }

            GameHelper.Broadcast($"All non-player titans are now of type {newTitanType.Value}!");
        }
    }
}
