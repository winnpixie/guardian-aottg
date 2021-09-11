using Guardian.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Guardian
{
    class Logger
    {
        public Dictionary<LogType, List<string>> EntryDict = new Dictionary<LogType, List<string>>();
        public Vector2 ScrollPosition = GameHelper.ScrollBottom;

        private void Log(LogType type, string message)
        {
            message = Mod.BlacklistedTags.Replace(message, string.Empty);

            if (message.Length > 0)
            {
                List<string> entries;

                if (!EntryDict.TryGetValue(type, out entries))
                {
                    EntryDict.Add(type, entries = new List<string>());
                }

                entries.Add(message);

                if (entries.Count > Mod.Properties.MaxLogLines.Value)
                {
                    entries.RemoveAt(0);
                }

                ScrollPosition = GameHelper.ScrollBottom;
            }
        }

        public void Info(string message)
        {
            Log(LogType.Info, "* ".AsColor("AAAAAA").AsBold() + message);
        }

        public void Warn(string message)
        {
            Log(LogType.Warnings, "* ".AsColor("FFCC00").AsBold() + message);
        }

        public void Error(string message)
        {
            Log(LogType.Errors, "* ".AsColor("FF0000").AsBold() + message);
        }

        public enum LogType
        {
            Info, Warnings, Errors
        }
    }
}
