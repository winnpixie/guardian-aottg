using Guardian.Utilities;
using System.Collections.Generic;

namespace Guardian.Features.Commands.Impl
{
    class CommandEmotes : Command
    {
        public CommandEmotes() : base("emotes", new string[0], string.Empty, false) { }

        public override void Execute(InRoomChat irc, string[] args)
        {
            irc.AddLine("Available Text Chat Emotes".AsColor("AAFF00"));

            foreach (KeyValuePair<string, string> emote in EmoteHelper.Emotes)
            {
                irc.AddLine($":{emote.Key}:".AsBold() + " = " + emote.Value);
            }
        }
    }
}
