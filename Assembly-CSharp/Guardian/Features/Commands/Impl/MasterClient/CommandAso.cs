using System;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandAso : Command
    {
        public CommandAso() : base("aso", new string[0], "<kdr/racing>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].Equals("kdr", StringComparison.OrdinalIgnoreCase))
                {
                    if (RCSettings.AsoPreserveKDR == 0)
                    {
                        RCSettings.AsoPreserveKDR = 1;
                        irc.AddLine("KDRs will be preserved from disconnects.".WithColor("FFCC00"));
                    }
                    else
                    {
                        RCSettings.AsoPreserveKDR = 0;
                        irc.AddLine("KDRs will not be preserved from disconnects.".WithColor("FFCC00"));
                    }
                }
                else if (args[0].Equals("racing", StringComparison.OrdinalIgnoreCase))
                {
                    if (RCSettings.RacingStatic == 0)
                    {
                        RCSettings.RacingStatic = 1;
                        irc.AddLine("Racing will not end on finish.".WithColor("FFCC00"));
                    }
                    else
                    {
                        RCSettings.RacingStatic = 0;
                        irc.AddLine("Racing will end on finish.".WithColor("FFCC00"));
                    }
                }
            }
        }
    }
}
