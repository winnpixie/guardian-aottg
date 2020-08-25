using System;
using System.Collections;
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
        if (Version == null || Version.StartsWith("error"))
        {
            versionText.text = "[ff0000]Verification failed.[-] Please try re-downloading Guardian from [0099ff]https://tiny.cc/GuardianMod[-]!";
        }
        else if (Version.StartsWith("outdated"))
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
            StartCoroutine(LoadRCAssets());
            FengGameManagerMKII.LoginState = 0;
        }
    }

    public IEnumerator LoadRCAssets()
    {
        string bundleUrl = Application.dataPath + "\\RCAssets.unity3d";
        if (!Application.isWebPlayer)
        {
            bundleUrl = "file://" + bundleUrl;
        }
        while (!Caching.ready)
        {
        }
        int assetVersion = 1;
        using (WWW www = WWW.LoadFromCacheOrDownload(bundleUrl, assetVersion))
        {
            yield return www;
            if (www.error != null)
            {
                throw new Exception("WWW download had an error:" + www.error);
            }
            FengGameManagerMKII.RCAssets = www.assetBundle;
            FengGameManagerMKII.IsAssetLoadeed = true;
        }
    }
}
