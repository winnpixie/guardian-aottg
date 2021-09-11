namespace Guardian.Features.Commands.Impl
{
    class CommandSay : Command
    {
        public CommandSay() : base("say", new string[0], "<message>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                string name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]).ColorParsed();
                if (name.Uncolored().Length < 1)
                {
                    name = GExtensions.AsString(PhotonNetwork.player.customProperties[PhotonPlayerProperty.Name]);
                }
                FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, InRoomChat.FormatMessage(string.Join(" ", args), name));
            }
        }
    }
}
