using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MinNetforUnity;

public class DummyClient : MonoBehaviourMinNet
{
    void Start()
    {
        // if (isMine)
        //     StartCoroutine("SendInfo");
    }

    IEnumerator SendInfo()
    {
        while(true)
        {
            RPC("SendText", MinNetRpcTarget.AllViaServer, "테스트용 문장 입니다 하하하!@#!@#asdasdf123123");
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            RPC("SendTcp", MinNetRpcTarget.AllViaServer, "이거슨 tcp");
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            RPCudp("SendUdp", MinNetRpcTarget.AllViaServer, "이거슨 udp");
        }

    }

    public void SendText(string text)
    {
        // Debug.Log(text);   
    }

    public void SendUdp(string text)
    {
        Debug.Log("udp : " + text);
    }

    public void SendTcp(string text)
    {
        Debug.Log("tcp : " + text);
    }
}