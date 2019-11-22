using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameTimer : MonoBehaviour
{
    [SerializeField]
    Text timerText;

    float leftTime;// 남은 시간

    public void SetTimer(float second)
    {
        leftTime = second;
    }

    void Update()
    {
        string txt = "";
        
        if(leftTime > 0.0f)
        {
            leftTime -= Time.deltaTime;

            int minute = (int)(leftTime / 60);
            int second = (int)(leftTime % 60);

            txt = minute.ToString() + " : " + second.ToString();
        }
        else
        {
            leftTime = 0.0f;
            txt = "0 : 0";
        }
        timerText.text = txt;
    }
}