using UnityEngine;

public class CheckBoxShowSSInGame : MonoBehaviour
{
    private bool init;

    private void Start()
    {
        init = true;
        if (PlayerPrefs.HasKey("showSSInGame"))
        {
            if (PlayerPrefs.GetInt("showSSInGame") == 1)
            {
                GetComponent<UICheckbox>().isChecked = true;
            }
            else
            {
                GetComponent<UICheckbox>().isChecked = false;
            }
        }
        else
        {
            GetComponent<UICheckbox>().isChecked = true;
            PlayerPrefs.SetInt("showSSInGame", 1);
        }
    }

    private void OnActivate(bool yes)
    {
        if (init)
        {
            if (yes)
            {
                PlayerPrefs.SetInt("showSSInGame", 1);
            }
            else
            {
                PlayerPrefs.SetInt("showSSInGame", 0);
            }
        }
    }
}
