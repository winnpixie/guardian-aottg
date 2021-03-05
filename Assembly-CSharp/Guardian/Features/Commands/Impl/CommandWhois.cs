using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl
{
    class CommandWhois : Command
    {
        public CommandWhois() : base("whois", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                var player = PhotonPlayer.Find(id);

                if (player != null)
                {
                    irc.AddLine($"Whois Report (#{player.Id})".WithColor("AAFF00").AsBold());

                    irc.AddLine("Name: ".WithColor("FFCC00") + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).Colored());
                    irc.AddLine("Guild: ".WithColor("FFCC00") + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Guild]).Colored());
                    irc.AddLine("Status: ".WithColor("FFCC00") + (GExtensions.AsBool(player.customProperties[PhotonPlayerProperty.Dead]) ? "Dead" : "Alive"));

                    var kills = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Kills]);
                    var deaths = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Deaths]);
                    var totalDamage = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.TotalDamage]);
                    irc.AddLine("Kills: ".WithColor("FFCC00") + kills);
                    irc.AddLine("Deaths: ".WithColor("FFCC00") + deaths);
                    irc.AddLine("K/D Ratio: ".WithColor("FFCC00") + (deaths == 0 ? kills : ((double)kills / (double)deaths)) + $" ({kills}:{deaths})");

                    irc.AddLine("Max Damage: ".WithColor("FFCC00") + GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.MaxDamage]));
                    irc.AddLine("Total Damage: ".WithColor("FFCC00") + totalDamage);

                    irc.AddLine("Average Damage: ".WithColor("FFCC00") + (kills == 0 ? "n/a" : (totalDamage / kills).ToString()));

                    var bombRadius = GExtensions.AsFloat(player.customProperties[PhotonPlayerProperty.RCBombRadius]);
                    if (player.customProperties.ContainsKey(PhotonPlayerProperty.RCBombRadius))
                    {
                        irc.AddLine("Bomb Radius: ".WithColor("FFCC00") + ((bombRadius - 20f) / 4f));
                    }

                    var team = "Human (Blade)";
                    if (GameHelper.IsAHSS(player))
                    {
                        team = "Human (AHSS)";
                    }
                    else if (GameHelper.IsPT(player))
                    {
                        team = "Player Titan";
                    }
                    irc.AddLine("Team: ".WithColor("FFCC00") + team);
                }
            }
        }
    }
}
