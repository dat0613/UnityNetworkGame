using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using MinNetforUnity;

public class ReadyRoomPanel : MonoBehaviourMinNetCallBack
{
    [SerializeField]
    Text roomNameText;

    [SerializeField]
    TeamLayout blueTeamLayout;

    [SerializeField]
    TeamLayout redTeamLayout;

    public ReadyUser readyUserPrefab;

    public override void UserEnterRoom(int roomNumber, string roomName)
    {
        if(roomName == "ReadyRoom")
        {
            MinNetUser.Instantiate(readyUserPrefab);
        }
    }

    public void SetMaster(bool isMaster)
    {
        if(!isMaster)
            return;
        
    }

    public void Reload(PlayerMove.Team team)
    {
        if(team == PlayerMove.Team.Blue)
        {
            blueTeamLayout.Reload();
        }
        else
        {
            redTeamLayout.Reload();
        }
    }

    public void ExitButton()
    {
        MinNetUser.EnterRoom("Lobby");
    }

    public void SetRoomName(string roomName)
    {
        roomNameText.text = roomName;
    }

    public void AddUser(ReadyUser user)
    {
        redTeamLayout.DelUser(user);
        blueTeamLayout.DelUser(user);

        if(user.GetTeam() == PlayerMove.Team.Blue)
        {
            blueTeamLayout.AddUser(user);
        }
        else
        {
            redTeamLayout.AddUser(user);
        }
    }

    public void DelUser(ReadyUser user)
    {
        if(user.GetTeam() == PlayerMove.Team.Blue)
        {
            blueTeamLayout.DelUser(user);
        }
        else
        {
            redTeamLayout.DelUser(user);
        }
    }
}