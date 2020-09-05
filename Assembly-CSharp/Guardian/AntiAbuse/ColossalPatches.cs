namespace Guardian.AntiAbuse
{
    class ColossalPatches
    {
        // COLOSSAL_TITAN.removeMe
        public static bool IsRemovalValid(PhotonMessageInfo info)
        {
            if (info != null)
            {
                Mod.Logger.Error($"'COLOSSAL_TITAN.removeMe' from #{info.sender.Id}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }
                return false;
            }
            return true;
        }
    }
}
