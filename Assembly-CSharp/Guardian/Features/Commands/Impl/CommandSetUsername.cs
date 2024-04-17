namespace Guardian.Features.Commands.Impl
{
    class CommandSetUsername : Command
    {
        public CommandSetUsername() : base("setusername", new string[] { "username", "setname", "name" }, "[name]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1) return; // Disallow blank names

            string name = string.Join(" ", args);
            LoginFengKAI.Player.Name = name;

            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.Name, name }
            });

            HERO hero = PhotonNetwork.player.GetHero();
            if (hero == null) return;

            FengGameManagerMKII.Instance.photonView.RPC("labelRPC", PhotonTargets.All, new object[] { hero.photonView.viewID });
        }
    }
}
