using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    GameTimer timer;

    [SerializeField]
    LogWindow logWindow;

    bool onGame = false;
    public bool skip1Frame = false;// 엔터를 한번 더 쳐서 채팅을 끝냈을때 엔터 이벤트가 한 프레임 동안 남아 있어서 한 프레임 스킵 해야함

    static UiManager instance = null;
    public static UiManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !skip1Frame)
        {
            switch (InputManager.Instance.Focus)
            {
                case InputManager.InputFocus.CharacterControl:// 게임 도중 엔터를 눌렀다면 채팅을 시작함
                    ChattingMode();
                    InputManager.Instance.ChangeFocus(InputManager.InputFocus.Chatting);
                    break;

                case InputManager.InputFocus.Chatting:// 채팅을 끝내고 게임으로 돌아감
                    //ChattingMode();
                    //InputManager.Instance.ChangeFocus(InputManager.InputFocus.CharacterControl);
                    break;

                case InputManager.InputFocus.Select:


                    break;
            }
        }
        skip1Frame = false;
    }

    public void SetTimer(float second)
    {
        timer.SetTimer(second);
    }

    public void ChattingMode()
    {
        logWindow.ChattingMode();
    }

    public void AddChat(string chat)
    {
        logWindow.AddLog(chat);
    }
}