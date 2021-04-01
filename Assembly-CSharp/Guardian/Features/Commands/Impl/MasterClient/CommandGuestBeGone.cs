using Guardian.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Guardian.Features.Commands.Impl.MasterClient
{
    class CommandGuestBeGone : Command
    {
        private Regex GuestExpression = new Regex("GUEST[-]?[0-9]+", RegexOptions.IgnoreCase);

        public CommandGuestBeGone() : base("guestbegone", new string[] { "gbg" }, string.Empty, true) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            List<string> guestsEliminated = new List<string>();

            foreach (PhotonPlayer player in PhotonNetwork.otherPlayers)
            {
                string name = GExtensions.AsString(player.customProperties[PhotonPlayerProperty.Name]);

                if (GuestExpression.IsMatch(name))
                {
                    FengGameManagerMKII.Instance.KickPlayer(player, false, string.Empty);
                    guestsEliminated.Add(name);
                }
            }

            GameHelper.Broadcast($"Guest-Be-Gone kicked {guestsEliminated.Count} guest(s)!");
            if (guestsEliminated.Count > 0)
            {
                GameHelper.Broadcast($"Guests kicked: " + string.Join(", ", guestsEliminated.Select(name => name.Colored()).ToArray()));
            }
        }
    }
}
