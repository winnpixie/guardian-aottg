using Guardian.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Guardian
{
    class Logger
    {
        public List<string> Entries = new List<string>();
        public Vector2 ScrollPosition = GameHelper.ScrollBottom;

        private void Log(string message)
        {
            message = Mod.BlacklistedTags.Replace(message, string.Empty);

            if (message.Length > 0)
            {
                Entries.Add(message);

                if (Entries.Count > Mod.Properties.MaxLogLines.Value)
                {
                    Entries.RemoveAt(0);
                }

                ScrollPosition = GameHelper.ScrollBottom;
            }
        }

        public void Info(string message)
        {
            Log("* ".AsColor("AAAAAA") + message);
        }

        public void Warn(string message)
        {
            Log("* ".AsColor("FFCC00") + message);
        }

        public void Error(string message)
        {
            Log("* ".AsColor("FF0000") + message);
        }

        public void Debug(string message)
        {
            Log("* ".AsColor("00FFFF") + message);
        }
    }
}
