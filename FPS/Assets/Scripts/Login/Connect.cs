using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using System;


using MinNetforUnity;
using UnityEngine.UI;

public class Connect : MonoBehaviourMinNetCallBack
{
    [SerializeField]
    Canvas canvas;

    [SerializeField]
    InputField ipField;

    [SerializeField]
    InputField nicknameField;

    [SerializeField]
    Button connectButton;

    [SerializeField]
    Notification notificationPrefab;

    private delegate void delegateEvent();

    Queue<delegateEvent> delegateQueue = new Queue<delegateEvent>();

    public void CreateNotification(string text)
    {
        var notification = Instantiate(notificationPrefab);
        notification.transform.SetParent(canvas.transform);
        notification.SetLocalScale();
        notification.transform.SetAsLastSibling();
        notification.SetText(text);
    }

    bool CheckIP()
    {
        string ip = ipField.text;

        if(string.IsNullOrEmpty(ip))
        {// ip 입력하라고 보여주기
            CreateNotification("IP를 입력해 주세요");
            Debug.Log("ip를 입력하지 않았음");
            return false;
        }

        System.Net.IPAddress ipAdress = null;

        try
        {
            ipAdress = System.Net.IPAddress.Parse(ip);
        }
        catch (Exception e)
        {
            if(e != null)
            {// 제대로된 ip를 입력하라고 보여줌
                Debug.Log("알맞지 않은 ip를 입력함");
                CreateNotification("올바른 IP를 입력해 주세요");
                return false;
            }
        }

        return true;
    }

    bool CheckNickName()
    {
        string nickname = nicknameField.text;

        if(string.IsNullOrEmpty(nickname))
        {
            Debug.Log("닉네임을 입력하지 않았음");
            CreateNotification("게임에서 사용할 닉네임을 입력해 주세요");
            return false;        
        }

        return true;
    }

    void Update()
    {
        while(delegateQueue.Count > 0)
        {
            delegateQueue.Dequeue()?.Invoke();
        }
    }

    public void TryConnectToServer()
    {
        if(CheckIP() && CheckNickName())
        {
            Debug.Log("접속 시도");
            
            // 뭔가 패널 같은걸 띄어서 추가 입력을 방지함
            
            LoadingPanel.Instance.LoadingStart(true);

            MinNetUser.ConnectToServer(ipField.text, 8300, (except) =>
            {
                LoadingPanel.Instance.LoadingStop();

                // 여기서 패널을 끔
                if (except == null)
                {

                }
                else
                {
                    Debug.Log("현재 접속 가능한 서버가 없습니다");
                    delegateQueue.Enqueue(()=>
                        {
                            CreateNotification("현재 접속 가능한 서버가 없습니다");
                        }
                    );
                }
            });
        }
    }

    public override void UserEnterRoom(int roomNumber, string roomName)
    {
        if(roomName == "Main")// 서버에 접속하여 메인 룸에 들어옴
        {
            // SceneManager.LoadScene("GameScene");
            MinNetUser.EnterRoom("Lobby");
        }
        
        // if(roomName == "Loby")
        // {

        // }
    }
}