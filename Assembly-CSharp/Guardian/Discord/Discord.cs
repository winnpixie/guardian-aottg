namespace Guardian.Discord
{
    class Discord
    {
        public static long StartTimestamp;
        private static bool Connected;

        public static void Connect()
        {
            if (!Connected)
            {
                DiscordRpc.EventHandlers handlers = new DiscordRpc.EventHandlers();
                handlers.readyCallback += (ref DiscordRpc.DiscordUser connectedUser) =>
                {
                    Connected = true;
                    Mod.Logger.Info($"Connected to Discord as {connectedUser.username}#{connectedUser.discriminator}");
                };
                handlers.disconnectedCallback += (int errorCode, string message) =>
                {
                    Connected = false;
                    Mod.Logger.Error($"Disconnected from Discord: \"{message}\" (Error Code {errorCode})");
                };
                handlers.errorCallback += (int errorCode, string message) =>
                {
                    Connected = false;
                    Mod.Logger.Error($"An error occured with the Discord RPC: \"{message}\" (Error Code {errorCode})");
                };
                handlers.joinCallback += (string secret) => { };
                handlers.spectateCallback += (string secret) => { };
                handlers.requestCallback += (ref DiscordRpc.DiscordUser request) =>
                {
                    Mod.Logger.Info($"Join request from {request.username}#{request.discriminator}");
                };
                DiscordRpc.Initialize("721934748825550931", ref handlers, true, "");
            }
        }

        public static void SetPresence(DiscordRpc.RichPresence presence)
        {
            Connect();
            presence.largeImageKey = "main_icon";
            presence.startTimestamp = StartTimestamp;
            DiscordRpc.UpdatePresence(presence);
        }
    }
}
