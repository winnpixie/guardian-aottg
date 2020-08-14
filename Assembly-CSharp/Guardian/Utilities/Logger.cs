using System.Collections.Generic;

namespace Guardian.Utilities
{
    class Logger
    {
        public List<string> Messages = new List<string>();

        private void Log(string message)
        {
            message = Mod.BlacklistedTags.Replace(message, "");
            if (message.Length != 0)
            {
                Messages.Add(message);
            }
        }

        public void Info(string message)
        {
            if (Mod.Properties.LogInfo.Value)
            {
                Log("[INFO]: ".WithColor("aaaaaa").AsBold() + message);
            }
        }

        public void Warn(string message)
        {
            if (Mod.Properties.LogWarnings.Value)
            {
                Log("[WARN]: ".WithColor("ffcc00").AsBold() + message);
            }
        }

        public void Error(string message)
        {
            if (Mod.Properties.LogErrors.Value)
            {
                Log("[ERROR]: ".WithColor("ff0000").AsBold() + message);

            }
        }
    }
}
