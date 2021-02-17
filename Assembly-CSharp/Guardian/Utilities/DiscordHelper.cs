namespace Guardian.Utilities
{
    class DiscordHelper
    {
        public static long StartTime;

        private static Discord.Discord DiscordInstance;

        public static void Initialize()
        {
            if (DiscordInstance == null && Mod.Properties.UseRichPresence.Value)
            {
                try
                {
                    DiscordInstance = new Discord.Discord(721934748825550931L, (ulong)Discord.CreateFlags.NoRequireDiscord);

                    DiscordInstance.SetLogHook(Discord.LogLevel.Debug, (logLevel, message) =>
                    {
                        switch (logLevel)
                        {
                            case Discord.LogLevel.Debug:
                            case Discord.LogLevel.Info:
                                Mod.Logger.Info(message);
                                break;
                            case Discord.LogLevel.Warn:
                                Mod.Logger.Warn(message);
                                break;
                            case Discord.LogLevel.Error:
                                Mod.Logger.Error(message);
                                break;
                        }
                    });
                }
                finally { }
            }
        }

        public static void RunCallbacks()
        {
            if (DiscordInstance != null)
            {
                DiscordInstance.RunCallbacks();
            }
        }

        public static void Shutdown()
        {
            if (DiscordInstance != null)
            {
                DiscordInstance.Dispose();
            }
        }

        public static void SetPresence(Discord.Activity activity)
        {
            Initialize();

            if (DiscordInstance != null)
            {
                activity.Assets = new Discord.ActivityAssets
                {
                    LargeImage = "main_icon"
                };

                activity.Timestamps = new Discord.ActivityTimestamps
                {
                    Start = StartTime
                };

                DiscordInstance.GetActivityManager().UpdateActivity(activity, result => { });
            }
        }
    }
}
