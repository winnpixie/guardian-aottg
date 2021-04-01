namespace Guardian.AntiAbuse
{
    class TitanPatches
    {
        // TITAN.netSetAbnormalType
        public static bool IsTitanTypeSetValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info == null || (titan.photonView.ownerId != info.sender.Id && !info.sender.isMasterClient))
                {
                    Mod.Logger.Error($"'TITAN.netSetAbnormalType' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }

                    return false;
                }
            }

            return true;
        }

        // TITAN.netCrossFade
        public static bool IsCrossFadeValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info == null || titan.photonView.ownerId != info.sender.Id)
                {
                    Mod.Logger.Error($"'TITAN.netCrossFade' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }

                    return false;
                }
            }

            return true;
        }

        // TITAN.netPlayAnimation
        public static bool IsAnimationPlayValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info == null || titan.photonView.ownerId != info.sender.Id)
                {
                    Mod.Logger.Error($"'TITAN.netPlayAnimation' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }

                    return false;
                }
            }

            return true;
        }

        // TITAN.netPlayAnimationAt
        public static bool IsAnimationSeekedPlayValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info == null || titan.photonView.ownerId != info.sender.Id)
                {
                    Mod.Logger.Error($"'TITAN.netPlayAnimationAt' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }

                    return false;
                }
            }

            return true;
        }

        // TITAN.setMyTarget
        public static bool IsTargetSetValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info == null || titan.photonView.ownerId != info.sender.Id)
                {
                    Mod.Logger.Error($"'TITAN.setMyTarget' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }

                    return false;
                }
            }

            return true;
        }

        // TITAN.playsoundRPC
        public static bool IsSoundPlayValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info != null && titan.photonView.ownerId != info.sender.Id)
                {
                    Mod.Logger.Error($"'TITAN.playsoundRPC' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }

                    return false;
                }
            }

            return true;
        }

        // TITAN.grabToRight
        public static bool IsRightGrabValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info != null && titan.photonView.ownerId != info.sender.Id)
                {
                    Mod.Logger.Error($"'TITAN.grabToRight' from #{info.sender.Id}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }

                    return false;
                }
            }

            return true;
        }

        // TITAN.grabToLeft
        public static bool IsLeftGrabValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info != null && titan.photonView.ownerId != info.sender.Id)
                {
                    Mod.Logger.Error($"'TITAN.grabToLeft' from #{info.sender.Id}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }

                    return false;
                }
            }

            return true;
        }

        // TITAN.netSetLevel
        public static bool IsLevelSetValid(TITAN titan, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info == null || (titan.photonView.ownerId != info.sender.Id && !info.sender.isMasterClient))
                {
                    Mod.Logger.Error($"'TITAN.netSetLevel' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }

                    return false;
                }
            }

            return true;
        }
    }
}
