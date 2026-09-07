using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Guardian.Utilities
{
    public static class EmoteHelper
    {
        public static readonly Dictionary<string, string> Emotes = new Dictionary<string, string>();

        private static readonly string EmoteListPath = GuardianClient.RootDir + "\\Emotes.txt";

        public static void Load()
        {
            RegisterDefaultEmotes();

            if (!File.Exists(EmoteListPath))
            {
                StringBuilder builder = new StringBuilder();
                foreach (KeyValuePair<string, string> emote in Emotes)
                {
                    builder.Append(emote.Key).Append("=").Append(emote.Value).AppendLine();
                }

                File.WriteAllText(EmoteListPath, builder.ToString());
            }

            foreach (string line in File.ReadAllLines(EmoteListPath))
            {
                string[] data = line.Split(new char[] { '=' }, 2);
                if (data[0].IndexOf(':') > -1)
                {
                    GuardianClient.Logger.Warn($"Unable to register emote '{data[0]}', names cannot contain ':'!");
                    continue;
                }

                if (data.Length < 2)
                {
                    GuardianClient.Logger.Warn(
                        $"Unable to register emote '{data[0]}', replacement text MUST be present!");
                    continue;
                }

                if (Emotes.ContainsKey(data[0]))
                {
                    GuardianClient.Logger.Warn($"Unable to register emote '{data[0]}', entry already exists (default emote?)!");
                    continue;
                }

                Emotes.Add(data[0], data[1]);
            }

            GuardianClient.Logger.Debug($"Registered {Emotes.Count} emotes.");
        }

        private static void RegisterDefaultEmotes()
        {
            Emotes.Add("heart", "\u2665".AsColor("B00B1E"));
            Emotes.Add("lenny", "( ͡° ͜ʖ ͡°)");
            Emotes.Add("shrug", "¯\\_(ツ)_/¯");
            Emotes.Add("table_flip", "(╯°□°)╯︵ ┻━┻");
            Emotes.Add("table_unflip", "┬─┬ノ( º _ ºノ)");
        }

        public static string FormatText(string text)
        {
            foreach (KeyValuePair<string, string> emote in Emotes)
            {
                text = text.Replace($":{emote.Key}:", emote.Value);
            }

            return text.Replace("<3", "\u2665".AsColor("B00B1E")); // Hardcoded emote <3
        }
    }
}