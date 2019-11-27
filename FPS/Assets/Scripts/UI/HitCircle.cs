using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class HitCircle : MonoBehaviour
{
    [SerializeField]
    Image image;

    public RectTransform rectTransform;

    Vector3 sizeDelta;
    Vector3 localScale;
    Quaternion localRotation;

    float aliveTime = 0.0f;

    Vector3 hitPosition;// 총알을 쏜 위치
    PlayerMove player;

    Coroutine coroutine = null;

    public void SetOption(Vector3 hitPosition)
    {
        this.hitPosition = hitPosition;
    }

    void Awake()
    {
        player = PlayerManager.GetMyPlayer();
        sizeDelta = rectTransform.sizeDelta;
        localScale = rectTransform.localScale;
        localRotation = rectTransform.localRotation;
        image.color = Color.red;
        image.enabled = false;

        StartCoroutine("SetColor");
    }

    public void SetLocalScale()// 부모를 다시 지정할때 크기가 어긋나는걸 원래대로 고침
    {
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.localScale = localScale;
        rectTransform.localRotation = localRotation;
        rectTransform.localPosition = Vector3.zero;
    }

    IEnumerator SetColor()
    {
        yield return new WaitForEndOfFrame();// 처음 생성하면 0도 위치에 있는게 보여서 첫 프레임동안은 모습을 숨김

        image.enabled = true;
    }

    IEnumerator StartDestroy()
    {
        float a = 1.0f;

        while(true)
        {
            a = Mathf.Lerp(a, 0.0f, 3.0f * Time.deltaTime);
            var color = new Color(1.0f, 0.0f, 0.0f, a);
            image.color = color;
            if(a < 0.1f)
                break;

            yield return 0;
        }

        Destroy(gameObject);
    }

    void Update()
    {
        aliveTime += Time.deltaTime;

        if(aliveTime > 1.0f && coroutine == null)
        {
            coroutine = StartCoroutine("StartDestroy");
        }

        if(player == null)
            return;

        var forwardVector = FollowCamera.Instance.transform.forward;// 카메라의 방향 벡터
        forwardVector.y = 0.0f;// 높이는 사용하지 않을 것임
        var hitVector = hitPosition - player.transform.position;// 플레이어 위치에서 총을 사격한 위치로 가는 방향벡터
        hitVector.y = 0.0f;

        var dot = Vector3.Dot(forwardVector, hitVector);// 두 벡터의 내적 = 두 벡터 크기의 곱 * Cos(벡터 사잇각)
        var magnitudeMultiple = forwardVector.magnitude * hitVector.magnitude;
        var radian = 0.0f;
        
        if(magnitudeMultiple > 0.01f)// 엄청난 우연의 일치로 같은 위치에서 서로 쏘고 맞을 수가 있더라
        {
            radian = Mathf.Acos(dot / magnitudeMultiple);// 그러므로  Cos(벡터 사잇각) = 두 벡터의 내적 / 두 벡터 크기의 곱
        }

        var deg = Mathf.Rad2Deg * radian + 90;                                         // 벡터 사잇각 = Acos(두 벡터의 내적 / 두 벡터 크기의 곱)

        var upVector = player.transform.up;

        var cross = Vector3.Cross(hitVector, forwardVector);// 두 벡터의 외적 = 해당 두 벡터를 하나의 면으로 봤을때 수직으로 올라가는 법선 벡터
                                                            // 그러므로 Cross(hitVector, forawardVector)는 hitVector가 forwardVector보다 왼쪽으로 갈때 y가 양수인 법선 벡터를 리턴함
        var crossValue = Vector3.Dot(upVector, cross);      // 여기서 내적을 하는 이유는 잘 모르겠음 아마 플레이어가 땅을 향한 상태로 있을때에도 공식이 성립하게 하기 위함 인거 같음

        if(crossValue < 0.0f)
        {// 외적값이 음수면 오브젝트가 오른쪽에 있는 것임
            var newDeg = deg - 90.0f;
            deg = 90 - newDeg;
        }

        rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, deg);
    }

    public void SetVisible(bool visible)
    {
        image.enabled = visible;
    }
}
