using Guardian.Features.Gamemodes.Impl;

namespace Guardian.Features.Gamemodes
{
    class GamemodeManager : FeatureManager<Gamemode>
    {
        public Gamemode Current;

        public override void Load()
        {
            Gamemode normal = new Gamemode("Normal", new string[] { "none" });
            Current = normal;
            base.Add(normal);

            base.Add(new TimeBomb());
            base.Add(new LastManStanding());
        }
    }
}
