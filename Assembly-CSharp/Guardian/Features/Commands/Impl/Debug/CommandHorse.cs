using System;

namespace Guardian.Features.Commands.Impl.Debug
{
    class CommandHorse : Command
    {
        public CommandHorse() : base("horse", new string[0], "<action>", false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            if (args.Length < 1) return;

            HERO hero = PhotonNetwork.player.GetHero();
            if (hero == null) return;
            if (hero.myHorse == null) return;

            if (args[0].Equals("follow", StringComparison.OrdinalIgnoreCase))
            {
                hero.myHorse.GetComponent<Horse>().g_shouldFollow = true;
                irc.AddLine("Your horse will now follow you");
            }
            else if (args[0].Equals("stay", StringComparison.OrdinalIgnoreCase))
            {
                hero.myHorse.GetComponent<Horse>().g_shouldFollow = false;
                irc.AddLine("Your horse will no longer follow you");
            }
        }
    }
}
