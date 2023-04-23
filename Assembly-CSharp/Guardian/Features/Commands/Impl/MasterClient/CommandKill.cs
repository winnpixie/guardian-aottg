namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandKill : Command
    {
        public CommandKill() : base("kill", new string[0], "<id> [reason]", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int id)) return;

            PhotonPlayer player = PhotonPlayer.Find(id);
            if (player == null) return;

            if (player.IsTitan)
            {
                TITAN titan = player.GetTitan();
                if (titan == null || titan.hasDie) return;

                titan.photonView.RPC("titanGetHit", player, titan.photonView.viewID, RCSettings.MinimumDamage > 10 ? RCSettings.MinimumDamage : 10);
            }
            else
            {
                HERO hero = player.GetHero();
                if (hero == null || hero.HasDied()) return;

                string reason = args.Length < 2 ? "[FF0000]Server " : string.Join(" ", args.CopyOfRange(2, args.Length - 1));
                hero.photonView.RPC("netDie", PhotonTargets.All, hero.transform.position, false, -1, reason, false);
            }
        }
    }
}
