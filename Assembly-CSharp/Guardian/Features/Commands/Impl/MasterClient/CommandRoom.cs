using System;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandRoom : Command
    {
        public CommandRoom() : base("room", new string[0], "<time/max/open/visible/pttl/rttl> <value>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 1)
            {
                // Time left
                if (args[0].Equals("time", StringComparison.OrdinalIgnoreCase) && int.TryParse(args[1], out int time))
                {
                    FengGameManagerMKII.Instance.AddTime(time);
                    FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, $"Added {time} seconds to the clock!".WithColor("ffcc00"), "");
                }

                // Max players
                if (args[0].Equals("max", StringComparison.OrdinalIgnoreCase) && int.TryParse(args[1], out int max))
                {
                    PhotonNetwork.room.maxPlayers = PhotonNetwork.room.expectedMaxPlayers = max;
                    FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, $"Max players is now {max}!".WithColor("ffcc00"), "");
                }

                // Allowing joins or not
                if (args[0].Equals("open", StringComparison.OrdinalIgnoreCase))
                {
                    PhotonNetwork.room.open = PhotonNetwork.room.expectedJoinability = args[1].Equals("true", StringComparison.OrdinalIgnoreCase);
                    FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, $"Room is {(PhotonNetwork.room.open ? "now" : "no longer")} allowing joins!".WithColor("ffcc00"), "");
                }

                // Visible in lobby or not
                if (args[0].Equals("visible", StringComparison.OrdinalIgnoreCase))
                {
                    PhotonNetwork.room.visible = PhotonNetwork.room.expectedVisibility = args[1].Equals("true", StringComparison.OrdinalIgnoreCase);
                    FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, $"Room is {(PhotonNetwork.room.visible ? "now" : "no longer")} being shown in the lobby!".WithColor("ffcc00"), "");
                }

                // Player TTL
                if (args[0].Equals("pttl", StringComparison.OrdinalIgnoreCase) && int.TryParse(args[1], out int pttl))
                {
                    if (PhotonNetwork.player.id == 1)
                    {
                        PhotonNetwork.room.playerTtl = pttl;
                        FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, $"Player TTL is now {pttl}ms!".WithColor("ffcc00"), "");
                    }
                    else
                    {
                        irc.AddLine("You must be the room creator to execute this.".WithColor("ff0000"));
                    }
                }

                // Room TTL
                if (args[0].Equals("rttl", StringComparison.OrdinalIgnoreCase) && int.TryParse(args[1], out int rttl))
                {
                    if (PhotonNetwork.player.id == 1)
                    {
                        PhotonNetwork.room.emptyRoomTtl = rttl;
                        FengGameManagerMKII.Instance.photonView.RPC("Chat", PhotonTargets.All, $"Room TTL is now {rttl}ms!".WithColor("ffcc00"), "");
                    }
                    else
                    {
                        irc.AddLine("You must be the room creator to execute this.".WithColor("ff0000"));
                    }
                }
            }
        }
    }
}
