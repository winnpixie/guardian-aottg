using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl
{
    class CommandSetName : Command
    {
        public CommandSetName() : base("setname", new string[] { "name" }, "[name]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            string name = "";
            if (args.Length > 0)
            {
                name = string.Join(" ", args);
            }
            LoginFengKAI.Player.name = name;
            FengGameManagerMKII.NameField = name;

            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.Name, name }
            });

            HERO hero = GameHelper.GetHero(PhotonNetwork.player);
            if (hero != null)
            {
                FengGameManagerMKII.Instance.photonView.RPC("labelRPC", PhotonTargets.All, new object[] { hero.photonView.viewID });
            }
        }
    }
}
