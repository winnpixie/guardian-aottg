using Discord;

namespace Guardian.Utilities
{
    public static class DiscordHelper
    {
        private static Discord.Discord _discord;
        private static long _startTime = -1L;

        public static void Initialize()
        {
            if (_startTime == -1L) _startTime = GameHelper.CurrentTimeMillis();
            
            if (_discord != null) return;
            if (!GuardianClient.Properties.UseRichPresence.Value) return;

            try
            {
                _discord = new Discord.Discord(721934748825550931L, (ulong)CreateFlags.NoRequireDiscord);

                _discord.SetLogHook(LogLevel.Debug, (logLevel, message) =>
                {
                    switch (logLevel)
                    {
                        case LogLevel.Debug:
                            GuardianClient.Logger.Debug(message);
                            break;
                        case LogLevel.Info:
                            GuardianClient.Logger.Info(message);
                            break;
                        case LogLevel.Warn:
                            GuardianClient.Logger.Warn(message);
                            break;
                        case LogLevel.Error:
                            GuardianClient.Logger.Error(message);
                            break;
                    }
                });

                _discord.GetUserManager().OnCurrentUserUpdate += () =>
                {
                    GuardianClient.Logger.Debug("Connected to Discord for Rich Presence.");
                };
            }
            catch
            {
            }
        }

        public static void RunCallbacks()
        {
            if (_discord == null) return;

            try
            {
                _discord.RunCallbacks();
            }
            catch
            {
            }
        }

        public static void Dispose()
        {
            if (_discord == null) return;

            try
            {
                _discord.GetActivityManager().ClearActivity(result =>
                {
                    _discord.Dispose();
                    _discord = null;
                });
            }
            catch
            {
            }
        }

        public static void SetPresence(Activity activity)
        {
            Initialize();

            if (_discord == null) return;

            try
            {
                activity.Assets = new ActivityAssets
                {
                    LargeImage = "main_icon",
                    LargeText = "G-Shield by Red"
                };

                activity.Timestamps = new ActivityTimestamps
                {
                    Start = _startTime
                };

                _discord.GetActivityManager().UpdateActivity(activity, result => { });
            }
            catch
            {
            }
        }
    }
}