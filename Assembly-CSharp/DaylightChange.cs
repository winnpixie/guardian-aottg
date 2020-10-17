using UnityEngine;

public class DaylightChange : MonoBehaviour
{
    private void OnSelectionChange()
    {
        if(GExtensions.TryParseEnum(GetComponent<UIPopupList>().selection, out DayLight dayLight))
        {
            IN_GAME_MAIN_CAMERA.Time = dayLight;
        }
    }
}
