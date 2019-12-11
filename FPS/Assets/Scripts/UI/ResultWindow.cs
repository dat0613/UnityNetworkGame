using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;

public class ResultWindow : MonoBehaviour
{
    [SerializeField]
    Text resultText;

    float alpha = 0.0f;
    string colorCode = "";
    string text = "";

    public void SetOption(bool isWin)
    {
        if(isWin)
        {
            colorCode = "FDD755";
            text = "승리!";
            SoundManager.Instance.PlaySound("Win");
        }
        else
        {
            colorCode = "AF0311";
            text = "패배";
            SoundManager.Instance.PlaySound("Defeat");
        }

        Time.timeScale = 0.5f;
    }

    void Update()
    {
        alpha = Mathf.Lerp(alpha, 1.0f, Time.deltaTime * 5.0f);

        string alphaCode = Convert.ToString((int)(alpha * 255), 16);

        resultText.text = "<color=#" + colorCode + alphaCode + ">" + text + "</color>";
    }

    void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }
}
