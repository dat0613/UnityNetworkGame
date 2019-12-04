using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using MinNetforUnity;

public class ReadyUser : MonoBehaviourMinNet
{
    ChattingWindow chattingWindow;
    ReadyRoomPanel readyRoomPanel;

    PlayerMove.Team team = PlayerMove.Team.None;

    public Notification notificationPrefab;

    public string nickName = "";

    public bool isMaster = false;

    Button StartButton = null;
    Canvas canvas = null;

    public void ChatCast(string chat)
    {
        chattingWindow.AddChat(chat);
    }

    void Awake()
    {
        var buttonLayout = GameObject.Find("ButtonLayout");
        StartButton = buttonLayout.transform.GetChild(0).GetComponent<Button>();
    }

    public override void OnSetID(int objectID)
    {
        chattingWindow = GameObject.Find("LogWindow").GetComponent<ChattingWindow>();
        readyRoomPanel = GameObject.Find("ReadyRoomPanel").GetComponent<ReadyRoomPanel>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        
        if(isMine)
        {
            StartButton.onClick.AddListener(delegate { ClickStartButton(); });
            GameObject.Find("ChangeTeamButton").GetComponent<Button>().onClick.AddListener(delegate { ClickChangeTeamButton(); });
            chattingWindow.myUser = this;
        }
    }

    public void ClickStartButton()
    {
        if(isMaster)
        {
            RPC("GameStart", MinNetRpcTarget.Server);
        }
    }

    public void ClickChangeTeamButton()
    {
        RPC("ChangeTeam", MinNetRpcTarget.Server);
    }

    public void SetNickName(string nickName)
    {
        this.nickName = nickName;

        readyRoomPanel.Reload(team);
    }

    public void SetMaster(bool isMaster)
    {
        this.isMaster = isMaster;
        readyRoomPanel.Reload(team);
        readyRoomPanel.SetMaster(isMaster);
        
        if(isMine)
            StartButton.gameObject.SetActive(isMaster);
    }

    public void SetTeam(int teamNumber)
    {
        var team = (PlayerMove.Team)teamNumber;

        this.team = team;

        readyRoomPanel.AddUser(this);
    }

    public PlayerMove.Team GetTeam() 
    {
        return team;
    }

    void OnDestroy()
    {
        readyRoomPanel.DelUser(this);
    }

    public void CantStartGame(string reason)
    {
        chattingWindow.AddChat(reason, new Color(223.0f / 255.0f, 100.0f / 255.0f, 100.0f / 255.0f, 1.0f));

        var notification = Instantiate(notificationPrefab, canvas.transform);
        notification.transform.SetAsLastSibling();
        notification.SetLocalScale();
        notification.SetText(reason);
    }
}
