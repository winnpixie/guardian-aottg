using Guardian.Utilities;
using UnityEngine;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandScatterTitans : Command
    {
        public CommandScatterTitans() : base("scatter", new string[0], string.Empty, true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            foreach (TITAN titan in FengGameManagerMKII.Instance.Titans)
            {
                if (!titan.photonView.isMine) continue;

                object[] point = GameHelper.GetRandomTitanRespawnPoint();
                titan.transform.position = (Vector3)point[0];
                titan.transform.rotation = (Quaternion)point[1];
            }

            GameHelper.Broadcast("All titans have been scattered!");
        }
    }
}
