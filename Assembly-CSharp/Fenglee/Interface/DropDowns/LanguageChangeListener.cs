using UnityEngine;

public class LanguageChangeListener : MonoBehaviour
{
    private UIPopupList languageList;

    void Awake()
    {
        languageList = GetComponent<UIPopupList>();
    }

    private void Start()
    {
        if (Language.type == -1)
        {
            if (PlayerPrefs.HasKey("language"))
            {
                Language.type = PlayerPrefs.GetInt("language");
            }
            else
            {
                PlayerPrefs.SetInt("language", 0);
                Language.type = 0;
            }
            Language.Init();
            languageList.selection = Language.GetLang(Language.type);
        }
        else
        {
            languageList.selection = Language.GetLang(Language.type);
        }
    }

    private void OnSelectionChange()
    {
        Language.type = Language.GetLangIndex(languageList.selection);
        PlayerPrefs.SetInt("language", Language.type);
    }
}
