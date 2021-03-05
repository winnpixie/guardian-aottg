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
                var difficulty = -2;
                switch (args[0].ToLower())
                {
                    case "training":
                        difficulty = -1;
                        GameHelper.Broadcast("Room difficulty is now TRAINING!");
                        break;
                    case "normal":
                        difficulty = 0;
                        GameHelper.Broadcast("Room difficulty is now NORMAL!");
                        break;
                    case "hard":
                        difficulty = 1;
                        GameHelper.Broadcast("Room difficulty is now HARD!");
                        break;
                    case "abnormal":
                        difficulty = 2;
                        GameHelper.Broadcast("Room difficulty is now ABNORMAL!");
                        break;
                    default:
                        irc.AddLine("Invalid difficulty level!".WithColor("FF0000"));
                        break;
                }

                if (difficulty != -2)
                {
                    FengGameManagerMKII.Instance.difficulty = difficulty;
                    IN_GAME_MAIN_CAMERA.Difficulty = difficulty;
                    GameHelper.Broadcast("This change will be effective on the next wave OR game restart.");
                }
            }
        }
    }
}
