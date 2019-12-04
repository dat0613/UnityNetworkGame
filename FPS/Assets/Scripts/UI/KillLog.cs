using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillLog : MonoBehaviour
{
    [SerializeField]
    Text killerText;

    [SerializeField]
    Text victimText;

    [SerializeField]
    Image arrowImage;

    [SerializeField]
    Image backgroundImage;

    public RectTransform rectTransform;

    [HideInInspector]
    public bool destroyMode = false;

    [HideInInspector]
    Vector3 targetLocalPosition = Vector3.zero;

    [HideInInspector]
    public float lerp;

    Vector3 sizeDelta;
    Vector3 localScale;
    Quaternion localRotation;

    float aliveTime = 0.0f;

    void Awake()
    {
        StartCoroutine("MoveStart");

        sizeDelta = rectTransform.sizeDelta;
        localScale = rectTransform.localScale;
        localRotation = rectTransform.localRotation;
    }

    public void SetOption(string killerName, PlayerMove.Team killerTeam, string victimName, PlayerMove.Team victimTeam, bool headShot)
    {
        killerText.text = killerName;
        killerText.color = GetColor(killerTeam);

        victimText.text = victimName;
        victimText.color = GetColor(victimTeam);

        if(headShot)// 헤드샷이면 화살표를 빨간색으로 함
            arrowImage.color = Color.red;
    }

    Color GetColor(PlayerMove.Team team)// 팀에 알맞은 색을 리턴함
    {
        Color retval = Color.black;

        switch(team)
        {
            case PlayerMove.Team.Blue:
            retval = new Color(84.0f / 255.0f, 119.0f / 255.0f, 151.0f / 255.0f, 1.0f);
            break;

            case PlayerMove.Team.Red:
            retval = new Color(223.0f / 255.0f, 100.0f / 255.0f, 100.0f / 255.0f, 1.0f);
            break;
        }

        return retval;
    }

    // public void MoveStart(Vector3 targetLocalPosition)
    // {
    //     this.targetLocalPosition = targetLocalPosition;
    // }

    public void AddTargetPosition(Vector3 add)
    {
        targetLocalPosition += add;
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetLocalPosition = position;
    }

    public void SetLocalScale()// 부모를 다시 지정할때 크기가 어긋나는걸 원래대로 고침
    {
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.localScale = localScale;
        rectTransform.localRotation = localRotation;
    }

    IEnumerator MoveStart()
    {
        while(true)
        {
            float deltaTime = Time.deltaTime;

            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, targetLocalPosition, deltaTime * lerp);
            aliveTime += deltaTime;

            if(destroyMode && Vector3.Distance(rectTransform.localPosition, targetLocalPosition) < 1.0f)
            {
                break;
            }

            yield return 0;
        }
        Destroy(gameObject);
    }

    public bool IsDie(float maxAliveTime)
    {
        if(aliveTime > maxAliveTime)
            return true;
            
        return false;
    }

    public void SetVisible(bool visible)
    {
        killerText.enabled = 
        victimText.enabled = 
        arrowImage.enabled = 
        backgroundImage.enabled = 
        visible;
    }
}
