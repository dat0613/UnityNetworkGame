using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class Panel : MonoBehaviour, IPointerClickHandler 
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // InputManager.Instance.ChangeFocus(InputManager.InputFocus.CharacterControl);
    }
}