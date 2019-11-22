using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class LogWindow : MonoBehaviour
{
    [SerializeField]
    InputField inputField;

    [SerializeField]
    GameObject content;

    [SerializeField]
    LogText logTextPrefab;

    [SerializeField]
    ScrollRect scrollRect;

    public Color defaultColor = Color.black;
    public int defaultSize = 14;
    
    void Awake()
    { 
        inputField.onEndEdit.AddListener(delegate {onEndEdit();});
    }

    public void AddLog(string txt)
    {
        AddLog(txt, defaultColor);
    }

    public void AddLog(string txt, Color color)
    {
        AddLog(txt, color, defaultSize);
    }

    public void AddLog(string txt, Color color, int size)
    {
        var logText = Instantiate(logTextPrefab, content.transform);

        logText.SetSize(size);
        logText.SetColor(color);
        logText.SetText(txt);

        Debug.Log("넣음");

        scrollRect.verticalNormalizedPosition = 0.0f;
        Debug.Log(scrollRect.verticalNormalizedPosition);
    }

    public void onEndEdit()
    {
        InputManager.Instance.ChangeFocus(InputManager.InputFocus.CharacterControl);

        var str = inputField.text;

        if(str.Equals(""))
        {// 빈 텍스트는 보내지 않음
        }
        else
        {
            PlayerManager.GetMyPlayer()?.SendChatting(str);
        }
        inputField.text = "";
        
        UiManager.Instance.skip1Frame = true;
    }

    public void ChattingMode()
    {
        inputField.Select();
    }
}
