using Guardian.Utilities;
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
                    GameHelper.Broadcast($"Added {time}s to the clock!");
                }

                // Max players
                if (args[0].Equals("max", StringComparison.OrdinalIgnoreCase) && int.TryParse(args[1], out int max))
                {
                    PhotonNetwork.room.expectedMaxPlayers = max;
                    PhotonNetwork.room.maxPlayers = PhotonNetwork.room.expectedMaxPlayers;
                    GameHelper.Broadcast($"Max players is now {max}!");
                }

                // Allowing joins or not
                if (args[0].Equals("open", StringComparison.OrdinalIgnoreCase))
                {
                    PhotonNetwork.room.expectedJoinability = args[1].Equals("true", StringComparison.OrdinalIgnoreCase);
                    PhotonNetwork.room.open = PhotonNetwork.room.expectedJoinability;
                    GameHelper.Broadcast($"Room is {(PhotonNetwork.room.open ? "now" : "no longer")} allowing joins!");
                }

                // Visible in lobby or not
                if (args[0].Equals("visible", StringComparison.OrdinalIgnoreCase))
                {
                    PhotonNetwork.room.expectedVisibility = args[1].Equals("true", StringComparison.OrdinalIgnoreCase);
                    PhotonNetwork.room.visible = PhotonNetwork.room.expectedVisibility;
                    GameHelper.Broadcast($"Room is {(PhotonNetwork.room.visible ? "now" : "no longer")} being shown in the lobby!");
                }

                // Player TTL
                if (args[0].Equals("pttl", StringComparison.OrdinalIgnoreCase) && int.TryParse(args[1], out int pttl))
                {
                    if (PhotonNetwork.player.Id == 1)
                    {
                        PhotonNetwork.room.playerTtl = pttl;
                        GameHelper.Broadcast($"Player TTL is now {pttl}ms!");
                    }
                    else
                    {
                        irc.AddLine("You must be the room creator to execute this!".WithColor("FF0000"));
                    }
                }

                // Room TTL
                if (args[0].Equals("rttl", StringComparison.OrdinalIgnoreCase) && int.TryParse(args[1], out int rttl))
                {
                    if (PhotonNetwork.player.Id == 1)
                    {
                        PhotonNetwork.room.emptyRoomTtl = rttl;
                        GameHelper.Broadcast($"Room TTL is now {rttl}ms!");
                    }
                    else
                    {
                        irc.AddLine("You must be the room creator to execute this!".WithColor("FF0000"));
                    }
                }
            }
        }
    }
}
