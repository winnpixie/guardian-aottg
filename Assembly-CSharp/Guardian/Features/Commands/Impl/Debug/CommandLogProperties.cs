using System;
using System.IO;
using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.Debug
{
    class CommandLogProperties : Command
    {
        private string SaveDir = Mod.RootDir + "\\Properties";

        public CommandLogProperties() : base("logpr", new string[0], "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                var player = PhotonPlayer.Find(id);
                if (player != null)
                {
                    var ienum = player.customProperties.GetEnumerator();
                    var output = string.Empty;

                    while (ienum.MoveNext())
                    {
                        var entry = ienum.Current;
                        output = $"{output}({entry.Value.GetType().Name}) {entry.Key}:{entry.Value}{Environment.NewLine}";
                    }

                    GameHelper.TryCreateFile(SaveDir, true);
                    File.WriteAllText($"{SaveDir}\\Properties_{id}.txt", output);
                    irc.AddLine($"Logged properties of {id} to 'Properties\\Properties_{id}.txt'.");
                }
            }
        }
    }
}
