using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using MinNetforUnity;

public class ReadyRoomPanel : MonoBehaviourMinNetCallBack
{
    [SerializeField]
    Text roomNameText;

    public ReadyUser readyUserPrefab;

    void Awake()
    {
                   
    }

    void Update()
    {
        
    }

    public override void UserEnterRoom(int roomNumber, string roomName)
    {
        if(roomName == "ReadyRoom")
        {
            MinNetUser.Instantiate(readyUserPrefab);
        }
    }

    public void SetRoomName(string roomName)
    {
        roomNameText.text = roomName;
    }
}
