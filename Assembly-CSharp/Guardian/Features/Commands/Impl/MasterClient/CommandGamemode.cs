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
                Gamemode oldMode = Mod.Gamemodes.Current;

                Gamemode newMode = Mod.Gamemodes.Find(args[0]);
                if (newMode != null)
                {
                    GameHelper.Broadcast($"Gamemode Switch ({oldMode.Name} -> {newMode.Name})!");

                    newMode.OnReset();
                    Mod.Gamemodes.Current = newMode;

                    oldMode.CleanUp();
                }
            }
            else
            {
                irc.AddLine("Available Gamemodes:".WithColor("AAFF00"));

                foreach (Gamemode mode in Mod.Gamemodes.Elements)
                {
                    irc.AddLine("> ".WithColor("00FF00").AsBold() + mode.Name);
                }
            }
        }
    }
}
