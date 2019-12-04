using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ResultWindow : MonoBehaviour
{
    [SerializeField]
    Text resultText;

    public void SetOption(bool isWin)
    {
        if(isWin)
            resultText.text = "<color=#FDD755>승리!</color>";
        else
            resultText.text = "<color=#AF0311>패배</color>";
    }
}
