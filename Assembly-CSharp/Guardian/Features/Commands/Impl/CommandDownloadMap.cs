using Guardian.Utilities;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Guardian.Features.Commands.Impl
{
    class CommandDownloadMap : Command
    {
        private Regex illegalPathChars = new Regex("[\\\\/:*?\"<>|]"); // According to Windows: \/:*?"<>|
        private string saveDirectory;

        public CommandDownloadMap() : base("dlmap", new string[0], "", false)
        {
            saveDirectory = $"{Application.dataPath}\\..\\Maps";
            GameHelper.TryCreateFile(saveDirectory, true);
        }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (Mod.MapData.Length != 0 && PhotonNetwork.inRoom)
            {
                string[] roomInfo = PhotonNetwork.room.name.Split('`');
                if (roomInfo.Length > 6)
                {
                    string roomName = roomInfo[0];
                    foreach (Match match in illegalPathChars.Matches(roomName))
                    {
                        roomName = roomName.Replace(match.Value, "");
                    }
                    long now = GameHelper.CurrentTimeMillis();
                    GameHelper.TryCreateFile(saveDirectory, true);
                    File.WriteAllText($"{saveDirectory}\\{roomName}_{now}.txt", Mod.MapData);
                    irc.AddLine($"Saved map to 'Maps\\{roomName}_{now}.txt'.");
                }
            }
        }
    }
}
