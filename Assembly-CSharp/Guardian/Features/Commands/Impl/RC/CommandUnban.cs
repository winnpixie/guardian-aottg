using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.RC
{
    class CommandUnban : Command
    {
        public CommandUnban() : base("unban", new string[] { "pardon" }, "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int id)) return;

            if (FengGameManagerMKII.OnPrivateServer)
            {
                FengGameManagerMKII.ServerRequestUnban(id.ToString());
            }
            else if (PhotonNetwork.isMasterClient)
            {
                if (!FengGameManagerMKII.BanHash.ContainsKey(id)) return;

                GameHelper.Broadcast($"{GExtensions.AsString(FengGameManagerMKII.BanHash[id])} has been unbanned.".AsColor("FFCC00"));
                FengGameManagerMKII.BanHash.Remove(id);
            }
            else
            {
                irc.AddLine("Command requires master client.".AsColor("FF0000"));
            }
        }
    }
}
