using System.Collections;
using UnityEngine;

public class PanelMultiJoin : MonoBehaviour
{
    public GameObject[] items;
    private int currentPage = 1;
    private int totalPage = 1;
    private float elapsedTime = 2f;
    private ArrayList filterRoom;
    private string filter = string.Empty;
    private UILabel pageLabel;

    // BEGIN Guardian
    public bool g_hideFullRooms;
    public bool g_hideLockedRooms;
    // END Guardian

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            items[i].SetActive(value: true);
            items[i].GetComponentInChildren<UILabel>().text = string.Empty;
            items[i].SetActive(value: false);
        }
    }

    private void ShowList()
    {
        if (filter.Length == 0)
        {
            RoomInfo[] rooms = PhotonNetwork.GetRoomList();
            if (rooms.Length > 0)
            {
                totalPage = (rooms.Length - 1) / 10 + 1;
            }
            else
            {
                totalPage = 1;
            }
        }
        else
        {
            UpdateFilteredRooms();
            if (filterRoom.Count > 0)
            {
                totalPage = (filterRoom.Count - 1) / 10 + 1;
            }
            else
            {
                totalPage = 1;
            }
        }

        if (currentPage < 1) currentPage = totalPage;
        if (currentPage > totalPage) currentPage = 1;

        ShowServerList();
    }

    private string GetServerDataString(RoomInfo room)
    {
        string[] info = room.name.Split('`');
        if (info.Length < 7) return "[FF0000]Invalid Room.";

        string difficulty = info[2].ToLower() switch
        {
            "normal" => "[00FF00]Normal[-]",
            "hard" => "[FFFF00]Hard[-]",
            "abnormal" => "[FF0000]Abnormal[-]",
            _ => info[2]
        };

        string daylight = info[4].ToLower() switch
        {
            "day" => "[FFFF00]Day[-]",
            "dawn" => "[FF6600]Dawn[-]",
            "night" => "[000000]Night[-]",
            _ => info[4]
        };

        string roomMeta = string.Empty;
        if (!room.open || (room.maxPlayers > 0 && room.playerCount >= room.maxPlayers))
        {
            roomMeta = "[FF0000]";
        }

        roomMeta += $"({room.playerCount}/{room.maxPlayers})";

        string pwd = info[5].Length == 0 ? string.Empty : "[FF0000](Pwd)[-] ";
        return $"{pwd}{info[0]}[-] [AAAAAA]:: [FFFFFF]{info[1]}[AAAAAA] / {difficulty} / {daylight}[-] {roomMeta}";
    }

    private void ShowServerList()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();

        // BEGIN Guardian
        ArrayList g_filteredRooms = new ArrayList();
        foreach (RoomInfo info in rooms)
        {
            if (!ShouldListRoom(info)) continue;

            g_filteredRooms.Add(info);
        }
        // END Guardian

        if (g_filteredRooms.Count > 0)
        {
            if (filter.Length == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int index = 10 * (currentPage - 1) + i;
                    if (index < g_filteredRooms.Count)
                    {
                        RoomInfo roomInfo = (RoomInfo)g_filteredRooms[index];
                        items[i].SetActive(value: true);
                        items[i].GetComponentInChildren<UILabel>().text = GetServerDataString(roomInfo);
                        items[i].GetComponentInChildren<BTN_Connect_To_Server_On_List>().roomName = roomInfo.name;
                    }
                    else
                    {
                        items[i].SetActive(value: false);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int index = 10 * (currentPage - 1) + i;
                    if (index < filterRoom.Count)
                    {
                        RoomInfo roomInfo = (RoomInfo)filterRoom[index];
                        items[i].SetActive(value: true);
                        items[i].GetComponentInChildren<UILabel>().text = GetServerDataString(roomInfo);
                        items[i].GetComponentInChildren<BTN_Connect_To_Server_On_List>().roomName = roomInfo.name;
                    }
                    else
                    {
                        items[i].SetActive(value: false);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].SetActive(value: false);
            }
        }

        if (pageLabel == null)
        {
            pageLabel = GameObject.Find("LabelServerListPage").GetComponent<UILabel>();
        }
        pageLabel.text = currentPage + " / " + totalPage;
    }

    public void PageUp()
    {
        currentPage--;
        if (currentPage < 1)
        {
            currentPage = totalPage;
        }
        ShowServerList();
    }

    public void PageDown()
    {
        currentPage++;
        if (currentPage > totalPage)
        {
            currentPage = 1;
        }
        ShowServerList();
    }

    private void OnEnable()
    {
        currentPage = 1;
        totalPage = 0;
        Refresh();
    }

    public void Refresh()
    {
        ShowList();
    }

    public void ConnectToIndex(int index, string roomName)
    {
        for (int num = 0; num < 10; num++)
        {
            items[num].SetActive(false);
        }
        string[] array = roomName.Split('`');
        if (array.Length < 7) return;

        if (array[5].Length > 0)
        {
            PanelMultiJoinPWD.Password = array[5];
            PanelMultiJoinPWD.RoomName = roomName;

            UIMainReferences ui = GameObject.Find("UIRefer").GetComponent<UIMainReferences>();
            NGUITools.SetActive(ui.PanelMultiPWD, state: true);
            NGUITools.SetActive(ui.panelMultiROOM, state: false);
        }
        else
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    private void OnFilterSubmit(string content)
    {
        filter = content;
        UpdateFilteredRooms();
        ShowList();
    }

    private void UpdateFilteredRooms()
    {
        filterRoom = new ArrayList();
        if (filter.Length == 0) return;

        foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
        {
            // Guardian
            if (!ShouldListRoom(roomInfo)) continue;

            if (!roomInfo.name.ToUpper().Contains(filter.ToUpper())) continue;

            filterRoom.Add(roomInfo);
        }
    }

    // Guardian
    private bool ShouldListRoom(RoomInfo info)
    {
        if (g_hideFullRooms && info.playerCount >= info.maxPlayers && info.maxPlayers > 0) return false;

        if (g_hideLockedRooms)
        {
            string[] data = info.name.Split('`');
            if (data.Length < 7) return true;

            return data[5].Length == 0;
        }

        return true;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 1f)
        {
            elapsedTime = 0f;
            ShowList();
        }
    }
}
