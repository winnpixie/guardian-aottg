namespace Guardian.AntiAbuse
{
    class TitanPatches
    {
        // TITAN.netSetAbnormalType
        public static bool IsTitanTypeSetValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE)
            {
                if (info == null || titan.photonView.ownerId != info.sender.id)
                {
                    Mod.Logger.Error($"'netSetAbnormalType' from #{(info == null ? "?" : info.sender.id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.id);
                    }
                    return false;
                }
            }
            return true;
        }

        // TITAN.netCrossFade
        public static bool IsCrossFadeValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE)
            {
                if (info == null || titan.photonView.ownerId != info.sender.id)
                {
                    Mod.Logger.Error($"'netCrossFade' from #{(info == null ? "?" : info.sender.id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.id);
                    }
                    return false;
                }
            }
            return true;
        }

        // TITAN.netPlayAnimation
        public static bool IsAnimationPlayValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE)
            {
                if (info == null || titan.photonView.ownerId != info.sender.id)
                {
                    Mod.Logger.Error($"'netPlayAnimation' from #{(info == null ? "?" : info.sender.id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.id);
                    }
                    return false;
                }
            }
            return true;
        }

        // TITAN.netPlayAnimationAt
        public static bool IsAnimationSeekedPlayValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE)
            {
                if (info == null || titan.photonView.ownerId != info.sender.id)
                {
                    Mod.Logger.Error($"'netPlayAnimationAt' from #{(info == null ? "?" : info.sender.id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.id);
                    }
                    return false;
                }
            }
            return true;
        }

        // TITAN.setMyTarget
        public static bool IsTargetSetValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE)
            {
                if (info == null || titan.photonView.ownerId != info.sender.id)
                {
                    Mod.Logger.Error($"'setMyTarget' from #{(info == null ? "?" : info.sender.id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.id);
                    }
                    return false;
                }
            }
            return true;
        }

        // TITAN.playsoundRPC
        public static bool IsSoundPlayValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE)
            {
                if (info != null && titan.photonView.ownerId != info.sender.id)
                {
                    Mod.Logger.Error($"'playsoundRPC' from #{(info == null ? "?" : info.sender.id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.id);
                    }
                    return false;
                }
            }
            return true;
        }

        // TITAN.grabToRight
        public static bool IsRightGrabValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE)
            {
                if (info != null && titan.photonView.ownerId != info.sender.id)
                {
                    Mod.Logger.Error($"'grabToRight' from #{info.sender.id}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.id);
                    }
                    return false;
                }
            }
            return true;
        }

        // TITAN.grabToLeft
        public static bool IsLeftGrabValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE)
            {
                if (info != null && titan.photonView.ownerId != info.sender.id)
                {
                    Mod.Logger.Error($"'grabToLeft' from #{info.sender.id}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.id);
                    }
                    return false;
                }
            }
            return true;
        }

        // TITAN.netSetLevel
        public static bool IsLevelSetValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GAMETYPE.SINGLE)
            {
                if (info == null || titan.photonView.ownerId != info.sender.id)
                {
                    Mod.Logger.Error($"'netSetLevel' from #{(info == null ? "?" : info.sender.id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.id);
                    }
                    return false;
                }
            }
            return true;
        }
    }
}
