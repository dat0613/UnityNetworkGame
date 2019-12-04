using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MinNetforUnity;

public class Launcher : MonoBehaviourMinNetCallBack
{
    public GameObject obj;
    
    void Start()
    {
        // MinNetUser.ConnectToServer("127.0.0.1", 8300, (except)=>{
        //     if(except == null)
        //     {
        //         Debug.Log("서버와 연결되었습니다");
        //     }
        //     else
        //     {
        //         Debug.Log("현재 접속 가능한 서버가 없습니다.");
        //     }
        // });

        MinNetUser.Instantiate(obj, new Vector3(0.0f, 10.0f, 0.0f), Quaternion.identity);
    }

    public override void UserEnterRoom(int roomNumber, string roomName)
    {
        // Debug.Log(roomName + " 에 들어옴 : " + roomNumber);
        
        // if(roomNumber == -1)
        // {
        //     MinNetUser.EnterRoom("BattleField");
        // }
        // if(roomName.Equals("BattleField"))
        // {   
        //     MinNetUser.Instantiate(obj, new Vector3(0.0f, 10.0f, 0.0f), Quaternion.identity);
        // }
    }

    void OnDestroy()
    {
        // MinNetUser.DisconnectToServer();    
    }
}
