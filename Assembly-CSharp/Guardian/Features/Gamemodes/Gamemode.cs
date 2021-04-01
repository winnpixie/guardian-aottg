namespace Guardian.Features.Gamemodes
{
    class Gamemode : Feature
    {
        public Gamemode(string name, string[] aliases) : base(name, aliases) { }

        public virtual void OnUpdate() { }

        public virtual void OnReset() { }

        public virtual void OnPlayerJoin(PhotonPlayer player) { }

        public virtual void OnPlayerLeave(PhotonPlayer player) { }

        public virtual void OnPlayerKilled(HERO hero, int killerId, bool wasKilledByTitan) { }

        public virtual void OnTitanKilled(TITAN titan, PhotonPlayer killer, int damage) { }

        public virtual void CleanUp() { }
    }
}
