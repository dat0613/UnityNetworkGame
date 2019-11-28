using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Notification : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Text contentText;

    [SerializeField]
    RectTransform rectTransform;

    Vector3 position;
    Vector3 sizeDelta;
    Vector3 localScale;
    Quaternion localRotation;

    void Awake()
    {
        position = rectTransform.localPosition;
        sizeDelta = rectTransform.sizeDelta;
        localScale = rectTransform.localScale;
        localRotation = rectTransform.localRotation;
    }

    public void SetLocalScale()// 부모를 다시 지정할때 크기가 어긋나는걸 원래대로 고침
    {
        rectTransform.localPosition = position;
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.localScale = localScale;
        rectTransform.localRotation = localRotation;
    }

    public void SetText(string text)
    {
        contentText.text = text;
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        DestroyThis();
    }
}
