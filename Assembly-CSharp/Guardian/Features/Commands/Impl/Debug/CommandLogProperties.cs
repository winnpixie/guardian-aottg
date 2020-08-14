using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine;
using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.Debug
{
    class CommandLogProperties : Command
    {
        private string SaveDirectory;

        public CommandLogProperties() : base("logpr", new string[0], "<id>", false)
        {
            SaveDirectory = $"{Application.dataPath}\\..\\Properties";
            GameHelper.TryCreateFile(SaveDirectory, true);
        }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                PhotonPlayer player = PhotonPlayer.Find(id);
                if (player != null)
                {
                    IEnumerator<DictionaryEntry> ienum = player.customProperties.GetEnumerator();
                    string output = "";
                    while (ienum.MoveNext())
                    {
                        DictionaryEntry entry = ienum.Current;
                        output = $"{output}({entry.Value.GetType().Name}) {entry.Key}:{entry.Value}\n";
                    }

                    GameHelper.TryCreateFile(SaveDirectory, true);
                    File.WriteAllText($"{SaveDirectory}\\Properties_{id}.txt", output);
                    irc.AddLine($"Logged properties of {id} to 'Properties\\Properties_{id}.txt'.");
                }
            }
        }
    }
}
