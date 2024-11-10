using ExitGames.Client.Photon;

namespace Guardian.Networking
{
    internal class SyncedSettings
    {
        public static bool InfiniteGas;
        public static bool InfiniteAmmo;

        public static void ApplySettings(Hashtable gameSettings)
        {
            gameSettings["g_infgas"] = GuardianClient.Properties.InfiniteGas.Value ? 1 : 0;
            gameSettings["g_infammo"] = GuardianClient.Properties.InfiniteAmmo.Value ? 1 : 0;
        }

        public static void UpdateSettings(Hashtable gameSettings)
        {
            if (gameSettings.ContainsKey("g_infgas") && gameSettings["g_infgas"] is int infGas)
            {
                InfiniteGas = infGas == 1;

                InRoomChat.Instance.AddLine("Infinite Gas is ".AsColor("FFCC00") + (InfiniteGas ? "ENABLED" : "DISABLED"));
            }

            if (gameSettings.ContainsKey("g_infammo") && gameSettings["g_infammo"] is int infAmmo) {
                InfiniteAmmo = infAmmo == 1;

                InRoomChat.Instance.AddLine("Infinite Blade + Gun Ammo is ".AsColor("FFCC00") + (InfiniteGas ? "ENABLED" : "DISABLED"));
            }
        }
    }
}
