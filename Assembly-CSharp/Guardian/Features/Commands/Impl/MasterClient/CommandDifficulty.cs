using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandDifficulty : Command
    {
        public CommandDifficulty() : base("difficulty", new string[0], "<level>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1) return;

            int difficulty = args[0].ToLower() switch
            {
                "training" => -1,
                "normal" => 0,
                "hard" => 1,
                "abnormal" => 2,
                _ => -2
            };

            if (difficulty < -1) return;

            FengGameManagerMKII.Instance.difficulty = difficulty;
            IN_GAME_MAIN_CAMERA.Difficulty = difficulty;

            GameHelper.Broadcast($"Room difficulty is now {args[0].ToUpper()}!");
            GameHelper.Broadcast("This change will be effective on the next wave OR game restart.");
        }
    }
}
