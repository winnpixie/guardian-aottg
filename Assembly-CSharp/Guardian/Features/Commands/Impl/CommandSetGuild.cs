namespace Guardian.Features.Commands.Impl
{
    class CommandSetGuild : Command
    {
        public CommandSetGuild() : base("setguild", new string[] { "guild" }, "[guild]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            string guild = args.Length > 0 ? string.Join(" ", args) : string.Empty;

            LoginFengKAI.Player.Guild = guild;
            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.Guild, guild }
            });

            HERO hero = PhotonNetwork.player.GetHero();
            if (hero == null) return;

            FengGameManagerMKII.Instance.photonView.RPC("labelRPC", PhotonTargets.All, new object[] { hero.photonView.viewID });
        }
    }
}
