namespace Guardian.AntiAbuse.Validators
{
    class AnnieChecker
    {
        // FEMALE_TITAN.netPlayAnimation
        public static bool IsAnimationPlayValid(FEMALE_TITAN annie, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Multiplayer
                || (info != null && annie.photonView.ownerId == info.sender.Id)) return true;

            GuardianClient.Logger.Error($"'FEMALE_TITAN.netPlayAnimation' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // FEMALE_TITAN.netPlayAnimationAt
        public static bool IsAnimationSeekedPlayValid(FEMALE_TITAN annie, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Multiplayer
                || (info != null && annie.photonView.ownerId == info.sender.Id)) return true;

            GuardianClient.Logger.Error($"'FEMALE_TITAN.netPlayAnimationAt' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // FEMALE_TITAN.netCrossFade
        public static bool IsCrossFadeValid(FEMALE_TITAN annie, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Multiplayer
                || (info != null && annie.photonView.ownerId == info.sender.Id)) return true;

            GuardianClient.Logger.Error($"'FEMALE_TITAN.netCrossFade' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }
    }
}
