using UnityEngine;

namespace Guardian.Features.Commands.Impl
{
    class CommandTime : Command
    {
        public CommandTime() : base("time", new string[0], "<day/dawn/night>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    if (GExtensions.TryParseEnum(args[0], out DayLight value))
                    {
                        IN_GAME_MAIN_CAMERA.DayLight = value;
                        Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setDayLight(IN_GAME_MAIN_CAMERA.DayLight);
                        irc.AddLine($"Lighting is now {args[0].ToUpper()}.");
                    }
                }
                catch { }
            }
        }
    }
}