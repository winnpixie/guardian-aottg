using UnityEngine;
using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl
{
    class CommandSetLighting : Command
    {
        public CommandSetLighting() : base("setlighting", new string[] { "lighting", "settime", "time" }, "<day/dawn/night>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1 || !GExtensions.TryParseEnum(args[0], out DayLight dayLight)) return;

            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                {
                    { "Lighting", args[0].ToUpper() }
                });
                GameHelper.Broadcast($"The current map lighting is now {args[0].ToUpper()}!");
            }
            else
            {
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetLighting(dayLight);
                irc.AddLine($"Map lighting is now {args[0].ToUpper()}.");
            }
        }
    }
}