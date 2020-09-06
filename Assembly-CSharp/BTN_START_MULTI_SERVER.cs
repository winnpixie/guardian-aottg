using Guardian.Utilities;
using UnityEngine;

public class BTN_START_MULTI_SERVER : MonoBehaviour
{
    private void OnClick()
    {
        string name = GameObject.Find("InputServerName").GetComponent<UIInput>().label.text;
        int max = int.Parse(GameObject.Find("InputMaxPlayer").GetComponent<UIInput>().label.text);
        int time = int.Parse(GameObject.Find("InputMaxTime").GetComponent<UIInput>().label.text);
        string map = GameObject.Find("PopupListMap").GetComponent<UIPopupList>().selection;
        string difficulty = GameObject.Find("CheckboxHard").GetComponent<UICheckbox>().isChecked ? "hard" : ((!GameObject.Find("CheckboxAbnormal").GetComponent<UICheckbox>().isChecked) ? "normal" : "abnormal");
        string daylight = IN_GAME_MAIN_CAMERA.DayLight.ToString().ToLower();
        string password = GameObject.Find("InputStartServerPWD").GetComponent<UIInput>().label.text;
        if (password.Length > 0)
        {
            password = new SimpleAES().Encrypt(password);
        }
        name = name + "`" + map + "`" + difficulty + "`" + time + "`" + daylight + "`" + password + "`" + MathHelper.RandomInt(int.MinValue, int.MaxValue);
        PhotonNetwork.CreateRoom(name, new RoomOptions
        {
            maxPlayers = max,
            customRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "Map", map },
                { "DayLight", daylight.ToUpper() }
            }
        }, TypedLobby.Default);
    }
}
