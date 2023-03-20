using RC.UI.Impl;
using UnityEngine;

namespace RC.UI
{
    class RCGuiManager
    {
        public static readonly RCGuiGeneralSettings GeneralSettingsScreen = new RCGuiGeneralSettings();
        public static readonly RCGuiKeyBindSettings KeyBindSettingsScreen = new RCGuiKeyBindSettings();
        public static readonly RCGuiHumanSkinSettings HumanSkinSettingsScreen = new RCGuiHumanSkinSettings();
        public static readonly RCGuiTitanSkinSettings TitanSkinSettingsScreen = new RCGuiTitanSkinSettings();
        public static readonly RCGuiLevelSkinSettings LevelSkinSettingsScreen = new RCGuiLevelSkinSettings();
        public static readonly RCGuiCustomMapSettings CustomMapSettingsScreen = new RCGuiCustomMapSettings();
        public static readonly RCGuiCustomLogicSettings CustomLogicScreenScreen = new RCGuiCustomLogicSettings();
        public static readonly RCGuiMasterClientSettings MasterClientSettingsScreen = new RCGuiMasterClientSettings();
        public static readonly RCGuiAbilitiesSettings AbilitiesSettingsScreen = new RCGuiAbilitiesSettings();

        private static RCGui CurrentRCScreen = GeneralSettingsScreen;

        public static void OpenScreen(RCGui screen)
        {
            CurrentRCScreen = screen;
        }

        public static void Draw(FengGameManagerMKII fgm)
        {
            float halfMenuWidth = (float)Screen.width / 2f - 350f;
            float halfMenuHeight = (float)Screen.height / 2f - 250f;
            GUI.Box(new Rect(halfMenuWidth, halfMenuHeight, 700f, 500f), string.Empty);
            if (GUI.Button(new Rect(halfMenuWidth + 7f, halfMenuHeight + 7f, 59f, 25f), "General"))
            {
                OpenScreen(GeneralSettingsScreen);
                FengGameManagerMKII.Settings[64] = 0;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 71f, halfMenuHeight + 7f, 60f, 25f), "Rebinds"))
            {
                OpenScreen(KeyBindSettingsScreen);
                FengGameManagerMKII.Settings[64] = 1;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 136f, halfMenuHeight + 7f, 85f, 25f), "Human Skins"))
            {
                OpenScreen(HumanSkinSettingsScreen);
                FengGameManagerMKII.Settings[64] = 2;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 226f, halfMenuHeight + 7f, 85f, 25f), "Titan Skins"))
            {
                OpenScreen(TitanSkinSettingsScreen);
                FengGameManagerMKII.Settings[64] = 3;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 316f, halfMenuHeight + 7f, 85f, 25f), "Level Skins"))
            {
                OpenScreen(LevelSkinSettingsScreen);
                FengGameManagerMKII.Settings[64] = 7;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 406f, halfMenuHeight + 7f, 85f, 25f), "Custom Map"))
            {
                OpenScreen(CustomMapSettingsScreen);
                FengGameManagerMKII.Settings[64] = 8;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 496f, halfMenuHeight + 7f, 93f, 25f), "Custom Logic"))
            {
                OpenScreen(CustomLogicScreenScreen);
                FengGameManagerMKII.Settings[64] = 9;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 594f, halfMenuHeight + 7f, 99f, 25f), "Game Settings"))
            {
                OpenScreen(MasterClientSettingsScreen);
                FengGameManagerMKII.Settings[64] = 10;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 7f, halfMenuHeight + 37f, 70f, 25f), "Abilities"))
            {
                OpenScreen(AbilitiesSettingsScreen);
                FengGameManagerMKII.Settings[64] = 11;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 623f, halfMenuHeight + 37f, 70f, 25f), "Guardian")) // Guardian
            {
                Guardian.GuardianClient.GuiController.OpenScreen(new Guardian.UI.Impl.GuiModConfiguration());
            }

            switch ((int)FengGameManagerMKII.Settings[64])
            {
                case 4: // Saved Settings
                    GUI.TextArea(new Rect(halfMenuWidth + 80f, halfMenuHeight + 57f, 270f, 30f), "Saved settings to PlayerPrefs!", 100, "Label");
                    break;
                case 5: // Loaded Settings
                    GUI.TextArea(new Rect(halfMenuWidth + 80f, halfMenuHeight + 57f, 270f, 30f), "Loaded settings from PlayerPrefs!", 100, "Label");
                    break;
                case 6:
                    // ?
                    break;
                default:
                    CurrentRCScreen.Draw(fgm, halfMenuWidth, halfMenuHeight);
                    break;
            }

            if (GUI.Button(new Rect(halfMenuWidth + 416f, halfMenuHeight + 468f, 42f, 25f), "Save"))
            {
                PlayerPrefs.SetInt("human", (int)FengGameManagerMKII.Settings[0]);
                PlayerPrefs.SetInt("titan", (int)FengGameManagerMKII.Settings[1]);
                PlayerPrefs.SetInt("level", (int)FengGameManagerMKII.Settings[2]);
                PlayerPrefs.SetString("horse", (string)FengGameManagerMKII.Settings[3]);
                PlayerPrefs.SetString("hair", (string)FengGameManagerMKII.Settings[4]);
                PlayerPrefs.SetString("eye", (string)FengGameManagerMKII.Settings[5]);
                PlayerPrefs.SetString("glass", (string)FengGameManagerMKII.Settings[6]);
                PlayerPrefs.SetString("face", (string)FengGameManagerMKII.Settings[7]);
                PlayerPrefs.SetString("skin", (string)FengGameManagerMKII.Settings[8]);
                PlayerPrefs.SetString("costume", (string)FengGameManagerMKII.Settings[9]);
                PlayerPrefs.SetString("logo", (string)FengGameManagerMKII.Settings[10]);
                PlayerPrefs.SetString("bladel", (string)FengGameManagerMKII.Settings[11]);
                PlayerPrefs.SetString("blader", (string)FengGameManagerMKII.Settings[12]);
                PlayerPrefs.SetString("gas", (string)FengGameManagerMKII.Settings[13]);
                PlayerPrefs.SetString("haircolor", (string)FengGameManagerMKII.Settings[14]);
                PlayerPrefs.SetInt("gasenable", (int)FengGameManagerMKII.Settings[15]);
                PlayerPrefs.SetInt("titantype1", (int)FengGameManagerMKII.Settings[16]);
                PlayerPrefs.SetInt("titantype2", (int)FengGameManagerMKII.Settings[17]);
                PlayerPrefs.SetInt("titantype3", (int)FengGameManagerMKII.Settings[18]);
                PlayerPrefs.SetInt("titantype4", (int)FengGameManagerMKII.Settings[19]);
                PlayerPrefs.SetInt("titantype5", (int)FengGameManagerMKII.Settings[20]);
                PlayerPrefs.SetString("titanhair1", (string)FengGameManagerMKII.Settings[21]);
                PlayerPrefs.SetString("titanhair2", (string)FengGameManagerMKII.Settings[22]);
                PlayerPrefs.SetString("titanhair3", (string)FengGameManagerMKII.Settings[23]);
                PlayerPrefs.SetString("titanhair4", (string)FengGameManagerMKII.Settings[24]);
                PlayerPrefs.SetString("titanhair5", (string)FengGameManagerMKII.Settings[25]);
                PlayerPrefs.SetString("titaneye1", (string)FengGameManagerMKII.Settings[26]);
                PlayerPrefs.SetString("titaneye2", (string)FengGameManagerMKII.Settings[27]);
                PlayerPrefs.SetString("titaneye3", (string)FengGameManagerMKII.Settings[28]);
                PlayerPrefs.SetString("titaneye4", (string)FengGameManagerMKII.Settings[29]);
                PlayerPrefs.SetString("titaneye5", (string)FengGameManagerMKII.Settings[30]);
                PlayerPrefs.SetInt("titanR", (int)FengGameManagerMKII.Settings[32]);
                PlayerPrefs.SetString("tree1", (string)FengGameManagerMKII.Settings[33]);
                PlayerPrefs.SetString("tree2", (string)FengGameManagerMKII.Settings[34]);
                PlayerPrefs.SetString("tree3", (string)FengGameManagerMKII.Settings[35]);
                PlayerPrefs.SetString("tree4", (string)FengGameManagerMKII.Settings[36]);
                PlayerPrefs.SetString("tree5", (string)FengGameManagerMKII.Settings[37]);
                PlayerPrefs.SetString("tree6", (string)FengGameManagerMKII.Settings[38]);
                PlayerPrefs.SetString("tree7", (string)FengGameManagerMKII.Settings[39]);
                PlayerPrefs.SetString("tree8", (string)FengGameManagerMKII.Settings[40]);
                PlayerPrefs.SetString("leaf1", (string)FengGameManagerMKII.Settings[41]);
                PlayerPrefs.SetString("leaf2", (string)FengGameManagerMKII.Settings[42]);
                PlayerPrefs.SetString("leaf3", (string)FengGameManagerMKII.Settings[43]);
                PlayerPrefs.SetString("leaf4", (string)FengGameManagerMKII.Settings[44]);
                PlayerPrefs.SetString("leaf5", (string)FengGameManagerMKII.Settings[45]);
                PlayerPrefs.SetString("leaf6", (string)FengGameManagerMKII.Settings[46]);
                PlayerPrefs.SetString("leaf7", (string)FengGameManagerMKII.Settings[47]);
                PlayerPrefs.SetString("leaf8", (string)FengGameManagerMKII.Settings[48]);
                PlayerPrefs.SetString("forestG", (string)FengGameManagerMKII.Settings[49]);
                PlayerPrefs.SetInt("forestR", (int)FengGameManagerMKII.Settings[50]);
                PlayerPrefs.SetString("house1", (string)FengGameManagerMKII.Settings[51]);
                PlayerPrefs.SetString("house2", (string)FengGameManagerMKII.Settings[52]);
                PlayerPrefs.SetString("house3", (string)FengGameManagerMKII.Settings[53]);
                PlayerPrefs.SetString("house4", (string)FengGameManagerMKII.Settings[54]);
                PlayerPrefs.SetString("house5", (string)FengGameManagerMKII.Settings[55]);
                PlayerPrefs.SetString("house6", (string)FengGameManagerMKII.Settings[56]);
                PlayerPrefs.SetString("house7", (string)FengGameManagerMKII.Settings[57]);
                PlayerPrefs.SetString("house8", (string)FengGameManagerMKII.Settings[58]);
                PlayerPrefs.SetString("cityG", (string)FengGameManagerMKII.Settings[59]);
                PlayerPrefs.SetString("cityW", (string)FengGameManagerMKII.Settings[60]);
                PlayerPrefs.SetString("cityH", (string)FengGameManagerMKII.Settings[61]);
                PlayerPrefs.SetInt("skinQ", QualitySettings.masterTextureLimit);
                PlayerPrefs.SetInt("skinQL", (int)FengGameManagerMKII.Settings[63]);
                PlayerPrefs.SetString("eren", (string)FengGameManagerMKII.Settings[65]);
                PlayerPrefs.SetString("annie", (string)FengGameManagerMKII.Settings[66]);
                PlayerPrefs.SetString("colossal", (string)FengGameManagerMKII.Settings[67]);
                PlayerPrefs.SetString("hoodie", (string)FengGameManagerMKII.Settings[14]);
                PlayerPrefs.SetString("cnumber", (string)FengGameManagerMKII.Settings[82]);
                PlayerPrefs.SetString("cmax", (string)FengGameManagerMKII.Settings[85]);
                PlayerPrefs.SetString("titanbody1", (string)FengGameManagerMKII.Settings[86]);
                PlayerPrefs.SetString("titanbody2", (string)FengGameManagerMKII.Settings[87]);
                PlayerPrefs.SetString("titanbody3", (string)FengGameManagerMKII.Settings[88]);
                PlayerPrefs.SetString("titanbody4", (string)FengGameManagerMKII.Settings[89]);
                PlayerPrefs.SetString("titanbody5", (string)FengGameManagerMKII.Settings[90]);
                PlayerPrefs.SetInt("customlevel", (int)FengGameManagerMKII.Settings[91]);
                PlayerPrefs.SetInt("traildisable", (int)FengGameManagerMKII.Settings[92]);
                PlayerPrefs.SetInt("wind", (int)FengGameManagerMKII.Settings[93]);
                PlayerPrefs.SetString("trailskin", (string)FengGameManagerMKII.Settings[94]);
                PlayerPrefs.SetString("snapshot", (string)FengGameManagerMKII.Settings[95]);
                PlayerPrefs.SetString("trailskin2", (string)FengGameManagerMKII.Settings[96]);
                PlayerPrefs.SetInt("reel", (int)FengGameManagerMKII.Settings[97]);
                PlayerPrefs.SetString("reelin", (string)FengGameManagerMKII.Settings[98]);
                PlayerPrefs.SetString("reelout", (string)FengGameManagerMKII.Settings[99]);
                PlayerPrefs.SetFloat("vol", AudioListener.volume);
                PlayerPrefs.SetString("tforward", (string)FengGameManagerMKII.Settings[101]);
                PlayerPrefs.SetString("tback", (string)FengGameManagerMKII.Settings[102]);
                PlayerPrefs.SetString("tleft", (string)FengGameManagerMKII.Settings[103]);
                PlayerPrefs.SetString("tright", (string)FengGameManagerMKII.Settings[104]);
                PlayerPrefs.SetString("twalk", (string)FengGameManagerMKII.Settings[105]);
                PlayerPrefs.SetString("tjump", (string)FengGameManagerMKII.Settings[106]);
                PlayerPrefs.SetString("tpunch", (string)FengGameManagerMKII.Settings[107]);
                PlayerPrefs.SetString("tslam", (string)FengGameManagerMKII.Settings[108]);
                PlayerPrefs.SetString("tgrabfront", (string)FengGameManagerMKII.Settings[109]);
                PlayerPrefs.SetString("tgrabback", (string)FengGameManagerMKII.Settings[110]);
                PlayerPrefs.SetString("tgrabnape", (string)FengGameManagerMKII.Settings[111]);
                PlayerPrefs.SetString("tantiae", (string)FengGameManagerMKII.Settings[112]);
                PlayerPrefs.SetString("tbite", (string)FengGameManagerMKII.Settings[113]);
                PlayerPrefs.SetString("tcover", (string)FengGameManagerMKII.Settings[114]);
                PlayerPrefs.SetString("tsit", (string)FengGameManagerMKII.Settings[115]);
                PlayerPrefs.SetInt("reel2", (int)FengGameManagerMKII.Settings[116]);
                PlayerPrefs.SetInt("humangui", (int)FengGameManagerMKII.Settings[133]);
                PlayerPrefs.SetString("horse2", (string)FengGameManagerMKII.Settings[134]);
                PlayerPrefs.SetString("hair2", (string)FengGameManagerMKII.Settings[135]);
                PlayerPrefs.SetString("eye2", (string)FengGameManagerMKII.Settings[136]);
                PlayerPrefs.SetString("glass2", (string)FengGameManagerMKII.Settings[137]);
                PlayerPrefs.SetString("face2", (string)FengGameManagerMKII.Settings[138]);
                PlayerPrefs.SetString("skin2", (string)FengGameManagerMKII.Settings[139]);
                PlayerPrefs.SetString("costume2", (string)FengGameManagerMKII.Settings[140]);
                PlayerPrefs.SetString("logo2", (string)FengGameManagerMKII.Settings[141]);
                PlayerPrefs.SetString("bladel2", (string)FengGameManagerMKII.Settings[142]);
                PlayerPrefs.SetString("blader2", (string)FengGameManagerMKII.Settings[143]);
                PlayerPrefs.SetString("gas2", (string)FengGameManagerMKII.Settings[144]);
                PlayerPrefs.SetString("hoodie2", (string)FengGameManagerMKII.Settings[145]);
                PlayerPrefs.SetString("trail2", (string)FengGameManagerMKII.Settings[146]);
                PlayerPrefs.SetString("horse3", (string)FengGameManagerMKII.Settings[147]);
                PlayerPrefs.SetString("hair3", (string)FengGameManagerMKII.Settings[148]);
                PlayerPrefs.SetString("eye3", (string)FengGameManagerMKII.Settings[149]);
                PlayerPrefs.SetString("glass3", (string)FengGameManagerMKII.Settings[150]);
                PlayerPrefs.SetString("face3", (string)FengGameManagerMKII.Settings[151]);
                PlayerPrefs.SetString("skin3", (string)FengGameManagerMKII.Settings[152]);
                PlayerPrefs.SetString("costume3", (string)FengGameManagerMKII.Settings[153]);
                PlayerPrefs.SetString("logo3", (string)FengGameManagerMKII.Settings[154]);
                PlayerPrefs.SetString("bladel3", (string)FengGameManagerMKII.Settings[155]);
                PlayerPrefs.SetString("blader3", (string)FengGameManagerMKII.Settings[156]);
                PlayerPrefs.SetString("gas3", (string)FengGameManagerMKII.Settings[157]);
                PlayerPrefs.SetString("hoodie3", (string)FengGameManagerMKII.Settings[158]);
                PlayerPrefs.SetString("trail3", (string)FengGameManagerMKII.Settings[159]);
                PlayerPrefs.SetString("customGround", (string)FengGameManagerMKII.Settings[162]);
                PlayerPrefs.SetString("forestskyfront", (string)FengGameManagerMKII.Settings[163]);
                PlayerPrefs.SetString("forestskyback", (string)FengGameManagerMKII.Settings[164]);
                PlayerPrefs.SetString("forestskyleft", (string)FengGameManagerMKII.Settings[165]);
                PlayerPrefs.SetString("forestskyright", (string)FengGameManagerMKII.Settings[166]);
                PlayerPrefs.SetString("forestskyup", (string)FengGameManagerMKII.Settings[167]);
                PlayerPrefs.SetString("forestskydown", (string)FengGameManagerMKII.Settings[168]);
                PlayerPrefs.SetString("cityskyfront", (string)FengGameManagerMKII.Settings[169]);
                PlayerPrefs.SetString("cityskyback", (string)FengGameManagerMKII.Settings[170]);
                PlayerPrefs.SetString("cityskyleft", (string)FengGameManagerMKII.Settings[171]);
                PlayerPrefs.SetString("cityskyright", (string)FengGameManagerMKII.Settings[172]);
                PlayerPrefs.SetString("cityskyup", (string)FengGameManagerMKII.Settings[173]);
                PlayerPrefs.SetString("cityskydown", (string)FengGameManagerMKII.Settings[174]);
                PlayerPrefs.SetString("customskyfront", (string)FengGameManagerMKII.Settings[175]);
                PlayerPrefs.SetString("customskyback", (string)FengGameManagerMKII.Settings[176]);
                PlayerPrefs.SetString("customskyleft", (string)FengGameManagerMKII.Settings[177]);
                PlayerPrefs.SetString("customskyright", (string)FengGameManagerMKII.Settings[178]);
                PlayerPrefs.SetString("customskyup", (string)FengGameManagerMKII.Settings[179]);
                PlayerPrefs.SetString("customskydown", (string)FengGameManagerMKII.Settings[180]);
                PlayerPrefs.SetInt("dashenable", (int)FengGameManagerMKII.Settings[181]);
                PlayerPrefs.SetString("dashkey", (string)FengGameManagerMKII.Settings[182]);
                PlayerPrefs.SetInt("vsync", (int)FengGameManagerMKII.Settings[183]);
                PlayerPrefs.SetString("fpscap", (string)FengGameManagerMKII.Settings[184]);
                PlayerPrefs.SetInt("speedometer", (int)FengGameManagerMKII.Settings[189]);
                PlayerPrefs.SetInt("bombMode", (int)FengGameManagerMKII.Settings[192]);
                PlayerPrefs.SetInt("teamMode", (int)FengGameManagerMKII.Settings[193]);
                PlayerPrefs.SetInt("rockThrow", (int)FengGameManagerMKII.Settings[194]);
                PlayerPrefs.SetInt("explodeModeOn", (int)FengGameManagerMKII.Settings[195]);
                PlayerPrefs.SetString("explodeModeNum", (string)FengGameManagerMKII.Settings[196]);
                PlayerPrefs.SetInt("healthMode", (int)FengGameManagerMKII.Settings[197]);
                PlayerPrefs.SetString("healthLower", (string)FengGameManagerMKII.Settings[198]);
                PlayerPrefs.SetString("healthUpper", (string)FengGameManagerMKII.Settings[199]);
                PlayerPrefs.SetInt("infectionModeOn", (int)FengGameManagerMKII.Settings[200]);
                PlayerPrefs.SetString("infectionModeNum", (string)FengGameManagerMKII.Settings[201]);
                PlayerPrefs.SetInt("banEren", (int)FengGameManagerMKII.Settings[202]);
                PlayerPrefs.SetInt("moreTitanOn", (int)FengGameManagerMKII.Settings[203]);
                PlayerPrefs.SetString("moreTitanNum", (string)FengGameManagerMKII.Settings[204]);
                PlayerPrefs.SetInt("damageModeOn", (int)FengGameManagerMKII.Settings[205]);
                PlayerPrefs.SetString("damageModeNum", (string)FengGameManagerMKII.Settings[206]);
                PlayerPrefs.SetInt("sizeMode", (int)FengGameManagerMKII.Settings[207]);
                PlayerPrefs.SetString("sizeLower", (string)FengGameManagerMKII.Settings[208]);
                PlayerPrefs.SetString("sizeUpper", (string)FengGameManagerMKII.Settings[209]);
                PlayerPrefs.SetInt("spawnModeOn", (int)FengGameManagerMKII.Settings[210]);
                PlayerPrefs.SetString("nRate", (string)FengGameManagerMKII.Settings[211]);
                PlayerPrefs.SetString("aRate", (string)FengGameManagerMKII.Settings[212]);
                PlayerPrefs.SetString("jRate", (string)FengGameManagerMKII.Settings[213]);
                PlayerPrefs.SetString("cRate", (string)FengGameManagerMKII.Settings[214]);
                PlayerPrefs.SetString("pRate", (string)FengGameManagerMKII.Settings[215]);
                PlayerPrefs.SetInt("horseMode", (int)FengGameManagerMKII.Settings[216]);
                PlayerPrefs.SetInt("waveModeOn", (int)FengGameManagerMKII.Settings[217]);
                PlayerPrefs.SetString("waveModeNum", (string)FengGameManagerMKII.Settings[218]);
                PlayerPrefs.SetInt("friendlyMode", (int)FengGameManagerMKII.Settings[219]);
                PlayerPrefs.SetInt("pvpMode", (int)FengGameManagerMKII.Settings[220]);
                PlayerPrefs.SetInt("maxWaveOn", (int)FengGameManagerMKII.Settings[221]);
                PlayerPrefs.SetString("maxWaveNum", (string)FengGameManagerMKII.Settings[222]);
                PlayerPrefs.SetInt("endlessModeOn", (int)FengGameManagerMKII.Settings[223]);
                PlayerPrefs.SetString("endlessModeNum", (string)FengGameManagerMKII.Settings[224]);
                PlayerPrefs.SetString("motd", (string)FengGameManagerMKII.Settings[225]);
                PlayerPrefs.SetInt("pointModeOn", (int)FengGameManagerMKII.Settings[226]);
                PlayerPrefs.SetString("pointModeNum", (string)FengGameManagerMKII.Settings[227]);
                PlayerPrefs.SetInt("ahssReload", (int)FengGameManagerMKII.Settings[228]);
                PlayerPrefs.SetInt("punkWaves", (int)FengGameManagerMKII.Settings[229]);
                PlayerPrefs.SetInt("mapOn", (int)FengGameManagerMKII.Settings[231]);
                PlayerPrefs.SetString("mapMaximize", (string)FengGameManagerMKII.Settings[232]);
                PlayerPrefs.SetString("mapToggle", (string)FengGameManagerMKII.Settings[233]);
                PlayerPrefs.SetString("mapReset", (string)FengGameManagerMKII.Settings[234]);
                PlayerPrefs.SetInt("globalDisableMinimap", (int)FengGameManagerMKII.Settings[235]);
                PlayerPrefs.SetString("chatRebind", (string)FengGameManagerMKII.Settings[236]);
                PlayerPrefs.SetString("hforward", (string)FengGameManagerMKII.Settings[237]);
                PlayerPrefs.SetString("hback", (string)FengGameManagerMKII.Settings[238]);
                PlayerPrefs.SetString("hleft", (string)FengGameManagerMKII.Settings[239]);
                PlayerPrefs.SetString("hright", (string)FengGameManagerMKII.Settings[240]);
                PlayerPrefs.SetString("hwalk", (string)FengGameManagerMKII.Settings[241]);
                PlayerPrefs.SetString("hjump", (string)FengGameManagerMKII.Settings[242]);
                PlayerPrefs.SetString("hmount", (string)FengGameManagerMKII.Settings[243]);
                PlayerPrefs.SetInt("chatfeed", (int)FengGameManagerMKII.Settings[244]);
                PlayerPrefs.SetFloat("bombR", (float)FengGameManagerMKII.Settings[246]);
                PlayerPrefs.SetFloat("bombG", (float)FengGameManagerMKII.Settings[247]);
                PlayerPrefs.SetFloat("bombB", (float)FengGameManagerMKII.Settings[248]);
                PlayerPrefs.SetFloat("bombA", (float)FengGameManagerMKII.Settings[249]);
                PlayerPrefs.SetFloat("bombRadius", (float)FengGameManagerMKII.Settings[250]);
                PlayerPrefs.SetFloat("bombRange", (float)FengGameManagerMKII.Settings[251]);
                PlayerPrefs.SetFloat("bombSpeed", (float)FengGameManagerMKII.Settings[252]);
                PlayerPrefs.SetFloat("bombCD", (float)FengGameManagerMKII.Settings[253]);
                PlayerPrefs.SetString("cannonUp", (string)FengGameManagerMKII.Settings[254]);
                PlayerPrefs.SetString("cannonDown", (string)FengGameManagerMKII.Settings[255]);
                PlayerPrefs.SetString("cannonLeft", (string)FengGameManagerMKII.Settings[256]);
                PlayerPrefs.SetString("cannonRight", (string)FengGameManagerMKII.Settings[257]);
                PlayerPrefs.SetString("cannonFire", (string)FengGameManagerMKII.Settings[258]);
                PlayerPrefs.SetString("cannonMount", (string)FengGameManagerMKII.Settings[259]);
                PlayerPrefs.SetString("cannonSlow", (string)FengGameManagerMKII.Settings[260]);
                PlayerPrefs.SetInt("deadlyCannon", (int)FengGameManagerMKII.Settings[261]);
                PlayerPrefs.SetString("liveCam", (string)FengGameManagerMKII.Settings[262]);
                FengGameManagerMKII.Settings[64] = 4;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 463f, halfMenuHeight + 468f, 40f, 25f), "Load"))
            {
                fgm.LoadConfig();
                FengGameManagerMKII.Settings[64] = 5;
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 508f, halfMenuHeight + 468f, 60f, 25f), "Default"))
            {
                GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().SetToDefault();
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 573f, halfMenuHeight + 468f, 75f, 25f), "Continue"))
            {
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                {
                    Time.timeScale = 1f;
                }
                if (!fgm.mainCamera.enabled)
                {
                    Screen.showCursor = true;
                    Screen.lockCursor = true;
                    GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
                    Camera.main.GetComponent<SpectatorMovement>().disable = false;
                    Camera.main.GetComponent<MouseLook>().disable = false;
                    return;
                }
                IN_GAME_MAIN_CAMERA.IsPausing = false;
                if (IN_GAME_MAIN_CAMERA.CameraMode == CameraType.TPS)
                {
                    Screen.showCursor = false;
                    Screen.lockCursor = true;
                }
                else
                {
                    Screen.showCursor = false;
                    Screen.lockCursor = false;
                }
                GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
                GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().JustUpdate();
            }
            else if (GUI.Button(new Rect(halfMenuWidth + 653f, halfMenuHeight + 468f, 40f, 25f), "Quit"))
            {
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer)
                {
                    Time.timeScale = 1f;
                }
                else
                {
                    PhotonNetwork.Disconnect();
                }
                Screen.lockCursor = false;
                Screen.showCursor = true;
                IN_GAME_MAIN_CAMERA.Gametype = GameType.Stop;
                fgm.gameStart = false;
                GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
                fgm.DestroyAllExistingCloths();
                UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
                Application.LoadLevel("menu");
            }
        }
    }
}
