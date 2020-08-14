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
                PhotonPlayer player = PhotonPlayer.Find(id);
                if (player != null)
                {
                    irc.AddLine($"-=+=- Whois Report (#{player.id}) -=+=-".WithColor("ffcc00"));
                    irc.AddLine("Name: ".WithColor("ffcc00") + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).Colored());
                    irc.AddLine("Guild: ".WithColor("ffcc00") + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Guild]).Colored());
                    irc.AddLine("Status: ".WithColor("ffcc00") + (GExtensions.AsBool(player.customProperties[PhotonPlayerProperty.Dead]) ? "Dead" : "Alive"));
                    int kills = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Kills]);
                    int deaths = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Deaths]);
                    irc.AddLine("Kills: ".WithColor("ffcc00") + kills);
                    irc.AddLine("Deaths: ".WithColor("ffcc00") + deaths);
                    irc.AddLine("K/D Ratio: ".WithColor("ffcc00") + (deaths == 0 ? kills : ((float)kills / (float)deaths)) + $" ({kills}:{deaths})");
                    irc.AddLine("Max Damage: ".WithColor("ffcc00") + GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.MaxDamage]));
                    irc.AddLine("Total Damage: ".WithColor("ffcc00") + GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.TotalDamage]));
                    string team = "Human (Blade)";
                    if (GameHelper.IsAHSS(player))
                    {
                        team = "Human (AHSS)";
                    }
                    else if (GameHelper.IsPT(player))
                    {
                        team = "Player Titan";
                    }
                    irc.AddLine("Team: ".WithColor("ffcc00") + team);
                }
            }
        }
    }
}
