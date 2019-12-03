using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogText : MonoBehaviour
{
    [SerializeField]
    RectTransform rectTransform;
 
    [SerializeField]
    Text text;
    
    public Color textColor = Color.black;
    public int fontSize = 14;
    public bool bold = false;

    public void SetBold(bool bold)
    {
        this.bold = bold;
        // text.
    }

    public void SetText(string txt)
    {
        text.text = txt;
    }

    public void SetColor(Color color)
    {
        textColor = color;
        text.color = color;
    }

    public void SetSize(int size)
    {
        fontSize = size;
        text.fontSize = size;
    }

    public void SetVisible(bool visible)
    {
        text.enabled = visible;
    }

    public void SetWidth(float width)
    {
        var size = rectTransform.sizeDelta;
        size.x = width; 
        rectTransform.sizeDelta = size;
    }
}
