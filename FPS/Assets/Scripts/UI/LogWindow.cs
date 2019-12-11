using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class LogWindow : MonoBehaviour
{
    [SerializeField]
    InputField inputField;

    [SerializeField]
    Image inputFieldImage;

    [SerializeField]
    GameObject content;

    [SerializeField]
    LogText logTextPrefab;

    [SerializeField]
    ScrollRect scrollRect;

    [SerializeField]
    List<Image> visibleImages;

    bool visible = true;

    public Color defaultColor = Color.black;
    public int defaultSize = 14;

    IEnumerator ScrollDown()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 0.0f;
    }
    
    void Awake()
    {
        inputField.onEndEdit.AddListener(delegate { onEndEdit(); });
        scrollRect.onValueChanged.AddListener(delegate { StartCoroutine("ScrollDown"); });
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
        SoundManager.Instance.PlaySound("Chatting");
        var logText = Instantiate(logTextPrefab, content.transform);

        logText.transform.SetAsLastSibling();

        logText.SetSize(size);
        logText.SetColor(color);
        logText.SetText(txt);
        logText.SetWidth(380);

        logText.SetVisible(visible);
    }

    public void onEndEdit()
    {
        InputManager.Instance.ChangeFocus(InputManager.InputFocus.CharacterControl);

        var str = inputField.text;

        if(string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
        {// 빈 텍스트는 보내지 않음
        }
        else
        {
            PlayerManager.GetMyPlayer()?.SendChatting(str);
        }
        inputField.text = "";
        
        UIManager.Instance.skip1Frame = true;
    }

    public void ChattingMode()
    {
        inputField.Select();
    }

    public void SetVisible(bool visible)
    {
        this.visible = visible;

        foreach (var image in visibleImages)
        {
            image.enabled = visible;
        }

        int childCount = content.transform.childCount;

        for(int i = 0; i < childCount; i++)
        {
            content.transform.GetChild(i).GetComponent<LogText>().SetVisible(visible);
        }

        inputFieldImage.enabled = visible;
    }
}
