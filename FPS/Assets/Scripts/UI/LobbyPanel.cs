using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using MinNetforUnity;

public class LobbyPanel : MonoBehaviourMinNetCallBack
{
    public LobbyUser lobbyUserPrefab;
    public Room roomPrefab;
    public Notification notificationPrefab;

    private LobbyUser myUser;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    GameObject roomList;

    [SerializeField]
    CreateRoomPanel createRoomPanel;

    public void Refresh()
    {
        var childCount = roomList.transform.childCount;

        for(int i = 1; i < childCount; i++)
        {// 새로고침 할때 기존에 있던 룸을 전부 없앰
            Destroy(roomList.transform.GetChild(i).gameObject);
        }

        roomList.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void CreateRoom()
    {
        createRoomPanel.gameObject.SetActive(true);        
    }

    public void AddRoom(string roomName, string roomState, int roomId, int nowUser, int maxUser)
    {
        roomList.transform.GetChild(0).gameObject.SetActive(false);
        var room = Instantiate(roomPrefab);
        room.transform.SetParent(roomList.transform);
        room.SetLocalScale();
        room.transform.SetAsLastSibling();

        room.SetOption(roomName, roomState, roomId, nowUser, maxUser);
    }

    public override void UserEnterRoom(int roomNumber, string roomName)
    {
        if(roomName == "Lobby")
        {
            if(lobbyUserPrefab == null)
            {
            }
            else
            {
                myUser = MinNetUser.Instantiate(lobbyUserPrefab, Vector3.zero, Quaternion.identity);
            }
        }
    }

    public override void UserEnterRoomFail(int roomNumber, string reason)
    {
        LoadingPanel.Instance.LoadingStop();

        var notification = Instantiate(notificationPrefab);

        notification.transform.SetParent(canvas.transform);
        notification.transform.SetAsLastSibling();
        notification.SetLocalScale();

        notification.SetText(reason);

        myUser.Refresh();
    }
}