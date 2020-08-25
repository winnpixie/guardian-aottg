using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl
{
    class CommandTeleport : Command
    {
        public CommandTeleport() : base("teleport", new string[] { "tp" }, "<id>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                PhotonPlayer player = PhotonPlayer.Find(id);
                if (player != null)
                {
                    Photon.MonoBehaviour target = GameHelper.IsPT(player) ? (Photon.MonoBehaviour)GameHelper.GetPT(player) : (Photon.MonoBehaviour)GameHelper.GetHero(player);
                    Photon.MonoBehaviour you = GameHelper.IsPT(PhotonNetwork.player) ? (Photon.MonoBehaviour)GameHelper.GetPT(PhotonNetwork.player)
                        : (Photon.MonoBehaviour)GameHelper.GetHero(PhotonNetwork.player);
                    if (you != null && target != null)
                    {
                        you.rigidbody.transform.position = target.rigidbody.transform.position;
                        you.rigidbody.velocity = new UnityEngine.Vector3(0, 0, 0);
                        irc.AddLine($"Teleported to #{id}.");
                    }
                }
            }
        }
    }
}
