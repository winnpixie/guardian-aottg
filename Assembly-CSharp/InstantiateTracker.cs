using UnityEngine;

public class InstantiateTracker
{
    public enum GameResource
    {
        none,
        shotGun,
        effect,
        flare,
        bladeHit,
        bloodEffect,
        general,
        name,
        bomb
    }

    private abstract class ThingToCheck
    {
        public GameResource type;

        public ThingToCheck()
        {
            type = GameResource.none;
        }

        public abstract bool KickWorthy();

        public abstract void reset();
    }

    private class BloodEffect : ThingToCheck
    {
        private float accumTime;
        private float lastHit;

        public BloodEffect()
        {
            accumTime = 0f;
            lastHit = Time.time;
            type = GameResource.bloodEffect;
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

        public override void reset()
        {
        }
    }

    private class BladeHitEffect : ThingToCheck
    {
        private float accumTime;
        private float lastHit;

        public BladeHitEffect()
        {
            accumTime = 0f;
            lastHit = Time.time;
            type = GameResource.bladeHit;
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

        public override void reset()
        {
        }
    }

    private class GenerallyExcessive : ThingToCheck
    {
        private int count;
        private float lastClear;

        public GenerallyExcessive()
        {
            count = 1;
            lastClear = Time.time;
            type = GameResource.general;
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

        public override void reset()
        {
        }
    }

    private class AhssShots : ThingToCheck
    {
        private int shots;
        private float lastShot;

        public AhssShots()
        {
            shots = 1;
            lastShot = Time.time;
            type = GameResource.shotGun;
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

        public override void reset()
        {
        }
    }

    private class ExcessiveEffect : ThingToCheck
    {
        private int effectCounter;
        private float lastEffectTime;

        public ExcessiveEffect()
        {
            effectCounter = 1;
            lastEffectTime = Time.time;
            type = GameResource.effect;
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

        public override void reset()
        {
        }
    }

    private class ExcessiveFlares : ThingToCheck
    {
        private int flares;

        private float lastFlare;

        public ExcessiveFlares()
        {
            flares = 1;
            lastFlare = Time.time;
            type = GameResource.flare;
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

        public override void reset()
        {
        }
    }

    private class ExcessiveNameChange : ThingToCheck
    {
        private int nameChanges;

        private float lastNameChange;

        public ExcessiveNameChange()
        {
            nameChanges = 1;
            lastNameChange = Time.time;
            type = GameResource.name;
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

        public override void reset()
        {
            nameChanges = 0;
            lastNameChange = Time.time;
        }
    }

    private class ExcessiveBombs : ThingToCheck
    {
        private int count;

        private float lastClear;

        public ExcessiveBombs()
        {
            count = 1;
            lastClear = Time.time;
            type = GameResource.bomb;
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

        public override void reset()
        {
        }
    }

    private class Player
    {
        public int id;
        private ThingToCheck[] thingsToCheck;

        public Player(int id)
        {
            this.id = id;
            thingsToCheck = new ThingToCheck[0];
        }

        private int GetThingToCheck(GameResource type)
        {
            for (int i = 0; i < thingsToCheck.Length; i++)
            {
                if (thingsToCheck[i].type == type)
                {
                    return i;
                }
            }
            return -1;
        }

        private ThingToCheck GameResourceToThing(GameResource gr)
        {
            switch (gr)
            {
                case GameResource.general:
                    return new GenerallyExcessive();
                case GameResource.effect:
                    return new ExcessiveEffect();
                case GameResource.shotGun:
                    return new AhssShots();
                case GameResource.bladeHit:
                    return new BladeHitEffect();
                case GameResource.flare:
                    return new ExcessiveFlares();
                case GameResource.bloodEffect:
                    return new BloodEffect();
                case GameResource.name:
                    return new ExcessiveNameChange();
                case GameResource.bomb:
                    return new ExcessiveBombs();
                default:
                    return null;
            }
        }

        public bool IsThingExcessive(GameResource gr)
        {
            int thingToCheck = GetThingToCheck(gr);
            if (thingToCheck > -1)
            {
                return thingsToCheck[thingToCheck].KickWorthy();
            }
            RCextensions.Add(ref thingsToCheck, GameResourceToThing(gr));
            return false;
        }

        public void resetNameTracking()
        {
            int thingToCheck = GetThingToCheck(GameResource.name);
            if (thingToCheck > -1)
            {
                thingsToCheck[thingToCheck].reset();
            }
        }
    }

    public static readonly InstantiateTracker instance;

    private Player[] players = new Player[0];

    public bool checkObj(string key, PhotonPlayer photonPlayer, int[] viewIDS)
    {
        if (!photonPlayer.isMasterClient && !photonPlayer.isLocal)
        {
            int num = photonPlayer.id * PhotonNetwork.MAX_VIEW_IDS;
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
                        return Instantiated(photonPlayer, GameResource.bomb);
                    }
                    if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.Instance.restartingBomb)
                    {
                        FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning bomb item (" + key + ").");
                    }
                    return false;
                case "hook":
                case "aottg_hero 1":
                    return Instantiated(photonPlayer, GameResource.general);
                case "hitmeat2":
                    return Instantiated(photonPlayer, GameResource.bloodEffect);
                case "hitmeat":
                case "redcross":
                case "redcross1":
                    return Instantiated(photonPlayer, GameResource.bladeHit);
                case "fx/flarebullet1":
                case "fx/flarebullet2":
                case "fx/flarebullet3":
                    return Instantiated(photonPlayer, GameResource.flare);
                case "fx/shotgun":
                case "fx/shotgun 1":
                    return Instantiated(photonPlayer, GameResource.shotGun);
                case "fx/fxtitanspawn":
                case "fx/boom1":
                case "fx/boom3":
                case "fx/boom5":
                case "fx/rockthrow":
                case "fx/bite":
                    if (!LevelInfo.GetInfo(FengGameManagerMKII.level).teamTitan && RCSettings.InfectionMode <= 0 && IN_GAME_MAIN_CAMERA.Gamemode != GAMEMODE.BOSS_FIGHT_CT)
                    {
                        if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.Instance.restartingTitan)
                        {
                            FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: false, "spawning titan effects.");
                        }
                        return false;
                    }
                    return Instantiated(photonPlayer, GameResource.effect);
                case "fx/boom2":
                case "fx/boom4":
                case "fx/fxtitandie":
                case "fx/fxtitandie1":
                case "fx/boost_smoke":
                case "fx/thunder":
                    return Instantiated(photonPlayer, GameResource.effect);
                case "rcasset/cannonballobject":
                    return Instantiated(photonPlayer, GameResource.bomb);
                case "rcasset/cannonwall":
                case "rcasset/cannonground":
                    if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.Instance.allowedToCannon.ContainsKey(photonPlayer.id) && !FengGameManagerMKII.Instance.restartingMC)
                    {
                        FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: false, "spawning cannon item (" + key + ").");
                    }
                    return Instantiated(photonPlayer, GameResource.general);
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
                    return Instantiated(photonPlayer, GameResource.general);
                case "fx/justSmoke":
                case "bloodexplore":
                case "bloodsplatter":
                    return Instantiated(photonPlayer, GameResource.effect);
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
                    return Instantiated(photonPlayer, GameResource.effect);
                case "fx/colossal_steam_dmg":
                case "fx/colossal_steam":
                case "fx/boom1_ct_kick":
                    if (PhotonNetwork.isMasterClient && IN_GAME_MAIN_CAMERA.Gamemode != GAMEMODE.BOSS_FIGHT_CT)
                    {
                        FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning colossal effect (" + key + ").");
                        return false;
                    }
                    return Instantiated(photonPlayer, GameResource.effect);
                case "rock":
                    if (PhotonNetwork.isMasterClient && IN_GAME_MAIN_CAMERA.Gamemode != GAMEMODE.BOSS_FIGHT_CT)
                    {
                        FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning MC item (" + key + ").");
                        return false;
                    }
                    return Instantiated(photonPlayer, GameResource.general);
                case "horse":
                    if (!LevelInfo.GetInfo(FengGameManagerMKII.level).horse && RCSettings.HorseMode == 0)
                    {
                        if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.Instance.restartingHorse)
                        {
                            FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: true, "spawning horse (" + key + ").");
                        }
                        return false;
                    }
                    return Instantiated(photonPlayer, GameResource.general);
                case "titan_ver3.1":
                    if (PhotonNetwork.isMasterClient)
                    {
                        if (!LevelInfo.GetInfo(FengGameManagerMKII.level).teamTitan && RCSettings.InfectionMode <= 0 && IN_GAME_MAIN_CAMERA.Gamemode != GAMEMODE.BOSS_FIGHT_CT && !FengGameManagerMKII.Instance.restartingTitan)
                        {
                            FengGameManagerMKII.Instance.KickPlayer(photonPlayer, ban: false, "spawning titan (" + key + ").");
                            return false;
                        }
                        if (IN_GAME_MAIN_CAMERA.Gamemode != GAMEMODE.BOSS_FIGHT_CT)
                        {
                            int num4 = 0;
                            foreach (TITAN titan in FengGameManagerMKII.Instance.titans)
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
                    else if (FengGameManagerMKII.MasterRC && IN_GAME_MAIN_CAMERA.Gamemode != GAMEMODE.BOSS_FIGHT_CT)
                    {
                        int num4 = 0;
                        foreach (TITAN titan2 in FengGameManagerMKII.Instance.titans)
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
                    return Instantiated(photonPlayer, GameResource.general);
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
                    return Instantiated(photonPlayer, GameResource.general);
                default:
                    return false;
            }
        }
        return true;
    }

    private bool TryGetPlayer(int id, out int result)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].id == id)
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
        if (TryGetPlayer(owner.id, out int result))
        {
            if (players[result].IsThingExcessive(type))
            {
                if (owner != null && PhotonNetwork.isMasterClient)
                {
                    FengGameManagerMKII.Instance.KickPlayer(owner, ban: true, "spamming instantiate (" + type.ToString() + ").");
                }
                RCextensions.RemoveAt(ref players, result);
                return false;
            }
        }
        else
        {
            RCextensions.Add(ref players, new Player(owner.id));
            players[players.Length - 1].IsThingExcessive(type);
        }
        return true;
    }

    public bool PropertiesChanged(PhotonPlayer owner)
    {
        if (TryGetPlayer(owner.id, out int result))
        {
            if (players[result].IsThingExcessive(GameResource.name))
            {
                return false;
            }
        }
        else
        {
            RCextensions.Add(ref players, new Player(owner.id));
            players[players.Length - 1].IsThingExcessive(GameResource.name);
        }
        return true;
    }

    public void resetPropertyTracking(int ID)
    {
        if (TryGetPlayer(ID, out int result))
        {
            players[result].resetNameTracking();
        }
    }

    public void TryRemovePlayer(int playerId)
    {
        int num = 0;
        while (true)
        {
            if (num < players.Length)
            {
                if (players[num].id == playerId)
                {
                    break;
                }
                num++;
                continue;
            }
            return;
        }
        RCextensions.RemoveAt(ref players, num);
    }

    public void Dispose()
    {
        players = null;
        players = new Player[0];
    }

    static InstantiateTracker()
    {
        instance = new InstantiateTracker();
    }
}
