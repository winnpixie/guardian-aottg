namespace Guardian.Features.Commands.Impl.RC
{
    class CommandRules : Command
    {
        public CommandRules() : base("rules", new string[0], string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            irc.AddLine("Currently activated gamemodes:".AsColor("FFCC00"));

            // Bomb Mode
            if (RCSettings.BombMode > 0)
            {
                irc.AddLine("PVP Bomb Mode enabled.".AsColor("FFCC00"));
            }

            // Global Minimap Disable
            if (RCSettings.GlobalDisableMinimap > 0)
            {
                irc.AddLine("Minimaps are disabled.".AsColor("FFCC00"));
            }

            // Horses
            if (RCSettings.HorseMode > 0)
            {
                irc.AddLine("Horses are enabled.".AsColor("FFCC00"));
            }

            // Punk Waves
            if (RCSettings.PunkWaves > 0)
            {
                irc.AddLine("Punk override every 5 waves enabled.".AsColor("FFCC00"));
            }

            // AHSS Air-Reload
            if (RCSettings.AhssReload > 0)
            {
                irc.AddLine("AHSS Air-Reload disabled.".AsColor("FFCC00"));
            }

            // Team Sorting
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

                irc.AddLine("Team Mode enabled (".AsColor("FFCC00") + str + ").".AsColor("FFCC00"));
            }

            // Point Limit
            if (RCSettings.PointMode > 0)
            {
                irc.AddLine("Point Limit enabled (".AsColor("FFCC00") + RCSettings.PointMode + ").".AsColor("FFCC00"));
            }

            // Punk Rocks
            if (RCSettings.DisableRock > 0)
            {
                irc.AddLine("Punk Rock-Throwing disabled.".AsColor("FFCC00"));
            }

            // Titan Explode
            if (RCSettings.ExplodeMode > 0)
            {
                irc.AddLine("Titan explode mode enabled (".AsColor("FFCC00")
                    + RCSettings.ExplodeMode + ").".AsColor("FFCC00"));
            }

            // Titan Health
            if (RCSettings.HealthMode > 0)
            {
                irc.AddLine("Titan health mode enabled (".AsColor("FFCC00")
                    + RCSettings.HealthLower + "-".AsColor("FFCC00")
                    + RCSettings.HealthUpper + ").".AsColor("FFCC00"));
            }

            // Infection
            if (RCSettings.InfectionMode > 0)
            {
                irc.AddLine("Infection mode enabled (".AsColor("FFCC00") + RCSettings.InfectionMode + ").".AsColor("FFCC00"));
            }

            // Anti-Eren
            if (RCSettings.BanEren > 0)
            {
                irc.AddLine("Anti-Eren enabled. Using Titan Eren will get you kicked.".AsColor("FFCC00"));
            }

            // Custom Titan Count
            if (RCSettings.MoreTitans > 0)
            {
                irc.AddLine("Custom Titan # enabled (".AsColor("FFCC00") + RCSettings.MoreTitans + ").".AsColor("FFCC00"));
            }

            // Minimum Damage
            if (RCSettings.DamageMode > 0)
            {
                irc.AddLine("Minimum nape damage enabled (".AsColor("FFCC00") + RCSettings.DamageMode + ").".AsColor("FFCC00"));
            }

            // Custom Titan Sizes
            if (RCSettings.SizeMode > 0)
            {
                irc.AddLine("Custom titan size enabled (".AsColor("FFCC00")
                    + RCSettings.SizeLower.ToString("F2") + ", ".AsColor("FFCC00")
                    + RCSettings.SizeUpper.ToString("F2") + ").".AsColor("FFCC00"));
            }

            // Custom Spawn Rates
            if (RCSettings.SpawnMode > 0)
            {
                irc.AddLine("Custom spawn rate enabled (".AsColor("FFCC00")
                    + RCSettings.NormalRate.ToString("F2") + "% Normal, ".AsColor("FFCC00")
                    + RCSettings.AberrantRate.ToString("F2") + "% Abnormal, ".AsColor("FFCC00")
                    + RCSettings.JumperRate.ToString("F2") + "% Jumper, ".AsColor("FFCC00")
                    + RCSettings.CrawlerRate.ToString("F2") + "% Crawler, ".AsColor("FFCC00")
                    + RCSettings.PunkRate.ToString("F2") + "% Punk.".AsColor("FFCC00"));
            }

            // Wave Mode (Titan count multiplier?)
            if (RCSettings.WaveModeOn == 1)
            {
                irc.AddLine("Custom wave mode enabled (".AsColor("FFCC00") + RCSettings.WaveModeNum + ").".AsColor("FFCC00"));
            }

            // Friendly Fire
            if (RCSettings.FriendlyMode > 0)
            {
                irc.AddLine("Friendly-Fire disabled. PVP is not allowed.".AsColor("FFCC00"));
            }

            // PVP Mode
            if (RCSettings.PvPMode > 0)
            {
                if (RCSettings.PvPMode == 1)
                {
                    irc.AddLine("AHSS/Blade PVP is on (Team-Based).".AsColor("FFCC00"));
                }
                else if (RCSettings.PvPMode == 2)
                {
                    irc.AddLine("AHSS/Blade PVP is on (FFA).".AsColor("FFCC00"));
                }
            }

            // Max Wave
            if (RCSettings.MaxWave > 0)
            {
                irc.AddLine("Max Wave set to ".AsColor("FFCC00") + RCSettings.MaxWave + ".".AsColor("FFCC00"));
            }

            // Endless Respawn
            if (RCSettings.EndlessMode > 0)
            {
                irc.AddLine("Endless Respawn enabled (".AsColor("FFCC00") + RCSettings.EndlessMode + "s).".AsColor("FFCC00"));
            }

            // Deadly Cannons
            if (RCSettings.DeadlyCannons > 0)
            {
                irc.AddLine("Cannons will kill humans.".AsColor("FFCC00"));
            }

            // ASO
            if (RCSettings.RacingStatic > 0)
            {
                irc.AddLine("Racing will not restart on win".AsColor("FFCC00"));
            }

            // MOTD
            if (RCSettings.Motd.Length > 0)
            {
                irc.AddLine("MOTD: ".AsColor("FFCC00") + RCSettings.Motd);
            }
        }
    }
}
