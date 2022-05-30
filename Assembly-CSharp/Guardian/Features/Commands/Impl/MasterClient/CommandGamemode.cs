using Guardian.Features.Gamemodes;
using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandGamemode : Command
    {
        public CommandGamemode() : base("gamemode", new string[] { "gm", "mode" }, "<mode>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                Gamemode oldMode = GuardianClient.Gamemodes.CurrentMode;
                Gamemode newMode = GuardianClient.Gamemodes.Find(args[0]);
                if (newMode == null) return;

                GameHelper.Broadcast($"Gamemode Switch ({oldMode.Name} -> {newMode.Name})!");

                newMode.OnReset();
                GuardianClient.Gamemodes.CurrentMode = newMode;

                oldMode.CleanUp();
            }
            else
            {
                irc.AddLine("Available Gamemodes:".AsColor("AAFF00"));

                foreach (Gamemode mode in GuardianClient.Gamemodes.Elements)
                {
                    irc.AddLine("> ".AsColor("00FF00").AsBold() + mode.Name);
                }
            }
        }
    }
}
