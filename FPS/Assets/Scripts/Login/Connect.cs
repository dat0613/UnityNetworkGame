using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


using MinNetforUnity;
using UnityEngine.UI;

public class Connect : MonoBehaviour
{
    [SerializeField]
    InputField ipField;

    [SerializeField]
    InputField nicknameField;

    [SerializeField]
    Button connectButton;

    bool CheckIP()
    {
        string ip = ipField.text;

        if(ip == null || ip == "")
        {// ip 입력하라고 보여주기
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
                return false;
            }
        }

        return true;
    }

    bool CheckNickName()
    {
        string nickname = nicknameField.text;

        if(nickname == null || nickname == "")
        {
            Debug.Log("닉네임을 입력하지 않았음");
            return false;        
        }

        return true;
    }

    public void TryConnectToServer()
    {
        if(CheckIP() && CheckNickName())
        {
            Debug.Log("접속 시도");
            
            // 뭔가 패널 같은걸 띄어서 추가 입력을 방지함
            MinNetUser.ConnectToServer(ipField.text, 8300, (except) =>
            {
                // 여기서 패널을 끔
                if (except == null)
                {
                    Debug.Log("서버와 연결되었습니다");
                }
                else
                {
                    Debug.Log("현재 접속 가능한 서버가 없습니다.");
                }

            });
        }
    }
}
