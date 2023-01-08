using Guardian.Utilities;
using UnityEngine;

namespace Guardian.AntiAbuse.Validators
{
    class HeroValidator
    {
        // HERO.killObject
        public static bool IsKillObjectValid(PhotonMessageInfo info)
        {
            if (info == null) return true;

            GuardianClient.Logger.Error($"'HERO.killObject' from #{info.sender.Id}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // HERO.showHitDamage
        public static bool IsHitDamageShowValid(PhotonMessageInfo info)
        {
            if (info == null) return true;

            GuardianClient.Logger.Error($"'HERO.showHitDamage' from #{info.sender.Id}.");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // HERO.whoIsMyErenTitan
        public static bool IsErenTitanDeclarationValid(int viewId, PhotonMessageInfo info)
        {
            PhotonView view = PhotonView.Find(viewId);
            if (info != null && view != null
                && view.ownerId == info.sender.Id
                && view.gameObject.GetComponent<TITAN_EREN>() != null) return true;

            GuardianClient.Logger.Warn($"'HERO.whoIsMyErenTitan' from #{(info == null ? "?" : info.sender.Id.ToString())}.");
            return false;
        }

        // TODO: Retire RPC check?
        // HERO.SetMyCannon
        public static bool IsCannonSetValid(int viewId, PhotonMessageInfo info)
        {
            // FIXME: No longer checking if PV has a Cannon object, un-tested.
            PhotonView view = PhotonView.Find(viewId);
            if (view != null && view.ownerId == info.sender.Id) return true;

            GuardianClient.Logger.Warn($"'HERO.SetMyCannon' from #{(info == null ? "?" : info.sender.Id.ToString())}");
            return false;
        }

        // HERO.loadskinRPC
        public static bool IsSkinLoadValid(HERO hero, PhotonMessageInfo info)
        {
            if (info != null && hero.photonView.ownerId == info.sender.Id) return true;

            GuardianClient.Logger.Error($"'HERO.loadskinRPC' from #{(info == null ? "?" : info.sender.Id.ToString())}");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }
            return false;
        }

        // HERO.netGrabbed
        public static bool IsGrabValid(int viewId, PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Multiplayer) return true;

            PhotonView view = PhotonView.Find(viewId);
            if (info != null && view != null)
            {
                GameObject go = view.gameObject;
                if (go != null)
                {
                    TITAN titan = go.GetComponent<TITAN>();
                    if (titan != null && titan.photonView.ownerId == info.sender.Id) return true;

                    FEMALE_TITAN annie = go.GetComponent<FEMALE_TITAN>();
                    if (annie != null && annie.photonView.ownerId == info.sender.Id) return true;
                }
            }

            GuardianClient.Logger.Error($"'HERO.netGrabbed' from #{(info == null ? "?" : info.sender.Id.ToString())}");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // TODO: Retire RPC check?
        // HERO.blowAway
        public static bool IsBlowAwayValid(PhotonMessageInfo info)
        {
            if (IN_GAME_MAIN_CAMERA.Gametype != GameType.Multiplayer
                || info == null
                || info.sender.isMasterClient
                || info.sender.isLocal
                || info.sender.IsTitan) return true;

            // FIXME: Dirty un-tested workaround for issues relating to cannons.
            HERO hero = info.sender.GetHero();
            if (hero != null && hero.isCannon) return true;

            GuardianClient.Logger.Error($"'HERO.blowAway' from #{(info == null ? "?" : info.sender.Id.ToString())}");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // HERO.netPlayAnimation
        public static bool IsAnimationPlayValid(HERO hero, PhotonMessageInfo info)
        {
            if (info == null
                || hero.photonView.ownerId == info.sender.Id
                || info.sender.isMasterClient
                || info.sender.IsTitan) return true;

            GuardianClient.Logger.Error($"'HERO.netPlayAnimation' from #{(info == null ? "?" : info.sender.Id.ToString())}");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // HERO.netPlayAnimationAt
        public static bool IsAnimationSeekedPlayValid(HERO hero, PhotonMessageInfo info)
        {
            if (info != null && hero.photonView.ownerId == info.sender.Id) return true;

            GuardianClient.Logger.Error($"'HERO.netPlayAnimationAt' from #{(info == null ? "?" : info.sender.Id.ToString())}");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // HERO.netCrossFade
        public static bool IsCrossFadeValid(HERO hero, PhotonMessageInfo info)
        {
            if (info != null && hero.photonView.ownerId == info.sender.Id) return true;

            GuardianClient.Logger.Error($"'HERO.netCrossFade' from #{(info == null ? "?" : info.sender.Id.ToString())}");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // HERO.netPauseAnimation
        public static bool IsAnimationPauseValid(HERO hero, PhotonMessageInfo info)
        {
            if (info != null && hero.photonView.ownerId == info.sender.Id) return true;

            GuardianClient.Logger.Error($"'HERO.netPauseAnimation' from #{(info == null ? "?" : info.sender.Id.ToString())}");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }

        // HERO.netContinueAnimation
        public static bool IsAnimationResumeValid(HERO hero, PhotonMessageInfo info)
        {
            if (info != null && hero.photonView.ownerId == info.sender.Id) return true;

            GuardianClient.Logger.Error($"'HERO.netContinueAnimation' from #{(info == null ? "?" : info.sender.Id.ToString())}");
            if (info.sender != null && !FengGameManagerMKII.IgnoreList.Contains(info.sender.Id))
            {
                FengGameManagerMKII.IgnoreList.Add(info.sender.Id);
            }

            return false;
        }
    }
}
