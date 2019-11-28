using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    static LoadingPanel instance = null;

    public static LoadingPanel Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    EasyLineRenderer easyLine;

    [SerializeField]
    Image backGroundImage;

    public float multiple = 1.0f;
    public float turnMultiple = 30.0f;

    public float minAngle = 95.0f;
    public float maxAngle = 300.0f;

    float centerDegree = 0.0f;
    float degree = 0.0f;

    Coroutine coroutine = null;

    bool stopOnNextFrame = false;// 비동기 함수에서 이 패널에 접근 해야 하는데 유니티가 혀용을 안해줌

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Stop();
    }
    

    public void LoadingStart(bool BackGroundVisible = false)
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine("DrawCircle");

        backGroundImage.enabled = BackGroundVisible;
    
        easyLine.SetVisible(true);

        stopOnNextFrame = false;
    }

    public void LoadingStop()
    {
        stopOnNextFrame = true;
    }

    private void Stop()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = null;

        backGroundImage.enabled = false;

        easyLine.SetVisible(false);
        stopOnNextFrame = false;
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

            if(stopOnNextFrame)
            {
                Stop();
                break;
            }

            yield return null;
        }
    }
}