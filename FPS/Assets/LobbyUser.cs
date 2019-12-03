using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MinNetforUnity;
using UnityEngine.UI;

public class LobbyUser : MonoBehaviourMinNet
{
    LobbyPanel lobbyPanel;

    void Awake()
    {
        var panelObject = GameObject.Find("LobbyPanel");
        if(panelObject == null)
        {
            Debug.Log("패널 못찾음");
            return;
        }

        lobbyPanel = panelObject.GetComponent<LobbyPanel>();

        var buttonObject = GameObject.Find("RefreshButton");
        if(buttonObject == null)
        {
            Debug.Log("버튼 못찾음");
            return;
        }

        buttonObject.GetComponent<Button>().onClick.AddListener(
            ()=>
            {
                Refresh();
            });
    }

    public override void OnSetID(int objectID)
    {
        Refresh();
    }

    public void Refresh()
    {
        // Debug.Log("새로고침 누름");
        lobbyPanel.Refresh();
        RPC("GetRoomList", MinNetRpcTarget.Server);
    }

    public void AddRoom(string roomName, string roomState, int roomId, int nowUser, int maxUser)
    {
        lobbyPanel.AddRoom(roomName, roomState, roomId, nowUser, maxUser);
    }
}