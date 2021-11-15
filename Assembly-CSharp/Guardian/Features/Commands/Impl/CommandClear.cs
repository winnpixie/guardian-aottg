using Guardian.Utilities;
using System.Collections.Generic;

namespace Guardian.Features.Commands.Impl
{
    class CommandClear : Command
    {
        public CommandClear() : base("clear", new string[0], "[log/global/id]", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "log":
                        Mod.Logger.Entries = new List<Logger.Entry>();
                        Mod.Logger.Info("Event log has been cleared!");
                        break;
                    case "global":
                        if (PhotonNetwork.isMasterClient)
                        {
                            InRoomChat.Messages = new List<InRoomChat.Message>();
                            for (int i = 0; i < 14; i++)
                            {
                                GameHelper.Broadcast(" ");
                            }
                            GameHelper.Broadcast("Global chat has been cleared!".AsColor("AAFF00"));
                        }
                        break;
                    default:
                        if (int.TryParse(args[0], out int id) && PhotonNetwork.isMasterClient)
                        {
                            PhotonPlayer player = PhotonPlayer.Find(id);
                            if (player != null)
                            {
                                for (int i = 0; i < 14; i++)
                                {
                                    FengGameManagerMKII.Instance.photonView.RPC("Chat", player, " ", "[MC]".AsColor("AAFF00").AsBold());
                                }
                                GameHelper.Broadcast("Your chat has been cleared!".AsColor("AAFF00"));
                            }
                        }
                        break;
                }
            }
            else
            {
                InRoomChat.Messages = new List<InRoomChat.Message>();
                irc.AddLine("Local chat has been cleared.".AsColor("AAFF00"));
            }
        }
    }
}
