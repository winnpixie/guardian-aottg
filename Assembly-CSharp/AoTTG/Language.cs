using UnityEngine;

public class Language
{
    public static int type = -1;
    public static string[] btn_single = new string[25];
    public static string[] btn_multiplayer = new string[25];
    public static string[] btn_option = new string[25];
    public static string[] btn_credits = new string[25];
    public static string[] btn_back = new string[25];
    public static string[] btn_refresh = new string[25];
    public static string[] btn_join = new string[25];
    public static string[] btn_start = new string[25];
    public static string[] btn_create_game = new string[25];
    public static string[] btn_LAN = new string[25];
    public static string[] btn_server_US = new string[25];
    public static string[] btn_server_EU = new string[25];
    public static string[] btn_server_ASIA = new string[25];
    public static string[] btn_server_JAPAN = new string[25];
    public static string[] btn_QUICK_MATCH = new string[25];
    public static string[] btn_default = new string[25];
    public static string[] btn_ready = new string[25];
    public static string[] server_name = new string[25];
    public static string[] server_ip = new string[25];
    public static string[] port = new string[25];
    public static string[] choose_map = new string[25];
    public static string[] choose_character = new string[25];
    public static string[] camera_type = new string[25];
    public static string[] camera_original = new string[25];
    public static string[] camera_wow = new string[25];
    public static string[] camera_tps = new string[25];
    public static string[] max_player = new string[25];
    public static string[] max_Time = new string[25];
    public static string[] game_time = new string[25];
    public static string[] difficulty = new string[25];
    public static string[] normal = new string[25];
    public static string[] hard = new string[25];
    public static string[] abnormal = new string[25];
    public static string[] mouse_sensitivity = new string[25];
    public static string[] change_quality = new string[25];
    public static string[] camera_tilt = new string[25];
    public static string[] invert_mouse = new string[25];
    public static string[] waiting_for_input = new string[25];
    public static string[] key_set_info_1 = new string[25];
    public static string[] key_set_info_2 = new string[25];
    public static string[] soldier = new string[25];
    public static string[] titan = new string[25];
    public static string[] select_titan = new string[25];
    public static string[] camera_info = new string[25];
    public static string[] btn_continue = new string[25];
    public static string[] btn_quit = new string[25];
    public static string[] choose_region_server = new string[25];

    public static void Init()
    {
        string text = ((TextAsset)Resources.Load("lang")).text;
        string[] lines = text.Split('\n');
        string lang = string.Empty;
        int langIndex = 0;

        foreach (string line in lines)
        {
            if (line.Contains("//"))
            {
                continue;
            }
            if (line.Contains("#START"))
            {
                lang = line.Split("@"[0])[1];
                langIndex = GetLangIndex(lang);
            }
            else if (line.Contains("#END"))
            {
                lang = string.Empty;
            }
            else if (lang.Length > 0 && line.Contains("@"))
            {
                string key = line.Split('@')[0];
                string value = line.Split('@')[1];

                switch (key)
                {
                    case "btn_single":
                        btn_single[langIndex] = value;
                        break;
                    case "btn_multiplayer":
                        btn_multiplayer[langIndex] = value;
                        break;
                    case "btn_option":
                        btn_option[langIndex] = value;
                        break;
                    case "btn_credits":
                        btn_credits[langIndex] = value;
                        break;
                    case "btn_back":
                        btn_back[langIndex] = value;
                        break;
                    case "btn_refresh":
                        btn_refresh[langIndex] = value;
                        break;
                    case "btn_join":
                        btn_join[langIndex] = value;
                        break;
                    case "btn_start":
                        btn_start[langIndex] = value;
                        break;
                    case "btn_create_game":
                        btn_create_game[langIndex] = value;
                        break;
                    case "btn_LAN":
                        btn_LAN[langIndex] = value;
                        break;
                    case "btn_server_US":
                        btn_server_US[langIndex] = value;
                        break;
                    case "btn_server_EU":
                        btn_server_EU[langIndex] = value;
                        break;
                    case "btn_server_ASIA":
                        btn_server_ASIA[langIndex] = value;
                        break;
                    case "btn_server_JAPAN":
                        btn_server_JAPAN[langIndex] = value;
                        break;
                    case "btn_QUICK_MATCH":
                        btn_QUICK_MATCH[langIndex] = value;
                        break;
                    case "btn_default":
                        btn_default[langIndex] = value;
                        break;
                    case "btn_ready":
                        btn_ready[langIndex] = value;
                        break;
                    case "server_name":
                        server_name[langIndex] = value;
                        break;
                    case "server_ip":
                        server_ip[langIndex] = value;
                        break;
                    case "port":
                        port[langIndex] = value;
                        break;
                    case "choose_map":
                        choose_map[langIndex] = value;
                        break;
                    case "choose_character":
                        choose_character[langIndex] = value;
                        break;
                    case "camera_type":
                        camera_type[langIndex] = value;
                        break;
                    case "camera_original":
                        camera_original[langIndex] = value;
                        break;
                    case "camera_wow":
                        camera_wow[langIndex] = value;
                        break;
                    case "camera_tps":
                        camera_tps[langIndex] = value;
                        break;
                    case "max_player":
                        max_player[langIndex] = value;
                        break;
                    case "max_Time":
                        max_Time[langIndex] = value;
                        break;
                    case "game_time":
                        game_time[langIndex] = value;
                        break;
                    case "difficulty":
                        difficulty[langIndex] = value;
                        break;
                    case "normal":
                        normal[langIndex] = value;
                        break;
                    case "hard":
                        hard[langIndex] = value;
                        break;
                    case "abnormal":
                        abnormal[langIndex] = value;
                        break;
                    case "mouse_sensitivity":
                        mouse_sensitivity[langIndex] = value;
                        break;
                    case "change_quality":
                        change_quality[langIndex] = value;
                        break;
                    case "camera_tilt":
                        camera_tilt[langIndex] = value;
                        break;
                    case "invert_mouse":
                        invert_mouse[langIndex] = value;
                        break;
                    case "waiting_for_input":
                        waiting_for_input[langIndex] = value;
                        break;
                    case "key_set_info_1":
                        key_set_info_1[langIndex] = value;
                        break;
                    case "key_set_info_2":
                        key_set_info_2[langIndex] = value;
                        break;
                    case "soldier":
                        soldier[langIndex] = value;
                        break;
                    case "titan":
                        titan[langIndex] = value;
                        break;
                    case "select_titan":
                        select_titan[langIndex] = value;
                        break;
                    case "camera_info":
                        camera_info[langIndex] = value;
                        break;
                    case "btn_continue":
                        btn_continue[langIndex] = value;
                        break;
                    case "btn_quit":
                        btn_quit[langIndex] = value;
                        break;
                    case "choose_region_server":
                        choose_region_server[langIndex] = value;
                        break;
                }
            }
        }
    }

    public static int GetLangIndex(string lang)
    {
        switch (lang)
        {
            case "ENGLISH":
                return 0;
            case "简体中文":
                return 1;
            case "SPANISH":
                return 2;
            case "POLSKI":
                return 3;
            case "ITALIANO":
                return 4;
            case "NORWEGIAN":
                return 5;
            case "PORTUGUESE":
                return 6;
            case "PORTUGUESE_BR":
                return 7;
            case "繁體中文_台":
                return 8;
            case "繁體中文_港":
                return 9;
            case "SLOVAK":
                return 10;
            case "GERMAN":
                return 11;
            case "FRANCAIS":
                return 12;
            case "TÜRKÇE":
                return 13;
            case "ARABIC":
                return 14;
            case "Thai":
                return 15;
            case "Русский":
                return 16;
            case "NEDERLANDS":
                return 17;
            case "Hebrew":
                return 18;
            case "DANSK":
                return 19;
        }

        return 0;
    }

    public static string GetLang(int id)
    {
        switch (id)
        {
            case 0:
                return "ENGLISH";
            case 1:
                return "简体中文";
            case 2:
                return "SPANISH";
            case 3:
                return "POLSKI";
            case 4:
                return "ITALIANO";
            case 5:
                return "NORWEGIAN";
            case 6:
                return "PORTUGUESE";
            case 7:
                return "PORTUGUESE_BR";
            case 8:
                return "繁體中文_台";
            case 9:
                return "繁體中文_港";
            case 10:
                return "SLOVAK";
            case 11:
                return "GERMAN";
            case 12:
                return "FRANCAIS";
            case 13:
                return "TÜRKÇE";
            case 14:
                return "ARABIC";
            case 15:
                return "Thai";
            case 16:
                return "Русский";
            case 17:
                return "NEDERLANDS";
            case 18:
                return "Hebrew";
            case 19:
                return "DANSK";
            default:
                return "ENGLISH";
        }
    }
}
