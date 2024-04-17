namespace Guardian.Features.Commands.Impl
{
    class CommandSay : Command
    {
        public CommandSay() : base("say", new string[0], "[message]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1) return;

            string name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity();
            if (name.StripNGUI().Length < 1)
            {
                name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]);
            }
            FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, InRoomChat.FormatMessage(string.Join(" ", args), name));
        }
    }
}
