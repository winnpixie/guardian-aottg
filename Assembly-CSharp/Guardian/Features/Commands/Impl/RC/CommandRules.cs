namespace Guardian.Features.Commands.Impl.RC
{
    class CommandRules : Command
    {
        public CommandRules() : base("rules", new string[0], string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            irc.AddLine("Currently activated gamemodes:".WithColor("ffcc00"));

            // Bomb Mode
            if (RCSettings.BombMode > 0)
            {
                irc.AddLine("PVP Bomb Mode enabled.".WithColor("ffcc00"));
            }

            // Team Mode
            if (RCSettings.TeamMode > 0)
            {
                string str = string.Empty;
                switch (RCSettings.TeamMode)
                {
                    case 1:
                        str = "No sort";
                        break;
                    case 2:
                        str = "Locked by Size";
                        break;
                    case 3:
                        str = "Locked by Skill";
                        break;
                }
                irc.AddLine("Team Mode enabled</color> (" + str + ").".WithColor("ffcc00"));
            }

            // Point Limit
            if (RCSettings.PointMode > 0)
            {
                irc.AddLine($"Point Limit enabled ({RCSettings.PointMode}).".WithColor("ffcc00"));
            }

            if (RCSettings.DisableRock > 0)
            {
                irc.AddLine("Punk Rock-Throwing disabled.".WithColor("ffcc00"));
            }
            if (RCSettings.SpawnMode > 0)
            {
                irc.AddLine(("Custom spawn rate enabled (" + RCSettings.NormalRate.ToString("F2") + "% Normal, " + RCSettings.AberrantRate.ToString("F2") + "% Abnormal, " + RCSettings.JumperRate.ToString("F2") + "% Jumper, " + RCSettings.CrawlerRate.ToString("F2") + "% Crawler, " + RCSettings.PunkRate.ToString("F2") + "% Punk ").WithColor("ffcc00"));
            }
            if (RCSettings.ExplodeMode > 0)
            {
                irc.AddLine($"Titan explode mode enabled ({RCSettings.ExplodeMode}).".WithColor("ffcc00"));
            }
            if (RCSettings.HealthMode > 0)
            {
                irc.AddLine(("Titan health mode enabled (" + RCSettings.HealthLower + "-" + RCSettings.HealthUpper + ").").WithColor("ffcc00"));
            }
            if (RCSettings.InfectionMode > 0)
            {
                irc.AddLine($"Infection mode enabled ({RCSettings.InfectionMode}).".WithColor("ffcc00"));
            }
            if (RCSettings.DamageMode > 0)
            {
                irc.AddLine($"Minimum nape damage enabled ({RCSettings.DamageMode}).".WithColor("ffcc00"));
            }
            if (RCSettings.MoreTitans > 0)
            {
                irc.AddLine(("Custom titan # enabled (" + RCSettings.MoreTitans + ").").WithColor("ffcc00"));
            }
            if (RCSettings.SizeMode > 0)
            {
                irc.AddLine(("Custom titan size enabled (" + RCSettings.SizeLower.ToString("F2") + "," + RCSettings.SizeUpper.ToString("F2") + ").").WithColor("ffcc00"));
            }
            if (RCSettings.BanEren > 0)
            {
                irc.AddLine("Anti-Eren enabled. Using Titan Eren will get you kicked.".WithColor("ffcc00"));
            }
            if (RCSettings.WaveModeOn == 1)
            {
                irc.AddLine($"Custom wave mode enabled ({RCSettings.WaveModeNum}).".WithColor("ffcc00"));
            }
            if (RCSettings.FriendlyMode > 0)
            {
                irc.AddLine("Friendly-Fire disabled. PVP is prohibited.".WithColor("ffcc00"));
            }
            if (RCSettings.PvPMode > 0)
            {
                if (RCSettings.PvPMode == 1)
                {
                    irc.AddLine("AHSS/Blade PVP is on (team-based).".WithColor("ffcc00"));
                }
                else if (RCSettings.PvPMode == 2)
                {
                    irc.AddLine("AHSS/Blade PVP is on (FFA).".WithColor("ffcc00"));
                }
            }
            if (RCSettings.MaxWave > 0)
            {
                irc.AddLine($"Max Wave set to {RCSettings.MaxWave}".WithColor("ffcc00"));
            }
            if (RCSettings.HorseMode > 0)
            {
                irc.AddLine("Horses are enabled.".WithColor("ffcc00"));
            }
            if (RCSettings.AhssReload > 0)
            {
                irc.AddLine("AHSS Air-Reload disabled.".WithColor("ffcc00"));
            }
            if (RCSettings.PunkWaves > 0)
            {
                irc.AddLine("Punk override every 5 waves enabled.".WithColor("ffcc00"));
            }
            if (RCSettings.EndlessMode > 0)
            {
                irc.AddLine($"Endless Respawn is enabled ({RCSettings.EndlessMode}s).".WithColor("ffcc00"));
            }
            if (RCSettings.GlobalDisableMinimap > 0)
            {
                irc.AddLine("Minimaps are disabled.".WithColor("ffcc00"));
            }
            if (RCSettings.DeadlyCannons > 0)
            {
                irc.AddLine("Cannons will kill humans.".WithColor("ffcc00"));
            }

            // MOTD
            if (RCSettings.Motd != string.Empty)
            {
                irc.AddLine("MOTD: ".WithColor("ffcc00") + RCSettings.Motd);
            }
        }
    }
}
