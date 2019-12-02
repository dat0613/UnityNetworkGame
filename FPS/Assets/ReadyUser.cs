using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MinNetforUnity;

public class ReadyUser : MonoBehaviourMinNet
{
    ChattingWindow chattingWindow;

    public void ChatCast(string chat)
    {
        chattingWindow.AddChat(chat);
    }

    public override void OnSetID(int objectID)
    {
        chattingWindow = GameObject.Find("LogWindow").GetComponent<ChattingWindow>();
        
        if(isMine)
        {
            chattingWindow.myUser = this;
        }   
    }
}
