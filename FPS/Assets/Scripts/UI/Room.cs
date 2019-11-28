using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using MinNetforUnity;

public class Room : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int roomId = -1;// 음수면 아직 id가 없는 것임
    [SerializeField]
    Image image;

    Color originalColor;

    void Awake()
    {
        originalColor = image.color;
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MinNetUser.EnterRoom(roomId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color newColor = originalColor;
        newColor.a = 0.5f;
        image.color = newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = originalColor;
    }
}
