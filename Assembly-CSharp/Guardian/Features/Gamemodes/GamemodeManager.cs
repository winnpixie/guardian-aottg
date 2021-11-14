using Guardian.Features.Gamemodes.Impl;

namespace Guardian.Features.Gamemodes
{
    class GamemodeManager : FeatureManager<Gamemode>
    {
        public Gamemode DefaultMode;
        public Gamemode CurrentMode;

        public override void Load()
        {
            base.Add(DefaultMode = new Gamemode("Normal", new string[] { "none" }));

            base.Add(new CageFight());
            base.Add(new LastManStanding());
            base.Add(new TimeBomb());

            CurrentMode = DefaultMode;
        }
    }
}
