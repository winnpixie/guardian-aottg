namespace Guardian.Features.Commands.Impl.RC
{
    class CommandPM : Command
    {
        public CommandPM() : base("pm", new string[] { "w", "whisper", "tell", "msg" }, "<id> <message>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 2 || !int.TryParse(args[0], out int id)) return;

            PhotonPlayer player = PhotonPlayer.Find(id);
            if (player == null) return;

            string message = InRoomChat.FormatMessage(string.Join(" ", args.CopyOfRange(1, args.Length)), string.Empty)[0] as string;
            FengGameManagerMKII.Instance.photonView.RPC("Chat", player, message, $"PM => You".AsColor("FFCC00"));
            irc.AddLine($"You => #{player.Id}".AsColor("FFCC00") + $": {message}");
        }
    }
}
