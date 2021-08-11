namespace Guardian.Utilities
{
    class DiscordHelper
    {
        public static long StartTime;

        private static Discord.Discord s_discordInstance;

        public static void Initialize()
        {
            if (s_discordInstance == null && Mod.Properties.UseRichPresence.Value)
            {
                try
                {
                    s_discordInstance = new Discord.Discord(721934748825550931L, (ulong)Discord.CreateFlags.NoRequireDiscord);

                    s_discordInstance.SetLogHook(Discord.LogLevel.Debug, (logLevel, message) =>
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
            if (s_discordInstance != null)
            {
                try
                {
                    s_discordInstance.RunCallbacks();
                }
                finally { }
            }
        }

        public static void Dispose()
        {
            if (s_discordInstance != null)
            {
                try
                {
                    s_discordInstance.GetActivityManager().ClearActivity((result) =>
                    {
                        s_discordInstance.Dispose();
                        s_discordInstance = null;
                    });
                }
                finally { }

            }
        }

        public static void SetPresence(Discord.Activity activity)
        {
            Initialize();

            if (s_discordInstance != null)
            {
                try
                {
                    activity.Assets = new Discord.ActivityAssets
                    {
                        LargeImage = "main_icon",
                        LargeText = "G-Shield by Red"
                    };

                    activity.Timestamps = new Discord.ActivityTimestamps
                    {
                        Start = StartTime
                    };

                    s_discordInstance.GetActivityManager().UpdateActivity(activity, result => { });
                }
                finally { }
            }
        }
    }
}
