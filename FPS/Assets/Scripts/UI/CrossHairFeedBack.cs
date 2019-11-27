using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairFeedBack : MonoBehaviour
{
    public List<LineRenderer> lineRenderer;
    public float feedBackTime = 0.3f;

    float length = 0.0f;
    float targetLength = 0.0f;

    float cumulativeTime = 0.0f;

    bool longMode = false;
    public float lerp;

    public bool isHeadShotFeedback = false;

    public void HitFeedBack(int damage)
    {
        targetLength = damage * 0.8f + 10.0f;
        cumulativeTime = 0.0f;
    }

    void Update()
    {
        cumulativeTime += Time.deltaTime;

        if (cumulativeTime > feedBackTime * 0.5f)
        {// 길이가 짧아질 시간
            targetLength = 10;
        }

        length = Mathf.Lerp(length, targetLength, Time.deltaTime * lerp);

        float time = feedBackTime - cumulativeTime;

        if(time < 0.0f)
            time = 0.0f;

        float alpha = time / feedBackTime;

        for (int i = 0; i < lineRenderer.Count; i++)
        {
            Vector3 direction = Vector3.zero;
            if(i <= 1)
                direction.x = -1;
            else
                direction.x = 1;

            if(i == 0 || i == 2)
                direction.y = 1;
            else
                direction.y = -1;

            lineRenderer[i].SetPosition(1, direction * length);

            if(isHeadShotFeedback)
                lineRenderer[i].startColor = lineRenderer[i].endColor = new Color(1.0f, 0.0f, 0.0f, alpha);
            else
                lineRenderer[i].startColor = lineRenderer[i].endColor = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
    }
}