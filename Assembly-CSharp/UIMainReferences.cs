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

    public static Texture2D AOT_2_LOGO;

    private void Start()
    {
        string rcBuild = "8/12/2015";

        NGUITools.SetActive(panelMain, state: true);
        GameObject.Find("VERSION").GetComponent<UILabel>().text = "[9999FF]RC " + rcBuild + "[-] | [0099FF]Guardian " + Guardian.Mod.Build;

        if (IsFirstLaunch)
        {
            IsFirstLaunch = false;

            Version = FengVersion;
            GameObject inputController = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("InputManagerController"));
            inputController.name = "InputManagerController";
            UnityEngine.Object.DontDestroyOnLoad(inputController);
            LoginFengKAI.LoginState = LoginState.LoggedOut;

            StartCoroutine(CoLoadAssets());
        }
    }

    private IEnumerator CoLoadAssets()
    {
        AssetBundleCreateRequest abcr = AssetBundle.CreateFromMemory(File.ReadAllBytes(Application.dataPath + "/RCAssets.unity3d"));
        yield return abcr;
        FengGameManagerMKII.RCAssets = abcr.assetBundle;

        using (WWW www = new WWW("file:///" + Application.dataPath + "/Resources/Textures/patreon.png"))
        {
            yield return www;
            AOT_2_LOGO = www.texture;
        }

        FengGameManagerMKII.IsAssetLoaded = true;
    }
}
