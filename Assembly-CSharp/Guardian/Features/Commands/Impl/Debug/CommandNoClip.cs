using Guardian.Utilities;
namespace Guardian.Features.Commands.Impl.Debug
{
    class CommandNoClip : Command
    {
        public CommandNoClip() : base("noclip", new string[0], "", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (!GameHelper.IsDead(PhotonNetwork.player) && !GameHelper.IsPT(PhotonNetwork.player))
            {
                HERO hero = GameHelper.GetHero(PhotonNetwork.player);
                hero.gameObject.rigidbody.detectCollisions = !hero.gameObject.rigidbody.detectCollisions;
                irc.AddLine($"NoClip is now {(hero.gameObject.rigidbody.detectCollisions ? "OFF" : "ON")}.");
            }
        }
    }
}
