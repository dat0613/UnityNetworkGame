using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField]
    EasyLineRenderer easyLine;

    public float multiple = 1.0f;
    public float turnMultiple = 30.0f;

    public float minAngle = 95.0f;
    public float maxAngle = 300.0f;

    float centerDegree = 0.0f;
    float degree = 0.0f;

    Coroutine coroutine = null;

    public void Start()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine("DrawCircle");
    }

    public void Stop()
    {

    }

    IEnumerator DrawCircle()
    {
        centerDegree = 0.0f;
        degree = 0.0f;
        
        while(true)
        {
            centerDegree += Time.deltaTime * multiple;

            float radian = centerDegree * Mathf.Deg2Rad;

            float sin = Mathf.Abs(Mathf.Sin(radian));

            easyLine.CenterAngle = (maxAngle - minAngle) * sin + minAngle;

            degree += sin;

            int integerDegree = (int)easyLine.Angle;

            if (integerDegree != 0)
                easyLine.Angle = integerDegree % 360;

            easyLine.Angle += sin * turnMultiple * Time.deltaTime;

            yield return null;
        }
    }
}
