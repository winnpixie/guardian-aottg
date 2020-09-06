using UnityEngine;
using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl
{
    class CommandTime : Command
    {
        public CommandTime() : base("time", new string[0], "<day/dawn/night>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && GExtensions.TryParseEnum(args[0], out DayLight dayLight))
            {
                IN_GAME_MAIN_CAMERA.DayLight = dayLight;

                if (PhotonNetwork.isMasterClient)
                {
                    PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                    {
                        { "DayLight", args[0].ToUpper() }
                    });
                    GameHelper.Broadcast($"The current map lighting is now {args[0].ToUpper()}!");
                }
                else
                {
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setDayLight(dayLight);
                    irc.AddLine($"Map lighting is now {args[0].ToUpper()}.");
                }
            }
        }
    }
}