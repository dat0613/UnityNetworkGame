using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;

public class KillText : MonoBehaviour
{
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    Text killText;
    [SerializeField]
    float dieTime = 0.8f;

    float aliveTime = 0.0f;
    float alpha = 0.0f;
    
    Vector3 sizeDelta;
    Vector3 localScale;
    Quaternion localRotation;
    string victimName = "";

    void Awake()
    {
        sizeDelta = rectTransform.sizeDelta;
        localScale = rectTransform.localScale;
        localRotation = rectTransform.localRotation;
    }

    public void SetOption(string victimName)
    {
        this.victimName = victimName;
        killText.text = "<color=#AF0311>" + victimName + "</color> 처치";
    }

    void Update()
    {
        aliveTime += Time.deltaTime;

        if(aliveTime > dieTime)
        {
            Destroy(gameObject);
        }

        var color = killText.color;

        aliveTime += Time.deltaTime;

        float time = dieTime - aliveTime;

        if(time < 0.0f)
            time = 0.0f;

        alpha = time / dieTime;

        alpha = alpha * 0.8f;

        if(aliveTime > dieTime)
        {
            alpha = 0.0f;
        }

        color.a = alpha;

        killText.color = color;

        killText.text =  "<color=#AF0311" + Convert.ToString((int)(alpha * 255), 16) + ">" + victimName + "</color> 처치";
        
        if(alpha < 0.1f)
            killText.enabled = false;
    }

    public void SetLocalScale()// 부모를 다시 지정할때 크기가 어긋나는걸 원래대로 고침
    {
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.localScale = localScale;
        rectTransform.localRotation = localRotation;

        rectTransform.localPosition = new Vector3(0.0f, -130.0f, 0.0f);
    }
}
