namespace Guardian.AntiAbuse
{
    class ErenPatches
    {
        // TITAN_EREN.removeMe
        public static bool IsRemovalValid(PhotonMessageInfo info)
        {
            if (info != null)
            {
                Mod.Logger.Error($"'TITAN_EREN.removeMe' from #{info.sender.id}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.id);
                }
                return false;
            }
            return true;
        }
    }
}
