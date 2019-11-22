using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogText : MonoBehaviour
{
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
}
