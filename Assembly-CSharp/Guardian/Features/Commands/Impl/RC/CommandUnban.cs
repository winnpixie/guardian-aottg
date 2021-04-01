using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.RC
{
    class CommandUnban : Command
    {
        public CommandUnban() : base("unban", new string[] { "pardon" }, "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                if (FengGameManagerMKII.OnPrivateServer)
                {
                    FengGameManagerMKII.ServerRequestUnban(id.ToString());
                }
                else if (PhotonNetwork.isMasterClient)
                {
                    if (FengGameManagerMKII.BanHash.ContainsKey(id))
                    {
                        GameHelper.Broadcast($"{GExtensions.AsString(FengGameManagerMKII.BanHash[id])} has been unbanned.".WithColor("FFCC00"));
                        FengGameManagerMKII.BanHash.Remove(id);
                    }
                    else
                    {
                        irc.AddLine($"Player #{id} is not banned.".WithColor("FFCC00"));
                    }
                }
                else
                {
                    irc.AddLine("Command requires master client.".WithColor("FF0000"));
                }
            }
        }
    }
}
