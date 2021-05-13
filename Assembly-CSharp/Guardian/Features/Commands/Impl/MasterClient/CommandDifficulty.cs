using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandDifficulty : Command
    {
        public CommandDifficulty() : base("difficulty", new string[0], "<training/normal/hard/abnormal>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0)
            {
                int difficulty = args[0].ToLower() switch
                {
                    "training" => -1,
                    "normal" => 0,
                    "hard" => 1,
                    "abnormal" => 2,
                    _ => -2
                };

                if (difficulty > -2)
                {
                    GameHelper.Broadcast($"Room difficulty is now {args[0].ToUpper()}!");

                    FengGameManagerMKII.Instance.difficulty = difficulty;
                    IN_GAME_MAIN_CAMERA.Difficulty = difficulty;
                    GameHelper.Broadcast("This change will be effective on the next wave OR game restart.");
                }
            }
        }
    }
}
