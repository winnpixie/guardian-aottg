namespace Guardian.Utilities
{
    class DiscordHelper
    {
        public static long StartTime;

        private static Discord.Discord _Discord;

        public static void Initialize()
        {
            if (_Discord != null) return;
            if (!GuardianClient.Properties.UseRichPresence.Value) return;

            try
            {
                _Discord = new Discord.Discord(721934748825550931L, (ulong)Discord.CreateFlags.NoRequireDiscord);

                _Discord.SetLogHook(Discord.LogLevel.Debug, (logLevel, message) =>
                {
                    switch (logLevel)
                    {
                        case Discord.LogLevel.Debug:
                            GuardianClient.Logger.Debug(message);
                            break;
                        case Discord.LogLevel.Info:
                            GuardianClient.Logger.Info(message);
                            break;
                        case Discord.LogLevel.Warn:
                            GuardianClient.Logger.Warn(message);
                            break;
                        case Discord.LogLevel.Error:
                            GuardianClient.Logger.Error(message);
                            break;
                    }
                });

                _Discord.GetUserManager().OnCurrentUserUpdate += () =>
                {
                    GuardianClient.Logger.Debug($"Connected to Discord for Rich Presence.");
                };
            }
            catch { }
        }

        public static void RunCallbacks()
        {
            if (_Discord == null) return;

            try
            {
                _Discord.RunCallbacks();
            }
            catch { }
        }

        public static void Dispose()
        {
            if (_Discord == null) return;

            try
            {
                _Discord.GetActivityManager().ClearActivity((result) =>
                {
                    _Discord.Dispose();
                    _Discord = null;
                });
            }
            catch { }
        }

        public static void SetPresence(Discord.Activity activity)
        {
            Initialize();

            if (_Discord == null) return;

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

                _Discord.GetActivityManager().UpdateActivity(activity, result => { });
            }
            catch { }
        }
    }
}
