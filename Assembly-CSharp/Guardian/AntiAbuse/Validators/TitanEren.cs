namespace Guardian.AntiAbuse.Validators
{
    class TitanEren
    {
        // TITAN_EREN.removeMe
        public static bool IsRemovalValid(PhotonMessageInfo info)
        {
            if (info != null)
            {
                Mod.Logger.Error($"'TITAN_EREN.removeMe' from #{info.sender.Id}.");
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
