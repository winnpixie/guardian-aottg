using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl
{
    class CommandSetGuild : Command
    {
        public CommandSetGuild() : base("setguild", new string[] { "guild" }, "[guild]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            var guild = string.Empty;
            if (args.Length > 0)
            {
                guild = string.Join(" ", args);
            }

            LoginFengKAI.Player.Guild = guild;
            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.Guild, guild }
            });

            var hero = GameHelper.GetHero(PhotonNetwork.player);
            if (hero != null)
            {
                FengGameManagerMKII.Instance.photonView.RPC("labelRPC", PhotonTargets.All, new object[] { hero.photonView.viewID });
            }
        }
    }
}
