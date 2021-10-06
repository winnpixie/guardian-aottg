using Guardian.Utilities;
using System.Collections.Generic;

namespace Guardian.Features.Commands.Impl
{
    class CommandClear : Command
    {
        public CommandClear() : base("clear", new string[0], "[log]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && args[0].Equals("log", System.StringComparison.OrdinalIgnoreCase))
            {
                Mod.Logger.Entries = new List<string>();
                Mod.Logger.Info("Cleared event log!");
                return;
            }

            InRoomChat.Messages = new List<InRoomChat.Message>();
            if (!PhotonNetwork.isMasterClient)
            {
                irc.AddLine("Chat has been cleared.".AsColor("AAFF00"));
            }
            else
            {
                for (int i = 0; i < 14; i++)
                {
                    GameHelper.Broadcast(" ");
                }
                GameHelper.Broadcast("Chat has been cleared!".AsColor("AAFF00"));
            }
        }
    }
}
