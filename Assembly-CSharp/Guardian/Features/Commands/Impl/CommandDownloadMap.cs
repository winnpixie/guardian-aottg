using Guardian.Utilities;
using System.IO;
using System.Text.RegularExpressions;

namespace Guardian.Features.Commands.Impl
{
    class CommandDownloadMap : Command
    {
        private Regex IllegalPathChars = new Regex("[\\\\/:*?\"<>|]", RegexOptions.IgnoreCase); // According to Windows: \/:*?"<>|
        private string SaveDir = Mod.RootDir + "\\Maps";

        public CommandDownloadMap() : base("downloadmap", new string[] { "dlmap" }, string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (Mod.MapData.Length != 0 && PhotonNetwork.inRoom)
            {
                string[] roomInfo = PhotonNetwork.room.name.Split('`');
                string roomName = roomInfo[0];
                roomName = IllegalPathChars.Replace(roomName, string.Empty);
                long now = GameHelper.CurrentTimeMillis();

                GameHelper.TryCreateFile(SaveDir, true);
                File.WriteAllText($"{SaveDir}\\{roomName}_{now}.txt", Mod.MapData);
                irc.AddLine($"Saved map to 'Maps\\{roomName}_{now}.txt'.");
            }
        }
    }
}
