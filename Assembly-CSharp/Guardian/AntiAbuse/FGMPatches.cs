using Guardian.Utilities;

namespace Guardian.AntiAbuse
{
    class FGMPatches
    {
        // FengGameManagerMKII.pauseRPC
        public static bool IsPauseStateChangeValid(PhotonMessageInfo info)
        {
            if (info == null || !info.sender.isMasterClient)
            {
                Mod.Logger.Error($"'FengGameManagerMKII.pauseRPC' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }

                return false;
            }

            return true;
        }

        // FengGameManagerMKII.RequireStatus
        public static bool IsStatusRequestValid(PhotonMessageInfo info)
        {
            if (info != null && !PhotonNetwork.isMasterClient)
            {
                Mod.Logger.Error($"'FengGameManagerMKII.RequireStatus' from #{info.sender.Id}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }

                return false;
            }

            return true;
        }

        // FengGameManagerMKII.refreshStatus
        public static bool IsStatusRefreshValid(PhotonMessageInfo info)
        {
            if (info == null || !info.sender.isMasterClient)
            {
                Mod.Logger.Error($"'FengGameManagerMKII.refreshStatus' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }

                return false;
            }

            return true;
        }

        // FengGameManangerMKII.refreshPVPStatus
        public static bool IsPVPStatusRefreshValid(PhotonMessageInfo info)
        {
            if (info == null || !(info.sender.isMasterClient || FengGameManagerMKII.Level.Mode == GameMode.PvPCapture))
            {
                Mod.Logger.Error($"'FengGameManagerMKII.refreshPVPStatus' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }

                return false;
            }

            return true;
        }

        // FengGameManagerMKII.refreshPVPStatus_AHSS
        public static bool IsAHSSStatusRefreshValid(PhotonMessageInfo info)
        {
            if (info == null || !info.sender.isMasterClient)
            {
                Mod.Logger.Error($"'FengGameManagerMKII.refreshPVPStatus_AHSS' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }

                return false;
            }

            return true;
        }

        // FengGameManagerMKII.titanGetKill
        public static bool IsTitanKillValid(PhotonMessageInfo info)
        {
            if (info != null)
            {
                Mod.Logger.Error($"'FengGameManagerMKII.titanGetKill' from #{info.sender.Id}");
                if (!FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }

                return false;
            }

            return true;
        }

        // FengGameManager.netShowDamage
        public static bool IsNetShowDamageValid(PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer
                && (info == null || !(info.sender.isMasterClient || GameHelper.IsPT(info.sender))))
            {
                Mod.Logger.Error($"'FengGameManagerMKII.netShowDamage' from #{(info == null ? "?" : info.sender.Id.ToString())}");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }

                return false;
            }

            return true;
        }

        // FengGameManagerMKII.updateKillInfo
        public static bool IsKillInfoUpdateValid(bool isKillerTitan, bool isVictimTitan, int damage, PhotonMessageInfo info)
        {
            if (info != null
                && (info.sender.isMasterClient
                || info.sender.isLocal
                || (isKillerTitan && damage == 0)
                || (isVictimTitan && (damage >= 10 || GameHelper.IsPT(info.sender)))
                || (isKillerTitan == isVictimTitan && damage == 0)))
            {
                return true;
            }

            Mod.Logger.Error($"'FengGameManagerMKII.updateKillInfo' from #{(info == null ? "?" : info.sender.Id.ToString())}");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // FengGameManagerMKII.showChatContent
        public static bool IsChatContentShowValid(PhotonMessageInfo info)
        {
            if (info != null)
            {
                Mod.Logger.Error($"'FengGameManagerMKII.showChatContent' from #{info.sender.Id}");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }

                return false;
            }

            return true;
        }
    }
}
