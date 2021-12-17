namespace Guardian.Features.Commands.Impl
{
    class CommandWhois : Command
    {
        public CommandWhois() : base("whois", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int id)) return;

            PhotonPlayer player = PhotonPlayer.Find(id);
            if (player == null) return;

            irc.AddLine($"Whois Report (#{player.Id})".AsColor("AAFF00").AsBold());

            irc.AddLine("Name: ".AsColor("FFCC00") + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]).NGUIToUnity());
            irc.AddLine("Guild: ".AsColor("FFCC00") + GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Guild]).NGUIToUnity());
            irc.AddLine("Status: ".AsColor("FFCC00") + (GExtensions.AsBool(player.customProperties[PhotonPlayerProperty.IsDead]) ? "Dead" : "Alive"));

            int kills = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Kills]);
            int deaths = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.Deaths]);
            int totalDamage = GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.TotalDamage]);
            irc.AddLine("Kills: ".AsColor("FFCC00") + kills);
            irc.AddLine("Deaths: ".AsColor("FFCC00") + deaths);
            irc.AddLine("K/D Ratio: ".AsColor("FFCC00") + (deaths < 2 ? kills : (kills / (double)deaths)).ToString("F2") + $" ({kills}:{deaths})");

            irc.AddLine("Max Damage: ".AsColor("FFCC00") + GExtensions.AsInt(player.customProperties[PhotonPlayerProperty.MaxDamage]));
            irc.AddLine("Total Damage: ".AsColor("FFCC00") + totalDamage);

            irc.AddLine("Average Damage: ".AsColor("FFCC00") + (kills == 0 ? "n/a" : (totalDamage / kills).ToString()));

            float bombRadius = GExtensions.AsFloat(player.customProperties[PhotonPlayerProperty.RCBombRadius]);
            if (player.customProperties.ContainsKey(PhotonPlayerProperty.RCBombRadius))
            {
                irc.AddLine("Bomb Radius: ".AsColor("FFCC00") + ((bombRadius - 20f) / 4f));
            }

            string team = "Human (Blade)";
            if (player.IsAhss)
            {
                team = "Human (AHSS)";
            }
            else if (player.IsTitan)
            {
                team = "Player Titan";
            }
            irc.AddLine("Team: ".AsColor("FFCC00") + team);
        }
    }
}
