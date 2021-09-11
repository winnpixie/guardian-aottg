using UnityEngine;
using Guardian.Utilities;

namespace Guardian.AntiAbuse.Validators
{
    class Hero
    {
        // HERO.killObject
        public static bool IsKillObjectValid(PhotonMessageInfo info)
        {
            if (info != null)
            {
                Mod.Logger.Error($"'HERO.killObject' from #{info.sender.Id}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }
                return false;
            }
            return true;
        }

        // HERO.showHitDamage
        public static bool IsHitDamageShowValid(PhotonMessageInfo info)
        {
            if (info != null)
            {
                Mod.Logger.Error($"'HERO.showHitDamage' from #{info.sender.Id}.");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }
                return false;
            }
            return true;
        }

        // HERO.whoIsMyErenTitan
        public static bool IsErenTitanDeclarationValid(int viewId, PhotonMessageInfo info)
        {
            PhotonView view = PhotonView.Find(viewId);
            if (info == null || view == null || view.ownerId != info.sender.Id || view.gameObject.GetComponent<TITAN_EREN>() == null)
            {
                Mod.Logger.Warn($"'HERO.whoIsMyErenTitan' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
                return false;
            }
            return true;
        }

        // HERO.SetMyCannon
        public static bool IsCannonSetValid(int viewId, PhotonMessageInfo info)
        {
            PhotonView photonView = PhotonView.Find(viewId);
            if (info == null || photonView == null || photonView.gameObject.GetComponent<Cannon>() == null)
            {
                Mod.Logger.Warn($"'HERO.SetMyCannon' from #{(info == null ? "?" : info.sender.Id.ToString())}");
                return false;
            }
            return true;
        }

        // HERO.loadskinRPC
        public static bool IsSkinLoadValid(HERO hero, PhotonMessageInfo info)
        {
            if (info == null || (hero.photonView.ownerId != info.sender.Id))
            {
                Mod.Logger.Error($"'HERO.loadskinRPC' from #{(info == null ? "?" : info.sender.Id.ToString())}");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }
                return false;
            }
            return true;
        }

        // HERO.netGrabbed
        public static bool IsGrabValid(int viewID, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info != null)
                {
                    PhotonView view = PhotonView.Find(viewID);
                    if (view != null)
                    {
                        GameObject go = PhotonView.Find(viewID).gameObject;
                        if (go != null)
                        {
                            TITAN titan = go.GetComponent<TITAN>();
                            if (titan != null && titan.photonView.ownerId == info.sender.Id)
                            {
                                return true;
                            }
                            FEMALE_TITAN annie = go.GetComponent<FEMALE_TITAN>();
                            if (annie != null && annie.photonView.ownerId == info.sender.Id)
                            {
                                return true;
                            }
                        }
                    }
                }
                Mod.Logger.Error($"'HERO.netGrabbed' from #{(info == null ? "?" : info.sender.Id.ToString())}");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }
                return false;
            }
            return true;
        }

        // HERO.blowAway
        public static bool IsBlowAwayValid(PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Singleplayer)
            {
                if (info == null || (!info.sender.isMasterClient && !info.sender.isLocal && !GameHelper.IsPT(info.sender)))
                {
                    Mod.Logger.Error($"'HERO.blowAway' from #{(info == null ? "?" : info.sender.Id.ToString())}");
                    if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                    {
                        FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                    }
                    return false;
                }
            }
            return true;
        }

        // HERO.netPlayAnimation
        public static bool IsAnimationPlayValid(HERO hero, PhotonMessageInfo info)
        {
            if (info != null && hero.photonView.ownerId != info.sender.Id && !(info.sender.isMasterClient || GameHelper.IsPT(info.sender)))
            {
                Mod.Logger.Error($"'HERO.netPlayAnimation' from #{(info == null ? "?" : info.sender.Id.ToString())}");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }
                return false;
            }
            return true;
        }

        // HERO.netPlayAnimationAt
        public static bool IsAnimationSeekedPlayValid(HERO hero, PhotonMessageInfo info)
        {
            if (info == null || (hero.photonView.ownerId != info.sender.Id))
            {
                Mod.Logger.Error($"'HERO.netPlayAnimationAt' from #{(info == null ? "?" : info.sender.Id.ToString())}");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }
                return false;
            }
            return true;
        }

        // HERO.netCrossFade
        public static bool IsCrossFadeValid(HERO hero, PhotonMessageInfo info)
        {
            if (info == null || (hero.photonView.ownerId != info.sender.Id))
            {
                Mod.Logger.Error($"'HERO.netCrossFade' from #{(info == null ? "?" : info.sender.Id.ToString())}");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }
                return false;
            }
            return true;
        }

        // HERO.netPauseAnimation
        public static bool IsAnimationPauseValid(HERO hero, PhotonMessageInfo info)
        {
            if (info == null || (hero.photonView.ownerId != info.sender.Id))
            {
                Mod.Logger.Error($"'HERO.netPauseAnimation' from #{(info == null ? "?" : info.sender.Id.ToString())}");
                if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
                {
                    FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
                }
                return false;
            }
            return true;
        }

        // HERO.netContinueAnimation
        public static bool IsAnimationResumeValid(HERO hero, PhotonMessageInfo info)
        {
            if (info == null || (hero.photonView.ownerId != info.sender.Id))
            {
                Mod.Logger.Error($"'HERO.netContinueAnimation' from #{(info == null ? "?" : info.sender.Id.ToString())}");
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
