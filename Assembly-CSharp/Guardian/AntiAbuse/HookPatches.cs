namespace Guardian.AntiAbuse
{
    class HookPatches
    {
        // Bullet.killObject
        public static bool IsKillObjectValid(PhotonMessageInfo info)
        {
            if (info != null)
            {
                Mod.Logger.Error($"'Bullet.killObject' from #{info.sender.Id}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }
                return false;
            }
            return true;
        }

        // Bullet.myMasterIs
        public static bool IsHookMasterSetValid(Bullet hook, int viewId, PhotonMessageInfo info)
        {
            PhotonView view = PhotonView.Find(viewId);
            if (info == null || view == null || hook.photonView.ownerId != info.sender.Id || view.gameObject.GetComponent<HERO>() == null)
            {
                Mod.Logger.Warn($"'Bullet.myMasterIs' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                return false;
            }
            return true;
        }

        // Bullet.tieMeToOBJ
        public static bool IsHookTieValid(Bullet hook, int viewId, PhotonMessageInfo info)
        {
            PhotonView view = PhotonView.Find(viewId);
            if (info == null || view == null || hook.photonView.ownerId != info.sender.Id)
            {
                Mod.Logger.Warn($"'Bullet.tieMeToOBJ' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                return false;
            }
            return true;
        }
    }
}
