using UnityEngine;
using Guardian.Utilities;

namespace Guardian.AntiAbuse
{
    class FGMPatches
    {
        // FengGameManagerMKII.RequireStatus
        public static bool IsStatusRequestValid(PhotonMessageInfo info)
        {
            if (info != null && !PhotonNetwork.isMasterClient)
            {
                Mod.Logger.Error($"'FengGameManagerMKII.RequireStatus' from #{info.sender.id}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.id);
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
                Mod.Logger.Error($"'FengGameManagerMKII.refreshStatus' from #{(info == null ? "?" : info.sender.id.ToString())}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.id);
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
                Mod.Logger.Error($"'FengGameManagerMKII.titanGetKill' from #{info.sender.id}");
                if (!FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.id);
                }
                return false;
            }
            return true;
        }

        // FengGameManager.netShowDamage
        public static bool IsNetShowDamageValid(PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE)
            {
                if (info != null && (info.sender.isMasterClient || GameHelper.IsPT(info.sender)))
                {
                    return true;
                }
                Mod.Logger.Error($"'FengGameManagerMKII.netShowDamage' from #{(info == null ? "?" : info.sender.id.ToString())}");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.id);
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
                || (GameHelper.IsPT(info.sender) && GameHelper.IsDead(info.sender))
                || (isKillerTitan && damage == 0)
                || (isVictimTitan && damage >= 10)
                || (isKillerTitan == isVictimTitan && damage == 0)))
            {
                return true;
            }
            Mod.Logger.Error($"'FengGameManagerMKII.updateKillInfo' from #{(info == null ? "?" : info.sender.id.ToString())}");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.id);
            }
            return false;
        }
    }
}
