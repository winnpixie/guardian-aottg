using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.Debug
{
    class CommandStopwatch : Command
    {
        private readonly MsTimer Watch = new MsTimer();

        public CommandStopwatch() : base("stopwatch", new string[] { "sw", "timer" }, "<action>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1) return;

            switch (args[0].ToLower())
            {
                case "start":
                    Watch.Update();
                    irc.AddLine("Timer started! Type <b>/stopwatch end</b> to see how long you waited.");
                    break;
                case "end":
                    irc.AddLine($"Timer stopped! You waited {Watch.GetElapsed()}ms to end it.");
                    break;
                default:
                    break;
            }
        }
    }
}
