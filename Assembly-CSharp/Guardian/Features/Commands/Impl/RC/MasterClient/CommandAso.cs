using System;

namespace Guardian.Features.Commands.Impl.RC.MasterClient
{
    class CommandAso : Command
    {
        public CommandAso() : base("aso", new string[0], "<kdr|racing>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1) return;

            switch (args[0].ToLower())
            {
                case "kdr":
                    if (RCSettings.AsoPreserveKDR == 0)
                    {
                        RCSettings.AsoPreserveKDR = 1;
                        irc.AddLine("KDRs will be preserved from disconnects.".AsColor("FFCC00"));
                    }
                    else
                    {
                        RCSettings.AsoPreserveKDR = 0;
                        irc.AddLine("KDRs will not be preserved from disconnects.".AsColor("FFCC00"));
                    }
                    break;
                case "racing":
                    if (RCSettings.RacingStatic == 0)
                    {
                        RCSettings.RacingStatic = 1;
                        irc.AddLine("Racing will not end on finish.".AsColor("FFCC00"));
                    }
                    else
                    {
                        RCSettings.RacingStatic = 0;
                        irc.AddLine("Racing will end on finish.".AsColor("FFCC00"));
                    }
                    break;
            }
        }
    }
}
