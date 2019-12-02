using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using MinNetforUnity;

public class ChattingWindow : MonoBehaviour
{
    [SerializeField]
    InputField inputField;

    [SerializeField]
    GameObject content;

    [SerializeField]
    LogText logTextPrefab;

    [SerializeField]
    ScrollRect scrollRect;

    [HideInInspector]
    public ReadyUser myUser = null;

    void Awake()
    {
        inputField.onEndEdit.AddListener( delegate{ SendText(); });
    }

    IEnumerator ScrollDown()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 0.0f;
    }

    public void SendText()
    {
        if(myUser != null)
        {
            myUser.RPC("SendChat", MinNetRpcTarget.Server, inputField.text);
            inputField.text = "";
        }
    }

    public void AddChat(string chat)
    {// 채팅창에 채팅 추가
        var logText = Instantiate(logTextPrefab, content.transform);

        logText.transform.SetAsLastSibling();

        logText.SetText(chat);

        StartCoroutine("ScrollDown");
    }
}