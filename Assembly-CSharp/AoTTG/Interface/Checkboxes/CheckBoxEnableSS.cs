using UnityEngine;

public class CheckBoxEnableSS : MonoBehaviour
{
    private bool init;

    private void Start()
    {
        init = true;
        if (PlayerPrefs.HasKey("EnableSS"))
        {
            if (PlayerPrefs.GetInt("EnableSS") == 1)
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
            PlayerPrefs.SetInt("EnableSS", 1);
        }
    }

    private void OnActivate(bool yes)
    {
        if (init)
        {
            if (yes)
            {
                PlayerPrefs.SetInt("EnableSS", 1);
            }
            else
            {
                PlayerPrefs.SetInt("EnableSS", 0);
            }
        }
    }
}
