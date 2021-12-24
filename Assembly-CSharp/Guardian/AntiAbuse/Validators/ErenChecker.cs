namespace Guardian.AntiAbuse.Validators
{
    class ErenChecker
    {
        // TITAN_EREN.netPlayAnimation
        public static bool IsAnimationPlayValid(TITAN_EREN eren, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Multiplayer ||
                (info != null && eren.photonView.ownerId == info.sender.Id)) return true;

            Mod.Logger.Error($"'TITAN_EREN.netPlayAnimation' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // TITAN_EREN.netPlayAnimationAt
        public static bool IsAnimationSeekedPlayValid(TITAN_EREN eren, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Multiplayer ||
                (info != null && eren.photonView.ownerId == info.sender.Id)) return true;

            Mod.Logger.Error($"'TITAN_EREN.netPlayAnimationAt' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // TITAN_EREN.netCrossFade
        public static bool IsCrossFadeValid(TITAN_EREN eren, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Multiplayer ||
                (info != null && eren.photonView.ownerId == info.sender.Id)) return true;

            Mod.Logger.Error($"'TITAN_EREN.netCrossFade' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // TITAN_EREN.removeMe
        public static bool IsRemovalValid(PhotonMessageInfo info)
        {
            if (info == null) return true;

            Mod.Logger.Error($"'TITAN_EREN.removeMe' from #{info.sender.Id}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }
    }
}
