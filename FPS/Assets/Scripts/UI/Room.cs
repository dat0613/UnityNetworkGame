using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

using MinNetforUnity;

public class Room : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField]
    RectTransform rectTransform;

    [SerializeField]
    Image image;

    [SerializeField]
    Text roomNameText;

    [SerializeField]
    Text roomStateText;

    [SerializeField]
    Text userCountText;

    int maxUser = 0;
    int nowUser = 0;

    public int roomId = -1;// 음수면 아직 id가 없는 것임

    [Range(0.0f, 1.0f)]
    public float onPointerAlpha = 0.5f;

    Color imageOriginalColor;
    Color textOriginalColor;

    Vector3 sizeDelta;
    Vector3 localScale;
    Quaternion localRotation;

    void Awake()
    {
        imageOriginalColor = image.color;
        textOriginalColor = userCountText.color;

        sizeDelta = rectTransform.sizeDelta;
        localScale = rectTransform.localScale;
        localRotation = rectTransform.localRotation;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MinNetUser.EnterRoom(roomId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color newColor = imageOriginalColor;
        newColor.a = onPointerAlpha;
        image.color = newColor;

        newColor = textOriginalColor;
        newColor.a = onPointerAlpha;
        userCountText.color = newColor;
        roomNameText.color = newColor;
        roomStateText.color = newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = imageOriginalColor;
        userCountText.color = textOriginalColor;
        roomNameText.color = textOriginalColor;
        roomStateText.color = textOriginalColor;
    }

    public void SetOption(string roomName, string roomState, int roomId, int nowUser, int maxUser)
    {
        this.roomNameText.text = roomName;
        this.roomStateText.text = roomState;
        this.roomId = roomId;

        this.nowUser = nowUser;
        this.maxUser = maxUser;

        userCountText.text = nowUser.ToString() + " / " + maxUser.ToString();
    }

    public void SetLocalScale()// 부모를 다시 지정할때 크기가 어긋나는걸 원래대로 고침
    {
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.localScale = localScale;
        rectTransform.localRotation = localRotation;
        rectTransform.localPosition = Vector3.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        LoadingPanel.Instance.LoadingStart(true);

        MinNetUser.EnterRoom(roomId);
    }
}
