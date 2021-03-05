using System.Collections.Generic;
using UnityEngine;

namespace Guardian.Utilities
{
    class Logger
    {
        public List<string> Messages = new List<string>();
        public Vector2 ScrollPosition = GameHelper.ScrollBottom;

        private void Log(string message)
        {
            message = Mod.BlacklistedTags.Replace(message, string.Empty);

            if (message.Length != 0)
            {
                if (Messages.Count > 49)
                {
                    Messages.RemoveAt(0);
                }

                Messages.Add(message);
                ScrollPosition = GameHelper.ScrollBottom;
            }
        }

        public void Info(string message)
        {
            if (Mod.Properties.LogInfo.Value)
            {
                Log("[INFO]: ".WithColor("AAAAAA").AsBold() + message);
            }
        }

        public void Warn(string message)
        {
            if (Mod.Properties.LogWarnings.Value)
            {
                Log("[WARN]: ".WithColor("FFCC00").AsBold() + message);
            }
        }

        public void Error(string message)
        {
            if (Mod.Properties.LogErrors.Value)
            {
                Log("[ERROR]: ".WithColor("FF0000").AsBold() + message);

            }
        }
    }
}
