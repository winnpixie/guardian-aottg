using UnityEngine;

public class ChangeQuality : MonoBehaviour
{
    private bool init;
    public static bool isTiltShiftOn;

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
        setQuality(base.gameObject.GetComponent<UISlider>().sliderValue);
    }

    private static void setQuality(float val)
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
        if (val < 0.9f)
        {
            turnOffTiltShift();
        }
        else
        {
            turnOnTiltShift();
        }
    }

    public static void setCurrentQuality()
    {
        if (PlayerPrefs.HasKey("GameQuality"))
        {
            setQuality(PlayerPrefs.GetFloat("GameQuality"));
        }
    }

    public static void turnOffTiltShift()
    {
        isTiltShiftOn = false;
        GameObject mainCam = GameObject.Find("MainCamera");
        if (mainCam != null)
        {
            mainCam.GetComponent<TiltShift>().enabled = false;
        }
    }

    public static void turnOnTiltShift()
    {
        isTiltShiftOn = true;
        GameObject mainCam = GameObject.Find("MainCamera");
        if (mainCam != null)
        {
            mainCam.GetComponent<TiltShift>().enabled = true;
        }
    }
}
