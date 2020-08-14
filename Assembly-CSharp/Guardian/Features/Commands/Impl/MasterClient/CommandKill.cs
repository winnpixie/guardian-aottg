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
                PhotonPlayer player = PhotonPlayer.Find(id);
                if (player != null)
                {
                    if (GameHelper.IsPT(player))
                    {
                        TITAN titan = GameHelper.GetPT(player);
                        if (titan != null)
                        {
                            titan.photonView.RPC("titanGetHit", player, titan.photonView.viewID, 10);
                        }
                    }
                    else
                    {
                        HERO hero = GameHelper.GetHero(player);
                        if (hero != null)
                        {
                            hero.photonView.RPC("netDie", PhotonTargets.All, hero.transform.position, false, -1, "[ff0000]Server", true);
                        }
                    }
                }
            }
        }
    }
}
