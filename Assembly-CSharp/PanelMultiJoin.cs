using System;
using System.Collections;
using UnityEngine;

public class PanelMultiJoin : MonoBehaviour
{
    public GameObject[] items;
    private int currentPage = 1;
    private int totalPage = 1;
    private float elapsedTime = 10f;
    private ArrayList filterRoom;
    private string filter = string.Empty;
    private UILabel pageLabel;

    private void Start()
    {
        pageLabel = GameObject.Find("LabelServerListPage").GetComponent<UILabel>();

        for (int num = 0; num < 10; num++)
        {
            items[num].SetActive(value: true);
            items[num].GetComponentInChildren<UILabel>().text = string.Empty;
            items[num].SetActive(value: false);
        }
    }

    private void showlist()
    {
        if (filter == string.Empty)
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
            updateFilterRooms();
            if (filterRoom.Count > 0)
            {
                totalPage = (filterRoom.Count - 1) / 10 + 1;
            }
            else
            {
                totalPage = 1;
            }
        }
        if (currentPage < 1)
        {
            currentPage = totalPage;
        }
        if (currentPage > totalPage)
        {
            currentPage = 1;
        }
        showServerList();
    }

    private string GetServerDataString(RoomInfo room)
    {
        string[] info = room.name.Split('`');
        if (info.Length < 7)
        {
            return "[FF0000]Insufficient room data to be playable.";
        }

        string pwd = info[5].Length == 0 ? string.Empty : "[FF0000](Pwd)[-] ";

        string difficulty = info[2];
        if (difficulty.Equals("normal", StringComparison.OrdinalIgnoreCase))
        {
            difficulty = $"[00FF00]Normal[-]";
        }
        else if (difficulty.Equals("hard", StringComparison.OrdinalIgnoreCase))
        {
            difficulty = $"[FF0000]Hard[-]";
        }
        else if (difficulty.Equals("abnormal", StringComparison.OrdinalIgnoreCase))
        {
            difficulty = $"[000000]Abnormal[-]";
        }

        string daylight = info[4];
        if (daylight.Equals("day", StringComparison.OrdinalIgnoreCase))
        {
            daylight = $"[FFFF00]Day[-]";
        }
        else if (daylight.Equals("dawn", StringComparison.OrdinalIgnoreCase))
        {
            daylight = $"[FF6600]Dawn[-]";
        }
        else if (daylight.Equals("night", StringComparison.OrdinalIgnoreCase))
        {
            daylight = $"[000000]Night[-]";
        }

        string roomMeta = string.Empty;
        if (!room.open || (room.maxPlayers != 0 && room.playerCount >= room.maxPlayers))
        {
            roomMeta = "[FF0000]";
        }
        roomMeta += $"{room.playerCount}/{room.maxPlayers}[-]";

        return $"{pwd}{info[0]}[-][FFFFFF]/{info[1]}/{difficulty}/{daylight} {roomMeta}";
    }

    private void showServerList()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        if (rooms.Length != 0)
        {
            if (filter == string.Empty)
            {
                for (int i = 0; i < 10; i++)
                {
                    int index = 10 * (currentPage - 1) + i;
                    if (index < rooms.Length)
                    {
                        RoomInfo roomInfo = rooms[index];
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
        pageLabel.text = currentPage + "/" + totalPage;
    }

    public void pageUp()
    {
        currentPage--;
        if (currentPage < 1)
        {
            currentPage = totalPage;
        }
        showServerList();
    }

    public void pageDown()
    {
        currentPage++;
        if (currentPage > totalPage)
        {
            currentPage = 1;
        }
        showServerList();
    }

    private void OnEnable()
    {
        currentPage = 1;
        totalPage = 0;
        refresh();
    }

    public void refresh()
    {
        showlist();
    }

    public void connectToIndex(int index, string roomName)
    {
        for (int num = 0; num < 10; num++)
        {
            items[num].SetActive(false);
        }
        string[] array = roomName.Split('`');
        if (array.Length > 6)
        {
            if (array[5] != string.Empty)
            {
                PanelMultiJoinPWD.Password = array[5];
                PanelMultiJoinPWD.roomName = roomName;

                UIMainReferences ui = GameObject.Find("UIRefer").GetComponent<UIMainReferences>();
                NGUITools.SetActive(ui.PanelMultiPWD, state: true);
                NGUITools.SetActive(ui.panelMultiROOM, state: false);
            }
            else
            {
                PhotonNetwork.JoinRoom(roomName);
            }
        }
    }

    private void OnFilterSubmit(string content)
    {
        filter = content;
        updateFilterRooms();
        showlist();
    }

    private void updateFilterRooms()
    {
        filterRoom = new ArrayList();
        if (filter.Length != 0)
        {
            RoomInfo[] roomList = PhotonNetwork.GetRoomList();
            foreach (RoomInfo roomInfo in roomList)
            {
                if (roomInfo.name.ToUpper().Contains(filter.ToUpper()))
                {
                    filterRoom.Add(roomInfo);
                }
            }
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 1f)
        {
            elapsedTime = 0f;
            showlist();
        }
    }
}
