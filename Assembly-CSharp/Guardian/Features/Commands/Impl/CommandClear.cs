using Guardian.Utilities;
using System.Collections.Generic;

namespace Guardian.Features.Commands.Impl
{
    class CommandClear : Command
    {
        public CommandClear() : base("clear", new string[0], "[CHAT/log]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "chat":
                        break;
                    case "log":
                        Mod.Logger.Messages = new List<string>();
                        Mod.Logger.Info("Cleared event log!");
                        return;
                }
            }

            InRoomChat.Messages = new List<InRoomChat.Message>();
            if (!PhotonNetwork.isMasterClient)
            {
                irc.AddLine("Chat has been cleared.".WithColor("ffcc00"));
            }
            else
            {
                for (int i = 0; i < 14; i++)
                {
                    GameHelper.Broadcast("~");
                }
                GameHelper.Broadcast("Chat has been cleared!".WithColor("ffcc00"));
            }
        }
    }
}
