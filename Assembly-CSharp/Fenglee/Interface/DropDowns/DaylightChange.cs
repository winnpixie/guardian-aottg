using UnityEngine;

public class DaylightChange : MonoBehaviour
{
    private UIPopupList lightingList;

    void Awake()
    {
        lightingList = GetComponent<UIPopupList>();
    }

    private void OnSelectionChange()
    {
        if (GExtensions.TryParseEnum(lightingList.selection, out DayLight dayLight))
        {
            IN_GAME_MAIN_CAMERA.Lighting = dayLight;
        }
    }
}
