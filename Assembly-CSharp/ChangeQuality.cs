using UnityEngine;

public class ChangeQuality : MonoBehaviour
{
    private bool init;
    public static bool TiltShift;

    private void OnSliderChange()
    {
        if (!init)
        {
            init = true;
            if (PlayerPrefs.HasKey("GameQuality"))
            {
                base.gameObject.GetComponent<UISlider>().sliderValue = PlayerPrefs.GetFloat("GameQuality");
            }
            else
            {
                PlayerPrefs.SetFloat("GameQuality", base.gameObject.GetComponent<UISlider>().sliderValue);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("GameQuality", base.gameObject.GetComponent<UISlider>().sliderValue);
        }
        SetQuality(base.gameObject.GetComponent<UISlider>().sliderValue);
    }

    private static void SetQuality(float val)
    {
        if (val < 0.167f)
        {
            QualitySettings.SetQualityLevel(0, applyExpensiveChanges: true);
        }
        else if (val < 0.33f)
        {
            QualitySettings.SetQualityLevel(1, applyExpensiveChanges: true);
        }
        else if (val < 0.5f)
        {
            QualitySettings.SetQualityLevel(2, applyExpensiveChanges: true);
        }
        else if (val < 0.67f)
        {
            QualitySettings.SetQualityLevel(3, applyExpensiveChanges: true);
        }
        else if (val < 0.83f)
        {
            QualitySettings.SetQualityLevel(4, applyExpensiveChanges: true);
        }
        else if (val <= 1f)
        {
            QualitySettings.SetQualityLevel(5, applyExpensiveChanges: true);
        }

        SetTiltShift(val >= 0.9f);
    }

    public static void SetCurrentQuality()
    {
        if (PlayerPrefs.HasKey("GameQuality"))
        {
            SetQuality(PlayerPrefs.GetFloat("GameQuality"));
        }
    }

    public static void SetTiltShift(bool tiltShift)
    {
        TiltShift = tiltShift;
        GameObject mainCam = GameObject.Find("MainCamera");
        if(mainCam)
        {
            mainCam.GetComponent<TiltShift>().enabled = tiltShift;
        }
    }
}
