namespace Guardian.AntiAbuse
{
    class ColossalPatches
    {
        // COLOSSAL_TITAN.removeMe
        public static bool IsRemovalValid(PhotonMessageInfo info)
        {
            if (info != null)
            {
                Mod.Logger.Error($"'COLOSSAL_TITAN.removeMe' from #{info.sender.id}.");
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
