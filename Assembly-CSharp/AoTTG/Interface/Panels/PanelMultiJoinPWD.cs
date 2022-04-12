using UnityEngine;

public class PanelMultiJoinPWD : MonoBehaviour
{
    public static string Password = string.Empty;
    public static string RoomName = string.Empty;

    private void OnEnable()
    {
        GameObject.Find("InputEnterPWD").GetComponent<UIInput>().maxChars = short.MaxValue;
    }
}
