namespace Guardian.AntiAbuse
{
    class HorsePatches
    {
        // Horse.netPlayAnimation
        public static bool IsAnimationPlayValid(Horse horse, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info == null || horse.photonView.ownerId != info.sender.Id)
                {
                    Mod.Logger.Error($"'Horse.netPlayAnimation' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }
                    return false;
                }
            }
            return true;
        }

        // Horse.netPlayAnimationAt
        public static bool IsAnimationSeekedPlayValid(Horse horse, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info == null || horse.photonView.ownerId != info.sender.Id)
                {
                    Mod.Logger.Error($"'Horse.netPlayAnimationAt' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }
                    return false;
                }
            }
            return true;
        }

        // Horse.netCrossFade
        public static bool IsCrossFadeValid(Horse horse, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info == null || horse.photonView.ownerId != info.sender.Id)
                {
                    Mod.Logger.Error($"'Horse.netCrossFade' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
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
