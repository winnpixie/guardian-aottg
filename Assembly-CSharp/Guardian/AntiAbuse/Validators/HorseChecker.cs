namespace Guardian.AntiAbuse.Validators
{
    class HorseChecker
    {
        // Horse.netPlayAnimation
        public static bool IsAnimationPlayValid(Horse horse, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer
                || (info != null && horse.photonView.ownerId == info.sender.Id)) return true;

            GuardianClient.Logger.Error($"'Horse.netPlayAnimation' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // Horse.netPlayAnimationAt
        public static bool IsAnimationSeekedPlayValid(Horse horse, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer
                || (info != null && horse.photonView.ownerId == info.sender.Id)) return true;

            GuardianClient.Logger.Error($"'Horse.netPlayAnimationAt' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // Horse.netCrossFade
        public static bool IsCrossFadeValid(Horse horse, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer
                || (info != null && horse.photonView.ownerId == info.sender.Id)) return true;

            GuardianClient.Logger.Error($"'Horse.netCrossFade' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }
    }
}
