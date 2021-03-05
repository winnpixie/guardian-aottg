namespace Guardian.Features.Commands.Impl.RC
{
    class CommandPM : Command
    {
        public CommandPM() : base("pm", new string[] { "w", "whisper", "tell", "msg" }, "<id> <message>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 1 && int.TryParse(args[0], out int id))
            {
                var player = PhotonPlayer.Find(id);
                if (player != null)
                {
                    var message = string.Join(" ", args.CopyOfRange(1, args.Length));
                    FengGameManagerMKII.Instance.photonView.RPC("Chat", player, $"FROM [{PhotonNetwork.player.Id}] ".WithColor("FFCC00") + message, string.Empty);
                    irc.AddLine($"TO [{player.Id}]".WithColor("FFCC00") + $": {message}");
                }
            }
        }
    }
}
