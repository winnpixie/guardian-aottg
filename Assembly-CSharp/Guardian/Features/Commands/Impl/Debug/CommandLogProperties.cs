using System;
using System.IO;
using System.Collections;
using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.Debug
{
    class CommandLogProperties : Command
    {
        private string SaveDir = GuardianClient.RootDir + "\\Properties";

        public CommandLogProperties() : base("logpr", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int id)) return;

            PhotonPlayer player = PhotonPlayer.Find(id);
            if (player == null) return;

            string output = string.Empty;
            foreach (DictionaryEntry entry in player.customProperties)
            {
                output = $"{output}({entry.Value.GetType().Name}) {entry.Key}={entry.Value}{Environment.NewLine}";
            }

            GameHelper.TryCreateFile(SaveDir, true);
            File.WriteAllText($"{SaveDir}\\Properties_{id}.txt", output);
            irc.AddLine($"Logged properties of {id} to 'Properties\\Properties_{id}.txt'.");
        }
    }
}
