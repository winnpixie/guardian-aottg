namespace Guardian.AntiAbuse.Validators
{
    class HookChecker
    {
        // Bullet.killObject
        public static bool IsKillObjectValid(PhotonMessageInfo info)
        {
            if (info == null) return true;

            GuardianClient.Logger.Error($"'Bullet.killObject' from #{info.sender.Id}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // Bullet.myMasterIs
        public static bool IsHookMasterSetValid(Bullet hook, int viewId, PhotonMessageInfo info)
        {
            PhotonView view = PhotonView.Find(viewId);
            if (info != null && view != null
                && hook.photonView.ownerId == info.sender.Id
                && view.gameObject.GetComponent<HERO>() != null) return true;

            GuardianClient.Logger.Warn($"'Bullet.myMasterIs' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            return false;
        }

        // Bullet.tieMeToOBJ
        public static bool IsHookTieValid(Bullet hook, int viewId, PhotonMessageInfo info)
        {
            PhotonView view = PhotonView.Find(viewId);
            if (info != null && view != null
                && hook.photonView.ownerId == info.sender.Id) return true;

            GuardianClient.Logger.Warn($"'Bullet.tieMeToOBJ' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            return false;

        }

        // Bullet.netLaunch
        public static bool IsLaunchValid(PhotonMessageInfo info)
        {
            if (info == null) return true;

            GuardianClient.Logger.Error($"'Bullet.netLaunch' from #{info.sender.Id}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // Bullet.netUpdatePhase1
        public static bool IsPhaseUpdateValid(PhotonMessageInfo info)
        {
            if (info == null) return true;

            GuardianClient.Logger.Error($"'Bullet.netUpdatePhase1' from #{info.sender.Id}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // Bullet.netUpdateLeviSpiral
        public static bool IsLeviSpiralValid(PhotonMessageInfo info)
        {
            if (info == null) return true;

            GuardianClient.Logger.Error($"'Bullet.netUpdateLeviSpiral' from #{info.sender.Id}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }
    }
}
