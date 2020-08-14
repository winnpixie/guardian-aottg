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
    private static bool isGAMEFirstLaunch = true;
    public static string version = "01042015";
    public static string fengVersion;

    private void Start()
    {
        string text = "8/12/2015";
        string versionForm = "08122015";
        fengVersion = "01042015";
        NGUITools.SetActive(panelMain, state: true);
        if (version == null || version.StartsWith("error"))
        {
            GameObject.Find("VERSION").GetComponent<UILabel>().text = "Verification failed. Please clear your cache or try another browser";
        }
        else if (version.StartsWith("outdated"))
        {
            GameObject.Find("VERSION").GetComponent<UILabel>().text = "Mod is outdated. Please clear your cache or try another browser.";
        }
        else
        {
            GameObject.Find("VERSION").GetComponent<UILabel>().text = "Client verified. Last updated " + text + ".";
        }
        if (isGAMEFirstLaunch)
        {
            version = fengVersion;
            isGAMEFirstLaunch = false;
            GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("InputManagerController"));
            gameObject.name = "InputManagerController";
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            GameObject.Find("VERSION").GetComponent<UILabel>().text = "Client verified. Last updated " + text + ".";
            FengGameManagerMKII.S = "verified343,hair,character_eye,glass,character_face,character_head,character_hand,character_body,character_arm,character_leg,character_chest,character_cape,character_brand,character_3dmg,r,character_blade_l,character_3dmg_gas_r,character_blade_r,3dmg_smoke,HORSE,hair,body_001,Cube,Plane_031,mikasa_asset,character_cap_,character_gun".Split(',');
            StartCoroutine(Request(text, versionForm));
            FengGameManagerMKII.LoginState = 0;
        }
    }

    public IEnumerator Request(string versionShow, string versionForm)
    {
        string bundleUrl = Application.dataPath + "/RCAssets.unity3d";
        if (!Application.isWebPlayer)
        {
            bundleUrl = "file://" + bundleUrl;
        }
        while (!Caching.ready)
        {
            yield return null;
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
