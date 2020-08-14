namespace Guardian.Features.Commands.Impl.RC
{
    class CommandRules : Command
    {
        public CommandRules() : base("rules", new string[0], "", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            irc.AddLine("Currently activated gamemodes:".WithColor("ffcc00"));
            if (RCSettings.BombMode > 0)
            {
                irc.AddLine("Bomb mode is on.".WithColor("ffcc00"));
            }
            if (RCSettings.TeamMode > 0)
            {
                if (RCSettings.TeamMode == 1)
                {
                    irc.AddLine("Team mode is on (no sort).".WithColor("ffcc00"));
                }
                else if (RCSettings.TeamMode == 2)
                {
                    irc.AddLine("Team mode is on (sort by size).".WithColor("ffcc00"));
                }
                else if (RCSettings.TeamMode == 3)
                {
                    irc.AddLine("Team mode is on (sort by skill).".WithColor("ffcc00"));
                }
            }
            if (RCSettings.PointMode > 0)
            {
                irc.AddLine($"Point mode is on ({RCSettings.PointMode}).".WithColor("ffcc00"));
            }
            if (RCSettings.DisableRock > 0)
            {
                irc.AddLine("Punk Rock-Throwing is disabled.".WithColor("ffcc00"));
            }
            if (RCSettings.SpawnMode > 0)
            {
                irc.AddLine(("Custom spawn rate is on (" + RCSettings.NormalRate.ToString("F2") + "% Normal, " + RCSettings.AberrantRate.ToString("F2") + "% Abnormal, " + RCSettings.JumperRate.ToString("F2") + "% Jumper, " + RCSettings.CrawlerRate.ToString("F2") + "% Crawler, " + RCSettings.PunkRate.ToString("F2") + "% Punk ").WithColor("ffcc00"));
            }
            if (RCSettings.ExplodeMode > 0)
            {
                irc.AddLine($"Titan explode mode is on ({RCSettings.ExplodeMode}).".WithColor("ffcc00"));
            }
            if (RCSettings.HealthMode > 0)
            {
                irc.AddLine(("Titan health mode is on (" + RCSettings.HealthLower + "-" + RCSettings.HealthUpper + ").").WithColor("ffcc00"));
            }
            if (RCSettings.InfectionMode > 0)
            {
                irc.AddLine($"Infection mode is on ({RCSettings.InfectionMode}).".WithColor("ffcc00"));
            }
            if (RCSettings.DamageMode > 0)
            {
                irc.AddLine($"Minimum nape damage is on ({RCSettings.DamageMode}).".WithColor("ffcc00"));
            }
            if (RCSettings.MoreTitans > 0)
            {
                irc.AddLine(("Custom titan # is on (" + RCSettings.MoreTitans + ").").WithColor("ffcc00"));
            }
            if (RCSettings.SizeMode > 0)
            {
                irc.AddLine(("Custom titan size is on (" + RCSettings.SizeLower.ToString("F2") + "," + RCSettings.SizeUpper.ToString("F2") + ").").WithColor("ffcc00"));
            }
            if (RCSettings.BanEren > 0)
            {
                irc.AddLine("Anti-Eren is on. Using Titan eren will get you kicked.".WithColor("ffcc00"));
            }
            if (RCSettings.WaveModeOn == 1)
            {
                irc.AddLine($"Custom wave mode is on ({RCSettings.WaveModeNum}).".WithColor("ffcc00"));
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
                irc.AddLine($"Endless Respawn is enabled ({RCSettings.EndlessMode} seconds).".WithColor("ffcc00"));
            }
            if (RCSettings.GlobalDisableMinimap > 0)
            {
                irc.AddLine("Minimaps are disabled.".WithColor("ffcc00"));
            }
            if (RCSettings.DeadlyCannons > 0)
            {
                irc.AddLine("Cannons will kill humans.".WithColor("ffcc00"));
            }
            if (RCSettings.Motd != string.Empty)
            {
                irc.AddLine(("MOTD: " + RCSettings.Motd).WithColor("ffcc00"));
            }
        }
    }
}
