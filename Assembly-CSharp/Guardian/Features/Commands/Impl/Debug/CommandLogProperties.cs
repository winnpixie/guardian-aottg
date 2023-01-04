using System.IO;
using System.Collections;
using Guardian.Utilities;
using Guardian.AntiAbuse.Validators;

namespace Guardian.Features.Commands.Impl.Debug
{
    class CommandLogProperties : Command
    {
        private readonly string SaveDir = GuardianClient.RootDir + "\\Properties";

        public CommandLogProperties() : base("logpr", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int id)) return;

            PhotonPlayer player = PhotonPlayer.Find(id);
            if (player == null) return;

            string output = $"RoomName={PhotonNetwork.room.name}\n\n";
            foreach (DictionaryEntry entry in player.customProperties)
            {
                if (!NetworkValidator.PropertyWhitelist.Contains(entry.Key))
                {
                    output += "!!! THE FOLLOWING KEY IS NOT A STANDARD PROPERTY !!!\n";
                }
                output += $"Type={entry.Value.GetType().Name}\n\tKey={entry.Key}\n\tValue={entry.Value}\n";
            }

            GameHelper.TryCreateFile(SaveDir, true);

            long time = GameHelper.CurrentTimeMillis();
            File.WriteAllText($"{SaveDir}\\ID{id}_T{time}.txt", output);
            irc.AddLine($"Logged properties of {id} to 'Properties\\ID{id}_T{time}.txt'.");
        }
    }
}
