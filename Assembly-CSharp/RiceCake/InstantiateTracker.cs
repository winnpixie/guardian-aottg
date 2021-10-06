using UnityEngine;

public class InstantiateTracker
{
    public enum GameResource
    {
        None,
        GunShot,
        Effect,
        Flare,
        BladeHit,
        BloodEffect,
        General,
        Name,
        Bomb
    }

    private abstract class ResourceData
    {
        public GameResource type;

        public ResourceData()
        {
            type = GameResource.None;
        }

        public abstract bool KickWorthy();

        public abstract void Reset();
    }

    private class BloodResource : ResourceData
    {
        private float accumTime;
        private float lastHit;

        public BloodResource()
        {
            accumTime = 0f;
            lastHit = Time.time;
            type = GameResource.BloodEffect;
        }

        public override bool KickWorthy()
        {
            float num = Time.time - lastHit;
            lastHit = Time.time;
            if (num <= 0.3f)
            {
                accumTime += num;
                return accumTime >= 2f;
            }
            accumTime = 0f;
            return false;
        }

        public override void Reset()
        {
        }
    }

    private class BladeHitResource : ResourceData
    {
        private float accumTime;
        private float lastHit;

        public BladeHitResource()
        {
            accumTime = 0f;
            lastHit = Time.time;
            type = GameResource.BladeHit;
        }

        public override bool KickWorthy()
        {
            float num = Time.time - lastHit;
            lastHit = Time.time;
            if (num <= 0.3f)
            {
                accumTime += num;
                return accumTime >= 1.25f;
            }
            accumTime = 0f;
            return false;
        }

        public override void Reset()
        {
        }
    }

    private class GeneralResource : ResourceData
    {
        private int count;
        private float lastClear;

        public GeneralResource()
        {
            count = 1;
            lastClear = Time.time;
            type = GameResource.General;
        }

        public override bool KickWorthy()
        {
            if (Time.time - lastClear > 5f)
            {
                count = 0;
                lastClear = Time.time;
            }
            count++;
            return count > 35;
        }

        public override void Reset()
        {
        }
    }

    private class GunShotResource : ResourceData
    {
        private int shots;
        private float lastShot;

        public GunShotResource()
        {
            shots = 1;
            lastShot = Time.time;
            type = GameResource.GunShot;
        }

        public override bool KickWorthy()
        {
            if (Time.time - lastShot < 1f)
            {
                shots++;
                if (shots > 2)
                {
                    return true;
                }
            }
            else
            {
                shots = 0;
            }
            lastShot = Time.time;
            return false;
        }

        public override void Reset()
        {
        }
    }

    private class ExcessiveEffect : ResourceData
    {
        private int effectCounter;
        private float lastEffectTime;

        public ExcessiveEffect()
        {
            effectCounter = 1;
            lastEffectTime = Time.time;
            type = GameResource.Effect;
        }

        public override bool KickWorthy()
        {
            if (Time.time - lastEffectTime >= 2f)
            {
                effectCounter = 0;
                lastEffectTime = Time.time;
            }
            effectCounter++;
            if (effectCounter > 8)
            {
                return true;
            }
            return false;
        }

        public override void Reset()
        {
        }
    }

    private class ExcessiveFlares : ResourceData
    {
        private int flares;

        private float lastFlare;

        public ExcessiveFlares()
        {
            flares = 1;
            lastFlare = Time.time;
            type = GameResource.Flare;
        }

        public override bool KickWorthy()
        {
            if (Time.time - lastFlare >= 10f)
            {
                flares = 0;
                lastFlare = Time.time;
            }
            flares++;
            if (flares > 4)
            {
                return true;
            }
            return false;
        }

        public override void Reset()
        {
        }
    }

    private class ExcessiveNameChange : ResourceData
    {
        private int nameChanges;

        private float lastNameChange;

        public ExcessiveNameChange()
        {
            nameChanges = 1;
            lastNameChange = Time.time;
            type = GameResource.Name;
        }

        public override bool KickWorthy()
        {
            float num = Time.time - lastNameChange;
            lastNameChange = Time.time;
            if (num >= 5f)
            {
                nameChanges = 0;
            }
            nameChanges++;
            if (nameChanges > 5)
            {
                return true;
            }
            return false;
        }

        public override void Reset()
        {
            nameChanges = 0;
            lastNameChange = Time.time;
        }
    }

    private class ExcessiveBombs : ResourceData
    {
        private int count;

        private float lastClear;

        public ExcessiveBombs()
        {
            count = 1;
            lastClear = Time.time;
            type = GameResource.Bomb;
        }

        public override bool KickWorthy()
        {
            if (Time.time - lastClear > 5f)
            {
                count = 0;
                lastClear = Time.time;
            }
            count++;
            return count > 20;
        }

        public override void Reset()
        {
        }
    }

    private class Player
    {
        public int Id;
        private ResourceData[] Resources;

        public Player(int id)
        {
            this.Id = id;
            Resources = new ResourceData[0];
        }

        private int GetResourceData(GameResource type)
        {
            for (int i = 0; i < Resources.Length; i++)
            {
                if (Resources[i].type == type)
                {
                    return i;
                }
            }

            return -1;
        }

        private ResourceData GameResourceToResourceData(GameResource gr)
        {
            switch (gr)
            {
                case GameResource.General:
                    return new GeneralResource();
                case GameResource.Effect:
                    return new ExcessiveEffect();
                case GameResource.GunShot:
                    return new GunShotResource();
                case GameResource.BladeHit:
                    return new BladeHitResource();
                case GameResource.Flare:
                    return new ExcessiveFlares();
                case GameResource.BloodEffect:
                    return new BloodResource();
                case GameResource.Name:
                    return new ExcessiveNameChange();
                case GameResource.Bomb:
                    return new ExcessiveBombs();
                default:
                    return null;
            }
        }

        public bool IsResourceExcessive(GameResource gr)
        {
            int thingToCheck = GetResourceData(gr);
            if (thingToCheck > -1)
            {
                return Resources[thingToCheck].KickWorthy();
            }
            RCextensions.Add(ref Resources, GameResourceToResourceData(gr));
            return false;
        }

        public void ResetNameTracking()
        {
            int thingToCheck = GetResourceData(GameResource.Name);
            if (thingToCheck > -1)
            {
                Resources[thingToCheck].Reset();
            }
        }
    }

    public static readonly InstantiateTracker Instance;

    private Player[] Players = new Player[0];

    public bool CheckObject(string key, PhotonPlayer photonPlayer, int[] viewIDS)
    {
        if (!photonPlayer.isMasterClient && !photonPlayer.isLocal)
        {
            int num = photonPlayer.Id * PhotonNetwork.MAX_VIEW_IDS;
            int num2 = num + PhotonNetwork.MAX_VIEW_IDS;
            foreach (int num3 in viewIDS)
            {
                if (num3 <= num || num3 >= num2)
                {
                    if (PhotonNetwork.isMasterClient)
                    {
                        FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning invalid photon view.");
                    }
                    return false;
                }
            }
            key = key.ToLower();
            switch (key)
            {
                case "rcasset/bombmain":
                case "rcasset/bombexplodemain":
                    if (RCSettings.BombMode > 0)
                    {
                        return Instantiated(photonPlayer, GameResource.Bomb);
                    }
                    if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.Instance.restartingBomb)
                    {
                        FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning bomb item (" + key + ").");
                    }
                    return false;
                case "hook":
                case "aottg_hero 1":
                    return Instantiated(photonPlayer, GameResource.General);
                case "hitmeat2":
                    return Instantiated(photonPlayer, GameResource.BloodEffect);
                case "hitmeat":
                case "redcross":
                case "redcross1":
                    return Instantiated(photonPlayer, GameResource.BladeHit);
                case "fx/flarebullet1":
                case "fx/flarebullet2":
                case "fx/flarebullet3":
                    return Instantiated(photonPlayer, GameResource.Flare);
                case "fx/shotgun":
                case "fx/shotgun 1":
                    return Instantiated(photonPlayer, GameResource.GunShot);
                case "fx/fxtitanspawn":
                case "fx/boom1":
                case "fx/boom3":
                case "fx/boom5":
                case "fx/rockthrow":
                case "fx/bite":
                    if (!FengGameManagerMKII.Level.PlayerTitans && RCSettings.InfectionMode <= 0 && FengGameManagerMKII.Level.Mode != GameMode.Colossal)
                    {
                        if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.Instance.restartingTitan)
                        {
                            FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: false, "spawning titan effects.");
                        }
                        return false;
                    }
                    return Instantiated(photonPlayer, GameResource.Effect);
                case "fx/boom2":
                case "fx/boom4":
                case "fx/fxtitandie":
                case "fx/fxtitandie1":
                case "fx/boost_smoke":
                case "fx/thunder":
                    return Instantiated(photonPlayer, GameResource.Effect);
                case "rcasset/cannonballobject":
                    return Instantiated(photonPlayer, GameResource.Bomb);
                case "rcasset/cannonwall":
                case "rcasset/cannonground":
                    if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.Instance.allowedToCannon.ContainsKey(photonPlayer.Id) && !FengGameManagerMKII.Instance.restartingMC)
                    {
                        FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: false, "spawning cannon item (" + key + ").");
                    }
                    return Instantiated(photonPlayer, GameResource.General);
                case "rcasset/cannonwallprop":
                case "rcasset/cannongroundprop":
                    if (PhotonNetwork.isMasterClient)
                    {
                        FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning MC item (" + key + ").");
                    }
                    return false;
                case "titan_eren":
                    if (GExtensions.AsString(photonPlayer.customProperties[PhotonPlayerProperty.Character]).ToUpper() != "EREN")
                    {
                        if (PhotonNetwork.isMasterClient)
                        {
                            FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning titan eren (" + key + ").");
                        }
                        return false;
                    }
                    if (RCSettings.BanEren > 0)
                    {
                        if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.Instance.restartingEren)
                        {
                            FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: false, "spawning titan eren (" + key + ").");
                        }
                        return false;
                    }
                    return Instantiated(photonPlayer, GameResource.General);
                case "fx/justSmoke":
                case "bloodexplore":
                case "bloodsplatter":
                    return Instantiated(photonPlayer, GameResource.Effect);
                case "hitmeatbig":
                    if (GExtensions.AsString(photonPlayer.customProperties[PhotonPlayerProperty.Character]).ToUpper() != "EREN")
                    {
                        if (PhotonNetwork.isMasterClient)
                        {
                            FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning eren effect (" + key + ").");
                        }
                        return false;
                    }
                    if (RCSettings.BanEren > 0)
                    {
                        if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.Instance.restartingEren)
                        {
                            FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: false, "spawning eren effect (" + key + ").");
                        }
                        return false;
                    }
                    return Instantiated(photonPlayer, GameResource.Effect);
                case "fx/colossal_steam_dmg":
                case "fx/colossal_steam":
                case "fx/boom1_ct_kick":
                    if (PhotonNetwork.isMasterClient && FengGameManagerMKII.Level.Mode != GameMode.Colossal)
                    {
                        FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning colossal effect (" + key + ").");
                        return false;
                    }
                    return Instantiated(photonPlayer, GameResource.Effect);
                case "rock":
                    if (PhotonNetwork.isMasterClient && FengGameManagerMKII.Level.Mode != GameMode.Colossal)
                    {
                        FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning MC item (" + key + ").");
                        return false;
                    }
                    return Instantiated(photonPlayer, GameResource.General);
                case "horse":
                    if (!FengGameManagerMKII.Level.Horses && RCSettings.HorseMode == 0)
                    {
                        if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.Instance.restartingHorse)
                        {
                            FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning horse (" + key + ").");
                        }
                        return false;
                    }
                    return Instantiated(photonPlayer, GameResource.General);
                case "titan_ver3.1":
                    if (PhotonNetwork.isMasterClient)
                    {
                        if (!FengGameManagerMKII.Level.PlayerTitans && RCSettings.InfectionMode <= 0 && FengGameManagerMKII.Level.Mode != GameMode.Colossal && !FengGameManagerMKII.Instance.restartingTitan)
                        {
                            FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: false, "spawning titan (" + key + ").");
                            return false;
                        }
                        if (FengGameManagerMKII.Level.Mode != GameMode.Colossal)
                        {
                            int num4 = 0;
                            foreach (TITAN titan in FengGameManagerMKII.Instance.Titans)
                            {
                                if (titan.photonView.owner == photonPlayer)
                                {
                                    num4++;
                                }
                            }
                            if (num4 > 1)
                            {
                                FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: false, "spawning titan (" + key + ").");
                                return false;
                            }
                        }
                    }
                    else if (FengGameManagerMKII.MasterRC && FengGameManagerMKII.Level.Mode != GameMode.Colossal)
                    {
                        int num4 = 0;
                        foreach (TITAN titan2 in FengGameManagerMKII.Instance.Titans)
                        {
                            if (titan2.photonView.owner == photonPlayer)
                            {
                                num4++;
                            }
                        }
                        if (num4 > 1)
                        {
                            return false;
                        }
                    }
                    return Instantiated(photonPlayer, GameResource.General);
                case "colossal_titan":
                case "female_titan":
                case "titan_eren_trost":
                case "aot_supply":
                case "monsterprefab":
                case "titan_new_1":
                case "titan_new_2":
                    if (PhotonNetwork.isMasterClient)
                    {
                        FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning MC item (" + key + ").");
                        return false;
                    }
                    if (FengGameManagerMKII.MasterRC)
                    {
                        return false;
                    }
                    return Instantiated(photonPlayer, GameResource.General);
                default:
                    return false;
            }
        }
        return true;
    }

    private bool TryGetPlayer(int id, out int result)
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (Players[i].Id == id)
            {
                result = i;
                return true;
            }
        }
        result = -1;
        return false;
    }

    public bool Instantiated(PhotonPlayer owner, GameResource type)
    {
        if (TryGetPlayer(owner.Id, out int result))
        {
            if (Players[result].IsResourceExcessive(type))
            {
                if (owner != null && PhotonNetwork.isMasterClient)
                {
                    FengGameManagerMKII.Instance.KickPlayer(owner, ban: true, "spamming instantiate (" + type.ToString() + ").");
                }
                RCextensions.RemoveAt(ref Players, result);
                return false;
            }
        }
        else
        {
            RCextensions.Add(ref Players, new Player(owner.Id));
            Players[Players.Length - 1].IsResourceExcessive(type);
        }
        return true;
    }

    public bool PropertiesChanged(PhotonPlayer owner)
    {
        if (TryGetPlayer(owner.Id, out int result))
        {
            if (Players[result].IsResourceExcessive(GameResource.Name))
            {
                return false;
            }
        }
        else
        {
            RCextensions.Add(ref Players, new Player(owner.Id));
            Players[Players.Length - 1].IsResourceExcessive(GameResource.Name);
        }
        return true;
    }

    public void ResetPropertyTracker(int id)
    {
        if (TryGetPlayer(id, out int result))
        {
            Players[result].ResetNameTracking();
        }
    }

    public void TryRemovePlayer(int playerId)
    {
        int num = 0;
        while (true)
        {
            if (num < Players.Length)
            {
                if (Players[num].Id == playerId)
                {
                    break;
                }
                num++;
                continue;
            }
            return;
        }
        RCextensions.RemoveAt(ref Players, num);
    }

    public void Dispose()
    {
        Players = null;
        Players = new Player[0];
    }

    static InstantiateTracker()
    {
        Instance = new InstantiateTracker();
    }
}
