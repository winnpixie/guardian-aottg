using Guardian.Utilities;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandKill : Command
    {
        public CommandKill() : base("kill", new string[0], "<id>", true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int id))
            {
                var player = PhotonPlayer.Find(id);
                if (player != null)
                {
                    if (GameHelper.IsPT(player))
                    {
                        var titan = GameHelper.GetPT(player);
                        if (titan != null)
                        {
                            titan.photonView.RPC("titanGetHit", player, titan.photonView.viewID, 10);
                        }
                    }
                    else
                    {
                        var hero = GameHelper.GetHero(player);
                        if (hero != null)
                        {
                            hero.photonView.RPC("netDie", PhotonTargets.All, hero.transform.position, false, -1, "[FF0000]Server", true);
                        }
                    }
                }
            }
        }
    }
}
