using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedBack : MonoBehaviour
{
    public SpriteRenderer render;

    public float feedBackTime = 0.3f;
    float cumulativeTime = 0.0f;
    float alpha = 0.0f;

    public void FeedBack()
    {
        cumulativeTime = 0.0f;
    }

    void Update()
    {
        cumulativeTime += Time.deltaTime;

        float time = feedBackTime - cumulativeTime;

        if(time < 0.0f)
            time = 0.0f;

        alpha = time / feedBackTime;

        alpha = alpha * 0.8f;

        if(cumulativeTime > feedBackTime)
        {
            alpha = 0.0f;
        }

        render.color = new Color(1.0f, 0.0f, 0.0f, alpha);
    }
}
