using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class UIMainReferences : MonoBehaviour
{
    public GameObject panelMain;
    public GameObject panelOption;
    public GameObject panelMultiROOM;
    public GameObject PanelMultiJoinPrivate;
    public GameObject PanelMultiWait;
    public GameObject PanelDisconnect;
    public GameObject panelMultiSet;
    public GameObject panelMultiStart;
    public GameObject panelCredits;
    public GameObject panelSingleSet;
    public GameObject PanelMultiPWD;
    public GameObject PanelSnapShot;
    private static bool IsFirstLaunch = true;
    public static string Version = "01042015";
    public static string FengVersion = "01042015";

    private void Start()
    {
        string rcBuild = "8/12/2015";

        NGUITools.SetActive(panelMain, state: true);
        UILabel versionText = GameObject.Find("VERSION").GetComponent<UILabel>();
        if (Version.StartsWith("outdated"))
        {
            versionText.text = "[ff0000]Mod is outdated![-] Please download the latest build from [0099ff]https://tiny.cc/GuardianMod[-]!";
        }
        else
        {
            versionText.text = "Client Verified | [9999ff]RC " + rcBuild + "[-] | [ffff00]Guardian " + Guardian.Mod.Build;
        }

        if (IsFirstLaunch)
        {
            Version = FengVersion;
            IsFirstLaunch = false;
            GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("InputManagerController"));
            gameObject.name = "InputManagerController";
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            FengGameManagerMKII.S = "verified343,hair,character_eye,glass,character_face,character_head,character_hand,character_body,character_arm,character_leg,character_chest,character_cape,character_brand,character_3dmg,r,character_blade_l,character_3dmg_gas_r,character_blade_r,3dmg_smoke,HORSE,hair,body_001,Cube,Plane_031,mikasa_asset,character_cap_,character_gun".Split(',');
            LoginFengKAI.LoginState = LoginState.LoggedOut;

            StartCoroutine(LoadCustomAssets());
        }
    }

    private IEnumerator LoadCustomAssets()
    {
        string url = Application.dataPath + "/RCAssets.unity3d";

        if (!Application.isPlaying)
        {
            url = "file://" + url;
        }

        while (!Caching.ready)
        {
            yield return 0;
        }

        using (WWW www = WWW.LoadFromCacheOrDownload(url, 1))
        {
            yield return www;
            FengGameManagerMKII.RCAssets = www.assetBundle;
            FengGameManagerMKII.IsAssetLoaded = true;
        }
    }
}
